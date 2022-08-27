using MvpMatch.Challenges.VendingMachine.Entities;
using System.Linq;

namespace MvpMatch.Challenges.VendingMachine.Data
{
    public class UserRepository : BaseRepository<User>
    {
        public User Get(string username)
            => ExecuteText<User>("SELECT * FROM [dbo].[User] WHERE [Username] = @username", new { username })
                .SingleOrDefault();

        public new User Get(int userId)
            => ExecuteText<User>("SELECT * FROM [dbo].[User] WHERE [Id] = @userId", new { userId })
                .SingleOrDefault();

        public new User Create(User entity)
            => ExecuteText<User>("INSERT [dbo].[User]([RoleId], [Username], [Password]) OUTPUT inserted.* VALUES (@roleId, @username, @password)"
                , new { entity.RoleId, entity.Username, entity.Password }
            ).SingleOrDefault();

        public User AddCoin(int userId, int coin)
            => ExecuteText<User>("UPDATE [dbo].[User] SET [Deposit] = [Deposit] + @coin OUTPUT inserted.* WHERE [Id] = @userId"
                , new { userId, coin }
            ).SingleOrDefault();

        public User ResetBalance(int userId)
            => ExecuteText<User>("UPDATE [dbo].[User] SET [Deposit] = 0 OUTPUT deleted.* WHERE [Id] = @userId"
                , new { userId }
            ).SingleOrDefault();
    }
}
