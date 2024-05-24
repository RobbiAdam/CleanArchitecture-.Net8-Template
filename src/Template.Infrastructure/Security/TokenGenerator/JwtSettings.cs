namespace Template.Infrastructure.Security.TokenGenerator
{
    public class JwtSettings
    {
        public const string Section = "JwtSettings";

        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int TokenExpirationInMinutes { get; set; }
    }
}
