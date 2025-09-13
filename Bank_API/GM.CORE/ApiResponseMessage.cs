namespace GM.CORE
{
    public class ApiResponseMessage
    {
        public string Message { get; private set; }

        private ApiResponseMessage(string message)
        {
            Message = message;
        }

        public static ApiResponseMessage ErrorMessage => new ApiResponseMessage("Error");
        public static ApiResponseMessage SuccessMessage => new ApiResponseMessage("Success");
        public static ApiResponseMessage BadRequestMessage => new ApiResponseMessage("Bad Request");
        public static ApiResponseMessage ExceptionMessage => new ApiResponseMessage("Exception");
        public static ApiResponseMessage InValidLoginMessage => new ApiResponseMessage("Invalid username or password");
        public static ApiResponseMessage UserExistMessage => new ApiResponseMessage("User exist");
        public static ApiResponseMessage UserNoExistMessage => new ApiResponseMessage("User no exist");
    }
}