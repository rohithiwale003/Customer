using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Customer.Controllers
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["serverSigningPassword"]));

            var jwtToken = new JwtSecurityToken(issuer: "grocare",
                audience: "grocare",
                claims: claims.Where(x => x.Type != "aud"),
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["accessTokenDurationInMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public string GenerateRefreshToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["serverSigningPassword"]));
            var randomNumber = new byte[32];
            string guid = "";
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                guid = Convert.ToBase64String(randomNumber);
            }
            var claims = new[]
            {
                    new Claim(ClaimTypes.Thumbprint, guid)
            };
            var jwtToken = new JwtSecurityToken(issuer: "grocare",
                audience: "grocare",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["serverSigningPassword"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(HttpContext httpContext)
        {
            string access_token = Task.Run(() => httpContext.GetTokenAsync("access_token")).Result;
            return GetPrincipalFromExpiredToken(access_token);
        }

        public bool IsTokenValid(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["serverSigningPassword"])),
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool IsExpiredTokenValid(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["serverSigningPassword"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

       
        public string GetClaimValue(string access_token, string claimTypes)
        {
            return GetPrincipalFromExpiredToken(access_token).Claims.Where(c => c.Type == claimTypes)
                    .Select(c => c.Value).SingleOrDefault();
        }

        public async Task<string> GetAccessTokenAsync(HttpContext httpContext)
        {
            return await httpContext.GetTokenAsync("access_token");
        }

        public async Task<string> GetDatabaseName(HttpContext httpContext)
        {
            string token = await httpContext.GetTokenAsync("access_token");
            return GetClaimValue(token, "domain");
        }

        public async Task<string> GetUserEmail(HttpContext httpContext)
        {
            string access_token = await httpContext.GetTokenAsync("access_token");
            string vc_id = GetPrincipalFromExpiredToken(access_token).Claims.Where(c => c.Type == "user_email")
                   .Select(c => c.Value).SingleOrDefault();
            return vc_id;
        }

        public async Task<string> GetCustomerId(HttpContext httpContext)
        {
            string access_token = await httpContext.GetTokenAsync("access_token");
            string vc_id = GetPrincipalFromExpiredToken(access_token).Claims.Where(c => c.Type == "customer_id")
                   .Select(c => c.Value).SingleOrDefault();
            return vc_id;
        }

        public string GetCustomerId(IEnumerable<Claim> claims)
        {
            string vc_id = claims.Where(c => c.Type == "customer_id")
                   .Select(c => c.Value).SingleOrDefault();
            return vc_id;
        }

        public string GetUserId(IEnumerable<Claim> claims)
        {
            string vc_id = claims.Where(c => c.Type == "user_id")
                    .Select(c => c.Value).SingleOrDefault();
            return vc_id;
        }

        public async Task<string> GetUserId(HttpContext httpContext)
        {
            string access_token = await httpContext.GetTokenAsync("access_token");
            string vc_id = GetPrincipalFromExpiredToken(access_token).Claims.Where(c => c.Type == "user_id")
                    .Select(c => c.Value).SingleOrDefault();
            return vc_id;
        }

        public string GetDatabaseName(string access_token)
        {
            return GetClaimValue(access_token, "domain");
        }

        public bool AuthorizeRole(HttpContext httpContext, string role)
        {
            return JsonConvert.DeserializeObject<List<string>>
                (GetClaimValue(httpContext.GetTokenAsync("access_token").Result, "user_roles"))
                .Contains(role);
        }

       
        ClaimsPrincipal ITokenService.GetPrincipalFromExpiredToken(string token)
        {
            throw new NotImplementedException();
        }

      
    }
}
