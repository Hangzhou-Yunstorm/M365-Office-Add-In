using ESAOfficePlugInsWeb.Models;
using ESAOfficePlugInsWeb.Utils;
using ESAWebApplication.Models;
using ESAWebApplication.Utils;
using ESAWebApplication.Utils.Services;
using Microsoft.Exchange.WebServices.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ESAOfficePlugInWeb.Controllers
{
    public class OutlookController : Controller
    {
        /// <summary>
        /// log4net
        /// </summary>
        log4net.ILog _log = log4net.LogManager.GetLogger("OutlookController");

        /// <summary>
        ///  Settings
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
        /// SaveAttachments
        /// </summary>
        /// <returns>View</returns>
        public ActionResult SaveAttachments()
        {
            return View();
        }

        /// <summary>
        /// SaveEmail
        /// </summary>
        /// <returns>View</returns>
        public ActionResult SaveEmail()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddAttachments()
        {
            ViewBag.ASUrl = Constant.OAuth2Url;
            return View();
        }

        /// <summary>
        /// SaveEmailToServer
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult SaveEmailToServer(SaveEmailModel model)
        {
            try
            {
                _log.Debug("SaveEmailToServer Start");
                var fileName = CommonHelper.DecodeUrl(model.FileName);
                var tokenId = CommonHelper.GetToken(model.TokenId);
                var docId = CommonHelper.DecodeUrl(model.Docid);
                var ewsUrl = CommonHelper.DecodeUrl(model.EwsUrl);
                var ewsId = CommonHelper.DecodeUrl(model.EwsId);
                var ewsToken = CommonHelper.DecodeUrl(model.EwsToken);
                _log.Debug($"SaveEmailToServer: {fileName}");

                _log.Debug($"SaveEmailToServer ExchangeService Start,  EWSUrl: {ewsUrl}");
                ExchangeService service = new ExchangeService();
                service.Url = new Uri(ewsUrl);
                service.Credentials = new OAuthCredentials("Bearer " + ewsToken);
                service.Timeout = 360 * 1000;

                List<ItemId> itemIds = new List<ItemId>() { ewsId };
                var items = service.BindToItems(itemIds, new PropertySet());
                var itemMessage = items.FirstOrDefault().Item;

                itemMessage.Load(new PropertySet(ItemSchema.MimeContent));
                MimeContent mimconm = itemMessage.MimeContent;
                _log.Debug($"SaveEmailToServer ExchangeService End");

                _log.Debug($"SaveEmailToServer UploadFile Start");
                SaveFileModel uploadModel = new SaveFileModel()
                {
                    Base64Str = mimconm.Content,
                    FileName = fileName,
                    Docid = docId,
                    TokenId = tokenId,
                    Ondup = model.Ondup
                };

                IAS7APIHelper helper = new AS7APIHelper();
                var uploadFileRes = helper.UploadFile(uploadModel);
                _log.Debug($"SaveEmailToServer UploadFile End");

                if (uploadFileRes.ErrorCode == 403002039)
                {
                    uploadFileRes.FileName = helper.GetSuggestFileName(tokenId, docId, fileName);
                }
                _log.Debug("SaveEmailToServer End");
                return Json(new JsonModel { Success = true, StatusCode = uploadFileRes.ErrorCode, Data = JsonConvert.SerializeObject(uploadFileRes) });
            }
            catch (Exception ex)
            {
                _log.Debug($"SaveEmailToServer Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// GetEmailAttachment
        /// </summary>
        /// <returns>Result</returns>
        private byte[] GetEmailAttachment()
        {
            byte[] emailAttach = null;
            try
            {
                _log.Debug("GetEmailAttachment Start");

                var attachId = CommonHelper.DecodeUrl(Request.Form["AttachmentId"]);
                if (!AttachmentHelper.TryGetAttachmentResult(attachId, out emailAttach))
                {
                    var ewsUrl = CommonHelper.DecodeUrl(Request.Form["EwsUrl"]);
                    var ewsId = CommonHelper.DecodeUrl(Request.Form["EwsId"]);
                    var ewsToken = CommonHelper.DecodeUrl(Request.Form["EwsToken"]);
                    _log.Debug($"GetEmailAttachment EWSUrl: {ewsUrl}");

                    ExchangeService service = new ExchangeService();
                    service.Url = new Uri(ewsUrl);
                    service.Credentials = new OAuthCredentials("Bearer " + ewsToken);
                    service.Timeout = 360 * 1000;

                    List<ItemId> itemIds = new List<ItemId>() { ewsId };
                    var items = service.BindToItems(itemIds, new PropertySet());
                    var itemMessage = items.FirstOrDefault().Item;

                    itemMessage.Load(new PropertySet(ItemSchema.Attachments));
                    if (itemMessage.HasAttachments)
                    {
                        var attchs = itemMessage.Attachments;
                        foreach (FileAttachment attachment in attchs)
                        {
                            attachment.Load();
                            if (attachId == attachment.Id)
                            {
                                emailAttach = attachment.Content;
                            }
                            AttachmentHelper.SetAttachmentResult(attachment.Id, attachment.Content);
                        }
                    }
                }
                _log.Debug("GetEmailAttachment End");
            }
            catch (Exception ex)
            {
                _log.Debug($"GetEmailAttachment Exception: {ex.Message}");
            }
            return emailAttach;
        }

        /// <summary>
        ///  SaveAttachmentToServer
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult SaveAttachmentToServer()
        {
            try
            {
                _log.Debug("SaveAttachmentToServer Start");

                var base64Str = GetEmailAttachment();
                if (base64Str != null)
                {
                    SaveFileModel model = new SaveFileModel()
                    {
                        Base64Str = base64Str,
                        FileName = CommonHelper.DecodeUrl(Request.Form["FileName"]),
                        Docid = CommonHelper.DecodeUrl(Request.Form["Docid"]),
                        TokenId = CommonHelper.GetToken(Request.Form["TokenId"]),
                        Ondup = Convert.ToInt64(Request.Form["Ondup"])
                    };

                    IAS7APIHelper helper = new AS7APIHelper();
                    var uploadFileRes = helper.UploadFile(model);
                    if (uploadFileRes.ErrorCode == 403002039)
                    {
                        uploadFileRes.FileName = helper.GetSuggestFileName(model.TokenId, model.Docid, model.FileName);
                    }

                    _log.Debug("SaveAttachmentToServer End");
                    return Json(new JsonModel { Success = true, StatusCode = uploadFileRes.ErrorCode, Data = JsonConvert.SerializeObject(uploadFileRes) });
                }
                else
                {
                    _log.Debug("SaveAttachmentToServer No Attachment Content");
                    return Json(new JsonModel { Success = false });
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"SaveAttachmentToServer Exception: {ex.Message}");

                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        ///  GetShareLinkConfig
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult GetShareLinkConfig()
        {
            try
            {
                var tokenId = CommonHelper.GetToken(Request.Form["TokenId"]);

                IShareLinkAPIHelper helper = new ShareLinkAPIHelper();
                long errorCode = 0;
                var model = helper.GetShareLinkConfig(tokenId, out errorCode);
                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }
                return Json(new JsonModel { Success = true, StatusCode = errorCode, Data = JsonConvert.SerializeObject(model) });

            }
            catch (Exception ex)
            {
                _log.Debug($"GetShareLinkConfig Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        ///  GetShareLinkSwitch
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult GetShareLinkSwitch()
        {
            try
            {
                var tokenId = CommonHelper.GetToken(Request.Form["TokenId"]);

                IShareLinkAPIHelper helper = new ShareLinkAPIHelper();
                long errorCode = 0;
                var model = helper.GetShareLinkSwitch(tokenId, out errorCode);
                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }
                return Json(new JsonModel { Success = true, StatusCode = errorCode, Data = JsonConvert.SerializeObject(model) });
            }
            catch (Exception ex)
            {
                _log.Debug($"GetShareLinkSwitch Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        ///  GetShareLinks
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult GetShareLinks()
        {
            try
            {
                var tokenId = CommonHelper.GetToken(Request.Form["TokenId"]);
                ShareLinkItem item = new ShareLinkItem()
                {
                    id = Request.Form["Docid"],
                    type = Request.Form["FileType"]
                };

                IShareLinkAPIHelper helper = new ShareLinkAPIHelper();
                long errorCode = 0;
                var model = helper.GetShareLink(tokenId, item, out errorCode);
                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }

                return Json(new JsonModel { Success = true, StatusCode = errorCode, Data = JsonConvert.SerializeObject(model) });
            }
            catch (Exception ex)
            {
                _log.Debug($"GetShareLinks Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        ///  CreateShareLink
        /// </summary>
        /// <returns>ShareLink Id</returns>
        [HttpPost]
        public JsonResult CreateShareLink()
        {
            try
            {
                _log.Debug("CreateShareLink Start");
                var tokenId = CommonHelper.GetToken(Request.Form["TokenId"]);
                CreatShareLinkModel item = new CreatShareLinkModel()
                {
                    item = new CItem()
                    {
                        id = CommonHelper.DecodeUrl(Request.Form["Docid"]),
                        type = Request.Form["FileType"],
                        perms = GetPerms(Convert.ToInt64(Request.Form["Perm"]))
                    },
                    expires_at = CommonHelper.DecodeUrl(Request.Form["ExpiresAt"]),
                    limited_times = string.IsNullOrEmpty(Request.Form["LimitedTimes"]) ? -1 : Convert.ToInt64(Request.Form["LimitedTimes"]),
                    password = string.IsNullOrEmpty(Request.Form["Password"]) ? "" : CommonHelper.DecodeUrl(Request.Form["Password"]),
                    title = CommonHelper.DecodeUrl(Request.Form["Title"])
                };

                IShareLinkAPIHelper helper = new ShareLinkAPIHelper();
                long errorCode = 0;
                var shareLinkId = helper.CreatShareLink(tokenId, item, out errorCode);
                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }

                _log.Debug("CreateShareLink End");
                return Json(new JsonModel { Success = true, StatusCode = errorCode, Data = shareLinkId });
            }
            catch (Exception ex)
            {
                _log.Debug($"CreateShareLink Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        ///  CreateShareLink
        /// </summary>
        /// <returns>ShareLink Id</returns>
        [HttpPost]
        public JsonResult UpdateShareLink()
        {
            try
            {
                _log.Debug("UpdateShareLink Start");
                var tokenId = CommonHelper.GetToken(Request.Form["TokenId"]);
                UpdateShareLinkModel item = new UpdateShareLinkModel()
                {
                    item = new UItem()
                    {
                        perms = GetPerms(Convert.ToInt64(Request.Form["Perm"]))
                    },
                    link_id = CommonHelper.DecodeUrl(Request.Form["LinkId"]),
                    expires_at = CommonHelper.DecodeUrl(Request.Form["ExpiresAt"]),
                    limited_times = string.IsNullOrEmpty(Request.Form["LimitedTimes"]) ? -1 : Convert.ToInt64(Request.Form["LimitedTimes"]),
                    password = string.IsNullOrEmpty(Request.Form["Password"]) ? "" : CommonHelper.DecodeUrl(Request.Form["Password"]),
                    title = CommonHelper.DecodeUrl(Request.Form["Title"])
                };

                IShareLinkAPIHelper helper = new ShareLinkAPIHelper();
                var errorCode = helper.UpdateShareLink(tokenId, item);
                _log.Debug("UpdateShareLink End");
                return Json(new JsonModel { Success = true, StatusCode = errorCode });
            }
            catch (Exception ex)
            {
                _log.Debug($"UpdateShareLink Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 获取权限集合
        /// </summary>
        /// <param name="perm">权限值</param>
        /// <returns>权限集合</returns>
        private List<string> GetPerms(long perm)
        {
            List<string> perms = null;
            switch (perm)
            {
                case 7:
                    perms = new List<string>() { "display", "read" };
                    break;
                case 25:
                    perms = new List<string>() { "display", "create", "modify" };
                    break;
                case 31:
                    perms = new List<string>() { "display", "read", "create", "modify" };
                    break;
                default:
                    perms = new List<string>();
                    break;
            }
            return perms;
        }

        /// <summary>
        ///  CreateShareLink
        /// </summary>
        /// <returns>ShareLink Id</returns>
        [HttpPost]
        public JsonResult RemoveShareLink()
        {
            try
            {
                _log.Debug("RemoveShareLink Start");
                var tokenId = CommonHelper.GetToken(Request.Form["TokenId"]);
                var link_id = CommonHelper.DecodeUrl(Request.Form["LinkId"]);

                IShareLinkAPIHelper helper = new ShareLinkAPIHelper();
                var errorCode = helper.DeleteShareLink(tokenId, link_id);
                _log.Debug("RemoveShareLink End");
                return Json(new JsonModel { Success = true, StatusCode = errorCode });
            }
            catch (Exception ex)
            {
                _log.Debug($"RemoveShareLink Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }


        /// <summary>
        ///  GetRealNameShareLink
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult GetRealNameShareLinkId()
        {
            try
            {
                _log.Debug("GetRealNameShareLink Start");
                var tokenId = CommonHelper.GetToken(Request.Form["TokenId"]);
                ShareLinkItem item = new ShareLinkItem()
                {
                    id = Request.Form["Docid"],
                    type = Request.Form["FileType"]
                };

                IShareLinkAPIHelper helper = new ShareLinkAPIHelper();
                long errorCode = 0;
                var realNameId = helper.GetRealNameShareLink(tokenId, item, out errorCode);
                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }
                if (string.IsNullOrEmpty(realNameId))
                {
                    item.id = CommonHelper.DecodeUrl(item.id);
                    CreatRealNameShareLinkModel model = new CreatRealNameShareLinkModel()
                    {
                        item = item
                    };
                    realNameId = helper.CreatRealNameShareLink(tokenId, model, out errorCode);
                    if (errorCode > 0)
                    {
                        return Json(new JsonModel { Success = true, StatusCode = errorCode });
                    }
                }

                _log.Debug("GetRealNameShareLink End");
                return Json(new JsonModel { Success = true, StatusCode = errorCode, Data = realNameId });
            }
            catch (Exception ex)
            {
                _log.Debug($"GetRealNameShareLink Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        ///  CheckOwner
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult CheckOwner()
        {
            try
            {
                var tokenId = CommonHelper.GetToken(Request.Form["TokenId"]);
                var gnsId = CommonHelper.DecodeUrl(Request.Form["Docid"]);

                IShareLinkAPIHelper helper = new ShareLinkAPIHelper();
                long errorCode = 0;
                var model = helper.CheckOwner(tokenId, gnsId, out errorCode);
                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }
                return Json(new JsonModel { Success = true, StatusCode = errorCode, Data = JsonConvert.SerializeObject(model) });

            }
            catch (Exception ex)
            {
                _log.Debug($"CheckOwner Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        ///  GetDirSize
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult GetDirSize()
        {
            try
            {
                var tokenId = CommonHelper.GetToken(Request.Form["TokenId"]);
                var gnsId = CommonHelper.DecodeUrl(Request.Form["Docid"]);

                IShareLinkAPIHelper helper = new ShareLinkAPIHelper();
                long errorCode = 0;
                var model = helper.GetDirSize(tokenId, gnsId, out errorCode);
                if (errorCode > 0)
                {
                    return Json(new JsonModel { Success = true, StatusCode = errorCode });
                }
                return Json(new JsonModel { Success = true, StatusCode = errorCode, Data = JsonConvert.SerializeObject(model) });
            }
            catch (Exception ex)
            {
                _log.Debug($"GetDirSize Exception: {ex.Message}");
                return Json(new JsonModel { Success = false, Message = ex.Message });
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
                    FileName = CommonHelper.DecodeUrl(Request.Form["FileName"]),
                    Docid = CommonHelper.DecodeUrl(Request.Form["Docid"]),
                    TokenId = CommonHelper.GetToken(Request.Form["TokenId"]),
                    Ondup = Convert.ToInt64(Request.Form["Ondup"])
                };
                _log.Debug($"SaveFileToServer: {model.FileName}");

                try
                {
                    var postFile = Request.Files[0];
                    var fileLength = postFile.ContentLength;
                    byte[] fileBytes = new byte[fileLength];
                    postFile.InputStream.Read(fileBytes, 0, fileLength);
                    model.Base64Str = fileBytes;
                }
                catch (Exception ex)
                {
                    _log.Debug($"SaveFileToServer Read Exception: {ex.Message}");
                    return Json(new JsonModel { Success = false, Message = ex.Message });
                }

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

                var fileBlob = Request.Files[0];
                var fileLength = fileBlob.ContentLength;
                byte[] fileBytes = new byte[fileLength];
                fileBlob.InputStream.Read(fileBytes, 0, fileBytes.Length);

                SaveBigFileModel model = new SaveBigFileModel()
                {
                    FileName = fileName,
                    Docid = CommonHelper.DecodeUrl(Request.Form["Docid"]),
                    TokenId = CommonHelper.GetToken(Request.Form["TokenId"]),
                    Rev = CommonHelper.DecodeUrl(Request.Form["Rev"]),
                    UploadId = CommonHelper.DecodeUrl(Request.Form["UploadId"]),
                    FileBytes = fileBytes,
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

    }
}