namespace MyWebServer.HTTP.Enums
{
    public enum HttpResponseStatusCode
    {
        Ok = 200,
        Created =201,
        Found = 302,
        PermenantRedirect =301,
        SeeOther = 303,
        TemporaryRedirect = 307,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500
    }
}
