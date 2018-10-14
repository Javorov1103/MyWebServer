namespace CakesWebApp.Controllers
{
    using CakesWebApp.Models;
    using CakesWebApp.ViewModels.Account;
    using MyWebServer.HTTP.Cookies;
    using MyWebServer.HTTP.Responses.Contracts;
    using SIS.MVCFrameworkd.Routing;
    using SIS.MVCFrameworkd.Services;
    using System.Linq;

    public class AccountController : BaseController
    {
        private IHashService hashService;

        public AccountController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/register")]
        public IHttpResponse Register()
        {
            return this.View("Register");
        }

        [HttpPost("/register")]
        public IHttpResponse DoRegister(DoRegisterInputModel model)
        {
            var userName = model.Username.Trim();
            var password = model.Password;
            var confirmPass = model.ConfirmPassword;



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

            return this.Redirect("/");
        }

        [HttpGet("/login")]
        public IHttpResponse Login()
        {
            return this.View("Login");
        }

        [HttpPost("/login")]
        public IHttpResponse DoLogin(DoLoginInputModel model)
        {
            var userName = model.Username.Trim();
            var password = model.Password;

            var hashedPass = this.hashService.Hash(password);

            var user = this.db.Users.FirstOrDefault(u => u.Username == userName && u.Password == hashedPass);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password");
            }

            ;
            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };

            this.Response.Cookies.Add(cookie);

            return this.Redirect("/");
        }

        [HttpGet("/logout")]
        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(".auth-cakes");

            cookie.Delete();
            
            this.Response.Cookies.Add(cookie);

            return this.Redirect("/");
        }
    }
}
