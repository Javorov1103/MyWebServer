namespace MyWebServer.HTTP.Requests.Contracts
{
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Headers.Contracts;
    using System.Collections.Generic;


    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string, object> FormData { get; }

        Dictionary<string, object> QueryData { get; }

        IHttpHeadersCollection Headers { get; }

        HttpRequestMethod RequestMethod { get; }
    }
}
