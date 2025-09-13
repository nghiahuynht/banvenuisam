namespace Infrastructure.Models
{
    public class InternalBaseDto<T>
    {
        public int Code { get; set; }
        public MessageDto Message { get; set; }
        public T Data { get; set; }
        public bool IsSuccess
        {
            get => this.Code == 200;
        }
        public bool IsNotFound
        {
            get => this.Code == -140;
        }
        public bool IsNoContent
        {
            get => this.Code == -141;
        }
        public bool IsConflict
        {
            get => this.Code == -142;
        }
     
        public class MessageDto
        {
            public string Message { get; set; }
            public string ExMessage { get; set; }
        }
    }
}
