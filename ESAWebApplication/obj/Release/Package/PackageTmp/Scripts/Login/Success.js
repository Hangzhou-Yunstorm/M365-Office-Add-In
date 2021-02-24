
$(document).ready(function () {
    localStorage.setItem("currentLanguage", $("#h_language").val());
    let UIText = UIStrings.getLocaleStrings();

    $("#title_p").text(UIText.LoginSuccess);
    $("#dis_p").text(UIText.LoginTips);
});

