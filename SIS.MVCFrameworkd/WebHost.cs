namespace SIS.MVCFrameworkd
{
    using MyWebServer.HTTP.Enums;
    using MyWebServer.HTTP.Requests.Contracts;
    using MyWebServer.HTTP.Responses.Contracts;
    using MyWebServer.WebServer;
    using MyWebServer.WebServer.Results;
    using MyWebServer.WebServer.Routing;
    using SIS.MVCFrameworkd.Routing;
    using SIS.MVCFrameworkd.Services;
    using SIS.MVCFrameworkd.Services.Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            var dependecyContainer = new ServiceCollection();

            application.ConfigureServices(dependecyContainer);

            ServerRoutingTable table = new ServerRoutingTable();

            AutoRegisterRoutes(table, application, dependecyContainer);

            application.Configure();

            Server server = new Server(8000, table);

            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable table, IMvcApplication application, IServiceCollection serviceCollection)
        {
           var controllers = application.GetType().Assembly.GetTypes().Where(t=>t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Controller)));

            foreach (var controller in controllers)
            {
                var getMethods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(method => method.CustomAttributes.Any(ca => ca.AttributeType.IsSubclassOf(typeof(HttpAttribute))));

                foreach (var methodInfo in getMethods)
                {
                   var httpAttribute = (HttpAttribute)methodInfo.GetCustomAttributes(true).FirstOrDefault(x => x.GetType().IsSubclassOf(typeof(HttpAttribute)));

                    if (httpAttribute == null)
                    {
                        continue;
                    }

                    table.Add(httpAttribute.Mehtod, httpAttribute.Path, (request) => ExecuteAction(controller, methodInfo, request, serviceCollection));
                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            var controllerInstance = serviceCollection.CreateInstance(controllerType) as Controller;

            if (controllerInstance == null)
            {
                return new TextResult("Controller not found", HttpResponseStatusCode.InternalServerError);
            }

            controllerInstance.Request = request;
            controllerInstance.UserCookieService = serviceCollection.CreateInstance<IUserCookieService>();

           var response =  methodInfo.Invoke(controllerInstance, new object[] { }) as IHttpResponse;

            return response;
        }
    }
}
