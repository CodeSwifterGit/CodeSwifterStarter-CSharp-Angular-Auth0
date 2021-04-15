using CodeSwifterStarter.Application.Interfaces;

namespace CodeSwifterStarter.Application.Tests.Fakes.Services
{
    public class FakeEnvironmentInformationProvider : IEnvironmentInformationProvider
    {
        public FakeEnvironmentInformationProvider()
        {
            EnvironmentName = "Development";
            ApplicationName = "CodeSwifterStarter.Application.Tests";
        }

        public string EnvironmentName { get; }
        public string ApplicationName { get; }
        public string ContentRootPath { get; }
        public string WebRootPath { get; }
    }
}