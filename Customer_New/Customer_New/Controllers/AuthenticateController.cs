using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Customer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        [HttpGet]
        public IActionResult Login(string username, string password)
        {
            if (UserServices.checklogin(username,password))
            {
                var usersClaims = new List<Claim>();
                usersClaims.Add(new Claim("username", username));
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aqwertfjibcxdgklobvgdhvkhohififkglgofkgbohbogklgog"));

                var jwtToken = new JwtSecurityToken(issuer: "northwind",
                    audience: "claims",
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                
                var access_token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                return new ObjectResult(new
                {
                    message = "Login Successful",
                    payload = new
                    {
                        access_token = access_token,
                    }
                });
            }
            else return BadRequest();
        }
    }
}