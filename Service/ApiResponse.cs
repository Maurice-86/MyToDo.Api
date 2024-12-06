namespace MyToDo.Api.Service
{
    public class ApiResponse
    {
        /// <summary>
        /// 一般成功
        /// </summary>
        /// <param name="data"></param>
        public ApiResponse(object? data = null)
        {
            this.Data = data;
        }

        /// <summary>
        /// 一般失败
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public ApiResponse(int code, string msg, object? data = null)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }

        public int Code { get; set; }
        public string Msg { get; set; } = "suc";
        public object? Data { get; set; }
    }
}
