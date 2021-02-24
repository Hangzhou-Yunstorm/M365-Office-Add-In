using ESAOfficePlugInsWeb.Utils;
using System.Web.Mvc;

namespace ESAOfficePlugInWeb.Controllers
{
    public class ExcelController : Controller
    {
        /// <summary>
        /// Home
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// Settings
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            ViewBag.HelpUrl = Constant.HelpUrl;
            ViewBag.VersionInfo = Constant.VersionInfo;
            ViewBag.PublishDate = Constant.PublishDate;
            return View();
        }

        /// <summary>
        /// OpenFile
        /// </summary>
        /// <returns>View</returns>
        public ActionResult OpenFile()
        {
            return View();
        }

        /// <summary>
        /// SaveFile
        /// </summary>
        /// <returns>View</returns>
        public ActionResult SaveFile()
        {
            return View();
        }
    }
}