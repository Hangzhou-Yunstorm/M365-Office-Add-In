var fileGnsId;

// 获取文件属性
function getFileProp() {
    Word.run(function (context) {
        let builtInProperties = context.document.properties;
        builtInProperties.load("comments");
        return context.sync().then(function () {
            var gnsId = builtInProperties.comments;
            console.log(gnsId);
            fileGnsId = gnsId;

            // 是否含有gnsId
            if (gnsId != null && gnsId != "" && gnsId.indexOf("gns://") > -1) {
                // AS file
                GetFilePath(gnsId);
            } else {
                // Local file
                setDefaultSelType(2);
                $("#AS_File").hide();
            }
        });
    }).catch(function (error) {
        console.log("Error: " + error);
        // Local file
        setDefaultSelType(2);
        $("#AS_File").hide();
    });
}

// 跳转到设置页面
function goToSettings() {
    window.location.href = "/Word";
}

// 跳转到主页面
function goToHome() {
    window.location.href = "/Word/Home";
}