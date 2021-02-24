using ESAWebApplication.Models;
using ESAWebApplication.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ESAWebApplication.Tests.Utils
{
    [TestClass]
    public class OAuthHelperTest
    {

        private static string flag = "abc1234567890";
        private static OAuth2Result oAuth2Result = new OAuth2Result()
        {
            AccessToken = "access_token",
            ClientId = "client_id",
            ClientSecret = "client_secret",
            ExpireIn = DateTime.Now.AddHours(1),
            IdToken = "id_token",
            RefreshToken = "refresh_token"
        };

        [TestMethod]
        public void SetOAuth2Result()
        {
            OAuthHelper.SetOAuth2Result(flag, oAuth2Result);
            Assert.IsTrue(OAuthHelper.OAuth2Results.Count > 0);
        }

        [TestMethod]
        public void SetOAuth2ResultWithOutNLB()
        {
            OAuthHelper.SetOAuth2ResultWithOutNLB(flag, oAuth2Result);
            Assert.IsTrue(OAuthHelper.OAuth2Results.Count > 0);

            OAuthHelper.SetOAuth2ResultWithOutNLB(flag, null);
            Assert.IsTrue(OAuthHelper.OAuth2Results.Count == 0);
        }

        [TestMethod]
        public void TryGetOAuth2Result()
        {
            OAuthHelper.SetOAuth2ResultWithOutNLB(flag, oAuth2Result);
            OAuthHelper.TryGetOAuth2Result(flag, out var result);

            Assert.IsNotNull(result);

            OAuthHelper.SetOAuth2ResultWithOutNLB(flag, null);
            OAuthHelper.TryGetOAuth2Result(flag, out var result2);
            Assert.IsNull(result2);
        }

        [TestMethod]
        public void RemoveExpireResult()
        {
            OAuth2Result result = new OAuth2Result()
            {
                AccessToken = "access_token",
                ClientId = "client_id",
                ClientSecret = "client_secret",
                ExpireIn = DateTime.Now.AddHours(-1),
                IdToken = "id_token",
                RefreshToken = "refresh_token"
            };
            OAuthHelper.OAuth2Results[flag] = result;

            OAuthHelper.RemoveExpireResult();

            Assert.IsTrue(OAuthHelper.OAuth2Results.Count == 0);
        }

        [TestMethod]
        public void RandomNumABC()
        {
            string rNum = OAuthHelper.RandomNumABC(6);
            Assert.IsTrue(rNum.Length == 6);
        }


    }
}