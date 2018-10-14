namespace IRunesWebApp
{
    using IRunesWebApp.Controllers;
    using MyWebServer.HTTP.Enums;
    using MyWebServer.WebServer;
    using MyWebServer.WebServer.Results;
    using MyWebServer.WebServer.Routing;

    public class StartUp
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            //GET
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Home/Index"] = request => new RedirectResult("/");
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/users/login"] = request => new UsersController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/users/register"] = request => new UsersController().Register(request);

            //POST
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/users/login"] = request => new UsersController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/users/register"] = request => new UsersController().DoRegister(request);

            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
