using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CodeSwifterStarter.Common.Models;
using RestSharp;
using CodeSwifterStarter.Common.Extensions;
using CodeSwifterStarter.Infrastructure.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;


// For more information how Auth0 management API works, visit https://auth0.com/docs/api/management/v2#!/
namespace CodeSwifterStarter.Infrastructure.Services
{
    public interface IAuthManagementService
    {
        Task<List<Role>> GetRoles(CancellationToken cancellationToken);
        Task<Role> GetRole(string name, CancellationToken cancellationToken);
        Task AddRole(string name, string description, CancellationToken cancellationToken);
        Task DeleteRole(string name, CancellationToken cancellationToken);
        Task<List<Role>> GetUserRoles(string userId, CancellationToken cancellationToken);
        Task<Role> GetUserRole(string userId, string role, CancellationToken cancellationToken);
        Task AddUserRoles(string userId, List<string> roles, CancellationToken cancellationToken);
        Task RemoveUserRole(string userId, string role, CancellationToken cancellationToken);
    }

    public class AuthManagementService : IAuthManagementService
    {
        private readonly ServerConfiguration _serverConfiguration;
        private readonly RestSharp.RestClient _tokenClient;
        private readonly RestSharp.RestClient _apiClient;
        private static AccessToken _accessToken;
        static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        public AuthManagementService(ServerConfiguration serverConfiguration)
        {
            _serverConfiguration = serverConfiguration;

            _tokenClient = new RestClient(serverConfiguration.SecurityProvider.Authority.EnsureEndsWith('/'));
            _apiClient = new RestClient(serverConfiguration.SecurityProvider.Audience.EnsureEndsWith('/'));
        }

        private async Task<AccessToken> GetAccessToken(CancellationToken cancellationToken)
        {
            await SemaphoreSlim.WaitAsync(cancellationToken);
            try
            {
                if (_accessToken != null && !_accessToken.IsExpired())
                {
                    return _accessToken;
                }

                var tokenRequest = new
                {
                    grant_type = "client_credentials",
                    client_id = _serverConfiguration.SecurityProvider.ManagementApi.ClientId,
                    client_secret = _serverConfiguration.SecurityProvider.ManagementApi.ClientSecret,
                    audience = _serverConfiguration.SecurityProvider.Audience
                };

                var request = new RestRequest("/oauth/token", Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter(
                    "application/x-www-form-urlencoded",
                    $"grant_type={tokenRequest.grant_type}&client_id={tokenRequest.client_id}&client_secret={tokenRequest.client_secret}&audience={tokenRequest.audience}", ParameterType.RequestBody);

                IRestResponse response = await _tokenClient.ExecuteAsync(request, cancellationToken);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new SecurityTokenException($"Cannot obtain access token from Auth0. {response.ErrorMessage}");
                }

                _accessToken = JsonConvert.DeserializeObject<AccessToken>(response.Content);
            }
            finally
            {
                SemaphoreSlim.Release();

            }

            return _accessToken;
        }

        private async Task<RestRequest> GetApiRequest(string path, RestSharp.Method method, CancellationToken cancellationToken)
        {
            var tokenResponse = await GetAccessToken(cancellationToken);

            var request = new RestRequest(path, method);
            request.AddHeader("Authorization", $"{tokenResponse.TokenType} {tokenResponse.Token}");

            return request;
        }

        public async Task<List<Role>> GetRoles(CancellationToken cancellationToken)
        {
            var request = await GetApiRequest("roles", Method.GET, cancellationToken);

            IRestResponse response = await _apiClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException($"Cannot get roles from Auth0. {response.ErrorMessage}");
            }

            return JsonConvert.DeserializeObject<List<Role>>(response.Content);
        }

        public async Task<Role> GetRole(string name, CancellationToken cancellationToken)
        {
            var roles = await GetRoles(cancellationToken);
            return roles.FirstOrDefault(r => r.Name == name);
        }

        public async Task AddRole(string name, string description, CancellationToken cancellationToken)
        {
            var request = await GetApiRequest("roles", Method.POST, cancellationToken);
            request.AddJsonBody(new
            {
                name,
                description
            });

            IRestResponse response = await _apiClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException($"Cannot create role ${name} on Auth0. {response.ErrorMessage}");
            }
        }

        public async Task DeleteRole(string name, CancellationToken cancellationToken)
        {
            var role = await GetRole(name, cancellationToken);
            if (role != null)
            {
                var request = await GetApiRequest($"roles/{role.Id}", Method.DELETE, cancellationToken);
                IRestResponse response = await _apiClient.ExecuteAsync(request, cancellationToken);

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    throw new InvalidOperationException($"Cannot create role ${name} on Auth0. {response.ErrorMessage}");
                }
            }
            else
            {
                throw new InvalidOperationException($"Role {name} doesn't exist on Auth0.");
            }
            
        }
        
        public async Task<List<Role>> GetUserRoles(string userId, CancellationToken cancellationToken)
        {
            var request = await GetApiRequest($"users/{userId}/roles", Method.GET, cancellationToken);

            IRestResponse response = await _apiClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(
                    $"Cannot get roles for user {userId} from Auth0. {response.ErrorMessage}");
            }

            return JsonConvert.DeserializeObject<List<Role>>(response.Content);

        }

        public async Task<Role> GetUserRole(string userId, string role, CancellationToken cancellationToken)
        {
            var roles = await GetUserRoles(userId, cancellationToken);

            return roles.FirstOrDefault(r => r.Name == role);

        }

        public async Task AddUserRoles(string userId, List<string> roles, CancellationToken cancellationToken)
        {
            var allRoles = await GetRoles(cancellationToken);
            var rolesToAdd = allRoles.Where(r => roles.Contains(r.Name));

            var request = await GetApiRequest($"users/{userId}/roles", Method.POST, cancellationToken);
            request.AddJsonBody(new {roles = rolesToAdd.Select(r => r.Id).ToArray()});

            IRestResponse response = await _apiClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new InvalidOperationException(
                    $"Error adding roles to user {userId} on Auth0. {response.ErrorMessage}");
            }
        }

        public async Task RemoveUserRole(string userId, string role, CancellationToken cancellationToken)
        {
            var allRoles = await GetRoles(cancellationToken);
            var rolesToRemove = allRoles.Where(r => r.Name == role);

            var request = await GetApiRequest($"users/{userId}/roles", Method.DELETE, cancellationToken);
            request.AddJsonBody(new {roles = rolesToRemove.Select(r => r.Id).ToArray()});

            IRestResponse response = await _apiClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new InvalidOperationException(
                    $"Cannot remove role {role} from user {userId} on Auth0. {response.ErrorMessage}");
            }
        }
    }
}
