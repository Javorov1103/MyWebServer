namespace SIS.MVCFrameworkd
{
    using MyWebServer.WebServer.Routing;

    public interface IMvcApplication
    {
        void Configure(ServerRoutingTable table);
        void ConfigureServices();
    }
}
