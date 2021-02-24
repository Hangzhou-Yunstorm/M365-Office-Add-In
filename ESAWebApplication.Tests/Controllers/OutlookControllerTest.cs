using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ESAOfficePlugInsWeb.Models;
using ESAOfficePlugInWeb.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ESAWebApplication.Tests.Controllers
{
    [TestClass]
    public class OutlookControllerTest
    {

        [TestMethod]
        public void Index()
        {
            OutlookController controller = new OutlookController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result.ViewBag.HelpUrl);
            Assert.IsNotNull(result.ViewBag.VersionInfo);
            Assert.IsNotNull(result.ViewBag.PublishDate);
        }

        [TestMethod]
        public void SaveEmail()
        {
            OutlookController controller = new OutlookController();
            ViewResult result = controller.SaveEmail() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SaveAttachments()
        {
            OutlookController controller = new OutlookController();
            ViewResult result = controller.SaveAttachments() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddAttachments()
        {
            OutlookController controller = new OutlookController();
            ViewResult result = controller.AddAttachments() as ViewResult;

            Assert.IsNotNull(result.ViewBag.ASUrl);
        }


        [TestMethod]
        public void SaveEmailToServer()
        {
            SaveEmailModel saveEmailModel = new SaveEmailModel()
            {
                Docid = TestData.folderId,
                EwsId = "EwsId",
                EwsToken = "EwsToken",
                EwsUrl = "EwsUrl",
                FileName = TestData.fileName,
                Ondup = 2,
                TokenId = TestData.access_token
            };
            OutlookController controller = new OutlookController();
            JsonResult result = controller.SaveEmailToServer(saveEmailModel);

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void SaveAttachmentToServer()
        {
            var requestParams = new NameValueCollection
            {
                { "FileName", TestData.fileName},
                { "Docid", TestData.folderId},
                { "Ondup", "2"},
                { "TokenId", TestData.access_token},
                { "AttachmentId", "AttachmentId"},
                { "EwsUrl", "EwsUrl"},
                { "EwsId", "EwsId"},
                { "EwsToken", "EwsToken"}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new OutlookController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.SaveAttachmentToServer();

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void GetShareLinkConfig()
        {
            var requestParams = new NameValueCollection
            {
                { "TokenId", TestData.access_token}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new OutlookController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.GetShareLinkConfig();

            Assert.IsNotNull(result.Data);
        }


        [TestMethod]
        public void GetShareLinkSwitch()
        {
            var requestParams = new NameValueCollection
            {
                { "TokenId",TestData.access_token}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new OutlookController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.GetShareLinkSwitch();

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void GetShareLinks()
        {
            var requestParams = new NameValueCollection
            {
                { "TokenId", TestData.access_token},
                { "Docid", TestData.fileId},
                { "FileType", "file"}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new OutlookController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.GetShareLinks();

            Assert.IsNotNull(result.Data);
        }

        private static string linkId = string.Empty;

        [TestMethod]
        public void CreateShareLink()
        {
            var requestParams = new NameValueCollection
            {
                { "TokenId", TestData.access_token},
                { "Docid", TestData.fileId},
                { "FileType", "file"},
                { "Perm", "7"},
                { "ExpiresAt", "2020-09-30T08:00:00+08:00"},
                { "LimitedTimes", "10"},
                { "Password", "Password"},
                { "Title", "Title"}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new OutlookController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.CreateShareLink();
            linkId = result.Data.ToString();

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void UpdateShareLink()
        {
            var requestParams = new NameValueCollection
            {
                { "TokenId", TestData.access_token},
                { "LinkId", linkId},
                { "Perm", "7"},
                { "ExpiresAt", "2020-09-30T08:00:00+08:00"},
                { "LimitedTimes", "10"},
                { "Password", "Password"},
                { "Title", "Title"}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new OutlookController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.UpdateShareLink();

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void RemoveShareLink()
        {
            var requestParams = new NameValueCollection
            {
               { "TokenId", TestData.access_token},
               { "LinkId", linkId}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new OutlookController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.RemoveShareLink();

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void GetRealNameShareLinkId()
        {
            var requestParams = new NameValueCollection
            {
                { "TokenId", TestData.access_token},
                { "Docid", TestData.fileId},
                { "FileType", "file"}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new OutlookController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.GetRealNameShareLinkId();

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void CheckOwner()
        {
            var requestParams = new NameValueCollection
            {
               { "TokenId", TestData.access_token},
                { "Docid", TestData.fileId}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new OutlookController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.CheckOwner();

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void GetDirSize()
        {
            var requestParams = new NameValueCollection
            {
                { "TokenId", TestData.access_token},
                { "Docid", TestData.folderId}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new OutlookController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.GetDirSize();

            Assert.IsNotNull(result.Data);
        }

    }
}
