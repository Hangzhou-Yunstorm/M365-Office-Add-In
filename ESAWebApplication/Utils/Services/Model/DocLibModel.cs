
namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// 文档对象
    /// </summary>
    public class DirFile
    {
        public string docid { get; set; }
        public string name { get; set; }
        public string rev { get; set; }
        public long size { get; set; }
        public long create_time { get; set; }
        public string creator { get; set; }
        public string editor { get; set; }
        public long modified { get; set; }
        public long csflevel { get; set; }
        public long duedate { get; set; }
        public long client_mtime { get; set; }
        public long attr { get; set; }

    }

    public class DocLibModel
    {
        public DirFile[] dirs { get; set; }
        public DirFile[] files { get; set; }
    }

}
