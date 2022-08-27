using System;

namespace MvpMatch.Challenges.VendingMachine.API.Models
{
    public class BaseApiResponse
    {
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    }
}