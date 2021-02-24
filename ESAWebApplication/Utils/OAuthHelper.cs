using ESAOfficePlugInsWeb.Utils;
using ESAWebApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ESAWebApplication.Utils
{
    /// <summary>
    /// OAuth2 登录结果帮助类
    /// </summary>
    public static class OAuthHelper
    {
        /// <summary>
        /// log4net
        /// </summary>
        static log4net.ILog _log = log4net.LogManager.GetLogger("LoginController");

        /// <summary>
        /// OAuth2登录集合
        /// </summary>
        public static Dictionary<string, OAuth2Result> OAuth2Results = new Dictionary<string, OAuth2Result>();

        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns></returns>
        public static bool TryGetOAuth2Result(string flag, out OAuth2Result result)
        {
            lock (OAuth2Results)
            {
                if (OAuth2Results.TryGetValue(flag, out result))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 设置登录信息(设置其他NLB)
        /// </summary>
        public static void SetOAuth2Result(string flag, OAuth2Result result)
        {
            SetOAuth2ResultWithOutNLB(flag, result);

            // Set Other NLB
            if (!string.IsNullOrEmpty(Constant.OtherNLBUrls))
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    SetOtherNLBResults(flag, result);
                });
            }

        }

        /// <summary>
        /// 设置登录信息
        /// </summary>
        public static void SetOAuth2ResultWithOutNLB(string flag, OAuth2Result result)
        {
            lock (OAuth2Results)
            {
                RemoveExpireResult();
                if (result == null)
                {
                    OAuth2Results.Remove(flag);
                }
                else
                {
                    OAuth2Results[flag] = result;
                }
            }
        }

        /// <summary>
        /// 删除过期信息
        /// </summary>
        public static void RemoveExpireResult()
        {
            var expires = OAuth2Results.Where(T => T.Value.ExpireIn < DateTime.Now);
            foreach (KeyValuePair<string, OAuth2Result> ex in expires)
            {
                OAuth2Results.Remove(ex.Key);
                if (expires == null || expires.Count() == 0)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Set Other NLB Results
        /// </summary>
        private static void SetOtherNLBResults(string flag, OAuth2Result auth2Result)
        {
            _log.Info($"SetOtherNLBResults Start.");

            var nlbUrlList = Constant.OtherNLBUrls.Split(',');
            foreach (string url in nlbUrlList)
            {
                try
                {
                    string nlbUrl = CommonHelper.GetUrl(url);

                    PostOAuth2Result postResult = new PostOAuth2Result()
                    {
                        Flag = flag,
                        Result = auth2Result
                    };
                    HttpContent formdata = new StringContent(JsonConvert.SerializeObject(postResult));

                    // WebRequestHandler
                    var handler = new WebRequestHandler();
                    handler.ServerCertificateValidationCallback = delegate { return true; };
                    using (var httpClient = new HttpClient(handler))
                    {
                        var reqResult = httpClient.PostAsync($"{nlbUrl}/NLBOAuth2/SetNLBOAuth2", formdata).Result;
                        var result = reqResult.Content.ReadAsStringAsync().Result;

                        _log.Info($"SetOtherNLBResults End, url: {url}, Message: {result}");
                    }
                }
                catch (Exception ex)
                {
                    _log.Info($"SetOtherNLBResults url: {url}, Exception: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="num">字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string RandomNumABC(int num)
        {
            string randomStr = string.Empty;
            string[] allKey = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            Random random = new Random();
            for (int i = 0; i < num; i++)
            {
                int t = random.Next(18);
                randomStr += allKey[t];
            }
            return randomStr;
        }


    }
}