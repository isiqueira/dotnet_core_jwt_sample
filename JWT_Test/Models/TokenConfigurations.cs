using System;

namespace JWT_Test.Models
{

    public class TokenConfigurations
    {
        public string audience { get; set; }
        public string issuer { get; set; }
        //public int seconds { get; set; }

        public DateTime notBefore { get; set; } = DateTime.UtcNow;
        public DateTime issuedAt { get; set; } = DateTime.UtcNow;
        public long validFor { get; set; } //Valid Time in seconds
        public DateTime expiration => this.issuedAt.AddSeconds(this.validFor);
    }
}
