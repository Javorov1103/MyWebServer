namespace SIS.MVCFrameworkd.Routing
{
    using MyWebServer.HTTP.Enums;
    using System;

    public abstract class HttpAttribute : Attribute
    {
        protected HttpAttribute(string path)
        {
            this.Path = path;
        }

        public string Path { get; protected set; }

        public abstract HttpRequestMethod Mehtod { get; }
    }
}
