namespace SIS.MVCFrameworkd
{
    
    using System.Net;

    public static class StringExtensions
    {
        public static string UrlDecode(this string input)
        {
            return WebUtility.UrlDecode(input);
        }
    }
}
