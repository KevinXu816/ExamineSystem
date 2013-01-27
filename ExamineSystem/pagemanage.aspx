<%@ Page Title="" Language="C#" MasterPageFile="~/master/Default.Master" AutoEventWireup="true" CodeBehind="pagemanage.aspx.cs" Inherits="ExamineSystem.pagemanage" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
<script language="javascript">
    parent.parent.HideShowDivFunc(true);
</script>

<script src="script/PagesFunc.js" type=text/javascript></script>
<script language="JavaScript">
window.onerror = function (){
  parent.parent.warn_Win("系统运行出现脚本错误<br>给您带来不便还请谅解",1);
  return true;
}

String.prototype.trim = function(){
	return this.replace(/(^\s*)|(\s*$)/g, "");
}

String.prototype.hchar = function(){
	return this.replace(/-*/g, "");
}

String.prototype.change = function(){
	var returnTxt = this;
	if (returnTxt=="False" || returnTxt=="false" || returnTxt=="FALSE") returnTxt="0";
	if (returnTxt=="True" || returnTxt=="true" || returnTxt=="TRUE") returnTxt="1";
	return returnTxt;
}

function ItemUpdate(userid){
	if (checkFunc(userid)){
		var TdObj = document.getElementById("td"+userid);
		var num = document.getElementById("num"+userid);
		var name = document.getElementById("name"+userid);
		var password = document.getElementById("password"+userid);
		var type = document.getElementById("type"+userid);
		var login = document.getElementById("status"+userid);
		var dostr = document.getElementById("do"+userid);
		var numstr = (type.value=="1")?num.value:("-"+num.value);
		var hiddenarray = document.getElementById("hidden"+userid).innerHTML.split("'");
		var oldnumstr = (hiddenarray[1]=="1")?hiddenarray[0]:("-"+hiddenarray[0]);
		if (numstr==oldnumstr){
			numstr="";
		}
		var t_str = name.value+"'"+type.value+"'"+login.value+"'"+dostr.value;
		var FuncStr = num.value+"'"+t_str;
 		var RunFunc = new Array("PageUpdateFunc(\""+userid+"\",\""+FuncStr+"\");");
		var PostStrT = userid+"'"+numstr+"'"+t_str+"'"+password.value;
 		parent.left.LoadDocFunc("编辑用户","pagemanage?type=edit",RunFunc,null,PostStrT);
	}
}

function PageUpdateFunc(userid,info){
	var infoarray = info.split("'");
	var TdObj = document.getElementById("td"+userid);
	var passobj = document.getElementById("password"+userid);
	var repassobj = document.getElementById("repassword"+userid);
	var nameobj = document.getElementById("name"+userid);
	var numobj = document.getElementById("num"+userid);
	var typeobj = document.getElementById("type"+userid);
	var loginobj = document.getElementById("status"+userid);
	var doobj = document.getElementById("do"+userid);
	var hiddenobj = document.getElementById("hidden"+userid);
	passobj.value = "";
	repassobj.value = "";
	numobj.value = infoarray[0];
	nameobj.value = infoarray[1];
	typeobj.value = infoarray[2];
	loginobj.value = infoarray[3];
	doobj.value = infoarray[4];
	hiddenobj.innerHTML = infoarray[0]+"'"+infoarray[2]+"'"+infoarray[3]+"'"+infoarray[4];
	var cuserid = "<%=this.CurrentUserId%>".trim();
	if (cuserid==userid){
		parent.left.changename(infoarray[1]);
	}
	
	var MainTable = TdObj.parentElement.parentElement;
	var MainWord = MainTable.rows[0].cells[1];
	var MainTRObj = MainTable.rows[1].cells[0];
	var FlagWord = MainTRObj.childNodes[0];
	var ShowEditObj = MainTRObj.childNodes[1]; 
	var DelImgObj = MainTRObj.childNodes[3];
	MainWord.innerHTML = "<B>"+ infoarray[1] +"</B>";
	var FlagStr = "普通学员考生";
	switch(infoarray[2]){
		case "1":
			FlagStr = "普通学员考生";
		break;
		case "2":
			FlagStr = "试题维护教师";
		break;
		case "3":
			FlagStr = "系统级管理员";
		break;
	}
	FlagWord.innerHTML = "&nbsp;<img src='images/admin.gif' width='11' height='14'>&nbsp; 所属权限组别：" + FlagStr;
	DelImgObj.alt = "删除"+ infoarray[1] +"用户";	
	if (TdObj.style.display == "none"){
		ShowEditObj.alt = "展开编辑"+ infoarray[1] +"用户菜单";
	}
	else{
		ShowEditObj.alt = "收缩编辑"+ infoarray[1] +"用户菜单";
	}

}


function ItemReSet(id){
	var TdObj = document.getElementById("td"+id);
	document.getElementById("name"+id).value = TdObj.parentElement.parentElement.rows[0].cells[1].innerText;
	document.getElementById("password"+id).value = "";
	document.getElementById("repassword"+id).value = "";
	var hiddenArray = document.getElementById("hidden"+id).innerHTML.split("'");
	document.getElementById("num"+id).value = hiddenArray[0];
	document.getElementById("type"+id).value = hiddenArray[1];
	document.getElementById("status"+id).value = hiddenArray[2].change();
	document.getElementById("do"+id).value = hiddenArray[3].change();
}


function ShowEditDiv(userid){
	var TdObj = document.getElementById("td"+userid);
	var username = TdObj.parentElement.parentElement.rows[0].cells[1].innerText;
	var ImgButtonObj = TdObj.parentElement.parentElement.rows[1].cells[0].childNodes[1];
	if (TdObj.style.display == "none"){
		ImgButtonObj.alt = "收缩编辑"+ username +"用户菜单";
		TdObj.style.display = "block";
	}
	else{
		ImgButtonObj.alt = "展开编辑"+ username +"用户菜单";
		TdObj.style.display = "none";
	}
}

function DelRecord(hisid){
	var Historyid = document.getElementById("historyid").innerHTML.trim();
	if(confirm("您真的要删除此记录及其所选记录吗？\n删除后将无法恢复")){
		var currentPageNum = SysHistoryListPages.page;
		var InputObjS = document.getElementsByName("HistroyItemName");
		for (var i=0; i<InputObjS.length; i++){
			if (InputObjS[i].type == "checkbox" && InputObjS[i].checked){
				if (InputObjS[i].value!=hisid){
					hisid = hisid+","+InputObjS[i].value;
				}
			}
		}
		var RunFunc = new Array("DelRecordRunFunc("+ currentPageNum +");");
		parent.left.LoadDocFunc ("删除历史记录","pagemanage?type=delhistory&id="+hisid+"&PageN="+currentPageNum,RunFunc,null,Historyid);
	}
}

function DelRecordRunFunc(pageNum){
	CreateHistoryList(pageNum);
	parent.left.DataArray = [];
}


function history(userid,username,usernum){
	var RunFunc = new Array("historyRunFunc(\""+userid+"\",\""+username+"\",\""+usernum+"\");");
	parent.left.LoadDocFunc("读取用户历史记录","pagemanage?type=historylist&pagenum=1",RunFunc,null,userid);
}

function historyRunFunc(userid,username,usernum){
	document.getElementById("historyid").innerHTML = userid;
	document.getElementById("historyname").innerHTML = username+"的历史分数记录（编号："+usernum+"）";
	CreateHistoryList(1);
	parent.left.DataArray = [];
	var historypanelobj = document.getElementById("historypanel");
	if (historypanelobj.style.display=="none"){
		historypanelobj.style.display = "block";
	}
}

function historyPanelControl(){
	var historypanelobj = document.getElementById("historypanel");
	historypanelobj.style.display = "none";
	document.getElementById("historyid").innerHTML = "";
	document.getElementById("historyname").innerHTML = "历史分数记录";
}

function CreateHistoryList(PageNumT,PageSum){
	if (PageNumT==null) PageNumT=SysHistoryListPages.page;
	if (PageSum==null) PageSum = parent.left.TmpStr;
	SysHistoryListPages.changePageCount(PageSum,PageNumT);
	var DataArray = parent.left.DataArray;
	var MangerListObjT = document.getElementById("historylist");
	for (var i=0; i<MangerListObjT.childNodes.length; i++){
		MangerListObjT.childNodes[i].removeNode(true);
	}
	for (var i=0; i<DataArray.length; i++){	
		CreateHistoryRecord(DataArray[i]);
	}
}

function ShowPaper(Papid){
	alert("功能还在开发中");
}

function CreateHistoryRecord(ItemObj){
	var MangerListObjT = document.getElementById("historylist");
	var RowNum = MangerListObjT.rows.length;
	var CurrentRow = MangerListObjT.insertRow(RowNum);
	var CurrentCell = CurrentRow.insertCell(0);
	var CreateHTML = '';
	CreateHTML += '<table border="0" cellpadding="0" cellspacing="0" width="550" height="50">';
	CreateHTML += '<tr>';
	CreateHTML += '<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="'+ ItemObj.history_id +'" name="HistroyItemName" title="同步操作复选框"></td>';
	CreateHTML += '<td align="center"><b>'+ ItemObj.history_ip +' &nbsp; （'+ ItemObj.history_score +'分）</b></td>';
	CreateHTML += '</tr>';
	CreateHTML += '<tr>';
	CreateHTML += '<td align="left"><span style="width: 430px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 时间：从 '+ ItemObj.history_stime +' 到 '+ ItemObj.history_etime +' </span><img src="images/edit.gif" alt="查看试卷" onClick="ShowPaper(\''+ ItemObj.history_id +'\');" style="display:none;"> &nbsp; <img src="images/del.gif" alt="删除记录" onClick="DelRecord(\''+ ItemObj.history_id +'\');"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的记录操作选框" onClick="selectHistoryItem();"></td>';
	CreateHTML += '</tr>';
	CreateHTML += '</table>';
	CurrentCell.innerHTML = CreateHTML;
}



function CreateRowFunc(ItemObj){
	var MangerListObjT = document.getElementById("mangerlist");
	var RowNum = MangerListObjT.rows.length;
	var CurrentRow = MangerListObjT.insertRow(RowNum);
	var CurrentCell = CurrentRow.insertCell(0);
	var FlagStr = "普通学员考生";
	switch(ItemObj.user_func){
		case "1":
			FlagStr = "普通学员考生";
		break;
		case "2":
			FlagStr = "试题维护教师";
		break;
		case "3":
			FlagStr = "系统级管理员";
		break;
	}
	var Usernum = ItemObj.user_num.hchar();
	ItemObj.user_login = ItemObj.user_login.change();
	ItemObj.user_do = ItemObj.user_do.change();
	var CreateHTML = '';
	CreateHTML += '<table border="0" cellpadding="0" cellspacing="0" width="550" height="50">';
	CreateHTML += '<tr>';
	CreateHTML += '<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="'+ ItemObj.user_id +'" name="ManageItemName" title="同步操作复选框"></td>';
	CreateHTML += '<td align="center"><b>'+ ItemObj.user_name +'</b></td>';
	CreateHTML += '</tr>';
	CreateHTML += '<tr>';
	CreateHTML += '<td align="left"><span style="width: 430px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 所属权限组别：'+ FlagStr +'</span><img src="images/edit.gif" alt="展开编辑'+ ItemObj.user_name +'用户菜单" onClick="ShowEditDiv(\''+ ItemObj.user_id +'\');"> &nbsp; <img src="images/del.gif" alt="删除'+ ItemObj.user_name +'用户" onClick="DelFunc(\''+ ItemObj.user_id +'\');"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的人员操作选框" onClick="selectManageItem();"></td>';
	CreateHTML += '</tr>';
	CreateHTML += '<tr>';
	CreateHTML += '<td colspan="2" align="center" style="display: none;" id="td'+ ItemObj.user_id +'">';
	CreateHTML += '<table border="0" cellpadding="0" cellspacing="0" width="85%" height="150">';
	CreateHTML += '<tr>';
	CreateHTML += '<td>';
	CreateHTML += '用户编号：<input type="text" name="num'+ ItemObj.user_id +'" id="num'+ ItemObj.user_id +'" size="14" maxlength="14" value="'+ Usernum +'" class="ip1"><br><br>';
	CreateHTML += '用户名称：<input type="text" name="name'+ ItemObj.user_id +'" id="name'+ ItemObj.user_id +'" size="14" maxlength="14" value="'+ ItemObj.user_name +'" class="ip1"><br><br>';
	CreateHTML += '更新密码：<input type="password" name="password'+ ItemObj.user_id +'" id="password'+ ItemObj.user_id +'" size="14" maxlength="14" class="ip1"><br><br>';
	CreateHTML += '重复一次：<input type="password" name="repassword'+ ItemObj.user_id +'" id="repassword'+ ItemObj.user_id +'" size="14" maxlength="14" class="ip1">';
	CreateHTML += '</td>';
	CreateHTML += '<td align="right">';
	CreateHTML += '用户权限：<select size="1" name="type'+ ItemObj.user_id +'" id="type'+ ItemObj.user_id +'"><option value="1" '+ ((ItemObj.user_func=="1")?"selected":"") +'>普通学员考生</option><option value="2" '+ ((ItemObj.user_func=="2")?"selected":"") +'>试题维护教师</option><option value="3" '+ ((ItemObj.user_func=="3")?"selected":"") +'>系统级管理员</option></select><br><br>';
	CreateHTML += '登陆状态：<select size="1" name="status'+ ItemObj.user_id +'" id="status'+ ItemObj.user_id +'"><option value="0" '+ ((ItemObj.user_login=="0")?"selected":"") +'>用户还未登陆</option><option value="1" '+ ((ItemObj.user_login=="1")?"selected":"") +'>用户正在登陆</option></select><br><br>';
	CreateHTML += '答题状态：<select size="1" name="do'+ ItemObj.user_id +'" id="do'+ ItemObj.user_id +'"><option value="0" '+ ((ItemObj.user_do=="0")?"selected":"") +'>用户禁止答卷</option><option value="1" '+ ((ItemObj.user_do=="1")?"selected":"") +'>用户开放答卷</option></select><br><br>';
	CreateHTML += '历史记录：<input type="button" style="width: 100px;" onClick="history(\''+ ItemObj.user_id +'\',\''+ ItemObj.user_name +'\',\''+ Usernum +'\')" value="查看历史分数" class="ip2">';
	CreateHTML += '</td>';
	CreateHTML += '</tr>';
	CreateHTML += '</table>';
	CreateHTML += '<span style="width: 450; visibility: hidden;" id="hidden'+ ItemObj.user_id +'">'+ Usernum +'\''+ ItemObj.user_func +'\''+ ItemObj.user_login +'\''+ ItemObj.user_do +'</span><img src="images/create.gif" alt="应用更新" onClick="ItemUpdate(\''+ ItemObj.user_id +'\');"> &nbsp; &nbsp;<img src="images/back.gif" alt="恢复初值" onClick="ItemReSet(\''+ ItemObj.user_id +'\');">&nbsp; &nbsp;';
	CreateHTML += '</td>';
	CreateHTML += '</tr>';
	CreateHTML += '</table>';

	CurrentCell.innerHTML = CreateHTML;
}


function DelFunc(Num){
	var H_Sign = 0;
	var Historyid = document.getElementById("historyid").innerHTML.trim();
	if (Num==Historyid) H_Sign = 1;
	var TdObj = document.getElementById("td"+Num);
	var Name = TdObj.parentElement.parentElement.rows[0].cells[1].innerText;
	if(confirm("您真的要删除 "+Name+" 用户及其所选用户吗？\n删除后将无法恢复")){
		var currentPageNum = SysUserListPages.page;
		var InputObjS = document.getElementsByName("ManageItemName");
		for (var i=0; i<InputObjS.length; i++){
			if (InputObjS[i].type == "checkbox" && InputObjS[i].checked){
				if (InputObjS[i].value!=Num){
					var CTNum = InputObjS[i].value;
					Num = Num+","+CTNum;
					if (H_Sign==0 && CTNum==Historyid) H_Sign = 1;
				}
			}
		}
		var RunFunc = new Array("DelRunFunc("+ currentPageNum +","+ H_Sign +");");
		parent.left.LoadDocFunc ("删除用户","pagemanage?type=del&id="+Num+"&PageN="+currentPageNum,RunFunc);
	}
}


function DelRunFunc(pageNum,sign){
	var SignArray = parent.left.TmpStr.split("|");
	if (SignArray[1]!=""){
		CreateUserList(pageNum,SignArray[1]);
	}
	parent.left.DataArray = [];
	if (sign==1){
		historyPanelControl();
	}
	if (SignArray[0]!=""){
		var Txt = "";
		var TstrArray = SignArray[0].split(",");
		for (var i=0; i<TstrArray.length; i++){
			switch(TstrArray[i]){
				case "1":
					Txt = Txt+"因系统不允许删除您自己的帐号，因此系统自动蒙蔽了此用户的删除操作！\n";
				break;
				case "2":
					Txt = Txt+"因系统不允许删除最后一位管理级用户，因此系统自动蒙蔽了此用户的删除操作！\n";
				break;
				case "3":
					Txt = Txt+"因系统自动蒙蔽不符合系统删除要求的用户，导致此任务没有任何符合删除的操作！\n";
				break;
			}
		}
		alert(Txt);
	}
}


function ReSet()
{
	var name = document.getElementById("newname");
	var password = document.getElementById("newpassword");
	var repassword = document.getElementById("newrepassword");
	var num = document.getElementById("newnum");
	var type = document.getElementById("newtype");

	name.value = "";
	password.value = "";
	repassword.value = "";
	num.value = "";
	type.options[0].selected = true;
}

function selectManageItem(){
	var InputObjS = document.getElementsByName("ManageItemName");
	for (var i=0; i<InputObjS.length; i++){
		if (InputObjS[i].type == "checkbox"){
			InputObjS[i].checked = !InputObjS[i].checked;
		}
	}
}

function selectHistoryItem(){
	var InputObjS = document.getElementsByName("HistroyItemName");
	for (var i=0; i<InputObjS.length; i++){
		if (InputObjS[i].type == "checkbox"){
			InputObjS[i].checked = !InputObjS[i].checked;
		}
	}
}


function checkFunc(userid)
{
	if (userid == null){
		var name = document.getElementById("newname");
		var password = document.getElementById("newpassword");
		var repassword = document.getElementById("newrepassword");
		var num = document.getElementById("newnum");
		var type = document.getElementById("newtype");
		if (password.value==""){
    		alert("请输入初始密码！");
    		password.select();
    		return false;
  		}
	  	if (repassword.value==""){
    		alert("请重复输入一次初始密码！");
    		repassword.select();
    		return false;
        }
        if (type.value == '0') {
            alert("请选择用户权限！");
            return false;
        }
	}
	else{
		var name = document.getElementById("name"+userid);
		var password = document.getElementById("password"+userid);
		var repassword = document.getElementById("repassword"+userid);
		var num = document.getElementById("num"+userid);
	}
	var RegObjS = /<[!a-z\/]|<$|[%\"\'\\\/ ,;\?&]/i;
  	if (RegObjS.test(name.value)){
    	alert("输入的用户名称中有非法字符！");
    	name.select();
    	return false;
  	}
  	if (name.value==""){
    	alert("请务必输入用户名称！");
    	name.select();
    	return false;
  	}
	if (num.value==""){
    	alert("请务必输入用户编号！");
    	num.select();
    	return false;
  	}
	if (/[^\d]+/.test(num.value)){
		alert("输入的用户编号不是数值！");
    	num.select();
		return false;
	}
  	if (password.value!="" && password.value.length<5){
    	alert("初始密码要求在5位以上！");
    	password.select();
    	return false;
  	}
	if (password.value!="" && RegObjS.test(password.value)){
		alert("输入的密码中有非法字符！");
    	password.select();
    	return false;
	}
  	if (password.value!=repassword.value){
    	alert("密码两次输入的不一致！");
    	password.select();
    	return false;
  	}
	var RegExpJPER = /[\u30B4|\u30AC|\u30AE|\u30B0|\u30B2|\u30B6|\u30B8|\u30BA|\u30C5|\u30C7|\u30C9|\u30DD|\u30D9|\u30D7|\u30D3|\u30D1|\u30F4|\u30DC|\u30DA|\u30D6|\u30D4|\u30D0|\u30C2|\u30C0|\u30BE|\u30BC]/g;
	name.value = name.value.replace(RegExpJPER,"□");
	password.value = password.value.replace(RegExpJPER,"□");
  	return true; 
}

function CreateFunc(){
	if (checkFunc()){
		var name = document.getElementById("newname");
		var password = document.getElementById("newpassword");
		var num = document.getElementById("newnum");
		var type = document.getElementById("newtype");
		var numstr = num.value+"";
		if (type.value!="1") numstr = "-"+numstr;
 		var UserInfoTmp = numstr+"'"+name.value+"'"+password.value+"'"+type.value;
 		var RunFunc = new Array("CreateRunFunc();");
 		parent.left.LoadDocFunc ("添加用户","pagemanage?type=add",RunFunc,null,UserInfoTmp);
	}
}

function CreateRunFunc(){
	ReSet();
	CreateUserList(1);
	parent.left.DataArray = [];
	changeTitle(1);
}


function CreateUserList(PageNumT,PageSum){
	if (PageNumT==null) PageNumT=SysUserListPages.page;
	if (PageSum==null) PageSum = parent.left.TmpStr;
	SysUserListPages.changePageCount(PageSum,PageNumT);
	var DataArray = parent.left.DataArray;
	var MangerListObjT = document.getElementById("mangerlist");
	for (var i=0; i<MangerListObjT.childNodes.length; i++){
		MangerListObjT.childNodes[i].removeNode(true);
	}
	for (var i=0; i<DataArray.length; i++){	
		CreateRowFunc(DataArray[i]);
	}
}


function showCurrentPageFunc(nameStr,Num){
	var ObjName = nameStr;
	var PageNum = Num;
	
	if (ObjName.indexOf("SysUserListPages")==0){
		var RunFunc = ["CreateUserList("+PageNum+");"];
		parent.left.LoadDocFunc ("读取用户内容列表","pagemanage?type=loadlist&pagenum="+PageNum,RunFunc);
	}
	else if(ObjName.indexOf("SysHistoryListPages")==0){
		var RunFunc = ["CreateHistoryList("+PageNum+");"];
		var userid = document.getElementById("historyid").innerHTML;
		parent.left.LoadDocFunc("读取用户历史记录","pagemanage?type=historylist&pagenum="+PageNum,RunFunc,null,userid);
	}
	else{}
}

function showAllContent(nameStr,that){
	var ObjName = nameStr;

	if (ObjName.indexOf("SysUserListPages")==0){
		if (that.value == "呈现全部"){
			var RunFunc = ["CreateUserList();"];
			parent.left.LoadDocFunc ("读取用户内容列表","pagemanage?type=loadlist&pagenum=0",RunFunc);
			that.value = "恢复分页";
		}
		else{
			var TmpPageNumT = SysUserListPages.page;
			var RunFunc = ["CreateUserList("+TmpPageNumT+");"];
			parent.left.LoadDocFunc ("读取用户内容列表","pagemanage?type=loadlist&pagenum="+TmpPageNumT,RunFunc);
			that.value = "呈现全部";
		}
	}
	else if (ObjName.indexOf("SysHistoryListPages")==0){
		var userid = document.getElementById("historyid").innerHTML;
		if (that.value == "呈现全部"){
			var RunFunc = ["CreateHistoryList();"];
			parent.left.LoadDocFunc("读取用户历史记录","pagemanage?type=historylist&pagenum=0",RunFunc,null,userid);
			that.value = "恢复分页";
		}
		else{
			var TmpPageNumT = SysHistoryListPages.page;
			var RunFunc = ["CreateHistoryList("+TmpPageNumT+");"];
			parent.left.LoadDocFunc("读取用户历史记录","pagemanage?type=historylist&pagenum="+TmpPageNumT,RunFunc,null,userid);
			that.value = "呈现全部";
		}
	}
	else{}
}

function selectNewUserType(that) {
    var num = document.getElementById("newnum");
    var enablenumblock = document.getElementById("enablenumblock");
    var disablenumblock = document.getElementById("disablenumblock");
	if (that.value == '1') {
	    num.value = "111111";
	    enablenumblock.style.display = "none";
	    disablenumblock.style.display = "inline";
	}
	else {
	    num.value = "";
	    enablenumblock.style.display = "inline";
	    disablenumblock.style.display = "none";
    }
}

</script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
<body topmargin="5" leftmargin="0" onclick="DBNPOC.closeWin();">
<div align="center">
  <table border="0" cellpadding="0" cellspacing="0" class="TableStyle">
    <tr>
      <td height="22" align="center"  class="HeadStyle">设置新用户</td>
    </tr>
    <tr>
      <td height="80" align="center" class="BodyStyle">
	  
	  <table border="0" cellpadding="0" cellspacing="0"  width="550">
	   <tr>
	   	<td align="center" valign="middle">
			<table border="0" cellpadding="0" cellspacing="0" width="85%" height="110">
			<tr>
				<td valign="top">
					<span id="enablenumblock" style="display:none;">用户编号：<input type="text" name="num" id="newnum" size="14" maxlength="14" class="ip1" value="111111"><br><br></span>
                    <span id="disablenumblock" style="display:inline;">用户编号：编号自动生成<br><br></span>
					初始密码：<input type="password" name="password" id="newpassword" size="14" maxlength="14" class="ip1"><br><br>
					用户权限：<select size="1" name="type" id="newtype" onchange="selectNewUserType(this);"><option value="1" selected>普通学员考生</option><option value="2">试题维护教师</option><option value="3">系统级管理员</option></select>
				</td>
				<td align="right" valign="top">
					用户名称：<input type="text" name="name" id="newname" size="14" maxlength="14" class="ip1"><br><br>
					重复一次：<input type="password" name="repassword" id="newrepassword" size="14" maxlength="14" class="ip1"><br><br><br><br>
					<img src="images/create.gif" alt="新建用户" onClick="CreateFunc();"> &nbsp; &nbsp; <img src="images/back.gif" alt="重新填写" onClick="ReSet();"> &nbsp; 
				</td>
			</tr>
			</table>
		</td>
	   </tr>
	  </table>

      </td>
    </tr>
	</table>
	
    <p></p>

<script language="javascript">
    var StatusPage = 1;

    function ListPageFunc() {
        var Ttxt = document.getElementById("listname").innerHTML;
        if (Ttxt == "系统用户列表") {
            StatusPage = SysUserListPages.page;
        }
    }

    function SearchFunc(sign) {
        var TxtObj = document.getElementById("fastword");
        var TxtStr = TxtObj.value;
        var Sign = '1';
        if (TxtStr == "") {
            alert("快速搜索框中不能为空，请更正");
            TxtObj.select();
            return;
        }
        if (sign == "name") {
            var RegObj = /(\\|'|"|%)/gi;
            if (RegObj.test(TxtStr)) {
                alert("快速搜索框中输入了非法字符，请更正");
                TxtObj.select();
                return;
            }
            var RegExpJPER = /[\u30B4|\u30AC|\u30AE|\u30B0|\u30B2|\u30B6|\u30B8|\u30BA|\u30C5|\u30C7|\u30C9|\u30DD|\u30D9|\u30D7|\u30D3|\u30D1|\u30F4|\u30DC|\u30DA|\u30D6|\u30D4|\u30D0|\u30C2|\u30C0|\u30BE|\u30BC]/g;
            TxtStr = TxtStr.replace(RegExpJPER, "□");
            Sign = '1';
        }
        else {
            if (/[^\d]+/.test(TxtStr)) {
                alert("快速搜索框中输入的用户编号不是数值，请更正");
                TxtObj.select();
                return;
            }
            Sign = '2';
        }
        ListPageFunc();
        var RunFunc = new Array("SearchFuncRun();");
        parent.left.LoadDocFunc("快速搜索用户", "pagemanage?type=search&sign=" + Sign, RunFunc, null, TxtStr);
    }

    function SearchFuncRun() {
        CreateUserList(1);
        parent.left.DataArray = [];
        changeTitle(0);
    }


    function hiddenSearchArea() {
        var SearchArea = document.getElementById("BetterSearchArea");
        if (SearchArea.style.display == "none") {
            SearchArea.style.display = "block";
        }
        else {
            SearchArea.style.display = "none";
        }
    }


    function changeSearchNormal() {
        var Ttxt = document.getElementById("listname").innerHTML;
        if (Ttxt == "系统用户列表") {
            alert("当前已是系统用户列表不用恢复！");
            return;
        }
        var RunFunc = ["changeTitle(1)", "CreateUserList(" + StatusPage + ");"];
        parent.left.LoadDocFunc("恢复系统用户列表", "pagemanage?type=reviece&pagenum=" + StatusPage, RunFunc);
    }

    function changeTitle(sign) {
        if (sign == 1) {
            document.getElementById("listname").innerHTML = "系统用户列表";
        }
        else {
            document.getElementById("listname").innerHTML = "用户搜索列表";
        }
    }
</script>
			  <table border="0" cellpadding="0" cellspacing="0" class="TableStyle">
			    <tr>
      				<td height="22" align="center" class="HeadStyle">搜索条件设定</td>
    			</tr>
    			<tr>
      				<td class="BodyStyle" align="center">
						快速搜索: <input type="text" size="25" class="ip1" id="fastword">&nbsp;
						<input type="button" value="名称搜索" class="ip2" onClick="SearchFunc('name');">&nbsp;
						<input type="button" value="编号搜索" class="ip2" onClick="SearchFunc('num');">&nbsp;
						<input type="button" value="高级搜索" class="ip2" onClick="hiddenSearchArea();">&nbsp;
						<input type="button" value="恢复列表" class="ip2" onClick="changeSearchNormal();"><br><br>
						<fieldset style="width: 90%; display:none; padding-top: 0px;" id="BetterSearchArea"><legend>高级搜索条件设定区</legend><br>
							搜索字段：<select size="1" name="filed" id="filed" onChange="BetterSearchObj.ChangeFiled();">
							<option value="0" checked>用户编号</option>
							<option value="1">用户名称</option>
							<option value="2">用户权限</option>
							<option value="3">登陆状态</option>
							<option value="4">答题状态</option>
							<option value="5">历史状态</option>
							</select> &nbsp; 
							操作条件：<select size="1" name="opare" id="opare">
							<option value="0" checked>大于操作</option>
							<option value="1">小于操作</option>
							<option value="2">等于操作</option>
							<option value="3">不等操作</option>
							<option value="4">模糊操作</option>
							</select> &nbsp; <span style="text-align: center; visibility:visible; width: 178px;" id="oparepanle">
							查询内容：<input type="text" size="15" class="ip1" id="fotxt" name="fotxt"></span><br><br>
							<table border=0 width="490">
							<tr>
    							<td colspan="2" align="left">高级组合搜索条件：</td>
							</tr>
							<tr>
								<td width="80%" align="center"><select size="6" multiple style="width: 350px;" name="sumoparesele" id="sumoparesele" ondblclick="BetterSearchObj.RemoveSeleItem();"></select></td>
								<td width="20%" align="right">
									&nbsp; <input type="button" onClick="BackDefaultFunc();" value="添加设置条件" class="ip2"><br><br>
									&nbsp; <input type="button" onClick="BetterSearchObj.RemoveSeleItem();" value="删除设置条件" class="ip2"><br><br>
									&nbsp; <input type="button" onClick="BetterSearchObj.RunSearch();" value="执行条件搜索" class="ip2">
								</td>
							</tr>
						</table><br>
						</fieldset>
				      </td>
				</tr>
			</table>
<div style="position: absolute; z-index: 99; width: 170px; display: none; border: #183789 1px solid;" id="DBNPO" onClick="DBNPOC.tmpFunc();">
	<table width="100%" border="0" cellspacing="0" cellpadding="0">
	  <tr>
		<td bgcolor="#cad7f7" height="20" valign="middle"><font color="#ff0000"><b>条件关系选择窗口</b></font><span style="width: 62; text-align: right;"><img src="images/del.gif" align="absmiddle" height="11" width="11" alt="关闭窗口" onClick="DBNPOC.closeWin();"></span>
		</td>
	  </tr>
	  <tr>
		<td bgcolor="#dfe6f8" align="center">
			<table cellpadding="0" cellspacing="0" align="center" width="100%">
				<tr><td align="center" valign="middle">
					<br>搜索条件之间关系选择：
					<br><select size="1" name="subopare"><option value="0" checked>或关系</option><option value="1">与关系</option></select> &nbsp; <input type="button" value="添加条件" class="ip2" onClick="BetterSearchObj.AddSumSeleItem();">
					<br><br>
					</td>
				</tr>
			</table>
		</td>
	  </tr>
	</table>
</div>
<script language="javascript">
    function BetterSearchClass() {
        this.ChangePanelObj = document.getElementById("oparepanle");
        this.FiledObj = document.getElementById("filed");
        this.OpareObj = document.getElementById("opare");
        this.FotxtObj = document.getElementById("fotxt");
        this.SumOpareObj = document.getElementById("sumoparesele");
        this.SubOpareObj = document.getElementById("subopare");
        this.ChangeOpare = function (vcs) {
            this.RemoveAll();
            switch (vcs) {
                case "0":
                    this.AddOptions("0", "大于操作");
                    this.AddOptions("1", "小于操作");
                    this.AddOptions("2", "等于操作");
                    this.AddOptions("3", "不等操作");
                    this.AddOptions("4", "模糊操作");
                    break;
                case "1":
                    this.AddOptions("0", "模糊查询");
                    break;
                case "2":
                    this.AddOptions("1", "学员考生");
                    this.AddOptions("2", "维护教师");
                    this.AddOptions("3", "系统管理");
                    break;
                case "3":
                    this.AddOptions("0", "还未登陆");
                    this.AddOptions("1", "正在登陆");
                    break;
                case "4":
                    this.AddOptions("0", "禁止答卷");
                    this.AddOptions("1", "开放答卷");
                    break;
                case "5":
                    this.AddOptions("0", "拥有历史");
                    this.AddOptions("1", "历史空白");
                    break;
            }
            this.OpareObj.options[0].selected = true;
        }

        this.AddOptions = function (txt, val) {
            this.OpareObj.options.add(new Option(val, txt));
        }

        this.RemoveAll = function () {
            for (var i = 0; i < this.OpareObj.options.length; i++) {
                this.OpareObj.options.remove(i);
                i--;
            }
        }

        this.AddSumSele = function (txt, val) {
            for (var i = 0; i < this.SumOpareObj.options.length; i++) {
                if (this.SumOpareObj.options[i].value == txt) {
                    if (!confirm("您筛选的条件已经存在是否还要继续添加？")) return;
                    break;
                }
            }
            this.SumOpareObj.options.add(new Option(val, txt));
        }

        this.ChangeFotxt = function (sign) {
            if (sign) {
                this.ChangePanelObj.style.visibility = "visible";
            }
            else {
                this.ChangePanelObj.style.visibility = "hidden";
            }
        }
        this.ChangeFiled = function () {
            var t_filedval = this.FiledObj.value;
            switch (t_filedval) {
                case "0":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(true);
                    break;
                case "1":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(true);
                    break;
                case "2":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(false);
                    break;
                case "3":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(false);
                    break;
                case "4":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(false);
                    break;
                case "5":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(false);
                    break;
                default:
                    this.ChangeOpare("0");
                    this.ChangeFotxt(true);
                    break;
            }
        }

        this.RemoveSeleItem = function () {
            var oparesign = false;
            for (var i = 0; i < this.SumOpareObj.options.length; i++) {
                if (this.SumOpareObj.options[i].selected) {
                    this.SumOpareObj.options.remove(i);
                    i--;
                    oparesign = true;
                }
            }
            if (!oparesign) {
                alert("请您先选择需要删除的筛选条件！");
            }
        }

        this.AddSumSeleItem = function () {
            var txt = "条件(";
            var val = "";
            var t_filedval = this.FiledObj.value;
            var t_opareobjval = (this.OpareObj.selectedIndex == -1) ? 0 : this.OpareObj.selectedIndex;
            var t_opareobj = this.OpareObj.options[t_opareobjval];
            var t_subopareval = this.SubOpareObj.value;
            var lent = this.SumOpareObj.options.length;
            switch (t_filedval) {
                case "0":
                    txt += "用户编号";
                    break;
                case "1":
                    txt += "用户名称";
                    break;
                case "2":
                    txt += "用户权限为";
                    break;
                case "3":
                    txt += "登陆状态为";
                    break;
                case "4":
                    txt += "答题状态为";
                    break;
                case "5":
                    txt += "历史状态为";
                    break;
                default:
                    t_filedval = "0";
                    txt += "用户编号";
                    break;
            }
            txt += t_opareobj.text;
            val = val + t_filedval + "'" + t_opareobj.value + "'";
            var t_temp = this.FotxtObjCheck();
            if (t_temp != "'") {
                if (t_temp == "") return;
                txt = txt + "'" + t_temp + "'";
                val = val + t_temp + "'";
            }
            else {
                val = val + "'";
            }
            txt = txt + ")";
            if (lent > 0) {
                txt = txt + "与上关系(";
                switch (t_subopareval) {
                    case "0":
                        txt = txt + "或关系";
                        break;
                    case "1":
                        txt = txt + "与关系";
                        break;
                    default:
                        t_subopareval = "0";
                        txt = txt + "或关系";
                        break;
                }
                txt = txt + ")";
            }
            val = val + t_subopareval;
            this.AddSumSele(val, txt);
            DBNPOC.closeWin();
        }

        this.FotxtObjCheck = function () {
            var returntxt = "'";
            var RegObj = /(\\|'|")/gi;
            var T_Obj = this.FotxtObj;
            var t_filedval = this.FiledObj.value;
            if (t_filedval == "0" || t_filedval == "1") {
                if (T_Obj.value == "") {
                    alert("光标所在的文本框不能为空，请输入内容！");
                    T_Obj.focus();
                    return "";
                }
            }
            if (t_filedval == "1") {
                if (RegObj.test(T_Obj.value)) {
                    alert("光标所在的文本框中输入了非法字符，请更正！");
                    T_Obj.select();
                    return "";
                }
                var RegExpJPER = /[\u30B4|\u30AC|\u30AE|\u30B0|\u30B2|\u30B6|\u30B8|\u30BA|\u30C5|\u30C7|\u30C9|\u30DD|\u30D9|\u30D7|\u30D3|\u30D1|\u30F4|\u30DC|\u30DA|\u30D6|\u30D4|\u30D0|\u30C2|\u30C0|\u30BE|\u30BC]/g;
                T_Obj.value = T_Obj.value.replace(RegExpJPER, "□");
                returntxt = T_Obj.value;
            }
            if (t_filedval == "0") {
                if (/[^\d]+/.test(T_Obj.value)) {
                    alert("光标所在的文本框中输入的不是数值，请更正");
                    T_Obj.select();
                    return "";
                }
                returntxt = T_Obj.value;
            }
            return returntxt;
        }

        this.RunSearch = function () {
            var len = this.SumOpareObj.options.length;
            if (len == 0) {
                alert("您没有放入任何筛选条件进入组合框，请先放入至少一个筛选条件！");
                return;
            }
            var BackTxt = "";
            for (var i = 0; i < len; i++) {
                if (BackTxt == "") {
                    BackTxt = this.SumOpareObj.options[i].value;
                }
                else {
                    BackTxt = BackTxt + "\"" + this.SumOpareObj.options[i].value;
                }
            }
            ListPageFunc();
            var RunFunc = new Array("SearchFuncRun();");
            parent.left.LoadDocFunc("搜索相关用户", "pagemanage?type=search&sign=3", RunFunc, null, BackTxt);
        }
    }
    var BetterSearchObj = new BetterSearchClass();
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
        var len = document.getElementById("sumoparesele").options.length;
        if (len == 0) {
            BetterSearchObj.AddSumSeleItem();
        }
        else {
            DBNPOC.showWin((event.clientX + document.body.scrollLeft), (event.clientY + document.body.scrollTop))
        }
    }
</script>
	<p></p>
  <table border="0" cellpadding="0" cellspacing="0" class="TableStyle">
    <tr>
      <td height="22" align="center" class="HeadStyle"><span id="listname">系统用户列表</span></td>
    </tr>
    <tr>
      <td align="center" class="BodyStyle">
      <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD" id="mangerlist">
      <asp:Repeater runat="server" ID="repUserList">
      <ItemTemplate>
        <tr>
          <td>
			<table border="0" cellpadding="0" cellspacing="0" width="550" height="50">
		  		<tr>
		  			<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="<%# Eval("UserId")%>" name="ManageItemName" title="同步操作复选框"></td>
		  			<td align="center"><b><%# Eval("UserName")%></b></td>
		  		</tr>
		  		<tr>
		  			<td align="left"><span style="width: 430px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 所属权限组别：<%#GetUserLevelName((int)Eval("UserLevel"))%></span><img src="images/edit.gif" alt="展开编辑<%# Eval("UserName")%>用户菜单" onClick="ShowEditDiv('<%# Eval("UserId")%>');"> &nbsp; <img src="images/del.gif" alt="删除<%# Eval("UserName")%>用户" onClick="DelFunc('<%# Eval("UserId")%>');"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的人员操作选框" onClick="selectManageItem();"></td>
		  		</tr>
				<tr>
					<td colspan="2" align="center" style="display: none;" id="td<%# Eval("UserId")%>">
					<!--------------------------------------------------------------------->
						<table border="0" cellpadding="0" cellspacing="0" width="85%" height="150">
							<tr>
								<td>
								用户编号：<input type="text" name="num<%# Eval("UserId")%>" id="num<%# Eval("UserId")%>" size="14" maxlength="14" value="<%#GetUserShowNumber((string)Eval("UserNo"))%>" class="ip1"><br><br>
								用户名称：<input type="text" name="name<%# Eval("UserId")%>" id="name<%# Eval("UserId")%>" size="14" maxlength="14" value="<%# Eval("UserName")%>" class="ip1"><br><br>
								更新密码：<input type="password" name="password<%# Eval("UserId")%>" id="password<%# Eval("UserId")%>" size="14" maxlength="14" class="ip1"><br><br>
								重复一次：<input type="password" name="repassword<%# Eval("UserId")%>" id="repassword<%# Eval("UserId")%>" size="14" maxlength="14" class="ip1">
								</td>
								<td align="right">
								用户权限：<select size="1" name="type<%# Eval("UserId")%>" id="type<%# Eval("UserId")%>"><option value="1" <%#GetSelectedLevel((int)Eval("UserLevel"), 1)%>>普通学员考生</option><option value="2" <%#GetSelectedLevel((int)Eval("UserLevel"), 2)%>>试题维护教师</option><option value="3" <%#GetSelectedLevel((int)Eval("UserLevel"), 3)%>>系统级管理员</option></select><br><br>
								登陆状态：<select size="1" name="status<%# Eval("UserId")%>" id="status<%# Eval("UserId")%>"><option value="0" <%#GetSelectedStatus((bool)Eval("IsLogin"), false)%>>用户还未登陆</option><option value="1" <%#GetSelectedStatus((bool)Eval("IsLogin"), true)%>>用户正在登陆</option></select><br><br>
								答题状态：<select size="1" name="do<%# Eval("UserId")%>" id="do<%# Eval("UserId")%>"><option value="0" <%#GetSelectedStatus((bool)Eval("DoTest"), false)%>>用户禁止答卷</option><option value="1" <%#GetSelectedStatus((bool)Eval("DoTest"), true)%>>用户开放答卷</option></select><br><br>
								历史记录：<input type="button" style="width: 100px;" onClick="history('<%# Eval("UserId")%>','<%# Eval("UserName")%>','<%#GetUserShowNumber((string)Eval("UserNo"))%>')" value="查看历史分数" class="ip2">
								</td>
							</tr>
						</table>
						<span style="width: 450; visibility: hidden;" id="hidden<%# Eval("UserId")%>"><%#GetUserShowNumber((string)Eval("UserNo"))%>'<%# (int)Eval("UserLevel")%>'<%# Eval("IsLogin")%>'<%# Eval("DoTest")%></span><img src="images/create.gif" alt="应用更新" onClick="ItemUpdate('<%# Eval("UserId")%>');"> &nbsp; &nbsp;<img src="images/back.gif" alt="恢复初值" onClick="ItemReSet('<%# Eval("UserId")%>');">&nbsp; &nbsp;
					<!--------------------------------------------------------------------->
					</td>
				</tr>
		  	</table>
		  </td>
        </tr>
        </ItemTemplate>
        </asp:Repeater>
      </table>
	  <span style="width: 85%;">
	  <br>
	  	<script language="javascript">
			var SysUserListPages = new showPages('SysUserListPages');
			SysUserListPages.pageCount = <%=this.UserCollectionPageCount%>; // 定义总页数(必要)
			SysUserListPages.printHtml(1);
		</script>
	</span>
      </td>
    </tr>
	</table>

	<p></p>
  <table border="0" cellpadding="0" cellspacing="0" class="TableStyle" id="historypanel" style="display: none;">
    <tr>
      <td height="22" align="center" class="HeadStyle"><span id="historyname">历史分数记录</span><span id="historyid" style="display: none;"></span></td>
    </tr>
    <tr>
      <td align="center" class="BodyStyle">
      <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD" id="historylist">
        <tr>
          <td>
		  </td>
        </tr>
      </table>
	  <span style="width: 85%;">
	  <br>
	  	<script language="javascript">
	  	    var SysHistoryListPages = new showPages('SysHistoryListPages');
	  	    SysHistoryListPages.pageCount = 1; // 定义总页数(必要)
	  	    SysHistoryListPages.printHtml(1);
		</script>
	</span>
      </td>
    </tr>
	</table>

</div>
</body>

<script language="javascript">
    parent.parent.HideShowDivFunc(false);
</script>

</asp:Content>
