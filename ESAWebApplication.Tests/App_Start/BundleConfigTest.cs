using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Optimization;

namespace ESAWebApplication.Tests
{
    [TestClass]
    public class BundleConfigTest
    {
        [TestMethod]
        public void RegisterBundles()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Assert.IsTrue(BundleTable.Bundles.Count > 0);
        }
    }
}
