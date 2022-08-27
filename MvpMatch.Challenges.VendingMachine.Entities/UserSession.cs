using System;

namespace MvpMatch.Challenges.VendingMachine.Entities
{
    public class UserSession
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public Guid UniqueId { get; set; }
    }
}
