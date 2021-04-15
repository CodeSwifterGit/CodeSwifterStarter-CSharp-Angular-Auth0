using System;
using Microsoft.AspNetCore.Authorization;

namespace CodeSwifterStarter.Application.Security
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission, string issuer)
        {
            Permission = permission ?? throw new ArgumentNullException(nameof(permission));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }

        public string Issuer { get; }
        public string Permission { get; }
    }
}