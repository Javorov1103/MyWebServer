namespace IRunesWebApp.Controllers
{
    using IRunesWebApp.Data;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Responses.Contracts;
    using SIS.MVCFrameworkd;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class BaseController : Controller
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
       
    }
}