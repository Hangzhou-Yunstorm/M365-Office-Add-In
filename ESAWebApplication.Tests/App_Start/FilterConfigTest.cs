using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Mvc;

namespace ESAWebApplication.Tests
{
    [TestClass]
    public class FilterConfigTest
    {
        [TestMethod]
        public void RegisterGlobalFilters()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            Assert.IsTrue(GlobalFilters.Filters.Count > 0);
        }
    }
}
