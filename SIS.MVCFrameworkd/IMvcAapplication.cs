namespace SIS.MVCFrameworkd
{    
    using SIS.MVCFrameworkd.Services.Contracts;

    public interface IMvcApplication
    {
        void Configure();
        void ConfigureServices(IServiceCollection collection);
    }
}
