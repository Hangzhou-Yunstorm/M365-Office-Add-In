using System.Collections.Generic;

namespace ESAWebApplication.Models
{
    /// <summary>
    /// 创建匿名共享
    /// </summary>
    public class CreatShareLinkModel
    {
        public CItem item { get; set; }
        public string title { get; set; }
        public string expires_at { get; set; }
        public string password { get; set; }
        public long limited_times { get; set; }

    }

    public class CItem
    {
        public string id { get; set; }
        public string type { get; set; }
        public List<string> perms { get; set; }
    }

    /// <summary>
    /// 创建实名共享
    /// </summary>
    public class CreatRealNameShareLinkModel
    {
        public ShareLinkItem item { get; set; }
    }

    public class ShareLinkItem
    {
        public string id { get; set; }
        public string type { get; set; }
    }
}