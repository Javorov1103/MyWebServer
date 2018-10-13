namespace CakesWebApp.Controllers
{
    using CakesWebApp.Models;
    using MyWebServer.HTTP.Cookies;
    using MyWebServer.HTTP.Responses.Contracts;
    using MyWebServer.WebServer.Results;
    using System;
    using System.Linq;

    public class AccountController : BaseController
    {
        

        public AccountController()
        {
            
        }

        public IHttpResponse Register()
        {
            return this.View("Register");
        }

        public IHttpResponse DoRegister()
        {
            var userName = this.Request.FormData["username"].ToString().Trim();
            var password = this.Request.FormData["password"].ToString();
            var confirmPass = this.Request.FormData["confirmPassword"].ToString();


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
            var hashedpassword = this.HashService.Hash(password);

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

        public IHttpResponse Login()
        {
            return this.View("Login");
        }

        public IHttpResponse DoLogin()
        {
            var userName = this.Request.FormData["username"].ToString().Trim();
            var password = this.Request.FormData["password"].ToString();

            var hashedPass = this.HashService.Hash(password);

            var user = this.db.Users.FirstOrDefault(u => u.Username == userName && u.Password == hashedPass);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password");
            }

            var response = new RedirectResult("/");
            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };

            response.Cookies.Add(cookie);

            return response;
        }

        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth-cakes"))
            {
             return new RedirectResult("/");
            }

            var cookie = this.Request.Cookies.GetCookie(".auth-cakes");

            cookie.Delete();

            var response = new RedirectResult("/");

            response.Cookies.Add(cookie);

            return response;
        }
    }
}
