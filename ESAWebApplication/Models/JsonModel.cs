
namespace ESAWebApplication.Models
{
    /// <summary>
    /// Post 返回 Json
    /// </summary>
    public class JsonModel
    {
        public string Data { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public long StatusCode { get; set; }
    }
}