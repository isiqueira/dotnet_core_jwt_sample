using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace JWT_Test.Models
{
    public class SigningConfigurations
    {
        public SecurityKey key { get; }
        public SigningCredentials signingCredentials { get; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                this.key = new RsaSecurityKey(provider.ExportParameters(true));
            }
            this.signingCredentials = new SigningCredentials(this.key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
