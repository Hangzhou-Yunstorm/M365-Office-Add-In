// 检测是否登录
CheckLogin();

var languageText;

(function () {
    "use strict";

    // 每次加载新页面时都必须运行初始化函数。
    Office.onReady(function () {
        $(document).ready(function () {
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
                //初始话默认的文件路径
                var defaultFolder = localStorage.getItem("defaultEmailFolderName");
                var defaultFolderId = localStorage.getItem("defaultEmailFolderUrl");
                $("#defaultFolderName").val(defaultFolder);
                $("#defaultFolderUrl").val(defaultFolderId);

                $("#ToDefaultPath").text(defaultFolder);
                $("#ToDefaultPath").attr('title', defaultFolder);
            } else {
                //$("#ToDefaultPath").hide();
                $("#ToDefaultPath").text(languageText.NoDefaultDir);
                $("#ToDefaultPath").attr('title', languageText.NoDefaultDir);
            }

            layui.use(['form'], function () {
                var layuiform = layui.form;
                layuiform.on("radio(TypeSelect)", function (data) {
                    var seltype = data.value;
                });
                layuiform.render();
            });

            // 文件名称
            var fileName = RemoveSpecialChar(Office.context.mailbox.item.subject);
            if (fileName == null || fileName == "") {
                $("#fileName").val(__default_file_name);
            } else {
                if (fileName.indexOf("\\") > -1) {
                    var subIndex = fileName.lastIndexOf("\\") + 1;
                    fileName = fileName.substring(subIndex);
                }
                if (fileName.indexOf("/") > -1) {
                    var subIndex = fileName.lastIndexOf("/") + 1;
                    fileName = fileName.substring(subIndex);
                }
                $("#fileName").val(fileName + ".eml");
            }
        });
    });

})();

// 确认选择
function confirmSelect() {
    var fileName = $("#fileName").val();
    var err = validateName(fileName, true);
    if (err != "") {
        layer_msg_notice(err);
        return;
    }

    var typeSel = $("input[name='TypeSelect']:checked").val();
    if (typeSel == "2") {
        var defaultFolderId = $("#defaultFolderUrl").val();
        if (defaultFolderId) {
            saveFile(true);
        } else {
            window.location.href = "/Outlook?ReturnUrl=/Outlook/SaveEmail";
        }
    } else if (typeSel == "3") {
        $(".file_type_select").hide();
        $(".SelectTypeEmailPath").hide();
        $(".SelectSavePath").show();
        $(".search-box").show();
        $(".all_folder").show();
        $("#history-container").show();
        if (!__cancel_footer_hidden) {
            $("#save-footer").show();
        }
    }
}

// 是否显示默认开关
function isShowDefault() {
    var isShow = false;
    // 默认文件路径开关
    var defaultFolderCKB = localStorage.getItem("defaultEmailFolderCKB");
    if (defaultFolderCKB && defaultFolderCKB == "1") {
        //初始话默认的文件路径
        var defaultFolder = localStorage.getItem("defaultEmailFolderName");
        var defaultFolderId = localStorage.getItem("defaultEmailFolderUrl");
        if (defaultFolder && defaultFolderId) {
            isShow = true;
        }
    }
    return isShow;
}

var index_layer;

/**
 * 
 * @param {any} toDefault 是否保存到默认路径
 */
function saveFile(toDefault) {
    var folderUrl = toDefault ? $("#defaultFolderUrl").val() : $("#folderUrl").val();
    var folderName = toDefault ? $("#defaultFolderName").val() : $("#folderName").val();
    if (!folderUrl) {
        layer_msg_notice('Please select Folder.');
        return;
    }
    $("#saveFolderUrl").val(folderUrl);
    $("#saveFolderName").val(folderName);

    var err = validateName($("#fileName").val(), true);
    if (err != "") {
        layer_msg_notice(err);
        return;
    }

    index_layer = layer_load();

    uploadFileToServer(1, $("#fileName").val());
}

// 上传文件到服务器
function uploadFileToServer(ondup, fileName) {

    Office.context.mailbox.getCallbackTokenAsync({ isRest: true }, function (result) {

        var dataObject = {
            EwsUrl: encodeURIComponent(Office.context.mailbox.ewsUrl),
            EwsId: encodeURIComponent(Office.context.mailbox.item.itemId),
            EwsToken: encodeURIComponent(result.value),
            FileName: encodeURIComponent(fileName),
            TokenId: getAccessToken(),
            Docid: encodeURIComponent($("#saveFolderUrl").val()),
            Ondup: ondup
        };

        $.ajax({
            url: "/Outlook/SaveEmailToServer",
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataObject),
            dataType: "json",
            success: function (data) {
                if (data.Success) {
                    var code = data.StatusCode;
                    var uploadData = new Function('return ' + data.Data)();
                    if (code == 0) {
                        layer_msg(languageText.SaveSuccess);
                    } else if (code == 403002039) {
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
                } else {
                    SystemError();
                    console.log(data.Message);
                }
                layer.close(index_layer);
            },
            error: function (ex) {
                SystemError();
                layer.close(index_layer);
            }
        });
    });
}

/**保留两者 */
function keepBoth() {
    index_layer = layer_load();

    hideDialog();
    var newFileName = $("#newFileName").val();
    uploadFileToServer(2, newFileName)
}

/**替换 */
function replace() {
    index_layer = layer_load();

    hideDialog();
    var fileName = $("#fileName").val();
    uploadFileToServer(3, fileName);
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
    // 是否显示默认开关
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
        $(".SelectTypeEmailPath").show();
        $(".SelectSavePath").hide();
        $(".search-box").hide();
        $(".all_folder").hide();
        $("#history-container").hide();
        $("#save-footer").hide();
    }
}
