namespace SIS.MVCFrameworkd
{
    using MyWebServer.WebServer;
    using MyWebServer.WebServer.Routing;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            application.ConfigureServices();

            ServerRoutingTable table = new ServerRoutingTable();

            application.Configure(table);

            Server server = new Server(8000, table);

            server.Run();
        }
    }
}
