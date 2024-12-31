namespace ProductManagementSystem.Application.Products.Dtos
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static ResponseDto Success(object data)
        {
            return new ResponseDto { IsSuccess = true, Data = data };
        }

        public static ResponseDto Failure(string message)
        {
            return new ResponseDto { IsSuccess = false, Message = message };
        }
    }

    public class ResultDto
    {
        public object Result { get; set; }
    }

}
