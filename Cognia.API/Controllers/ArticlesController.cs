using Cognia.API.Data;
using Cognia.API.Models;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cognia.API.Controllers;

/// <summary>Self-help articles. Reading is public; writing requires the Admin role.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ArticlesController(ApplicationDbContext db) : ControllerBase
{
    private bool IsAdmin => User.IsInRole("Admin");

    private static ArticleResponse ToResponse(Article a) =>
        new(a.Id, a.Title, a.Content, a.Category, a.Author, a.IsPublished, a.CreatedAt, a.UpdatedAt);

    /// <summary>List articles. Anonymous users only ever see published articles.</summary>
    /// <param name="published">Admins only: filter by published state.</param>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<ArticleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ArticleResponse>>> GetAll(
        string? category, string? search, bool? published, int page = 1, int pageSize = 20)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = db.Articles.AsNoTracking().AsQueryable();

        if (!IsAdmin) query = query.Where(a => a.IsPublished);
        else if (published.HasValue) query = query.Where(a => a.IsPublished == published);

        if (!string.IsNullOrWhiteSpace(category)) query = query.Where(a => a.Category == category);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(a => a.Title.Contains(search) || a.Content.Contains(search));

        var total = await query.CountAsync();
        var items = await query.OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(a => new ArticleResponse(a.Id, a.Title, a.Content, a.Category, a.Author,
                a.IsPublished, a.CreatedAt, a.UpdatedAt))
            .ToListAsync();

        return Ok(new PagedResult<ArticleResponse>(items, page, pageSize, total));
    }

    /// <summary>List the categories that have published articles.</summary>
    [HttpGet("categories")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<string>>> GetCategories()
    {
        var categories = await db.Articles.AsNoTracking()
            .Where(a => a.IsPublished)
            .Select(a => a.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        return Ok(categories);
    }

    /// <summary>Get an article. Unpublished articles are only visible to admins.</summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticleResponse>> GetById(int id)
    {
        var article = await db.Articles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (article is null || (!article.IsPublished && !IsAdmin)) return NotFound();

        return Ok(ToResponse(article));
    }

    /// <summary>Create an article.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ArticleResponse>> Create(ArticleRequest req)
    {
        var article = new Article
        {
            Title = req.Title,
            Content = req.Content,
            Category = req.Category,
            Author = req.Author,
            IsPublished = req.IsPublished
        };
        db.Articles.Add(article);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = article.Id }, ToResponse(article));
    }

    /// <summary>Update an article.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticleResponse>> Update(int id, ArticleRequest req)
    {
        var article = await db.Articles.FindAsync(id);
        if (article is null) return NotFound();

        article.Title = req.Title;
        article.Content = req.Content;
        article.Category = req.Category;
        article.Author = req.Author;
        article.IsPublished = req.IsPublished;
        article.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        return Ok(ToResponse(article));
    }

    /// <summary>Delete an article.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await db.Articles.FindAsync(id);
        if (article is null) return NotFound();

        db.Articles.Remove(article);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
