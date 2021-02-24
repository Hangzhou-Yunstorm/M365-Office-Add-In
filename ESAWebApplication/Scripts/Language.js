// 设置页面
function SettingsLanguage() {
    let UIText = UIStrings.getLocaleStrings();
    $(".settings_sp").text(UIText.Settings);
    $(".settings_sp1").text(UIText.General);
    $(".help_sp").text(UIText.Help);
    $(".version_sp").text(UIText.Versions);
    $(".current_sp").text(UIText.VersionInfo);
    $(".publish_sp").text(UIText.PublishDate);
    $(".user_name_sp").text(UIText.UserName);
    $(".user_account_sp").text(UIText.Account);
    $(".btn-logout").val(UIText.LogOut);
    $("#settingBtn").val(UIText.SaveBtn);
    $("#cancelBtn").val(UIText.Cancel);
    $("#select-creatfolder").val(UIText.NewFolder);
    $(".SetDefaultFolder").text(UIText.SetDefaultFolder);
    $(".Add_Default_Path").text(UIText.AddDefaultPath);
    $(".SetLanguage").text(UIText.SetLanguage);
    $("#addDefaultFolder").val(UIText.AddDefaultFolder);
    $("#addDefaultEmailFolder").val(UIText.AddDefaultFolder);
    $("#addDefaultAttachmentFolder").val(UIText.AddDefaultFolder);
    $(".AllDoc").text(UIText.AllDoc);
    $(".PersonalDoc").text(UIText.PersonalDoc);
    $(".ShareDoc").text(UIText.ShareDoc);
    $(".DepartmentDoc").text(UIText.DepartmentDoc);
    $(".DocLib").text(UIText.DocLib);
    $("#searchText").attr('placeholder', UIText.Search);
    $("#iptDirName").attr('placeholder', UIText.PEnterFolderName);
    $("#createDirBtn").val(UIText.Confirm);
    $("#cancelDirBtn").val(UIText.Cancel);
    $(".NewFolder").text(UIText.NewFolder);
    $(".EnterFolderName").text(UIText.EnterFolderName);
    $("#select-btn").val(UIText.Confirm);
    $("#select-cancel").val(UIText.Cancel);
    $(".SetEmailDefaultFolder").text(UIText.SetEmailDefaultFolder);
    $(".SetAttachDefaultFolder").text(UIText.SetAttachDefaultFolder);
    $(".SetAttachSize").text(UIText.SetAttachSize);
    $(".SetAttachSizeNotice").text(UIText.SetAttachSizeNotice);
    $(".no_path_notice").text(UIText.NoDefaultPath);
    $("#defaultFolderName").attr('placeholder', UIText.NoDefaultPath);
    $("#defaultEmailFolderName").attr('placeholder', UIText.NoDefaultPath);
    $("#defaultAttachmentFolderName").attr('placeholder', UIText.NoDefaultPath);
}

// 保存页面
function SaveFileLanguage() {
    let UIText = UIStrings.getLocaleStrings();
    $("#name").text(UIText.Name);
    $(".SelectSavePath").text(UIText.SelectSavePath);
    $(".SelectTypePath").text(UIText.SaveNotice);
    $(".SelectTypeEmailPath").text(UIText.SaveMailNotice);
    $(".SelectTypeAttachmentPath").text(UIText.SaveAttachmentNotice);
    $("#TypeSelectTitle").text(UIText.YouCanSelect);
    $("#TypeSelectMailTitle").text(UIText.YouCanSelectMail);
    $("#TypeSelectAttachmentTitle").text(UIText.YouCanSelectAttachment);
    $(".AllDoc").text(UIText.AllDoc);
    $(".NewFolder").text(UIText.NewFolder);
    $(".EnterFolderName").text(UIText.EnterFolderName);
    $("#createDirBtn").val(UIText.Confirm);
    $("#confirmSelect").val(UIText.Confirm);
    $("#select-cancel").val(UIText.Cancel);
    $("#CreatFolderBtn").val(UIText.NewFolder);
    $("#cancelDirBtn").val(UIText.Cancel);
    $("#saveDefBtn").val(UIText.SaveToDefault);
    $("#saveBtn").val(UIText.SaveBtn);
    $("#cancelBtn").val(UIText.Cancel);
    $("#Replace").val(UIText.Replace);
    $("#SaveTwo").val(UIText.SaveTwo);
    $("#Return").val(UIText.Return);
    $(".HadSameFile").text(UIText.HadSameFile);
    $(".HadSameFileNotice").text(UIText.HadSameFileNotice);
    $(".PersonalDoc").text(UIText.PersonalDoc);
    $(".ShareDoc").text(UIText.ShareDoc);
    $(".DepartmentDoc").text(UIText.DepartmentDoc);
    $(".DocLib").text(UIText.DocLib);
    $("#searchText").attr('placeholder', UIText.Search);
    $("#iptDirName").attr('placeholder', UIText.PEnterFolderName);

    $("input[name=TypeSelect][value=1]").attr('title', UIText.SaveToAS);
    $("input[name=TypeSelect][value=2]").attr('title', UIText.SaveToDefault2);
    $("input[name=TypeSelect][value=3]").attr('title', UIText.SaveToSelect);
}

function openFileLanguage() {
    let UIText = UIStrings.getLocaleStrings();
    $(".openFileFromCloud").text(UIText.OpenFileFromServer);
    $(".AllDoc").text(UIText.AllDoc);
    $(".PersonalDoc").text(UIText.PersonalDoc);
    $(".ShareDoc").text(UIText.ShareDoc);
    $(".DepartmentDoc").text(UIText.DepartmentDoc);
    $(".DocLib").text(UIText.DocLib);
    $("#searchText").attr('placeholder', UIText.Search);
    $("#openBtn").val(UIText.Confirm);
    $("#cancelBtn").val(UIText.Cancel);
}

// 登录页面设置
function SetLoginPage() {
    let UIText = UIStrings.getLocaleStrings();
    $("#btn-login").val(UIText.Login);
    $("#span_logining").text(UIText.Logining);
}

// 版本比较页面
function CompareFileLanguage() {
    let UIText = UIStrings.getLocaleStrings();
    $("#compareFileBtn").val(UIText.CompareSelect);
    $("#cancelBtn").val(UIText.Cancel);
    $(".CompareFile").text(UIText.FileVersions);
}

// 系统错误提示
function SystemError() {
    let UIText = UIStrings.getLocaleStrings();
    layer_alert(UIText.SystemError);
}

// 错误代码提示
function ErrorCodeNotice(code, fileName, folderName) {
    let UIText = UIStrings.getLocaleStrings();
    var msg = CheckLogInByCode(code, UIText);
    if (msg != null && msg != "") {
        layer_alert_logout(msg);
        return;
    }
    var errorMsg;
    switch (code) {
        case 403001171:
            errorMsg = UIText.AccountFrozen;
            break;
        case 403001172:
            errorMsg = UIText.DocLibFrozen;
            break;
        case 403002001:
            errorMsg = UIText.NoMemory;
            break;
        case 403002181:
            errorMsg = String.format(UIText.ExtNotPer, fileName);
            break;
        case 403001002:
        case 403002056:
            errorMsg = UIText.NoNewFilePer;
            break;
        case 403002040:
            errorMsg = String.format(UIText.NoEditPer, fileName);
            break;
        case 403001108:
        case 403002065:
            errorMsg = String.format(UIText.NoLevel, fileName);
            break;
        case 404001024:
        case 404002006:
        case 404002013:
            errorMsg = String.format(UIText.FolderNotFound2, fileName);
            if (folderName) {
                errorMsg = String.format(UIText.FolderNotFound, folderName);
            }
            break;
        case 500002009:
            errorMsg = UIText.SearchError;
            break;
        default:
            errorMsg = UIText.UnknownError + code;
            break;
    }
    layer_alert(errorMsg);
}

// 打开文件错误代码提示
function OpenFileErrorCodeNotice(code, fileName) {
    let UIText = UIStrings.getLocaleStrings();
    var msg = CheckLogInByCode(code, UIText);
    if (msg != null && msg != "") {
        layer_alert_logout(msg);
        return;
    }
    var errorMsg;
    switch (code) {
        case 403001002:
        case 403002056:
            errorMsg = String.format(UIText.NoReadFilePer, fileName);
            break;
        case 404001024:
        case 404002006:
        case 404002013:
        case 403002205:
            errorMsg = String.format(UIText.FileNotFound, fileName);
            break;
        case 500002009:
            errorMsg = UIText.SearchError;
            break;
        default:
            errorMsg = UIText.UnknownError + code;
            break;
    }
    layer_alert(errorMsg);
}

// Outlook错误代码提示
function OutlookErrorCodeNotice(code) {
    let UIText = UIStrings.getLocaleStrings();
    var msg = CheckLogInByCode(code, UIText);
    if (msg != null && msg != "") {
        layer_alert_logout(msg);
        return false;
    }
    return true;
}


// 检测是否下线
function CheckLogInByCode(code, UIText) {
    var errorMsg = "";
    switch (code) {
        // 账户被禁用
        case 401001004:
        case 401001027:
        case 401001040:
            errorMsg = UIText.UserLimit;
            break;
        // IP网段限制
        case 401001031:
            errorMsg = UIText.IPLimit;
            break;
        // 设备绑定
        case 401001009:
        case 401001011:
            errorMsg = UIText.PCLimit;
            break;
        // 客户端被禁用
        case 401001033:
            errorMsg = UIText.CustomLimit;
            break;
        default:
            break;
    }
    return errorMsg;
}

function layer_msg_save(content) {
    layer.msg(content, { icon: 1, skin: "layer_msg_ok" }, function () {
        // 跳转到主页面
        goToHome();
    }); 
}

// 提示信息
function layer_msg(content) {
    layer.msg(content, { icon: 1, skin: "layer_msg_ok" });
}

// 提示信息
function layer_msg_notice(content) {
    layer_alert(content, "Tips");
}

// 提示信息
function layer_alert(content, title) {
    let UIText = UIStrings.getLocaleStrings();
    if (title == "Tips") {
        title = UIText.Tips;
    } else {
        title = UIText.NotSupportOperate;
    }
    var htmlContent = "<p class=\"p_title\">" + title + "</p><p class=\"p_content\">" + content + "</p>";
    layer.alert(htmlContent, { icon: 0, btn: UIText.Confirm, title: null, closeBtn: 0, skin: "layer_alert" });
}

// 提示信息并退出
function layer_alert_logout(content) {
    let UIText = UIStrings.getLocaleStrings();
    var htmlContent = "<p class=\"p_title\">" + UIText.NotSupportOperate + "</p><p class=\"p_content\">" + content + "</p>";
    layer.alert(htmlContent, { icon: 0, btn: UIText.Confirm, title: null, closeBtn: 0, skin: "layer_alert" }, function () {
        log_out();
    });
}

// 提示信息
function layer_load() {
    return layer.load(0, {
        shade: [0.8, '#f5f5f5'],
        skin: "layer_load"
    });
}

// 退出登录
function log_out() {
    localStorage.removeItem('AccessToken');
    window.location.href = "/Login?ReturnUrl=" + window.location.pathname;
}
