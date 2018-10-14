namespace SIS.MVCFrameworkd.Routing
{
    using System;
    using MyWebServer.HTTP.Enums;

    public class HttpPostAttribute : HttpAttribute
    {
        public HttpPostAttribute(string path)
            :base (path)
        {
           
        }

        public override HttpRequestMethod Mehtod => HttpRequestMethod.POST;
    }
}
