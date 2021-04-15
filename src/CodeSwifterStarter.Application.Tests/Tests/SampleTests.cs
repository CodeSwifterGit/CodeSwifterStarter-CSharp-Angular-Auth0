using CodeSwifterStarter.Application.Interfaces;
using CodeSwifterStarter.Common.Models;
using CodeSwifterStarter.Domain;
using Microsoft.Extensions.Logging;

namespace CodeSwifterStarter.Application.Tests.Tests
{
    public class CodeFormattingTests : BaseTest
    {
        private readonly ILogger<CodeFormattingTests> _logger;
        private readonly ServerConfiguration _serverConfiguration;

        public CodeFormattingTests(ICodeSwifterStarterDbContext context,
            IAuthenticatedUserService authenticatedUserService,
            ServerConfiguration serverConfiguration, ILogger<CodeFormattingTests> logger) :
            base(context, authenticatedUserService)
        {
            _serverConfiguration = serverConfiguration;
            _logger = logger;
        }
    }
}