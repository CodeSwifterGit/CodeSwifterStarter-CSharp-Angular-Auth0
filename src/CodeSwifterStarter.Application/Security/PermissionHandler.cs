using System.Threading.Tasks;
using CodeSwifterStarter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CodeSwifterStarter.Application.Security
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public PermissionHandler(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context != null && requirement != null)
                if (_authenticatedUserService.HasPermission(requirement.Permission))
                    context?.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}