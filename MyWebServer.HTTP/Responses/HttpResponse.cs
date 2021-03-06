﻿namespace MyWebServer.HTTP.Responses
{
    using MyWebServer.HTTP.Common;
    using MyWebServer.HTTP.Cookies;
    using MyWebServer.HTTP.Cookies.Contracts;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Headers;
    using MyWebServer.HTTP.Headers.Contracts;
    using MyWebServer.HTTP.Responses.Contracts;
    using System.Linq;
    using System.Text;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
            this.Headers = new HttpHeadersCollection();
            this.Content = new byte[0];
            this.Cookies = new HttpCookieCollection();
        }

        public HttpResponse(
            HttpResponseStatusCode statusCode) : this()
        {
            CoreValidator.ThrowIfNull(statusCode, nameof(statusCode));
            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeadersCollection Headers { get; }

        public byte[] Content { get; set; }

        public IHttpCookieCollection Cookies { get; }

        public void AddCookie(HttpCookie cookie)
        {
            this.Cookies.Add(cookie);
        }

        public void AddHeader(HttpHeader header)
        {
            this.Headers.Add(header);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString()).Concat(this.Content).ToArray();
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            //HTTP/1.1 200 OK
            //
            result
                .Append($"{GlobalConstants.HttpOneProtocolFragment} {(int)this.StatusCode} {this.StatusCode.ToString()}")
                .Append(GlobalConstants.HttpNewLine)
                .Append(this.Headers)
                .Append(GlobalConstants.HttpNewLine);

            if (this.Cookies.HasCookies())
            {
                foreach (var httpCookie in this.Cookies)
                {
                    result.AppendLine($"{GlobalConstants.CookieResponseHeaderName}: {httpCookie}");
                }
               
            }

            result.AppendLine();


            return result.ToString();
        }
    }
}
