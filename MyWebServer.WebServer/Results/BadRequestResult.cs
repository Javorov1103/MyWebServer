namespace MyWebServer.WebServer.Results
{
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Headers;
    using MyWebServer.HTTP.Responses;
    using System;
    using System.Text;

    public class BadRequestResult : HttpResponse
    {
        private const string errorMessage = "<h1>Error occurd!</h1>";

        public BadRequestResult(string content, HttpResponseStatusCode statusCode)
           : base(statusCode)
        {
            content = errorMessage + Environment.NewLine + content;

            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
