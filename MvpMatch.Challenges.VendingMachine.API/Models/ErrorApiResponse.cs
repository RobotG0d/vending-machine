using System;

namespace MvpMatch.Challenges.VendingMachine.API.Models
{
    public class ErrorApiResponse : BaseApiResponse
    {
        public string ErrorMessage { get; set; }

        public ErrorApiResponse(string message) 
            => ErrorMessage = message;

        public ErrorApiResponse(string message, params object[] values)
            => ErrorMessage = string.Format(message, values);

        public ErrorApiResponse(Exception exception) : this(exception?.Message) { }
    }
}