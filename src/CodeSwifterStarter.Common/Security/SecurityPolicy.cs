using System.Collections.Generic;

namespace CodeSwifterStarter.Common.Security
{
    public class SecurityPolicy
    {
        
        public const string Administrator = "Administrator";
        public const string User = "User";

        public string Name { get; set; }
        public string Description;
        public List<SecurityRequirement> Permissions { get; set; }

        public SecurityPolicy(string name)
        {
            
        }

        public SecurityPolicy(string name, string description, List<SecurityRequirement> permissions)
        {
            Description = description;
            Name = name;
            Permissions = permissions;
        }
    }
}