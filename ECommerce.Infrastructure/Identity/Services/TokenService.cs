using ECommerce.Application.Contracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Identity.Services
{
    internal class TokenService : ITokenService
    {
        public string CreateToken(string userId, string email, string userName, IReadOnlyList<string> roles)
        {

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            claims.Add(new Claim(ClaimTypes.Email, email));
            claims.Add(new Claim(ClaimTypes.UserData, userName));

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = "MYSECUirtyKeyForAuthENTICATIONMYSECUirtyKeyForAuthENTICATIONMYSECUirtyKeyForAuthENTICATION";
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7220",
                audience: "MyApp",
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256Signature));
       


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
