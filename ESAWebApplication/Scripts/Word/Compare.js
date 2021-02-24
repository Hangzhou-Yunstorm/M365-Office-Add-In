// 检测是否登录
CheckLogin();

var languageText, layuiform;

(function () {
    "use strict";

    // 每次加载新页面时都必须运行初始化函数。
    Office.onReady(function () {
        $(document).ready(function () {
            // 语言包
            languageText = UIStrings.getLocaleStrings();

            // 页面语言
            CompareFileLanguage();

            // 获取文件属性
            Word.run(function (context) {
                let builtInProperties = context.document.properties;
                builtInProperties.load("comments");
                return context.sync().then(function () {
                    var gnsId = builtInProperties.comments;
                    console.log(gnsId);

                    // 是否含有gnsId
                    if (gnsId != null && gnsId != "" && gnsId.indexOf("gns://") > -1) {
                        $("#fileUrl").val(gnsId);
                        getFileVersions(gnsId);
                    } else {
                        $("#file_rev").html("<div class=\"empty\"><p>" + languageText.OnlyASFile + "</p></div>");
                    }
                });
            });

        });
    });

})();

// 获取文件版本信息
function getFileVersions(fileId) {

    var index = layer_load();

    var formdata = new FormData();
    formdata.append('TokenId', getAccessToken());
    formdata.append('Docid', encodeURIComponent(fileId));

    $.ajax({
        url: "/Word/GetFileRevisions",
        type: "post",
        data: formdata,
        processData: false,
        contentType: false,
        dataType: 'json',
        success: function (data) {
            if (data.Success) {
                var code = data.StatusCode;
                if (code == 0) {
                    // Run a batch operation against the Word object model.
                    var versions = new Function('return ' + data.Data)();

                    $("#file_rev").empty();
                    if (versions != null && versions.length > 0) {
                        var ul = document.createElement("ul");
                        for (var m = 0; m < versions.length; m++) {
                            var version = versions[m];
                            var li = document.createElement("li");
                            var editInfo = version.Editor + languageText.ModifiedOn + version.Modified;
                            li.innerHTML = "<div class=\"li_div\" onclick=\"VersionChecked(this)\">" +
                                "<div class=\"first_img\"><input type=\"radio\" value=\"" + version.Rev + "\" name=\"Version\" lay-filter=\"Version\" /><img src=\"/Images/Icons/SaveAndOpen/word.png\" /></div>" +
                                "<div class=\"first_a\"><p class=\"p_title\" title=\"" + version.Name + "\">" + version.Name + "</p><p class=\"p_sub\" title=\"" + editInfo + "\">" + editInfo + "</p></div>" +
                                "</div>";
                            ul.appendChild(li);
                        }

                        layui.use(['form'], function () {
                            layuiform = layui.form;
                            layuiform.on("radio(Version)", function (data) {
                                //var version = data.value;
                                $("#compareFileBtn").attr("disabled", false);
                            });
                        });

                        document.getElementById("file_rev").appendChild(ul);
                        $("#compare-footer").show();
                    } else {
                        $("#file_rev").html("<div class=\"empty\"><p>" + languageText.NoVersion + "</p></div>");
                    }
                } else {
                    $("#file_rev").html("<div class=\"empty\"><p>" + languageText.OnlyASFile + "</p></div>");
                    ErrorCodeNotice(code);
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

//点击版本Div
function VersionChecked(e) {
    e.firstElementChild.firstElementChild.click();
    $("#compareFileBtn").attr("disabled", false);
    layuiform.render();
}

// 点击比对
var index;
function compareFile() {

    var Version = $("input[name='Version']:checked").val();
    if (!Version) {
        layer_msg_notice('Please select a file version.');
        return;
    }

    index = layer_load();

    // 获取当前文件信息
    GetFileInfo();
}

// 提交到后台比对
function goToCompare(compareGuid) {
    var formdata = new FormData();
    formdata.append('TokenId', getAccessToken());
    formdata.append('Docid', encodeURIComponent($("#fileUrl").val()));
    formdata.append('Rev', encodeURIComponent($("input[name='Version']:checked").val()));
    formdata.append('Guid', compareGuid);

    $.ajax({
        url: "/Word/CompareFileFromServer",
        type: "post",
        data: formdata,
        processData: false,
        contentType: false,
        dataType: 'json',
        success: function (data) {
            if (data.Success) {
                var code = data.StatusCode;
                if (code == 0) {
                    // Run a batch operation against the Word object model.
                    Word.run(function (context) {
                        var newDoc = context.application.createDocument(data.Data);
                        return context.sync()
                            .then(function () {
                                newDoc.open();
                            }).catch(function (myError) {
                                SystemError();
                            });

                    }).catch(function (error) {
                        SystemError();
                    });
                } else {
                    ErrorCodeNotice(code);
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

// The following example gets the document in Office Open XML ("compressed") format in 65536 bytes (64 KB) slices.
// Note: The implementation of console.log in this example is from the Visual Studio template for Office Add-ins.
/**
 * 获取当前文件信息
 */
function GetFileInfo() {
    Office.context.document.getFileAsync(Office.FileType.Compressed, { sliceSize: 4194304 },
        function (result) {
            if (result.status == Office.AsyncResultStatus.Succeeded) {
                // If the getFileAsync call succeeded, then result.value will return a valid File Object.
                var myFile = result.value;
                var sliceCount = myFile.sliceCount;
                console.log("File size:" + myFile.size + " ,Slices count: " + sliceCount);

                // Get the file slices.
                getSliceAsync(myFile, 0, sliceCount, 1, getGuid());
            }
            else {
                console.log("Error:", result.error.message);
                SystemError();
                layer.close(index);
            }
        });
}

// 分开获取文件
function getSliceAsync(file, nextSlice, sliceCount, slicesReceived, compareGuid) {
    console.log("Slice:" + slicesReceived + " Of " + sliceCount);

    file.getSliceAsync(nextSlice, function (sliceResult) {
        if (sliceResult.status == Office.AsyncResultStatus.Succeeded) {

            var postFD = new FormData();
            postFD.append('Start', slicesReceived);
            postFD.append('End', sliceCount);
            postFD.append('Guid', compareGuid);
            postFD.append('Base64Str', base64EncArr(sliceResult.value.data));

            $.ajax({
                url: '/Word/CompareFileTemp',
                type: 'post',
                data: postFD,
                processData: false,
                contentType: false,
                dataType: 'json',
                success: function (data) {
                    if (data.Success) {
                        var code = data.StatusCode;
                        if (code == 0) {
                            // Got one slice, store it in a temporary array.  (Or you can do something else, such as  send it to a third-party server.)
                            if (slicesReceived == sliceCount) {
                                // All slices have been received.
                                file.closeAsync();
                                // Send File
                                goToCompare(compareGuid);
                            } else {
                                slicesReceived++;
                                getSliceAsync(file, ++nextSlice, sliceCount, slicesReceived, compareGuid);
                            }
                        } else {
                            layer.close(index_layer);
                            SystemError();
                        }
                    } else {
                        file.closeAsync();
                        layer.close(index_layer);
                        SystemError();
                    }
                },
                error: function (ex) {
                    file.closeAsync();
                    console.error(ex);
                    layer.close(index_layer);
                    SystemError();
                }
            });
        }
        else {
            file.closeAsync();
            console.log("getSliceAsync Error:", sliceResult.error.message);
            layer.close(index_layer);
            SystemError();
        }
    });
}

//生成随机 GUID 数
function getGuid() {
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
};
