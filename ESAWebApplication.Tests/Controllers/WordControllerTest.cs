using ESAOfficePlugInsWeb.Models;
using ESAOfficePlugInsWeb.Utils;
using ESAOfficePlugInWeb.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace ESAWebApplication.Tests.Controllers
{
    [TestClass]
    public class WordControllerTest
    {
        [TestMethod]
        public void Home()
        {
            WordController controller = new WordController();
            ViewResult result = controller.Home() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Index()
        {
            WordController controller = new WordController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result.ViewBag.HelpUrl);
            Assert.IsNotNull(result.ViewBag.VersionInfo);
            Assert.IsNotNull(result.ViewBag.PublishDate);
        }

        [TestMethod]
        public void OpenFile()
        {
            WordController controller = new WordController();
            ViewResult result = controller.OpenFile() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SaveFile()
        {
            WordController controller = new WordController();
            ViewResult result = controller.SaveFile() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CompareFile()
        {
            WordController controller = new WordController();
            ViewResult result = controller.CompareFile() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetLoginUser()
        {
            WordController controller = new WordController();
            JsonResult result = controller.GetLoginUser(TestData.access_token);

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void SaveFileToServer()
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\TestFile\\testfile.docx";
            string base64 = CommonHelper.GetBase64FormPath(path);

            var requestParams = new NameValueCollection
            {
                { "Base64Str", base64},
                { "FileName", TestData.fileName},
                { "Docid", TestData.folderId},
                { "TokenId", TestData.access_token},
                { "Ondup", "2"}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new WordController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.SaveFileToServer();

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void OpenFileFromServer()
        {
            WordController controller = new WordController();

            OpenFileModel openFileModel = new OpenFileModel()
            {
                DocType = "word",
                FileId = TestData.fileId,
                Rev = null,
                TokenId = TestData.access_token
            };
            JsonResult result = controller.OpenFileFromServer(openFileModel);

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void CompareFileFromServer()
        {

            string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\TestFile\\testfile.docx";
            string base64 = CommonHelper.GetBase64FormPath(path);

            var requestParams = new NameValueCollection
            {
                { "Base64Str", base64},
                { "Docid", TestData.fileId},
                { "TokenId", TestData.access_token},
                { "Rev", ""}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new WordController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.CompareFileFromServer();

            Assert.IsNotNull(result.Data);
        }


        [TestMethod]
        public void GetEntryDocLibs()
        {
            WordController controller = new WordController();
            JsonResult result = controller.GetEntryDocLibs(TestData.access_token, "user_doc_lib");

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void GetDocLibsById()
        {
            WordController controller = new WordController();
            JsonResult result = controller.GetDocLibsById(TestData.access_token, TestData.folderId, "");

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void CreateDir()
        {
            WordController controller = new WordController();
            JsonResult result = controller.CreateDir(TestData.access_token, TestData.folderId, TestData.folderName);

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void Search()
        {
            WordController controller = new WordController();
            JsonResult result = controller.Search(TestData.access_token, "", TestData.searchKey, 3, "");

            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void GetFileRevisions()
        {
            var requestParams = new NameValueCollection
            {
                { "Docid", TestData.fileId},
                { "TokenId", TestData.access_token}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Form).Returns(requestParams);

            var controller = new WordController()
            {
                ControllerContext = context.Object
            };
            JsonResult result = controller.GetFileRevisions();

            Assert.IsNotNull(result.Data);
        }



    }
}
