namespace CakesWebApp.Controllers
{
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Requests.Contracts;
    using MyWebServer.HTTP.Responses.Contracts;
    using MyWebServer.WebServer.Results;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            return this.View("Index");
        }
    }
}
    
