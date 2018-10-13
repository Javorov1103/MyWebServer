﻿namespace CakesWebApp.Controllers
{
    using CakesWebApp.Extensions;
    using CakesWebApp.Models;
    using MyWebServer.HTTP.Requests.Contracts;
    using MyWebServer.HTTP.Responses.Contracts;
    using MyWebServer.WebServer.Results;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class CakesController : BaseController
    {
        public IHttpResponse AddCakes()
        {
            return this.View("AddCakes");
        }

        public IHttpResponse DoAddCakes()
        {
            var name = this.Request.FormData["name"].ToString().Trim().UrlDecode();
            var price = decimal.Parse(this.Request.FormData["price"].ToString().UrlDecode());
            var picture = this.Request.FormData["picture"].ToString().Trim().UrlDecode();

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
            return new RedirectResult("/");
        }

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