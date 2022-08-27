using System;

namespace MvpMatch.Challenges.VendingMachine.API.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}