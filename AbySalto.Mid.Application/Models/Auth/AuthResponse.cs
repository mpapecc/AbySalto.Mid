namespace AbySalto.Mid.Application.Models.Auth
{
    public class AuthResponse
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
