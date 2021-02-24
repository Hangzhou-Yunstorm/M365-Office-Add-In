var __docType = "excel";

// 打开文件
function openFile() {
    var fileId = $("#openFileUrl").val();
    var fileName = $("#openFileName").val();
    if (!fileId) {
        layer_msg_notice('Please select File.');
        return
    }

    var index = layer_load();

    var dataObject = {
        TokenId: getAccessToken(),
        FileId: fileId,
        DocType: __docType
    };

    $.ajax({
        url: "/Word/OpenFileFromServer",
        type: "post",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataObject),
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var code = data.StatusCode;
                if (code == 0) {
                    Excel.createWorkbook(data.Data);
                } else if (code == 403002070) {
                    // 被锁定
                    var message = new Function('return ' + data.Message)();
                    var errorMsg = String.format(languageText.FileSizeLimit, fileName, getFileSize(message.file_limit_size));
                    layer_alert(errorMsg);
                } else {
                    OpenFileErrorCodeNotice(code, fileName);
                }
            } else {
                SystemError();
            }
            layer.close(index);
        },
        error: function (e) {
            SystemError();
            layer.close(index);
        }
    });
}
