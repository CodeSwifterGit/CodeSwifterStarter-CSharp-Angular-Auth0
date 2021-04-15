using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using CodeSwifterStarter.Application.Interfaces;
using CodeSwifterStarter.Common.Models;
using CodeSwifterStarter.Web.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CodeSwifterStarter.Web.Api.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IMediator _mediator;


        [Description("Use this constructor for virtual user (not signed in).")]
        public AuthenticatedUserService(IMediator mediator)
        {
            _mediator = mediator;
            IsAuthenticated = false;
        }


        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _mediator = mediator;
            IsAuthenticated = httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

            if (httpContextAccessor?.HttpContext?.User?.Claims != null && IsAuthenticated)
            {
                Id = httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == AuthenticatedUserConstants.ClaimTypeUserId)?.Value;
                Name = httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == AuthenticatedUserConstants.ClaimTypeName)?.Value;
                Email = httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == AuthenticatedUserConstants.ClaimTypeEmail)?.Value;

                if (httpContextAccessor.HttpContext.User.Claims.Any(x =>
                    x.Type == AuthenticatedUserConstants.ClaimTypeEmailVerified))
                    EmailVerified = bool.Parse(httpContextAccessor.HttpContext.User.Claims
                        .FirstOrDefault(x =>
                            x.Type == AuthenticatedUserConstants.ClaimTypeEmailVerified)
                        ?.Value ?? "false");

                Nickname = httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == AuthenticatedUserConstants.ClaimTypeNickName)?.Value;
                Picture = httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == AuthenticatedUserConstants.ClaimTypePicture)?.Value;

                var updatedAtString =
                    (httpContextAccessor.HttpContext.User.Claims
                        .FirstOrDefault(x => x.Type == AuthenticatedUserConstants.ClaimTypeUpdatedAt)
                        ?.Value ?? "").Replace("\"", "", StringComparison.InvariantCulture);

                var lastLogin = DateTime.Parse(updatedAtString, CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal);

                LastLogin = lastLogin.ToLocalTime();

                var createdAtString =
                    (httpContextAccessor.HttpContext.User.Claims
                        .FirstOrDefault(x => x.Type == AuthenticatedUserConstants.ClaimTypeCreatedAt)
                        ?.Value ?? "").Replace("\"", "", StringComparison.InvariantCulture);

                var createdAt = DateTime.Parse(createdAtString, CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal);

                CreatedAt = createdAt.ToLocalTime();

                Permissions = httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == AuthenticatedUserConstants.ClaimTypePermissions)?.Value.Split(' ')
                    .ToList();
            }
            else
            {
                Permissions = new List<string>();
            }
        }

        public string Id { get; }
        public string Name { get; }
        public string Email { get; }
        public bool EmailVerified { get; }
        public List<string> Permissions { get; }
        public string Nickname { get; }
        public string Picture { get; }
        public DateTime? LastLogin { get; }
        public DateTime? LastActivity { get; }
        public DateTime? CreatedAt { get; }
        public bool IsAuthenticated { get; }

        public string BundledUserInfo()
        {
            if (Id == null || Name == null)
                return null;
            return ObfuscatedUser.ToUserInfo(new ObfuscatedUser(Id, Name));
        }

        public bool HasPermission(string permission)
        {
            return Permissions.Contains(permission);
        }
    }
}