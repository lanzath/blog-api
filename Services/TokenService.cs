using Blog.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Blog.Services;
public class TokenService
{
    public string GenerateToken(User user)
    {
        // Instância responsável por gerar o token.
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

        // Instância que irá definir as especificações do token.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddHours(8), // Duração do token de 8 horas
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), // Método de credenciamento através de chave simetrica no servidor
                SecurityAlgorithms.HmacSha256Signature)
        };

        // Criação do token.
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
