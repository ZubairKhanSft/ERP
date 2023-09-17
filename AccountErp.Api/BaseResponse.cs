namespace AccountErp.Api
{
    public class BaseResponse<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }

    

    public class BaseResponseGet<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public int TotalCount { get; set; }
        public string Message { get; set; }

    }

    public class BaseResponsePagination<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public int TotalCount { get; set; }
        public string Message { get; set; }
    }

    public class BaseResponseAdd
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class BaseResponseCount
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public int TotalCount { get; set; }
    }
}
