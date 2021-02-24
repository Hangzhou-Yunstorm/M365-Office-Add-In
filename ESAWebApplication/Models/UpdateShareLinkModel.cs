using System.Collections.Generic;

namespace ESAWebApplication.Models
{
    /// <summary>
    /// 更新匿名共享对象
    /// </summary>
    public class UpdateShareLinkModel
    {
        public string link_id { get; set; }
        public UItem item { get; set; }
        public string title { get; set; }
        public string expires_at { get; set; }
        public string password { get; set; }
        public long limited_times { get; set; }
    }

    public class UItem
    {
        public List<string> perms { get; set; }
    }
}