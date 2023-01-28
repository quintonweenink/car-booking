namespace BookingService.Controllers;

public class Response
{
    public class ResultResponse<T> : BaseResponse
    {
        public T Result { get; set; }
    }

    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    } 
}