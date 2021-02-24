using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ESAOfficePlugInWeb.Controllers;
using ESAWebApplication.Models;
using ESAWebApplication.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ESAWebApplication.Tests.Controllers
{
    [TestClass]
    public class LoginControllerTest
    {
        [TestMethod]
        public void Index()
        {
            LoginController controller = new LoginController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewBag.Flag);
        }

        [TestMethod]
        public void OAuth()
        {
            string flag = "abc1234567890";
            string lang = "zh-cn";
            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.Url).Returns(new Uri("http://localhost:61543"));

            var controller = new LoginController()
            {
                ControllerContext = context.Object
            };

            ViewResult result = controller.OAuth(flag, lang) as ViewResult;
            //Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Callback()
        {
            string flag = "abc1234567890";
            string lang = "zh-cn";

            var requestParams = new NameValueCollection
            {
                { "code", "abc1234567890"}
            };

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.QueryString).Returns(requestParams);

            var controller = new LoginController()
            {
                ControllerContext = context.Object
            };


            ViewResult result = controller.Callback(flag, lang) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RefreshToken()
        {
            var controller = new LoginController();
            var Result = new OAuth2Result()
            {
                AccessToken = "AccessToken",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                ExpireIn = Convert.ToDateTime("2020-09-09 09:00:00"),
                IdToken = "IdToken",
                RefreshToken = "RefreshToken"
            };
            JsonResult result = controller.RefreshToken(Result);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void WaitLogin()
        {
            var controller = new LoginController();

            string flag = "abc1234567890";
            OAuth2Result oAuth2Result = new OAuth2Result()
            {
                AccessToken = "access_token",
                ClientId = "client_id",
                ClientSecret = "client_secret",
                ExpireIn = DateTime.Now.AddHours(-1),
                IdToken = "id_token",
                RefreshToken = "refresh_token"
            };
            OAuthHelper.OAuth2Results[flag] = oAuth2Result;

            JsonResult result = controller.WaitLogin(flag);
            Assert.IsNotNull(result.Data);

            JsonResult result2 = controller.WaitLogin("abc123456789");
            Assert.IsNotNull(result2.Data);
        }
    }
}
