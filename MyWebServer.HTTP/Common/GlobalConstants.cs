namespace MyWebServer.HTTP.Common
{
   public static class GlobalConstants
    {
        public const string HttpOneProtocolFragment = "HTTP/1.1";

        public const string HostHeaderKey = "Host";

        public const int ExactNumberOfParametersInRequestParameterKeValuePair = 2;

        public const string CookieRequestHeaderName = "Cookie";
        public const string CookieResponseHeaderName = "Set-Cookie";

        public static string[] ResourceExtensions = { ".js", ".css" };
    }
}
