using System;

namespace MvpMatch.Challenges.VendingMachine.API.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}