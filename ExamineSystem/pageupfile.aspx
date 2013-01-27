<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pageupfile.aspx.cs" Inherits="ExamineSystem.pageupfile" %>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<meta http-equiv="Content-Language" content="utf-8" />
<title>音频上传</title>
</head>

<body>
<script language="JavaScript">
    var FileLock = false;
    function checkFunc(that) {
        if (that.file.value != "") {
            var url = that.file.value;
            var testUrl = /\.[^\.]+$/.exec(url);
            var ExName = (testUrl != null) ? testUrl[0].toUpperCase() : "***";
            if (url != "" && ".WMA;.MP3;".indexOf(ExName + ";") == -1) {
                alert("您上传的文件不是音频文件(.WMA,.MP3)\n请您重新选择文件");
                return false;
            }
        }
        if (that.typeid.value == "") {
            alert("您不可以在系统外进行文件提交");
            return false;
        }
        FileLock = true;
        return true;
    }
</script>

<form name="formObj" id="formObj" method="post" action="pageupfileback.aspx" enctype="multipart/form-data">
<input type="file" name="file" id="file" value="">
<input type="hidden" name="typeid" id="typeid" value="">
</form>
</body>
</html>
