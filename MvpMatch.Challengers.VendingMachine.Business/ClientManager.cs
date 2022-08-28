using MvpMatch.Challenges.VendingMachine.Data;
using MvpMatch.Challenges.VendingMachine.Entities;
using MvpMatch.Challenges.VendingMachine.Entities.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvpMatch.Challenges.VendingMachine.Business
{
    public class ClientManager
    {
        public static int[] ValidCoins = new int[] { 100, 50, 20, 10, 5 };

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

        public ProductBuyResult Buy(int userId, int productId, int amount, out string error)
        {
            try
            {
                error = null;

                var result = new ProductRepository().Buy(userId, productId, amount);
                result.Change = ReturnCoins(result.ChangeValue);

                return result;
            }
            catch (ApplicationException ex)
            {
                error = ex.Message;
                return null;
            }
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
