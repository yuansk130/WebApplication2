using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Repository
{
    public class JwtHandlerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        public JwtHandlerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JWT");
        }
        public SigningCredentials GetSigningCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
        public List<Claim> GetClaims(IdentityUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName)
        };
            return claims;
        }
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
