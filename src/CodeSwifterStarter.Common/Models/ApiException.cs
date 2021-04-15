using System;
using System.Net;

namespace CodeSwifterStarter.Common.Models
{
    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode status, string message) : base(message)
        {
            Status = status;
        }

        public ApiException()
        {
        }

        public HttpStatusCode Status { get; }
    }
}