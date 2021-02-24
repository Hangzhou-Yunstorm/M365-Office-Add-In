using ESAOfficePlugInsWeb.Utils;
using ESAWebApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ESAWebApplication.Utils.Services
{
    /// <summary>
    /// ShareLink API Helper
    /// </summary>
    public class ShareLinkAPIHelper : IShareLinkAPIHelper
    {
        /// <summary>
        /// log4net
        /// </summary>
        log4net.ILog _log = log4net.LogManager.GetLogger("ShareLinkAPIHelper");

        /// <summary>
        /// 创建匿名共享
        /// </summary>
        /// <returns>匿名共享ID</returns>
        public string CreatShareLink(string token, CreatShareLinkModel model, out long errorCode)
        {
            try
            {
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/shared-link/v1/document/anonymous", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"CreatShareLink Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;

                        // 时间无效 
                        if (!string.IsNullOrEmpty(errModel.cause) && errModel.cause.Contains("expires") && errModel.code == 400000000)
                        {
                            errorCode = 400001010;
                        }
                        return string.Empty;
                    }
                    if (resCode == 201)
                    {
                        errorCode = 0;
                        return JsonConvert.DeserializeObject<ShareLinkItem>(resStr).id;
                    }
                    errorCode = resCode;
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"CreatShareLink Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 创建实名共享
        /// </summary>
        /// <returns>实名共享ID</returns>
        public string CreatRealNameShareLink(string token, CreatRealNameShareLinkModel model, out long errorCode)
        {
            try
            {
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/shared-link/v1/document/realname", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"CreatRealNameShareLink Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        if (errorCode == 403001063)
                        {
                            errorCode = 4030010632;
                        }
                        return string.Empty;
                    }
                    if (resCode == 201)
                    {
                        errorCode = 0;
                        return JsonConvert.DeserializeObject<ShareLinkItem>(resStr).id;
                    }
                    errorCode = resCode;
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"CreatRealNameShareLink Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 修改匿名共享
        /// </summary>
        /// <returns>匿名共享</returns>
        public long UpdateShareLink(string token, UpdateShareLinkModel model)
        {
            try
            {
                long errorCode = 0;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var res = httpClient.PutAsync($"{Constant.OAuth2Url}/api/shared-link/v1/document/anonymous/{model.link_id}", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"UpdateShareLink Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;

                        // 时间无效 
                        if (!string.IsNullOrEmpty(errModel.cause) && errModel.cause.Contains("expires") && errModel.code == 400000000)
                        {
                            errorCode = 400001010;
                        }

                        return errorCode;
                    }
                    if (resCode != 204)
                    {
                        errorCode = resCode;
                    }
                    return errorCode;
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"UpdateShareLink Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 删除匿名共享
        /// </summary>
        /// <returns>匿名共享</returns>
        public long DeleteShareLink(string token, string link_id)
        {
            try
            {
                long errorCode = 0;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                    var res = httpClient.DeleteAsync($"{Constant.OAuth2Url}/api/shared-link/v1/document/anonymous/{link_id}").Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"DeleteShareLink Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                    }
                    return errorCode;
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"DeleteShareLink Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        ///  获取匿名共享
        /// </summary>
        /// <param name="item">ShareLinkItem</param>
        /// <returns>匿名共享集合</returns>
        public List<ShareLinkModel> GetShareLink(string token, ShareLinkItem item, out long errorCode)
        {
            try
            {
                errorCode = 0;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                    var res = httpClient.GetAsync($"{Constant.OAuth2Url}/api/shared-link/v1/document/{item.type}/{item.id}?type=anonymous&offset=0&limit=1000").Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"GetShareLink Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        return null;
                    }
                    var oList = JsonConvert.DeserializeObject<List<OriginShareLinkModel>>(resStr);
                    return GetShareLinkModels(oList);
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"DeleteShareLink Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 获取匿名共享列表
        /// </summary>
        /// <param name="originShareLinks">原始数据</param>
        /// <returns>匿名共享列表</returns>
        private List<ShareLinkModel> GetShareLinkModels(List<OriginShareLinkModel> originShareLinks)
        {
            List<ShareLinkModel> shareLinkModels = new List<ShareLinkModel>();
            foreach (var origin in originShareLinks)
            {
                ShareLinkModel model = new ShareLinkModel()
                {
                    id = origin.id,
                    created_at = origin.created_at,
                    expires_at = origin.expires_at,
                    limited_times = origin.limited_times,
                    password = origin.password,
                    title = origin.title,
                    type = origin.type,
                    item = GetSItem(origin.item)
                };
                shareLinkModels.Add(model);
            }
            return shareLinkModels;
        }

        /// <summary>
        /// 获取权限数值
        /// </summary>
        /// <param name="item">原始数据</param>
        /// <returns>权限数值</returns>
        private SItem GetSItem(OItem item)
        {
            SItem sItem = new SItem();
            var perms = item.perms;
            if (perms.Contains("display") && perms.Contains("read") && perms.Contains("create") && perms.Contains("modify"))
            {
                sItem.perm = 31;
            }
            else if (perms.Contains("display") && perms.Contains("create") && perms.Contains("modify"))
            {
                sItem.perm = 25;
            }
            else
            {
                sItem.perm = 7;
            }
            return sItem;
        }

        /// <summary>
        ///  获取实名共享
        /// </summary>
        /// <param name="item">ShareLinkItem</param>
        /// <returns>实名共享ID</returns>
        public string GetRealNameShareLink(string token, ShareLinkItem item, out long errorCode)
        {
            try
            {
                errorCode = 0;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                    var res = httpClient.GetAsync($"{Constant.OAuth2Url}/api/shared-link/v1/document/{item.type}/{item.id}?type=realname&offset=0&limit=1000").Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"GetRealNameShareLink Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        if (errorCode == 403001063)
                        {
                            errorCode = 4030010632;
                        }
                        return string.Empty;
                    }
                    return JsonConvert.DeserializeObject<List<ShareLinkItem>>(resStr).FirstOrDefault()?.id;
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"GetRealNameShareLink Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 获取匿名共享模板
        /// </summary>
        /// <returns>匿名共享模板</returns>
        public ShareLinkConfigModel GetShareLinkConfig(string token, out long errorCode)
        {
            try
            {
                errorCode = 0;
                ShareLinkConfigModel shareLinkConfig = new ShareLinkConfigModel()
                {
                    accesspassword = false,
                    defaultperm = 7,
                    allowperm = 31,
                    allowaccesstimes = 0,
                    allowexpiredays = -1,
                    limitaccesstimes = false,
                    limitexpiredays = false
                };
                return shareLinkConfig;
            }
            catch (Exception ex)
            {
                _log.Debug($"GetShareLinkConfig Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 获取共享开关
        /// </summary>
        /// <returns>共享开关</returns>
        public ShareLinkSwitchModel GetShareLinkSwitch(string token, out long errorCode)
        {
            try
            {
                errorCode = 0;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/eacp/v1/perm1/getsharedocconfig", null).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"GetShareLinkConfig Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        return null;
                    }
                    return JsonConvert.DeserializeObject<ShareLinkSwitchModel>(resStr);
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"GetShareLinkConfig Exception: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 检查是否是所有者
        /// </summary>
        /// <returns>是否是所有者</returns>
        public CheckOwnerModel CheckOwner(string token, string gnsId, out long errorCode)
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

                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/eacp/v1/owner/check", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"CheckOwner Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        return null;
                    }
                    return JsonConvert.DeserializeObject<CheckOwnerModel>(resStr);
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"CheckOwner Exception: {ex.Message}");
                throw ex;
            }
        }


        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <returns>文件夹大小</returns>
        public DirSizeModel GetDirSize(string token, string gnsId, out long errorCode)
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

                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/api/efast/v1/dir/size", content).Result;
                    var resStr = res.Content.ReadAsStringAsync().Result;

                    int resCode = (int)res.StatusCode;
                    // 若为错误返回码则抛出异常
                    if (resCode < 200 || resCode >= 300)
                    {
                        var errModel = JsonConvert.DeserializeObject<ErrorModel>(resStr);
                        _log.Debug($"CheckOwner Exception: {errModel.cause}, Code: {errModel.code}");
                        errorCode = errModel.code;
                        return null;
                    }
                    return JsonConvert.DeserializeObject<DirSizeModel>(resStr);
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"GetDirSize Exception: {ex.Message}");
                throw ex;
            }
        }

    }
}