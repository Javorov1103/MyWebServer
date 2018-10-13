namespace CakesWebApp
{
    using CakesWebApp.Controllers;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.WebServer;
    using MyWebServer.WebServer.Routing;

    public class Program
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            // {controller}/{action}/{id}
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/"] = request =>
                new HomeController { Request = request}.Index();
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/register"] = request =>
                new AccountController { Request = request }.Register();
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/login"] = request =>
                new AccountController { Request = request }.Login();
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/register"] = request =>
                new AccountController { Request = request }.DoRegister();
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/login"] = request =>
                new AccountController { Request = request }.DoLogin();
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/hello"] = request =>
                new HomeController { Request = request }.HelloUser();
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/logout"] = request =>
                new AccountController { Request = request }.Logout();
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/cakes/add"] = request =>
                new CakesController { Request = request }.AddCakes();
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/cakes/add"] = request =>
                new CakesController { Request = request }.DoAddCakes();
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/cakes/view"] = request =>
                new CakesController() { Request = request}.ById();

            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
