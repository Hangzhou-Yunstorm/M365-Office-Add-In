using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Routing;

namespace ESAWebApplication.Tests
{
    [TestClass]
    public class RouteConfigTest
    {
        [TestMethod]
        public  void RegisterRoutes( )
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Assert.IsNotNull(RouteTable.Routes);
        }
    }
}
