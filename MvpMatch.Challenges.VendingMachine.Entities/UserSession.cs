using System;

namespace MvpMatch.Challenges.VendingMachine.Entities
{
    public class UserSession
    {
        public int Id { get; set; }

        public int UniqueId { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
