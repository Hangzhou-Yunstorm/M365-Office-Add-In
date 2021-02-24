// 检测是否登录
CheckLogin();

$(document).ready(function () {
    // 语言包
    var languageText = UIStrings.getLocaleStrings();

    $(".OfficeTitle").text(languageText.OfficeTitle);
    $(".SaveFile").text(languageText.SaveFile);
    $(".CompareFile").text(languageText.CompareFile);
    $(".OpenFile").text(languageText.OpenFile);
    $(".Settings").text(languageText.Settings);
});
