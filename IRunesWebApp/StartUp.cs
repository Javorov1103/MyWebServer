namespace IRunesWebApp
{
    using MyWebServer.WebServer;
    using MyWebServer.WebServer.Routing;

    public class StartUp
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
