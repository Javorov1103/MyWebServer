using IRunesWebApp.Models;
using MyWebServer.HTTP.Cookies;
using MyWebServer.HTTP.Enums;
using MyWebServer.HTTP.Requests.Contracts;
using MyWebServer.HTTP.Responses.Contracts;
using MyWebServer.WebServer.Results;
using Services;
using System.Linq;

namespace IRunesWebApp.Controllers
{
    public class UsersController : BaseController
    {
        private  HashService hashService;
        private  UserCookieService cookieService;

        public IHttpResponse Login (IHttpRequest request)
        {
            this.hashService = new HashService();
            this.cookieService = new UserCookieService();
            return this.View();
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            
            var username = request.FormData["username"];
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Context.Users.FirstOrDefault(u => u.Username == username.ToString() && u.HashedPassword == hashedPassword);

            if (user == null)
            {
                return new RedirectResult("/users/login");
            }

            

            var response = new RedirectResult("/");
            var cookieContent = this.cookieService.GetUserCookie(user.Username);

            var cookie = new HttpCookie(".auth-runes", cookieContent, 7) { HttpOnly = true };

            response.Cookies.Add(cookie);

            return response;

        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var userName = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPass = request.FormData["confirmPassword"].ToString();


            //Validate
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 4)
            {
                return new BadRequestResult("Please provide valid username with lenght atleast 4 charachters"
                    ,HttpResponseStatusCode.BadRequest);
            }

            if (this.Context.Users.Any(x => x.Username == userName))
            {
                return new BadRequestResult("User with the same username already exist!", HttpResponseStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return new BadRequestResult("Please provide password with at least 6 chars length!", HttpResponseStatusCode.BadRequest);
            }

            if (password != confirmPass)
            {
                return new BadRequestResult("Password do not match!", HttpResponseStatusCode.BadRequest);
            }

            //Generete password hash
            var hashedpassword = this.hashService.Hash(password);

            //Create user

            var user = new User
            {
                Username = userName,
                HashedPassword = hashedpassword
            };

            Context.Users.Add(user);

            try
            {
                this.Context.SaveChanges();
            }
            catch (System.Exception e)
            {
                // TODO: Log error
                return this.ServerError(e.Message);
            }

            // TODO: Login

            return new RedirectResult("/");
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View();
        }
    }
}
