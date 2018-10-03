using MyWebServer.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebServer.HTTP.Exceptions
{
    public class BadRequestException : Exception
    {
        private const string errorMessage = "The Request was malformed or contains unsupported elements.";

        public const HttpResponseStatusCode StatusCode = HttpResponseStatusCode.BadRequest;

        public override string Message => "Error";

        public BadRequestException()
            : base(errorMessage)
        {

        }
    }
}
