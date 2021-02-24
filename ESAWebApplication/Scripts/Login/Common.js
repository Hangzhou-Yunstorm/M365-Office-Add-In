// 获取AccessToken
function getAccessToken() {
    CheckLogin();
    return encodeURIComponent(localStorage.getItem("AwUnmdkDT5"));
}

// 检测登录
function IsLogin() {
    var accessToken = localStorage.getItem("AwUnmdkDT5");
    if (!accessToken) {
        return false;
    }

    var lastLogin = localStorage.getItem("LdVjWTdfET");
    if (lastLogin) {
        var currentTime = new Date().getTime();
        if ((currentTime - lastLogin) / 60 / 1000 < 30) {
            return true;
        }
    }
    return RefreshToken();
}

// 检测是否登录
function CheckLogin() {
    if (!IsLogin()) {
        LogOut();
    }
}

// 注销
function LogOut() {
    localStorage.removeItem('AwUnmdkDT5');
    window.location.href = "/Login?ReturnUrl=" + window.location.pathname;
}

// 刷新Token
function RefreshToken() {
    var isLogin = false;
    var dataObject = {
        RefreshToken: localStorage.getItem('Rda1s7wQki'),
        ClientId: localStorage.getItem('ZWKL6EVWcC'),
        ClientSecret: localStorage.getItem('LIt5ZdmX7L')
    };
    $.ajax({
        beforeSend: function () { },
        url: "/Login/RefreshToken",
        type: "post",
        contentType: "application/json; charset=utf-8",
        async: false,
        data: JSON.stringify(dataObject),
        dataType: "json",
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
}

// 去掉特殊字符
function RemoveSpecialChar(str) {
    str = str.replace(/[\\/\\:\\*\\?\\"\\<\\>\\|\\]/g, '');
    return str;
}

// 设置语言
function SetLanguage() {
    var language = localStorage.getItem("currentLanguage");
    if (!language) {
        var syLanguage = Office.context.displayLanguage;
        switch (syLanguage) {
            case 'en-US':
            case 'en-us':
                language = "en-us"
                break;
            case 'zh-CN':
            case 'zh-cn':
                language = "zh-cn";
                break;
            case 'zh-TW':
            case 'zh-tw':
                language = "zh-tw";
                break;
            default:
                language = "zh-cn";
                break;
        }
        localStorage.setItem("currentLanguage", language);
    }
    $("#languageSelect").val(language);
}

// 获取参数
function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) {
            return pair[1];
        }
    }
    return "/";
}

// 字符串Format
String.format = function () {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
};