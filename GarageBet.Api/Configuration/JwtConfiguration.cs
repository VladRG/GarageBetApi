
namespace GarageBet.Api.Configuration
{
    public class JwtConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int Validity { get; set; }
    }
}
