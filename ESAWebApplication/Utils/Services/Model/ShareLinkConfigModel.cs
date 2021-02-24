
namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 匿名共享配置对象
    /// </summary>
    public class ShareLinkConfigModel
    {
        public bool limitexpiredays { get; set; }
        public long allowexpiredays { get; set; }
        public long allowperm { get; set; }
        public long defaultperm { get; set; }
        public bool limitaccesstimes { get; set; }
        public long allowaccesstimes { get; set; }
        public bool accesspassword { get; set; }
    }
}