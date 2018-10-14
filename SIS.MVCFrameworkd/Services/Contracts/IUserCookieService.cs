
namespace SIS.MVCFrameworkd.Services
{
    public interface IUserCookieService
    {
        string GetUserCookie(string userName);
        string GetUserData(string cookie);
    }
}
