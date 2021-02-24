using ESAOfficePlugInsWeb.Utils;
using ESAWebApplication.Models;
using ESAWebApplication.Utils;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;

namespace ESAOfficePlugInWeb.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// log4net
        /// </summary>
        log4net.ILog _log = log4net.LogManager.GetLogger("LoginController");

        /// <summary>
        /// Login
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            ViewBag.Flag = Guid.NewGuid().ToString();
            return View();
        }

        /// <summary>
        /// OAuth 2.0 Login
        /// </summary>
        public ActionResult OAuth(string flag, string language)
        {
            try
            {
                _log.Debug($"OAuth Start");

                var redirectUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/Login/Callback?flag={flag}&language={language}";
                var json = "{\"client_name\":\"yunstorm\",\"grant_types\":[\"authorization_code\",\"implicit\",\"refresh_token\"],\"response_types\":[\"token id_token\",\"code\",\"token\"],\"scope\":\"offline openid all\",\"redirect_uris\":[\"" + redirectUrl + "\"],\"post_logout_redirect_uris\":[\"" + redirectUrl + "\"],\"metadata\":{\"device\":{\"name\":\"yunstorm\",\"client_type\":\"windows\",\"description\":\"pc\"}}}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string resStr = string.Empty;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/oauth2/clients", content).Result;
                    resStr = res.Content.ReadAsStringAsync().Result;
                    _log.Debug($"OAuth oauth2 clients: '{resStr}'");
                }

                var obj = JsonConvert.DeserializeObject<dynamic>(resStr);
                var clientId = obj["client_id"].Value;
                var clientSecret = obj["client_secret"].Value;
                OAuth2Result auth2Result = new OAuth2Result()
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    ExpireIn = DateTime.Now.AddMinutes(30)
                };
                OAuthHelper.SetOAuth2Result(flag, auth2Result);

                var loginUrl = $"{Constant.OAuth2Url}/oauth2/auth?client_id={clientId}&redirect_uri={CommonHelper.EncodeUrl(redirectUrl)}&response_type=code&scope=offline+openid+all&state={OAuthHelper.RandomNumABC(24)}&lang={language}";
                _log.Debug($"OAuth End");
                return Redirect(loginUrl);
            }
            catch (Exception ex)
            {
                _log.Debug($"OAuth Exception: {ex.Message}");
                return Redirect($"/Login/Error?language={language}");
            }
        }

        /// <summary>
        /// Callback
        /// </summary>
        public ActionResult Callback(string flag, string language)
        {
            try
            {
                _log.Debug($"Callback Start");

                OAuth2Result auth2Result = null;
                bool oauth2 = OAuthHelper.TryGetOAuth2Result(flag, out auth2Result);
                var code = Request.QueryString["code"];
                _log.Debug($"Callback Code: '{code}'");

                if (!oauth2 || string.IsNullOrEmpty(code))
                {
                    _log.Debug($"Callback Invalid Param.");
                    return Redirect($"/Login/Error?language={language}");
                }
                else
                {
                    var redirectUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/Login/Callback?flag={flag}&language={language}";

                    var basic = Convert.ToBase64String(Encoding.Default.GetBytes($"{auth2Result.ClientId}:{auth2Result.ClientSecret}"));
                    string resStr = string.Empty;
                    var handler = new WebRequestHandler();
                    handler.ServerCertificateValidationCallback = delegate { return true; };
                    using (var httpClient = new HttpClient(handler))
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + basic);
                        var json = $"grant_type=authorization_code&code={code}&redirect_uri={CommonHelper.EncodeUrl(redirectUrl)}";
                        var content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");

                        var res = httpClient.PostAsync($"{Constant.OAuth2Url}/oauth2/token", content).Result;
                        resStr = res.Content.ReadAsStringAsync().Result;
                        _log.Debug($"Callback oauth2 token: '{resStr}'");
                    }

                    var obj = JsonConvert.DeserializeObject<dynamic>(resStr);
                    auth2Result.AccessToken = ScryptHelper.EncryptDES($"Bearer {obj["access_token"].Value}");
                    auth2Result.ExpireIn = DateTime.Now.AddSeconds(obj["expires_in"].Value);
                    auth2Result.IdToken = obj["id_token"].Value;
                    auth2Result.RefreshToken = ScryptHelper.EncryptDES(obj["refresh_token"].Value);

                    OAuthHelper.SetOAuth2Result(flag, auth2Result);
                    _log.Debug($"Callback End");
                    return Redirect($"/Login/Success?language={language}");
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"Callback Exception: {ex.Message}");
                return Redirect($"/Login/Error?language={language}");
            }
        }

        /// <summary>
        /// Login Error
        /// </summary>
        public ActionResult Error(string language)
        {
            ViewBag.Language = language;
            return View();
        }

        /// <summary>
        /// Login Success
        /// </summary>
        public ActionResult Success(string language)
        {
            ViewBag.Language = language;
            return View();
        }

        /// <summary>
        /// RefreshToken
        /// </summary>
        /// <param name="model">OAuth2Result</param>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult RefreshToken(OAuth2Result model)
        {
            try
            {
                // 删除日志
                HangfireHelper.StartHanfireWork();

                var clientId = model.ClientId;
                var clientSecret = model.ClientSecret;
                var refreshToken = ScryptHelper.DecryptDES(model.RefreshToken);

                var basic = Convert.ToBase64String(Encoding.Default.GetBytes($"{clientId}:{clientSecret}"));
                string resStr = string.Empty;
                var handler = new WebRequestHandler();
                handler.ServerCertificateValidationCallback = delegate { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + basic);
                    var json = $"grant_type=refresh_token&refresh_token={refreshToken}";
                    var content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");

                    var res = httpClient.PostAsync($"{Constant.OAuth2Url}/oauth2/token", content).Result;
                    resStr = res.Content.ReadAsStringAsync().Result;
                    _log.Debug($"RefreshToken oauth2 token: '{resStr}'");
                }

                var obj = JsonConvert.DeserializeObject<dynamic>(resStr);
                OAuth2Result result = new OAuth2Result()
                {
                    AccessToken = ScryptHelper.EncryptDES($"Bearer {obj["access_token"].Value}"),
                    ExpireIn = DateTime.Now.AddSeconds(obj["expires_in"].Value),
                    IdToken = obj["id_token"].Value,
                    RefreshToken = ScryptHelper.EncryptDES(obj["refresh_token"].Value),
                    ClientId = model.ClientId,
                    ClientSecret = model.ClientSecret
                };
                return Json(new { success = true, token = JsonConvert.SerializeObject(result) });
            }
            catch (Exception ex)
            {
                _log.Debug($"RefreshToken Parameter: {JsonConvert.SerializeObject(model)}, Exception: {ex.Message}");
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// WaitLogin
        /// </summary>
        /// <param name="flag">Login flag</param>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult WaitLogin(string flag)
        {
            try
            {
                OAuth2Result auth2Result = null;
                bool oauth2 = OAuthHelper.TryGetOAuth2Result(flag, out auth2Result);
                if (!oauth2 || string.IsNullOrEmpty(auth2Result.AccessToken) || string.IsNullOrEmpty(auth2Result.RefreshToken))
                {
                    return Json(new { success = false });
                }
                else
                {
                    var resultJson = JsonConvert.SerializeObject(auth2Result);
                    _log.Debug($"WaitLogin result: {resultJson}");
                    return Json(new { success = true, token = resultJson });
                }
            }
            catch (Exception ex)
            {
                _log.Debug($"WaitLogin Exception: {ex.Message}");
                return Json(new { success = false });
            }
        }

    }
}