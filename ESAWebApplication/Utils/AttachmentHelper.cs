using System;
using System.Collections.Generic;
using System.Linq;

namespace ESAWebApplication.Utils
{
    /// <summary>
    /// 附件帮助类
    /// </summary>
    public static class AttachmentHelper
    {
        /// <summary>
        /// 附件集合
        /// </summary>
        public static Dictionary<string, AttachmentContent> AttachmentResults = new Dictionary<string, AttachmentContent>();

        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <returns></returns>
        public static bool TryGetAttachmentResult(string flag, out byte[] result)
        {
            lock (AttachmentResults)
            {
                AttachmentContent attachment = null;
                if (AttachmentResults.TryGetValue(flag, out attachment))
                {
                    if (attachment.ExpireIn < DateTime.Now)
                    {
                        AttachmentResults.Remove(flag);
                        result = null;
                        return false;
                    }
                    result = attachment.Content;
                    return true;
                }
            }
            result = null;
            return false;
        }

        /// <summary>
        /// 设置附件信息
        /// </summary>
        public static void SetAttachmentResult(string flag, byte[] result)
        {
            lock (AttachmentResults)
            {
                RemoveExpireResult();
                if (result == null)
                {
                    AttachmentResults.Remove(flag);
                }
                else
                {
                    AttachmentContent attachment = new AttachmentContent()
                    {
                        Content = result,
                        ExpireIn = DateTime.Now.AddDays(1)
                    };
                    AttachmentResults[flag] = attachment;
                }
            }
        }

        /// <summary>
        /// 删除过期信息
        /// </summary>
        public static void RemoveExpireResult()
        {
            var expires = AttachmentResults.Where(T => T.Value.ExpireIn < DateTime.Now);
            if (expires != null && expires.Count() > 0)
            {
                var length = expires.Count();
                foreach (KeyValuePair<string, AttachmentContent> ex in expires)
                {
                    AttachmentResults.Remove(ex.Key);
                    if (expires == null || expires.Count() == 0)
                    {
                        break;
                    }
                }
            }
        }
    }

    public class AttachmentContent
    {
        public DateTime ExpireIn { get; set; }
        public byte[] Content { get; set; }
    }
}