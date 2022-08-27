using MvpMatch.Challenges.VendingMachine.API.Models;
using MvpMatch.Challenges.VendingMachine.Common.Utils;
using MvpMatch.Challenges.VendingMachine.Entities;
using Newtonsoft.Json;
using System;
using System.Web;

namespace MvpMatch.Challenges.VendingMachine.API.Utils
{
    public class SessionUtils
    {
        private const string ItemName = "VendingMachineSession";
        private const string HeaderName = "x-session-token";

        internal static SessionToken GetToken()
        {
            var contextToken = HttpContext.Current.Items[ItemName] as SessionToken;
            if (contextToken != null)
                return contextToken;

            var headerToken = HttpContext.Current.Request.Headers.Get(HeaderName);
            if (headerToken == null)
                throw new Exception("User does not have an active session.");

            var decryptedToken = CryptographyUtils.Decrypt(headerToken);
            var token = JsonConvert.DeserializeObject<SessionToken>(decryptedToken);

            HttpContext.Current.Items.Add(ItemName, token);

            return token;
        }

        internal static SessionToken SetToken(User user, UserSession userSession)
        {
            var sessionToken = new SessionToken()
            {
                RoleId = user.RoleId,
                UserId = user.Id,
                SessionId = userSession.UniqueId
            };

            var serializedToken = JsonConvert.SerializeObject(sessionToken);
            var encryptedToken = CryptographyUtils.Encrypt(serializedToken);

            HttpContext.Current.Items.Add(ItemName, sessionToken);
            HttpContext.Current.Response.Headers.Add(HeaderName, encryptedToken);

            return sessionToken;
        }

        internal static void EndToken()
        {
            HttpContext.Current.Items.Remove(ItemName);
            HttpContext.Current.Response.Headers.Remove(HeaderName);
        }
    }
}