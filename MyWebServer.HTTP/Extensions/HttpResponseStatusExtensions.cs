using MyWebServer.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebServer.HTTP.Extensions
{
    public static class HttpResponseStatusExtensions
    {
        public static string GetResponseLine(this HttpResponseStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpResponseStatusCode.Ok:
                    return "200 OK";
                case HttpResponseStatusCode.Created:
                    return "201 Created";
                case HttpResponseStatusCode.Found:
                    return "302 Found";   
                case HttpResponseStatusCode.SeeOther:
                    return "303 See Other";
                case HttpResponseStatusCode.BadRequest:
                    return "400 Bad Request"; 
                case HttpResponseStatusCode.Unauthorized:
                    return "401 Unauthorized";
                case HttpResponseStatusCode.Forbidden:
                    return "403 Forbidden";
                case HttpResponseStatusCode.NotFound:
                    return "404 Not Found";
                case HttpResponseStatusCode.InternalServerError:
                    return "500 Internal Server Error";
                default:
                    return "No case for this status code!";

            }
        }
    }
}
