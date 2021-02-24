
namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 文件夹大小对象
    /// </summary>
    public class DirSizeModel
    {
        public long dirnum { get; set; }
        public long filenum { get; set; }
        public long recyclesize { get; set; }
        public long totalsize { get; set; }
    }
}