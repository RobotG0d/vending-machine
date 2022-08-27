namespace MvpMatch.Challenges.VendingMachine.API.Models
{
    public class SuccessApiResponse<T> : BaseApiResponse
    {
        public T Data { get; set; }

        public SuccessApiResponse(T data) => Data = data;
    }

}