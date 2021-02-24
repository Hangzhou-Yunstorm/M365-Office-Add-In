using ESAWebApplication.Utils;
using System;
using System.IO;
using System.Net.Http;

namespace ESAOfficePlugInsWeb.Utils
{
    /// <summary>
    /// 公共帮助类
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// log4net
        /// </summary>
        static log4net.ILog _log = log4net.LogManager.GetLogger("CommonHelper");

        /// <summary>
        /// Base64转字节数字
        /// </summary>
        /// <param name="base64String">字符串</param>
        /// <returns>字节数字</returns>
        public static byte[] FromBase64String(string base64String)
        {
            string dummyData = base64String.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }
            return Convert.FromBase64String(dummyData);
        }

        /// <summary>
        /// 文件转成 Base64 形式的String  
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>Base64</returns>
        public static string GetBase64FormPath(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                byte[] buff = new byte[fs.Length];
                fs.Read(buff, 0, (int)fs.Length);
                return Convert.ToBase64String(buff);
            }
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="url">URL字符串</param>
        /// <returns>编码后的URL字符串<</returns>
        public static string EncodeUrl(string url)
        {
            //url = System.Web.HttpUtility.UrlEncode(url, Encoding.UTF8);
            url = Microsoft.JScript.GlobalObject.encodeURIComponent(url);
            return url;
        }

        /// <summary>
        /// URL解码
        /// <param name="url">URL字符串</param>
        /// <returns>解码后的URL字符串<</returns>
        public static string DecodeUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            //url = System.Web.HttpUtility.UrlDecode(url, Encoding.UTF8);
            url = Microsoft.JScript.GlobalObject.decodeURIComponent(url);
            return url;
        }

        /// <summary>
        /// 获取解密后token
        /// <param name="token">token</param>
        /// <returns>解密后token<</returns>
        public static string GetToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return token;
            }
            return ScryptHelper.DecryptDES(Microsoft.JScript.GlobalObject.decodeURIComponent(token));
        }

        /// <summary>
        /// 时间戳转本地时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns>本地时间</returns>
        public static string StampToDateTime(long? timeStamp)
        {
            if (!timeStamp.HasValue)
            {
                return string.Empty;
            }

            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = timeStamp.Value * 10;
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow).ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        ///  文件大小单位转换
        /// </summary>
        /// <param name="length">文件大小</param>
        /// <returns>单位转换</returns>
        public static string StampToSize(long? length)
        {
            if (!length.HasValue)
            {
                return "0B";
            }

            string lengthStr = string.Empty;
            long lengVal = length.Value;
            if (length >= 1024 * 1024)
            {
                lengthStr = (lengVal / 1024 / 1024).ToString("f2") + "MB";
            }
            else if (length >= 1024)
            {
                lengthStr = (lengVal / 1024).ToString("f2") + "KB";
            }
            else
            {
                lengthStr = lengVal + "B";
            }
            return lengthStr;
        }

        /// <summary>
        /// 获取URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns>URL</returns>
        public static string GetUrl(string url)
        {
            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }
            return url;
        }

    }
}