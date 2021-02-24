
namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 文件版本对象
    /// </summary>
    public class VersionModel
    {
        public string rev { get; set; }
        public string name { get; set; }
        public string editor { get; set; }
        public long modified { get; set; }
        public long size { get; set; }
        public long client_mtime { get; set; }
    }
}