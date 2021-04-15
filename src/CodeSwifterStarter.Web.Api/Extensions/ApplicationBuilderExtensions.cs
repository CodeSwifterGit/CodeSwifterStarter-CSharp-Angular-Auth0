using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CodeSwifterStarter.Web.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRootRewrite(this IApplicationBuilder builder)
        {
            var socks = "/sockjs-node";

            builder.Use((context, next) =>
            {
                if (!HttpMethods.IsGet(context.Request.Method) && !context.Request.Path.StartsWithSegments(socks))
                {
                    if (!context.Response.HasStarted)
                    {
                        context.Response.StatusCode = 404;
                        context.Response.CompleteAsync().GetAwaiter().GetResult();
                        return null; 

                    }
                }
                
                return next();
            });

            return builder;
        }
    }
}
