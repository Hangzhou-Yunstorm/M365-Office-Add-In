using ESAOfficePlugInWeb.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace ESAWebApplication.Tests.Controllers
{
    [TestClass]
    public class PowerPointControllerTest
    {
        [TestMethod]
        public void Home()
        {
            PowerPointController controller = new PowerPointController();
            ViewResult result = controller.Home() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Index()
        {
            PowerPointController controller = new PowerPointController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result.ViewBag.HelpUrl);
            Assert.IsNotNull(result.ViewBag.VersionInfo);
            Assert.IsNotNull(result.ViewBag.PublishDate);
        }

        [TestMethod]
        public void OpenFile()
        {
            PowerPointController controller = new PowerPointController();
            ViewResult result = controller.OpenFile() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SaveFile()
        {
            PowerPointController controller = new PowerPointController();
            ViewResult result = controller.SaveFile() as ViewResult;

            Assert.IsNotNull(result);
        }

    }
}
