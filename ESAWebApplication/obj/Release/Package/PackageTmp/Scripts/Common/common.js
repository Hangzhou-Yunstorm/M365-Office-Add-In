// anyshare 前缀
var anysharePath = "AnyShare://";

// ==============================新建文件夹==============================

/**
 * 新建文件夹
 * @param {any} gnsId
 * @param {any} name
 */
function CreateDir(gnsId, name) {
    var dataObject = {
        token: getAccessToken(),
        gnsId: gnsId,
        name: name
    };
    $.ajax({
        url: "/Word/CreateDir",
        type: "post",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataObject),
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var code = data.StatusCode;
                if (code == 0) {
                    layer_msg(languageText.CreatedSuccess);
                    closeCreateDir();
                    reloadDirs();
                } else if (code == 403002039) {
                    showCreateError(languageText.HadSameFolder);
                } else if (code == 403001002 || code == 403002056) {
                    layer_alert(languageText.NoNewFolderPer);
                } else {
                    layer_alert(languageText.UnknownError + code);
                }
            } else {
                SystemError();
                console.log(data.Message);
            }
        },
        error: function (e) {
            SystemError();
        }
    });
}

/**
 * 显示错误信息
 */
function showCreateError(error) {
    $("#iptDirName").addClass("yc-error");
    $("#create-error").text(error);
    $("#create-error").show();
}

// 清除创建错误信息
function removeCreateError() {
    $("#iptDirName").removeClass("yc-error");
    $("#create-error").text("");
    $("#create-error").hide();
}

/**显示新建文件夹 */
function showCreateDir() {
    $("#select-path-body").hide();
    $("#create-container").show();
    $("#create-back").show();
}

/**关闭新建文件夹 */
function closeCreateDir() {
    $("#iptDirName").val("");
    removeCreateError();
    $("#createDirBtn").attr("disabled", true);
    $("#create-back").hide();
    $("#create-container").hide();
    $("#select-path-body").show();
}

/**输入文件夹名称 */
function onDirNameChange() {
    var dirName = $("#iptDirName").val();
    console.log(dirName);
    dirName = dirName.trim();
    if (dirName != null && dirName != "") {
        var err = validateName(dirName, false);
        if (err == "") {
            $("#createDirBtn").attr("disabled", false);
            removeCreateError();
        } else {
            $("#createDirBtn").attr("disabled", true);
            showCreateError(err);
        }
    } else {
        $("#createDirBtn").attr("disabled", true);
        removeCreateError();
    }
}

/**输入文件名称 */
function onFileNameChange() {
    var fileName = $("#fileName").val();
    console.log(fileName);
    fileName = fileName.trim();
    if (fileName != null && fileName != "") {
        $("#confirmSelect").attr("disabled", false);
    } else {
        $("#confirmSelect").attr("disabled", true);
    }
}


/**确认创建文件夹 */
function onCreateDir() {
    var gnsId = $("#folderUrl").val();
    var name = $("#iptDirName").val();
    // 创建文件夹
    CreateDir(gnsId, name);
}

// 检测名称合法性
function validateName(name, isFile) {
    let UIText = UIStrings.getLocaleStrings();
    if (name.length > 255) {
        return isFile ? UIText.FileMax255 : UIText.Max255;
    } else if (/[\\/\\:\\*\\?\\"\\<\\>\\|\\]/g.test(name)) {
        return isFile ? UIText.FileNoSpecialChar : UIText.NoSpecialChar;
    }
    return "";
}

// 顶级目录
function loadTopDirs() {
    let UIText = UIStrings.getLocaleStrings();
    __dirs = [
        { id: "user_doc_lib", name: UIText.PersonalDoc, icon: "person" },
        { id: "shared_user_doc_lib", name: UIText.ShareDoc, icon: "share" },
        { id: "department_doc_lib", name: UIText.DepartmentDoc, icon: "groupfolder" },
        { id: "custom_doc_lib", name: UIText.DocLib, icon: "document" }
    ];
}

// 点击任务地方隐藏面包屑
function HideCrumbs() {
    $(document).bind("click", function (e) {
        var target = $(e.target);
        //点击id为#之外的地方触发
        if (target.closest("#dropdown").length == 0 && target.closest("#history-container").length == 0) {
            if (__crumbs_isopened) {
                hideCrumbs();
            }
        }
    })
}

/**
 * 获取 type 对应的名称
 * @param {any} typeId
 */
function getNameByTypeId(typeId) {
    for (var i = 0; i < __dirs.length; i++) {
        if (__dirs[i].id == typeId) {
            return __dirs[i].name;
        }
    }
    return "";
}

/**无搜索结果 */
function resultIsEmpty() {
    return "<div class='empty'><img src='/Images/Icons/SaveAndOpen/searchempty.png'/><p>" + languageText.NoSearchResult + "</p></div>";
}

/**加载中 */
function resultIsLoading() {
    return "<div class='empty'><img src='/Images/Icons/SaveAndOpen/loading.gif'/><p>" + languageText.Loading + "</p></div>";
}

/**无子文件夹 */
function noChild() {
    return "<div class='empty-container'><div class='empty'><img src='/Images/Icons/SaveAndOpen/nochild.png'/><p>" + languageText.NoChildFolder + "</p></div></div>";
}

/**无子文件 */
function noChildFile() {
    return "<div class='empty-container'><div class='empty'><img src='/Images/Icons/SaveAndOpen/emptyfolder.png'/><p>" + languageText.NoChildFile + "</p></div></div>";
}

/**
 * 获取文件大小（格式化）
 * @param {any} size
 */
function getFileSize(size) {
    var filesize = size + "B";
    if (size >= 1024 * 1024 * 1024) {
        filesize = (size / (1024 * 1024 * 1024)).toFixed(1) + "GB";
    } else if (size >= 1024 * 1024) {
        filesize = (size / (1024 * 1024)).toFixed(1) + "MB";
    } else if (size >= 1024) {
        filesize = (size / 1024).toFixed(1) + "KB";
    }
    return filesize;
}