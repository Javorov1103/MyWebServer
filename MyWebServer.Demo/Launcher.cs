namespace MyWebServer.Demo
{
    using MyWebServer.HTTP.Enums;
    using MyWebServer.WebServer;
    using MyWebServer.WebServer.Routing;

   public class Launcher
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable(); 

            serverRoutingTable.Routes[HttpRequestMethod.GET]["/"] = request => new HomeController().Index(request);

            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
