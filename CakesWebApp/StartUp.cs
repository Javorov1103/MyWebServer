namespace CakesWebApp
{
    using SIS.MVCFrameworkd;
    using SIS.MVCFrameworkd.Logger;
    using SIS.MVCFrameworkd.Logger.Contracts;
    using SIS.MVCFrameworkd.Services;
    using SIS.MVCFrameworkd.Services.Contracts;

    public class StartUp : IMvcApplication
    {
        public void Configure()
        {
           
        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<IHashService, HashService>();
            collection.AddService<IUserCookieService, UserCookieService>();
            collection.AddService<ILogger, ConsoleLogger>();
        }
    }

   



}
