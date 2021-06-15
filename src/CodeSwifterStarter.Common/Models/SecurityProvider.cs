namespace CodeSwifterStarter.Common.Models
{
    public class SecurityProvider
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
        public SingleSignOn SingleSignOn { get; set; }
        public ManagementApi ManagementApi { get; set; }
    }
}