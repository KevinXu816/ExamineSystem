<%@ Page Language="C#" MasterPageFile="~/master/Default.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ExamineSystem.login" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
<script language="JavaScript">

    if (top.location != self.location) top.location = self.location;

    function check(that) {
        var RegObj = /(\\|'|")/gi;
        var ArrayObj = new Array(that.nam, that.pss, that.valt);
        for (var i = 0; i < ArrayObj.length; i++) {
            var T_Obj = ArrayObj[i];
            if (T_Obj.value == "") {
                alert("光标所在的文本框不能为空，请输入内容！");
                T_Obj.focus();
                return false;
            }
            if (RegObj.test(T_Obj.value)) {
                alert("光标所在的文本框中输入了非法字符，请更正！");
                T_Obj.select();
                return false;
            }
            if (i != 1) {
                if (/[^\d]+/.test(T_Obj.value)) {
                    alert("光标所在的文本框中输入的不是数值，请更正");
                    T_Obj.select();
                    return false;
                }
            }
        }
        var RegExpJPER = /[\u30B4|\u30AC|\u30AE|\u30B0|\u30B2|\u30B6|\u30B8|\u30BA|\u30C5|\u30C7|\u30C9|\u30DD|\u30D9|\u30D7|\u30D3|\u30D1|\u30F4|\u30DC|\u30DA|\u30D6|\u30D4|\u30D0|\u30C2|\u30C0|\u30BE|\u30BC]/g;
        that.nam.value = that.nam.value.replace(RegExpJPER, "□");
        that.pss.value = that.pss.value.replace(RegExpJPER, "□");
        return true;
    }

    function ReLoadValCode() {
        var GetCode = document.getElementById("ValCode");
        GetCode.src = "validate.aspx?random=" + Math.random();
    }

</script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
<body scroll="no" topmargin="60" style="background-color: #ffffff;" onclick="DBNPOC.closeWin();">
<div style="position: absolute; z-index: 99; width: 170px; display: none; border: #183789 1px solid;" id="DBNPO" onClick="DBNPOC.tmpFunc();">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td bgcolor="#cad7f7" height="20" valign="middle"><font color="#ff0000"><b>用户类型选择窗口</b></font><span style="width: 62; text-align: right;"><img src="images/del.gif" align="absmiddle" height="11" width="11" alt="关闭窗口" onClick="DBNPOC.closeWin();"></span>
	</td>
  </tr>
  <tr>
    <td bgcolor="#dfe6f8" align="center">
		<table cellpadding="0" cellspacing="0" align="center" width="100%">
			<tr><td align="center" valign="middle"><br>
				<b>用户类型：</b> <select size="1" id="typ">
				<option value="0" selected>考生类型</option>
				<option value="1">教师类型</option>
				</select><br><br>
			</td></tr>
		</table>
	</td>
  </tr>
</table>
</div>
<script language="javascript">
    function DBNPOClass() {
        this.DBNPObj = document.getElementById("DBNPO");
        this.FlagDo = 0;
        this.tmpFunc = function () {
            this.FlagDo = 1;
        };
        this.closeWin = function () {
            if (this.DBNPObj.style.display != "none") {
                if (this.FlagDo == 1) {
                    this.FlagDo = 0;
                    return;
                }
                this.DBNPObj.style.display = "none";
            }
        };
        this.showWin = function (XX, YY) {
            this.FlagDo = 1;
            this.DBNPObj.style.top = YY;
            this.DBNPObj.style.left = XX;
            this.DBNPObj.style.display = "block";
        };

    }
    var DBNPOC = new DBNPOClass();

    function BackDefaultFunc() {
        DBNPOC.showWin((event.clientX + document.body.scrollLeft), (event.clientY + document.body.scrollTop))
    }
</script>

<form method="POST" id="loginform">
<table cellspacing="0" cellpadding="0" width="600" align="center" border="0">
  <tr><td><img height="157" src="images/login_1.jpg" width="595"></td></tr>
  <tr><td valign="middle" height="20" align="center" background="/images/login_2.jpg">
  <span id="infostr">
  <%=this.WarningText%>
  </span>
  </td></tr>
  <tr><td valign="top" height="93" background="/images/login_3.jpg" align="center">
		  <table cellspacing="0" cellpadding="0" border="0">
		  <tr>
			<td colspan="7" height="6"></td>
		  </tr>
		  <tr>
			<td rowspan="2" width="36" align="right"><img height="30" src="images/login_user.gif" width="30"></td>
			<td width="126"><font color="#043bc9">用户编号：</font></td>
			<td rowspan="2" width="36" align="center"><img height="30" src="images/login_pass.gif" width="30"></td>
			<td width="131"><font color="#043bc9">用户密码：</font></td>
			<td rowspan="2" width="33" align="center"><img height="30" src="images/login_vali.gif" width="30"></td>
			<td colspan="2"><font color="#043bc9">验证码：</font></td>
		  </tr>
		  <tr>
			<td><input type="text" name="nam" id="nam" size="14" maxlength="14" class="InputStyleF"></td>
			<td><input type="password" name="pss" id="pss" size="14" maxlength="14" class="InputStyleF"></td>
			<td><input type="text" name="valt" id="valt" size="10" maxlength="14" class="InputStyleF"></td>
			<td><img id="ValCode" src="validate.aspx" style="cursor: pointer;" onClick="ReLoadValCode();" alt="重新更换验证码"></td>
		  </tr>
		  <tr>
			<td colspan="7" height="10"></td>
		  </tr>
		  <tr>
			<td colspan="7" align="center">
		<input type="button" value="用户类型" class="ip2" onClick="BackDefaultFunc();"> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <input type="button" value="登陆系统" class="ip2" onClick="dosubmit();"> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <input type="reset" value="内容重置" class="ip2">
			</td>
		  </tr>
		</table>
  </td></tr>
</table>
</form>

<script language="javascript">
    document.getElementById("nam").focus();

    window.onerror = function () {
        warn_Win("系统运行出现脚本错误，给您带来不便还请谅解");
        Lock = false;
        return true;
    }

    function dosubmit() {
        var loginformobj = document.getElementById("loginform");
        if (check(loginformobj)) {
            var typeobj = document.getElementById("typ");
            var namenum = loginformobj.nam.value + "";
            if (typeobj.value == '1') namenum = "-" + namenum;
            var tempstr = namenum + "'" + loginformobj.pss.value + "'" + loginformobj.valt.value;
            LoadDocFunc("用户登陆系统", "login", tempstr);
        }
    }

    function warn_Win(txt) {
        document.getElementById("infostr").innerHTML = txt;
    }

    function document.body.onbeforeunload() {
        if (Lock) {
            event.returnValue = "您刚申请的操作还未完成，您是否真要结束操作并离开页面？";
            event.cancelBubble = true;
            return false;
        }
    }

    var DocObj = null;
    var Lock = false;
    var OpareStr = "";
    var sign = false;

    function LoadDocFunc(Str, url, OtherStr) {
        if (Lock) {
            alert("您上次申请的任务系统还未完成，请稍等片刻...");
            return;
        }
        Lock = true;
        sign = false;
        OpareStr = Str;
        setTimeout('warn_Win("请稍候...，系统开始执行' + OpareStr + '操作");', 2);

        if (window.ActiveXObject) {
            DocObj = new ActiveXObject("Microsoft.XMLHTTP");
            if (DocObj) {
                DocObj.onreadystatechange = DocChangeFunc;
                url = url.replace(/\?/g, "&");
                url = "/handler/ActionHandler.ashx?action=" + url;
                DocObj.open("POST", url, true);
                DocObj.send(escape(OtherStr));
            }
        }
    }

    function DocChangeFunc() {
        var returnInfo = "";
        var BadInfo = "";
        if (DocObj.readyState == 4) {
            if (DocObj.status == 200) {
                returnInfo = unescape(DocObj.responseText);
                if (returnInfo != null && returnInfo != "NULL" && returnInfo != "") eval(returnInfo);

                if (sign == true) {
                    setTimeout('warn_Win("' + OpareStr + '，操作成功");', 2);
                }
                else {
                    if (BadInfo == "") BadInfo = OpareStr + "，操作失败";
                    setTimeout('warn_Win("' + BadInfo + '");', 2);
                }
            }
            else {
                setTimeout('warn_Win("' + DocObj.status + '/操作执行失败，请您重新操作");', 2);
            }
            Lock = false;
            OpareStr = "";
        }
    }
</script>

</body>
</asp:Content>
