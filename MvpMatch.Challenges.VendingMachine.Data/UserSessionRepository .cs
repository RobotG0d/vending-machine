using MvpMatch.Challenges.VendingMachine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvpMatch.Challenges.VendingMachine.Data
{
    public class UserSessionRepository : BaseRepository<UserSession>
    {
        public UserSession Get(int userId, Guid sessionId)
            => ExecuteText<UserSession>("SELECT * FROM [dbo].[UserSession] WHERE [UserId] = @userId AND [UniqueId] = @sessionId", new { userId, sessionId })
                .SingleOrDefault();

        public IEnumerable<UserSession> List(int userId)
            => ExecuteText<UserSession>("SELECT * FROM [dbo].[UserSession] WHERE [UserId] = @userId", new { userId });

        public void Delete(int userId, Guid sessionId)
            => ExecuteText<UserSession>("DELETE FROM [dbo].[UserSession] WHERE [UserId] = @userId AND [UniqueId] = @sessionId", new { userId, sessionId });

        public void DeleteByUser(int userId)
            => ExecuteText<UserSession>("DELETE FROM [dbo].[UserSession] WHERE [UserId] = @userId", new { userId });
    }
}
