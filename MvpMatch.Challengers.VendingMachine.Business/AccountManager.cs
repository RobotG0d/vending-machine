using MvpMatch.Challenges.VendingMachine.Common.Utils;
using MvpMatch.Challenges.VendingMachine.Data;
using MvpMatch.Challenges.VendingMachine.Entities;
using MvpMatch.Challenges.VendingMachine.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvpMatch.Challenges.VendingMachine.Business
{
    public class AccountManager
    {
        public bool Validate(User user, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(user.Username))
                errors.Add("User must have a username.");
            else if (new UserRepository().Get(user.Username) != null)
                errors.Add("Username is already taken.");

            if (string.IsNullOrWhiteSpace(user.Password))
                errors.Add("User must have a password.");

            if(!Enum.IsDefined(typeof(Roles), user.RoleId))
                errors.Add("User must have a valid role: 1 (Seller); 2 (Buyer).");


            return !errors.Any();
        }

        public User Create(User user)
        {
            user.Password = CryptographyUtils.HashWithSalt(user.Password);
            return new UserRepository().Create(user);
        }

        public User Get(int id)
            => new UserRepository().Get(id);

        public User Authenticate(string username, string password)
        {
            var user = new UserRepository().Get(username);
            var valid = CryptographyUtils.VerifyHashWithSalt(password, user.Password);

            return valid ? user : null;
        }

        public void DeleteSession(int userId, Guid sessionId)
            => new UserSessionRepository().Delete(userId, sessionId);

        public UserSession CreateSession(int userId)
        {
            var session = new UserSession()
            {
                UserId = userId,
                UniqueId = Guid.NewGuid()
            };

            session = new UserSessionRepository().Create(session);

            return session;
        }

        public void DeleteSessions(int userId)
            => new UserSessionRepository().DeleteByUser(userId);

        public UserSession GetSession(int userId, Guid sessionId)
            => new UserSessionRepository().Get(userId, sessionId);
        
        public IEnumerable<UserSession> ListSessions(int userId)
            => new UserSessionRepository().List(userId);
    }
}