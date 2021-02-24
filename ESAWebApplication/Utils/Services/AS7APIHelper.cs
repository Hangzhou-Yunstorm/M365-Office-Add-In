using ESAOfficePlugInsWeb.Models;
using ESAOfficePlugInsWeb.Utils;
using ESAWebApplication.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// AS7 API Helper
    /// </summary>
    public class AS7APIHelper : IAS7APIHelper
    {
        /// <summary>
        /// log4net
        /// </summary>
        log4net.ILog _log = log4net.LogManager.GetLogger("AS7APIHelper");

        /// <summary>
        /// GetLoginUser
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>LoginUser</returns>
        public UserModel GetLoginUser(string token, out long errorCode)
        {
            try
            {
                errorCode = 0;

                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/eacp/v1/user/get", null).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;


                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"GetLoginUser Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        return null;
                    }
                    return JsonConvert.DeserializeObject<UserModel>(resStr);
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"GetLoginUser Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// GetEntryDocLibs
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="type">type</param>
        /// <returns>EntryDocLibs</returns>
        public List<EntryDocLibModel> GetEntryDocLibs(string token, string type, out long errorCode)
        {
            try
            {
                errorCode = 0;

                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);
                    var res = httpClient.GetAsync($"{Constant.OAuth2Url}/api/efast/v1/entry-doc-lib?type={type}").Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;


                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"GetFileVersions Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<EntryDocLibModel>>(resStr);
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"GetEntryDocLibs Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// GetDocLibsById
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="gnsId">gnsId</param>
        /// <returns>DocLibs</returns>
        public DocLibModel GetDocLibsById(string token, string gnsId, out long errorCode)
        {
            try
            {
                errorCode = 0;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);
                    var json = "{\"docid\":\"" + gnsId + "\",\"by\":\"name\"}";
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/dir/list", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"GetDocLibsById Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        return null;
                    }

                    return JsonConvert.DeserializeObject<DocLibModel>(resStr);
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"GetDocLibsById Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// GetFileVersions
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="gnsId">gnsId</param>
        /// <returns>FileVersions</returns>
        public List<VersionModel> GetFileVersions(string token, string gnsId, out long errorCode)
        {
            try
            {
                errorCode = 0;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                    var json = "{\"docid\":\"" + gnsId + "\"}";
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var httpResponse = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/file/revisions", content).Result;
                    var resStr = httpResponse.Content.ReadAsStringAsync().Result;


                    int resCode = (int)httpResponse.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"GetFileVersions Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<VersionModel>>(resStr);
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"GetFileVersions Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// CreateDir
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="gnsId">gnsId</param>
        /// <returns>Dir</returns>
        public long CreateDir(string token, string gnsId, string name)
        {
            try
            {
                long errorCode = 0;

                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                    var json = "{\"docid\":\"" + gnsId + "\",\"name\":\"" + name + "\"}";
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/dir/create", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;


                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"UploadFile Osbeginupload Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                    }
                    return errorCode;
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"CreateDir Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// GetSuggestFileName
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="gnsId">gnsId</param>
        /// <returns>SuggestFileName</returns>
        public string GetSuggestFileName(string token, string gnsId, string name)
        {
            try
            {
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                    var json = "{\"docid\":\"" + gnsId + "\",\"name\":\"" + name + "\"}";
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/file/getsuggestname", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;
                    int resCode = (int)res.StatusCode;

                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"GetSuggestFileName Exception: {errModel.cause}, Code: {errModel.code}");
                        return $"{Guid.NewGuid()}{Path.GetExtension(name)}";
                    }

                    return JsonConvert.DeserializeObject<dynamic>(resStr)["name"].Value;
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"GetSuggestFileName Exception: {ex.Message}");
                return $"{Guid.NewGuid()}{Path.GetExtension(name)}";
            }
        }

        /// <summary>
        /// 文件下载（返回Base64）
        /// </summary>
        /// <returns>文件Base64</returns>
        public DownloadFileRes DownloadFileBase64(OpenFileModel model)
        {
            DownloadFileRes downloadFile = new DownloadFileRes();
            try
            {
                var downloadRes = GetDownloadFileStream(model);
                if (downloadRes.ErrorCode > 0)
                {
                    downloadFile.ErrorCode = downloadRes.ErrorCode;
                    downloadFile.ErrorDetail = downloadRes.ErrorDetail;
                    return downloadFile;
                }

                using (MemoryStream memstream = new MemoryStream())
                {
                    const int bufferLen = 10 * 1024 * 1024;
                    byte[] buffer = new byte[bufferLen];
                    int count = 0;
                    while ((count = downloadRes.Stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memstream.Write(buffer, 0, count);
                    }
                    downloadFile.FileValue = Convert.ToBase64String(memstream.ToArray());
                }
                downloadFile.ErrorCode = 0;
                return downloadFile;
            }
            catch (Exception ex)
            {
                _log.Debug($"DownloadFileBase64 Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 文件下载（返回路径）
        /// </summary>
        /// <returns>文件路径</returns>
        public DownloadFileRes FileDownload(OpenFileModel model, string savePath)
        {
            DownloadFileRes downloadFile = new DownloadFileRes();
            try
            {
                var downloadRes = GetDownloadFileStream(model);
                if (downloadRes.ErrorCode > 0)
                {
                    downloadFile.ErrorCode = downloadRes.ErrorCode;
                    downloadFile.ErrorDetail = downloadRes.ErrorDetail;
                    return downloadFile;
                }

                if (!string.IsNullOrEmpty(model.Rev))
                {
                    savePath = $"{savePath}\\{model.Rev}";
                }
                //如果不存在，创建它
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                // 文件全路径
                string filePath = $"{savePath}\\{downloadRes.FileName}";

                //写入数据
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    const int bufferLen = 10 * 1024 * 1024;

                    byte[] buffer = new byte[bufferLen];
                    int count = 0;
                    while ((count = downloadRes.Stream.Read(buffer, 0, bufferLen)) > 0)
                    {
                        fs.Write(buffer, 0, count);
                    }
                    fs.Dispose();
                }
                downloadFile.FileValue = filePath;
                downloadFile.ErrorCode = 0;
                return downloadFile;
            }
            catch (Exception ex)
            {
                _log.Debug($"FileDownload Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// GetDownloadFileStream
        /// </summary>
        /// <param name="model">OpenFileModel</param>
        /// <param name="fileName">fileName</param>
        /// <returns>FileStream</returns>
        private DownloadRes GetDownloadFileStream(OpenFileModel model)
        {
            DownloadRes downloadRes = new DownloadRes();
            string resStr = string.Empty;
            var handler = new WebRequestHandler();
            handler.ServerCertificateValidationCallback = delegate { return true; };
            using (var httpClient = new HttpClient(handler))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", model.TokenId);

                var json = "{\"docid\":\"" + model.FileId + "\"}";
                if (!string.IsNullOrEmpty(model.Rev))
                {
                    json = "{\"docid\":\"" + model.FileId + "\",\"rev\":\"" + model.Rev + "\"}";
                }
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/file/osdownload", content).Result;
                resStr = res.Content.ReadAsStringAsync().Result;


                int resCode = (int)res.StatusCode;
                // 若为错误返回码则抛出异常
                if (resCode < 200 || resCode >= 300)
                {
                    var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                    _log.Debug($"GetDownloadFileStream Exception: {errModel.cause}, Code: {errModel.code}");
                    downloadRes.ErrorCode = errModel.code;
                    downloadRes.ErrorDetail = JsonConvert.SerializeObject(errModel.detail);
                    return downloadRes;
                }
            }

            var dynamicObj = JsonConvert.DeserializeObject<dynamic>(resStr);
            string method = dynamicObj["authrequest"][0].Value;
            string url = dynamicObj["authrequest"][1].Value;
            downloadRes.FileName = dynamicObj["name"].Value;

            List<string> headers = new List<string>();
            for (int i = 2; i < dynamicObj["authrequest"].Count; ++i)
            {
                headers.Add(dynamicObj["authrequest"][i].Value);
            }

            OSSAPIHelper ossHttpHelper = new OSSAPIHelper();
            HttpWebResponse ossResult = ossHttpHelper.SendReqToOSS(method, url, headers, null);
            downloadRes.Stream = ossResult.GetResponseStream();

            int oosCode = (int)ossResult.StatusCode;
            // 若为错误返回码则抛出异常
            if (oosCode < 200 || oosCode >= 300)
            {
                string errBody = string.Empty;
                using (StreamReader reader = new StreamReader(downloadRes.Stream, Encoding.UTF8))
                {
                    errBody = reader.ReadToEnd();
                }
                var errModel = JsonConvert.DeserializeObject<ErrorModel>(errBody);
                _log.Debug($"UploadFile SendReqToOSS Exception: {errModel.cause}");
                downloadRes.ErrorCode = errModel.code;
                downloadRes.ErrorDetail = JsonConvert.SerializeObject(errModel.detail);
            }
            return downloadRes;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="model">SaveFileModel</param>
        public UploadFileRes UploadFile(SaveFileModel model)
        {
            UploadFileRes uploadFileRes = new UploadFileRes();
            try
            {
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", model.TokenId);

                    // 文件body
                    byte[] body = model.Base64Str;
                    var json = "{\"docid\":\"" + model.Docid + "\",\"name\":\"" + model.FileName.Trim() + "\",\"length\":" + body.Length + ",\"ondup\":" + model.Ondup + "}";

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/file/osbeginupload", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"UploadFile Osbeginupload Exception: {errModel.cause}, Code: {errModel.code}");
                        uploadFileRes.ErrorDetail = JsonConvert.SerializeObject(errModel.detail);
                        uploadFileRes.ErrorCode = errModel.code;
                        return uploadFileRes;
                    }

                    var dynamicObj = JsonConvert.DeserializeObject<dynamic>(resStr);
                    string method = dynamicObj["authrequest"][0].Value;
                    string url = dynamicObj["authrequest"][1].Value;
                    string rev = dynamicObj["rev"].Value;
                    string docid = dynamicObj["docid"].Value;
                    uploadFileRes.FileId = docid;
                    uploadFileRes.FileName = model.FileName.Trim();

                    List<string> headers = new List<string>();
                    for (int i = 2; i < dynamicObj["authrequest"].Count; ++i)
                    {
                        headers.Add(dynamicObj["authrequest"][i].Value);
                    }

                    OSSAPIHelper ossHttpHelper = new OSSAPIHelper();
                    HttpWebResponse ossResult = ossHttpHelper.SendReqToOSS(method, url, headers, body);
                    int oosCode = (int)ossResult.StatusCode;

                    // 若为错误返回码则抛出异常
                    if (oosCode < 200 || oosCode >= 300)
                    {
                        Stream resStream = ossResult.GetResponseStream();
                        string errBody = string.Empty;
                        using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
                        {
                            errBody = reader.ReadToEnd();
                        }
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(errBody);
                        _log.Debug($"UploadFile SendReqToOSS Exception: {errModel.cause}");
                        uploadFileRes.ErrorDetail = JsonConvert.SerializeObject(errModel.detail);
                        uploadFileRes.ErrorCode = errModel.code;
                        ossResult.Close();
                        return uploadFileRes;
                    }
                    ossResult.Close();

                    string sendJson = "{\"docid\":\"" + docid + "\",\"rev\":\"" + rev + "\"}";
                    var sendContent = new StringContent(sendJson, Encoding.UTF8, "application/json");
                    var sendRes = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/file/osendupload", sendContent).Result;
                    var sendResStr = sendRes.Content.ReadAsStringAsync().Result;

                    int sendCode = (int)sendRes.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (sendCode < 200 || sendCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(sendResStr);
                        _log.Debug($"UploadFile Oosendupload Exception: {errModel.cause}");
                        uploadFileRes.ErrorDetail = JsonConvert.SerializeObject(errModel.detail);
                        uploadFileRes.ErrorCode = errModel.code;
                        return uploadFileRes;
                    }
                }
                return uploadFileRes;
            }
            catch (Exception ex)
            {
                _log.Debug($"UploadFile Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 大文件上传开始
        /// </summary>
        /// <param name="model">SaveBigFileInitModel</param>
        public UploadFileRes UploadBigFileInit(SaveBigFileInitModel model)
        {
            UploadFileRes uploadFileRes = new UploadFileRes();
            try
            {
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", model.TokenId);

                    // 文件body
                    var json = "{\"docid\":\"" + model.Docid + "\",\"name\":\"" + model.FileName + "\",\"length\":" + model.Length + ",\"ondup\":" + model.Ondup + "}";

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/file/osinitmultiupload", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"UploadBigFile Osinitmultiupload Exception: {errModel.cause}, Code: {errModel.code}");
                        uploadFileRes.ErrorDetail = JsonConvert.SerializeObject(errModel.detail);
                        uploadFileRes.ErrorCode = errModel.code;
                        return uploadFileRes;
                    }
                    var uModel = JsonConvert.DeserializeObject<BeginUploadModel>(resStr);
                    uploadFileRes.FileName = JsonConvert.SerializeObject(uModel);
                }
                return uploadFileRes;
            }
            catch (Exception ex)
            {
                _log.Debug($"UploadBigFile Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 大文件上传
        /// </summary>
        /// <param name="model">SaveBigFileModel</param>
        public UploadFileRes UploadBigFile(SaveBigFileModel model)
        {
            UploadFileRes uploadFileRes = new UploadFileRes();
            try
            {
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", model.TokenId);

                    byte[] buf = model.FileBytes;
                    int writeSize = buf.Length; // 记录当前块写入的字节
                    Dictionary<string, List<object>> partInfo = new Dictionary<string, List<object>>();
                    if (!string.IsNullOrEmpty(model.PartsInfo))
                    {
                        partInfo = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(model.PartsInfo);
                    }

                    // 文件body
                    var json1 = "{\"docid\":\"" + model.Docid + "\",\"rev\":\"" + model.Rev + "\",\"uploadid\":\"" + model.UploadId + "\",\"parts\":\"" + model.PartIndex + "\"}";

                    var content1 = new StringContent(json1, Encoding.UTF8, "application/json");
                    var res1 = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/file/osuploadpart", content1).Result;
                    var resStr1 = res1.Content.ReadAsStringAsync().Result;
                    int resCode1 = (int)res1.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode1 < 200 || resCode1 >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr1);
                        _log.Debug($"UploadBigFile Osuploadpart Exception: {errModel.cause}, Code: {errModel.code}");
                        uploadFileRes.ErrorDetail = JsonConvert.SerializeObject(errModel.detail);
                        uploadFileRes.ErrorCode = errModel.code;
                        return uploadFileRes;
                    }

                    var jResult = JsonConvert.DeserializeObject<AuthRequestsModel>(resStr1);
                    var results = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jResult.authrequests.ToString());

                    List<string> headers = new List<string>();
                    List<string> authRequestList = new List<string>();
                    results.TryGetValue(model.PartIndex.ToString(), out authRequestList);
                    for (int i = 2; i < authRequestList.Count; ++i)
                    {
                        string header = authRequestList[i];
                        headers.Add(header);
                    }

                    string etag = null;
                    OSSAPIHelper ossHttpHelper = new OSSAPIHelper();

                    // 文件块上传
                    using (HttpWebResponse ossResult = ossHttpHelper.SendReqToOSS(authRequestList[0], authRequestList[1], headers, buf))
                    {
                        // 获取etag,由于报头中"etag"可能为"Etag","ETag","ETAG"等情况，故这里对报头key值进行遍历，将key值变为大写后与"ETAG"进行比较，若相等则让etag等于其value，退出循环。
                        WebHeaderCollection headerArray = ossResult.Headers;
                        for (int i = 0; i < headerArray.Count; ++i)
                        {
                            string key = headerArray.GetKey(i);
                            if (key.ToUpper().Equals("ETAG"))
                            {
                                etag = headerArray[key];
                                i = headerArray.Count;
                            }
                        }
                    }

                    List<object> tempList = new List<object>();
                    tempList.Add(etag);
                    tempList.Add(writeSize);
                    partInfo.Add(model.PartIndex.ToString(), tempList);

                    uploadFileRes.FileName = JsonConvert.SerializeObject(partInfo);
                    return uploadFileRes;
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"UploadBigFile Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 大文件上传发送
        /// </summary>
        /// <param name="model">SaveBigFileModel</param>
        public UploadFileRes UploadBigFileSend(SaveBigFileSendModel model)
        {
            UploadFileRes uploadFileRes = new UploadFileRes();
            try
            {
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", model.TokenId);

                    Dictionary<string, List<object>> partInfo = new Dictionary<string, List<object>>();
                    if (!string.IsNullOrEmpty(model.PartsInfo))
                    {
                        partInfo = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(model.PartsInfo);
                    }
                    OSSAPIHelper ossHttpHelper = new OSSAPIHelper();

                    var json2 = "{\"docid\":\"" + model.Docid + "\",\"rev\":\"" + model.Rev + "\",\"uploadid\":\"" + model.UploadId + "\",\"partinfo\":" + JsonConvert.SerializeObject(partInfo) + "}";
                    var content2 = new StringContent(json2, Encoding.UTF8, "application/json");
                    var res2 = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/file/oscompleteupload", content2).Result;
                    var resStr2 = res2.Content.ReadAsStringAsync().Result;

                    int resCode2 = (int)res2.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode2 < 200 || resCode2 >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr2);
                        _log.Debug($"UploadBigFileSend Oscompleteupload Exception: {errModel.cause}, Code: {errModel.code}");
                        uploadFileRes.ErrorDetail = JsonConvert.SerializeObject(errModel.detail);
                        uploadFileRes.ErrorCode = errModel.code;
                        return uploadFileRes;
                    }

                    var resultsJson = resStr2.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None);
                    var bodyJson = resultsJson[1].Split(new string[] { "--" }, StringSplitOptions.None).FirstOrDefault();
                    var headersJson = resultsJson[2].Split(new string[] { "--" }, StringSplitOptions.None).FirstOrDefault();

                    byte[] body = Encoding.UTF8.GetBytes(bodyJson);
                    JObject returnJson = (JObject)JsonConvert.DeserializeObject(headersJson);
                    JArray authRequest = (JArray)returnJson["authrequest"];
                    string method = (string)authRequest[0];
                    string url = (string)authRequest[1];
                    List<string> resHeaders = new List<string>();
                    for (int i = 2; i < authRequest.Count; i++)
                    {
                        resHeaders.Add((string)authRequest[i]);
                    }

                    var sendR = ossHttpHelper.SendReqToOSS(method, url, resHeaders, body);
                    sendR.Close();

                    string sendJson = "{\"docid\":\"" + model.Docid + "\",\"rev\":\"" + model.Rev + "\"}";
                    var sendContent = new StringContent(sendJson, Encoding.UTF8, "application/json");
                    var sendRes = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/file/osendupload", sendContent).Result;
                    var sendResStr = sendRes.Content.ReadAsStringAsync().Result;

                    int sendResCode = (int)sendRes.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (sendResCode < 200 || sendResCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(sendResStr);
                        _log.Debug($"UploadBigFileSend Osendupload Exception: {errModel.cause}, Code: {errModel.code}");
                        uploadFileRes.ErrorDetail = JsonConvert.SerializeObject(errModel.detail);
                        uploadFileRes.ErrorCode = errModel.code;
                        return uploadFileRes;
                    }
                    uploadFileRes.FileName = model.FileName;
                    uploadFileRes.FileId = model.Docid;
                }
                return uploadFileRes;
            }
            catch (Exception ex)
            {
                _log.Debug($"UploadBigFileSend Exception: {ex.Message}");
                throw ex;
            }
        }


        /// <summary>
        /// Search
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="model">SearchModel</param>
        /// <returns>Result</returns>
        public List<SearchDoc> Search(string token, SearchModel model, out long errorCode)
        {
            try
            {
                List<SearchDoc> searchDocs = new List<SearchDoc>();
                errorCode = 0;
                string resStr = string.Empty;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/ecosearch/v1/search", content).Result;
                    resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"Search Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        return null;
                    }
                }
                var serachResults = JsonConvert.DeserializeObject<SearchResult>(resStr);
                if (serachResults != null && serachResults.response != null
                    && serachResults.response.docs != null && serachResults.response.docs.Count > 0)
                {
                    var docs = serachResults.response.docs;
                    foreach (var doc in docs)
                    {
                        var docPro = doc._source;
                        var docHL = doc.highlight;
                        SearchDoc sDoc = new SearchDoc
                        {
                            hlbasename = string.IsNullOrEmpty(docHL.basename.FirstOrDefault()) ? docPro.basename : docHL.basename.FirstOrDefault(),
                            basename = docPro.basename,
                            docid = docPro.docid,
                            csflevel = docPro.csflevel,
                            editor = docPro.editor,
                            ext = docPro.ext,
                            modified = docPro.modified,
                            parentpath = "AnyShare://" + docPro.parentpath,
                            size = docPro.size,
                            tags = docPro.tags,
                            distance = docPro.distance,
                            summary = docPro.summary,
                            created = docPro.created,
                            creator = docPro.creator
                        };
                        searchDocs.Add(sDoc);
                    }
                }
                return searchDocs;
            }
            catch (Exception ex)
            {
                _log.Debug($"Search Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// GetFilePath
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="gnsId">gnsId</param>
        /// <returns>FilePath</returns>
        public string GetFilePath(string token, string gnsId, out long errorCode)
        {
            try
            {
                errorCode = 0;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);
                    var json = "{\"docid\":\"" + gnsId + "\"}";
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/file/convertpath", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"GetFilePath Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        return null;
                    }

                    return JsonConvert.DeserializeObject<PathModel>(resStr)?.namepath;
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"GetFilePath Exception: {ex.Message}");
                throw ex;
            }
        }

    }
}