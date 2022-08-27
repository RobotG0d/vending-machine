using Newtonsoft.Json;

namespace MvpMatch.Challenges.VendingMachine.Entities
{
    public class User
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public int Deposit { get; set; }
    }
}
