using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Template.Application.Common.Interfaces.Authentication;

namespace Template.Infrastructure.Security.TokenGenerator
{
    public class JwtTokenGenerator(IOptions<JwtSettings> jwtOptions) : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings = jwtOptions.Value;
        
        public string GenerateToken(string userId, string userName, string email, string roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName),
                //new Claim(ClaimTypes.Role, roles)
            };

            //claims.Add(new Claim(ClaimTypes.Role, IsAdmin ? "admin" : "customer"));
            //roles.ForEach(role => claims.Add(new(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);           

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,                
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                signingCredentials: credentials
                ); 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
