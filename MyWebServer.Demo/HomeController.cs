using MyWebServer.HTTP.Enums;
using MyWebServer.HTTP.Responses.Contracts;
using MyWebServer.WebServer.Results;

namespace MyWebServer.Demo
{
    public class HomeController
    {
        public IHttpResponse Index()
        {
            string content = "<h1>Hello World!</h1>";

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}
