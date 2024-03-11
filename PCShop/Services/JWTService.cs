using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PCShop.Abstraction;
using PCShop.Entities;
using PCShop.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PCShop.Services
{
    public class JWTService : IJWTService
    {
        private readonly SymmetricSecurityKey jwtKey;
        private readonly int expiresDays;
        private readonly string issuer;

        public JWTService(IOptions<JWTServiceOption> options)
        {
            jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.JwtKey!));
            expiresDays = options.Value.ExpiresDays;
            issuer = options.Value.Issuer;
        }

        public int ExpiresDays
            => expiresDays;

        public string CreateJWT(User user, string role)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.GivenName, user.FirstName!),
                new Claim(ClaimTypes.Surname, user.LastName!),
                new Claim(ClaimTypes.NameIdentifier, user.Username!),
            };

            var credentials = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256);
            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(expiresDays),
                SigningCredentials = credentials,
                Issuer = issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDecriptor);

            return tokenHandler.WriteToken(jwt);
        }
    }
}
