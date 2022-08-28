using MvpMatch.Challenges.VendingMachine.Entities;
using MvpMatch.Challenges.VendingMachine.Entities.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MvpMatch.Challenges.VendingMachine.Data
{
    public class ProductRepository : BaseRepository<Product>
    {
        public IEnumerable<Product> List()
            => ExecuteText<Product>("SELECT * FROM [dbo].[Product]");

        public ProductBuyResult Buy(int userId, int productId, int productAmount)
        {
            try
            {
                return ExecuteStoredProcedure<ProductBuyResult>(
                    "[dbo].[ProductBuy]",
                    new { userId, productId, productAmount }
                ).SingleOrDefault();
            }
            catch(SqlException ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
