namespace MyWebServer.HTTP.Requests.Contracts
{
    using MyWebServer.HTTP.Cookies.Contracts;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Headers.Contracts;
    using MyWebServer.HTTP.Sessions.Contracts;
    using System.Collections.Generic;


    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string, object> FormData { get; }

        Dictionary<string, object> QueryData { get; }

        IHttpHeadersCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        HttpRequestMethod RequestMethod { get; }

        IHttpSession Session { get; set; }
    }
}
