using System.Collections.Generic;

namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 匿名共享对象
    /// </summary>
    public class ShareLinkBasicModel
    {
        public string type { get; set; }
        public string id { get; set; }
        public string created_at { get; set; }
        public string password { get; set; }
        public long limited_times { get; set; }
        public string expires_at { get; set; }
        public string title { get; set; }
    }

    /// <summary>
    /// 匿名共享对象
    /// </summary>
    public class ShareLinkModel : ShareLinkBasicModel
    {
        public SItem item { get; set; }
    }

    public class SItem
    {
        public long perm { get; set; }
    }

    /// <summary>
    /// 匿名共享对象原数据
    /// </summary>
    public class OriginShareLinkModel : ShareLinkBasicModel
    {
        public OItem item { get; set; }
    }

    public class OItem
    {
        public List<string> perms { get; set; }
    }
}