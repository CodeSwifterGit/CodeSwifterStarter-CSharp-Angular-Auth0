using System.Collections.Generic;
using System.Linq;

namespace CodeSwifterStarter.Common.Security
{
    public static class SecurityPoliciesFactory
    {
        private static List<SecurityPolicy> _policies;

        public static List<SecurityPolicy> Policies => 
        _policies ??= new List<SecurityPolicy>{
                new SecurityPolicy(SecurityPolicy.Administrator, "Administrators", new List<SecurityRequirement>
                {
                    SecurityRequirementsFactory.Permissions.FirstOrDefault(p => p.Name == SecurityRequirement.ReadUsers),
                    SecurityRequirementsFactory.Permissions.FirstOrDefault(p => p.Name == SecurityRequirement.WriteUsers),
                    SecurityRequirementsFactory.Permissions.FirstOrDefault(p => p.Name == SecurityRequirement.ReadNonSensitiveData),
                    SecurityRequirementsFactory.Permissions.FirstOrDefault(p => p.Name == SecurityRequirement.WriteNonSensitiveData)
                }),
                new SecurityPolicy(SecurityPolicy.User, "Users", new List<SecurityRequirement>
                {
                    SecurityRequirementsFactory.Permissions.FirstOrDefault(p => p.Name == SecurityRequirement.ReadNonSensitiveData),
                    SecurityRequirementsFactory.Permissions.FirstOrDefault(p => p.Name == SecurityRequirement.WriteNonSensitiveData)
                })
            };
    }
}