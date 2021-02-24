
namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 错误对象
    /// </summary>
    public class ErrorModel
    {
        public string cause { get; set; }
        public long code { get; set; }
        public string message { get; set; }
        public object detail { get; set; }

    }
}