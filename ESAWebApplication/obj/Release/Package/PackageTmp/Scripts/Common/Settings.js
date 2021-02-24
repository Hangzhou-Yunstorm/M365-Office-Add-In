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

            // 设置语言
            SetLanguage();

            // 设置用户信息
            SetUserInfo();

            // 设置页面
            SettingsLanguage();

            // 设置默认信息
            SetDefault();

        });
    });

})();

// 设置默认信息
function SetDefault() {
    //初始话默认的文件路径开关
    var defaultFolderCKB = localStorage.getItem("defaultFolderCKB");
    if (defaultFolderCKB && defaultFolderCKB == "1") {
        $("#defaultFolderCKB").prop("checked", true);
        $("#addDefaultFolder").attr("disabled", false);
    } else {
        $("#defaultFolderCKB").prop("checked", false);
        $("#addDefaultFolder").attr("disabled", true);
    }

    //初始话默认的文件路径
    var defaultFolder = localStorage.getItem("defaultFolderName");
    var defaultFolderId = localStorage.getItem("defaultFolderId");
    if (defaultFolder && defaultFolderId) {
        $("#defaultFolderName").val(defaultFolder);
        $("#defaultFolderName").attr("title", defaultFolder);
        $("#defaultFolderUrl").val(defaultFolderId);
    } else {
        $("#defaultFolderName").val("");
        $("#defaultFolderName").attr("title", "");
        $("#defaultFolderUrl").val("");
    }

    layui.use(['form'], function () {
        var form = layui.form;
        form.on('select(languageSelect)', function (data) {
            $("#languageSelect").val(data.value);
            $("#settingBtn").attr("disabled", false);
        });

        form.on('checkbox(defaultFolderCKB)', function (data) {
            if (data.elem.checked) {
                $("#addDefaultFolder").attr("disabled", false);
            } else {
                $("#addDefaultFolder").attr("disabled", true);
            }
            $("#settingBtn").attr("disabled", false);
            $(".path_notice").hide();
        });
        form.render();
    });
}

// 设置用户信息
function SetUserInfo() {
    var dataObject = { token: getAccessToken() };
    $.ajax({
        url: "/Word/GetLoginUser",
        type: "post",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataObject),
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var code = data.StatusCode;
                if (code == 0) {
                    var user = new Function('return ' + data.Data)();
                    $(".user_name").text(user.name);
                    $(".user_account").text(user.account);
                } else {
                    ErrorCodeNotice(code);
                }
            } else {
                SystemError();
            }
        },
        error: function (e) {
            SystemError();
        }
    });
}

// 设置
function Settings() {
    // Show Settings
    $(".index_div").hide();
    $(".settings_div").show();
    $("#settingBtn").attr("disabled", true);
}

// 保存设置
function SaveSettings() {

    // 默认路径开关
    var defaultFolderCKB = $("#defaultFolderCKB").prop("checked");
    // 默认路径
    var defaultFolder = $("#defaultFolderName").val();
    var defaultFolderId = $("#defaultFolderUrl").val();

    if (defaultFolderCKB) {
        if (defaultFolder && defaultFolderId) {
            localStorage.setItem("defaultFolderCKB", "1");
        } else {
            $(".path_notice").show();
            return;
        }
    } else {
        localStorage.setItem("defaultFolderCKB", "0");
    }

    if (defaultFolder && defaultFolderId) {
        localStorage.setItem("defaultFolderName", defaultFolder);
        localStorage.setItem("defaultFolderId", defaultFolderId);
    }

    // 语言
    localStorage.setItem("currentLanguage", $("#languageSelect").val());

    // 语言包
    languageText = UIStrings.getLocaleStrings();

    // 顶级目录
    loadTopDirs();

    // 设置页面
    SettingsLanguage();

    // Close Settings
    CloseSettings(false);
}

function CloseSettings(isReload) {
    if (isReload) {
        // 设置语言
        SetLanguage();

        // 设置默认信息
        SetDefault();
    }

    $(".settings_div").hide();
    $(".index_div").show();
    $(".path_notice").hide();
}

// 选择路径
function SelectPath() {
    renderTopDirs();
    $(".settings_div").hide();
    $("#select-path-container").show();
}

// 在线帮助
function GoHelp() {
    var helpUrl = $("#help_url").val();
    try {
        Office.context.ui.openBrowserWindow(helpUrl);
    } catch (ex) {
        window.open(helpUrl);
    }
}

// 版本信息
function ShowVersion() {
    var mask = document.createElement("div");
    mask.id = "yc-mask";
    mask.className = "yc-mask";
    document.body.appendChild(mask);
    $("#versionDialog").show();
}

// 关闭版本信息
function HideVersion() {
    $("#yc-mask").remove();
    $("#versionDialog").hide();
}


/**打开文件夹 */
/**============================================================================================== */

/**历史记录 */
var __historyDirs = [];
/**顶级文档库 */
var __dirs = [];
/**导航栏是否打开 */
var __crumbs_isopened = false;

// 获取文档库内容
function openLib(_type) {
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
                    pushHistory(_type, getNameByTypeId(_type));
                    renderChildDirs(libs);
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

/** 获取文件夹内容*/
function getDocLibsById(gnsId) {
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
                    enableCreateBtn()
                    var name = __historyDirs.slice(1, __historyDirs.length).map(function (h) { return h.name; }).join("/")
                    onSelectChanged(gnsId, name);
                    var dataDocs = new Function('return ' + data.Data)();
                    var dirs = dataDocs.dirs.map(function (i) { return ({ id: i.docid, name: i.name }) });
                    renderChildDirs(dirs)
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
 * 渲染子级目录中的文件夹或文件
 */
function renderChildDirs(dirs) {
    renderCrumbs();
    $("#dirs").empty();
    if (dirs.length <= 0) {
        $("#dirs").html(noChild());
    } else {
        //var clickTimer = null;
        $.each(dirs, function (index, item) {
            var id = item.id;
            var name = item.name;
            var li = document.createElement("li");
            var container = document.createElement("div");
            container.className = "li_div";
            container.title = name;
            // 打开文件夹
            container.addEventListener("click", function () {
                hideCrumbs();
                pushHistory(id, name);
                getDocLibsById(id);
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
    }
}

/**渲染顶级文档库 */
function renderTopDirs() {
    renderCrumbs();
    $("#dirs").empty();
    $.each(__dirs, function (_, dir) {
        var id = dir.id;
        var name = dir.name;
        var _icon = dir.icon;
        //var { id, name } = dir;
        var li = document.createElement("li");
        var container = document.createElement("div");
        container.className = "li_div";
        container.addEventListener("click", function () {
            openLib(id)
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

/**渲染面包屑 */
function renderCrumbs() {
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
        back.addEventListener("click", goBack)
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
                        html += '<div class="crumbs-item" title="' + __historyDirs[i].name + '"><div onclick="goToDir(\'' + __historyDirs[i].id + '\', ' + i + ')" class="dir-item yc-text-overflow"><img src="/Images/Icons/SaveAndOpen/folder.png" />' + __historyDirs[i].name + '</div><img class="connector" src="/Images/Icons/SaveAndOpen/back.png" /></div>';
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

/**
 * 添加历史记录
 * @param {any} id
 * @param {any} name
 */
function pushHistory(id, name) {
    var hd = __historyDirs.filter(function (item, index, array) {
        return item.id == id;
    });
    if (hd == null || hd.length == 0) {
        __historyDirs.push({ id: id, name: name });
    }

    setFolderValues(id, name);
}

/**清除历史记录 */
function clearHistory() {
    __historyDirs = [];
    setFolderValues("", "");
}

/**
 * 设置
 * @param {any} url
 * @param {any} name
 */
function setFolderValues(url, name) {
    // 跳过顶级目录
    if (url != "" && url.indexOf("gns://") < 0) {
        return;
    }
    $("#folderUrl").val(url);
    $("#folderName").val(name);
}

/**隐藏导航栏 */
function hideCrumbs() {
    if (__crumbs_isopened) {
        $("#dropdown").hide();
        __crumbs_isopened = false;
    }
}


/**返回上一层 */
function goBack() {
    $("#dropdown").hide();
    __historyDirs.pop();
    $("#openBtn").attr("disabled", true);
    $("#openFileUrl").val("");
    var length = __historyDirs.length;
    if (length === 1) {
        var _type = __historyDirs[0].id;
        clearHistory();
        openLib(_type);
        hideOpenFile();
        disableCreateBtn();
    } else if (length > 1) {
        var last = __historyDirs[length - 1]
        getDocLibsById(last.id);
        setFolderValues(last.id, last.name);
    } else {
        clearHistory();
        renderTopDirs();
        hideOpenFile();
        disableCreateBtn();
    }
    renderCrumbs();
}

/**
 * 选择历史记录中的某个路径
 */
function goToDir(id, level) {
    hideCrumbs();
    $("#openBtn").attr("disabled", true);
    $("#openFileUrl").val("");
    if (level < 0) {
        clearHistory();
        renderTopDirs();
        hideOpenFile();
        disableCreateBtn();
    }
    else if (level === 0) {
        clearHistory();
        openLib(id);
        hideOpenFile();
        disableCreateBtn();
    } else {
        __historyDirs = __historyDirs.slice(0, level + 1);
        var length = __historyDirs.length;
        var last = __historyDirs[length - 1]
        getDocLibsById(id);
        setFolderValues(last.id, last.name);
    }
}

/**显示底部按钮区 */
function showOpenFile(isShowCreateBtn) {
    $("#select-path-footer").show();
    if (isShowCreateBtn) {
        $("#select-creatfolder").show();
    } else {
        $("#select-creatfolder").hide();
    }
}

/**隐藏底部按钮区 */
function hideOpenFile() {
    $("#select-path-footer").hide();
}

/**启用创建文件夹按钮 */
function enableCreateBtn() {
    $("#create-btn").attr("disabled", false);
}

/**禁用创建文件夹按钮 */
function disableCreateBtn() {
    $("#create-btn").attr("disabled", true);
}

/**
 * 选中文件夹改变
 * @param {any} id
 * @param {any} name
 */
function onSelectChanged(id, name) {
    $("#selectedFolderUrl").val(id);
    $("#selectedFolderName").val(name);
    if (id != "") {
        showOpenFile(true);
    } else {
        hideOpenFile();
    }

}

/**重新加载当前文件夹列表 */
function reloadDirs() {
    var url = $("#folderUrl").val();
    if (url.indexOf("gns") == 0) {
        getDocLibsById(url);
    } else {
        openLib(url);
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
var __footer_hidden = true;

/**搜索框获取到焦点 */
function beginSearch() {
    $("#dirs .li_div.checked").removeClass("checked");
    __searching = true;
    hideCrumbs();
    __create_btn_disabled = $("#create-btn").is(":disabled");
    __footer_hidden = $("#select-path-footer").is(":hidden");
    disableCreateBtn();
    hideOpenFile();
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
            openSearchResult(last.id, last.name);
        } else {
            $("#folderUrl").val(__search_gnsId);
            $("#folderName").val(__search_gnsName);
            // 返回到搜索结果
            renderSearchResults();
        }
    } else {
        exitSearch();
    }
}

/**
 * 渲染搜索结果
 * @param {any} results
 */
function renderSearchResults() {
    $("#search-results").empty();
    if (__search_results == null || __search_results.length <= 0) {
        hideOpenFile();
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
            li.className = "result-item";
            li.addEventListener("click", function () {
                openSearchResult(docid, parentPath + "/" + name);
            });
            li.innerHTML = '<div><img src="/Images/Icons/SaveAndOpen/folder.png" /><div class="result-content"><div class="result-name yc-text-overflow" title="' + name + '">' + hlbasename + '</div><div class="result-parent yc-text-overflow" title="' + parentPath + '">' + parentPath + '</div></div></div>';
            ul.appendChild(li);
        });
        document.getElementById("search-results").appendChild(ul);
    }
}

// 打开搜索结果
function openSearchResult(gnsId, name) {
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
                    onSelectChanged(gnsId, name);
                    showOpenFile(false);
                    $("#folderUrl").val(gnsId);
                    $("#folderName").val(name);
                    __search_dirs.push({ id: gnsId, name: name });
                    var libs = new Function('return ' + data.Data)();
                    var dirs = libs.dirs.map(function (i) { return ({ id: i.docid, name: i.name, parent: name }) });
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
        var clickTimer = null;
        $.each(dirs, function (index, dir) {
            var id = dir.id;
            var name = dir.name;
            var parent = dir.parent;
            var li = document.createElement("li");
            var container = document.createElement("div");
            container.className = "li_div";
            container.setAttribute("title", name);
            container.addEventListener("click", function () {
                openSearchResult(id, parent + "/" + name);
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


/**顶部返回按钮 */
function onBack() {
    onSelectChanged("", "");
    //  搜索中返回
    if (__searching) {
        exitSearch();
    } else {
        exitSelect();
    }
}

/**确定选择 */
function onSelected() {
    var url = $("#selectedFolderUrl").val();
    var name = $("#selectedFolderName").val();
    if (name.indexOf(anysharePath) < 0) {
        name = anysharePath + name;
    }

    $("#defaultFolderUrl").val(url);
    $("#defaultFolderName").val(name);
    $("#defaultFolderName").attr("title", name);
    $("#settingBtn").attr("disabled", false);
    $(".path_notice").hide();

    // 返回主设置页面
    if (__searching) {
        exitSearch();
    }
    exitSelect();
}

function exitSearch() {
    __searching = false;
    __search_results = [];
    __search_dirs = [];
    $("#folderUrl").val(__search_gnsId);
    $("#folderName").val(__search_gnsName);
    $("#searchText").val("");
    $("#img-cancel").hide();
    $("#search-results").empty();
    $("#search-results").hide();
    $("#create-btn").attr("disabled", __create_btn_disabled);
    if (false == __footer_hidden) {
        showOpenFile(true);
    } else {
        hideOpenFile();
    }
}

function exitSelect() {
    hideOpenFile();
    disableCreateBtn();
    $("#dirs").empty();
    clearHistory();
    $("#select-path-container").hide();
    $(".settings_div").show();
}

function cancel() {
    if (__searching) {
        $("#selectedFolderUrl").val(__search_gnsId);
        $("#selectedFolderName").val(__search_gnsName);
        exitSearch();
    } else {
        exitSelect();
    }
}