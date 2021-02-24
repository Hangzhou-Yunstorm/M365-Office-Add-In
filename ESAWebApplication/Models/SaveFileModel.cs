
namespace ESAOfficePlugInsWeb.Models
{
    /// <summary>
    /// 保存文件对象
    /// </summary>
    public class SaveFileModel
    {
        public string FileName { get; set; }

        public string TokenId { get; set; }

        public byte[] Base64Str { get; set; }

        public string Docid { get; set; }

        public long Ondup { get; set; }
    }
}