using ESAOfficePlugInsWeb.Models;
using ESAOfficePlugInsWeb.Utils;
using ESAWebApplication.Models;
using ESAWebApplication.Utils.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ESAWebApplication.Tests.Utils
{
    [TestClass]
    public class AS7APIHelperTest
    {

        private static IAS7APIHelper helper = new AS7APIHelper();

        [TestMethod]
        public void GetLoginUser()
        {
            var user = helper.GetLoginUser(TestData.access_token, out long code);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void GetEntryDocLibs()
        {
            var models = helper.GetEntryDocLibs(TestData.access_token, "user_doc_lib", out long code);
            Assert.IsNotNull(models);
        }

        [TestMethod]
        public void GetDocLibsById()
        {
            var models = helper.GetDocLibsById(TestData.access_token, TestData.folderId, out long code);
            Assert.IsNotNull(models);
        }

        [TestMethod]
        public void GetFileVersions()
        {
            var models = helper.GetFileVersions(TestData.access_token, TestData.fileId, out long code);
            Assert.IsNotNull(models);
        }

        [TestMethod]
        public void CreateDir()
        {
            var code = helper.CreateDir(TestData.access_token, TestData.folderId, TestData.folderName);
            Assert.IsNotNull(code);
        }

        [TestMethod]
        public void GetSuggestFileName()
        {
            var name = helper.GetSuggestFileName(TestData.access_token, TestData.folderId, TestData.fileName);
            Assert.IsNotNull(name);
        }

        [TestMethod]
        public void DownloadFileBase64()
        {
            OpenFileModel model = new OpenFileModel()
            {
                DocType = "word",
                FileId = TestData.fileId,
                Rev = null,
                TokenId = TestData.access_token
            };
            var res = helper.DownloadFileBase64(model);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void FileDownload()
        {
            OpenFileModel model = new OpenFileModel()
            {
                DocType = "word",
                FileId = TestData.fileId,
                Rev = null,
                TokenId = TestData.access_token
            };
            var savePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Guid.NewGuid()}";
            var res = helper.FileDownload(model, savePath);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void UploadFile()
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\TestFile\\testfile.docx";
            string base64 = CommonHelper.GetBase64FormPath(path);
            byte[] base64Bytes = System.Text.Encoding.Default.GetBytes(base64);
            SaveFileModel model = new SaveFileModel()
            {
                Base64Str = base64Bytes,
                FileName = TestData.fileName,
                Docid = TestData.folderId,
                TokenId = TestData.access_token,
                Ondup = 2
            };
            var res = helper.UploadFile(model);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Search()
        {

            SearchModel model = new SearchModel()
            {
                start = 0,
                range = new List<string>(),
                keys = TestData.searchKey,
                hl = true,
                keysfields = new List<string>() { "basename" },
                doctype = 3,
                rows = 1000
            };
            var result = helper.Search(TestData.access_token, model, out long code);
            Assert.IsNotNull(result);
        }


    }
}