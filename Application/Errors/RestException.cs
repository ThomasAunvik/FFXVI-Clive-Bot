using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Application.Errors
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public HttpStatusCode Code { get; }

        public override string Message { get; }
    }
}
