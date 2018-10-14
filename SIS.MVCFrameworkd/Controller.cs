namespace SIS.MVCFrameworkd
{
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Headers;
    using MyWebServer.HTTP.Requests.Contracts;
    using MyWebServer.HTTP.Responses;
    using MyWebServer.HTTP.Responses.Contracts;
    using SIS.MVCFrameworkd.Services;
    using System.Collections.Generic;
    using System.Text;

    public class Controller
    {
        public Controller()
        {
            this.Response = new HttpResponse();
        }

        public IHttpResponse Response { get; set; }

        public IHttpRequest Request { get; set; }


        public IUserCookieService UserCookieService { get; internal set; }

        //protected IHashService HashService { get; set; }

        protected string User
        {
            get
            {
                if (!this.Request.Cookies.ContainsCookie(".auth-cakes"))
                {
                    return null;
                }

                var cookie = this.Request.Cookies.GetCookie(".auth-cakes");
                var cookieContent = cookie.Value;
                var userName = this.UserCookieService.GetUserData(cookieContent);
                return userName;
            }
        }

        protected IHttpResponse View(string viewName, IDictionary<string, string> viewBag = null)
        {
            if (viewBag == null)
            {
                viewBag = new Dictionary<string, string>();
            }

            var allContent = this.GetViewContent(viewName, viewBag);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse File(byte[] content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Response.Content = content;
            this.Response.StatusCode = HttpResponseStatusCode.Ok;

            return this.Response;
        }

        protected IHttpResponse Redirect(string location)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.Location, location));
            this.Response.StatusCode = HttpResponseStatusCode.SeeOther;
            return this.Response;
        }

        protected IHttpResponse Text(string content)
        {
            this.Response.Headers.Add(new HttpHeader("Content-Type", "text/plain"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
            this.Response.StatusCode = HttpResponseStatusCode.Ok;

            return this.Response;
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Error", errorMessage);
            var allContent = this.GetViewContent("Error", viewBag);
            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;

            return this.Response;
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Error", errorMessage);
            var allContent = this.GetViewContent("Error", viewBag);
            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpResponseStatusCode.InternalServerError;
            return this.Response;
        }

        private string GetViewContent(string viewName,
            IDictionary<string, string> viewBag)
        {
            var layoutContent = System.IO.File.ReadAllText("Views/_Layout.html");
            var content = System.IO.File.ReadAllText("Views/" + viewName + ".html");
            foreach (var item in viewBag)
            {
                content = content.Replace("@Model." + item.Key, item.Value);
            }

            var allContent = layoutContent.Replace("@RenderBody()", content);
            return allContent;
        }

        private void PrepareHtmlResult(string content)
        {
            this.Response.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
