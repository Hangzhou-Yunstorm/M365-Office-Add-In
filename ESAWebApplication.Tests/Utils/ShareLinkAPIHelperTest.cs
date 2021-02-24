using ESAOfficePlugInsWeb.Utils;
using ESAWebApplication.Models;
using ESAWebApplication.Utils.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ESAWebApplication.Tests.Utils
{
    [TestClass]
    public class ShareLinkAPIHelperTest
    {
        private static IShareLinkAPIHelper helper = new ShareLinkAPIHelper();

        [TestMethod]
        public void CreatShareLink()
        {
            CreatShareLinkModel item = new CreatShareLinkModel()
            {
                item = new CItem()
                {
                    id = TestData.fileId,
                    type = "file",
                    perm = 7
                },
                expires_at = "2020-09-30T08:00:00+08:00",
                limited_times = 10,
                password = "1234",
                title = TestData.fileName
            };
            var linkId = helper.CreatShareLink(TestData.access_token, item, out long code);
            Assert.IsNotNull(linkId);
        }

        [TestMethod]
        public void CreatRealNameShareLink()
        {
            CreatRealNameShareLinkModel model = new CreatRealNameShareLinkModel()
            {
                item = new ShareLinkItem()
                {
                    id = CommonHelper.EncodeUrl(TestData.fileId),
                    type = "file"
                }
            };
            var res = helper.CreatRealNameShareLink(TestData.access_token, model, out long code);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void UpdateShareLink()
        {
            CreatShareLinkModel item = new CreatShareLinkModel()
            {
                item = new CItem()
                {
                    id = TestData.fileId,
                    type = "file",
                    perm = 7
                },
                expires_at = "2020-09-30T08:00:00+08:00",
                limited_times = 10,
                password = "1234",
                title = TestData.fileName
            };
            var linkId = helper.CreatShareLink(TestData.access_token, item, out long code);

            UpdateShareLinkModel uitem = new UpdateShareLinkModel()
            {
                item = new UItem()
                {
                    perm = 7
                },
                link_id = linkId,
                expires_at = "2020-10-30T08:00:00+08:00",
                limited_times = 10,
                password = "2345",
                title = TestData.fileName
            };
            var code2 = helper.UpdateShareLink(TestData.access_token, uitem);
            Assert.IsNotNull(linkId);
            Assert.IsNotNull(code2);
        }

        [TestMethod]
        public void DeleteShareLink()
        {
            CreatShareLinkModel item = new CreatShareLinkModel()
            {
                item = new CItem()
                {
                    id = TestData.fileId,
                    type = "file",
                    perm = 7
                },
                expires_at = "2020-09-30T08:00:00+08:00",
                limited_times = 10,
                password = "1234",
                title = TestData.fileName
            };
            var linkId = helper.CreatShareLink(TestData.access_token, item, out long code);

            var code2 = helper.DeleteShareLink(TestData.access_token, linkId);

            Assert.IsNotNull(linkId);
            Assert.IsNotNull(code2);
        }

        [TestMethod]
        public void GetShareLink()
        {
            var model = new ShareLinkItem()
            {
                id = CommonHelper.EncodeUrl(TestData.fileId),
                type = "file"
            };
            var res = helper.GetShareLink(TestData.access_token, model, out long code);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void GetRealNameShareLink()
        {
            var model = new ShareLinkItem()
            {
                id = CommonHelper.EncodeUrl(TestData.fileId),
                type = "file"
            };
            var res = helper.GetRealNameShareLink(TestData.access_token, model, out long code);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void GetShareLinkConfig()
        {
            var res = helper.GetShareLinkConfig(TestData.access_token, out long code);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void GetShareLinkSwitch()
        {
            var res = helper.GetShareLinkSwitch(TestData.access_token, out long code);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void CheckOwner()
        {
            var res = helper.CheckOwner(TestData.access_token, TestData.fileId, out long code);
            Assert.IsNotNull(res);
        }


        [TestMethod]
        public void GetDirSize()
        {
            var res = helper.GetDirSize(TestData.access_token, TestData.folderId, out long code);
            Assert.IsNotNull(res);
        }

    }
}