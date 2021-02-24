﻿/* Store the locale-specific strings */

var UIStrings = (function () {

    var UIStrings = {};

    //简体
    UIStrings.CN =
    {
        "DefaultFolder": "默认文件夹",
        "Folder": "文件夹",
        "Name": "文件名：",
        "SaveBtn": "保存",
        "Login": "登录",
        "Logining": "登录中...",
        "Login0": "登录失败，请重试。",
        "Login1": "无法连接服务器。",
        "LoginSuccess": "欢迎使用AnyShare Office插件!",
        "LoginError": "登录失败，请重试",
        "LoginTips": "登录成功！请手动关闭此页面",
        "SystemError": "系统错误，请重试。",
        "Settings": "设置",
        "Help": "在线帮助",
        "Versions": "版本信息",
        "UserName": "用户名",
        "Account": "账号",
        "LogOut": "退出登录",
        "Max255": "文件夹名不能超过255个字符。",
        "FileMax255": "文件名不能超过255个字符。",
        "NoSpecialChar": "文件夹名不能包含 \\ / : * ? \" <> | 特殊字符。",
        "FileNoSpecialChar": "文件名不能包含 \\ / : * ? \" <> | 特殊字符。",
        "SelectSavePath": "选择保存路径",
        "AllDoc": "全部文档",
        "PersonalDoc": "个人文档库",
        "ShareDoc": "共享文档库",
        "DepartmentDoc": "部门文档库",
        "DocLib": "文档库",
        "NewFolder": "新建文件夹",
        "EnterFolderName": "输入文件夹名称: ",
        "PEnterFolderName": "请输入文件夹名称",
        "Confirm": "确定",
        "Cancel": "取消",
        "SaveToDefault": "保存至默认路径",
        "HadSameFile": "目标位置已存在同名文档",
        "HadSameFileNotice": "您可选择跳过、替换或保留两者（当前文件将重命名为",
        "SaveTwo": "保留两者",
        "Replace": "替换",
        "Return": "跳过",
        "Search": "搜索",
        "NoChildFolder": "无子文件夹",
        "NoChildFile": "文件列表为空",
        "NoSearchResult": "无搜索结果",
        "HadSameFolder": "文件夹名已存在。",
        "SaveSuccess": "保存成功",
        "CreatedSuccess": "新建成功",
        "VersionInfo": "当前版本：",
        "PublishDate": "发布时间：",
        "SetDefaultFolder": "设置默认保存路径：",
        "AddDefaultFolder": "添加路径",
        "AddDefaultPath": "添加默认路径",
        "SetLanguage": "设置语言：",
        "AccountFrozen": "您的账号已被冻结",
        "DocLibFrozen": "文档库已被冻结",
        "ExtNotPer": "文件 “{0}” 的格式已被禁止上传。",
        "NoMemory": "您选择的目标位置配额空间不足。",
        "NoLevel": "您对同名文件 “{0}” 的密级权限不足。",
        "NoPer": "您没有权限执行此操作。",
        "NoNewFolderPer": "您没有该文件夹没有新建权限。",
        "NoNewFilePer": "您对选择的目标文件夹没有新建权限。",
        "NoReadFilePer": "您对文件 “{0}” 没有读取权限。",
        "NoEditPer": "您对同名文件 “{0}” 没有修改权限。",
        "FileLocked": "文件 “{0}” 已被用户 “{1}” 锁定。",
        "UnknownError": "未知错误码：",
        "FileNotFound": "文件 “{0}” 不存在，可能其所在路径发生变更。",
        "FolderNotFound": "您选择的目标文件夹 “{0}” 不存在，可能其所在路径发生变更。",
        "FolderNotFound2": "您选择的目标文件夹不存在，可能其所在路径发生变更。",
        "SearchError": "内容分析及检索服务无法连接，请联系管理员。",
        "OpenFileFromServer": "从云端打开文件",
        "Loading": "加载中...",
        "FileVersions": "历史版本",
        "CompareSelect": "比较选择的版本",
        "NoVersion": "暂无历史版本",
        "OnlyASFile": "仅支持查看云端文件历史版本",
        "ModifiedOn": " 修改于 ",
        "SetEmailDefaultFolder": "设置邮件默认保存路径：",
        "SetAttachDefaultFolder": "设置附件默认保存路径：",
        "SetAttachSize": "设置附件大小：",
        "SetAttachSizeNotice": "当本地文件超过该限制时，自动转为SharedLink共享",
        "SetAttachSizeError": "阀值可填写范围为1-30MB的整数，请重新输入。",
        "FileSizeLimit": "文件“{0}”已超过{1}文件大小限制。",
        "SaveFile": "保存到云端",
        "OpenFile": "打开云端文件",
        "CompareFile": "版本对比",
        "OfficeTitle": "Office 插件",
        "NoDefaultPath": "您还未添加默认保存路径",
        "UserLimit": "您的账号已被禁用，请联系管理员",
        "IPLimit": "您受到IP网段限制，无法继续登录，请联系管理员。",
        "PCLimit": "当前设备无法继续登录，请使用绑定的设备登录或联系管理员。",
        "CustomLimit": "管理员已禁止此类客户端登录。",
        "NoAttachments": "无附件。",
        "Details": "详情",
        "YouCanSelect": "您可将当前文件：",
        "YouCanSelectMail": "您可将当前邮件：",
        "YouCanSelectAttachment": "您可将当前附件：",
        "SaveToAS": "保存至原路径",
        "SaveToDefault2": "保存至默认路径",
        "SaveToSelect": "自定义保存路径",
        "SaveNotice": "保存文件提醒",
        "SaveMailNotice": "保存邮件提醒",
        "SaveAttachmentNotice": "保存附件提醒",
        "NotSupportOperate": "无法执行此操作",
        "Tips": "提示",
        "UrlTitle": "链接标题",
        "Size": "大小",
        "Password": "访问密码",
        "ShareType": "共享类型",
        "WithUsers": "实名共享",
        "WithAnyone": "匿名共享",
        "NoDefaultDir": "暂无默认路径，请前往[设置]-[通用]页面进行设定。",
        "General": "通用"
    };

    //繁体
    UIStrings.TW =
    {
        "DefaultFolder": "默認文件夾",
        "Folder": "文件夾",
        "Name": "檔案名：",
        "SaveBtn": "儲存",
        "Login": "登入",
        "Logining": "登入中...",
        "Login0": "登入失敗，請重試。",
        "Login1": "無法連接服務器。",
        "LoginSuccess": "歡迎使用AnyShare Office外掛程式！",
        "LoginError": "登入失敗，請重試",
        "LoginTips": "登入成功！請手動關閉此頁面",
        "SystemError": "系統錯誤，請重試。",
        "Settings": "設定",
        "Help": "線上說明",
        "Versions": "版本資訊",
        "UserName": "使用者名稱",
        "Account": "帳戶",
        "LogOut": "登出",
        "Max255": "資料夾名不能超過255個字元。",
        "FileMax255": "檔案名不能超過255個字元。",
        "NoSpecialChar": "資料夾名不能包含\\ / : * ? \" <> | 特殊字元。",
        "FileNoSpecialChar": "檔案名不能包含\\ / : * ? \" <> | 特殊字元。",
        "SelectSavePath": "選擇儲存路徑",
        "AllDoc": "全部檔案",
        "PersonalDoc": "個人文件庫",
        "ShareDoc": "共用文件庫",
        "DepartmentDoc": "部門文件庫",
        "DocLib": "文件庫",
        "NewFolder": "新增資料夾",
        "EnterFolderName": "輸入資料夾名稱: ",
        "PEnterFolderName": "請輸入資料夾名稱",
        "Confirm": "確定",
        "Cancel": "取消",
        "SaveToDefault": "儲存至預設路徑",
        "HadSameFile": "目標位置已存在同名文件",
        "HadSameFileNotice": "您可選擇跳過、替換或保留兩者（當前檔案將重命名為",
        "SaveTwo": "保留兩者",
        "Replace": "替換",
        "Return": "跳過",
        "Search": "搜尋",
        "NoChildFolder": "這個清單是空的",
        "NoChildFile": "文件列表为空",
        "NoSearchResult": "無搜尋結果",
        "HadSameFolder": "資料夾名已存在。",
        "SaveSuccess": "儲存成功",
        "CreatedSuccess": "新增成功",
        "VersionInfo": "當前版本：",
        "PublishDate": "發佈時間：",
        "SetDefaultFolder": "設定預設儲存路徑：",
        "AddDefaultFolder": "添加路徑",
        "AddDefaultPath": "添加預設路徑",
        "SetLanguage": "設定語言：",
        "AccountFrozen": "您的帳戶已被凍結。",
        "DocLibFrozen": "文件庫已被凍結。",
        "ExtNotPer": "檔案 “{0}” 的格式已被禁止上傳。",
        "NoMemory": "您選擇的目標位置配額空間不足。",
        "NoLevel": "您對同名檔案 “{0}” 的密級權限不足。",
        "NoPer": "您沒有許可權執行此操作。",
        "NoNewFolderPer": "您對該資料夾沒有新增權限。",
        "NoNewFilePer": "您對選擇的目標資料夾沒有新增權限。",
        "NoReadFilePer": "您對檔案 “{0}” 沒有讀取權限。",
        "NoEditPer": "您對同名檔案 “{0}” 沒有修改權限。",
        "FileLocked": "檔案 “{0}” 已被使用者 “{0}” 鎖定。",
        "UnknownError": "未知錯誤碼：",
        "FileNotFound": "檔案  “{0}”  不存在，可能其所在路徑發生變更。",
        "FolderNotFound": "您選擇的目標資料夾 “{0}” 不存在，可能其所在路徑發生變更。",
        "FolderNotFound2": "您選擇的目標資料夾不存在，可能其所在路徑發生變更。",
        "SearchError": "內容分析及擷取服務無法連接，請聯繫管理員。",
        "OpenFileFromServer": "從雲端開啟檔案",
        "Loading": "加載中...",
        "FileVersions": "歷史版本",
        "CompareSelect": "比較選擇的版本",
        "NoVersion": "暫無歷史版本",
        "OnlyASFile": "僅支援檢視雲端檔案歷史版本",
        "ModifiedOn": " 修改於 ",
        "SetEmailDefaultFolder": "設定郵件預設儲存路徑：",
        "SetAttachDefaultFolder": "設定附件預設儲存路徑：",
        "SetAttachSize": "設定附件大小：",
        "SetAttachSizeNotice": "當附件超過該限制時，自動轉為SharedLink共用",
        "SetAttachSizeError": "閾值可填寫範圍為1-30MB的整數，請重新輸入。",
        "FileSizeLimit": "檔案“{0}”已超過{1}檔案大小限制。",
        "SaveFile": "儲存至雲端",
        "OpenFile": "開啟雲端檔案",
        "CompareFile": "版本對比",
        "OfficeTitle": "Office 挿件",
        "NoDefaultPath": "您還未添加預設儲存路徑",
        "UserLimit": "您的帳戶已被停用，請聯繫管理員。",
        "IPLimit": "您受到IP網段限制，無法繼續登入，請聯繫管理員。",
        "PCLimit": "當前裝置無法登入，請使用繫結的裝置登入或聯繫管理員。",
        "CustomLimit": "管理員已禁止此類用戶端登入。",
        "NoAttachments": "無附件。",
        "Details": "詳情",
        "YouCanSelect": "您可將當前檔案：",
        "YouCanSelectMail": "您可將當前郵件：",
        "YouCanSelectAttachment": "您可將當前附件：",
        "SaveToAS": "儲存至原路徑",
        "SaveToDefault2": "儲存至預設路徑",
        "SaveToSelect": "自訂儲存路徑",
        "SaveNotice": "儲存檔案提醒",
        "SaveMailNotice": "儲存郵件提醒",
        "SaveAttachmentNotice": "儲存附件提醒",
        "NotSupportOperate": "無法執行此操作",
        "Tips": "提示",
        "UrlTitle": "連結標題",
        "Size": "大小",
        "Password": "存取密碼",
        "ShareType": "共用類型",
        "WithUsers": "實名共用",
        "WithAnyone": "匿名共用",
        "NoDefaultDir": "暫無預設路徑，請前往[設定]-[通用]頁面進行設定。",
        "General": "通用"
    };

    //英文
    UIStrings.US =
    {
        "DefaultFolder": "Default folder",
        "Folder": "Folder",
        "Name": "File name: ",
        "SaveBtn": "Save",
        "Login": "Log In",
        "Logining": "Logging in",
        "Login0": "Login failed, please try again.",
        "Login1": "Unable to connect to server.",
        "LoginSuccess": "Welcome to AnyShare Office Add-In!",
        "LoginError": "Login failed. Try again.",
        "LoginTips": "Login succeeded! Manually close this page, please.",
        "SystemError": "System error, please try again.",
        "Settings": "Settings",
        "Help": "Online Help",
        "Versions": "About",
        "UserName": "Username",
        "Account": "Account",
        "LogOut": "Log out",
        "Max255": "Folder name cannot exceed 255 characters.",
        "FileMax255": "File name cannot exceed 255 characters.",
        "NoSpecialChar": "Folder name cannot contain special characters like \\ / : * ? \" <> |.",
        "FileNoSpecialChar": "File name cannot contain special characters like \\ / : * ? \" <> |.",
        "SelectSavePath": "Select location to save",
        "AllDoc": "All",
        "PersonalDoc": "My Documents",
        "ShareDoc": "Shared Documents",
        "DepartmentDoc": "Department Documents",
        "DocLib": "Other Documents",
        "NewFolder": "New Folder",
        "EnterFolderName": "Enter folder name: ",
        "PEnterFolderName": "Please enter folder name ",
        "Confirm": "OK",
        "Cancel": "Cancel",
        "SaveToDefault": "Save to default location",
        "HadSameFile": "File with the same name already exists in this location.",
        "HadSameFileNotice": "A conflict is found. Below come your choices. （Keep both and the current one will be renamed as ",
        "SaveTwo": "Keep both",
        "Replace": "Replace",
        "Return": "Skip",
        "Search": "Search",
        "NoChildFolder": "No subfolders",
        "NoChildFile": "This list is empty",
        "NoSearchResult": "No results found",
        "HadSameFolder": "This folder name already exists.",
        "SaveSuccess": "Saved successfully",
        "CreatedSuccess": "Created successfully",
        "VersionInfo": "Version: ",
        "PublishDate": "Released at ",
        "SetDefaultFolder": "Select the default directory to save: ",
        "AddDefaultFolder": "Add",
        "AddDefaultPath": "Add default directory to save",
        "SetLanguage": "Select Languages: ",
        "AccountFrozen": "Your account has been frozen.",
        "DocLibFrozen": "Document Library has been frozen.",
        "ExtNotPer": "The format of file  “{0}”  has been disabled to upload.",
        "NoMemory": "Insufficient quota in the selected location.",
        "NoLevel": "You do not have sufficient security level for file “{0}”.",
        "NoPer": "You do not have permission to perform this operation.",
        "NoNewFolderPer": "You do not have permission to create in this folder.",
        "NoNewFilePer": "You do not have permission to create in target folder.",
        "NoReadFilePer": "You do not have permission to read file “{0}”.",
        "NoEditPer": "You do not have permission to modify file “{0}”.",
        "FileLocked": "File “{0}” has been locked by user  “{0}” .",
        "UnknownError": "Unknown error code: ",
        "FileNotFound": "File “{0}” does not exist, Maybe its path has been changed.",
        "FolderNotFound": "The folder you choose “{0}” does not exist，Maybe its path has been changed.",
        "FolderNotFound2": "The folder you choose does not exist，Maybe its path has been changed.",
        "SearchError": "Unable to connect to Content Analysis and Index Service. Please contact your admin.",
        "OpenFileFromServer": "Open Cloud Files",
        "Loading": "Loading",
        "FileVersions": "Historical Versions",
        "CompareSelect": "Compare",
        "NoVersion": "No historical versions",
        "OnlyASFile": "Only available for historical versions of cloud files.",
        "ModifiedOn": " Modified at ",
        "SetEmailDefaultFolder": "Select the default directory to save email: ",
        "SetAttachDefaultFolder": "Select the default directory to save attachment: ",
        "SetAttachSize": "Set the size limit of attachment: ",
        "SetAttachSizeNotice": "You will share the attachment via SharedLink if it exceeds the limit.",
        "SetAttachSizeError": "The value should be an integer from 1 to 30. Please enter again.",
        "FileSizeLimit": "File “{0}” has exceeded the size limit of {1}.",
        "SaveFile": "Save to Cloud",
        "OpenFile": "Open Cloud File",
        "CompareFile": "Compare Versions",
        "OfficeTitle": "Office Add-In",
        "NoDefaultPath": "No directories yet",
        "UserLimit": "Your account has been disabled. Please contact your admin.",
        "IPLimit": "Login failed due to network restrictions. Please contact your admin.",
        "PCLimit": "Unable to log in via this device. Please log in with bound devices or contact your admin.",
        "CustomLimit": "You are not allowed to log in to this client, restricted by admin.",
        "NoAttachments": "No attachments.",
        "Details": "Details",
        "YouCanSelect": "You can save this file to ",
        "YouCanSelectMail": "You can save this email to ",
        "YouCanSelectAttachment": "You can save these attachments to ",
        "SaveToAS": "Original path",
        "SaveToDefault2": "Default path",
        "SaveToSelect": "Custom path",
        "SaveNotice": "Settings for File Saving",
        "SaveMailNotice": "Settings for Email Saving",
        "SaveAttachmentNotice": "Settings for Attachment Saving",
        "NotSupportOperate": "Operation failed",
        "Tips": "Tips",
        "UrlTitle": "Description",
        "Size": "Size",
        "Password": "Password",
        "ShareType": "Share",
        "WithUsers": "With users",
        "WithAnyone": "With anyone",
        "NoDefaultDir": "No default path. Please set it in Settings-General.",
        "General": "General"
    };

    UIStrings.getLocaleStrings = function () {

        let language = localStorage.getItem("currentLanguage");
        if (!language) {
            language = "zh-cn";
        }

        var text;

        // Get the resource strings that match the language.
        switch (language) {
            case 'zh-cn':
                text = UIStrings.CN;
                break;
            case 'zh-tw':
                text = UIStrings.TW;
                break;
            case 'en-us':
                text = UIStrings.US;
                break;
            default:
                text = UIStrings.CN;
                break;
        }

        return text;
    };
    return UIStrings;
})();