namespace MvpMatch.Challenges.VendingMachine.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public int SellerId { get; set; }

        public string Name { get; set; }

        public int Cost { get; set; }

        public int AmountAvailable { get; set; }
    }
}
