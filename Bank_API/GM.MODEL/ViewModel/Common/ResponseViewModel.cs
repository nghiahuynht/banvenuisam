using Newtonsoft.Json;

namespace GM.MODEL.ViewModel.Common
{
    public class ResponseViewModel<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; } = 0;

        public ResponseViewModel()
        {
        }

        public ResponseViewModel(string message, int? errorCode)
        {
            IsSuccess = false;
            Message = message;
            ErrorCode = errorCode ?? 204; // No content
        }

        public ResponseViewModel(T data, string message = null)
        {
            IsSuccess = true;
            Data = data;
            Message = message;
        }


    }

    public class ResponseModel
    {
        public string Message { get; set; }
        public int ErrorCode { get; set; }

        public ResponseModel()
        {
        }

        public ResponseModel(string message, int? errorCode)
        {
            Message = message;
            ErrorCode = errorCode ?? 302;
        }

        public ResponseModel(string message = null)
        {
            Message = message;
            ErrorCode = 0;
        }


    }

    public class ExternalApiResponseModel<T>
    {
        [JsonProperty("success")]
        public string Success { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }


}