using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PizzalandCore.Interfaces;
namespace PizzalandCore.Utils;

public class JwtGenerator : IJwtGenerator
{
    public string GetJwt(string email, string name)
    {
        var handler = new JwtSecurityTokenHandler();

        Claim[] claims = [
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.GivenName, name),];

        string key = "SuperSecretKeyDontUseLikeThisInRealEnvironment";
        var signingCred = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: "PizzalandCore",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: signingCred
        );

        return handler.WriteToken(securityToken);
    }
}
