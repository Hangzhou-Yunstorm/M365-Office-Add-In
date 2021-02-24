using ESAOfficePlugInsWeb.Models;
using ESAWebApplication.Models;
using ESAWebApplication.Utils.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ESAWebApplication.Tests.Models
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void CreatShareLinkModelTest()
        {
            CItem cItem = new CItem()
            {
                id = "id",
                perm = 25,
                type = "type"
            };
            var id = cItem.id;
            var perm = cItem.perm;
            var type = cItem.type;

            Assert.IsTrue(id == "id");
            Assert.IsTrue(perm == 25);
            Assert.IsTrue(type == "type");

            CreatShareLinkModel creatShareLinkModel = new CreatShareLinkModel()
            {
                item = cItem,
                expires_at = "expires_at",
                limited_times = 1,
                password = "password",
                title = "title"
            };
            var item = creatShareLinkModel.item;
            var expires_at = creatShareLinkModel.expires_at;
            var limited_times = creatShareLinkModel.limited_times;
            var password = creatShareLinkModel.password;
            var title = creatShareLinkModel.title;

            Assert.IsTrue(item == cItem);
            Assert.IsTrue(expires_at == "expires_at");
            Assert.IsTrue(limited_times == 1);
            Assert.IsTrue(password == "password");
            Assert.IsTrue(title == "title");
        }

        [TestMethod]
        public void CreatRealNameShareLinkModelTest()
        {
            ShareLinkItem shareLinkItem = new ShareLinkItem()
            {
                id = "id",
                type = "type"
            };
            var id = shareLinkItem.id;
            var type = shareLinkItem.type;

            Assert.IsTrue(id == "id");
            Assert.IsTrue(type == "type");

            CreatRealNameShareLinkModel creatRealNameShareLinkModel = new CreatRealNameShareLinkModel()
            {
                item = shareLinkItem
            };
            var item = creatRealNameShareLinkModel.item;
            Assert.IsTrue(item == shareLinkItem);
        }

        [TestMethod]
        public void FileVersionModelTest()
        {
            FileVersionModel fileVersionModel = new FileVersionModel()
            {
                Editor = "Editor",
                Modified = "Modified",
                Name = "Name",
                Rev = "Rev",
                Size = "Size",
            };
            var Editor = fileVersionModel.Editor;
            var Modified = fileVersionModel.Modified;
            var Name = fileVersionModel.Name;
            var Rev = fileVersionModel.Rev;
            var Size = fileVersionModel.Size;

            Assert.IsTrue(Editor == "Editor");
            Assert.IsTrue(Modified == "Modified");
            Assert.IsTrue(Name == "Name");
            Assert.IsTrue(Rev == "Rev");
            Assert.IsTrue(Size == "Size");
        }

        [TestMethod]
        public void JsonModelTest()
        {
            JsonModel jsonModel = new JsonModel()
            {
                Data = "Data",
                Message = "Message",
                StatusCode = 0,
                Success = true
            };
            var Data = jsonModel.Data;
            var Message = jsonModel.Message;
            var StatusCode = jsonModel.StatusCode;
            var Success = jsonModel.Success;

            Assert.IsTrue(Data == "Data");
            Assert.IsTrue(Message == "Message");
            Assert.IsTrue(StatusCode == 0);
            Assert.IsTrue(Success == true);
        }

        [TestMethod]
        public void LoginModelTest()
        {
            LoginModel loginModel = new LoginModel()
            {
                Account = "Account",
                Password = "Password"
            };
            var Account = loginModel.Account;
            var Password = loginModel.Password;

            Assert.IsTrue(Account == "Account");
            Assert.IsTrue(Password == "Password");
        }


        [TestMethod]
        public void OAuth2ResultTest()
        {
            OAuth2Result oAuth2Result = new OAuth2Result()
            {
                AccessToken = "AccessToken",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                ExpireIn = Convert.ToDateTime("2020-09-09 09:00:00"),
                IdToken = "IdToken",
                RefreshToken = "RefreshToken"
            };

            var AccessToken = oAuth2Result.AccessToken;
            var ClientId = oAuth2Result.ClientId;
            var ClientSecret = oAuth2Result.ClientSecret;
            var ExpireIn = oAuth2Result.ExpireIn;
            var IdToken = oAuth2Result.IdToken;
            var RefreshToken = oAuth2Result.RefreshToken;

            Assert.IsTrue(AccessToken == "AccessToken");
            Assert.IsTrue(ClientId == "ClientId");
            Assert.IsTrue(ClientSecret == "ClientSecret");
            Assert.IsTrue(ExpireIn == Convert.ToDateTime("2020-09-09 09:00:00"));
            Assert.IsTrue(IdToken == "IdToken");
            Assert.IsTrue(RefreshToken == "RefreshToken");

            PostOAuth2Result postOAuth2Result = new PostOAuth2Result()
            {
                Flag = "Flag",
                Result = oAuth2Result
            };
            var Flag = postOAuth2Result.Flag;
            var Result = postOAuth2Result.Result;

            Assert.IsTrue(Flag == "Flag");
            Assert.IsTrue(Result == oAuth2Result);

        }

        [TestMethod]
        public void OpenFileModelTest()
        {
            OpenFileModel openFileModel = new OpenFileModel()
            {
                DocType = "DocType",
                FileId = "FileId",
                Rev = "Rev",
                TokenId = "TokenId"
            };
            var DocType = openFileModel.DocType;
            var FileId = openFileModel.FileId;
            var Rev = openFileModel.Rev;
            var TokenId = openFileModel.TokenId;

            Assert.IsTrue(FileId == "FileId");
            Assert.IsTrue(DocType == "DocType");
            Assert.IsTrue(Rev == "Rev");
            Assert.IsTrue(TokenId == "TokenId");
        }

        [TestMethod]
        public void SaveEmailModelTest()
        {
            SaveEmailModel saveEmailModel = new SaveEmailModel()
            {
                Docid = "Docid",
                EwsId = "EwsId",
                EwsToken = "EwsToken",
                EwsUrl = "EwsUrl",
                FileName = "FileName",
                Ondup = 1,
                TokenId = "TokenId"
            };
            var Docid = saveEmailModel.Docid;
            var EwsId = saveEmailModel.EwsId;
            var EwsToken = saveEmailModel.EwsToken;
            var EwsUrl = saveEmailModel.EwsUrl;
            var FileName = saveEmailModel.FileName;
            var TokenId = saveEmailModel.TokenId;
            var Ondup = saveEmailModel.Ondup;

            Assert.IsTrue(Docid == "Docid");
            Assert.IsTrue(EwsId == "EwsId");
            Assert.IsTrue(EwsToken == "EwsToken");
            Assert.IsTrue(EwsUrl == "EwsUrl");
            Assert.IsTrue(FileName == "FileName");
            Assert.IsTrue(TokenId == "TokenId");
            Assert.IsTrue(Ondup == 1);
        }

        [TestMethod]
        public void SaveFileModelTest()
        {
            SaveFileModel saveFileModel = new SaveFileModel()
            {
                Docid = "Docid",
                Base64Str = null,
                FileName = "FileName",
                Ondup = 1,
                TokenId = "TokenId",
            };

            var Docid = saveFileModel.Docid;
            var FileName = saveFileModel.FileName;
            var TokenId = saveFileModel.TokenId;
            var Ondup = saveFileModel.Ondup;
            var Base64Str = saveFileModel.Base64Str;

            Assert.IsTrue(Docid == "Docid");
            Assert.IsTrue(FileName == "FileName");
            Assert.IsTrue(TokenId == "TokenId");
            Assert.IsTrue(Ondup == 1);
            Assert.IsTrue(Base64Str == null);
        }

        [TestMethod]
        public void SearchModelTest()
        {
            SearchModel searchModel = new SearchModel()
            {
                doctype = 2,
                hl = true,
                keys = "keys",
                keysfields = null,
                range = null,
                rows = 100,
                start = 0
            };
            var doctype = searchModel.doctype;
            var hl = searchModel.hl;
            var keys = searchModel.keys;
            var keysfields = searchModel.keysfields;
            var range = searchModel.range;
            var rows = searchModel.rows;
            var start = searchModel.start;

            Assert.IsTrue(doctype == 2);
            Assert.IsTrue(rows == 100);
            Assert.IsTrue(start == 0);
            Assert.IsTrue(keys == "keys");
            Assert.IsTrue(hl == true);
            Assert.IsTrue(keysfields == null);
            Assert.IsTrue(range == null);
        }

        [TestMethod]
        public void UpdateShareLinkModelTest()
        {
            UItem uItem = new UItem()
            {
                perm = 25
            };
            var perm = uItem.perm;
            Assert.IsTrue(perm == 25);

            UpdateShareLinkModel updateShareLinkModel = new UpdateShareLinkModel()
            {
                item = uItem,
                expires_at = "expires_at",
                limited_times = 10,
                link_id = "link_id",
                password = "password",
                title = "title",
            };
            var item = updateShareLinkModel.item;
            var expires_at = updateShareLinkModel.expires_at;
            var limited_times = updateShareLinkModel.limited_times;
            var link_id = updateShareLinkModel.link_id;
            var password = updateShareLinkModel.password;
            var title = updateShareLinkModel.title;

            Assert.IsTrue(item == uItem);
            Assert.IsTrue(limited_times == 10);
            Assert.IsTrue(expires_at == "expires_at");
            Assert.IsTrue(link_id == "link_id");
            Assert.IsTrue(password == "password");
            Assert.IsTrue(title == "title");

        }

        [TestMethod]
        public void CheckOwnerModelTest()
        {
            CheckOwnerModel checkOwnerModel = new CheckOwnerModel()
            {
                isowner = true
            };
            var isowner = checkOwnerModel.isowner;
            Assert.IsTrue(isowner == true);
        }

        [TestMethod]
        public void DirSizeModelTest()
        {
            DirSizeModel model = new DirSizeModel()
            {
                dirnum = 10,
                filenum = 10,
                recyclesize = 10,
                totalsize = 10
            };
            var dirnum = model.dirnum;
            var filenum = model.filenum;
            var recyclesize = model.recyclesize;
            var totalsize = model.totalsize;

            Assert.IsTrue(dirnum == 10);
            Assert.IsTrue(filenum == 10);
            Assert.IsTrue(recyclesize == 10);
            Assert.IsTrue(totalsize == 10);

        }

        [TestMethod]
        public void DirFileTest()
        {
            DirFile model = new DirFile()
            {
                docid = "docid",
                name = "name",
                rev = "rev",
                editor = "editor",
                creator = "creator",
                size = 1,
                duedate = 1,
                attr = 1,
                client_mtime = 1,
                create_time = 1,
                csflevel = 1,
                modified = 1
            };
            var docid = model.docid;
            var name = model.name;
            var rev = model.rev;
            var editor = model.editor;
            var creator = model.creator;
            var size = model.size;
            var duedate = model.duedate;
            var attr = model.attr;
            var client_mtime = model.client_mtime;
            var create_time = model.create_time;
            var csflevel = model.csflevel;
            var modified = model.modified;

            Assert.IsTrue(docid == "docid");
            Assert.IsTrue(name == "name");
            Assert.IsTrue(rev == "rev");
            Assert.IsTrue(editor == "editor");
            Assert.IsTrue(creator == "creator");
            Assert.IsTrue(size == 1);
            Assert.IsTrue(duedate == 1);
            Assert.IsTrue(attr == 1);
            Assert.IsTrue(client_mtime == 1);
            Assert.IsTrue(create_time == 1);
            Assert.IsTrue(csflevel == 1);
            Assert.IsTrue(modified == 1);

            DocLibModel docLibModel = new DocLibModel()
            {
                dirs = new DirFile[] { model },
                files = new DirFile[] { model }
            };
            var dirs = docLibModel.dirs;
            var files = docLibModel.files;

            Assert.IsTrue(dirs.Length == 1);
            Assert.IsTrue(files.Length == 1);
        }

        [TestMethod]
        public void DownloadFileResTest()
        {
            DownloadFileRes fileRes = new DownloadFileRes()
            {
                ErrorDetail = "ErrorDetail",
                ErrorCode = 0,
                FileValue = "FileValue"
            };
            var ErrorDetail = fileRes.ErrorDetail;
            var ErrorCode = fileRes.ErrorCode;
            var FileValue = fileRes.FileValue;

            Assert.IsTrue(ErrorDetail == "ErrorDetail");
            Assert.IsTrue(FileValue == "FileValue");
            Assert.IsTrue(ErrorCode == 0);
        }

        [TestMethod]
        public void DownloadResTest()
        {
            DownloadRes fileRes = new DownloadRes()
            {
                ErrorDetail = "ErrorDetail",
                ErrorCode = 0,
                FileName = "FileName",
                Stream = null
            };
            var ErrorDetail = fileRes.ErrorDetail;
            var ErrorCode = fileRes.ErrorCode;
            var FileName = fileRes.FileName;
            var Stream = fileRes.Stream;

            Assert.IsTrue(ErrorDetail == "ErrorDetail");
            Assert.IsTrue(ErrorCode == 0);
            Assert.IsTrue(FileName == "FileName");
            Assert.IsNull(Stream);
        }

        [TestMethod]
        public void EntryDocLibModelTest()
        {
            UserBy userBy = new UserBy()
            {
                id = "id",
                name = "name",
                type = "type"
            };
            var id = userBy.id;
            var name = userBy.name;
            var type = userBy.type;

            Assert.IsTrue(id == "id");
            Assert.IsTrue(name == "name");
            Assert.IsTrue(type == "type");

            EntryDocLibModel docLibModel = new EntryDocLibModel()
            {
                attr = 1,
                created_at = "created_at",
                modified_at = "modified_at",
                created_by = userBy,
                id = "id",
                modified_by = userBy,
                name = "name",
                rev = "rev",
                type = "type",
            };

            var id2 = docLibModel.id;
            var name2 = docLibModel.name;
            var type2 = docLibModel.type;
            var attr = docLibModel.attr;
            var created_at = docLibModel.created_at;
            var modified_at = docLibModel.modified_at;
            var created_by = docLibModel.created_by;
            var modified_by = docLibModel.modified_by;
            var rev = docLibModel.rev;

            Assert.IsTrue(id2 == "id");
            Assert.IsTrue(name2 == "name");
            Assert.IsTrue(type2 == "type");
            Assert.IsTrue(attr == 1);
            Assert.IsTrue(created_at == "created_at");
            Assert.IsTrue(modified_at == "modified_at");
            Assert.IsTrue(created_by == userBy);
            Assert.IsTrue(modified_by == userBy);

        }
        [TestMethod]
        public void ErrorModelTest()
        {
            ErrorModel errorModel = new ErrorModel()
            {
                detail = "detail",
                cause = "cause",
                code = 0,
                message = "message"
            };

            var detail = errorModel.detail;
            var cause = errorModel.cause;
            var code = errorModel.code;
            var message = errorModel.message;

            Assert.IsNotNull(detail);
            Assert.IsTrue(cause == "cause");
            Assert.IsTrue(message == "message");
            Assert.IsTrue(code == 0);

        }

        [TestMethod]
        public void SearchResponseTest()
        {
            SearchDoc searchDoc = new SearchDoc()
            {
                distance = 1,
                basename = "basename",
                csflevel = 1,
                docid = "docid",
                editor = "editor",
                ext = "ext",
                hlbasename = "hlbasename",
                modified = 1,
                parentpath = "parentpath",
                summary = "summary",
                size = 1,
                tags = null,
            };
            var distance = searchDoc.distance;
            var csflevel = searchDoc.csflevel;
            var modified = searchDoc.modified;
            var size = searchDoc.size;
            var basename = searchDoc.basename;
            var tags = searchDoc.tags;
            var docid = searchDoc.docid;
            var editor = searchDoc.editor;
            var ext = searchDoc.ext;
            var hlbasename = searchDoc.hlbasename;
            var parentpath = searchDoc.parentpath;
            var summary = searchDoc.summary;

            Assert.IsNull(tags);
            Assert.IsTrue(basename == "basename");
            Assert.IsTrue(docid == "docid");
            Assert.IsTrue(editor == "editor");
            Assert.IsTrue(ext == "ext");
            Assert.IsTrue(hlbasename == "hlbasename");
            Assert.IsTrue(parentpath == "parentpath");
            Assert.IsTrue(summary == "summary");
            Assert.IsTrue(distance == 1);
            Assert.IsTrue(csflevel == 1);
            Assert.IsTrue(modified == 1);
            Assert.IsTrue(size == 1);
        }

        [TestMethod]
        public void ShareLinkConfigModelTest()
        {
            ShareLinkConfigModel model = new ShareLinkConfigModel()
            {
                defaultperm = 1,
                accesspassword = true,
                allowaccesstimes = 1,
                allowexpiredays = 1,
                allowperm = 1,
                limitaccesstimes = true,
                limitexpiredays = true
            };

            Assert.IsTrue(model.defaultperm == 1);
            Assert.IsTrue(model.allowaccesstimes == 1);
            Assert.IsTrue(model.allowexpiredays == 1);
            Assert.IsTrue(model.allowperm == 1);
            Assert.IsTrue(model.accesspassword);
            Assert.IsTrue(model.limitaccesstimes);
            Assert.IsTrue(model.limitexpiredays);
        }

        [TestMethod]
        public void ShareLinkModelTest()
        {
            ShareLinkModel model = new ShareLinkModel()
            {
                created_at = "created_at",
                expires_at = "expires_at",
                id = "id",
                title = "title",
                type = "type",
                limited_times = 1,
                item = "item",
                password = "password"
            };

            Assert.IsTrue(model.limited_times == 1);
            Assert.IsTrue(model.created_at == "created_at");
            Assert.IsTrue(model.expires_at == "expires_at");
            Assert.IsTrue(model.id == "id");
            Assert.IsTrue(model.title == "title");
            Assert.IsTrue(model.type == "type");
            Assert.IsTrue(model.item?.ToString() == "item");
            Assert.IsTrue(model.password == "password");

            SItem item = new SItem()
            {
                perm = 1
            };
            Assert.IsTrue(item.perm == 1);
        }

        [TestMethod]
        public void ShareLinkSwitchModelTest()
        {
            ShareLinkSwitchModel model = new ShareLinkSwitchModel()
            {
                enable_user_doc_inner_link_share = true,
                enable_user_doc_out_link_share = true
            };

            Assert.IsTrue(model.enable_user_doc_inner_link_share);
            Assert.IsTrue(model.enable_user_doc_out_link_share);
        }

        [TestMethod]
        public void UploadFileResTest()
        {
            UploadFileRes model = new UploadFileRes()
            {
                ErrorDetail = "ErrorDetail",
                ErrorCode = 0,
                FileName = "FileName",
                FileId = "FileId"
            };

            var ErrorDetail = model.ErrorDetail;
            var ErrorCode = model.ErrorCode;
            var FileName = model.FileName;
            var FileId = model.FileId;

            Assert.IsTrue(ErrorDetail == "ErrorDetail");
            Assert.IsTrue(ErrorCode == 0);
            Assert.IsTrue(FileName == "FileName");
            Assert.IsTrue(FileId == "FileId");
        }

        [TestMethod]
        public void VersionModelTest()
        {
            VersionModel model = new VersionModel()
            {
                client_mtime = 1,
                editor = "editor",
                modified = 1,
                name = "name",
                rev = "rev",
                size = 1,
            };

            Assert.IsTrue(model.client_mtime == 1);
            Assert.IsTrue(model.modified == 1);
            Assert.IsTrue(model.size == 1);
            Assert.IsTrue(model.editor == "editor");
            Assert.IsTrue(model.name == "name");
            Assert.IsTrue(model.rev == "rev");
        }

        [TestMethod]
        public void UserModelTest()
        {
            Directdepinfo directdepinfo = new Directdepinfo()
            {
                depid = "depid",
                name = "name"
            };

            Assert.IsTrue(directdepinfo.depid == "depid");
            Assert.IsTrue(directdepinfo.name == "name");

            Roleinfo roleinfo = new Roleinfo()
            {
                id = "id",
                name = "name"
            };

            Assert.IsTrue(roleinfo.id == "id");
            Assert.IsTrue(roleinfo.name == "name");

            UserModel model = new UserModel()
            {
                userid = "userid",
                account = "account",
                name = "name",
                mail = "mail",
                telnumber = "telnumber",
                csflevel = 1,
                leakproofvalue = 1,
                pwdcontrol = 1,
                usertype = 1,
                roletypes = new long[] { 1 },
                agreedtotermsofuse = true,
                freezestatus = true,
                ismanager = true,
                needsecondauth = true,
                directdepinfos = new Directdepinfo[] { directdepinfo },
                roleinfos = new Roleinfo[] { roleinfo }
            };

            Assert.IsTrue(model.userid == "userid");
            Assert.IsTrue(model.name == "name"); 
            Assert.IsTrue(model.account == "account");
            Assert.IsTrue(model.mail == "mail");
            Assert.IsTrue(model.telnumber == "telnumber");
            Assert.IsTrue(model.csflevel == 1);
            Assert.IsTrue(model.leakproofvalue == 1);
            Assert.IsTrue(model.pwdcontrol == 1);
            Assert.IsTrue(model.usertype == 1);
            Assert.IsTrue(model.roletypes.Length == 1);
            Assert.IsTrue(model.directdepinfos.Length == 1);
            Assert.IsTrue(model.roleinfos.Length == 1);
            Assert.IsTrue(model.agreedtotermsofuse);
            Assert.IsTrue(model.freezestatus);
            Assert.IsTrue(model.ismanager);
            Assert.IsTrue(model.needsecondauth);

        }

    }
}
