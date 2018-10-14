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
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

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
            //Get all controllers
            var controllers = application.GetType().Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Controller)));


            //Go through all the controllers
            foreach (var controller in controllers)
            {
                //Take all the methods of every controller
                var getMethods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(method => method.CustomAttributes.Any(ca => ca.AttributeType.IsSubclassOf(typeof(HttpAttribute))));

                //Go trough all the methods of the controller
                foreach (var methodInfo in getMethods)
                {
                    //Get the custom HttpAttribute of the method GET or POST (for now...)
                    var httpAttribute = (HttpAttribute)methodInfo.GetCustomAttributes(true).FirstOrDefault(x => x.GetType().IsSubclassOf(typeof(HttpAttribute)));

                    if (httpAttribute == null)
                    {
                        continue;
                    }

                    //Add to the routing table (HttpMethod (from the HttpAttribute), the path (from the HttpAttribute) and Invoke a specific Action that accepts Request and returns a Response)
                    table.Add(httpAttribute.Mehtod, httpAttribute.Path, (request) => ExecuteAction(controller, methodInfo, request, serviceCollection));
                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            //Instanciate the controller
            var controllerInstance = serviceCollection.CreateInstance(controllerType) as Controller;

            if (controllerInstance == null)
            {
                return new TextResult("Controller not found", HttpResponseStatusCode.InternalServerError);
            }

            //Set the Request of the controller
            controllerInstance.Request = request;

            //Create instance if the UserCookieService
            controllerInstance.UserCookieService = serviceCollection.CreateInstance<IUserCookieService>();

            //Get the parameters of the method that we will invoke
            List<object> actionParametersObjects = GetActionParameters(methodInfo, request, serviceCollection);

            //Invoke the method with the controller and the parameters and we make the response that we will return
            var response = methodInfo.Invoke(controllerInstance, actionParametersObjects.ToArray()) as IHttpResponse;

            return response;
        }

        private static List<object> GetActionParameters(MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            //We take all the parameters that an action need for its invokation
            var actionParameters = methodInfo.GetParameters();
            var actionParametersObjects = new List<object>();

            //Go trough all the parameters
            foreach (var actionParameter in actionParameters)
            {
                //we create an instance ef every parameter of the action
                var instance = serviceCollection.CreateInstance(actionParameter.ParameterType);

                //we get the properities of the concreate parameter
                var properies = actionParameter.ParameterType.GetProperties();


                //Go trough all the properies of the parameter of the action and set them with the data form the request
                foreach (var propertyInfo in properies)
                {
                    var key = propertyInfo.Name.ToLower();
                    string stringValue = null;
                    if (request.FormData.Any(x => x.Key.ToLower() == key))
                    {
                        stringValue = request.FormData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();
                    }
                    else if (request.QueryData.Any(x => x.Key.ToLower() == key))
                    {
                        stringValue = request.QueryData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();
                    }

                    var typeCode = Type.GetTypeCode(propertyInfo.PropertyType);

                    //Invoke our custom TryParse Method
                    // If we want to use another type of data just add it to the Switch-case statement
                    object value = TryParse(stringValue, typeCode);

                    propertyInfo.SetMethod.Invoke(instance, new object[]
                    {
                        value
                    });
                }

                actionParametersObjects.Add(instance);
            }

            return actionParametersObjects;
        }


        // If we want to use another type of data just add it to the Switch-case statement
        private static object TryParse(string stringValue, TypeCode typeCode)
        {
            object value = stringValue;

            switch (typeCode)
            {
                case TypeCode.Int32:
                    if (int.TryParse(stringValue, out var intValue))
                    {
                        value = intValue;
                    }
                    break;
                case TypeCode.Char:
                    if (char.TryParse(stringValue, out var charValue))
                    {
                        value = charValue;
                    }
                    break;

                case TypeCode.Int64:
                    if (long.TryParse(stringValue, out var longValue))
                    {
                        value = longValue;
                    }
                    break;
                case TypeCode.DateTime:
                    if (DateTime.TryParse(stringValue, out var dateTimeValue))
                    {
                        value = dateTimeValue;
                    }
                    break;

                case TypeCode.Decimal:
                    if (decimal.TryParse(stringValue, out var decimalValue))
                    {
                        value = decimalValue;
                    }
                    break;
                case TypeCode.Double:
                    if (double.TryParse(stringValue, out var doubleValue))
                    {
                        value = doubleValue;
                    }
                    break;
            }

            return value;
        }
    }
}
