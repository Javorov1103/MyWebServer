namespace CakesWebApp.Controllers
{
    using CakesWebApp.Models;
    using CakesWebApp.ViewModels.Cake;
    using MyWebServer.HTTP.Responses.Contracts;
    using SIS.MVCFrameworkd.Routing;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class CakesController : BaseController
    {
        [HttpGet("/cakes/add")]
        public IHttpResponse AddCakes()
        {
            return this.View("AddCakes");
        }

        [HttpPost("/cakes/add")]
        public IHttpResponse DoAddCakes(DoAddCakesInputModel model)
        {
            var name = model.Name.Trim();
            var price = decimal.Parse(model.Price);
            var picture = model.Picture;

            // TODO: Validation

            var product = new Product
            {
                Name = name,
                Price = price,
                ImageUrl = picture
            };
            this.db.Products.Add(product);

            try
            {
                this.db.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return this.ServerError(e.Message);
            }

            // Redirect
            return this.Redirect("/");
        }

        [HttpGet("/cakes/view")]
        public IHttpResponse ById()
        {
            var id = int.Parse(this.Request.QueryData["id"].ToString());
            var product = this.db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return this.BadRequestError("Cake not found.");
            }

            var viewBag = new Dictionary<string, string>
            {
                {"Name", product.Name},
                {"Price", product.Price.ToString(CultureInfo.InvariantCulture)},
                {"ImageUrl", product.ImageUrl}
            };
            return this.View("CakeById", viewBag);
        }
    }
}
