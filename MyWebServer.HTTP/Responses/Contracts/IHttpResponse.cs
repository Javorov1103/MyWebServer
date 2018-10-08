namespace MyWebServer.HTTP.Responses.Contracts
{
    using MyWebServer.HTTP.Cookies;
    using MyWebServer.HTTP.Cookies.Contracts;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Headers;
    using MyWebServer.HTTP.Headers.Contracts;

    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; }

        IHttpHeadersCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        void AddCookie(HttpCookie cookie);

        byte[] GetBytes();
    }
}
