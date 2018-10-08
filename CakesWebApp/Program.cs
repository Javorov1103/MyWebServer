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

            serverRoutingTable.Routes[HttpRequestMethod.GET]["/hello"] = request => new HomeController().HelloUser(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/logout"] = request => new AccountController().Logout(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/register"] = request => new AccountController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/login"] = request => new AccountController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/register"] = request => new AccountController().DoRegister(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/login"] = request => new AccountController().DoLogin(request);

            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
