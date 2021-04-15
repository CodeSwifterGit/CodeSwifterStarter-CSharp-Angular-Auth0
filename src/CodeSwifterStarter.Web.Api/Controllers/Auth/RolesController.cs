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
    public class RolesController : BaseAuthController
    {
        private readonly IAuthManagementService _authManagementService;

        public RolesController(IAuthManagementService authManagementService)
        {
            _authManagementService = authManagementService;
        }

        [Authorize(Policy = SecurityPolicy.Administrator)]
        [HttpGet()]
        [ProducesResponseType(typeof(List<string>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<string>>> Get(CancellationToken cancellationToken)
        {
            var roles = await _authManagementService.GetRoles(cancellationToken);
            return roles.Select(r => r.Name).ToList();
        }

        [Authorize(Policy = SecurityPolicy.Administrator)]
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<string>> Get(string name, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _authManagementService.GetRole(name, cancellationToken);

            return role?.Name;
        }

        [Authorize(Policy = SecurityPolicy.Administrator)]
        [HttpPost()]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<string>> Post(CreateRole model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authManagementService.AddRole(model.Name, model.Description, cancellationToken);

            return Ok();
        }

        [Authorize(Policy = SecurityPolicy.Administrator)]
        [HttpDelete("{name}")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.NoContent)]
        public async Task<ActionResult<string>> Delete(string name, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authManagementService.DeleteRole(name, cancellationToken);

            return NoContent();
        }
        
    }
}