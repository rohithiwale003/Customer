using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Customer.Controllers
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        ClaimsPrincipal GetPrincipalFromExpiredToken(HttpContext httpContext);
        bool IsTokenValid(string token);
        bool IsExpiredTokenValid(string token);
        Task<string> GetCustomerId(HttpContext httpContext);
        string GetCustomerId(IEnumerable<Claim> claims);
        Task<string> GetUserId(HttpContext httpContext);
        string GetUserId(IEnumerable<Claim> claims);
        Task<string> GetUserEmail(HttpContext httpContext);

        string GetClaimValue(string access_token, string claimTypes);
        Task<string> GetAccessTokenAsync(HttpContext httpContext);
        Task<string> GetDatabaseName(HttpContext httpContext);
        string GetDatabaseName(string access_token);
        bool AuthorizeRole(HttpContext httpContext, string role);
    }
}
