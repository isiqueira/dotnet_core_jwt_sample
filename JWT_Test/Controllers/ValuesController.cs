using JWT_Test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace JWT_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        public string get() {
            return "OK!";
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public object login([FromForm]User user,
                            [FromServices]SigningConfigurations signingConfigurations,
                            [FromServices]TokenConfigurations tokenConfigurations)
        {


            if (user.auth())
            {
                var identity = new ClaimsIdentity
                (
                    new GenericIdentity(user.username, "Login"),
                    new[] { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")), new Claim(JwtRegisteredClaimNames.UniqueName, user.username) }
                );

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.issuer,
                    Audience = tokenConfigurations.audience,
                    SigningCredentials = signingConfigurations.signingCredentials,
                    Subject = identity,
                    NotBefore = DateTime.Now,
                    Expires = DateTime.Now + TimeSpan.FromSeconds(tokenConfigurations.seconds)
                });

                var token = handler.WriteToken(securityToken);

                return new
                {
                    authenticated = true,
                    created = (DateTime.Now + TimeSpan.FromSeconds(tokenConfigurations.seconds)).ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };

            } else
            {
                return new
                {
                    authenticated = false,
                    created = "",
                    expiration = "",
                    accessToken = "",
                    message = "Authentication Fail."
                };

            }


        }

        [HttpGet("{id}")]
        [Authorize("Bearer")]
        public string loggedEndpoint(string id, [FromHeader]string Authorization)
        {


            return $"Logged!!!{id} - {Authorization}";
        }
    }
}
