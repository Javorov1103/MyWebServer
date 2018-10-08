namespace MyWebServer.HTTP.Responses
{
    using MyWebServer.HTTP.Common;
    using MyWebServer.HTTP.Cookies;
    using MyWebServer.HTTP.Cookies.Contracts;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Extensions;
    using MyWebServer.HTTP.Headers;
    using MyWebServer.HTTP.Headers.Contracts;
    using MyWebServer.HTTP.Responses.Contracts;
    using System.Linq;
    using System.Text;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse(){}

        public HttpResponse(
            HttpResponseStatusCode statusCode)
        {
            this.StatusCode = statusCode;
            this.Headers = new HttpHeadersCollection();
            this.Content = new byte[0];
            this.Cookies = new HttpCookieCollection();
        }

        public HttpResponseStatusCode StatusCode { get; }

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

            result.AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}")
                .AppendLine($"{this.Headers}");

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
