<%@ Page Title="" Language="C#" MasterPageFile="~/master/Default.Master" AutoEventWireup="true" CodeBehind="pageexamine.aspx.cs" Inherits="ExamineSystem.pageexamine" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
<script language="javascript">
    parent.parent.HideShowDivFunc(true);
</script>
<bgsound id="music" loop="1" volume="80">
<script language="JavaScript">
    window.onerror = function () {
        parent.parent.warn_Win("系统运行出现脚本错误<br>给您带来不便还请谅解", 1);
        return true;
    }

    String.prototype.trim = function () {
        return this.replace(/(^\s*)|(\s*$)/g, "");
    }

    String.prototype.getid = function () {
        return this.substring(5, this.length);
    }

    String.prototype.change = function () {
        var returnTxt = this;
        if (returnTxt == "False" || returnTxt == "false" || returnTxt == "FALSE") returnTxt = "0";
        if (returnTxt == "True" || returnTxt == "true" || returnTxt == "TRUE") returnTxt = "1";
        return returnTxt;
    }
</script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
<body topmargin="5" leftmargin="0">
<script language="javascript">
    var musicArray = [];
    var musicPlayObj = null;
    function playMusic(Val, Count, that) {
        var sign = true;
        for (var i = 0; i < musicArray.length; i++) {
            if (Val == musicArray[i].val) {
                sign = false;
                if (musicArray[i].count == musicArray[i].sum) {
                    alert("此考试听力音频播放次数已超过规定的" + musicArray[i].sum + "遍");
                    return;
                }
                else {
                    musicArray[i].count = (musicArray[i].count - 0) + 1;
                }
            }
        }
        if (sign) {
            musicArray[musicArray.length] = { val: Val, sum: Count, count: 1 };
        }
        if (musicPlayObj != that.nextSibling) {
            if (musicPlayObj != null) musicPlayObj.style.visibility = "hidden";
            that.nextSibling.nextSibling.style.visibility = "visible";
            musicPlayObj = that.nextSibling.nextSibling;
        }
        var musicAddr = unescape(Val);
        document.getElementById("music").src = musicAddr;
    }

    function radioSelect() {
        var that = event.srcElement;
        that.checked = true;
        var InputObjS = that.parentNode.parentNode.parentNode.getElementsByTagName("INPUT");
        for (var i = 0; i < InputObjS.length; i++) {
            if (InputObjS[i] != that && InputObjS[i].type == "radio") {
                InputObjS[i].checked = false;
            }
        }
    }

    function SubmitFunc(Sing) {
        if (Sing == null) {
            if (!confirm("您是否真要完成答卷，请确保您所有类型试题都作答完毕！")) return;
        }
        LockPage();
        var BackStr = CollectionFunc();
        var RunFunc = ["SubmitRunFunc();"];
        var historyId = <%= this.CurrentHistoryId %>;
        parent.left.LoadDocFunc("考试试卷判分", "pageexamine?type=submit&history="+historyId, RunFunc, null, BackStr);
    }

    function SubmitRunFunc() {
        var TmpStrx = parent.left.TmpStr;
        alert("您本次考试的分数为：" + TmpStrx + "分");
        parent.left.examing = false;
    }

    function CollectionFunc() {
        var ReturnStr = "";
        var TabObjS = document.getElementsByTagName("TABLE");
        for (var i = 0; i < TabObjS.length; i++) {
            var TabObj = TabObjS[i];
            if (TabObj.id.indexOf("Ques_") == 0) {
                var EveryStr = "";
                EveryStr = EveryStr + TabObj.id.getid() + "\"";
                var InputObjS = TabObj.getElementsByTagName("INPUT");
                var answer = "";
                for (var x = 0; x < InputObjS.length; x++) {
                    if (InputObjS[x].checked) {
                        if (answer == "") {
                            answer = InputObjS[x].value;
                        }
                        else {
                            answer = answer + "," + InputObjS[x].value;
                        }
                    }
                }
                EveryStr = EveryStr + answer;
                if (ReturnStr == "") {
                    ReturnStr = EveryStr;
                }
                else {
                    ReturnStr = ReturnStr + "'" + EveryStr;
                }
            }
        }
        return ReturnStr;
    }

    function LockPage() {
        var TabObjS = document.getElementsByTagName("TABLE");
        for (var i = 0; i < TabObjS.length; i++) {
            var TabObj = TabObjS[i];
            if (TabObj.id.indexOf("Ques_") == 0) {
                var InputObjS = TabObj.getElementsByTagName("INPUT");
                for (var x = 0; x < InputObjS.length; x++) {
                    InputObjS[x].disabled = true;
                }
            }
        }
        document.getElementById("subbut").disabled = true;
        document.getElementById("cenbut").disabled = true;
    }


    function CanelFunc() {
        if (!confirm("您是否真要全部清空所有类型题目中的答案！")) return;
        var TabObjS = document.getElementsByTagName("TABLE");
        for (var i = 0; i < TabObjS.length; i++) {
            var TabObj = TabObjS[i];
            if (TabObj.id.indexOf("Ques_") == 0) {
                var InputObjS = TabObj.getElementsByTagName("INPUT");
                for (var x = 0; x < InputObjS.length; x++) {
                    InputObjS[x].checked = false;
                }
            }
        }
    }

    var offset = <%= this.ExaminationTime %>;
    var stime = new Date().getMinutes();
    function TimeCountFunc() {
        if (document.getElementById("subbut").disabled == true) return;
        var etime = new Date().getMinutes();
        var diff = etime - stime;
        parent.left.TimeShow("剩余时间：" + (offset - diff) + "分");
        if (diff >= offset) {
            alert("抱歉！您答题时间已到，系统将自动提交您的试卷！");
            SubmitFunc(1);
        } else {
            setTimeout('TimeCountFunc()', 1000);
        }
    }
</script>
	<table border="0" cellpadding="0" cellspacing="0" class="TableStyle" align="center" id="PaperPanel">
	    <tr>
    		<td height="22" align="center" class="HeadStyle">网络在线考试(您考试IP：<%= this.CurrentClientIP%>)</td>
    	</tr>
		<tr>
      		<td class="BodyStyle" align="center" height="100" valign="middle" style="display:block;">
				考试试卷随机生成中....请您稍等片刻.....
			</td>
		</tr>
    	<tr>
      		<td class="BodyStyle" align="center" height="100" valign="middle" id="oContainer" style="display:none;">
			</td>
		</tr>
		<tr>
      		<td class="BodyStyle" align="center" height="100" align="right" style="display:none;">
				<input type="button" value="提交答卷" class="ip2" onClick="SubmitFunc();" id="subbut"> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <input type="button" value="重新填写" class="ip2" onClick="CanelFunc();" id="cenbut"> &nbsp;&nbsp;
			</td>
		</tr>
	</table>

<comment id="Content1">
    <%= this.RandomListenQuestion%>
</comment>
<comment id="Content2">
    <%= this.RandomSingleQuestion%>
</comment>      
<comment id="Content3">
    <%= this.RandomMultiQuestion%>
</comment>
<comment id="Content4">
    <%= this.RandomJudgeQuestion%>
</comment>      
<comment id="Content5">
    <%= this.RandomReadQuestion%>
</comment>
</body>
<script src="script/tabpage.js" type=text/javascript></script>
<script language="javascript">
    parent.parent.HideShowDivFunc(false);
    alert("试卷生成完毕，单击“确定”后，系统真正进入答题阶段！");
    parent.left.examing = true;
    var MainPanelObj = document.getElementById("PaperPanel");
    MainPanelObj.rows[1].cells[0].style.display = "none";
    MainPanelObj.rows[2].cells[0].style.display = "block";
    MainPanelObj.rows[3].cells[0].style.display = "block";
    TimeCountFunc();
</script>
</asp:Content>
