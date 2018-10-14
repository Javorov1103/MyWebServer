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
    using System.Collections.Generic;
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
            var controllers = application.GetType().Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Controller)));

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


            var actionParameters = methodInfo.GetParameters();
            var actionParametersObjects = new List<object>();
            foreach (var actionParameter in actionParameters)
            {
                var instance = serviceCollection.CreateInstance(actionParameter.ParameterType);

                var properies = actionParameter.ParameterType.GetProperties();

                foreach (var propertyInfo in properies)
                {
                    var key = propertyInfo.Name.ToLower();
                    object value = null;
                    if (request.FormData.Any(x => x.Key.ToLower() == key))
                    {
                        value = request.FormData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();
                    }
                    else if (request.QueryData.Any(x => x.Key.ToLower() == key))
                    {
                        value = request.QueryData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();
                    }

                    
                    propertyInfo.SetMethod.Invoke(instance, new object[]
                    {
                        value
                    });
                }

                actionParametersObjects.Add(instance);
            }

            var response = methodInfo.Invoke(controllerInstance, actionParametersObjects.ToArray()) as IHttpResponse;

            return response;
        }
    }
}
