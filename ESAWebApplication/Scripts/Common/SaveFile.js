// 检测是否登录
CheckLogin();

var languageText;

(function () {
    "use strict";

    // 每次加载新页面时都必须运行初始化函数。
    Office.onReady(function () {
        $(document).ready(function () {

            var index_load = layer_load();

            // 获取文件属性
            getFileProp();

            // 点击任意地方隐藏面包屑
            HideCrumbs();

            // 语言包
            languageText = UIStrings.getLocaleStrings();

            // 顶级目录
            loadTopDirs();

            //设置页面中的多语言
            SaveFileLanguage();

            // 是否显示默认开关
            if (isShowDefault()) {
                var defaultFolder = localStorage.getItem("defaultFolderName");
                var defaultFolderId = localStorage.getItem("defaultFolderId");
                $("#defaultFolderName").val(defaultFolder);
                $("#defaultFolderUrl").val(defaultFolderId);

                $("#ToDefaultPath").text(defaultFolder);
                $("#ToDefaultPath").attr('title', defaultFolder);
            } else {
                //$("#ToDefaultPath").hide();
                $("#ToDefaultPath").text(languageText.NoDefaultDir);
                $("#ToDefaultPath").attr('title', languageText.NoDefaultDir);
            }

            layer.close(index_load);
        });
    });

})();

// 设置默认类型
function setDefaultSelType(selType, asFileName) {
    $("input[name=TypeSelect][value=" + selType + "]").prop("checked", "checked");
    layui.use(['form'], function () {
        var layuiform = layui.form;
        layuiform.on("radio(TypeSelect)", function (data) {
            var seltype = data.value;
        });
        layuiform.render();
    });

    // 文件名称
    var fileName = Office.context.document.url;
    if (fileName == null || fileName == "") {
        if (selType == 1) {
            $("#fileName").val(asFileName);
        } else {
            $("#fileName").val(__default_file_name);
        }
    } else {
        if (fileName.indexOf("\\") > -1) {
            var subIndex = fileName.lastIndexOf("\\") + 1;
            fileName = fileName.substring(subIndex);
        }
        if (fileName.indexOf("/") > -1) {
            var subIndex = fileName.lastIndexOf("/") + 1;
            fileName = fileName.substring(subIndex);
        }
        $("#fileName").val(fileName);
    }
}

// 确认选择
function confirmSelect() {
    var fileName = $("#fileName").val();
    var err = validateName(fileName, true);
    if (err != "") {
        layer_msg_notice(err);
        return;
    }

    var typeSel = $("input[name='TypeSelect']:checked").val();
    if (typeSel == "1") {
        saveFile(1, 3);
    } else if (typeSel == "2") {
        var defaultFolderId = $("#defaultFolderUrl").val();
        if (defaultFolderId) {
            saveFile(2, 1);
        } else {
            goToSettings();
        }
    } else if (typeSel == "3") {
        $(".file_type_select").hide();
        $(".SelectTypePath").hide();
        $(".SelectSavePath").show();
        $(".search-box").show();
        $(".all_folder").show();
        $("#history-container").show();
        if (!__cancel_footer_hidden) {
            $("#save-footer").show();
        }
    }
}

// 获取文档库内容
function GetFilePath(fileId) {
    var formdata = new FormData();
    formdata.append('TokenId', getAccessToken());
    formdata.append('Docid', encodeURIComponent(fileId));

    $.ajax({
        url: "/Word/GetFilePath",
        type: "post",
        data: formdata,
        processData: false,
        contentType: false,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var code = data.StatusCode;
                if (code == 0) {
                    // 文件名
                    var fileFullPath = data.Data;
                    // 文件夹
                    var folderName = fileFullPath;
                    if (fileFullPath.indexOf("/") > -1) {
                        var subIndex = fileFullPath.lastIndexOf("/");
                        folderName = fileFullPath.substring(0, subIndex);
                        fileFullPath = fileFullPath.substring(subIndex + 1);
                    }
                    // 文件所在文件夹ID
                    var folderId = fileGnsId;
                    var folderIndex = folderId.lastIndexOf("/");
                    if (folderIndex > 5) {
                        folderId = folderId.substring(0, folderIndex);
                    }
                    $("#ASFileFolderUrl").val(folderId);
                    $("#ASFileFolderName").val(folderName);
                    $("#ASFilePath").text(anysharePath + folderName);
                    $("#ASFilePath").attr('title', anysharePath + folderName);

                    setDefaultSelType(1, fileFullPath);
                } else {
                    // Local file
                    setDefaultSelType(2);
                    $("#AS_File").hide();
                }
            } else {
                // Local file
                setDefaultSelType(2);
                $("#AS_File").hide();
            }
        },
        error: function (e) {
            console.log("Error:" + e);
        }
    });
}

// 是否显示默认开关
function isShowDefault() {
    var isShow = false;
    // 默认文件路径开关
    var defaultFolderCKB = localStorage.getItem("defaultFolderCKB");
    if (defaultFolderCKB && defaultFolderCKB == "1") {
        //初始话默认的文件路径
        var defaultFolder = localStorage.getItem("defaultFolderName");
        var defaultFolderId = localStorage.getItem("defaultFolderId");
        if (defaultFolder && defaultFolderId) {
            isShow = true;
        }
    }
    return isShow;
}

var index_layer;
var Select_Type;

// The following example gets the document in Office Open XML ("compressed") format in 65536 bytes (64 KB) slices, Max: 4194304 (4M).
// Note: The implementation of console.log in this example is from the Visual Studio template for Office Add-ins.
/**
 * 保存文件
 * @param {any} toSelectType 选择类型
 */
function saveFile(toSelectType, ondup, fileName) {

    var folderUrl = null;
    var folderName = null;
    if (toSelectType == 1) {
        folderUrl = $("#ASFileFolderUrl").val();
        folderName = $("#ASFileFolderName").val();
    } else if (toSelectType == 2) {
        folderUrl = $("#defaultFolderUrl").val();
        folderName = $("#defaultFolderName").val();
    } else if (toSelectType == 3) {
        folderUrl = $("#folderUrl").val();
        folderName = $("#folderName").val();
    }

    if (!folderUrl) {
        layer_msg_notice('Please select Folder.');
        return;
    }
    Select_Type = toSelectType;

    $("#saveFolderUrl").val(folderUrl);
    $("#saveFolderName").val(folderName);

    index_layer = layer_load();

    if (!fileName) {
        fileName = $("#fileName").val();
    }
    var fileType = Office.FileType.Compressed;
    var index1 = fileName.lastIndexOf('.');
    if (index1 >= 1) {
        var fileExt = fileName.substring(index1, fileName.length);
        if (fileExt.toLocaleLowerCase() == ".pdf") {
            fileType = Office.FileType.Pdf;
        }
    }

    Office.context.document.getFileAsync(fileType, { sliceSize: 4194304 },
        function (result) {
            if (result.status == Office.AsyncResultStatus.Succeeded) {
                // If the getFileAsync call succeeded, then result.value will return a valid File Object.
                var myFile = result.value;
                var sliceCount = myFile.sliceCount;
                console.log("File size:" + myFile.size + " ,Slices count: " + sliceCount);

                var postFD = new FormData();
                postFD.append('TokenId', getAccessToken());
                postFD.append('FileName', encodeURIComponent(fileName));
                postFD.append('Docid', encodeURIComponent(folderUrl));
                postFD.append('Ondup', ondup);
                postFD.append('FileLength', myFile.size);

                $.ajax({
                    url: '/Word/SaveBigFileInit',
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

                                // Get the file slices.
                                getSliceAsync(myFile, 0, sliceCount, 1, "", fileInfo);

                            } else {
                                var uploadData = new Function('return ' + data.Data)();
                                if (code == 403002039) {
                                    // 新名称
                                    var newFileName = uploadData.FileName;
                                    $("#newFileName").val(newFileName)
                                    showDialog(newFileName);
                                } else if (code == 403001031) {
                                    // 被锁定
                                    var message = new Function('return ' + uploadData.ErrorDetail)();
                                    var errorMsg = String.format(languageText.FileLocked, fileName, message.locker);
                                    layer_alert(errorMsg);
                                } else if (code == 403002070) {
                                    // 大小限制
                                    var message = new Function('return ' + uploadData.ErrorDetail)();
                                    var errorMsg = String.format(languageText.FileSizeLimit, fileName, getFileSize(message.file_limit_size));
                                    layer_alert(errorMsg);
                                } else {
                                    ErrorCodeNotice(code, fileName, $("#saveFolderName").val());
                                }
                                myFile.closeAsync();
                                layer.close(index_layer);
                            }
                        } else {
                            myFile.closeAsync();
                            layer.close(index_layer);
                            SystemError();
                        }
                    },
                    error: function (ex) {
                        myFile.closeAsync();
                        console.error(ex);
                        layer.close(index_layer);
                        SystemError();
                    }
                });
            }
            else {
                console.log("Error:" + result.error.message);
                layer.close(index_layer);
                SystemError();
            }
        });
}

// 分开获取文件
function getSliceAsync(file, nextSlice, sliceCount, slicesReceived, partsInfo, fileInfo) {
    console.log("Slice:" + slicesReceived + " Of " + sliceCount);

    file.getSliceAsync(nextSlice, function (sliceResult) {
        if (sliceResult.status == Office.AsyncResultStatus.Succeeded) {

            var postFD = new FormData();
            postFD.append('TokenId', getAccessToken());
            postFD.append('FileName', encodeURIComponent(fileInfo.name));
            postFD.append('Docid', encodeURIComponent(fileInfo.docid));
            postFD.append('Rev', encodeURIComponent(fileInfo.rev));
            postFD.append('UploadId', encodeURIComponent(fileInfo.uploadid));
            postFD.append('Start', slicesReceived);
            postFD.append('End', sliceCount);
            postFD.append('PartsInfo', encodeURIComponent(partsInfo));
            postFD.append('Base64Str', base64EncArr(sliceResult.value.data));

            $.ajax({
                url: '/Word/SaveBigFileToServer',
                type: 'post',
                data: postFD,
                processData: false,
                contentType: false,
                dataType: 'json',
                success: function (data) {
                    if (data.Success) {
                        var code = data.StatusCode;
                        if (code == 0) {
                            // 上传结果
                            var resData = new Function('return ' + data.Data)();
                            partsInfo = resData.FileName;

                            // Got one slice, store it in a temporary array.  (Or you can do something else, such as  send it to a third-party server.)
                            if (slicesReceived == sliceCount) {
                                // All slices have been received.
                                file.closeAsync();
                                // Send File
                                uploadBigFileSend(fileInfo, partsInfo);
                            } else {
                                slicesReceived++;
                                getSliceAsync(file, ++nextSlice, sliceCount, slicesReceived, partsInfo, fileInfo);
                            }
                        } else {
                            layer.close(index_layer);
                            SystemError();
                        }
                    } else {
                        layer.close(index_layer);
                        SystemError();
                    }
                },
                error: function (ex) {
                    console.error(ex);
                    layer.close(index_layer);
                    SystemError();
                }
            });
        }
        else {
            file.closeAsync();
            layer.close(index_layer);
            SystemError();
            console.log("getSliceAsync Error:" + sliceResult.error.message);
        }
    });
}

function uploadBigFileSend(fileInfo, partsInfo) {

    var postFD = new FormData();
    postFD.append('TokenId', getAccessToken());
    postFD.append('FileName', encodeURIComponent(fileInfo.name));
    postFD.append('Docid', encodeURIComponent(fileInfo.docid));
    postFD.append('Rev', encodeURIComponent(fileInfo.rev));
    postFD.append('UploadId', encodeURIComponent(fileInfo.uploadid));
    postFD.append('PartsInfo', encodeURIComponent(partsInfo));

    $.ajax({
        url: '/Word/SaveBigFileSend',
        type: 'post',
        data: postFD,
        processData: false,
        contentType: false,
        dataType: 'json',
        success: function (data) {
            // 错误检测
            var code = data.StatusCode;
            if (code == 0) {
                layer_msg_save(languageText.SaveSuccess);
            } else {
                ErrorCodeNotice(code, fileInfo.name, $("#saveFolderName").val());
            }
            layer.close(index_layer);
        },
        error: function (ex) {
            console.error(ex);
            layer.close(index_layer);
            SystemError();
        }
    });
}

/**保留两者 */
function keepBoth() {
    index_layer = layer_load();

    hideDialog();
    var newFileName = $("#newFileName").val();
    saveFile(Select_Type, 2, newFileName)
}

/**替换 */
function replace() {
    index_layer = layer_load();

    hideDialog();
    saveFile(Select_Type, 3);
}

/**跳过 */
function skip() {
    hideDialog();
}

// 显示弹出框
function showDialog(newFileName) {
    $("#newFileNameText").text(newFileName);
    $("#saveFileDialog").show();
    $("#saveFileDialogMask").show();
}

// 隐藏弹出框
function hideDialog() {
    $("#saveFileDialog").hide();
    $("#saveFileDialogMask").hide();
}


var __historyDirs = [];
var __dirs = [];
var __crumbs_isopened = false;

// 获取文档库内容
function OpenLib(_type) {
    var index_load = layer_load();
    var dataObject = {
        token: getAccessToken(),
        type: _type
    };
    $.ajax({
        url: "/Word/GetEntryDocLibs",
        type: "post",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataObject),
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var code = data.StatusCode;
                if (code == 0) {
                    var libs = new Function('return ' + data.Data)();
                    PushHistory(_type, getNameByTypeId(_type));
                    RenderChildDirs(libs);
                } else {
                    ErrorCodeNotice(code);
                }
            } else {
                SystemError();
                console.log(data.Message);
            }
            layer.close(index_load);
        },
        error: function (e) {
            layer.close(index_load);
        }
    });
}

// 获取文件夹内容
function GetDocLibsById(gnsId) {
    var index_load = layer_load();
    var dataObject = {
        token: getAccessToken(),
        gnsId: gnsId
    };
    $.ajax({
        url: "/Word/GetDocLibsById",
        type: "post",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataObject),
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var code = data.StatusCode;
                if (code == 0) {
                    ShowSaveFooter(true);
                    $("#create-btn").attr("disabled", false);
                    var libs = new Function('return ' + data.Data)();
                    var dirs = libs.dirs.map(function (i) { return ({ id: i.docid, name: i.name }) });
                    RenderChildDirs(dirs)
                } else {
                    ErrorCodeNotice(code);
                }
            } else {
                SystemError();
                console.log(data.Message);
            }
            layer.close(index_load);
        },
        error: function (e) {
            SystemError();
            layer.close(index_load);
        }
    });
}

/**
 * 渲染文档库子级目录
 */
function RenderChildDirs(dirs) {
    RenderCrumbs();
    $("#dirs").empty();
    if (dirs.length && dirs.length > 0) {
        $.each(dirs, function (index, dir) {
            var id = dir.id;
            var name = dir.name;
            var li = document.createElement("li");
            var container = document.createElement("div");
            container.className = "li_div";
            container.title = name;
            container.addEventListener("click", function () {
                hideCrumbs();
                PushHistory(id, name);
                GetDocLibsById(id);
            });

            var icon = document.createElement("div");
            icon.className = "first_img";
            var img = document.createElement("img");
            img.src = "/Images/Icons/SaveAndOpen/folder.png";
            icon.appendChild(img)

            var content = document.createElement("div");
            content.className = "first_a yc-text-overflow";
            var link = document.createElement("a");
            link.href = "javascript:void(0);";
            link.innerText = name;
            content.appendChild(link)

            container.appendChild(icon);
            container.appendChild(content);
            li.appendChild(container);
            document.getElementById("dirs").appendChild(li);
        });
    } else {
        $("#dirs").html(noChild());
    }
}

/**渲染顶级文档库 */
function RenderTopDirs() {
    RenderCrumbs();
    $("#dirs").empty();
    $.each(__dirs, function (index, dir) {
        var id = dir.id;
        var name = dir.name;
        var _icon = dir.icon;
        var li = document.createElement("li");
        var container = document.createElement("div");
        container.className = "li_div";
        container.addEventListener("click", function () {
            OpenLib(id)
        });

        var icon = document.createElement("div");
        icon.className = "first_img";
        var img = document.createElement("img");
        img.src = "/Images/Icons/SaveAndOpen/" + _icon + ".png";
        icon.appendChild(img)

        var content = document.createElement("div");
        content.className = "first_a";
        var link = document.createElement("a");
        link.href = "javascript:void(0);";
        link.className = "";
        link.innerText = name;
        content.appendChild(link)

        container.appendChild(icon);
        container.appendChild(content);
        li.appendChild(container);
        document.getElementById("dirs").appendChild(li);
    });
}

// 显示保存文件按钮
function ShowSaveFooter(isShowCreateBtn) {
    $("#save-footer").show();
    if (isShowCreateBtn) {
        $("#CreatFolderBtn").show();
    } else {
        $("#CreatFolderBtn").hide();
    }
}

// 隐藏保存文件按钮
function HideSaveFooter() {
    $("#save-footer").hide();
}

/**渲染面包屑 */
function RenderCrumbs() {
    $("#history-container").empty();
    var history = document.getElementById("history-container");
    var length = __historyDirs.length;
    if (length > 0) {
        var last = __historyDirs[length - 1];

        var current = document.createElement("div");
        current.className = "current";

        var back = document.createElement("img");
        back.src = "/Images/Icons/SaveAndOpen/back.png";
        back.className = "back-icon";
        back.addEventListener("click", GoBack)
        current.appendChild(back);

        var currentDir = document.createElement("span");
        currentDir.innerText = last.name;
        currentDir.className = "current-dir yc-text-overflow";
        var arrow = document.createElement("img");
        arrow.src = "/Images/Icons/SaveAndOpen/select.png";
        arrow.className = "arrow";
        currentDir.appendChild(arrow);
        currentDir.addEventListener("click", function () {
            if (__crumbs_isopened) {
                $("#dropdown").hide();
            } else {
                var $crumbs = $("#dropdown .crumbs");
                $crumbs.empty();
                var html = "";
                for (var i = 0; i < __historyDirs.length; i++) {
                    if (i === __historyDirs.length - 1) {
                        html += '<div class="crumbs-item" title="' + __historyDirs[i].name + '"><div class="dir-item yc-text-overflow"><img src="/Images/Icons/SaveAndOpen/folder.png" />' + __historyDirs[i].name + '</div></div>';
                    } else {
                        html += '<div class="crumbs-item" title="' + __historyDirs[i].name + '"><div onclick="GoToDir(\'' + __historyDirs[i].id + '\', ' + i + ')" class="dir-item yc-text-overflow"><img src="/Images/Icons/SaveAndOpen/folder.png" />' + __historyDirs[i].name + '</div><img class="connector" src="/Images/Icons/SaveAndOpen/back.png" /></div>';
                    }
                }
                $crumbs.html(html);
                $("#dropdown").show();
            }

            __crumbs_isopened = !__crumbs_isopened;
        });
        current.appendChild(currentDir);
        history.appendChild(current);
    } else {
        document.getElementById("history-container").innerHTML = "<span class=\"current-dir AllDoc\">" + languageText.AllDoc + "</span>";
    }

}

/**返回上一层 */
function GoBack() {
    $("#dropdown").hide();
    __historyDirs.pop();
    var length = __historyDirs.length;
    if (length === 1) {
        var _type = __historyDirs[0].id;
        ClearHistory();
        OpenLib(_type);
        HideSaveFooter();
        $("#create-btn").attr("disabled", true);
    } else if (length > 1) {
        var last = __historyDirs[length - 1]
        GetDocLibsById(last.id);
        SetFolderValues(last.id, last.name);
    } else {
        ClearHistory();
        RenderTopDirs();
        HideSaveFooter();
        $("#create-btn").attr("disabled", true);
    }
    RenderCrumbs();
}

/**
 * 选择历史记录中的某个路径
 */
function GoToDir(id, level) {
    hideCrumbs();
    if (level < 0) {
        ClearHistory();
        RenderTopDirs();
        HideSaveFooter();
        $("#create-btn").attr("disabled", true);
    }
    else if (level === 0) {
        ClearHistory();
        OpenLib(id);
        HideSaveFooter();
        $("#create-btn").attr("disabled", true);
    } else {
        __historyDirs = __historyDirs.slice(0, level + 1);
        var length = __historyDirs.length;
        var last = __historyDirs[length - 1]
        GetDocLibsById(id);
        SetFolderValues(last.id, last.name);
    }
}

// 加入到点击历史
function PushHistory(id, name) {
    var hd = __historyDirs.filter(function (item, index, array) {
        return item.id == id;
    });
    if (hd == null || hd.length == 0) {
        __historyDirs.push({ id: id, name: name });
    }
    SetFolderValues(id, name);
}

// 清空点击历史
function ClearHistory() {
    __historyDirs = [];
    SetFolderValues("", "");
}

// 设置目录信息
function SetFolderValues(url, name) {
    // 跳过顶级目录
    if (url != "" && url.indexOf("gns://") < 0) {
        return;
    }
    $("#folderUrl").val(url);
    $("#folderName").val(name);
}

/**重新加载当前文件夹列表 */
function reloadDirs() {
    var url = $("#folderUrl").val();
    if (url.indexOf("gns") == 0) {
        GetDocLibsById(url);
    } else {
        OpenLib(url);
    }
}

/**隐藏导航栏 */
function hideCrumbs() {
    if (__crumbs_isopened) {
        $("#dropdown").hide();
        __crumbs_isopened = false;
    }
}

// ===========================搜索文件夹==============================

/**搜索操作中 */
var __searching = false;
/**搜索结果临时保存 */
var __search_results = null;
/**搜索此目录 */
var __search_gnsId = "";
/**搜索此目录 */
var __search_gnsName = "";
/**搜索关键字 */
var __search_key = "";
/**通过搜索结果进入的目录记录 */
var __search_dirs = [];
/**进入搜索前的创建按钮是否不可用 */
var __create_btn_disabled = true;
/**进入搜索前底部保存按钮是否隐藏 */
var __save_footer_hidden = true;

/**搜索框获取到焦点 */
function beginSearch() {
    __searching = true;
    hideCrumbs();
    __create_btn_disabled = $("#create-btn").is(":disabled");
    __save_footer_hidden = $("#save-footer").is(":hidden");
    $("#img-back").show();
    $("#img-home").hide();
    $("#create-btn").attr("disabled", true);
    HideSaveFooter();
    $("#search-results").show();
}

/**搜索文件夹 */
function onSearch() {
    var gnsId = $("#folderUrl").val();
    var gnsName = $("#folderName").val();
    var val = $("#searchText").val().trim();
    if (val != "") {
        $("#img-cancel").show();
        __search_gnsId = gnsId;
        __search_gnsName = gnsName;

        Search(gnsId, val);
    } else {
        __search_results = null;
        $("#img-cancel").hide();
        $("#search-results").empty();
    }
}

/**
 * 搜索文件夹
 * @param {any} gnsId
 * @param {any} key
 */
function Search(gnsId, key) {
    __search_key = key;
    $("#search-results").html(resultIsLoading());
    var dataObject = {
        token: getAccessToken(),
        gnsId: gnsId,
        key: key,
        doctype: 2
    };
    $.ajax({
        url: "/Word/Search",
        type: "post",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataObject),
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var code = data.StatusCode;
                if (code == 0) {
                    var results = new Function('return ' + data.Data)();
                    __search_results = results;
                    renderSearchResults();
                } else {
                    ErrorCodeNotice(code);
                    $("#search-results").html(resultIsEmpty());
                }
            } else {
                SystemError();
                $("#search-results").html(resultIsEmpty());
                console.log(data.Message);
            }
        },
        error: function (e) {
            SystemError();
            $("#search-results").html(resultIsEmpty());
        }
    });
}

/**清除搜索框与结果 */
function clearSearch() {
    __search_dirs = [];
    __search_key = "";
    __search_results = null;
    $("#searchText").val("");
    $("#img-cancel").hide();
    $("#search-results").empty();
}

/**搜索中点击返回 */
function searchBack() {
    if (__search_dirs.length > 0) {
        __search_dirs.pop();
        var length = __search_dirs.length;
        if (length >= 1) {
            var last = __search_dirs.pop();
            openSearchResult(last);
        } else {
            $("#folderUrl").val(__search_gnsId);
            $("#folderName").val(__search_gnsName);
            HideSaveFooter();
            // 返回到搜索结果
            renderSearchResults();
        }
    } else {
        __searching = false;
        $("#folderUrl").val(__search_gnsId);
        $("#folderName").val(__search_gnsName);
        $("#img-back").hide();
        $("#img-home").show();
        $("#searchText").val("");
        $("#img-cancel").hide();
        $("#search-results").empty();
        $("#search-results").hide();
        $("#create-btn").attr("disabled", __create_btn_disabled);
        if (false == __save_footer_hidden) {
            ShowSaveFooter(true);
        } else {
            HideSaveFooter();
        }
    }
}

/**
 * 渲染搜索结果
 * @param {any} results
 */
function renderSearchResults() {
    $("#search-results").empty();
    if (__search_results == null || __search_results.length <= 0) {
        $("#search-results").html(resultIsEmpty());
    } else {
        var ul = document.createElement("ul");
        ul.className = "results";
        $.each(__search_results, function (index, doc) {
            var name = doc.basename;
            var hlbasename = doc.hlbasename;
            var docid = doc.docid;
            var parentPath = doc.parentpath.replace("gns://", "");

            var li = document.createElement("li");
            li.addEventListener("click", function () {
                openSearchResult(docid);
            });
            li.innerHTML = '<div class="result-item"><img src="/Images/Icons/SaveAndOpen/folder.png" /><div class="result-content"><div class="result-name yc-text-overflow" title="' + name + '">' + hlbasename + '</div><div class="result-parent yc-text-overflow" title="' + parentPath + '">' + parentPath + '</div></div></div>';
            ul.appendChild(li);
        });
        document.getElementById("search-results").appendChild(ul);
    }
}

// 打开搜索结果
function openSearchResult(gnsId) {
    var index_load = layer_load();
    var dataObject = {
        token: getAccessToken(),
        gnsId: gnsId
    };
    $.ajax({
        url: "/Word/GetDocLibsById",
        type: "post",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataObject),
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var code = data.StatusCode;
                if (code == 0) {
                    ShowSaveFooter(false);
                    $("#folderUrl").val(gnsId);
                    $("#folderName").val(name);
                    __search_dirs.push(gnsId);
                    var libs = new Function('return ' + data.Data)();
                    var dirs = libs.dirs.map(function (i) { return ({ id: i.docid, name: i.name }) });
                    renderResultChildDirs(dirs)
                } else {
                    ErrorCodeNotice(code);
                }
            } else {
                SystemError();
                console.log(data.Message);
            }
            layer.close(index_load);
        },
        error: function (e) {
            SystemError();
            layer.close(index_load);
        }
    });
}

// 渲染搜索结果信息
function renderResultChildDirs(dirs) {
    $("#search-results").empty();
    if (dirs.length && dirs.length > 0) {
        var ul = document.createElement("ul");
        ul.className = "first_ul";
        $.each(dirs, function (index, dir) {
            var id = dir.id;
            var name = dir.name;
            var li = document.createElement("li");
            var container = document.createElement("div");
            container.className = "li_div";
            container.setAttribute("title", name);
            container.addEventListener("click", function () {
                openSearchResult(id);
            });

            var icon = document.createElement("div");
            icon.className = "first_img";
            var img = document.createElement("img");
            img.src = "/Images/Icons/SaveAndOpen/folder.png";
            icon.appendChild(img)

            var content = document.createElement("div");
            content.className = "first_a yc-text-overflow";
            var link = document.createElement("a");
            link.href = "javascript:void(0);";
            link.innerText = name;
            content.appendChild(link)

            container.appendChild(icon);
            container.appendChild(content);
            li.appendChild(container);
            ul.appendChild(li);
        });
        document.getElementById("search-results").appendChild(ul);
    } else {
        $("#search-results").html(noChild());
    }
}


/**返回主页前底部保存按钮是否隐藏 */
var __cancel_footer_hidden = true;

/**取消 */
function cancel() {
    // 取消搜索操作
    if (__searching) {
        __search_dirs = [];
        searchBack();
    } else {
        // 取消
        __cancel_footer_hidden = $("#save-footer").is(":hidden");
        $(".file_type_select").show();
        $(".SelectTypePath").show();
        $(".SelectSavePath").hide();
        $(".search-box").hide();
        $(".all_folder").hide();
        $("#history-container").hide();
        $("#save-footer").hide();
    }
}
