<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/Default.Master" CodeBehind="pagehistory.aspx.cs" Inherits="ExamineSystem.pagehistory" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
<script language="javascript">
    parent.parent.HideShowDivFunc(true);
</script>
<script src="script/PagesFunc.js" type=text/javascript></script>
<script language="JavaScript">
    window.onerror = function () {
        parent.parent.warn_Win("系统运行出现脚本错误<br>给您带来不便还请谅解", 1);
        return true;
    }

    String.prototype.trim = function () {
        return this.replace(/(^\s*)|(\s*$)/g, "");
    }

    String.prototype.change = function () {
        var returnTxt = this;
        if (returnTxt == "False" || returnTxt == "false" || returnTxt == "FALSE") returnTxt = "0";
        if (returnTxt == "True" || returnTxt == "true" || returnTxt == "TRUE") returnTxt = "1";
        return returnTxt;
    }


    function DelRecord(hisid) {
        if (confirm("您真的要删除此记录及其所选记录吗？\n删除后将无法恢复")) {
            var currentPageNum = SysHistoryListPages.page;
            var InputObjS = document.getElementsByName("HistroyItemName");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "checkbox" && InputObjS[i].checked) {
                    if (InputObjS[i].value != hisid) {
                        hisid = hisid + "," + InputObjS[i].value;
                    }
                }
            }
            var RunFunc = new Array("DelRecordRunFunc(" + currentPageNum + ");");
            parent.left.LoadDocFunc("删除历史记录", "pagehistory?type=delhistory&id=" + hisid + "&PageN=" + currentPageNum, RunFunc);
        }
    }

    function DelRecordRunFunc(pageNum) {
        CreateHistoryList(pageNum);
        parent.left.DataArray = [];
    }


    function history(userid, username, usernum) {
        var RunFunc = new Array("historyRunFunc(\"" + userid + "\",\"" + username + "\",\"" + usernum + "\");");
        parent.left.LoadDocFunc("读取用户历史记录", "pagehistory?type=historylist&pagenum=1", RunFunc, null, userid);
    }

    function historyRunFunc(userid, username, usernum) {
        document.getElementById("historyid").innerHTML = userid;
        document.getElementById("historyname").innerHTML = username + "的历史分数记录（编号：" + usernum + "）";
        CreateHistoryList(1);
        parent.left.DataArray = [];
        var historypanelobj = document.getElementById("historypanel");
        if (historypanelobj.style.display == "none") {
            historypanelobj.style.display = "block";
        }
    }

    function historyPanelControl() {
        var historypanelobj = document.getElementById("historypanel");
        historypanelobj.style.display = "none";
        document.getElementById("historyid").innerHTML = "";
        document.getElementById("historyname").innerHTML = "历史分数记录";
    }

    function CreateHistoryList(PageNumT, PageSum) {
        if (PageNumT == null) PageNumT = SysHistoryListPages.page;
        if (PageSum == null) PageSum = parent.left.TmpStr;
        SysHistoryListPages.changePageCount(PageSum, PageNumT);
        var DataArray = parent.left.DataArray;
        var MangerListObjT = document.getElementById("historylist");
        for (var i = 0; i < MangerListObjT.childNodes.length; i++) {
            MangerListObjT.childNodes[i].removeNode(true);
        }
        for (var i = 0; i < DataArray.length; i++) {
            CreateHistoryRecord(DataArray[i]);
        }
    }

    function ShowPaper(Papid) {
        alert("功能还在开发中");
    }

    function CreateHistoryRecord(ItemObj) {
        var MangerListObjT = document.getElementById("historylist");
        var RowNum = MangerListObjT.rows.length;
        var CurrentRow = MangerListObjT.insertRow(RowNum);
        var CurrentCell = CurrentRow.insertCell(0);
        var CreateHTML = '';
        CreateHTML += '<table border="0" cellpadding="0" cellspacing="0" width="550" height="50">';
        CreateHTML += '<tr>';
        CreateHTML += '<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="' + ItemObj.history_id + '" name="HistroyItemName" title="同步操作复选框"></td>';
        CreateHTML += '<td align="center"><b>' + ItemObj.history_ip + ' &nbsp; （' + ItemObj.history_score + '分）</b></td>';
        CreateHTML += '</tr>';
        CreateHTML += '<tr>';
        CreateHTML += '<td align="left"><span style="width: 430px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 时间：从 ' + ItemObj.history_stime + ' 到 ' + ItemObj.history_etime + ' </span><img src="images/edit.gif" alt="查看试卷" onClick="ShowPaper(\'' + ItemObj.history_id + '\');" style="display:none;"> &nbsp; <img src="images/del.gif" alt="删除记录" onClick="DelRecord(\'' + ItemObj.history_id + '\');"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的记录操作选框" onClick="selectHistoryItem();"></td>';
        CreateHTML += '</tr>';
        CreateHTML += '</table>';
        CurrentCell.innerHTML = CreateHTML;
    }

    function selectHistoryItem() {
        var InputObjS = document.getElementsByName("HistroyItemName");
        for (var i = 0; i < InputObjS.length; i++) {
            if (InputObjS[i].type == "checkbox") {
                InputObjS[i].checked = !InputObjS[i].checked;
            }
        }
    }

    function showCurrentPageFunc(nameStr, Num) {
        var ObjName = nameStr;
        var PageNum = Num;

        if (ObjName.indexOf("SysHistoryListPages") == 0) {
            var RunFunc = ["CreateHistoryList(" + PageNum + ");"];
            parent.left.LoadDocFunc("读取用户历史记录", "pagehistory?type=historylist&pagenum=" + PageNum, RunFunc);
        }
    }

    function showAllContent(nameStr, that) {
        var ObjName = nameStr;

        if (ObjName.indexOf("SysHistoryListPages") == 0) {
            if (that.value == "呈现全部") {
                var RunFunc = ["CreateHistoryList();"];
                parent.left.LoadDocFunc("读取用户历史记录", "pagehistory?type=historylist&pagenum=0", RunFunc);
                that.value = "恢复分页";
            }
            else {
                var TmpPageNumT = SysHistoryListPages.page;
                var RunFunc = ["CreateHistoryList(" + TmpPageNumT + ");"];
                parent.left.LoadDocFunc("读取用户历史记录", "pagehistory?type=historylist&pagenum=" + TmpPageNumT, RunFunc);
                that.value = "呈现全部";
            }
        }
    }

</script>
</asp:Content> 

<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
<body topmargin="5" leftmargin="0" onclick="DBNPOC.closeWin();">
<script src="script/date.js" type="text/javascript"></script>
<div align="center">
<script language="javascript">
    var StatusPage = 1;

    function ListPageFunc() {
        var Ttxt = document.getElementById("hisnam").innerHTML;
        var Ttmp = CT_UserName + "的历史分数记录（编号：" + CT_UserNum + "）";
        if (Ttxt == Ttmp) {
            StatusPage = SysHistoryListPages.page;
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
            var RegObj = /^([01]?\d{1,2}|2[0-4]\d|25[0-5])(\.([01]?\d{1,2}|2[0-4]\d|25[0-5])){3}$/;
            if (!RegObj.test(TxtStr)) {
                alert("快速搜索框中输入的不是正确IP地址，请更正");
                TxtObj.select();
                return;
            }
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
        parent.left.LoadDocFunc("快速搜索历史记录", "pagehistory?type=search&sign=" + Sign, RunFunc, null, TxtStr);
    }

    function SearchFuncRun() {
        CreateHistoryList(1);
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
        var Ttxt = document.getElementById("hisnam").innerHTML;
        var Ttmp = CT_UserName + "的历史分数记录（编号：" + CT_UserNum + "）";
        if (Ttxt == Ttmp) {
            alert("当前已是系统用户列表不用恢复！");
            return;
        }
        var RunFunc = ["changeTitle(1)", "CreateHistoryList(" + StatusPage + ");"];
        parent.left.LoadDocFunc("恢复历史记录列表", "pagehistory?type=reviece&pagenum=" + StatusPage, RunFunc);
    }

    function changeTitle(sign) {
        if (sign == 1) {
            document.getElementById("hisnam").innerHTML = CT_UserName + "的历史分数记录（编号：" + CT_UserNum + "）";
        }
        else {
            document.getElementById("hisnam").innerHTML = CT_UserName + "的搜索分数记录（编号：" + CT_UserNum + "）";
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
						<input type="button" value="分数搜索" class="ip2" onClick="SearchFunc('num');">&nbsp;
						<input type="button" value="地址搜索" class="ip2" onClick="SearchFunc('name');">&nbsp;
						<input type="button" value="高级搜索" class="ip2" onClick="hiddenSearchArea();">&nbsp;
						<input type="button" value="恢复列表" class="ip2" onClick="changeSearchNormal();"><br><br>
						<fieldset style="width: 90%; display:none; padding-top: 0px;" id="BetterSearchArea"><legend>高级搜索条件设定区</legend><br>
							搜索字段：<select size="1" name="filed" id="filed" onChange="BetterSearchObj.ChangeFiled();">
							<option value="0" checked>考试分数</option>
							<option value="1">考试地址</option>
							<option value="2">考试日期</option>
							</select> &nbsp; 
							操作条件：<select size="1" name="opare" id="opare">
							<option value="0" checked>大于操作</option>
							<option value="1">小于操作</option>
							<option value="2">等于操作</option>
							<option value="3">不等操作</option>
							</select> &nbsp; <span style="text-align: center; width: 178px;">
							<span id="fotxtspan" style="display:block;">查询内容：<input type="text" size="15" class="ip1" id="fotxt" name="fotxt"></span><span id="datxtspan" style="display:none; position:relative; top: 6px;">
							<SCRIPT language="javascript">
							    var dateobj = new UncCalendar("date", "today");
							    dateobj.inputName = "datatxt";
							    dateobj.inputSize = 15;
							    dateobj.color = "#000080";
							    dateobj.bgColor = "#EEEEFF";
							    dateobj.buttonWidth = 70;
							    dateobj.buttonWords = "日期选择";
							    dateobj.canEdits = false;
							    dateobj.hidesSelects = false;
							    dateobj.display();
							</SCRIPT>
							</span></span><br><br>
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
        this.FOSpanObj = document.getElementById("fotxtspan");
        this.DataSpanObj = document.getElementById("datxtspan");
        this.FiledObj = document.getElementById("filed");
        this.OpareObj = document.getElementById("opare");
        this.FotxtObj = document.getElementById("fotxt");
        this.DataObj = document.getElementById("datatxt");
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
                    break;
                case "1":
                    this.AddOptions("0", "精确查询");
                    break;
                case "2":
                    this.AddOptions("0", "大于操作");
                    this.AddOptions("1", "小于操作");
                    this.AddOptions("2", "等于操作");
                    this.AddOptions("3", "不等操作");
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
                this.FOSpanObj.style.display = "block";
                this.DataSpanObj.style.display = "none";
            }
            else {
                this.FOSpanObj.style.display = "none";
                this.DataSpanObj.style.display = "block";
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
                    txt += "考试分数";
                    break;
                case "1":
                    txt += "考试地址";
                    break;
                case "2":
                    txt += "考试日期";
                    break;
                default:
                    t_filedval = "0";
                    txt += "考试分数";
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
            var T_Obj = this.FotxtObj;
            var t_filedval = this.FiledObj.value;
            if (t_filedval == "2") {
                returntxt = this.DataObj.value;
            }
            if (t_filedval == "0" || t_filedval == "1") {
                if (T_Obj.value == "") {
                    alert("光标所在的文本框不能为空，请输入内容！");
                    T_Obj.focus();
                    return "";
                }
            }
            if (t_filedval == "1") {
                var RegObj = /^([01]?\d{1,2}|2[0-4]\d|25[0-5])(\.([01]?\d{1,2}|2[0-4]\d|25[0-5])){3}$/;
                if (!RegObj.test(T_Obj.value)) {
                    alert("光标所在的文本框中输入的不是正确IP地址，请更正");
                    T_Obj.select();
                    return "";
                }
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
            parent.left.LoadDocFunc("搜索相关历史记录", "pagehistory?type=search&sign=3", RunFunc, null, BackTxt);
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
<script language="javascript">
var CT_UserName = "<%=this.CurrentUserName %>";
var CT_UserNum = "<%=this.CurrentUserNo %>";
</script>
	<p></p>
  <table border="0" cellpadding="0" cellspacing="0" class="TableStyle">
    <tr>
      <td height="22" align="center" class="HeadStyle"><span id="hisnam"><%=this.CurrentUserName %>的历史分数记录（编号：<%=this.CurrentUserNo %>）</span></td>
    </tr>
    <tr>
      <td align="center" class="BodyStyle">
      <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD" id="historylist">
      <asp:Repeater runat="server" ID="repHistoryList">
      <ItemTemplate>
        <tr>
          <td>
			
			<table border="0" cellpadding="0" cellspacing="0" width="550" height="50">
				<tr>
					<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="<%# Eval("HistoryId")%>" name="HistroyItemName" title="同步操作复选框"></td>
					<td align="center"><b><%# Eval("HistoryIp")%> &nbsp; （<%# Eval("HistoryScore")%>分）</b></td>
				</tr>
				<tr>
					<td align="left"><span style="width: 430px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 时间：从 <%# Eval("HistoryStartTime")%> 到 <%# Eval("HistoryEndTime")%> </span><img src="images/edit.gif" alt="查看试卷" onClick="ShowPaper('<%# Eval("HistoryId")%>');"  style="display:none;"> &nbsp; <img src="images/del.gif" alt="删除记录" onClick="DelRecord('<%# Eval("HistoryId")%>');"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的记录操作选框" onClick="selectHistoryItem();"></td>
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
			var SysHistoryListPages = new showPages('SysHistoryListPages');
			SysHistoryListPages.pageCount = <%=this.HistoryCollectionPageCount%>; // 定义总页数(必要)
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
