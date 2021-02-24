using System;
using System.IO;

namespace ESAWebApplication.Utils
{
    public class HangfireHelper
    {

        /// <summary>
        /// log4net
        /// </summary>
        static log4net.ILog log = log4net.LogManager.GetLogger("HangfireHelper");

        /// <summary>
        /// 上一次执行时间
        /// </summary>
        private static DateTime LastTime = DateTime.MinValue;

        /// <summary>
        /// Start Hanfire Work
        /// </summary>
        public static void StartHanfireWork()
        {
            try
            {
                if ((DateTime.Now - LastTime).TotalHours > 12)
                {
                    LastTime = DateTime.Now;
                    System.Threading.Tasks.Task.Run(() =>
                    {                        
                        DeleteCacheLogs();
                    });
                }
            }
            catch (Exception ex)
            {
                log.Debug($"StartHanfireWork Exception：{ex.Message}");
            }
        }

        /// <summary>
        /// Delete Cache Logs
        /// </summary>
        public static void DeleteCacheLogs()
        {
            try
            {
                string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\App_Data";
                log.Debug($"DeleteCacheLogs path: {path}");

                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    try
                    {
                        if (file.LastWriteTime.AddDays(7) < DateTime.Now && !"readme.txt".Equals(file.Name))
                        {
                            file.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug($"DeleteCacheLogs Excepion1: {ex.Message}");
                    }
                }

                DirectoryInfo[] directories = dir.GetDirectories();
                foreach (DirectoryInfo directory in directories)
                {
                    try
                    {
                        if (directory.LastWriteTime.AddDays(7) < DateTime.Now)
                        {
                            directory.Delete(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug($"DeleteCacheLogs Excepion2: {ex.Message}");
                    }
                }

                log.Debug($"DeleteCacheLogs End");
            }
            catch (Exception ex)
            {
                log.Info($"DeleteCacheLogs Excepion: {ex.Message}");
            }
        }

    }
}