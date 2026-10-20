using Cognia.API.Data;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Cognia.API.Controllers;

[ApiController]
[Route("api/articles")]
public sealed class ArticlesController : ControllerBase
{
    private readonly CogniaMongoDatabase _database;

    public ArticlesController(CogniaMongoDatabase database) => _database = database;

    [HttpGet]
    public async Task<ActionResult<List<WellnessArticle>>> GetArticles(CancellationToken cancellationToken)
    {
        var articles = await _database.Articles.Find(FilterDefinition<WellnessArticle>.Empty)
            .SortByDescending(article => article.CreatedAt)
            .ToListAsync(cancellationToken);
        return Ok(articles);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WellnessArticle>> GetArticle(int id, CancellationToken cancellationToken)
    {
        var article = await _database.Articles.Find(item => item.Id == id).FirstOrDefaultAsync(cancellationToken);
        return article is null ? NotFound() : Ok(article);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<WellnessArticle>> CreateArticle(WellnessArticle article, CancellationToken cancellationToken)
    {
        article.Id = await _database.NextIdAsync("articles", cancellationToken);
        article.CreatedAt = DateTime.UtcNow;
        await _database.Articles.InsertOneAsync(article, cancellationToken: cancellationToken);
        return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
    }
}