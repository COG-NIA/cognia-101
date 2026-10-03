using Cognia.API.Data;
using Cognia.API.Models;
using Cognia.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cognia.API.Controllers;

/// <summary>Admin management of user accounts. Registration and login live in the auth controller.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize(Roles = "Admin")]
public class UsersController(
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : ControllerBase
{
    /// <summary>List users (paged).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<UserResponse>>> GetAll(int page = 1, int pageSize = 20)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = db.Users.AsNoTracking().OrderBy(u => u.Email);
        var total = await query.CountAsync();
        var users = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var ids = users.Select(u => u.Id).ToList();
        var roleRows = await (from ur in db.UserRoles
                              join r in db.Roles on ur.RoleId equals r.Id
                              where ids.Contains(ur.UserId)
                              select new { ur.UserId, r.Name }).ToListAsync();
        var rolesByUser = roleRows.ToLookup(x => x.UserId, x => x.Name!);

        var items = users
            .Select(u => ToResponse(u, rolesByUser[u.Id].ToList()))
            .ToList();

        return Ok(new PagedResult<UserResponse>(items, page, pageSize, total));
    }

    /// <summary>Get a user by id.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetById(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        return Ok(ToResponse(user, (await userManager.GetRolesAsync(user)).ToList()));
    }

    /// <summary>Create a user.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> Create(CreateUserRequest req)
    {
        if (!string.IsNullOrWhiteSpace(req.Role) && !await roleManager.RoleExistsAsync(req.Role))
        {
            ModelState.AddModelError(nameof(req.Role), $"Role '{req.Role}' does not exist.");
            return ValidationProblem(ModelState);
        }

        var user = new ApplicationUser
        {
            UserName = req.Email,
            Email = req.Email,
            DisplayName = req.DisplayName
        };

        var result = await userManager.CreateAsync(user, req.Password);
        if (!result.Succeeded) return IdentityErrors(result);

        var roles = new List<string>();
        if (!string.IsNullOrWhiteSpace(req.Role))
        {
            await userManager.AddToRoleAsync(user, req.Role);
            roles.Add(req.Role);
        }

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, ToResponse(user, roles));
    }

    /// <summary>Update a user's email and display name.</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> Update(string id, UpdateUserRequest req)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        user.DisplayName = req.DisplayName;
        if (!string.Equals(user.Email, req.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailResult = await userManager.SetEmailAsync(user, req.Email);
            if (!emailResult.Succeeded) return IdentityErrors(emailResult);

            // Login uses the email as the username.
            var nameResult = await userManager.SetUserNameAsync(user, req.Email);
            if (!nameResult.Succeeded) return IdentityErrors(nameResult);
        }

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded) return IdentityErrors(result);

        return Ok(ToResponse(user, (await userManager.GetRolesAsync(user)).ToList()));
    }

    /// <summary>Delete a user and their mood entries.</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        if (id == User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            ModelState.AddModelError(nameof(id), "You cannot delete your own account.");
            return ValidationProblem(ModelState);
        }

        var user = await userManager.FindByIdAsync(id);
        if (user is null) return NotFound();

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded ? NoContent() : IdentityErrors(result);
    }

    private static UserResponse ToResponse(ApplicationUser u, IReadOnlyList<string> roles) =>
        new(u.Id, u.Email ?? string.Empty, u.DisplayName, roles, u.CreatedAt);

    private ActionResult IdentityErrors(IdentityResult result)
    {
        foreach (var e in result.Errors) ModelState.AddModelError(e.Code, e.Description);
        return ValidationProblem(ModelState);
    }
}
