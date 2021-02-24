using System.IO;

namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 下载文件对象
    /// </summary>
    public class DownloadRes
    {
        public Stream Stream { get; set; }
        public long ErrorCode { get; set; }
        public string ErrorDetail { get; set; }
        public string FileName { get; set; }

    }
}