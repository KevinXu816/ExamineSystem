<%@ Page Language="C#" MasterPageFile="~/master/Default.Master" AutoEventWireup="true" CodeBehind="pageleft.aspx.cs" Inherits="ExamineSystem.pageleft" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
<script>
    parent.parent.HideShowDivFunc(true, "Sys");
</script>
<base target="right">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
<body leftmargin="0" topmargin="0">
<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
<tr><td width="158" height="100%">

<table width="158" height="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#6093dc">
  <tr>
    <td valign="top" height="42">
	  <img src="images/title.gif" width="158" height="38">
    </td>
  </tr>
  <tr>
    <td valign="top" height="25" class="menu_title" background="images/title_bg_quit1.gif" style="line-height:25px;">
	  <span>欢迎您： <span id="namplane"><%=this.CurrentUserName%></span> <br> <a href="javascript:" onClick="exitfunc();"><b>退出校园网络考试系统</b></a></span>    </td>
  </tr>
  <tr>
    <td valign="top" height="20"></td>
  </tr>
  <tr>
    <td valign="top" height="25" class="menu_title" background="images/title_bg_quit.gif" style="line-height:25px;">
	  <span>功能选项</span>	</td>
  </tr>
  <tr>
    <td class="sec_menu" valign="top" height="70">
		<table cellpadding="0" cellspacing="0" align="center" width="156">
			<tr><td height="10"> </td></tr>
			<tr><td> &nbsp;<img src="images/bullet.gif" width="15" height="20" border="0" align="absmiddle"> <a  href="pagestart.aspx"  onclick ="return inFuncPageLike();">简介及其帮助</a></td></tr>
            <% if (this.CurrentUserLevel == 1) {%>
			<tr><td> &nbsp;<img src="images/bullet.gif" border="0" width="15" height="20" align="absmiddle"> <a href="pageexamine.aspx" onclick="return inFuncPageLike();">进入网络考场</a></td></tr>
            <% } %>
            <% if (this.CurrentUserLevel == 1) {%>
			<tr><td> &nbsp;<img src="images/bullet.gif" border="0" width="15" height="20" align="absmiddle"> <a href="pagehistory.aspx" onclick="return inFuncPageLike();">历史分数查询</a></td></tr>
            <% } %>
            <% if (this.CurrentUserLevel == 2) {%>
			<tr><td> &nbsp;<img src="images/bullet.gif" border="0" width="15" height="20" align="absmiddle"> <a href="pagehistoryadv.aspx" onclick="return inFuncPageLike();">历史分数查询</a></td></tr>
            <% } %>
			<% if (this.CurrentUserLevel == 2 || this.CurrentUserLevel == 3) {%>
			<tr><td> &nbsp;<img src="images/bullet.gif" border="0" width="15" height="20" align="absmiddle"> <a href="pagequestion.aspx" onclick="return inFuncPageLike();">考试题目维护</a></td></tr>
			<% } %>
			<% if (this.CurrentUserLevel == 3){ %>
			<tr><td> &nbsp;<img src="images/bullet.gif" border="0" width="15" height="20" align="absmiddle"> <a href="pagemanage.aspx" onclick="return inFuncPageLike();">系统人员维护</a></td></tr>
			<% } %>
			<tr><td> &nbsp;<img src="images/bullet.gif" border="0" width="15" height="20" align="absmiddle"> <a href="pageselfinfo.aspx" onclick="return inFuncPageLike();">自我信息修改</a></td></tr>
			<tr><td height="10"> </td></tr>
		</table>
	</td>
  </tr>
  <tr>
    <td valign="top" height="20"> </td>
  </tr>
  <tr>
    <td valign="top" height="25" class="menu_title" background="images/title_bg_quit.gif" style="line-height:25px;">
	  <span>系统提示</span>	</td>
  </tr>
  <tr>
    <td class="sec_menu" valign="top" height="99">
	<table cellpadding="0" cellspacing="0" align="center" width="156">
		<tr><td align="center" valign="middle" height="99">
			<span id="InfoSpan">欢迎您使用<br>校园网络在线考试系统</span>
		</td></tr>
	</table>
	</td>
  </tr>
  <tr>
    <td valign="top" height="20"> </td>
  </tr>
   <tr>
    <td valign="top" height="25" class="menu_title" background="images/title_bg_quit.gif" style="line-height:25px;">
	  <span>制作信息</span>	</td>
  </tr>
  <tr>
    <td class="sec_menu" valign="top" height="30">
	<table cellpadding="0" cellspacing="0" align="center" width="156">
			<tr><td height="10"> </td></tr>
			<tr><td> &nbsp;<img src="images/bullet.gif" width="15" height="20" border="0" align="absmiddle"> 制作信息</td></tr>
			<tr><td> &nbsp;<img src="images/bullet.gif" border="0" width="15" height="20" align="absmiddle"> 制作工具</td></tr>
			<tr><td> &nbsp;<img src="images/bullet.gif" border="0" width="15" height="20" align="absmiddle"> 相关技术</td></tr>
			<tr><td height="10"> </td></tr>
		</td></tr>
	</table>
	</td>
  </tr>
  <tr>
   <td height="100%" valign="bottom" align="center">
   		<img src="images/iconBig.gif">
   </td>
  </tr>
</table>
</td>
<td width="1" bgcolor="#183789">
</td>
</tr>
</table>

<script language="javascript">

    var examing = false;
    function inFuncPageLike() {
        if (examing) {
            alert("当前正在考试期间，在您没有提交试卷前不能离开考场！");
            return false;
        }
        else {
            loadingFuncPage();
            return true;
        }
    }

    function TimeShow(Str) {
        document.getElementById("InfoSpan").innerHTML = Str;
    }

    function exitfunc() {
        if (confirm("您是否真要退出校园网络考试系统吗？")) {
            Leave = false;
            top.location.href = "pageexit.aspx";
        }
    }

    function changename(txt) {
        document.getElementById("namplane").innerHTML = txt;
    }

    function loadingFuncPage() {
        parent.parent.HideShowDivFunc(true);
    }

    window.onerror = function () {
        parent.parent.warn_Win("系统引擎运行出现脚本错误<br>给您带来不便还请谅解", 1);
        Lock = false;
        return true;
    }

    var Leave = true;
    var TmpStr = "";
    var IfAsyn = true;
    var DocObj = null;
    var Lock = false;
    var OpareStr = "";
    var RunFunc = [];
    var BadFunc = [];
    var CancelFunc = [];
    var DataArray = [];
    var sign = false;
    var TmpUrlS = "";
    var TmpOtherStr = "";

    function LoadDocFunc(Str, url, FuncArray, BFuncArray, OtherStr, CFuncArray) {
        if (Lock) {
            if (confirm("上一任务还未完成！您是否要取消，而进行本次任务\n警告：此操作可能有一定风险，并且已执行完的部分无法取消！")) {
                DocObj.abort();
                DocObj = null;
                if (CancelFunc != null) {
                    for (var i = 0; i < CancelFunc.length; i++) {
                        try {
                            eval("parent.right." + CancelFunc[i]);
                        }
                        catch (e) { }
                    }
                    CancelFunc = [];
                }
            }
            else {
                return;
            }
        }
        Lock = true;
        sign = false;
        RunFunc = FuncArray;
        BadFunc = BFuncArray;
        CancelFunc = CFuncArray;
        OpareStr = Str;
        TmpUrlS = url;
        TmpOtherStr = (OtherStr != null) ? OtherStr : "";
        setTimeout('parent.parent.warn_Win("请稍候...<br>系统开始执行' + OpareStr + '操作",0);', 2);

        if (window.ActiveXObject) {
            DocObj = new ActiveXObject("Microsoft.XMLHTTP");
            if (DocObj) {
                DocObj.onreadystatechange = DocChangeFunc;
                url = url.replace(/\?/g, "&");
                url = "/handler/ActionHandler.ashx?action=" + url;
                DocObj.open("POST", url, IfAsyn);
                if (OtherStr == null) {
                    DocObj.send();
                }
                else {
                    DocObj.send(escape(OtherStr));
                }
            }
        }
    }

    function DocChangeFunc() {
        var returnInfo = "";
        var BadInfo = "";
        var ClearFlag = true;
        if (DocObj.readyState == 4) {
            if (DocObj.status == 200) {
                returnInfo = unescape(DocObj.responseText);
                if (returnInfo != null && returnInfo != "NULL" && returnInfo != "") eval(returnInfo);

                if (sign == true) {
                    setTimeout('parent.parent.warn_Win("' + OpareStr + '<br>操作成功",1);', 2);
                    if (RunFunc != null) {
                        for (var i = 0; i < RunFunc.length; i++) {
                            try {
                                eval("parent.right." + RunFunc[i]);
                            }
                            catch (e) { }
                        }
                        RunFunc = [];
                    }
                }
                else {
                    if (BadInfo == "") BadInfo = OpareStr + "<br>操作失败";
                    setTimeout('parent.parent.warn_Win("' + BadInfo + '",1);', 2);
                    if (BadFunc != null) {
                        for (var i = 0; i < BadFunc.length; i++) {
                            try {
                                eval("parent.right." + BadFunc[i]);
                            }
                            catch (e) { }
                        }
                        BadFunc = [];
                    }
                }
            }
            else {
                var ErrorNumVal = DocObj.status;
                if (ErrorNumVal == "404") {
                    DocObj = null;
                    Lock = false;
                    ClearFlag = false;
                }
                else {
                    setTimeout('parent.parent.warn_Win("' + ErrorNumVal + '/操作执行失败，请您重新操作",1);', 2);
                    if (BadFunc != null) {
                        for (var i = 0; i < BadFunc.length; i++) {
                            try {
                                eval("parent.right." + BadFunc[i]);
                            }
                            catch (e) { }
                        }
                        BadFunc = [];
                    }
                    //var win = window.open();
                    //win.document.open('text/html', 'replace');
                    //win.document.writeln(unescape(DocObj.responseText));
                    //win.document.close();
                }
            }
            if (ClearFlag) {
                TmpUrlS = "";
                TmpOtherStr = "";
                Lock = false;
                IfAsyn = true;
                TmpStr = "";
                OpareStr = "";
                RunFunc = [];
                BadFunc = [];
                CancelFunc = [];
                DataArray = [];
            }
        }
    }

    /////////////////////////////////////////////////////////////////////

    var SDocObj = null;
    var SLock = false;
    var SRunFunc = [];
    var SBadFunc = [];
    var SCancelFunc = [];
    var SDataArray = [];
    var STmpStr = "";
    var Ssign = false;
    var STmpUrlS = "";
    var STmpOtherStr = "";

    function SLoadDocFunc(Surl, SFuncArray, SBFuncArray, SOtherStr, SCFuncArray) {

        if (SLock) {
            SDocObj.abort();
            SDocObj = null;
            if (SCancelFunc != null) {
                for (var i = 0; i < SCancelFunc.length; i++) {
                    try {
                        eval("parent.right." + SCancelFunc[i]);
                    }
                    catch (e) { }
                }
                SCancelFunc = [];
            }
        }

        SLock = true;
        Ssign = false;
        SRunFunc = SFuncArray;
        SBadFunc = SBFuncArray;
        SCancelFunc = SCFuncArray;
        STmpUrlS = Surl;
        STmpOtherStr = (SOtherStr != null) ? SOtherStr : "";

        if (window.ActiveXObject) {
            SDocObj = new ActiveXObject("Microsoft.XMLHTTP");
            if (SDocObj) {
                SDocObj.onreadystatechange = SDocChangeFunc;
                Surl = Surl.replace(/\?/g, "&");
                Surl = "/handler/ActionHandler.ashx?os=1&action=" + Surl;
                SDocObj.open("POST", Surl, true);
                if (SOtherStr == null) {
                    SDocObj.send();
                }
                else {
                    SDocObj.send(escape(SOtherStr));
                }
            }
        }
    }

    function SDocChangeFunc() {
        var returnInfo = "";
        var BadInfo = "";
        var ClearFlag = true;
        if (SDocObj.readyState == 4) {
            if (SDocObj.status == 200) {
                returnInfo = unescape(SDocObj.responseText);
                if (returnInfo != null && returnInfo != "NULL" && returnInfo != "") eval(returnInfo);

                if (Ssign == true) {
                    if (SRunFunc != null) {
                        for (var i = 0; i < SRunFunc.length; i++) {
                            try {
                                eval("parent.right." + SRunFunc[i]);
                            }
                            catch (e) { }
                        }
                        SRunFunc = [];
                    }
                }
                else {
                    if (SBadFunc != null) {
                        for (var i = 0; i < SBadFunc.length; i++) {
                            try {
                                eval("parent.right." + SBadFunc[i]);
                            }
                            catch (e) { }
                        }
                        SBadFunc = [];
                    }
                }
            }
            else {
                if (SDocObj.status == "404") {
                    SDocObj = null;
                    SLock = false;
                    ClearFlag = false;
                }
                else {
                    if (SBadFunc != null) {
                        for (var i = 0; i < SBadFunc.length; i++) {
                            try {
                                eval("parent.right." + SBadFunc[i]);
                            }
                            catch (e) { }
                        }
                        SBadFunc = [];
                    }
                    //var win = window.open();
                    //win.document.open('text/html', 'replace');
                    //win.document.writeln(unescape(SDocObj.responseText));
                    //win.document.close();
                }
            }

            if (ClearFlag) {

                STmpStr = "";
                SDataArray = [];
                SLock = false;
                SRunFunc = [];
                SBadFunc = [];
                SCancelFunc = [];
                STmpUrlS = "";
                STmpOtherStr = "";
            }
        }
    }

</script>
</body>

<script>
    parent.parent.HideShowDivFunc(false, "Sys");
</script>

</asp:Content>