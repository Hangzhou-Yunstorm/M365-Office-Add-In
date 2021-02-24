
namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 文档库对象
    /// </summary>
    public class EntryDocLibModel
    {
        public long attr { get; set; }
        public string created_at { get; set; }
        public UserBy created_by { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public UserBy modified_by { get; set; }
        public string modified_at { get; set; }
        public string name { get; set; }
        public string rev { get; set; }
    }

    public class UserBy
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

}
