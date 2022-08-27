namespace MvpMatch.Challenges.VendingMachine.API.Models.Products
{
    public class ProductCreateRequest
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public int AmountAvailable { get; set; }
    }
}