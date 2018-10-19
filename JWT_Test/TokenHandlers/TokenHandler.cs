using JWT_Test.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace JWT_Test.TokenHandlers
{
    public class TokenHandler
    {

        private readonly SigningConfigurations signingConfigurations;
        private readonly TokenConfigurations tokenConfigurations;

        public TokenHandler(SigningConfigurations signingConfigurations,
                            TokenConfigurations tokenConfigurations)
        {
            this.signingConfigurations = signingConfigurations;
            this.tokenConfigurations = tokenConfigurations;
        }

        public Result generateNewToken() {

            var identity = this.getIdentity("");

            var claims = new ClaimsIdentity
            (
                identity,
                new[]
                {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, ""),
                        new Claim(JwtRegisteredClaimNames.NameId, ""),
                        new Claim(JwtRegisteredClaimNames.Email, ""),
                        new Claim(JwtRegisteredClaimNames.Sub, ""),
                        new Claim(JwtRegisteredClaimNames.Iat, toUnixEpochDate(tokenConfigurations.issuedAt).ToString(), ClaimValueTypes.Integer64),
                }
            );

            var jwt = new JwtSecurityToken(
                issuer: this.tokenConfigurations.issuer,
                audience: this.tokenConfigurations.audience,
                claims: claims.Claims,
                notBefore: this.tokenConfigurations.notBefore,
                expires: this.tokenConfigurations.expiration,
                signingCredentials: this.signingConfigurations.signingCredentials
            );

            return new Result
            {
                authenticated = true,
                created = this.tokenConfigurations.issuedAt,
                expiration = this.tokenConfigurations.expiration,
                message = "OK",
                accessToken = new JwtSecurityTokenHandler().WriteToken(jwt).ToString()
            };
        }

        public object decodeToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token); //handler.ReadJwtToken(token);
        }

        private GenericIdentity getIdentity(string username)
        {
            return new GenericIdentity(username, "Login");
        }

        private static long toUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
