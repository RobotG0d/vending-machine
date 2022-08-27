namespace MvpMatch.Challenges.VendingMachine.Entities
{
    public class ProductLog
    {
        public int Id { get; set; }

        public int LogTypeId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public long Amount { get; set; }
    }
}
