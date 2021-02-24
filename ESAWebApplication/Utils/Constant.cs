using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ESAOfficePlugInsWeb.Utils
{
    /// <summary>
    /// 配置文件类
    /// </summary>
    public class Constant
    {

        /// <summary>
        /// 在线帮助地址
        /// </summary>
        public static string HelpUrl = ConfigurationManager.AppSettings["HelpUrl"];

        /// <summary>
        /// 版本信息
        /// </summary>
        public static string VersionInfo = GetVersion();

        /// <summary>
        /// 发布时间
        /// </summary>
        public static string PublishDate = GetPublishDate();

        /// <summary>
        /// OAuth2 网址
        /// </summary>
        public static string OAuth2Url = CommonHelper.GetUrl(ConfigurationManager.AppSettings["OAuth2Url"]);

        /// <summary>
        /// 其他负载均衡地址
        /// </summary>
        public static string OtherNLBUrls = ConfigurationManager.AppSettings["OtherNLBUrls"];

        /// <summary>
        /// 获取版本信息
        /// </summary>
        public static string GetVersion()
        {
            try
            {
                string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\Config\\version.txt";
                string result = string.Empty;
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    string content = string.Empty;
                    while ((content = sr.ReadLine()) != null)
                    {
                        result = content;
                        break;
                    }
                }
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取发布日期
        /// </summary>
        public static string GetPublishDate()
        {
            try
            {
                string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\Config\\date.txt";
                string result = string.Empty;
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    string content = string.Empty;
                    while ((content = sr.ReadLine()) != null)
                    {
                        result = content;
                        break;
                    }
                }
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }



    }
}