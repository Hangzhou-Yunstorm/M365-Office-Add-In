using ESAOfficePlugInsWeb.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ESAWebApplication.Tests.Utils
{
    [TestClass]
    public class CommonHelperTest
    {

        [TestMethod]
        public void FromBase64String()
        {
            string base64String = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("abc1234567890"));
            var bytes = CommonHelper.FromBase64String(base64String);
            Assert.IsNotNull(bytes);
        }

        [TestMethod]
        public void GetBase64FormPath()
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\TestFile\\testfile.docx";
            string base64 = CommonHelper.GetBase64FormPath(path);
            Assert.IsNotNull(base64);
        }

        public const string enUrl = "https%3A%2F%2Fanyshare.eisoo.cn";
        public const string deUrl = "https://anyshare.eisoo.cn";

        [TestMethod]
        public void EncodeUrl()
        {
            var url = CommonHelper.EncodeUrl(deUrl);
            Assert.AreEqual(url, enUrl);
        }

        [TestMethod]
        public void DecodeUrl()
        {
            var url = CommonHelper.DecodeUrl(enUrl);
            Assert.AreEqual(url, deUrl);

            var url2 = CommonHelper.DecodeUrl(null);
            Assert.IsNull(url2);
        }

        [TestMethod]
        public void StampToDateTime()
        {
            long timeStamp = 1380245084296354;
            var timeStr = CommonHelper.StampToDateTime(timeStamp);
            Assert.IsNotNull(timeStr);

            var timeNull = CommonHelper.StampToDateTime(null);
            Assert.IsTrue(string.IsNullOrEmpty(timeNull));
        }

        [TestMethod]
        public void StampToSize()
        {
            long length = 1024;
            var sizeStr = CommonHelper.StampToSize(length);
            Assert.AreEqual("1.00KB", sizeStr);

            length = 1024 * 1024;
            var sizeStr2 = CommonHelper.StampToSize(length);
            Assert.AreEqual("1.00MB", sizeStr2);

            length = 1023;
            var sizeStr3 = CommonHelper.StampToSize(length);
            Assert.AreEqual("1023B", sizeStr3);

            var size0B = CommonHelper.StampToSize(null);
            Assert.AreEqual("0B", size0B);
        }

    }
}