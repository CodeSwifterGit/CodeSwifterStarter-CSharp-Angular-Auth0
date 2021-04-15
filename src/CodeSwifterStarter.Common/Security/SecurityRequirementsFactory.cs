using System.Collections.Generic;

namespace CodeSwifterStarter.Common.Security
{
    public static class SecurityRequirementsFactory
    {
        private static List<SecurityRequirement> _permissions;

        public static List<SecurityRequirement> Permissions =>
            _permissions ??= new List<SecurityRequirement>
            {
                new SecurityRequirement(SecurityRequirement.ReadNonSensitiveData, "Read non sensitive data"),
                new SecurityRequirement(SecurityRequirement.WriteNonSensitiveData, "Write non sensitive data"),
                new SecurityRequirement(SecurityRequirement.ReadUsers, "Read users"),
                new SecurityRequirement(SecurityRequirement.WriteUsers, "Write users")
            };
    }
}