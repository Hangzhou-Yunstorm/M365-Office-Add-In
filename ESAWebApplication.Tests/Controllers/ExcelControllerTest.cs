using ESAOfficePlugInWeb.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace ESAWebApplication.Tests.Controllers
{
    [TestClass]
    public class ExcelControllerTest
    {
        [TestMethod]
        public void Home()
        {
            ExcelController controller = new ExcelController();
            ViewResult result = controller.Home() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Index()
        {
            ExcelController controller = new ExcelController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewBag.HelpUrl);
            Assert.IsNotNull(result.ViewBag.VersionInfo);
            Assert.IsNotNull(result.ViewBag.PublishDate);
        }

        [TestMethod]
        public void OpenFile()
        {
            ExcelController controller = new ExcelController();
            ViewResult result = controller.OpenFile() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SaveFile()
        {
            ExcelController controller = new ExcelController();
            ViewResult result = controller.SaveFile() as ViewResult;

            Assert.IsNotNull(result);
        }

    }
}
