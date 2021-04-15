using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CodeSwifterStarter.Common.Security;
using CodeSwifterStarter.Infrastructure.Models;
using CodeSwifterStarter.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSwifterStarter.Web.Api.Controllers.Auth
{
    public class UserRolesController : BaseAuthController
    {
        private readonly IAuthManagementService _authManagementService;

        public UserRolesController(IAuthManagementService authManagementService)
        {
            _authManagementService = authManagementService;
        }

        [Authorize(Policy = SecurityPolicy.Administrator)]
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(List<string>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<string>>> Get(string userId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roles = await _authManagementService.GetUserRoles(userId, cancellationToken);
            return roles.Select(r => r.Name).ToList();
        }

        [Authorize(Policy = SecurityPolicy.Administrator)]
        [HttpGet("{userId}/{role}")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<string>> Get(string userId, string role, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userRole = await _authManagementService.GetUserRole(userId, role, cancellationToken);
            return userRole?.Name;
        }

        [Authorize(Policy = SecurityPolicy.Administrator)]
        [HttpPost("{userId}")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.NoContent)]
        public async Task<ActionResult<string>> Post(string userId, CreateRole model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authManagementService.AddUserRoles(userId, new List<string> { model.Name }, cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = SecurityPolicy.Administrator)]
        [HttpDelete("{userId}/{role}")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.NoContent)]
        public async Task<ActionResult<string>> Delete(string userId, string role, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authManagementService.RemoveUserRole(userId, role, cancellationToken);

            return NoContent();
        }
        
    }
}