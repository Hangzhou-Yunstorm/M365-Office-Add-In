var anyshare = anyshare || {};

(function () {
    Office.onReady(function () { });

    /* BASE **********************************************/
    anyshare.base = anyshare.base || {};
    //初始话默认的文件路径
    var defaultFolder = localStorage.getItem('defaultFolderName');
    var defaultFolderId = localStorage.getItem('defaultFolderId');
    if (defaultFolder && defaultFolderId) {
        anyshare.base.defaultFolderName = defaultFolder;
        anyshare.base.defaultFolderId = defaultFolderId;
    }

    /* AUTHORIZATION **********************************************/
    var refreshToken = function () {
        var isLogin = false;
        var dataObject = {
            RefreshToken: localStorage.getItem('Rda1s7wQki'),
            ClientId: localStorage.getItem('ZWKL6EVWcC'),
            ClientSecret: localStorage.getItem('LIt5ZdmX7L')
        };
        $.ajax({
            beforeSend: function () { },
            url: '/Login/RefreshToken',
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            async: false,
            data: JSON.stringify(dataObject),
            dataType: 'json',
            success: function (data) {
                if (data.success) {
                    var token = new Function('return ' + data.token)();
                    localStorage.setItem('AwUnmdkDT5', token.AccessToken);
                    localStorage.setItem('Rda1s7wQki', token.RefreshToken);
                    localStorage.setItem('ZWKL6EVWcC', token.ClientId);
                    localStorage.setItem('LIt5ZdmX7L', token.ClientSecret);
                    localStorage.setItem('LdVjWTdfET', new Date().getTime());
                    isLogin = true;
                } else {
                    isLogin = false;
                }
            },
            error: function (e) {
                isLogin = false;
            }
        });
        return isLogin;
    };

    var isLogin = function () {
        var accessToken = localStorage.getItem('AwUnmdkDT5');
        if (!accessToken) {
            return false;
        }

        var lastLogin = localStorage.getItem('LdVjWTdfET');
        if (lastLogin) {
            var currentTime = new Date().getTime();
            if ((currentTime - lastLogin) / 60 / 1000 < 30) {
                return true;
            }
        }
        return refreshToken();
    };

    var logout = function () {
        localStorage.removeItem('AwUnmdkDT5');
        window.location.href =
            '/Login?ReturnUrl=' + window.location.pathname + window.location.hash;
    };

    var checkLogin = function () {
        if (!isLogin()) {
            logout();
        }
    };

    var getAccessToken = function () {
        checkLogin();
        return encodeURIComponent(localStorage.getItem('AwUnmdkDT5'));
    };

    anyshare.auth = anyshare.auth || {};
    anyshare.auth.refreshToken = refreshToken;
    anyshare.auth.isLogin = isLogin;
    anyshare.auth.logout = logout;
    anyshare.auth.checkLogin = checkLogin;
    anyshare.auth.getAccessToken = getAccessToken;

    /* ANYSHARE OUTLOOK **********************************************/
    anyshare.outlook = anyshare.outlook || {};

    /** 获取标题图片 */
    var getBgImage = function () {
        var lang = localStorage.getItem('currentLanguage');
        var image = window.location.origin + '/Images/Icons/Outlook/zh-cn.png';
        if (lang) {
            if (lang == 'en-us') {
                image = window.location.origin + '/Images/Icons/Outlook/en-us.png';
            } else if (lang == 'zh-tw') {
                image = window.location.origin + '/Images/Icons/Outlook/zh-tw.png';
            }
        }
        return image;
    };

    //生成随机 GUID 数
    var getGuid = function () {
        function S4() {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        }
        return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
    };

    /** 多语言字段 */
    var detailsL, passwordL, sizeL, urlTitleL, shareTypeL, withUsersL, withAnyoneL;

    /** 获取多语言文本 */
    var getLanguageTitle = function () {
        var UIText = UIStrings.getLocaleStrings();
        detailsL = UIText.Details;
        passwordL = UIText.Password;
        sizeL = UIText.Size;
        urlTitleL = UIText.UrlTitle;
        shareTypeL = UIText.ShareType;
        withUsersL = UIText.WithUsers;
        withAnyoneL = UIText.WithAnyone;
    };

    /**
     * 获取文件格式对应图片
     * @param {any} fileName 文件名
     */
    var getFileIcon = function (fileName) {
        var fileExt;
        var index1 = fileName.lastIndexOf('.');
        //获取字符串长度
        if (index1 >= 1) {
            fileExt = fileName.substring(index1, fileName.length);
        }

        var icon = '';
        switch (fileExt) {
            case '.doc':
            case '.docx':
                icon = window.location.origin + '/Images/Icons/Outlook/word.png';
                break;
            case '.xls':
            case '.xlsx':
                icon = window.location.origin + '/Images/Icons/Outlook/excel.png';
                break;
            case '.ppt':
            case '.pptx':
                icon = window.location.origin + '/Images/Icons/Outlook/ppt.png';
                break;
            case '.pdf':
                icon = window.location.origin + '/Images/Icons/Outlook/pdf.png';
                break;
            case '.txt':
                icon = window.location.origin + '/Images/Icons/Outlook/txt.png';
                break;
            case '.msi':
            case '.exe':
                icon = window.location.origin + '/Images/Icons/Outlook/exe.png';
                break;
            case '.mp3':
            case '.m3u':
            case '.wav':
            case '.wma':
            case '.mid':
            case '.midi':
            case '.vqf':
            case '.flac':
                icon = window.location.origin + '/Images/Icons/Outlook/mp3.png';
                break;
            case '.mp4':
            case '.avi':
            case '.wmv':
            case '.rmvb':
            case '.rm':
            case '.flash':
            case '.3gp':
            case '.mkv':
                icon = window.location.origin + '/Images/Icons/Outlook/video.png';
                break;
            case '.jpg':
            case '.jpeg':
            case '.gif':
            case '.png':
            case '.bmp':
            case '.pic':
            case '.ico':
                icon = window.location.origin + '/Images/Icons/Outlook/pic.png';
                break;
            case '.zip':
            case '.rar':
                icon = window.location.origin + '/Images/Icons/Outlook/zip.png';
                break;
            default:
                icon = window.location.origin + '/Images/Icons/Outlook/default.png';
                break;
        }
        return icon;
    };

    // 256M
    var LENGTH = 256 * 1024 * 1024;

    /**
     * 上传大文件块
     * @param {any} fileSize
     * @param {any} start
     * @param {any} end
     * @param {any} postFile
     * @param {any} fileName
     * @param {any} filePath
     * @param {any} ondup
     * @param {any} totalPieces
     * @param {any} currentPieces
     * @param {any} guid
     * @param {any} callback
     */
    var uploadBigFile = function (fileSize, start, end, postFile, totalPieces, currentPieces, fileInfo, partsInfo, callback) {

        end = start + LENGTH;
        if (end > fileSize) {
            end = fileSize;
        }

        var chunk = postFile.slice(start, end);//切割文件

        var postFD = new FormData();
        postFD.append('TokenId', getAccessToken());
        postFD.append('FileName', encodeURIComponent(fileInfo.name));
        postFD.append('Docid', encodeURIComponent(fileInfo.docid));
        postFD.append('Rev', encodeURIComponent(fileInfo.rev));
        postFD.append('UploadId', encodeURIComponent(fileInfo.uploadid));
        postFD.append('Start', currentPieces);
        postFD.append('End', totalPieces);
        postFD.append('PartsInfo', encodeURIComponent(partsInfo));
        postFD.append('PostFile', chunk);

        $.ajax({
            url: '/Outlook/SaveBigFileToServer',
            type: 'post',
            data: postFD,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (data) {
                if (data.Success) {
                    var code = data.StatusCode;
                    if (code == 0) {
                        start = end;
                        currentPieces++;
                        // 上传结果
                        var resData = new Function('return ' + data.Data)();
                        partsInfo = resData.FileName;
                        if (start == fileSize) {
                            uploadBigFileSend(fileInfo, partsInfo, callback);
                        } else {
                            uploadBigFile(fileSize, start, end, postFile, totalPieces, currentPieces, fileInfo, partsInfo, callback);
                        }
                    } else {
                        // token过期再次上传
                        if (code == 401001001) {
                            uploadBigFile(fileSize, start, end, postFile, totalPieces, currentPieces, fileInfo, partsInfo, callback);
                        } else {
                            if (OutlookErrorCodeNotice(code)) {
                                callback(data);
                            }
                        }
                    }
                } else {
                    callback();
                }
            },
            error: function (ex) {
                console.error(ex);
                callback();
            }
        });
    };

    /**
     * 上传大文件发送
     * @param {any} fileInfo
     * @param {any} partsInfo
     * @param {any} callback
     */
    var uploadBigFileSend = function (fileInfo, partsInfo, callback) {

        var postFD = new FormData();
        postFD.append('TokenId', getAccessToken());
        postFD.append('FileName', encodeURIComponent(fileInfo.name));
        postFD.append('Docid', encodeURIComponent(fileInfo.docid));
        postFD.append('Rev', encodeURIComponent(fileInfo.rev));
        postFD.append('UploadId', encodeURIComponent(fileInfo.uploadid));
        postFD.append('PartsInfo', encodeURIComponent(partsInfo));

        $.ajax({
            url: '/Outlook/SaveBigFileSend',
            type: 'post',
            data: postFD,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (data) {
                // 错误检测
                var code = data.StatusCode;
                if (OutlookErrorCodeNotice(code)) {
                    callback(data);
                }
            },
            error: function (ex) {
                console.error(ex);
                callback();
            }
        });
    };

    /**
     * 添加到邮件内容
     * @param {string} name 名称
     * @param {number} length
     * @param {string} url
     * @param {string} fileId
     */
    anyshare.outlook.addCloudFiles = function (name, length, url, fileId, password, anonymousType) {
        //获取多语言文本
        getLanguageTitle();
        var icon;
        if (length < 0) {
            icon = window.location.origin + '/Images/Icons/Outlook/folder.png';
            length = anyshare.outlook.getDirSize(fileId);
        } else {
            icon = getFileIcon(name);
        }

        // 构造链接
        url = window.asUrl + "/link/" + url;
        var pswContent = "";
        if (password) {
            pswContent = password;
            url = url + passwordL + "：" + password
        }

        // 共享类型
        var shareTypeText = withUsersL;
        if (anonymousType) {
            shareTypeText = withAnyoneL;
        }

        var lengthStr = length + 'B';
        if (length >= 1024 * 1024 * 1024) {
            lengthStr = (length / (1024 * 1024 * 1024)).toFixed(1) + 'GB';
        } else if (length >= 1024 * 1024) {
            lengthStr = (length / (1024 * 1024)).toFixed(1) + 'MB';
        } else if (length >= 1024) {
            lengthStr = (length / 1024).toFixed(1) + 'KB';
        }

        var hideTR = 'id="append_attach_tr_hidden"><td colspan=5 style=\'border:none;padding:1.0pt 1.0pt 1.0pt 1.0pt\'></td></tr>';

        var addTR = '><td style="border:none;padding:10pt 2pt 2pt 2pt"><p style="max-width: 400px;display: inline-block;white-space: nowrap;overflow: hidden;text-overflow: ellipsis;font-size: 9.5pt;font-family: Microsoft YaHei;" ><span><img alt="Picture" width = "24" src="' +
            icon + '" />&nbsp;</span><span>&nbsp;' + name + '</span></p></td>' +
            '<td style="border:none;padding:2pt 5pt"><p><span style="font-size: 9.5pt;font-family: Microsoft YaHei;">' +
            lengthStr + '</span></p></td>' +
            '<td style="border:none;padding:2pt 5pt"><p><span style="font-size: 9.5pt;font-family: Microsoft YaHei;">' +
            shareTypeText + '</span></p></td>' +
            '<td style="border:none;padding:2pt 5pt"><p><span style="font-size: 9.5pt;font-family: Microsoft YaHei;">' +
            pswContent + '</span></p></td>' +
            '<td style="border:none;padding:2pt"><p><a style="font-size: 9.5pt;font-family: Microsoft YaHei;" href="' + url + '">' +
            detailsL + '</a><p></td>' +
            '</tr><tr ' + hideTR;

        Office.context.mailbox.item.body.getAsync(
            'html',
            { asyncContext: addTR },
            function callback(result) {
                var resultHtml = result.value;
                var resultTR = result.asyncContext;

                resultHtml = encodeURIComponent(resultHtml);
                resultTR = encodeURIComponent(resultTR);
                hideTR = encodeURIComponent(hideTR);

                var onlineTR1 = 'id%3D%22x_append_attach_tr_hidden%22%3E%0A%3Ctd%20colspan%3D%225%22%20style%3D%22border%3Anone%3B%20padding%3A1.0pt%201.0pt%201.0pt%201.0pt%22%3E%3C%2Ftd%3E%0A%3C%2Ftr%3E';
                var onlineTR2 = 'id%3D%22x_append_attach_tr_hidden%22%3E%0A%3Ctd%20style%3D%22padding%3A%201pt%3B%20border%3A%20currentColor%3B%20border-image%3A%20none%3B%22%20colspan%3D%225%22%3E%3C%2Ftd%3E%0A%3C%2Ftr%3E';
                var onlineTR3 = 'id%3D%22x_append_attach_tr_hidden%22%3E%0A%3Ctd%20style%3D%22border%3Anone%3B%20padding%3A1.0pt%201.0pt%201.0pt%201.0pt%22%20colspan%3D%225%22%3E%3C%2Ftd%3E%0A%3C%2Ftr%3E';

                if (resultHtml.indexOf(hideTR) > -1 || resultHtml.indexOf(onlineTR1) > -1 || resultHtml.indexOf(onlineTR2) > -1 || resultHtml.indexOf(onlineTR3) > -1) {
                    var repalceHtml = null;
                    if (resultHtml.indexOf(onlineTR1) > -1) {
                        repalceHtml = resultHtml.replace(onlineTR1, resultTR);
                    } else if (resultHtml.indexOf(onlineTR2) > -1) {
                        repalceHtml = resultHtml.replace(onlineTR2, resultTR);
                    } else if (resultHtml.indexOf(onlineTR3) > -1) {
                        repalceHtml = resultHtml.replace(onlineTR3, resultTR);
                    } else {
                        repalceHtml = resultHtml.replace(hideTR, resultTR);
                    }
                    Office.context.mailbox.item.body.setAsync(
                        decodeURIComponent(repalceHtml),
                        { coercionType: 'html' },
                        function callback(result) { }
                    );
                } else {
                    var addHtml =
                        '<table width="800" style="border:1px solid #eee" class="append_attach_tb">' +
                        '<tr><td colspan=5 style="border:none;"><img alt="Picture" width="100%" style="position: relative;" src="' +
                        getBgImage() + '" /></td></tr><tr>' +
                        '<td style="border:none;padding:2pt 5pt"><p><span style="font-size: 9.5pt;font-weight: 700;font-family: Microsoft YaHei;">' +
                        urlTitleL + '</span></p></td>' +
                        '<td style="border:none;padding:2pt 5pt"><p><span style="font-size: 9.5pt;font-weight: 700;font-family: Microsoft YaHei;">' +
                        sizeL + '</span></p></td>' +
                        '<td style="border:none;padding:2pt 5pt"><p><span style="font-size: 9.5pt;font-weight: 700;font-family: Microsoft YaHei;">' +
                        shareTypeL + '</span></p></td>' +
                        '<td style="border:none;padding:2pt 5pt"><p><span style="font-size: 9.5pt;font-weight: 700;font-family: Microsoft YaHei;">' +
                        passwordL + '</span></p></td>' +
                        '<td style="border:none;padding:2pt 5pt"></td>' +
                        '</tr><tr' + decodeURIComponent(resultTR) +
                        '</table>';
                    Office.context.mailbox.item.body.prependAsync(
                        addHtml,
                        { coercionType: 'html' },
                        function callback(result) { }
                    );
                }
            }
        );
    };

    /** 是否支持2019高版本 */
    anyshare.outlook.isSupportType = function () {
        try {
            if (Office.context.requirements.isSetSupported('Mailbox', '1.8')) {
                return true;
            } else {
                return false;
            }
        } catch (ex) {
            return false;
        }
    };

    /**
     * 添加附件到Outlook
     * @param {any} name
     * @param {any} base64
     */
    anyshare.outlook.addAttachment = function (name, base64, callback) {
        Office.context.mailbox.item.addFileAttachmentFromBase64Async(
            base64,
            name,
            function (result) {
                callback(result.status == Office.AsyncResultStatus.Succeeded);
            }
        );
    };

    /**
     * 获取文件夹大小
     * @param {string} docId 文件夹id
     */
    anyshare.outlook.getDirSize = function (docId, error) {
        var data = new FormData();
        data.append('TokenId', getAccessToken());
        data.append('Docid', encodeURIComponent(docId));

        var length = 0;
        $.ajax({
            url: '/Outlook/GetDirSize',
            type: 'post',
            data: data,
            processData: false,
            contentType: false,
            async: false,
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    var code = response.StatusCode;
                    if (code == 0) {
                        var model = new Function('return ' + response.Data)();
                        length = model.totalsize;
                    } else {
                        if (OutlookErrorCodeNotice(code)) {
                            if (error) {
                                error(code);
                            }
                        }
                    }
                }
            },
            error: function (ex) {
                console.error(ex);
            }
        });
        return length;
    };

    /**
     * 保存文件
     * @param {number} ondup 1 检查冲突 2 保留两种 3 替换
     * @param {string} fileName 文件名
     * @param {string} fileBase64
     * @param {string} filePath
     * @param {Function} callback
     */
    anyshare.outlook.uploadFileToServer = function (ondup, fileName, postFile, filePath, callback) {
        var fileSize = postFile.size;

        //if (fileSize / LENGTH > 20) {
        //    callback({ Success: true, StatusCode: 401402403, Data: "" }); // 5G
        //}
        // 文件大于1.75G, 分块上传
        if (fileSize / LENGTH > 7) {

            var postFD = new FormData();
            postFD.append('TokenId', getAccessToken());
            postFD.append('FileName', encodeURIComponent(fileName));
            postFD.append('Docid', encodeURIComponent(filePath));
            postFD.append('Ondup', ondup);
            postFD.append('FileLength', fileSize);

            $.ajax({
                url: '/Outlook/SaveBigFileInit',
                type: 'post',
                data: postFD,
                processData: false,
                contentType: false,
                dataType: 'json',
                success: function (data) {
                    if (data.Success) {
                        var code = data.StatusCode;
                        if (code == 0) {
                            var resData = new Function('return ' + data.Data)();
                            var fileInfo = new Function('return ' + resData.FileName)();

                            //计算文件切片总数
                            var totalPieces = Math.ceil(fileSize / LENGTH);
                            // 分块上传
                            uploadBigFile(fileSize, 0, 0, postFile, totalPieces, 1, fileInfo, "", callback);
                        } else {
                            if (OutlookErrorCodeNotice(code)) {
                                callback(data);
                            }
                        }
                    } else {
                        callback();
                    }
                },
                error: function (ex) {
                    console.error(ex);
                    callback();
                }
            });
        } else {
            var formData = new FormData();
            formData.append('TokenId', getAccessToken());
            formData.append('FileName', encodeURIComponent(fileName));
            formData.append('Docid', encodeURIComponent(filePath));
            formData.append('Ondup', ondup);
            formData.append('PostFile', postFile);

            $.ajax({
                url: '/Outlook/SaveFileToServer',
                type: 'post',
                data: formData,
                processData: false,
                contentType: false,
                dataType: 'json',
                success: function (data) {
                    // 错误检测
                    var code = data.StatusCode;
                    if (OutlookErrorCodeNotice(code)) {
                        callback(data);
                    }
                },
                error: function (ex) {
                    console.error(ex);
                    callback();
                }
            });
        }
    };

    /**
     * 获取实名共享 ShareLink
     * @param {string} docId
     * @param {string} fileType
     */
    anyshare.outlook.getRealNameShareLink = function (docId, fileType, callback) {
        var formdata = new FormData();
        formdata.append('TokenId', getAccessToken());
        formdata.append('Docid', encodeURIComponent(docId));
        formdata.append('FileType', fileType);

        $.ajax({
            url: '/Outlook/GetRealNameShareLinkId',
            type: 'post',
            data: formdata,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (data) {
                // 错误检测
                var code = data.StatusCode;
                if (OutlookErrorCodeNotice(code)) {
                    callback(data);
                }
            },
            error: function (ex) {
                console.error(ex);
                callback();
            }
        });
    };

    /** SHARELINK ***************************************************/

    var getShareLinkConfig = function (callback, error) {
        // var config = undefined;
        var data = new FormData();
        data.append('TokenId', getAccessToken());

        $.ajax({
            url: '/Outlook/GetShareLinkConfig',
            type: 'post',
            data: data,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    var code = response.StatusCode;
                    if (code == 0) {
                        callback(new Function('return ' + response.Data)());
                        return;
                    } else {
                        // 错误检测
                        if (OutlookErrorCodeNotice(code)) {
                            if (error) {
                                error(code);
                            }
                        }
                    }
                } else {
                    callback({});
                }
            },
            error: function (ex) {
                console.error(ex);
                callback({});
            }
        });
        // return config;
    };

    var getShareLinkSwitch = function (callback, error) {
        var data = new FormData();
        data.append('TokenId', getAccessToken());

        $.ajax({
            url: '/Outlook/GetShareLinkSwitch',
            type: 'post',
            data: data,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    var code = response.StatusCode;
                    if (code == 0) {
                        var switches = new Function('return ' + response.Data)();
                        callback(switches);
                        return;
                    } else {
                        // 错误检测
                        if (OutlookErrorCodeNotice(code)) {
                            if (error) {
                                error(code);
                            }
                        }
                    }
                } else {
                    callback({});
                }
            },
            error: function (ex) {
                console.error(ex);
                callback({});
            }
        });
    };
    /**
     * 检查用户是否被冻结
     * @param {function} callback
     */
    var checkUser = function (callback, error) {
        var data = {
            token: getAccessToken(),
        };
        $.ajax({
            url: '/Word/GetLoginUser',
            type: 'post',
            data: JSON.stringify(data),
            processData: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    var code = response.StatusCode;
                    if (code == 0) {
                        var user = new Function('return ' + response.Data)();
                        callback(user.freezestatus);
                        return;
                    } else {
                        // 错误检测
                        if (OutlookErrorCodeNotice(code)) {
                            if (error) {
                                error(code);
                            }
                        }
                    }
                } else {
                    callback(true);
                }
            },
            error: function (ex) {
                console.error(ex);
                callback(true);
            }
        });
    };
    /**
     * 检查是否为本人文档
     * @param {string} docId 文档id
     */
    var checkOwner = function (docId, callback, error) {
        var data = new FormData();
        data.append('TokenId', getAccessToken());
        data.append('Docid', encodeURIComponent(docId));

        $.ajax({
            url: '/Outlook/CheckOwner',
            type: 'post',
            data: data,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    var code = response.StatusCode;
                    if (code == 0) {
                        var owner = new Function('return ' + response.Data)();
                        callback(owner.isowner);
                        return;
                    } else {
                        // 错误检测
                        if (OutlookErrorCodeNotice(code)) {
                            if (error) {
                                error(code);
                            }
                        }
                    }
                } else {
                    callback(false);
                }
            },
            error: function (ex) {
                console.error(ex);
                callback(false);
            }
        });
    };

    anyshare.sharelink = anyshare.sharelink || {};
    anyshare.sharelink.strategy = {};
    anyshare.sharelink.initialized = false;
    anyshare.sharelink.initialize = function (callback, error) {
        // if (!anyshare.sharelink.initialized) {
        getShareLinkConfig(function (config) {
            getShareLinkSwitch(function (switches) {
                if (config && switches) {
                    anyshare.sharelink.strategy = {
                        config: config,
                        switches: switches,
                    };
                    anyshare.sharelink.initialized = true;
                    callback({ config: config, switches: switches });
                    return;
                }
                if (error) {
                    error(-1);
                }
            }, error);
        }, error);
        // }
    };

    anyshare.sharelink.check = function (docId, callback, error) {
        checkUser(function (freezeStatus) {
            checkOwner(
                docId,
                function (isOwner) {
                    console.log('callback');
                    callback(freezeStatus, isOwner);
                },
                error
            );
        }, error);
    };

    /** SYSTEM **************************************************************************************/

    anyshare.system = anyshare.system || {};
    anyshare.system.alert = layer_alert;
    anyshare.system.success = layer_msg;
    anyshare.system.notice = layer_msg_notice;
})();
