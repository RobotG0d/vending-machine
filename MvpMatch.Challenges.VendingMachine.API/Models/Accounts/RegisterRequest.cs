namespace MvpMatch.Challenges.VendingMachine.API.Models.Accounts
{
    public class RegisterRequest
    {
        public int RoleId { get; set; }

        public string Username { get; set; }
        
        public string Password { get; set; }
    }
}