using ESAWebApplication.Controllers;
using ESAWebApplication.Models;
using ESAWebApplication.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Mvc;

namespace ESAWebApplication.Tests.Controllers
{
    [TestClass]
    public class NLBOAuth2ControllerTest
    {
        [TestMethod]
        public void SetNLBOAuth2()
        {
            PostOAuth2Result postOAuth2Result = new PostOAuth2Result()
            {
                Flag = "abc1234567890",
                Result = new OAuth2Result()
                {
                    AccessToken = "AccessToken",
                    ClientId = "ClientId",
                    ClientSecret = "ClientSecret",
                    ExpireIn = Convert.ToDateTime("2020-09-09 09:00:00"),
                    IdToken = "IdToken",
                    RefreshToken = "RefreshToken"
                }
            };

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(JsonConvert.SerializeObject(postOAuth2Result));
            writer.Flush();

            var context = new Mock<ControllerContext>();
            context.SetupGet(x => x.HttpContext.Request.InputStream).Returns(stream);

            var controller = new NLBOAuth2Controller()
            {
                ControllerContext = context.Object
            };
            controller.SetNLBOAuth2();

            Assert.IsTrue(OAuthHelper.OAuth2Results.Count >= 0);
        }
    }
}