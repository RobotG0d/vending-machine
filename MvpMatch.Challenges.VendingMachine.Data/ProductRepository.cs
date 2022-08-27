using MvpMatch.Challenges.VendingMachine.Entities;
using System.Collections.Generic;

namespace MvpMatch.Challenges.VendingMachine.Data
{
    public class ProductRepository : BaseRepository<Product>
    {
        public IEnumerable<Product> List()
            => ExecuteText<Product>("SELECT * FROM [dbo].[Product]");
    }
}
