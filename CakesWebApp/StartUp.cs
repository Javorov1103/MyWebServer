namespace CakesWebApp
{
    using CakesWebApp.Controllers;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.WebServer.Routing;
    using SIS.MVCFrameworkd;

    public class StartUp : IMvcApplication
    {
        public void Configure(ServerRoutingTable table)
        {
            // {controller}/{action}/{id}
            table.Routes[HttpRequestMethod.GET]["/"] = request =>
                new HomeController { Request = request }.Index();
            table.Routes[HttpRequestMethod.GET]["/register"] = request =>
                new AccountController { Request = request }.Register();
            table.Routes[HttpRequestMethod.GET]["/login"] = request =>
                new AccountController { Request = request }.Login();
            table.Routes[HttpRequestMethod.POST]["/register"] = request =>
                new AccountController { Request = request }.DoRegister();
            table.Routes[HttpRequestMethod.POST]["/login"] = request =>
                new AccountController { Request = request }.DoLogin();
            table.Routes[HttpRequestMethod.GET]["/logout"] = request =>
                new AccountController { Request = request }.Logout();
            table.Routes[HttpRequestMethod.GET]["/cakes/add"] = request =>
                new CakesController { Request = request }.AddCakes();
            table.Routes[HttpRequestMethod.POST]["/cakes/add"] = request =>
                new CakesController { Request = request }.DoAddCakes();
            table.Routes[HttpRequestMethod.GET]["/cakes/view"] = request =>
                new CakesController() { Request = request }.ById();
        }

        public void ConfigureServices()
        {
           //TODO: Implement IoC/DI container
        }
    }

   

}
