namespace CakesWebApp.Controllers
{
    using CakesWebApp.Data;
    using CakesWebApp.Models;
    using CakesWebApp.Services;
    using MyWebServer.HTTP.Cookies;
    using MyWebServer.HTTP.Requests.Contracts;
    using MyWebServer.HTTP.Responses.Contracts;
    using MyWebServer.WebServer.Results;
    using System;
    using System.Linq;

    public class AccountController : BaseController
    {
        private IHashService hashService;

        public AccountController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View("Register");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var userName = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPass = request.FormData["confirmPassword"].ToString();


            //Validate
            if (string.IsNullOrWhiteSpace(userName) || userName.Length<4)
            {
                return this.BadRequestError("Please provide valid username with lenght atleast 4 charachters");
            }

            if (this.db.Users.Any(x => x.Name == userName))
            {
                return this.BadRequestError("User with the same username already exist!");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length<6)
            {
                return this.BadRequestError("Please provide password with at least 6 chars length!");
            }

            if (password != confirmPass)
            {
                return this.BadRequestError("Password do not match!");
            }

            //Generete password hash
            var hashedpassword = this.hashService.Hash(password);

            //Create user

            var user = new User
            {
                Name = userName,
                Username = userName,
                Password = hashedpassword
            };

            db.Users.Add(user);

            try
            {
                this.db.SaveChanges();
            }
            catch (System.Exception e)
            {
                // TODO: Log error
                return this.ServerError(e.Message);
            }
           
            // TODO: Login

            return new RedirectResult("/");
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View("Login");
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var userName = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPass = this.hashService.Hash(password);

            var user = this.db.Users.FirstOrDefault(u => u.Username == userName && u.Password == hashedPass);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password");
            }

            var response = new RedirectResult("/");
            var cookieContent = this.cookieService.GetUserCookie(user.Username);

            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };

            response.Cookies.Add(cookie);

            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-cakes"))
            {
             return new RedirectResult("/");
            }

            var cookie = request.Cookies.GetCookie(".auth-cakes");

            cookie.Delete();

            var response = new RedirectResult("/");

            response.Cookies.Add(cookie);

            return response;
        }
    }
}
