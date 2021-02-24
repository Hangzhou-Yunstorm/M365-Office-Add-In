
namespace ESAOfficePlugInsWeb.Models
{
    /// <summary>
    /// 保存大文件对象
    /// </summary>
    public class SaveBigFileModel
    {
        public string FileName { get; set; }

        public string TokenId { get; set; }

        public byte[] FileBytes { get; set; }

        public string Docid { get; set; }

        public string Rev { get; set; }

        public string UploadId { get; set; }

        public long PartIndex { get; set; }

        public long TotalParts { get; set; }

        public string PartsInfo { get; set; }
    }

    /// <summary>
    /// 保存大文件对象发送
    /// </summary>
    public class SaveBigFileSendModel
    {
        public string FileName { get; set; }

        public string TokenId { get; set; }

        public string Docid { get; set; }

        public string Rev { get; set; }

        public string UploadId { get; set; }

        public string PartsInfo { get; set; }
    }

    /// <summary>
    /// 保存大文件对象开始
    /// </summary>
    public class SaveBigFileInitModel
    {
        public string FileName { get; set; }

        public string TokenId { get; set; }

        public long Length { get; set; }

        public string Docid { get; set; }

        public long Ondup { get; set; }
    }

    public class BeginUploadModel
    {
        public string docid { get; set; }
        public string name { get; set; }
        public string rev { get; set; }
        public string uploadid { get; set; }
    }

    public class AuthRequestsModel
    {
        public object authrequests { get; set; }
    }

}