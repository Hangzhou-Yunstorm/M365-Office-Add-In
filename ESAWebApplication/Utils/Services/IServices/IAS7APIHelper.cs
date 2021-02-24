using ESAOfficePlugInsWeb.Models;
using ESAWebApplication.Models;
using System.Collections.Generic;

namespace ESAWebApplication.Utils.Services
{
    public interface IAS7APIHelper
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        UserModel GetLoginUser(string token, out long errorCode);

        /// <summary>
        /// 获取入口文档库
        /// </summary>
        List<EntryDocLibModel> GetEntryDocLibs(string token, string type, out long errorCode);

        /// <summary>
        /// 获取文件夹的内容
        /// </summary>
        DocLibModel GetDocLibsById(string token, string gnsId, out long errorCode);

        /// <summary>
        /// 创建文件夹
        /// </summary>
        long CreateDir(string token, string gnsId, string name);

        /// <summary>
        /// 获取建议名称
        /// </summary>
        string GetSuggestFileName(string token, string gnsId, string name);

        // <summary>
        /// 文件下载（返回Base64）
        /// </summary>
        DownloadFileRes DownloadFileBase64(OpenFileModel model);

        /// <summary>
        /// 文件下载（返回路径）
        /// </summary>
        DownloadFileRes FileDownload(OpenFileModel model, string savePath);

        /// <summary>
        /// 文件上传
        /// </summary>
        UploadFileRes UploadFile(SaveFileModel model);

        /// <summary>
        /// 大文件上传开始
        /// </summary>
        UploadFileRes UploadBigFileInit(SaveBigFileInitModel model);

        /// <summary>
        /// 大文件上传
        /// </summary>
        UploadFileRes UploadBigFile(SaveBigFileModel model);

        /// <summary>
        /// 大文件上传发送
        /// </summary>
        UploadFileRes UploadBigFileSend(SaveBigFileSendModel model);

        /// <summary>
        /// 获取文件版本信息
        /// </summary>
        List<VersionModel> GetFileVersions(string token, string gnsId, out long errorCode);

        /// <summary>
        /// 搜索
        /// </summary>
        List<SearchDoc> Search(string token, SearchModel model, out long errorCode);

        /// <summary>
        /// 获取文件路径
        /// </summary>
        string GetFilePath(string token, string gnsId, out long errorCode);

    }
}