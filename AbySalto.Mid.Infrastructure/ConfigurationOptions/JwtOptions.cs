namespace AbySalto.Mid.Infrastructure.ConfigurationOptions
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
        public string Secret { get; set; }
    }
}
