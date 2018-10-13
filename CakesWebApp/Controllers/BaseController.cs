namespace CakesWebApp.Controllers
{
    using CakesWebApp.Data;
    using SIS.MVCFrameworkd;

    public abstract class BaseController : Controller
    {

        protected BaseController()
        {
            this.db = new CakesDbContext();
        }

        protected CakesDbContext db { get; }

       

       
    }
}
