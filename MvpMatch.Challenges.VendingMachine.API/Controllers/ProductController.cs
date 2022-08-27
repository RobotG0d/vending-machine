using MvpMatch.Challenges.VendingMachine.API.Filters;
using MvpMatch.Challenges.VendingMachine.API.Models;
using MvpMatch.Challenges.VendingMachine.API.Models.Products;
using MvpMatch.Challenges.VendingMachine.API.Utils;
using MvpMatch.Challenges.VendingMachine.Business;
using MvpMatch.Challenges.VendingMachine.Entities;
using MvpMatch.Challenges.VendingMachine.Entities.Enums;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvpMatch.Challenges.VendingMachine.API.Controllers
{
    [RoutePrefix("product")]
    public class ProductController : BaseVendingMachineController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage List()
        {
            var productManager = new ProductManager();

            var product = productManager.List();

            return Request.CreateResponse(product);
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var productManager = new ProductManager();

            var product = productManager.Get(id);
            if (product == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(product);
        }

        [HttpPost]
        [Route("")]
        [RoleFilter(Roles.Seller)]
        public HttpResponseMessage Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Cost = request.Cost,
                SellerId = SessionToken.UserId,
                AmountAvailable = request.AmountAvailable
            };

            var productManager = new ProductManager();
            if (!productManager.Validate(product, out var errors))
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);

            product = productManager.Create(product);

            return Request.CreateResponse(HttpStatusCode.Created, product);
        }

        [HttpPut]
        [Route("{id}")]
        [RoleFilter(Roles.Seller)]
        public HttpResponseMessage Update(int id, ProductUpdateRequest request)
        {
            var productManager = new ProductManager();

            var product = productManager.Get(id);
            product.Cost = request.Cost;
            product.AmountAvailable = request.AmountAvailable;

            if (product.SellerId != SessionToken.UserId)
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Only the product's owner is allowed to make changes.");

            if (!productManager.Validate(product, out var errors))
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);

            product = productManager.Update(product);

            return Request.CreateResponse(product);
        }

        [HttpDelete]
        [Route("{id}")]
        [RoleFilter(Roles.Seller)]
        public HttpResponseMessage Delete(int id)
        {
            var productManager = new ProductManager();
            var product = productManager.Get(id);

            if (product.SellerId != SessionToken.UserId)
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Only the product's owner is allowed to remove the product.");

            productManager.Delete(id);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
