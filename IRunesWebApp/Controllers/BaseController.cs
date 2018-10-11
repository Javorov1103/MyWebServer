namespace IRunesWebApp.Controllers
{
    using IRunesWebApp.Data;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Responses.Contracts;
    using MyWebServer.WebServer.Results;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class BaseController
    {
        private const string RootDirRelParh = "../../../";

        private const string ViewsFolderName = "Views";

        private string GetControllerName => this.GetType().Name.Replace("Controller", string.Empty);

        private const string FileExtension = ".html";

        private const string DirectoryDelimiter = "/";

        protected IRunesDbContext Context { get; private set; }

        public BaseController()
        {
            this.Context = new IRunesDbContext();
        }

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            string filePath = RootDirRelParh + ViewsFolderName + DirectoryDelimiter + this.GetControllerName
                + DirectoryDelimiter + viewName + FileExtension;

            if (!File.Exists(filePath))
            {
                return new BadRequestResult($"View {viewName} not found!", HttpResponseStatusCode.NotFound);
            }

            var fileContent = File.ReadAllText(filePath);

            var response = new HtmlResult(fileContent, HttpResponseStatusCode.Ok);

            return response;
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult(errorMessage, HttpResponseStatusCode.InternalServerError);
        }
    }
}