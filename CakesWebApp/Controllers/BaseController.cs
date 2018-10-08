namespace CakesWebApp.Controllers
{
    using CakesWebApp.Data;
    using CakesWebApp.Services;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Requests.Contracts;
    using MyWebServer.HTTP.Responses.Contracts;
    using MyWebServer.WebServer.Results;
    using System.IO;

    public abstract class BaseController
    {

        protected BaseController()
        {
            this.db = new CakesDbContext();
            this.cookieService = new UserCookieService();
        }

        protected CakesDbContext db { get; }

        protected IUserCookieService cookieService;

        protected string GetUsername(IHttpRequest request)
        {
            if(!request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return null;
            }

            var cookie = request.Cookies.GetCookie(".auth-cakes");

            var cookieContent = cookie.Value;

            var userName = this.cookieService.GetUserData(cookieContent);

            return userName;
        }

        protected IHttpResponse View(string viewName)
        {
            var content = File.ReadAllText("Views/" + viewName + ".html");

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        protected IHttpResponse BadRequestError (string errorMessage)
        {
            return new HtmlResult(errorMessage, HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult(errorMessage, HttpResponseStatusCode.InternalServerError);
        }
    }
}
