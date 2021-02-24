
namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 上传文件对象
    /// </summary>
    public class UploadFileRes
    {
        public string FileId { get; set; }
        public long ErrorCode { get; set; }
        public string ErrorDetail { get; set; }
        public string FileName { get; set; }

    }
}