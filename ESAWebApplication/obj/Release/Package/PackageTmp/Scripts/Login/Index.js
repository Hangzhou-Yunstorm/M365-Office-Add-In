
(function () {
    "use strict";

    // 每次加载新页面时都必须运行初始化函数。
    Office.onReady(function () {
        $(document).ready(function () {

            // 设置语言
            SetLanguage();

            // 设置页面
            SetLoginPage();

            layui.use(['form'], function () {
                var form = layui.form;
                form.on('select(languageSelect)', function (data) {
                    $("#languageSelect").val(data.value);
                    localStorage.setItem("currentLanguage", data.value);
                    // 设置页面
                    SetLoginPage();
                });
            });
        
        });
    });

})();

// 登录页面跳转
function Login() {
    var flag = $("#g_flag").val();
    var language = $("#languageSelect").val();
    var loginUrl = window.location.origin + '/Login/OAuth?flag=' + flag + '&language=' + language;
    WaitForLogin();
    try {
        Office.context.ui.openBrowserWindow(loginUrl);
    } catch (ex) {
        window.open(loginUrl);
    }
}

function LoginBrowser() {
    var flag = $("#g_flag").val();
    var language = $("#languageSelect").val();
    var loginUrl = window.location.origin + '/Login/OAuth?flag=' + flag + '&language=' + language;
    WaitForLogin();
    window.open(loginUrl);
}


// 等待登录
var loginTime;
function WaitForLogin() {
    loginTime = new Date().getTime();

    $("#login-error-msg").empty();
    let UIText = UIStrings.getLocaleStrings();
    $("#btn-login").val(UIText.Logining);
    $(".btn-login").css("background", "#8E97BC");
    $(".btn-login").css("border", "#1px solid #8E97BC");
    $("input").attr('disabled', "disabled");

    WaitLogin();
}

// 等待后台登录结果
function WaitLogin() {
    var dataObject = { flag: $("#g_flag").val() };
    $.ajax({
        beforeSend: function () { },
        url: "/Login/WaitLogin",
        type: "post",
        contentType: "application/json; charset=utf-8",
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

                // 成功 返回页面信息
                var returnUrl = getQueryVariable("ReturnUrl");
                window.location.href = returnUrl;
            } else {
                var currentTime = new Date().getTime();
                if ((currentTime - loginTime) / 60 / 1000 < 2) {
                    // 2分钟登录
                    WaitLogin();
                } else {
                    // 超时失败
                    LoginFail();
                }
            }
        },
        error: function (e) {
            LoginFail();
        }
    });
}

// 登录失败
function LoginFail() {
    let UIText = UIStrings.getLocaleStrings();
    $("#login-error-msg").html(UIText.Login0);
    $("#btn-login").val(UIText.Login);
    $(".btn-login").css("background", "#54639C");
    $(".btn-login").css("border", "#1px solid #54639C");
    $("input").removeAttr("disabled");
}