// 检测是否登录
CheckLogin();

// 语言包
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
            loadTopDirs(languageText);

            //设置页面中的多语言
            openFileLanguage();
        });
    });

})();

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
                    renderChildDirsAndFiles(libs, []);
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
        gnsId: gnsId,
        officeType: __docType ? __docType : ""
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
                    var dataDocs = new Function('return ' + data.Data)();
                    var dirs = dataDocs.dirs.map(function (i) { return ({ id: i.docid, name: i.name }) });
                    var files = dataDocs.files.map(function (i) { return ({ id: i.docid, name: i.name, isClick: isClick(i.name) }) });
                    renderChildDirsAndFiles(dirs, files)
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

// 是否可用类型
function isClick(fileName) {
    var fileExt;
    var index1 = fileName.lastIndexOf('.');
    //获取字符串长度
    if (index1 >= 1) {
        fileExt = fileName.substring(index1, fileName.length);
    }
    if (fileExt == ".docx" && __docType == "word") {
        return true;
    } else if (fileExt == ".xlsx" && __docType == "excel") {
        return true;
    } else if (fileExt == ".pptx" && __docType == "ppt") {
        return true;
    } else {
        return false;
    }
}

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
            icon = '/Images/Icons/SaveAndOpen/word.png';
            break;
        case '.xls':
        case '.xlsx':
            icon = '/Images/Icons/SaveAndOpen/excel.png';
            break;
        case '.ppt':
        case '.pptx':
            icon = '/Images/Icons/SaveAndOpen/ppt.png';
            break;
        case '.pdf':
            icon = '/Images/Icons/SaveAndOpen/pdf.png';
            break;
        case '.txt':
            icon = '/Images/Icons/SaveAndOpen/txt.png';
            break;
        case '.msi':
        case '.exe':
            icon = '/Images/Icons/SaveAndOpen/exe.png';
            break;
        case '.mp3':
        case '.m3u':
        case '.wav':
        case '.wma':
        case '.mid':
        case '.midi':
        case '.vqf':
        case '.flac':
            icon = '/Images/Icons/SaveAndOpen/mp3.png';
            break;
        case '.mp4':
        case '.avi':
        case '.wmv':
        case '.rmvb':
        case '.rm':
        case '.flash':
        case '.3gp':
        case '.mkv':
            icon = '/Images/Icons/SaveAndOpen/video.png';
            break;
        case '.jpg':
        case '.jpeg':
        case '.gif':
        case '.png':
        case '.bmp':
        case '.pic':
        case '.ico':
            icon = '/Images/Icons/SaveAndOpen/pic.png';
            break;
        case '.zip':
        case '.rar':
            icon = '/Images/Icons/SaveAndOpen/zip.png';
            break;
        default:
            icon = '/Images/Icons/SaveAndOpen/default.png';
            break;
    }
    return icon;
};

/**
 * 渲染子级目录中的文件夹或文件
 */
function renderChildDirsAndFiles(dirs, files) {
    renderCrumbs();
    $("#dirs").empty();
    if (dirs.length <= 0 && files.length <= 0) {
        $("#dirs").html(noChildFile());
    } else {
        $.each(dirs, function (index, item) {
            var id = item.id;
            var name = item.name;
            var li = document.createElement("li");
            var container = document.createElement("div");
            container.className = "li_div";
            container.title = name;
            container.addEventListener("click", function () {
                hideCrumbs();
                pushHistory(id, name);
                getDocLibsById(id);
                showOpenFile();

                $("#openFileUrl").val("");
                $("#openBtn").attr("disabled", true);
            });

            var icon = document.createElement("div");
            icon.className = "first_img";
            var img = document.createElement("img");
            img.src = "/Images/Icons/SaveAndOpen/folder.png";
            icon.appendChild(img);

            var content = document.createElement("div");
            content.className = "first_a yc-text-overflow";
            var link = document.createElement("a");
            link.href = "javascript:void(0);";
            link.innerText = name;
            content.appendChild(link);

            container.appendChild(icon);
            container.appendChild(content);
            li.appendChild(container);
            document.getElementById("dirs").appendChild(li);
        });

        files = sortFile(files);
        $.each(files, function (index, item) {
            var id = item.id;
            var name = item.name;
            var li = document.createElement("li");
            var container = document.createElement("div");

            container.title = name;
            if (item.isClick) {
                container.className = "li_div";
                container.addEventListener("click", function () {
                    $("#dirs .li_div.checked").removeClass("checked");
                    hideCrumbs();
                    container.className += " checked";

                    // 可点击
                    $("#openFileUrl").val(id);
                    $("#openFileName").val(name);
                    $("#openBtn").attr("disabled", false);
                });
            } else {
                container.className = "li_div dis_click";
            }

            var icon = document.createElement("div");
            icon.className = "first_img";
            var img = document.createElement("img");
            img.src = getFileIcon(name);
            icon.appendChild(img);

            var content = document.createElement("div");
            content.className = "first_a yc-text-overflow";
            var link = document.createElement("a");
            link.href = "javascript:void(0);";
            link.innerText = name;
            content.appendChild(link);

            container.appendChild(icon);
            container.appendChild(content);
            li.appendChild(container);

            document.getElementById("dirs").appendChild(li);
        });
    }
}

// 文件排序（可点击显示在前面）
function sortFile(files) {
    function compare(property) {
        return function (obj1, obj2) {
            return obj2[property] - obj1[property];
        }
    }
    return files.sort(compare('isClick'));
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
    } else if (length > 1) {
        var last = __historyDirs[length - 1]
        getDocLibsById(last.id);
        setFolderValues(last.id, last.name);
    } else {
        clearHistory();
        renderTopDirs();
        hideOpenFile();
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
    }
    else if (level === 0) {
        clearHistory();
        openLib(id);
        hideOpenFile();
    } else {
        __historyDirs = __historyDirs.slice(0, level + 1);
        var length = __historyDirs.length;
        var last = __historyDirs[length - 1]
        getDocLibsById(id);
        setFolderValues(last.id, last.name);
    }
}

/**显示底部按钮区 */
function showOpenFile() {
    $("#open-footer").show();
}

/**隐藏底部按钮区 */
function hideOpenFile() {
    $("#open-footer").hide();
}


// 搜索文件夹
// ==================================================================
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
var __open_btn_disabled = true;
/**进入搜索前底部保存按钮是否隐藏 */
var __open_footer_hidden = true;
/**进入搜索前已选中文件 */
var __open_file_url = "";
var __open_file_name = "";

/**搜索框获取到焦点 */
function beginSearch() {
    __open_file_url = $("#openFileUrl").val();
    __open_file_name = $("#openFileName").val();
    $("#search-results .result-item.checked").removeClass("checked");
    __searching = true;
    hideCrumbs();
    __open_btn_disabled = $("#openBtn").is(":disabled");
    __open_footer_hidden = $("#open-footer").is(":hidden");
    $("#img-back").show();
    $("#img-home").hide();
    $("#openBtn").attr("disabled", true);
    if (__search_key == '') {
        $("#open-footer").hide();
    }
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
        __search_results = "";
        $("#open-footer").hide();
        $("#img-cancel").hide();
        $("#search-results").empty();
        $("#openBtn").attr("disabled", true);
        $("#openFileUrl").val("");
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
        doctype: 3,
        officeType: __docType
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
    $("#openBtn").attr("disabled", true);
    $("#openFileUrl").val("");
    hideOpenFile();
}

/**搜索中点击返回 */
function searchBack() {
    if (__search_dirs.length > 0) {
        __search_dirs.pop();
        $("#openBtn").attr("disabled", true);
        $("#openFileUrl").val("");
        var length = __search_dirs.length;
        if (length >= 1) {
            var last = __search_dirs.pop();
            openSearchResult(last);
        } else {
            $("#folderUrl").val(__search_gnsId);
            $("#folderName").val(__search_gnsName);
            $("#save-footer").hide();
            // 返回到搜索结果
            renderSearchResults();
        }
    } else { // 退出搜索
        __searching = false;
        __search_key = "";
        $("#openFileUrl").val(__open_file_url);
        $("#openFileName").val(__open_file_name);
        $("#img-back").hide();
        $("#img-home").show();
        $("#searchText").val("");
        $("#img-cancel").hide();
        $("#search-results").empty();
        $("#search-results").hide();
        $("#openBtn").attr("disabled", __open_btn_disabled);
        if (false == __open_footer_hidden) {
            showOpenFile();
        } else {
            hideOpenFile();
        }
    }
}

/**
 * 渲染搜索结果
 * @param {any} results
 */
function renderSearchResults() {
    $("#search-results").empty();
    if (__search_results == null ||
        __search_results.length <= 0) {
        $("#openBtn").attr("disabled", true);
        $("#openFileUrl").attr("");
        $("#open-footer").hide();

        $("#search-results").html(resultIsEmpty());
    } else {
        $("#openBtn").attr("disabled", true);
        $("#openFileUrl").attr("");
        $("#open-footer").show();

        var ul = document.createElement("ul");
        ul.className = "results";
        $.each(__search_results, function (index, doc) {
            var size = doc.size;
            var name = doc.basename;
            var hlbasename = doc.hlbasename;
            var id = doc.docid;
            var parentPath = doc.parentpath.replace("gns://", "");

            var li = document.createElement("li");
            if (size == -1) {
                li.addEventListener("click", function () {
                    openSearchResult(id);
                });
                li.innerHTML = '<div class="result-item"><img src="/Images/Icons/SaveAndOpen/folder.png" /><div class="result-content"><div class="result-name yc-text-overflow" title="' + name + '">' + hlbasename + '</div><div class="result-parent yc-text-overflow" title="' + parentPath + '">' + parentPath + '</div></div></div>';
            } else {
                var div = document.createElement("div");
                div.className = "result-item";
                li.addEventListener("click", function () {
                    $("#search-results .result-item.checked").removeClass("checked");
                    div.className += " checked";
                    $("#openFileUrl").val(id);
                    $("#openFileName").val(name);
                    $("#openBtn").attr("disabled", false);
                });
                div.innerHTML = '<img src="/Images/Icons/SaveAndOpen/' + __docType + '.png" /><div class="result-content"><div class="result-name yc-text-overflow" title="' + name + '">' + hlbasename + '</div><div class="result-parent yc-text-overflow" title="' + parentPath + '">' + parentPath + '</div></div>';
                li.appendChild(div);
            }

            ul.appendChild(li);
        });
        document.getElementById("search-results").appendChild(ul);
    }
}

/**
 * 打开搜索结果（文件夹）
 * @param {any} gnsId
 */
function openSearchResult(gnsId) {
    var index_load = layer_load();
    var dataObject = {
        token: getAccessToken(),
        gnsId: gnsId,
        officeType: __docType
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
                    __search_dirs.push(gnsId);
                    var libs = new Function('return ' + data.Data)();
                    var dirs = libs.dirs.map(function (i) { return ({ id: i.docid, name: i.name }) });
                    var files = libs.files.map(function (i) { return ({ id: i.docid, name: i.name }) });
                    renderResultChildDirsAndFiles(dirs, files)
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
 * 渲染结果文件夹和文件
 * @param {any} dirs
 * @param {any} files
 */
function renderResultChildDirsAndFiles(dirs, files) {
    $("#search-results").empty();
    if (dirs.length <= 0 && files.length <= 0) {
        $("#search-results").html(noChildFile());
    } else {
        var ul = document.createElement("ul");
        ul.className = "first_ul";

        // 文件夹
        $.each(dirs, function (index, dir) {
            var id = dir.id;
            var name = dir.name;
            var li = document.createElement("li");
            var container = document.createElement("div");
            container.className = "li_div";
            container.setAttribute("title", name);
            container.addEventListener("click", function () {
                $("#openFileUrl").val("");
                $("#openBtn").attr("disabled", true);
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
        // 文件
        $.each(files, function (index, dir) {
            var id = dir.id;
            var name = dir.name;
            var li = document.createElement("li");
            var container = document.createElement("div");
            container.className = "li_div";
            container.setAttribute("title", name);
            container.addEventListener("click", function () {
                $("#search-results .li_div.checked").removeClass("checked");
                container.className += " checked";
                $("#openFileUrl").val(id);
                $("#openFileName").val(name);
                $("#openBtn").attr("disabled", false);
            });

            var icon = document.createElement("div");
            icon.className = "first_img";
            var img = document.createElement("img");
            img.src = "/Images/Icons/SaveAndOpen/" + __docType + ".png";
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
    }
}

/**取消 */
function cancel() {
    // 取消搜索操作
    if (__searching) {
        __search_dirs = [];
        searchBack();
    } else {
        // 取消
    }
}