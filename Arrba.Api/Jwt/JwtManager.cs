using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Arrba.Api.Jwt
{
    public class JwtManager
    {
        private readonly IJwtSigningEncodingKey _signingEncodingKey;

        public JwtManager(IJwtSigningEncodingKey signingEncodingKey)
        {
            this._signingEncodingKey = signingEncodingKey;
        }

        public string GenerateToken(long userId, string userName, string userEmail, string role = null, int expireMinutes = 120)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, userEmail)
            };

            if (string.IsNullOrEmpty(role) is false)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: "ArrbaApplication",
                audience: "ArrbaApplicationClient",
                claims: claims.ToArray(),
                expires: DateTime.Now.AddMinutes(expireMinutes),
                signingCredentials: new SigningCredentials(
                    this._signingEncodingKey.GetKey(),
                    this._signingEncodingKey.SigningAlgorithm)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        public static bool RoleExsits(IIdentity userIdentity, string roleName)
        {
            List<string> GetRoles(IIdentity identity)
            {
                var roles = ((ClaimsIdentity)identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();

                return roles;
            }

            return GetRoles(userIdentity).Any(name => name == roleName);
        }
    }
}
