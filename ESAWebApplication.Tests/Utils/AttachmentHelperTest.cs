using ESAWebApplication.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ESAWebApplication.Tests.Utils
{
    [TestClass]
    public class AttachmentHelperTest
    {
        private static string flag = "abc1234567890";
        private static byte[] byteArray = System.Text.Encoding.Default.GetBytes(flag);

        [TestMethod]
        public void SetAttachmentResult()
        {
            AttachmentHelper.SetAttachmentResult(flag, byteArray);
            Assert.IsTrue(AttachmentHelper.AttachmentResults.Count > 0);

            AttachmentHelper.SetAttachmentResult(flag, null);
            Assert.IsTrue(AttachmentHelper.AttachmentResults.Count == 0);
        }


        [TestMethod]
        public void TryGetAttachmentResult()
        {
            AttachmentHelper.SetAttachmentResult(flag, byteArray);
            AttachmentHelper.TryGetAttachmentResult(flag, out var result);

            Assert.IsNotNull(result);


            AttachmentContent attachment = new AttachmentContent()
            {
                Content = byteArray,
                ExpireIn = DateTime.Now.AddDays(-1)
            };
            AttachmentHelper.AttachmentResults[flag] = attachment;
            AttachmentHelper.TryGetAttachmentResult(flag, out var result2);

            Assert.IsNull(result2);

        }

        [TestMethod]
        public void RemoveExpireResult()
        {
            AttachmentContent attachment = new AttachmentContent()
            {
                Content = byteArray,
                ExpireIn = DateTime.Now.AddDays(-1)
            };
            AttachmentHelper.AttachmentResults[flag] = attachment;

            AttachmentHelper.RemoveExpireResult();

            Assert.IsTrue(AttachmentHelper.AttachmentResults.Count == 0);
        }
    }

}