<%@ Page Language="C#" MasterPageFile="~/master/Default.Master" AutoEventWireup="true" CodeBehind="welcome.aspx.cs" Inherits="ExamineSystem.welcome" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
<body onResize = "controlPos();controlDivSize();controlDivSizeMenu();" topmargin="0" leftmargin="0" scroll="no">
<script language="JavaScript">
var optmp = !!(window.opera && document.getElementById);	
var ietype = !!(navigator.userAgent.toLowerCase().indexOf("msie") >= 0 && document.all && !optmp);
if (!ietype){
	alert("您使用的浏览器不符合校园网络考试系统需要，本系统需要运行在IE浏览器中！");
	top.location = "login.aspx";
}

if(top.location != self.location) top.location=self.location;

</script>

<script src="/script/functions.js" type=text/javascript></script>
<script language="javascript">
    function LoadSysPage() {
        try {
            hhbackpage.left.location = "pageleft.aspx";
        }
        catch (e) { }
    }
</script>

<DIV id="InfoInputColor" STYLE="position: absolute; filter: alpha(opacity=70); top: 0px; left: 159px; width: 999px; height: 999px; z-index: 996; background-color: #5e86d7; display: block;"></DIV>
<DIV id="InfoInput" STYLE="position: absolute; top: 0px; left: 159px; width: 999px; height: 999px; z-index: 997; display: block;">


<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
<tr>
<td align="center" valign="middle">
<img src="images/loading.gif" width="150" height="13"><font color="ffffff"><br><br>
&nbsp;&nbsp;功能页面加载中，请您稍等片刻...<br><br>
<br>
提示：如果页面长时间没有反映，请您重新单击此功能页面
</font></td>
</tr>
</table>


</DIV>


<DIV id="InfoInputColorMenu" STYLE="position: absolute; filter: alpha(opacity=70); top: 0px; left: 0px; width: 160px; height: 999px; z-index: 996; background-color: #5e86d7; display: block;"></DIV>
<DIV id="InfoInputMenu" STYLE="position: absolute; top: 0px; left: 0px; width: 160px; height: 999px; z-index: 997; display: block;">


<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
<tr>
<td align="center" valign="middle">
<img src="images/loading.gif" width="150" height="13"><font color="ffffff"><br><br>
系统引擎页面加载中<br>请您稍等片刻...<br><br>
<br>
提示：如页面长时间没有反映，<a href="##" onClick="LoadSysPage();"><font color="ffffff">请您单击此处</font></a>
</font></td>
</tr>
</table>


</DIV>

<script language="javascript">

    function document.body.onbeforeunload() {
        if (document.hhbackpage.left.Leave) {
            if (document.hhbackpage.left.Lock) {
                event.returnValue = "您的后台操作还未完成，是否真要结束操作并离开页面？\n警告：此操作可能有一定风险，并且已执行完的部分无法取消！\n例如：会发生下次无法登陆现象！";
            }
            else {
                event.returnValue = "您未按照正常流程离开系统，建议您单击“退出按钮”离开系统，您是否还需继续离开？\n警告：此离开操作可能有一定风险，并且导致您下次无法登陆！";
            }
            event.cancelBubble = true;
            return false;
        }
    }

    function controlDivSizeMenu() {
        var InfoDivStyle = document.getElementById("InfoInputMenu").style;
        var InfoDivStyleColor = document.getElementById("InfoInputColorMenu").style;
        if (InfoDivStyle.display == "block" && InfoDivStyleColor.display == "block") {
            var bodyheight = document.body.clientHeight;
            InfoDivStyle.height = bodyheight + "px";
            InfoDivStyleColor.height = bodyheight + "px";
        }
    }

    function controlDivSize() {
        var InfoDivStyle = document.getElementById("InfoInput").style;
        var InfoDivStyleColor = document.getElementById("InfoInputColor").style;
        if (InfoDivStyle.display == "block" && InfoDivStyleColor.display == "block") {
            var bodywidth = document.body.clientWidth;
            var bodyheight = document.body.clientHeight;
            InfoDivStyle.width = (bodywidth - 152) + "px";
            InfoDivStyle.height = bodyheight + "px";
            InfoDivStyleColor.width = (bodywidth - 152) + "px";
            InfoDivStyleColor.height = bodyheight + "px";
        }
    }

    function HideShowDivFunc(Flag, DivN) {
        if (DivN == null) {
            var InfoDivStyle = document.getElementById("InfoInput").style;
            var InfoDivStyleColor = document.getElementById("InfoInputColor").style;
        }
        else {
            var InfoDivStyle = document.getElementById("InfoInputMenu").style;
            var InfoDivStyleColor = document.getElementById("InfoInputColorMenu").style;
        }
        if (Flag) {
            InfoDivStyle.display = "block";
            InfoDivStyleColor.display = "block";
            if (DivN == null) {
                controlDivSize();
            }
            else {
                controlDivSizeMenu();
            }
        }
        else {
            InfoDivStyle.display = "none";
            InfoDivStyleColor.display = "none";
        }
    }

    controlDivSize();
    controlDivSizeMenu();
</script>



<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td>
		<IFRAME marginHeight=0 marginWidth=0 topmargin=0 src="titlepage.aspx" frameBorder=0 width="100%" scrolling=no height="100%" leftmargin="0" name="hhbackpage">
		</IFRAME>
	</td>
  </tr>
</table>
</body>
</asp:Content>
