using ESAWebApplication.Models;
using ESAWebApplication.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace ESAWebApplication.Controllers
{
    public class NLBOAuth2Controller : Controller
    {
        /// <summary>
        /// log4net
        /// </summary>
        log4net.ILog _log = log4net.LogManager.GetLogger("NLBOAuth2Controller");

        /// <summary>
        /// Set NLB Result
        /// </summary>
        /// <returns>Result</returns>
        [HttpPost]
        public JsonResult SetNLBOAuth2()
        {
            try
            {
                string postString = string.Empty;
                using (StreamReader sr = new StreamReader(Request.InputStream, Encoding.UTF8))
                {
                    postString = sr.ReadToEnd();
                }

                if (!string.IsNullOrEmpty(postString))
                {
                    var postResult = JsonConvert.DeserializeObject<PostOAuth2Result>(postString);
                    OAuthHelper.SetOAuth2ResultWithOutNLB(postResult.Flag, postResult.Result);
                    return Json("Set Success.");
                }
                else
                {
                    _log.Info($"SetNLBOAuth2 Exception: Post Data Is Null.");
                    return Json("Post Data Is Null.");
                }
            }
            catch (Exception ex)
            {
                _log.Info($"SetNLBOAuth2 Exception: {ex.Message}");
                return Json(ex.Message);
            }
        }
    }
}