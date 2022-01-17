using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/posts")]
public class PostController : ControllerBase
{
    private BlogDataContext _context;

    public PostController()
        => _context = new BlogDataContext();

    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery] int page = 0, [FromQuery] int pageSize = 25)
    {
        try
        {
            var count = await _context.Posts.CountAsync();
            var posts = await _context
                .Posts
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    LastUpdateDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author = $"{x.Author.Name} ({x.Author.Email})"
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = count,
                page,
                pageSize,
                posts
            }));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna no servidor"));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPost([FromRoute] int id)
    {
        try
        {
            var post = await _context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .ThenInclude(x => x.Roles)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (post == null)
                return NotFound(new ResultViewModel<Post>("Conteúdo não encontrado."));

            return Ok(new ResultViewModel<Post>(post));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor."));
        }
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(
        [FromRoute] string category,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 25)
    {
        try
        {
            var count = await _context.Posts.AsNoTracking().CountAsync();
            var posts = await _context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Where(x => x.Category.Slug == category)
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    LastUpdateDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author = $"{x.Author.Name} ({x.Author.Email})"
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = count,
                page,
                pageSize,
                posts
            }));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna no servidor"));
        }
    }
}
