using JWT_Test.Models;
using JWT_Test.TokenHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly TokenHandler _tokenHandler;

        public ValuesController(TokenHandler tokenHandler)
        {
            this._tokenHandler = tokenHandler;
        }

        public string get() {
            return "OK!";
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public object login([FromForm]User user)
        {
            return user.auth() ? (object)this._tokenHandler.generateNewToken() : new
            {
                authenticated = false,
                created = "",
                expiration = "",
                accessToken = "",
                message = "Authentication Fail."
            };
        }

        [HttpGet("{id}")]
        [Authorize("Bearer")]
        public object loggedEndpoint(string id, [FromHeader]string Authorization)
        {
            var tokenData = this._tokenHandler.decodeToken(Authorization.Split(' ')[1].ToString());

            return new
            {
                id = id,
                authenticated = true,
                tokenData = tokenData
            };
        }

    }
}
