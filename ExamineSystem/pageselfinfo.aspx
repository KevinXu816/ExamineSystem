<%@ Page Title="" Language="C#" MasterPageFile="~/master/Default.Master" AutoEventWireup="true" CodeBehind="pageselfinfo.aspx.cs" Inherits="ExamineSystem.pageselfinfo" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
<script>
    parent.parent.HideShowDivFunc(true);
</script>
<script language="JavaScript">

    window.onerror = function () {
        parent.parent.warn_Win("系统运行出现脚本错误<br>给您带来不便还请谅解", 1);
        return true;
    }

    function ItemUpdate(userid) {
        if (!confirm("您真的要编辑您的相关信息吗？\n编辑后将无法恢复")) return;
        if (checkFunc(userid)) {
            var TdObj = document.getElementById("td" + userid);
            var OldWord = TdObj.parentElement.parentElement.rows[0].cells[1].innerText;
            var name = document.getElementById("name" + userid);
            var password = document.getElementById("password" + userid);
            var oldpassword = document.getElementById("oldpassword" + userid);

            if (name.value == OldWord && password.value == "") {
                alert("您还没有更改任何信息，请更改后再进行操作！");
                return;
            }
            else {
                if (oldpassword.value != "") {
                    var RunFunc = new Array("PageUpdateFunc('" + userid + "','" + name.value + "');");
                    var userinfo = ((name.value != OldWord) ? (name.value) : "") + "'" + ((password.value != "") ? (password.value) : "") + "'" + ((oldpassword.value != "") ? (oldpassword.value) : "");
                    parent.left.LoadDocFunc("编辑自我信息", "pageselfinfo?type=edit&id=" + userid, RunFunc, null, userinfo);
                }
                else {
                    alert("更改任何信息都需要您提供原始密码");
                    oldpassword.select();
                }
            }
        }
    }

    function PageUpdateFunc(userid, username) {
        document.getElementById("password" + userid).value = "";
        document.getElementById("repassword" + userid).value = "";
        document.getElementById("oldpassword" + userid).value = "";
        var TdObj = document.getElementById("td" + userid);
        var MainTable = TdObj.parentElement.parentElement;
        var MainWord = MainTable.rows[0].cells[1];
        var MainTRObj = MainTable.rows[1].cells[0];
        var ShowEditObj = MainTRObj.childNodes[1];

        MainWord.innerHTML = "<B>" + username + "</B>";
        if (TdObj.style.display == "none") {
            ShowEditObj.alt = "展开编辑" + username + "用户菜单";
        }
        else {
            ShowEditObj.alt = "收缩编辑" + username + "用户菜单";
        }
        parent.left.changename(username);
    }



    function ItemReSet(id) {
        var TdObj = document.getElementById("td" + id);
        var username = TdObj.parentElement.parentElement.rows[0].cells[1].innerText;
        document.getElementById("name" + id).value = username;
        document.getElementById("password" + id).value = "";
        document.getElementById("repassword" + id).value = "";
        document.getElementById("oldpassword" + id).value = "";
    }


    function ShowEditDiv(userid) {
        var TdObj = document.getElementById("td" + userid);
        var username = TdObj.parentElement.parentElement.rows[0].cells[1].innerText;
        var ImgButtonObj = TdObj.parentElement.parentElement.rows[1].cells[0].childNodes[1];
        if (TdObj.style.display == "none") {
            ImgButtonObj.alt = "收缩编辑" + username + "用户菜单";
            TdObj.style.display = "block";
        }
        else {
            ImgButtonObj.alt = "展开编辑" + username + "用户菜单";
            TdObj.style.display = "none";
        }
    }

    function checkFunc(userid) {
        var name = document.getElementById("name" + userid);
        var password = document.getElementById("password" + userid);
        var repassword = document.getElementById("repassword" + userid);
        var oldpassword = document.getElementById("oldpassword" + userid);

        var RegObjS = /<[!a-z\/]|<$|[%\"\'\\\/ ,;\?&]/i;
        if (RegObjS.test(name.value)) {
            alert("您输入的用户名称中含有非法字符！");
            name.select();
            return false;
        }
        if (name.value == "") {
            alert("请您务必要输入用户名称！");
            name.select();
            return false;
        }
        if (oldpassword.value != "" && RegObjS.test(oldpassword.value)) {
            alert("输入的原始密码中有非法字符！");
            oldpassword.select();
            return false;
        }
        if (password.value != "" && password.value.length < 5) {
            alert("初始密码要求在5位以上！");
            password.select();
            return false;
        }
        if (password.value != "" && RegObjS.test(password.value)) {
            alert("输入的密码中有非法字符！");
            password.select();
            return false;
        }
        if (password.value != repassword.value) {
            alert("密码两次输入的不一致！");
            password.select();
            return false;
        }

        var RegExpJPER = /[\u30B4|\u30AC|\u30AE|\u30B0|\u30B2|\u30B6|\u30B8|\u30BA|\u30C5|\u30C7|\u30C9|\u30DD|\u30D9|\u30D7|\u30D3|\u30D1|\u30F4|\u30DC|\u30DA|\u30D6|\u30D4|\u30D0|\u30C2|\u30C0|\u30BE|\u30BC]/g;
        name.value = name.value.replace(RegExpJPER, "□");
        password.value = password.value.replace(RegExpJPER, "□");
        oldpassword.value = oldpassword.value.replace(RegExpJPER, "□");

        return true;
    }
</script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
<body topmargin="5" leftmargin="0">

<div align="center">
  <table border="0" cellpadding="0" cellspacing="0" class="TableStyle">
    <tr>
      <td height="22" align="center" class="HeadStyle">自我信息列表</td>
    </tr>
    <tr>
      <td align="center" class="BodyStyle">
      <table border="0" cellpadding="0" cellspacing="0"  id="mangerlist">
        <tr>
          <td>
			<table border="0" cellpadding="0" cellspacing="0" width="550" height="50">
		  		<tr>
		  			<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<img src="images/admin.gif" width="11" height="14"></td>
		  			<td align="center"><b><%=this.CurrentUserName%></b></td>
		  		</tr>
		  		<tr>
		  			<td align="left"><span style="width: 480px;">&nbsp; 所属权限组别：<%=this.CurrentUserLevelName%></span><img src="images/edit.gif" alt="展开编辑<%=this.CurrentUserName%>用户菜单" onClick="ShowEditDiv('<%=this.CurrentUserId%>');"> &nbsp; </td>
		  		</tr>
				<tr>
					<td colspan="2" align="center" style="display: none;" id="td<%=this.CurrentUserId%>">
					<!--------------------------------------------------------------------->
						<table border="0" cellpadding="0" cellspacing="0" width="85%" height="130">
							<tr>
								<td>
								用户名称：<input type="text" name="name<%=this.CurrentUserId%>" id="name<%=this.CurrentUserId%>" size="14" maxlength="14" value="<%=this.CurrentUserName%>" class="ip1"><br><br>
								原始密码：<input type="password" name="oldpassword<%=this.CurrentUserId%>" id="oldpassword<%=this.CurrentUserId%>" size="14" maxlength="14" class="ip1">
								</td>
								<td align="center">
								更新密码：<input type="password" name="password<%=this.CurrentUserId%>" id="password<%=this.CurrentUserId%>" size="14" maxlength="14" class="ip1"><br><br>
								重复一次：<input type="password" name="repassword<%=this.CurrentUserId%>" id="repassword<%=this.CurrentUserId%>" size="14" maxlength="14" class="ip1">
								</td>
							</tr>
						</table>
						<span style="width: 450;"></span><img src="images/create.gif" alt="应用更新" onClick="ItemUpdate('<%=this.CurrentUserId%>');"> &nbsp; &nbsp; <img src="images/back.gif" alt="恢复初值" onClick="ItemReSet('<%=this.CurrentUserId%>');"> &nbsp;
					<!--------------------------------------------------------------------->
					</td>
				</tr>
		  	</table>
		  </td>
        </tr>
      </table>
      </td>
    </tr>

  </table>
</div>
</body>

<script>
    parent.parent.HideShowDivFunc(false);
</script>

</asp:Content>