namespace CodeSwifterStarter.Common.Models
{
    public class SingleSignOn
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public AuthApi Api { get; set; }
    }
}
