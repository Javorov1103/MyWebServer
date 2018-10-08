namespace CakesWebApp.Controllers
{
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Requests.Contracts;
    using MyWebServer.HTTP.Responses.Contracts;
    using MyWebServer.WebServer.Results;

    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            return this.View("Index");
        }

        public IHttpResponse HelloUser(IHttpRequest request)
        {
            return new HtmlResult($"<h1>Hello,{this.GetUsername(request)}!", HttpResponseStatusCode.Ok);
        }
    }
}
    
