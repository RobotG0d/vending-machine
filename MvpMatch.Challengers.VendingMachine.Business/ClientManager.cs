using MvpMatch.Challenges.VendingMachine.Data;
using MvpMatch.Challenges.VendingMachine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvpMatch.Challenges.VendingMachine.Business
{
    public class ClientManager
    {
        public static int[] ValidCoins = new int[] { 200, 100, 50, 20, 10, 5 };

        public bool ValidateCoin(int coin, out List<string> errors)
        {
            errors = new List<string>();
            if (!ValidCoins.Contains(coin))
                errors.Add("Invalid coin. Allowed coins are: 5; 10; 20; 50; 100; 200");

            return !errors.Any();
        }

        public User AddCoin(int userId, int coin)
            => new UserRepository().AddCoin(userId, coin);

        public int[] ResetBalance(int userId)
        {
            var user = new UserRepository().ResetBalance(userId);
            return ReturnCoins(user.Deposit);
        }


        #region Private Utils

        private int[] ReturnCoins(int amount)
        {
            var coins = new int[ValidCoins.Length];
            for (var i = 0; i < ValidCoins.Length; i++)
            {
                var coinValue = ValidCoins[i];
                if (amount >= coinValue)
                {
                    coins[coins.Length - 1 - i] = amount / coinValue;
                    amount = amount % coinValue;
                }
            }

            return coins;
        }

        #endregion
    }
}
