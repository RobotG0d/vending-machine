using Newtonsoft.Json;

namespace MvpMatch.Challenges.VendingMachine.Entities.Results
{
    public class ProductBuyResult : Product
    {
        public int TotalSpent { get; set; }

        public int ProductAmount { get; set; }

        public int[] Change { get; set; }

        public int ChangeValue { get; set; }
    }
}
