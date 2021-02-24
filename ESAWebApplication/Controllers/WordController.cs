using ESAOfficePlugInsWeb.Models;
using ESAOfficePlugInsWeb.Utils;
using ESAWebApplication.Models;
using ESAWebApplication.Utils;
using ESAWebApplication.Utils.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ESAOfficePlugInWeb.Controllers
{
    public class WordController : Controller
    {
        /// <summary>
        /// log4net
        /// </summary>
        log4net.ILog _log = log4net.LogManager.GetLogger("WordController");

        /// <summary>
        /// Home
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// Settings
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            ViewBag.HelpUrl = Constant.HelpUrl;
            ViewBag.VersionInfo = Constant.VersionInfo;
            ViewBag.PublishDate = Constant.PublishDate;
            return View();
        }

        /// <summary>
        /// OpenFile
        /// </summary>
        /// <returns>View</returns>
        public ActionResult OpenFile()
        {
            return View();
        }

        /// <summary>
        /// SaveFile
        /// </summary>
        /// <returns>View</returns>
        public ActionResult SaveFile()
        {
            return View();
        }

        /// <summary>
        /// CompareFile
        /// </summary>
        /// <returns>View</returns>
        public ActionResult CompareFile()
        {
            return View();
        }

        /// <summary>
        /// GetLoginUser
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>LoginUser</returns>
        [HttpPost]
        public JsonResult GetLoginUser(string token)
        {
            try
            {
                token = CommonHelper.GetToken(token);
                IAS7APIHelper helper = new AS7APIHelper();
                long errorCode = 0;
                var user = helper.GetLoginUser(token, out errorCode);
                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }
                return Json(new JsonModel { Success = true, StatusCode = errorCode, Data = JsonConvert.SerializeObject(user) });
            }
            catch (Exception ex)
            {
                _log.Debug($"GetLoginUser Exception: {ex.Message}");
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// SaveFileToServer
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult SaveFileToServer()
        {
            try
            {
                _log.Debug("SaveFileToServer Start");

                SaveFileModel model = new SaveFileModel()
                {
                    Base64Str = CommonHelper.FromBase64String(Request.Form["Base64Str"]),
                    FileName = CommonHelper.DecodeUrl(Request.Form["FileName"]),
                    Docid = CommonHelper.DecodeUrl(Request.Form["Docid"]),
                    TokenId = CommonHelper.GetToken(Request.Form["TokenId"]),
                    Ondup = Convert.ToInt64(Request.Form["Ondup"])
                };
                _log.Debug($"SaveFileToServer: {model.FileName}");

                IAS7APIHelper helper = new AS7APIHelper();
                var uploadFileRes = helper.UploadFile(model);
                if (uploadFileRes.ErrorCode == 403002039)
                {
                    uploadFileRes.FileName = helper.GetSuggestFileName(model.TokenId, model.Docid, model.FileName);
                }

                _log.Debug("SaveFileToServer End");
                return Json(new JsonModel { Success = true, StatusCode = uploadFileRes.ErrorCode, Data = JsonConvert.SerializeObject(uploadFileRes) });
            }
            catch (Exception ex)
            {
                _log.Debug($"SaveFileToServer Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// SaveBigFileInit
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult SaveBigFileInit()
        {
            try
            {
                _log.Debug("SaveBigFileInit Start");
                SaveBigFileInitModel model = new SaveBigFileInitModel()
                {
                    FileName = CommonHelper.DecodeUrl(Request.Form["FileName"]),
                    Docid = CommonHelper.DecodeUrl(Request.Form["Docid"]),
                    TokenId = CommonHelper.GetToken(Request.Form["TokenId"]),
                    Ondup = Convert.ToInt64(Request.Form["Ondup"]),
                    Length = Convert.ToInt64(Request.Form["FileLength"])
                };
                _log.Debug($"SaveBigFileInit: {model.FileName}");

                IAS7APIHelper helper = new AS7APIHelper();
                var uploadFileRes = helper.UploadBigFileInit(model);
                if (uploadFileRes.ErrorCode == 403002039)
                {
                    uploadFileRes.FileName = helper.GetSuggestFileName(model.TokenId, model.Docid, model.FileName);
                }

                _log.Debug("SaveBigFileInit End");
                return Json(new JsonModel { Success = true, StatusCode = uploadFileRes.ErrorCode, Data = JsonConvert.SerializeObject(uploadFileRes) });
            }
            catch (Exception ex)
            {
                _log.Debug($"SaveBigFileToServer Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// SaveBigFileToServer
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult SaveBigFileToServer()
        {
            try
            {
                long start = Convert.ToInt64(Request.Form["Start"]);
                long end = Convert.ToInt64(Request.Form["End"]);
                var fileName = CommonHelper.DecodeUrl(Request.Form["FileName"]);
                _log.Debug($"SaveBigFileToServer Save Blob {start} Of {end}, FileName: {fileName}");

                SaveBigFileModel model = new SaveBigFileModel()
                {
                    FileName = fileName,
                    Docid = CommonHelper.DecodeUrl(Request.Form["Docid"]),
                    TokenId = CommonHelper.GetToken(Request.Form["TokenId"]),
                    Rev = CommonHelper.DecodeUrl(Request.Form["Rev"]),
                    UploadId = CommonHelper.DecodeUrl(Request.Form["UploadId"]),
                    FileBytes = CommonHelper.FromBase64String(Request.Form["Base64Str"]),
                    PartIndex = start,
                    TotalParts = end,
                    PartsInfo = CommonHelper.DecodeUrl(Request.Form["PartsInfo"])
                };

                IAS7APIHelper helper = new AS7APIHelper();
                var uploadFileRes = helper.UploadBigFile(model);
                return Json(new JsonModel { Success = true, StatusCode = uploadFileRes.ErrorCode, Data = JsonConvert.SerializeObject(uploadFileRes) });
            }
            catch (Exception ex)
            {
                _log.Debug($"SaveBigFileToServer Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// SaveBigFileSend
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult SaveBigFileSend()
        {
            try
            {
                _log.Debug("SaveBigFileSend Start");
                SaveBigFileSendModel model = new SaveBigFileSendModel()
                {
                    FileName = CommonHelper.DecodeUrl(Request.Form["FileName"]),
                    Docid = CommonHelper.DecodeUrl(Request.Form["Docid"]),
                    TokenId = CommonHelper.GetToken(Request.Form["TokenId"]),
                    Rev = CommonHelper.DecodeUrl(Request.Form["Rev"]),
                    UploadId = CommonHelper.DecodeUrl(Request.Form["UploadId"]),
                    PartsInfo = CommonHelper.DecodeUrl(Request.Form["PartsInfo"])
                };
                _log.Debug($"SaveBigFileSend: {model.FileName}");

                IAS7APIHelper helper = new AS7APIHelper();
                var uploadFileRes = helper.UploadBigFileSend(model);

                _log.Debug("SaveBigFileSend End");
                return Json(new JsonModel { Success = true, StatusCode = uploadFileRes.ErrorCode, Data = JsonConvert.SerializeObject(uploadFileRes) });
            }
            catch (Exception ex)
            {
                _log.Debug($"SaveBigFileSend Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// OpenFileFromServer
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public ContentResult OpenFileFromServer(OpenFileModel model)
        {
            string savePath = string.Empty;
            try
            {
                _log.Debug("OpenFileFromServer Start");
                model.TokenId = CommonHelper.GetToken(model.TokenId);

                IAS7APIHelper helper = new AS7APIHelper();

                // 文件临时路径
                savePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\App_Data\\{Guid.NewGuid()}";

                // 文件下载
                var downloadFile = helper.FileDownload(model, savePath);
                if (downloadFile.ErrorCode > 0)
                {
                    return new ContentResult
                    {
                        Content = JsonConvert.SerializeObject(new JsonModel { Success = true, StatusCode = downloadFile.ErrorCode, Message = downloadFile.ErrorDetail }),
                        ContentType = "application/json"
                    };
                }
                // 文件下载路径
                string downloadFilePath = downloadFile.FileValue;

                // 设置文件标识
                if (string.Equals("word", model.DocType, StringComparison.CurrentCultureIgnoreCase))
                {
                    OfficeHelper.SetWordFileID(ref downloadFilePath, model.FileId);
                }
                else if (string.Equals("excel", model.DocType, StringComparison.CurrentCultureIgnoreCase))
                {
                    OfficeHelper.SetExcelFileID(downloadFilePath, model.FileId);
                }
                else if (string.Equals("ppt", model.DocType, StringComparison.CurrentCultureIgnoreCase))
                {
                    //OfficeHelper.SetPPTFileID(downloadFilePath, model.FileId);
                }

                // 读取文件流
                string base64String = CommonHelper.GetBase64FormPath(downloadFilePath);
                _log.Debug("OpenFileFromServer End");

                return new ContentResult
                {
                    Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new JsonModel { Success = true, Data = base64String }),
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                _log.Debug($"OpenFileFromServer Exception: {ex.Message}");
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new JsonModel { Success = false, Message = ex.Message }),
                    ContentType = "application/json"
                };
            }
            finally
            {
                DeletePath(savePath);
            }
        }

        /// <summary>
        /// CompareFileTemp
        /// </summary>
        /// <returns>Result</returns>
        public JsonResult CompareFileTemp()
        {
            string savePath = string.Empty;
            try
            {
                long start = Convert.ToInt64(Request.Form["Start"]);
                long end = Convert.ToInt64(Request.Form["End"]);
                var guid = Request.Form["Guid"];
                var fileName = $"{guid}.docx";

                _log.Debug($"CompareFileTemp Save Blob {start} Of {end}, FileName: {fileName}");

                //文件临时路径
                savePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\App_Data\\{guid}";
                //如果不存在，创建它
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                var filePath = Path.Combine(savePath, fileName);
                var fileBytes = CommonHelper.FromBase64String(Request.Form["Base64Str"]);
                //对文件流进行存储
                using (FileStream fs = new FileStream(filePath, start == 1 ? FileMode.Create : FileMode.Append))
                {
                    fs.Write(fileBytes, 0, fileBytes.Length);
                }
                return Json(new JsonModel { Success = true });
            }
            catch (Exception ex)
            {
                DeletePath(savePath);
                _log.Debug($"CompareFileTemp Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// CompareFileFromServer
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult CompareFileFromServer()
        {
            // 文件临时路径
            string savePath = string.Empty;
            try
            {
                _log.Debug("CompareFileFromServer Start");

                var guid = Request.Form["Guid"];
                // 文件路径
                savePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\App_Data\\{guid}";

                // 本地文件
                var localFilePath = Path.Combine(savePath, $"{guid}.docx");

                // 下载指定版本文件
                OpenFileModel downloadReq = new OpenFileModel()
                {
                    TokenId = CommonHelper.GetToken(Request.Form["TokenId"]),
                    FileId = CommonHelper.DecodeUrl(Request.Form["Docid"]),
                    Rev = CommonHelper.DecodeUrl(Request.Form["Rev"])
                };
                IAS7APIHelper helper = new AS7APIHelper();
                var downloadFile = helper.FileDownload(downloadReq, savePath);
                if (downloadFile.ErrorCode > 0)
                {
                    return new ContentResult
                    {
                        Content = JsonConvert.SerializeObject(new JsonModel { Success = true, StatusCode = downloadFile.ErrorCode, Message = downloadFile.ErrorDetail }),
                        ContentType = "application/json"
                    };
                }

                // 对比文件路径
                var cFilePath = OfficeHelper.CompareFile(downloadFile.FileValue, localFilePath, savePath);

                // 获取对比文件Base64 返回前台进行显示
                string base64String = CommonHelper.GetBase64FormPath(cFilePath);
                _log.Debug("CompareFileFromServer End");

                return new ContentResult
                {
                    Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new JsonModel { Success = true, Data = base64String }),
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                _log.Debug($"CompareFileFromServer Exception: {ex.Message}");
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new JsonModel { Success = false, Message = ex.Message }),
                    ContentType = "application/json"
                };
            }
            finally
            {
                DeletePath(savePath);
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="savePath">文件夹</param>
        private void DeletePath(string savePath)
        {
            try
            {
                if (Directory.Exists(savePath))
                {
                    Directory.Delete(savePath, true);
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"DeletePath Exception: {ex.Message}");
            }
        }


        /// <summary>
        /// GetEntryDocLibs
        /// </summary>
        /// <returns>EntryDocLibs</returns>
        [HttpPost]
        public JsonResult GetEntryDocLibs(string token, string type)
        {
            try
            {
                token = CommonHelper.GetToken(token);

                IAS7APIHelper helper = new AS7APIHelper();
                long errorCode = 0;
                var libs = helper.GetEntryDocLibs(token, type, out errorCode);

                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }
                string libsJson = JsonConvert.SerializeObject(libs);

                return Json(new JsonModel { Success = true, Data = libsJson, StatusCode = errorCode });
            }
            catch (Exception ex)
            {
                _log.Debug($"GetEntryDocLibs Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// GetDocLibsById
        /// </summary>
        /// <returns>DocLibs</returns>
        [HttpPost]
        public JsonResult GetDocLibsById(string token, string gnsId, string officeType = "")
        {
            try
            {
                token = CommonHelper.GetToken(token);

                IAS7APIHelper helper = new AS7APIHelper();
                long errorCode = 0;
                var libs = helper.GetDocLibsById(token, gnsId, out errorCode);

                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }

                // 筛选文件类型
                //if (libs.files != null && libs.files.Length > 0)
                //{
                //    if (string.Equals("word", officeType, StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        libs.files = libs.files.Where(T => ".docx".Equals(Path.GetExtension(T.name), StringComparison.CurrentCultureIgnoreCase)).ToArray();
                //    }
                //    else if (string.Equals("excel", officeType, StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        libs.files = libs.files.Where(T => ".xlsx".Equals(Path.GetExtension(T.name), StringComparison.CurrentCultureIgnoreCase)).ToArray();
                //    }
                //    else if (string.Equals("ppt", officeType, StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        libs.files = libs.files.Where(T => ".pptx".Equals(Path.GetExtension(T.name), StringComparison.CurrentCultureIgnoreCase)).ToArray();
                //    }
                //}

                string libsJson = JsonConvert.SerializeObject(libs);
                return Json(new JsonModel { Success = true, Data = libsJson, StatusCode = errorCode });
            }
            catch (Exception ex)
            {
                _log.Debug($"GetDocLibsById Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// CreateDir
        /// </summary>
        /// <returns>Dir</returns>
        [HttpPost]
        public JsonResult CreateDir(string token, string gnsId, string name)
        {
            try
            {
                token = CommonHelper.GetToken(token);

                _log.Debug("CreateDir Start");
                IAS7APIHelper helper = new AS7APIHelper();
                long code = helper.CreateDir(token, gnsId, name.Trim());
                _log.Debug("CreateDir End");
                return Json(new JsonModel { Success = true, StatusCode = code });
            }
            catch (Exception ex)
            {
                _log.Debug($"CreateDir Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <returns>Search</returns>
        [HttpPost]
        public JsonResult Search(string token, string gnsId, string key, int doctype, string officeType = "")
        {
            try
            {
                token = CommonHelper.GetToken(token);

                List<string> rangeList = new List<string>();
                if (!string.IsNullOrEmpty(gnsId))
                {
                    rangeList.Add(gnsId);
                }

                IAS7APIHelper helper = new AS7APIHelper();
                SearchModel model = new SearchModel()
                {
                    start = 0,
                    range = rangeList,
                    keys = key,
                    hl = true,
                    keysfields = new List<string>() { "basename" },
                    doctype = doctype,
                    rows = 1000
                };
                long errorCode = 0;
                var searchDocs = helper.Search(token, model, out errorCode);
                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }

                // 筛选文件类型
                if (searchDocs != null && searchDocs.Count > 0)
                {
                    if (string.Equals("word", officeType, StringComparison.CurrentCultureIgnoreCase))
                    {
                        searchDocs = searchDocs.Where(T => ".docx".Equals(Path.GetExtension(T.ext), StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(T.ext)).ToList();
                    }
                    else if (string.Equals("excel", officeType, StringComparison.CurrentCultureIgnoreCase))
                    {
                        searchDocs = searchDocs.Where(T => ".xlsx".Equals(Path.GetExtension(T.ext), StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(T.ext)).ToList();
                    }
                    else if (string.Equals("ppt", officeType, StringComparison.CurrentCultureIgnoreCase))
                    {
                        searchDocs = searchDocs.Where(T => ".pptx".Equals(Path.GetExtension(T.ext), StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(T.ext)).ToList();
                    }
                    searchDocs = searchDocs.OrderBy(T => T.size).ThenBy(T => T.basename).ToList();
                }

                var dataJson = JsonConvert.SerializeObject(searchDocs);

                return new JsonResult() { Data = new JsonModel { Success = true, Data = dataJson, StatusCode = errorCode }, MaxJsonLength = int.MaxValue };
            }
            catch (Exception ex)
            {
                _log.Debug($"Search Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// GetFileRevisions
        /// </summary>
        /// <returns>FileRevisions</returns>
        [HttpPost]
        public JsonResult GetFileRevisions()
        {
            try
            {
                var token = CommonHelper.GetToken(Request.Form["TokenId"]);
                var gnsId = CommonHelper.DecodeUrl(Request.Form["Docid"]);

                IAS7APIHelper helper = new AS7APIHelper();
                long errorCode = 0;
                var resItems = helper.GetFileVersions(token, gnsId, out errorCode);

                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }

                List<FileVersionModel> fileVersions = new List<FileVersionModel>();
                foreach (var res in resItems)
                {
                    fileVersions.Add(new FileVersionModel()
                    {
                        Rev = res.rev,
                        Name = res.name,
                        Size = CommonHelper.StampToSize(res.size),
                        Editor = res.editor,
                        Modified = CommonHelper.StampToDateTime(res.modified)
                    });
                }
                string fileVersionsJson = JsonConvert.SerializeObject(fileVersions);
                return Json(new JsonModel { Success = true, Data = fileVersionsJson, StatusCode = errorCode });
            }
            catch (Exception ex)
            {
                _log.Debug($"GetFileRevisions Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// GetFilePath
        /// </summary>
        /// <returns>FilePath</returns>
        [HttpPost]
        public JsonResult GetFilePath()
        {
            try
            {
                var token = CommonHelper.GetToken(Request.Form["TokenId"]);
                var gnsId = CommonHelper.DecodeUrl(Request.Form["Docid"]);

                IAS7APIHelper helper = new AS7APIHelper();
                long errorCode = 0;
                var filePath = helper.GetFilePath(token, gnsId, out errorCode);

                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }
                return Json(new JsonModel { Success = true, Data = filePath, StatusCode = errorCode });
            }
            catch (Exception ex)
            {
                _log.Debug($"GetFilePath Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

    }
}