namespace GM.MODEL.ViewModel
{
    public class LoginViewModel
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string StartedTime { get; set; }
        public string ExpiredTime { get; set; }
    }

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}