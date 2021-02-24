
namespace ESAOfficePlugInsWeb.Models
{
    /// <summary>
    /// 保存邮件对象
    /// </summary>
    public class SaveEmailModel
    {
        public string EwsUrl { get; set; }

        public string EwsId { get; set; }

        public string EwsToken { get; set; }

        public string TokenId { get; set; }

        public string FileName { get; set; }

        public string Docid { get; set; }

        public int Ondup { get; set; }
    }
}