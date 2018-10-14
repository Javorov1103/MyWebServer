namespace CakesWebApp.Controllers
{
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Requests.Contracts;
    using MyWebServer.HTTP.Responses.Contracts;
    using MyWebServer.WebServer.Results;
    using SIS.MVCFrameworkd.Routing;
    using System.Collections.Generic;

    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            return this.View("Index");
        }

        [HttpGet("/hello")]
        public IHttpResponse HelloUser()
        {
            return this.View("HelloUser", new Dictionary<string, string>
            {
                { "Username", this.User}
            }
            );
        }
    }
}
    
