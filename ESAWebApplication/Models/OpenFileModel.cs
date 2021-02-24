
namespace ESAOfficePlugInsWeb.Models
{
    /// <summary>
    /// 打开文件对象
    /// </summary>
    public class OpenFileModel
    {
        public string FileId { get; set; }

        public string TokenId { get; set; }

        public string Rev { get; set; }

        public string DocType { get; set; }
    }
}