using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Application.Errors
{
    public class RestException(HttpStatusCode code, string message) : Exception
    {
        public HttpStatusCode Code { get; } = code;

        public override string Message { get; } = message;
    }
}
