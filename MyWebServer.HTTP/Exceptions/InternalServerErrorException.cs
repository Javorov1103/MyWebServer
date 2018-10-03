using MyWebServer.HTTP.Enums;
using System;

namespace MyWebServer.HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        private const string errorMessage = "The Server has encountered an error.";

        public const HttpResponseStatusCode StatusCode = HttpResponseStatusCode.InternalServerError;

        public override string Message => "Error";

        public InternalServerErrorException()
            : base(errorMessage)
        {

        }
    }
}
