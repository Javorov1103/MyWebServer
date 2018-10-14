namespace SIS.MVCFrameworkd.Routing
{
    using MyWebServer.HTTP.Enums;

    public class HttpGetAttribute : HttpAttribute
    {

        public HttpGetAttribute(string path)
            :base(path)
        {
        }

        public override HttpRequestMethod Mehtod => HttpRequestMethod.GET;

    }
}
