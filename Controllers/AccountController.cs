using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;

[Route("api/v1/accounts")]
[ApiController]
 public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AccountController(TokenService tokenService)
        => _tokenService = tokenService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterViewModel model,
        [FromServices] BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        // Gera uma senha de 25 caracteres, incluindo especiais e lowercase e depois gera um hash desta senha.
        string password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email,
                    password
                }
            ));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(400, new ResultViewModel<string>("E-mail já cadastrado"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna do servidor"));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel model,
        [FromServices] BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

        // Verifica se a senha possui um hash compatível com o do banco.
        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

        try
        {
             string token = _tokenService.GenerateToken(user);
             return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna do servidor"));

        }
    }
}
