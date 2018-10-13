namespace MyWebServer.WebServer.Results
{
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Headers;
    using MyWebServer.HTTP.Responses;
    public class InlineResouceResult : HttpResponse
    {
        public InlineResouceResult(byte[] content, HttpResponseStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            this.Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Content = content;
        }
    }
}