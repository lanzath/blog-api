using System.Security.Claims;
using Blog.Models;

namespace Blog.Extensions;

public static class RoleClaimsExtension
{
    public static IEnumerable<Claim> GetClaims(this User user) // Adiciona o método como extensão de User
    {
        var result = new List<Claim>
        {
            new (ClaimTypes.Name, user.Email) // User.Identity.Name
        };

        result.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug)));

        return result;
    }
}
