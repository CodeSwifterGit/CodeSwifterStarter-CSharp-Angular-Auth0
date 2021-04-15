using System;
using Serilog;
using Serilog.Formatting.Compact;

namespace CodeSwifterStarter.Common.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        /* IMPORTANT INFORMATION 
            Check "Serilog logging.txt" information in docs folder in the root of this repository, for how to run a docker instance of SEQ locally.
        */
        private const string AspNetCoreEnvironmentName = "DOTNET_ENVIRONMENT";
        private const string Local = null;
        private const string Development = null;
        private const string Staging = null;
        private const string Production = null;
        private const string DockerSeqInstance = "http://localhost:5341";
        private const string LiveSeqInstance = "http://seq.codeswifterstarter.com:5341";

        public static LoggerConfiguration WriteForEnvironment(this LoggerConfiguration configuration, string seqKey = null)
        {
            switch (Environment.GetEnvironmentVariable(AspNetCoreEnvironmentName))
            {
                case nameof(Local):
                case nameof(Development):
                    // Run docker command to make this working
                    // docker run --rm -it -e ACCEPT_EULA=Y -p 5341:80 datalust/seq
                    configuration = configuration
                        .WriteTo.Console(new RenderedCompactJsonFormatter())
                        .WriteTo.Seq(DockerSeqInstance);
                    break;
                case nameof(Staging):
                    configuration = configuration
                        .WriteTo.Seq(serverUrl:LiveSeqInstance, apiKey: seqKey);
                    break;
                case nameof(Production):
                    configuration = configuration
                        .WriteTo.Seq(LiveSeqInstance, apiKey: seqKey);
                    break;
            }

            return configuration;
        }
    }
}