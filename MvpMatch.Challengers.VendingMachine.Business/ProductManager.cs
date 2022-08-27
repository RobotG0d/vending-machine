using MvpMatch.Challenges.VendingMachine.Data;
using MvpMatch.Challenges.VendingMachine.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MvpMatch.Challenges.VendingMachine.Business
{
    public class ProductManager
    {
        public bool Validate(Product product, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(product.Name))
                errors.Add("Product must have a name.");

            if (product.Cost % 5 != 0)
                errors.Add("Product's cost must be a multiple of 5.");

            return !errors.Any();
        }

        public Product Create(Product product)
            => new ProductRepository().Create(product);

        public IEnumerable<Product> List()
            => new ProductRepository().List();

        public Product Get(int id)
            => new ProductRepository().Get(id);

        public Product Update(Product product)
            => new ProductRepository().Update(product);

        public void Delete(int id)
            => new ProductRepository().Delete(id);
    }
}
