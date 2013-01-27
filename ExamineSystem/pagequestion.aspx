<%@ Page Title="" Language="C#" MasterPageFile="~/master/Default.Master" AutoEventWireup="true" CodeBehind="pagequestion.aspx.cs" Inherits="ExamineSystem.pagequestion" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
<script language="javascript">
    parent.parent.HideShowDivFunc(true);
</script>

<bgsound id="music" loop="1" volume="80">
<script src="script/PagesFunc.js" type=text/javascript></script>
<script language="JavaScript">
    window.onerror = function () {
        parent.parent.warn_Win("系统运行出现脚本错误<br>给您带来不便还请谅解", 1);
        return true;
    }

    String.prototype.trim = function () {
        return this.replace(/(^\s*)|(\s*$)/g, "");
    }

    String.prototype.hchar = function () {
        return this.replace(/_*/g, "");
    }

    String.prototype.change = function () {
        var returnTxt = this;
        if (returnTxt == "False" || returnTxt == "false" || returnTxt == "FALSE") returnTxt = "0";
        if (returnTxt == "True" || returnTxt == "true" || returnTxt == "TRUE") returnTxt = "1";
        return returnTxt;
    }

    Number.prototype.sequence = function () {
        var ReturnTxt = "A";
        Num = (parseInt(this) % 20);
        switch (Num) {
            case 0: ReturnTxt = "A"; break;
            case 1: ReturnTxt = "B"; break;
            case 2: ReturnTxt = "C"; break;
            case 3: ReturnTxt = "D"; break;
            case 4: ReturnTxt = "E"; break;
            case 5: ReturnTxt = "F"; break;
            case 6: ReturnTxt = "G"; break;
            case 7: ReturnTxt = "H"; break;
            case 8: ReturnTxt = "I"; break;
            case 9: ReturnTxt = "J"; break;
            case 10: ReturnTxt = "K"; break;
            case 11: ReturnTxt = "L"; break;
            case 12: ReturnTxt = "M"; break;
            case 13: ReturnTxt = "N"; break;
            case 14: ReturnTxt = "O"; break;
            case 15: ReturnTxt = "P"; break;
            case 16: ReturnTxt = "Q"; break;
            case 17: ReturnTxt = "R"; break;
            case 18: ReturnTxt = "S"; break;
            case 19: ReturnTxt = "T"; break;
        }
        return ReturnTxt;
    }

    function changeBig(that, newn) {
        that.rows = newn;
    }

    function changeBack(that, oldn) {
        if (that.rows != oldn) {
            that.rows = oldn;
        }
    }

    var SelectSumObjArray = [];
    var newSelectObj = null;


    function SingleSelect(ObjName, Panel, Status, id, Linkid) {
        var Html = "";
        this.SizeArray = new Array(25, 65, 60);
        if (Linkid != null) {
            this.SizeArray = new Array(11, 51, 50);
        }

        Html += '<table border="0" cellpadding="0" cellspacing="0" width="85%">';
        Html += '<tr><td>';
        Html += '检索描述：<input type="text" size="' + this.SizeArray[0] + '" class="ip1">';
        Html += '&nbsp;&nbsp;&nbsp;指定分数：<input type="text" size="3" class="ip1" value="2">';
        Html += '&nbsp;&nbsp;&nbsp;指定难度：<select size="1">';
        Html += '<option value="0">容易</option>';
        Html += '<option value="1" selected>一般</option>';
        Html += '<option value="2">较难</option>';
        Html += '<option value="3">困难</option>';
        Html += '</select>';
        Html += '<input type="hidden" value="-1">';
        Html += '</td></tr>';
        Html += '<tr><td>';
        Html += '试题题干：<input type="text" size="' + this.SizeArray[1] + '" class="ip1">';
        Html += '</td></tr>';
        Html += '<tr><td>';
        Html += '试题题枝：<br>';
        Html += '<fieldset style="width: 100%; display:block; padding-top: 0px;">';
        Html += '<table border="0" width="100%">';
        Html += '<tr><td align="center" width="60">试题答案</td><td align="center">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 试题题枝</td><td align="right" width="190">';
        Html += '<input type="button" onClick="' + ObjName + '.AddQuess(0);" value="新增" class="ip2">';
        Html += '&nbsp; <input type="button" onClick="' + ObjName + '.AddQuess(1);" value="插入" class="ip2">';
        Html += '&nbsp; <input type="button" onClick="' + ObjName + '.DelQuess();" value="删除" class="ip2">&nbsp;';
        Html += '</td></tr>';
        Html += '</table>';
        Html += '<table border="0" width="100%">';
        Html += '<tr onClick="' + ObjName + '.SelectQuess();" style="background-color:#cad7f7;"><td align="center" width="60"><input type="radio" onClick="' + ObjName + '.ResultQuess();" value="0"> A</td><td align="center"><input type="text" size="' + this.SizeArray[2] + '" class="ip1"></td></tr>';
        Html += '<tr onClick="' + ObjName + '.SelectQuess();" style="background-color:#cad7f7;"><td align="center" width="60"><input type="radio" onClick="' + ObjName + '.ResultQuess();" value="1"> B</td><td align="center"><input type="text" size="' + this.SizeArray[2] + '" class="ip1"></td></tr>';
        Html += '</table>';
        Html += '</fieldset>';
        Html += '</td></tr>';
        Html += '<tr><td align="right">';
        Html += '<br>';
        if (id == null) {
            if (Linkid == -1) {
                Html += '<img src="images/create.gif" alt="保存试题" onClick="' + ObjName + '.ReturnSaveQuess();">';
            }
            else {
                Html += '<img src="images/create.gif" alt="保存试题" onClick="' + ObjName + '.SaveQuess();">';
            }
        }
        else {
            if (Linkid == -1) {
                Html += '<img src="images/create.gif" alt="更新试题" onClick="' + ObjName + '.ReturnUpdateQuess(\'' + id + '\');">';
            }
            else {
                Html += '<img src="images/create.gif" alt="更新试题" onClick="' + ObjName + '.UpdateQuess(\'' + id + '\');">';
            }
        }
        Html += ' &nbsp; &nbsp; <img src="images/back.gif" alt="重新填写" onClick="' + ObjName + '.ReSetQuess();"> &nbsp; ';
        Html += '</td></tr>';
        Html += '</table>';

        var PackObj = null;
        if (Panel == null) {
            document.write("<span id='" + ObjName + "'>" + Html + "</span>");
            PackObj = document.getElementById(ObjName);
        }
        else {
            PackObj = document.createElement("span");
            PackObj.innerHTML = Html;
            if (typeof (Panel) == "string") {
                document.getElementById(Panel).appendChild(PackObj);
            }
            else {
                Panel.appendChild(PackObj);
            }
        }

        this.LinkId = Linkid;
        this.StatusStr = Status;
        this.ObjNameStr = ObjName;
        this.TableObj = PackObj.firstChild;
        var TmpArray = this.TableObj.rows[0].cells[0].childNodes;
        this.DescriptionObj = TmpArray[1];
        this.ScoreObj = TmpArray[3];
        this.DifficultObj = TmpArray[5];
        this.HiddenObj = TmpArray[6];
        this.QuessMainObj = this.TableObj.rows[1].cells[0].childNodes[1];
        this.QuessSubTableObj = this.TableObj.rows[2].cells[0].childNodes[2].childNodes[1];

        this.SelectQuess = function () {
            var TRObj = null;
            switch (event.srcElement.nodeName) {
                case "TD":
                    TRObj = event.srcElement.parentNode;
                    break;
                case "INPUT":
                    TRObj = event.srcElement.parentNode.parentNode;
                    break;
            }
            if (TRObj == null) return;
            TRObj.style.backgroundColor = "#5e86d7";
            var OldRow = this.getClickNum();
            if (OldRow != "" && OldRow != "-1") {
                if (OldRow != TRObj.rowIndex) {
                    this.QuessSubTableObj.rows[OldRow].style.backgroundColor = "#cad7f7";
                }
            }
            this.HiddenObj.value = TRObj.rowIndex + "";
        }

        this.ResultQuess = function () {
            var that = event.srcElement;
            that.checked = true;
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i] != that && InputObjS[i].type == "radio") {
                    InputObjS[i].checked = false;
                }
            }
        }

        this.ClearClickNum = function () {
            var RowsArray = this.QuessSubTableObj.rows;
            for (var i = 0; i < RowsArray.length; i++) {
                RowsArray[i].style.backgroundColor = "#cad7f7";
            }
            this.HiddenObj.value = "-1";
        }

        this.getClickNum = function () {
            var TClickNum = this.HiddenObj.value;
            if (TClickNum == "") TClickNum = "-1";
            return TClickNum;
        }

        this.checkBound = function (Sign) {
            var RowsNuber = this.QuessSubTableObj.rows.length;
            if (Sign == 0) {
                if (RowsNuber > 9) {
                    alert("单选题题枝最多只能添加到10条！");
                    return false;
                }
            }
            else {
                if (RowsNuber < 3) {
                    alert("单选题题枝最少要保持2条！");
                    return false;
                }
            }
            return true;
        }

        this.AddQuess = function (Sign) {
            if (!this.checkBound(0)) return;
            var RowNum = -1;
            if (Sign == 0) {
                RowNum = this.QuessSubTableObj.rows.length;
            }
            else {
                RowNum = parseInt(this.getClickNum());
                if (RowNum == -1) {
                    alert("请选择插入行的位置信息");
                    return;
                }
                RowNum = RowNum + 1;
            }
            var CurrentRow = this.QuessSubTableObj.insertRow(RowNum);
            CurrentRow.onclick = function () {
                var TRObj = null;
                var HideObj = null;
                switch (event.srcElement.nodeName) {
                    case "TD":
                        TRObj = event.srcElement.parentNode;
                        break;
                    case "INPUT":
                        TRObj = event.srcElement.parentNode.parentNode;
                        break;
                }
                if (TRObj == null) return;
                TRObj.style.backgroundColor = "#5e86d7";
                HideObj = TRObj.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.rows[0].cells[0].childNodes[6];
                var OldRow = HideObj.value;
                if (OldRow != "" && OldRow != "-1") {
                    if (OldRow != TRObj.rowIndex) {
                        TRObj.parentNode.rows[OldRow].style.backgroundColor = "#cad7f7";
                    }
                }
                HideObj.value = TRObj.rowIndex + "";
            }
            CurrentRow.style.backgroundColor = "#cad7f7";
            var FirstCell = CurrentRow.insertCell(0);
            FirstCell.align = "center";
            FirstCell.width = "60";
            FirstCell.innerHTML = '<input type="radio" onClick="' + this.ObjNameStr + '.ResultQuess();" value=""> ';
            var SecondCell = CurrentRow.insertCell(1);
            SecondCell.align = "center";
            SecondCell.innerHTML = '<input type="text" size="' + this.SizeArray[2] + '" class="ip1">';
            this.ClearClickNum();
            this.SetSequenceNum();
        }

        this.SetSequenceNum = function () {
            var RowArray = this.QuessSubTableObj.rows;
            for (var i = 0; i < RowArray.length; i++) {
                RowArray[i].cells[0].firstChild.value = i;
                RowArray[i].cells[0].childNodes[1].nodeValue = " " + i.sequence();
            }
        }

        this.SetQuessText = function (Num, Txt) {
            this.QuessSubTableObj.rows[Num].cells[1].firstChild.value = Txt;
        }

        this.SetQuessAnswer = function (Num) {
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "radio") {
                    InputObjS[i].checked = false;
                }
            }
            this.QuessSubTableObj.rows[Num].cells[0].firstChild.checked = true;
        }

        this.DelQuess = function () {
            if (!this.checkBound(1)) return;
            var RowNum = parseInt(this.getClickNum());
            if (RowNum == -1) {
                alert("请选择删除行的位置信息");
                return;
            }
            this.QuessSubTableObj.deleteRow(RowNum);
            this.ClearClickNum();
            this.SetSequenceNum();
        }

        this.SaveItemQuess = function () {
            var RowArray = this.QuessSubTableObj.rows;
            var Answer = -1;
            var QuessTxt = "";
            var QuessNum = -1;
            var QuessTxtNum = 0;
            for (var i = 0; i < RowArray.length; i++) {
                var RadioObj = RowArray[i].cells[0].firstChild;
                if (RadioObj.checked) {
                    Answer = RadioObj.value;
                    QuessNum = i;
                }
                var InputObj = RowArray[i].cells[1].firstChild;
                if (InputObj.value.trim() == "") {
                    if (QuessNum == i) {
                        alert("您给出的答案没有相应的题枝内容，请更正！");
                        return "";
                    }
                }
                else {
                    if (QuessTxt == "") {
                        QuessTxt = escape(InputObj.value);
                    }
                    else {
                        QuessTxt += ("," + escape(InputObj.value));
                        QuessTxtNum = QuessTxtNum + 1;
                    }
                }
            }
            if (Answer == -1) {
                alert("您没有给出题目正确答案，请更正！");
                return "";
            }
            if (QuessTxtNum < 1) {
                alert("您题目的题枝少于2条，请更正！");
                return "";
            }
            return (QuessTxt + "'" + Answer);
        }


        this.CheckValue = function () {
            if (/[^\d]+/.test(this.ScoreObj.value)) {
                alert("光标所在的文本框中输入的不是数值，请更正");
                this.ScoreObj.select();
                return false;
            }
            if (/^0\d*$/.test(this.ScoreObj.value)) {
                alert("光标所在的文本框中输入的不是数学整数，请更正");
                this.ScoreObj.select();
                return false;
            }
            if (this.QuessMainObj.value.trim() == "") {
                alert("光标所在的文本框中不能留空，请更正");
                this.QuessMainObj.focus();
                return false;
            }
            if (this.DescriptionObj.value.length > 32) {
                alert("检索描述信息不能大于32个字，请更正");
                this.DescriptionObj.focus();
                return false;
            }
            return true;
        }

        this.CollectInfo = function () {
            if (!this.CheckValue()) return "";
            var BackStr = "";
            var T_Desmp = this.DescriptionObj.value.trim();
            if (T_Desmp == "") {
                T_Desmp = this.QuessMainObj.value.substring(0, 10);
            }
            BackStr += (escape(T_Desmp) + "'");
            T_Desmp = this.ScoreObj.value.trim();
            if (T_Desmp == "") {
                T_Desmp = "0";
            }
            BackStr += (T_Desmp + "'");
            BackStr += (this.DifficultObj.value + "'");
            BackStr += (escape(this.QuessMainObj.value) + "'");
            var SaveItemQuessStr = this.SaveItemQuess();
            if (SaveItemQuessStr == "") {
                return "";
            }
            BackStr += SaveItemQuessStr;
            return BackStr;
        }

        this.SaveQuess = function () {
            var SaveStr = this.CollectInfo();
            if (SaveStr == "") return;
            var RunFunc = null;
            if (this.LinkId != null && this.LinkId != -1) {
                RunFunc = new Array(this.ObjNameStr + ".ClearQuess();", "CreateSubFunc(" + this.LinkId + ");");
            }
            else {
                RunFunc = new Array(this.ObjNameStr + ".ClearQuess();", "CreateRunFunc();");
            }
            if (this.LinkId == null || this.LinkId == "") this.LinkId = 0;
            parent.left.LoadDocFunc("添加单选题目", "pagequestion?type=AddSelect&style=0&linkid=" + this.LinkId, RunFunc, null, SaveStr);
        }

        this.ReturnSaveQuess = function () {
            var SaveStr = this.CollectInfo();
            if (SaveStr == "") return;
            try {
                newSelectObj.SetSubQuesSave(SaveStr, 0);
            }
            catch (e) { }
            this.ClearQuess();
        }


        this.UpdateQuess = function (id) {
            var UpdateStr = this.CollectInfo();
            if (UpdateStr == "") return;
            var RunFunc = new Array(this.ObjNameStr + ".UpdateQuessRunFunc(\"" + UpdateStr + "\");");
            parent.left.LoadDocFunc("更新单选题目", "pagequestion?type=UpdateSelect&quesid=" + id, RunFunc, null, UpdateStr);
        }

        this.ReturnUpdateQuess = function (id) {
            var UpdateStr = this.CollectInfo();
            if (UpdateStr == "") return;
            try {
                newSelectObj.UpdateSubQuesSave(UpdateStr, id);
            }
            catch (e) { }
            this.UpdateQuessRunFunc(UpdateStr);
        }

        this.UpdateQuessRunFunc = function (InfoStr) {
            this.StatusStr = InfoStr;
            this.ReSetQuess();
            try {
                this.TableObj.parentNode.parentNode.parentNode.parentNode.rows[0].cells[1].innerHTML = "<b>" + unescape(this.DescriptionObj.value) + "</b>";
            }
            catch (e) { }
        }


        this.ClearQuessItem = function () {
            for (var i = 0; i < this.QuessSubTableObj.rows.length; i++) {
                this.QuessSubTableObj.deleteRow(i);
                i--;
            }
        }

        this.ClearQuess = function () {
            this.StatusStr = "";
            this.DescriptionObj.value = "";
            this.ScoreObj.value = "2";
            this.DifficultObj.value = "1";
            this.QuessMainObj.value = "";
            this.ClearQuessItem();
            this.AddQuess(0);
            this.AddQuess(0);
        }

        this.ReSetQuess = function () {
            if (this.StatusStr == null || this.StatusStr == "") {
                this.ClearQuess();
                return;
            }
            var ContentArray = this.StatusStr.split("'");
            this.DescriptionObj.value = unescape(ContentArray[0]);
            this.ScoreObj.value = ContentArray[1];
            this.DifficultObj.value = ContentArray[2];
            this.QuessMainObj.value = unescape(ContentArray[3]);
            this.ClearQuessItem();
            var ItemQuessArray = ContentArray[4].split(",");
            for (var i = 0; i < ItemQuessArray.length; i++) {
                this.AddQuess(0);
                this.SetQuessText(i, unescape(ItemQuessArray[i]));
            }
            this.SetQuessAnswer(ContentArray[5]);
        }

        this.ReSetQuess();
    }


    function MultiSelect(ObjName, Panel, Status, id, Linkid) {
        var Html = "";
        this.SizeArray = new Array(25, 65, 60);
        if (Linkid != null) {
            this.SizeArray = new Array(11, 51, 50);
        }
        Html += '<table border="0" cellpadding="0" cellspacing="0" width="85%">';
        Html += '<tr><td>';
        Html += '检索描述：<input type="text" size="' + this.SizeArray[0] + '" class="ip1">';
        Html += '&nbsp;&nbsp;&nbsp;指定分数：<input type="text" size="3" class="ip1" value="4">';
        Html += '&nbsp;&nbsp;&nbsp;指定难度：<select size="1">';
        Html += '<option value="0">容易</option>';
        Html += '<option value="1" selected>一般</option>';
        Html += '<option value="2">较难</option>';
        Html += '<option value="3">困难</option>';
        Html += '</select>';
        Html += '<input type="hidden" value="-1">';
        Html += '</td></tr>';
        Html += '<tr><td>';
        Html += '试题题干：<input type="text" size="' + this.SizeArray[1] + '" class="ip1">';
        Html += '</td></tr>';
        Html += '<tr><td>';
        Html += '试题题枝：<br>';
        Html += '<fieldset style="width: 100%; display:block; padding-top: 0px;">';
        Html += '<table border="0" width="100%">';
        Html += '<tr><td align="center" width="60">试题答案</td><td align="center">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 试题题枝</td><td align="right" width="190">';
        Html += '<input type="button" onClick="' + ObjName + '.AddQuess(0);" value="新增" class="ip2">';
        Html += '&nbsp; <input type="button" onClick="' + ObjName + '.AddQuess(1);" value="插入" class="ip2">';
        Html += '&nbsp; <input type="button" onClick="' + ObjName + '.DelQuess();" value="删除" class="ip2">&nbsp;';
        Html += '</td></tr>';
        Html += '</table>';
        Html += '<table border="0" width="100%">';
        Html += '<tr onClick="' + ObjName + '.SelectQuess();" style="background-color:#cad7f7;"><td align="center" width="60"><input type="checkbox" value="0"> A</td><td align="center"><input type="text" size="' + this.SizeArray[2] + '" class="ip1"></td></tr>';
        Html += '<tr onClick="' + ObjName + '.SelectQuess();" style="background-color:#cad7f7;"><td align="center" width="60"><input type="checkbox" value="1"> B</td><td align="center"><input type="text" size="' + this.SizeArray[2] + '" class="ip1"></td></tr>';
        Html += '</table>';
        Html += '</fieldset>';
        Html += '</td></tr>';
        Html += '<tr><td align="right">';
        Html += '<br>';
        if (id == null) {
            if (Linkid == -1) {
                Html += '<img src="images/create.gif" alt="保存试题" onClick="' + ObjName + '.ReturnSaveQuess();">';
            }
            else {
                Html += '<img src="images/create.gif" alt="保存试题" onClick="' + ObjName + '.SaveQuess();">';
            }
        }
        else {
            if (Linkid == -1) {
                Html += '<img src="images/create.gif" alt="更新试题" onClick="' + ObjName + '.ReturnUpdateQuess(\'' + id + '\');">';
            }
            else {
                Html += '<img src="images/create.gif" alt="更新试题" onClick="' + ObjName + '.UpdateQuess(\'' + id + '\');">';
            }
        }
        Html += ' &nbsp; &nbsp; <img src="images/back.gif" alt="重新填写" onClick="' + ObjName + '.ReSetQuess();"> &nbsp; ';
        Html += '</td></tr>';
        Html += '</table>';

        var PackObj = null;
        if (Panel == null) {
            document.write("<span id='" + ObjName + "'>" + Html + "</span>");
            PackObj = document.getElementById(ObjName);
        }
        else {
            PackObj = document.createElement("span");
            PackObj.innerHTML = Html;
            if (typeof (Panel) == "string") {
                document.getElementById(Panel).appendChild(PackObj);
            }
            else {
                Panel.appendChild(PackObj);
            }
        }

        this.LinkId = Linkid;
        this.StatusStr = Status;
        this.ObjNameStr = ObjName;
        this.TableObj = PackObj.firstChild;
        var TmpArray = this.TableObj.rows[0].cells[0].childNodes;
        this.DescriptionObj = TmpArray[1];
        this.ScoreObj = TmpArray[3];
        this.DifficultObj = TmpArray[5];
        this.HiddenObj = TmpArray[6];
        this.QuessMainObj = this.TableObj.rows[1].cells[0].childNodes[1];
        this.QuessSubTableObj = this.TableObj.rows[2].cells[0].childNodes[2].childNodes[1];

        this.SelectQuess = function () {
            var TRObj = null;
            switch (event.srcElement.nodeName) {
                case "TD":
                    TRObj = event.srcElement.parentNode;
                    break;
                case "INPUT":
                    TRObj = event.srcElement.parentNode.parentNode;
                    break;
            }
            if (TRObj == null) return;
            TRObj.style.backgroundColor = "#5e86d7";
            var OldRow = this.getClickNum();
            if (OldRow != "" && OldRow != "-1") {
                if (OldRow != TRObj.rowIndex) {
                    this.QuessSubTableObj.rows[OldRow].style.backgroundColor = "#cad7f7";
                }
            }
            this.HiddenObj.value = TRObj.rowIndex + "";
        }

        this.ClearClickNum = function () {
            var RowsArray = this.QuessSubTableObj.rows;
            for (var i = 0; i < RowsArray.length; i++) {
                RowsArray[i].style.backgroundColor = "#cad7f7";
            }
            this.HiddenObj.value = "-1";
        }

        this.getClickNum = function () {
            var TClickNum = this.HiddenObj.value;
            if (TClickNum == "") TClickNum = "-1";
            return TClickNum;
        }

        this.checkBound = function (Sign) {
            var RowsNuber = this.QuessSubTableObj.rows.length;
            if (Sign == 0) {
                if (RowsNuber > 9) {
                    alert("多选题题枝最多只能添加到10条！");
                    return false;
                }
            }
            else {
                if (RowsNuber < 3) {
                    alert("多选题题枝最少要保持2条！");
                    return false;
                }
            }
            return true;
        }

        this.AddQuess = function (Sign) {
            if (!this.checkBound(0)) return;
            var RowNum = -1;
            if (Sign == 0) {
                RowNum = this.QuessSubTableObj.rows.length;
            }
            else {
                RowNum = parseInt(this.getClickNum());
                if (RowNum == -1) {
                    alert("请选择插入行的位置信息");
                    return;
                }
                RowNum = RowNum + 1;
            }
            var CurrentRow = this.QuessSubTableObj.insertRow(RowNum);
            CurrentRow.onclick = function () {
                var TRObj = null;
                var HideObj = null;
                switch (event.srcElement.nodeName) {
                    case "TD":
                        TRObj = event.srcElement.parentNode;
                        break;
                    case "INPUT":
                        TRObj = event.srcElement.parentNode.parentNode;
                        break;
                }
                if (TRObj == null) return;
                TRObj.style.backgroundColor = "#5e86d7";
                HideObj = TRObj.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.rows[0].cells[0].childNodes[6];
                var OldRow = HideObj.value;
                if (OldRow != "" && OldRow != "-1") {
                    if (OldRow != TRObj.rowIndex) {
                        TRObj.parentNode.rows[OldRow].style.backgroundColor = "#cad7f7";
                    }
                }
                HideObj.value = TRObj.rowIndex + "";
            }
            CurrentRow.style.backgroundColor = "#cad7f7";
            var FirstCell = CurrentRow.insertCell(0);
            FirstCell.align = "center";
            FirstCell.width = "60";
            FirstCell.innerHTML = '<input type="checkbox" value=""> ';
            var SecondCell = CurrentRow.insertCell(1);
            SecondCell.align = "center";
            SecondCell.innerHTML = '<input type="text" size="' + this.SizeArray[2] + '" class="ip1">';
            this.ClearClickNum();
            this.SetSequenceNum();
        }

        this.SetSequenceNum = function () {
            var RowArray = this.QuessSubTableObj.rows;
            for (var i = 0; i < RowArray.length; i++) {
                RowArray[i].cells[0].firstChild.value = i;
                RowArray[i].cells[0].childNodes[1].nodeValue = " " + i.sequence();
            }
        }

        this.SetQuessText = function (Num, Txt) {
            this.QuessSubTableObj.rows[Num].cells[1].firstChild.value = Txt;
        }

        this.SetQuessAnswer = function (Num) {
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "checkbox") {
                    InputObjS[i].checked = false;
                }
            }
            var AnswerArray = Num.split(",");
            for (var i = 0; i < AnswerArray.length; i++) {
                this.QuessSubTableObj.rows[AnswerArray[i]].cells[0].firstChild.checked = true;
            }
        }

        this.DelQuess = function () {
            if (!this.checkBound(1)) return;
            var RowNum = parseInt(this.getClickNum());
            if (RowNum == -1) {
                alert("请选择删除行的位置信息");
                return;
            }
            this.QuessSubTableObj.deleteRow(RowNum);
            this.ClearClickNum();
            this.SetSequenceNum();
        }

        this.SaveItemQuess = function () {
            var RowArray = this.QuessSubTableObj.rows;
            var Answer = "";
            var QuessTxt = "";
            var QuessNum = -1;
            var QuessTxtNum = 0;
            for (var i = 0; i < RowArray.length; i++) {
                var RadioObj = RowArray[i].cells[0].firstChild;
                if (RadioObj.checked) {
                    if (Answer == "") {
                        Answer = RadioObj.value;
                    }
                    else {
                        Answer += ("," + RadioObj.value);
                    }
                    QuessNum = i;
                }
                var InputObj = RowArray[i].cells[1].firstChild;
                if (InputObj.value.trim() == "") {
                    if (QuessNum == i) {
                        alert("您给出的某个答案选项没有相应的题枝内容，请更正！");
                        return "";
                    }
                }
                else {
                    if (QuessTxt == "") {
                        QuessTxt = escape(InputObj.value);
                    }
                    else {
                        QuessTxt += ("," + escape(InputObj.value));
                        QuessTxtNum = QuessTxtNum + 1;
                    }
                }
            }
            if (Answer == "") {
                alert("您没有给出题目正确答案，请更正！");
                return "";
            }
            if (QuessTxtNum < 1) {
                alert("您题目的题枝少于2条，请更正！");
                return "";
            }
            return (QuessTxt + "'" + Answer);
        }


        this.CheckValue = function () {
            if (/[^\d]+/.test(this.ScoreObj.value)) {
                alert("光标所在的文本框中输入的不是数值，请更正");
                this.ScoreObj.select();
                return false;
            }
            if (/^0\d*$/.test(this.ScoreObj.value)) {
                alert("光标所在的文本框中输入的不是数学整数，请更正");
                this.ScoreObj.select();
                return false;
            }
            if (this.QuessMainObj.value.trim() == "") {
                alert("光标所在的文本框中不能留空，请更正");
                this.QuessMainObj.focus();
                return false;
            }
            if (this.DescriptionObj.value.length > 32) {
                alert("检索描述信息不能大于32个字，请更正");
                this.DescriptionObj.focus();
                return false;
            }
            return true;
        }

        this.CollectInfo = function () {
            if (!this.CheckValue()) return "";
            var BackStr = "";
            var T_Desmp = this.DescriptionObj.value.trim();
            if (T_Desmp == "") {
                T_Desmp = this.QuessMainObj.value.substring(0, 10);
            }
            BackStr += (escape(T_Desmp) + "'");
            T_Desmp = this.ScoreObj.value.trim();
            if (T_Desmp == "") {
                T_Desmp = "0";
            }
            BackStr += (T_Desmp + "'");
            BackStr += (this.DifficultObj.value + "'");
            BackStr += (escape(this.QuessMainObj.value) + "'");
            var SaveItemQuessStr = this.SaveItemQuess();
            if (SaveItemQuessStr == "") {
                return "";
            }
            BackStr += SaveItemQuessStr;
            return BackStr;
        }

        this.SaveQuess = function () {
            var SaveStr = this.CollectInfo();
            if (SaveStr == "") return;
            var RunFunc = null;
            if (this.LinkId != null && this.LinkId != -1) {
                RunFunc = new Array(this.ObjNameStr + ".ClearQuess();", "CreateSubFunc(" + this.LinkId + ");");
            }
            else {
                RunFunc = new Array(this.ObjNameStr + ".ClearQuess();", "CreateRunFunc();");
            }
            if (this.LinkId == null || this.LinkId == "") this.LinkId = 0;
            parent.left.LoadDocFunc("添加多选题目", "pagequestion?type=AddSelect&style=1&linkid=" + this.LinkId, RunFunc, null, SaveStr);
        }

        this.ReturnSaveQuess = function () {
            var SaveStr = this.CollectInfo();
            if (SaveStr == "") return;
            try {
                newSelectObj.SetSubQuesSave(SaveStr, 1);
            }
            catch (e) { }
            this.ClearQuess();
        }

        this.UpdateQuess = function (id) {
            var UpdateStr = this.CollectInfo();
            if (UpdateStr == "") return;
            var RunFunc = new Array(this.ObjNameStr + ".UpdateQuessRunFunc(\"" + UpdateStr + "\");");
            parent.left.LoadDocFunc("更新多选题目", "pagequestion?type=UpdateSelect&quesid=" + id, RunFunc, null, UpdateStr);
        }

        this.ReturnUpdateQuess = function (id) {
            var UpdateStr = this.CollectInfo();
            if (UpdateStr == "") return;
            try {
                newSelectObj.UpdateSubQuesSave(UpdateStr, id);
            }
            catch (e) { }
            this.UpdateQuessRunFunc(UpdateStr);
        }

        this.UpdateQuessRunFunc = function (InfoStr) {
            this.StatusStr = InfoStr;
            this.ReSetQuess();
            try {
                this.TableObj.parentNode.parentNode.parentNode.parentNode.rows[0].cells[1].innerHTML = "<b>" + unescape(this.DescriptionObj.value) + "</b>";
            }
            catch (e) { }
        }


        this.ClearQuessItem = function () {
            for (var i = 0; i < this.QuessSubTableObj.rows.length; i++) {
                this.QuessSubTableObj.deleteRow(i);
                i--;
            }
        }

        this.ClearQuess = function () {
            this.StatusStr = "";
            this.DescriptionObj.value = "";
            this.ScoreObj.value = "4";
            this.DifficultObj.value = "1";
            this.QuessMainObj.value = "";
            this.ClearQuessItem();
            this.AddQuess(0);
            this.AddQuess(0);
        }

        this.ReSetQuess = function () {
            if (this.StatusStr == null || this.StatusStr == "") {
                this.ClearQuess();
                return;
            }
            var ContentArray = this.StatusStr.split("'");
            this.DescriptionObj.value = unescape(ContentArray[0]);
            this.ScoreObj.value = ContentArray[1];
            this.DifficultObj.value = ContentArray[2];
            this.QuessMainObj.value = unescape(ContentArray[3]);
            this.ClearQuessItem();
            var ItemQuessArray = ContentArray[4].split(",");
            for (var i = 0; i < ItemQuessArray.length; i++) {
                this.AddQuess(0);
                this.SetQuessText(i, unescape(ItemQuessArray[i]));
            }
            this.SetQuessAnswer(ContentArray[5]);
        }

        this.ReSetQuess();
    }


    function JudgeSelect(ObjName, Panel, Status, id, Linkid) {
        var Html = "";
        this.SizeArray = new Array(25, 65);
        if (Linkid != null) {
            this.SizeArray = new Array(11, 51);
        }
        Html += '<table border="0" cellpadding="0" cellspacing="0" width="85%">';
        Html += '<tr><td>';
        Html += '检索描述：<input type="text" size="' + this.SizeArray[0] + '" class="ip1">';
        Html += '&nbsp;&nbsp;&nbsp;指定分数：<input type="text" size="3" class="ip1" value="1">';
        Html += '&nbsp;&nbsp;&nbsp;指定难度：<select size="1">';
        Html += '<option value="0">容易</option>';
        Html += '<option value="1" selected>一般</option>';
        Html += '<option value="2">较难</option>';
        Html += '<option value="3">困难</option>';
        Html += '</select>';
        Html += '</td></tr>';
        Html += '<tr><td>';
        Html += '试题题干：<input type="text" size="' + this.SizeArray[1] + '" class="ip1">';
        Html += '</td></tr>';
        Html += '<tr><td>';
        Html += '试题答案：&nbsp; <input type="radio" onClick="' + ObjName + '.ResultQuess();" value="1">对 &nbsp; &nbsp; <input type="radio" onClick="' + ObjName + '.ResultQuess();" value="0">错';
        Html += '</td></tr>';
        Html += '<tr><td align="right">';
        Html += '<br>';
        if (id == null) {
            if (Linkid == -1) {
                Html += '<img src="images/create.gif" alt="保存试题" onClick="' + ObjName + '.ReturnSaveQuess();">';
            }
            else {
                Html += '<img src="images/create.gif" alt="保存试题" onClick="' + ObjName + '.SaveQuess();">';
            }
        }
        else {
            if (Linkid == -1) {
                Html += '<img src="images/create.gif" alt="更新试题" onClick="' + ObjName + '.ReturnUpdateQuess(\'' + id + '\');">';
            }
            else {
                Html += '<img src="images/create.gif" alt="更新试题" onClick="' + ObjName + '.UpdateQuess(\'' + id + '\');">';
            }
        }
        Html += ' &nbsp; &nbsp; <img src="images/back.gif" alt="重新填写" onClick="' + ObjName + '.ReSetQuess();"> &nbsp; ';
        Html += '</td></tr>';
        Html += '</table>';

        var PackObj = null;
        if (Panel == null) {
            document.write("<span id='" + ObjName + "'>" + Html + "</span>");
            PackObj = document.getElementById(ObjName);
        }
        else {
            PackObj = document.createElement("span");
            PackObj.innerHTML = Html;
            if (typeof (Panel) == "string") {
                document.getElementById(Panel).appendChild(PackObj);
            }
            else {
                Panel.appendChild(PackObj);
            }
        }

        this.LinkId = Linkid;
        this.StatusStr = Status;
        this.ObjNameStr = ObjName;
        this.TableObj = PackObj.firstChild;
        var TmpArray = this.TableObj.rows[0].cells[0].childNodes;
        this.DescriptionObj = TmpArray[1];
        this.ScoreObj = TmpArray[3];
        this.DifficultObj = TmpArray[5];
        this.QuessMainObj = this.TableObj.rows[1].cells[0].childNodes[1];
        this.QuessSubTableObj = this.TableObj.rows[2].cells[0];


        this.ResultQuess = function () {
            var that = event.srcElement;
            that.checked = true;
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i] != that && InputObjS[i].type == "radio") {
                    InputObjS[i].checked = false;
                }
            }
        }


        this.SetQuessAnswer = function (Num) {
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "radio") {
                    InputObjS[i].checked = false;
                }
            }
            var Right = this.QuessSubTableObj.childNodes[1];
            var Wrong = this.QuessSubTableObj.childNodes[3];
            if (Num == 0) {
                Right.checked = false;
                Wrong.checked = true;
            }
            if (Num == 1) {
                Right.checked = true;
                Wrong.checked = false;
            }
        }


        this.SaveItemQuess = function () {
            var Right = this.QuessSubTableObj.childNodes[1];
            var Wrong = this.QuessSubTableObj.childNodes[3];
            var Answer = "-1";
            if (Right.checked) {
                Answer = "1";
            }
            if (Wrong.checked) {
                Answer = "0";
            }
            if (Answer == "-1") {
                alert("您没有给出题目正确答案，请更正！");
                return "";
            }
            return ("'" + Answer);
        }


        this.CheckValue = function () {
            if (/[^\d]+/.test(this.ScoreObj.value)) {
                alert("光标所在的文本框中输入的不是数值，请更正");
                this.ScoreObj.select();
                return false;
            }
            if (/^0\d*$/.test(this.ScoreObj.value)) {
                alert("光标所在的文本框中输入的不是数学整数，请更正");
                this.ScoreObj.select();
                return false;
            }
            if (this.QuessMainObj.value.trim() == "") {
                alert("光标所在的文本框中不能留空，请更正");
                this.QuessMainObj.focus();
                return false;
            }
            if (this.DescriptionObj.value.length > 32) {
                alert("检索描述信息不能大于32个字，请更正");
                this.DescriptionObj.focus();
                return false;
            }
            return true;
        }

        this.CollectInfo = function () {
            if (!this.CheckValue()) return "";
            var BackStr = "";
            var T_Desmp = this.DescriptionObj.value.trim();
            if (T_Desmp == "") {
                T_Desmp = this.QuessMainObj.value.substring(0, 10);
            }
            BackStr += (escape(T_Desmp) + "'");
            T_Desmp = this.ScoreObj.value.trim();
            if (T_Desmp == "") {
                T_Desmp = "0";
            }
            BackStr += (T_Desmp + "'");
            BackStr += (this.DifficultObj.value + "'");
            BackStr += (escape(this.QuessMainObj.value) + "'");
            var SaveItemQuessStr = this.SaveItemQuess();
            if (SaveItemQuessStr == "") {
                return "";
            }
            BackStr += SaveItemQuessStr;
            return BackStr;
        }

        this.SaveQuess = function () {
            var SaveStr = this.CollectInfo();
            if (SaveStr == "") return;
            var RunFunc = null;
            if (this.LinkId != null && this.LinkId != -1) {
                RunFunc = new Array(this.ObjNameStr + ".ClearQuess();", "CreateSubFunc(" + this.LinkId + ");");
            }
            else {
                RunFunc = new Array(this.ObjNameStr + ".ClearQuess();", "CreateRunFunc();");
            }
            if (this.LinkId == null || this.LinkId == "") this.LinkId = 0;
            parent.left.LoadDocFunc("添加判断题目", "pagequestion?type=AddSelect&style=2&linkid=" + this.LinkId, RunFunc, null, SaveStr);
        }

        this.ReturnSaveQuess = function () {
            var SaveStr = this.CollectInfo();
            if (SaveStr == "") return;
            try {
                newSelectObj.SetSubQuesSave(SaveStr, 2);
            }
            catch (e) { }
            this.ClearQuess();
        }

        this.UpdateQuess = function (id) {
            var UpdateStr = this.CollectInfo();
            if (UpdateStr == "") return;
            var RunFunc = new Array(this.ObjNameStr + ".UpdateQuessRunFunc(\"" + UpdateStr + "\");");
            parent.left.LoadDocFunc("更新判断题目", "pagequestion?type=UpdateSelect&quesid=" + id, RunFunc, null, UpdateStr);
        }

        this.ReturnUpdateQuess = function (id) {
            var UpdateStr = this.CollectInfo();
            if (UpdateStr == "") return;
            try {
                newSelectObj.UpdateSubQuesSave(UpdateStr, id);
            }
            catch (e) { }
            this.UpdateQuessRunFunc(UpdateStr);
        }

        this.UpdateQuessRunFunc = function (InfoStr) {
            this.StatusStr = InfoStr;
            this.ReSetQuess();
            try {
                this.TableObj.parentNode.parentNode.parentNode.parentNode.rows[0].cells[1].innerHTML = "<b>" + unescape(this.DescriptionObj.value) + "</b>";
            }
            catch (e) { }
        }

        this.ClearQuess = function () {
            this.StatusStr = "";
            this.DescriptionObj.value = "";
            this.ScoreObj.value = "1";
            this.DifficultObj.value = "1";
            this.QuessMainObj.value = "";
            this.SetQuessAnswer(3);
        }

        this.ReSetQuess = function () {
            if (this.StatusStr == null || this.StatusStr == "") {
                this.ClearQuess();
                return;
            }
            var ContentArray = this.StatusStr.split("'");
            this.DescriptionObj.value = unescape(ContentArray[0]);
            this.ScoreObj.value = ContentArray[1];
            this.DifficultObj.value = ContentArray[2];
            this.QuessMainObj.value = unescape(ContentArray[3]);
            this.SetQuessAnswer(ContentArray[5]);
        }

        this.ReSetQuess();
    }



    function ReadSelect(ObjName, Panel, Status, id) {
        var Html = "";
        Html += '<table border="0" cellpadding="0" cellspacing="0" width="85%">';
        Html += '<tr><td>';
        Html += '检索描述：<input type="text" size="43" class="ip1">';
        Html += '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;指定难度：<select size="1">';
        Html += '<option value="0">容易</option>';
        Html += '<option value="1" selected>一般</option>';
        Html += '<option value="2">较难</option>';
        Html += '<option value="3">困难</option>';
        Html += '</select>';
        Html += '</td></tr>';
        Html += '<tr><td>';
        Html += '阅读材料：<br>';
        Html += '<textarea cols="76" rows="6" onFocus="changeBig(this,15);" onBlur="changeBack(this,6);"></textarea><br>';
        Html += '</td></tr>';
        Html += '<tr><td align="right">';
        Html += '<br>';
        if (id == null) {
            Html += '<img src="images/edit.gif" alt="添加子题" onClick="' + ObjName + '.AddSubQuess();">';
            Html += ' &nbsp; &nbsp; <img src="images/create.gif" alt="保存试题" onClick="' + ObjName + '.SaveQuess();">';
            Html += ' &nbsp; &nbsp; <img src="images/back.gif" alt="重写试题" onClick="' + ObjName + '.EndQuess();"> &nbsp; ';
        }
        else {
            id = id.hchar();
            Html += '<img src="images/create.gif" alt="更新材料" onClick="' + ObjName + '.UpdateQuess(\'' + id + '\');">';
            Html += ' &nbsp; &nbsp; <img src="images/back.gif" alt="重写材料" onClick="' + ObjName + '.ReSetQuess();">';
            Html += ' &nbsp; &nbsp; <img src="images/edit.gif" alt="添加子题" onClick="' + ObjName + '.AddSubQuess();">';
            Html += ' &nbsp; &nbsp; <img src="images/show.gif" alt="子题列表" onClick="' + ObjName + '.ListSubQuess();"> &nbsp; ';
        }
        Html += '</td></tr>';
        Html += '<tr><td style="display: none;" align="center"><hr>';
        Html += '<table border="0" width="100%">';
        Html += '<tr><td>';
        Html += '添加子试题类型选择：<select size="1" name="type" onChange="' + ObjName + '.ChangeSubType(this);">';
        Html += '<option value="0" selected>单项选择</option>';
        Html += '<option value="1">多项选择</option>';
        Html += '<option value="2">对错判断</option>';
        Html += '</select>';
        Html += '</td></tr>';
        Html += '<tr><td width="100%">';
        Html += '<fieldset style="width: 100%; padding-top: 0px; text-align: center;">';
        Html += '</fieldset><br><br>';
        Html += '</td></tr>';
        Html += '</table>';
        Html += '</td></tr>';
        Html += '<tr><td style="display: none;" align="center">';
        Html += '<fieldset style="width: 100%; padding-top: 0px; text-align: center;"><legend>子试题列表</legend><br><span><table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD"></table></span><br>';
        Html += '</fieldset><br><br>';
        Html += '</td></tr>';
        Html += '</table>';

        var PackObj = null;
        if (Panel == null) {
            document.write("<span id='" + ObjName + "'>" + Html + "</span>");
            PackObj = document.getElementById(ObjName);
        }
        else {
            PackObj = document.createElement("span");
            PackObj.innerHTML = Html;
            if (typeof (Panel) == "string") {
                document.getElementById(Panel).appendChild(PackObj);
            }
            else {
                Panel.appendChild(PackObj);
            }
        }

        this.Linkid = ((id == null || id == "") ? -1 : id);
        this.StatusStr = Status;
        this.ObjNameStr = ObjName;
        this.TableObj = PackObj.firstChild;
        var TmpArray = this.TableObj.rows[0].cells[0].childNodes;
        this.DescriptionObj = TmpArray[1];
        this.DifficultObj = TmpArray[3];
        this.QuessMainObj = this.TableObj.rows[1].cells[0].childNodes[2];
        this.QuessNewSubObj = this.TableObj.rows[3].cells[0];
        this.QuessNewSubPanelObj = this.QuessNewSubObj.childNodes[1].rows[1].cells[0].firstChild;
        this.QuessSubTableObj = this.TableObj.rows[4].cells[0];
        this.newTmpSelectObj = null;
        this.SubQuesResult = [];
        this.SubQuesObjArray = [];
        this.SubPagesObj = null;

        this.SetSubQuesSave = function (Str, Type) {
            this.SubQuesResult[this.SubQuesResult.length] = Str + "'" + Type;
            this.ShowSubItem(Str, Type, (this.SubQuesResult.length - 1));
            if (this.SubQuesResult.length == 1) {
                alert("温馨提示：当您添加完此阅读理解的全部子试题后，需单击最上排的“保存试题”按钮后，\n系统才会将您创建的全部题目进行保存工作！");
            }
        }

        this.UpdateSubQuesSave = function (UpdateStr, id) {
            try {
                var TypeArray = this.SubQuesResult[id].split("'");
                var Type = TypeArray[TypeArray.length - 1];
                this.SubQuesResult[id] = UpdateStr + "'" + Type;
            }
            catch (e) { }
        }

        this.ShowSubItem = function (Str, Type, NumId) {
            if (this.QuessSubTableObj.style.display == "none") {
                this.QuessSubTableObj.style.display = "block";
            }
            var QuesType = "单项选择题";
            var QuesShort = unescape(Str.split("'")[0]);
            var QuesStatus = Str;
            var QuesId = NumId;
            switch (parseInt(Type)) {
                case 0:
                    QuesType = "单项选择题";
                    break;
                case 1:
                    QuesType = "多项选择题";
                    break;
                case 2:
                    QuesType = "对错判断题";
                    break;
            }

            var CreateHTML = '';
            CreateHTML += '<table border="0" cellpadding="0" cellspacing="0" width="445" height="50">';
            CreateHTML += '<tr>';
            CreateHTML += '<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="' + QuesId + '" name="SubQuesItemName" title="同步操作复选框"></td>';
            CreateHTML += '<td align="center"><b>' + QuesShort + '</b></td>';
            CreateHTML += '</tr>';
            CreateHTML += '<tr>';
            CreateHTML += '<td align="left"><span style="width: 332px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 试题类型：' + QuesType + '</span><img src="images/edit.gif" alt="展开编辑试题" onClick="ShowEditDiv(this);"> &nbsp; <img src="images/del.gif" alt="删除试题" onClick="' + this.ObjNameStr + '.DelFunc(\'' + QuesId + '\',this);"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的试题操作选框" onClick="' + this.ObjNameStr + '.selectQuesItem();"></td>';
            CreateHTML += '</tr>';
            CreateHTML += '<tr>';
            CreateHTML += '<td colspan="2" align="center" style="display: none;">';
            CreateHTML += '</td>';
            CreateHTML += '</tr>';
            CreateHTML += '</table>';

            var SubListObjT = this.QuessSubTableObj.firstChild.childNodes[2].firstChild;
            var RowNum = SubListObjT.rows.length;
            var CurrentRow = SubListObjT.insertRow(RowNum);
            var CurrentCell = CurrentRow.insertCell(0);
            CurrentCell.innerHTML = CreateHTML;
            var TmpListBase = CurrentCell.firstChild.rows[2].cells[0];
            switch (parseInt(Type)) {
                case 0:
                    this.SubQuesObjArray[this.SubQuesObjArray.length] = new SingleSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, QuesStatus, QuesId, -1);
                    break;
                case 1:
                    this.SubQuesObjArray[this.SubQuesObjArray.length] = new MultiSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, QuesStatus, QuesId, -1);
                    break;
                case 2:
                    this.SubQuesObjArray[this.SubQuesObjArray.length] = new JudgeSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, QuesStatus, QuesId, -1);
                    break;
            }
        }

        this.selectQuesItem = function () {
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "checkbox" && InputObjS[i].name == "SubQuesItemName") {
                    InputObjS[i].checked = !InputObjS[i].checked;
                }
            }
        }

        this.DelFunc = function (id, that) {
            this.SubQuesResult[id] = "";
            var RemoveRowObj = null;
            var TempTableObj = null;
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "checkbox" && InputObjS[i].name == "SubQuesItemName") {
                    if (InputObjS[i].checked && InputObjS[i].value != id) {
                        this.SubQuesResult[InputObjS[i].value] = "";
                        RemoveRowObj = InputObjS[i].parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
                        TempTableObj = RemoveRowObj.parentNode;
                        TempTableObj.deleteRow(RemoveRowObj.rowIndex);
                    }
                }
            }
            RemoveRowObj = that.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
            TempTableObj = RemoveRowObj.parentNode;
            TempTableObj.deleteRow(RemoveRowObj.rowIndex);
        }

        this.DelSubFunc = function (id) {
            var DelStr = id;
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "checkbox" && InputObjS[i].name == "SubQuesItemName") {
                    if (InputObjS[i].checked && InputObjS[i].value != id) {
                        DelStr = DelStr + "," + InputObjS[i].value;
                    }
                }
            }
            var TpageNum = this.SubPagesObj.page;
            var RunFunc = new Array(this.ObjNameStr + ".createSubQuesPages(" + TpageNum + ");");
            parent.left.LoadDocFunc("删除子试题", "pagequestion?type=delLinkSelect&subid=" + this.Linkid + "&ids=" + DelStr + "&pagenum=" + TpageNum, RunFunc);
        }


        this.AddSubQuess = function () {
            if (this.QuessNewSubPanelObj.innerHTML == "") {
                this.ChangeSubType();
            }
            if (this.QuessNewSubObj.style.display == "none") {
                this.QuessNewSubObj.style.display = "block";
            }
            else {
                this.QuessNewSubObj.style.display = "none";
            }
        }

        this.ChangeSubType = function (that) {
            var NewTypeVal = 0;
            if (that != null) {
                NewTypeVal = parseInt(that.value);
            }
            switch (NewTypeVal) {
                case 0:
                    this.QuessNewSubPanelObj.innerHTML = "<legend>子试题添加区（单项选择）</legend><br><span></span>";
                    var BasePanelObj = this.QuessNewSubPanelObj.childNodes[2];
                    this.newTmpSelectObj = new SingleSelect(this.ObjNameStr + ".newTmpSelectObj", BasePanelObj, null, null, this.Linkid);
                    break;
                case 1:
                    this.QuessNewSubPanelObj.innerHTML = "<legend>子试题添加区（多项选择）</legend><br><span></span>";
                    var BasePanelObj = this.QuessNewSubPanelObj.childNodes[2];
                    this.newTmpSelectObj = new MultiSelect(this.ObjNameStr + ".newTmpSelectObj", BasePanelObj, null, null, this.Linkid);
                    break;
                case 2:
                    this.QuessNewSubPanelObj.innerHTML = "<legend>子试题添加区（对错判断）</legend><br><span></span>";
                    var BasePanelObj = this.QuessNewSubPanelObj.childNodes[2];
                    this.newTmpSelectObj = new JudgeSelect(this.ObjNameStr + ".newTmpSelectObj", BasePanelObj, null, null, this.Linkid);
                    break;
            }
        }


        this.CheckValue = function () {
            if (this.QuessMainObj.value.trim() == "") {
                alert("光标所在的文本框中不能留空，请更正");
                this.QuessMainObj.focus();
                return false;
            }
            if (this.DescriptionObj.value.length > 32) {
                alert("检索描述信息不能大于32个字，请更正");
                this.DescriptionObj.focus();
                return false;
            }
            return true;
        }

        this.CollectInfo = function () {
            if (!this.CheckValue()) return "";
            var BackStr = "";
            var T_Desmp = this.DescriptionObj.value.trim();
            if (T_Desmp == "") {
                T_Desmp = this.QuessMainObj.value.substring(0, 10);
            }
            BackStr += (escape(T_Desmp) + "''");
            BackStr += (this.DifficultObj.value + "'");
            BackStr += (escape(this.QuessMainObj.value) + "''");
            return BackStr;
        }

        this.SaveQuess = function () {
            var SaveStr = this.CollectInfo();
            if (SaveStr == "") return;
            for (var i = 0; i < this.SubQuesResult.length; i++) {
                if (this.SubQuesResult[i].trim() == "") continue;
                SaveStr = SaveStr + "\"" + this.SubQuesResult[i];
            }
            var RunFunc = new Array(this.ObjNameStr + ".EndQuess();", "CreateRunFunc();");
            parent.left.LoadDocFunc("添加阅读试题", "pagequestion?type=AddLinkSelect&style=0", RunFunc, null, SaveStr);
        }

        this.UpdateQuess = function (id) {
            var UpdateStr = this.CollectInfo();
            if (UpdateStr == "") return;
            var RunFunc = new Array(this.ObjNameStr + ".UpdateQuessRunFunc(\"" + UpdateStr + "\");");
            parent.left.LoadDocFunc("更新阅读材料", "pagequestion?type=UpdateLinkSelect&linkid=" + id, RunFunc, null, UpdateStr);
        }

        this.UpdateQuessRunFunc = function (InfoStr) {
            this.StatusStr = InfoStr;
            this.ReSetQuess();
            try {
                this.TableObj.parentNode.parentNode.parentNode.parentNode.rows[0].cells[1].innerHTML = "<b>" + unescape(this.DescriptionObj.value) + "</b>";
            }
            catch (e) { }
        }

        this.ClearQuess = function () {
            this.StatusStr = "";
            this.DescriptionObj.value = "";
            this.DifficultObj.value = "1";
            this.QuessMainObj.value = "";
        }

        this.ListSubQuess = function () {
            if (this.QuessSubTableObj.firstChild.lastChild.tagName != "SPAN") {
                this.ListSubLoad();
            }
            if (this.QuessSubTableObj.style.display == "none") {
                this.QuessSubTableObj.style.display = "block";
            }
            else {
                this.QuessSubTableObj.style.display = "none";
            }
        }

        this.ListSubLoad = function () {
            var RunFunc = new Array(this.ObjNameStr + ".ListSubLoadRunFunc();");
            parent.left.LoadDocFunc("读取阅读子试题", "pagequestion?type=loadLinkSelectlist&subid=" + this.Linkid + "&pagenum=1", RunFunc);
        }

        this.ListSubLoadRunFunc = function () {
            this.SubPagesObj = new showPages(this.ObjNameStr + ".SubPagesObj");
            this.QuessSubTableObj.firstChild.innerHTML += ("<SPAN>" + this.SubPagesObj.returnHtml() + "</SPAN>");
            var DataArray = parent.left.DataArray;
            var PCount = parent.left.TmpStr;
            var TmpHTML = "";
            var SubQuesTmpStatus = [];
            this.SubQuesObjArray = [];
            for (var i = 0; i < DataArray.length; i++) {
                TmpHTML += this.createQuesItemFunc(DataArray[i]);
                var QuesStatus = "";
                QuesStatus += DataArray[i].ques_short + "'";
                QuesStatus += DataArray[i].ques_score + "'";
                QuesStatus += DataArray[i].ques_diff + "'";
                QuesStatus += DataArray[i].ques_text + "'";
                QuesStatus += DataArray[i].ques_item + "'";
                QuesStatus += DataArray[i].ques_answer;
                SubQuesTmpStatus[SubQuesTmpStatus.length] = { Status: QuesStatus, Id: DataArray[i].ques_id, Style: DataArray[i].ques_type };
            }
            if (TmpHTML != "") {
                TmpHTML = '<table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD">' + TmpHTML + '</table>';
                this.QuessSubTableObj.firstChild.childNodes[2].innerHTML = TmpHTML;
            }
            for (var i = 0; i < SubQuesTmpStatus.length; i++) {
                var TmpListBase = this.QuessSubTableObj.firstChild.childNodes[2].firstChild.rows[i].cells[0].firstChild.rows[2].cells[0];
                switch (parseInt(SubQuesTmpStatus[i].Style)) {
                    case 0:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new SingleSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                    case 1:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new MultiSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                    case 2:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new JudgeSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                }
            }
            this.SubPagesObj.pageCount = PCount;
            this.SubPagesObj.printHtml(2, 1);
        }

        this.createQuesItemFunc = function (ItemObj) {
            var QuesType = "单项选择题";
            var QuesId = "0";
            var QuesShort = "";
            var QuesStatus = "";
            switch (ItemObj.ques_type) {
                case "0":
                    QuesType = "单项选择题";
                    break;
                case "1":
                    QuesType = "多项选择题";
                    break;
                case "2":
                    QuesType = "对错判断题";
                    break;
            }
            QuesId = ItemObj.ques_id;
            QuesShort = unescape(ItemObj.ques_short);
            var CreateHTML = '<tr><td>';
            CreateHTML += '<table border="0" cellpadding="0" cellspacing="0" width="445" height="50">';
            CreateHTML += '<tr>';
            CreateHTML += '<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="' + QuesId + '" name="SubQuesItemName" title="同步操作复选框"></td>';
            CreateHTML += '<td align="center"><b>' + QuesShort + '</b></td>';
            CreateHTML += '</tr>';
            CreateHTML += '<tr>';
            CreateHTML += '<td align="left"><span style="width: 332px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 试题类型：' + QuesType + '</span><img src="images/edit.gif" alt="展开编辑试题" onClick="ShowEditDiv(this);"> &nbsp; <img src="images/del.gif" alt="删除试题" onClick="' + this.ObjNameStr + '.DelSubFunc(\'' + QuesId + '\',this);"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的试题操作选框" onClick="' + this.ObjNameStr + '.selectQuesItem();"></td>';
            CreateHTML += '</tr>';
            CreateHTML += '<tr>';
            CreateHTML += '<td colspan="2" align="center" style="display: none;">';
            CreateHTML += '</td>';
            CreateHTML += '</tr>';
            CreateHTML += '</table>';
            CreateHTML += '</td></tr>';

            return CreateHTML;
        }

        this.createSubQuesPages = function (PageNumT, Hide) {
            var DataArray = null;
            var PCount = null;
            if (Hide != null && Hide == true) {
                DataArray = parent.left.SDataArray;
                PCount = parent.left.STmpStr;
            }
            else {
                DataArray = parent.left.DataArray;
                PCount = parent.left.TmpStr;
            }

            var TmpHTML = "";
            var SubQuesTmpStatus = [];
            this.SubQuesObjArray = [];
            for (var i = 0; i < DataArray.length; i++) {
                TmpHTML += this.createQuesItemFunc(DataArray[i]);
                var QuesStatus = "";
                QuesStatus += DataArray[i].ques_short + "'";
                QuesStatus += DataArray[i].ques_score + "'";
                QuesStatus += DataArray[i].ques_diff + "'";
                QuesStatus += DataArray[i].ques_text + "'";
                QuesStatus += DataArray[i].ques_item + "'";
                QuesStatus += DataArray[i].ques_answer;
                SubQuesTmpStatus[SubQuesTmpStatus.length] = { Status: QuesStatus, Id: DataArray[i].ques_id, Style: DataArray[i].ques_type };
            }
            TmpHTML = '<table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD">' + TmpHTML + '</table>';
            this.QuessSubTableObj.firstChild.childNodes[2].innerHTML = TmpHTML;
            for (var i = 0; i < SubQuesTmpStatus.length; i++) {
                var TmpListBase = this.QuessSubTableObj.firstChild.childNodes[2].firstChild.rows[i].cells[0].firstChild.rows[2].cells[0];
                switch (parseInt(SubQuesTmpStatus[i].Style)) {
                    case 0:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new SingleSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                    case 1:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new MultiSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                    case 2:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new JudgeSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                }
            }
            if (PageNumT == null) PageNumT = this.SubPagesObj.page;
            this.SubPagesObj.changePageCount(PCount, PageNumT);
        }

        this.RefreshSubList = function () {
            if (this.QuessSubTableObj.firstChild.lastChild.tagName != "SPAN") return;
            var RunFunc = new Array(this.ObjNameStr + ".createSubQuesPages(1,true);");
            parent.left.SLoadDocFunc("pagequestion?type=loadLinkSelectlist&subid=" + this.Linkid + "&pagenum=1&other=1", RunFunc);
        }

        this.EndQuess = function () {
            this.ClearQuess();
            this.QuessNewSubObj.style.display = "none";
            this.QuessSubTableObj.style.display = "none";
            this.QuessSubTableObj.firstChild.innerHTML = '<legend>子试题列表</legend><br><span><table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD"></table></span><br>';
            this.QuessNewSubPanelObj.innerHTML = "";
            this.Linkid = -1;
            this.SubQuesResult = [];
        }

        this.ReSetQuess = function () {
            if (this.StatusStr == null || this.StatusStr == "") {
                this.ClearQuess();
                return;
            }
            var ContentArray = this.StatusStr.split("'");
            this.DescriptionObj.value = unescape(ContentArray[0]);
            this.DifficultObj.value = ContentArray[2];
            this.QuessMainObj.value = unescape(ContentArray[3]);
        }

        this.ReSetQuess();
    }


    function ListenSelect(ObjName, Panel, Status, id) {
        var Html = "";
        Html += '<table border="0" cellpadding="0" cellspacing="0" width="85%">';
        Html += '<tr><td>';
        Html += '检索描述：<input type="text" size="25" class="ip1">';
        Html += '&nbsp;播放次数：<input type="text" size="5" class="ip1">&nbsp;指定难度：<select size="1">';
        Html += '<option value="0">容易</option>';
        Html += '<option value="1" selected>一般</option>';
        Html += '<option value="2">较难</option>';
        Html += '<option value="3">困难</option>';
        Html += '</select>';
        Html += '</td></tr>';
        Html += '<tr><td>';
        Html += '音频材料：<input type="text" size="50" value="" class="ip1">&nbsp;<input type="button" value="上传音频" onClick="' + ObjName + '.upfileFunc(this);" class="ip2">';
        Html += '</td></tr>';
        Html += '<tr><td align="right">';
        Html += '<br>';
        if (id == null) {
            Html += '<img src="images/edit.gif" alt="添加子题" onClick="' + ObjName + '.AddSubQuess();">';
            Html += ' &nbsp; &nbsp; <img src="images/create.gif" alt="保存试题" onClick="' + ObjName + '.SaveQuess();">';
            Html += ' &nbsp; &nbsp; <img src="images/back.gif" alt="重写试题" onClick="' + ObjName + '.EndQuess();">';
        }
        else {
            id = id.hchar();
            Html += '<img src="images/create.gif" alt="更新材料" onClick="' + ObjName + '.UpdateQuess(\'' + id + '\');">';
            Html += ' &nbsp; &nbsp; <img src="images/back.gif" alt="重写材料" onClick="' + ObjName + '.ReSetQuess();">';
            Html += ' &nbsp; &nbsp; <img src="images/edit.gif" alt="添加子题" onClick="' + ObjName + '.AddSubQuess();">';
            Html += ' &nbsp; &nbsp; <img src="images/show.gif" alt="子题列表" onClick="' + ObjName + '.ListSubQuess();">';
        }
        Html += ' &nbsp; &nbsp; <img src="images/sound.gif" alt="试听音频" onClick="' + ObjName + '.playMusic();"> &nbsp; ';
        Html += '</td></tr>';
        Html += '<tr><td style="display: none;" align="center"><hr>';
        Html += '<table border="0" width="100%">';
        Html += '<tr><td>';
        Html += '添加子试题类型选择：<select size="1" name="type" onChange="' + ObjName + '.ChangeSubType(this);">';
        Html += '<option value="0" selected>单项选择</option>';
        Html += '<option value="1">多项选择</option>';
        Html += '<option value="2">对错判断</option>';
        Html += '</select>';
        Html += '</td></tr>';
        Html += '<tr><td width="100%">';
        Html += '<fieldset style="width: 100%; padding-top: 0px; text-align: center;">';
        Html += '</fieldset><br><br>';
        Html += '</td></tr>';
        Html += '</table>';
        Html += '</td></tr>';
        Html += '<tr><td style="display: none;" align="center">';
        Html += '<fieldset style="width: 100%; padding-top: 0px; text-align: center;"><legend>子试题列表</legend><br><span><table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD"></table></span><br>';
        Html += '</fieldset><br><br>';
        Html += '</td></tr>';
        Html += '</table>';

        var PackObj = null;
        if (Panel == null) {
            document.write("<span id='" + ObjName + "'>" + Html + "</span>");
            PackObj = document.getElementById(ObjName);
        }
        else {
            PackObj = document.createElement("span");
            PackObj.innerHTML = Html;
            if (typeof (Panel) == "string") {
                document.getElementById(Panel).appendChild(PackObj);
            }
            else {
                Panel.appendChild(PackObj);
            }
        }

        this.Linkid = ((id == null || id == "") ? -1 : id);
        this.StatusStr = Status;
        this.ObjNameStr = ObjName;
        this.TableObj = PackObj.firstChild;
        var TmpArray = this.TableObj.rows[0].cells[0].childNodes;
        this.DescriptionObj = TmpArray[1];
        this.PlayCount = TmpArray[3];
        this.DifficultObj = TmpArray[5];
        this.QuessMainObj = this.TableObj.rows[1].cells[0].childNodes[1];
        this.QuessNewSubObj = this.TableObj.rows[3].cells[0];
        this.QuessNewSubPanelObj = this.QuessNewSubObj.childNodes[1].rows[1].cells[0].firstChild;
        this.QuessSubTableObj = this.TableObj.rows[4].cells[0];
        this.newTmpSelectObj = null;
        this.SubQuesResult = [];
        this.SubQuesObjArray = [];
        this.SubPagesObj = null;


        this.upfileFunc = function (that) {
            var FileObj = upfilearea.document.getElementById("file");
            if (FileObj == null || upfilearea.FileLock) {
                alert("上次上传任务还未完成\n请您稍等片刻");
                return;
            }
            var oldStr = FileObj.value;
            FileObj.click();
            var newStr = FileObj.value;
            if (oldStr == newStr || newStr == "") return;

            var TypeIdObj = upfilearea.document.getElementById("typeid");
            var FormObj = upfilearea.document.getElementById("formObj");
            TypeIdObj.value = this.ObjNameStr;
            if (upfilearea.checkFunc(FormObj)) {
                parent.left.Lock = true;
                parent.left.CancelFunc = new Array(this.ObjNameStr + ".upfileFuncBad();");
                parent.parent.warn_Win("请稍候...<br>系统开始执行上传音频操作", 0);
                FormObj.submit();
            }
        }

        this.upfileFuncBad = function () {
            try {
                document.getElementById("upfilearea").src = "pageupfile.aspx";
            }
            catch (e) { }
        }

        this.upfileFuncEnd = function (FileName) {
            var FileNameStr = "music/" + FileName;
            this.QuessMainObj.value = FileNameStr;
        }

        this.playMusic = function () {
            var musicObj = document.getElementById("music");
            if (this.QuessMainObj.value.trim() == "") {
                alert("请填写音频材料地址，或者上传您的音频文件！");
                return;
            }
            if (musicObj.src == "" || musicObj.src != this.QuessMainObj.value.trim()) {
                musicObj.src = this.QuessMainObj.value.trim();
            }
            else {
                musicObj.src = "";
            }
        }

        this.SetSubQuesSave = function (Str, Type) {
            this.SubQuesResult[this.SubQuesResult.length] = Str + "'" + Type;
            this.ShowSubItem(Str, Type, (this.SubQuesResult.length - 1));
            if (this.SubQuesResult.length == 1) {
                alert("温馨提示：当您添加完此听力分析的全部子试题后，需单击最上排的“保存试题”按钮后，\n系统才会将您创建的全部题目进行保存工作！");
            }
        }

        this.UpdateSubQuesSave = function (UpdateStr, id) {
            try {
                var TypeArray = this.SubQuesResult[id].split("'");
                var Type = TypeArray[TypeArray.length - 1];
                this.SubQuesResult[id] = UpdateStr + "'" + Type;
            }
            catch (e) { }
        }

        this.ShowSubItem = function (Str, Type, NumId) {
            if (this.QuessSubTableObj.style.display == "none") {
                this.QuessSubTableObj.style.display = "block";
            }
            var QuesType = "单项选择题";
            var QuesShort = unescape(Str.split("'")[0]);
            var QuesStatus = Str;
            var QuesId = NumId;
            switch (parseInt(Type)) {
                case 0:
                    QuesType = "单项选择题";
                    break;
                case 1:
                    QuesType = "多项选择题";
                    break;
                case 2:
                    QuesType = "对错判断题";
                    break;
            }

            var CreateHTML = '';
            CreateHTML += '<table border="0" cellpadding="0" cellspacing="0" width="445" height="50">';
            CreateHTML += '<tr>';
            CreateHTML += '<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="' + QuesId + '" name="SubQuesItemName" title="同步操作复选框"></td>';
            CreateHTML += '<td align="center"><b>' + QuesShort + '</b></td>';
            CreateHTML += '</tr>';
            CreateHTML += '<tr>';
            CreateHTML += '<td align="left"><span style="width: 332px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 试题类型：' + QuesType + '</span><img src="images/edit.gif" alt="展开编辑试题" onClick="ShowEditDiv(this);"> &nbsp; <img src="images/del.gif" alt="删除试题" onClick="' + this.ObjNameStr + '.DelFunc(\'' + QuesId + '\',this);"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的试题操作选框" onClick="' + this.ObjNameStr + '.selectQuesItem();"></td>';
            CreateHTML += '</tr>';
            CreateHTML += '<tr>';
            CreateHTML += '<td colspan="2" align="center" style="display: none;">';
            CreateHTML += '</td>';
            CreateHTML += '</tr>';
            CreateHTML += '</table>';

            var SubListObjT = this.QuessSubTableObj.firstChild.childNodes[2].firstChild;
            var RowNum = SubListObjT.rows.length;
            var CurrentRow = SubListObjT.insertRow(RowNum);
            var CurrentCell = CurrentRow.insertCell(0);
            CurrentCell.innerHTML = CreateHTML;
            var TmpListBase = CurrentCell.firstChild.rows[2].cells[0];
            switch (parseInt(Type)) {
                case 0:
                    this.SubQuesObjArray[this.SubQuesObjArray.length] = new SingleSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, QuesStatus, QuesId, -1);
                    break;
                case 1:
                    this.SubQuesObjArray[this.SubQuesObjArray.length] = new MultiSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, QuesStatus, QuesId, -1);
                    break;
                case 2:
                    this.SubQuesObjArray[this.SubQuesObjArray.length] = new JudgeSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, QuesStatus, QuesId, -1);
                    break;
            }
        }

        this.selectQuesItem = function () {
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "checkbox" && InputObjS[i].name == "SubQuesItemName") {
                    InputObjS[i].checked = !InputObjS[i].checked;
                }
            }
        }

        this.DelFunc = function (id, that) {
            this.SubQuesResult[id] = "";
            var RemoveRowObj = null;
            var TempTableObj = null;
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "checkbox" && InputObjS[i].name == "SubQuesItemName") {
                    if (InputObjS[i].checked && InputObjS[i].value != id) {
                        this.SubQuesResult[InputObjS[i].value] = "";
                        RemoveRowObj = InputObjS[i].parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
                        TempTableObj = RemoveRowObj.parentNode;
                        TempTableObj.deleteRow(RemoveRowObj.rowIndex);
                    }
                }
            }
            RemoveRowObj = that.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
            TempTableObj = RemoveRowObj.parentNode;
            TempTableObj.deleteRow(RemoveRowObj.rowIndex);
        }

        this.DelSubFunc = function (id) {
            var DelStr = id;
            var InputObjS = this.QuessSubTableObj.getElementsByTagName("INPUT");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "checkbox" && InputObjS[i].name == "SubQuesItemName") {
                    if (InputObjS[i].checked && InputObjS[i].value != id) {
                        DelStr = DelStr + "," + InputObjS[i].value;
                    }
                }
            }
            var TpageNum = this.SubPagesObj.page;
            var RunFunc = new Array(this.ObjNameStr + ".createSubQuesPages(" + TpageNum + ");");
            parent.left.LoadDocFunc("删除子试题", "pagequestion?type=delLinkSelect&subid=" + this.Linkid + "&ids=" + DelStr + "&pagenum=" + TpageNum, RunFunc);
        }


        this.AddSubQuess = function () {
            if (this.QuessNewSubPanelObj.innerHTML == "") {
                this.ChangeSubType();
            }
            if (this.QuessNewSubObj.style.display == "none") {
                this.QuessNewSubObj.style.display = "block";
            }
            else {
                this.QuessNewSubObj.style.display = "none";
            }
        }

        this.ChangeSubType = function (that) {
            var NewTypeVal = 0;
            if (that != null) {
                NewTypeVal = parseInt(that.value);
            }
            switch (NewTypeVal) {
                case 0:
                    this.QuessNewSubPanelObj.innerHTML = "<legend>子试题添加区（单项选择）</legend><br><span></span>";
                    var BasePanelObj = this.QuessNewSubPanelObj.childNodes[2];
                    this.newTmpSelectObj = new SingleSelect(this.ObjNameStr + ".newTmpSelectObj", BasePanelObj, null, null, this.Linkid);
                    break;
                case 1:
                    this.QuessNewSubPanelObj.innerHTML = "<legend>子试题添加区（多项选择）</legend><br><span></span>";
                    var BasePanelObj = this.QuessNewSubPanelObj.childNodes[2];
                    this.newTmpSelectObj = new MultiSelect(this.ObjNameStr + ".newTmpSelectObj", BasePanelObj, null, null, this.Linkid);
                    break;
                case 2:
                    this.QuessNewSubPanelObj.innerHTML = "<legend>子试题添加区（对错判断）</legend><br><span></span>";
                    var BasePanelObj = this.QuessNewSubPanelObj.childNodes[2];
                    this.newTmpSelectObj = new JudgeSelect(this.ObjNameStr + ".newTmpSelectObj", BasePanelObj, null, null, this.Linkid);
                    break;
            }
        }


        this.CheckValue = function () {
            if (this.QuessMainObj.value.trim() == "") {
                alert("光标所在的文本框中不能留空，请更正");
                this.QuessMainObj.focus();
                return false;
            }
            if (this.DescriptionObj.value.length > 32) {
                alert("检索描述信息不能大于32个字，请更正");
                this.DescriptionObj.focus();
                return false;
            }
            if (this.PlayCount.value.trim() == "") {
                alert("光标所在的文本框中不能留空，请更正");
                this.PlayCount.focus();
                return false;
            }
            if (/[^\d]+/.test(this.PlayCount.value)) {
                alert("光标所在的文本框中输入的不是数值，请更正");
                this.PlayCount.select();
                return false;
            }
            return true;
        }

        this.CollectInfo = function () {
            if (!this.CheckValue()) return "";
            var BackStr = "";
            var T_Desmp = this.DescriptionObj.value.trim();
            if (T_Desmp == "") {
                T_Desmp = this.QuessMainObj.value.substring(0, 10);
            }
            BackStr += (escape(T_Desmp) + "''");
            BackStr += (this.DifficultObj.value + "'");
            BackStr += (escape(this.QuessMainObj.value) + "'");
            BackStr += (escape(this.PlayCount.value) + "'");
            return BackStr;
        }

        this.SaveQuess = function () {
            var SaveStr = this.CollectInfo();
            if (SaveStr == "") return;
            for (var i = 0; i < this.SubQuesResult.length; i++) {
                if (this.SubQuesResult[i].trim() == "") continue;
                SaveStr = SaveStr + "\"" + this.SubQuesResult[i];
            }
            var RunFunc = new Array(this.ObjNameStr + ".EndQuess();", "CreateRunFunc();");
            parent.left.LoadDocFunc("添加听力试题", "pagequestion?type=AddLinkSelect&style=1", RunFunc, null, SaveStr);
        }

        this.UpdateQuess = function (id) {
            var UpdateStr = this.CollectInfo();
            if (UpdateStr == "") return;
            var RunFunc = new Array(this.ObjNameStr + ".UpdateQuessRunFunc(\"" + UpdateStr + "\");");
            parent.left.LoadDocFunc("更新听力材料", "pagequestion?type=UpdateLinkSelect&linkid=" + id, RunFunc, null, UpdateStr);
        }

        this.UpdateQuessRunFunc = function (InfoStr) {
            this.StatusStr = InfoStr;
            this.ReSetQuess();
            try {
                this.TableObj.parentNode.parentNode.parentNode.parentNode.rows[0].cells[1].innerHTML = "<b>" + unescape(this.DescriptionObj.value) + "</b>";
            }
            catch (e) { }
        }

        this.ClearQuess = function () {
            this.StatusStr = "";
            this.DescriptionObj.value = "";
            this.DifficultObj.value = "1";
            this.QuessMainObj.value = "";
            this.PlayCount.value = "1";
        }

        this.ListSubQuess = function () {
            if (this.QuessSubTableObj.firstChild.lastChild.tagName != "SPAN") {
                this.ListSubLoad();
            }
            if (this.QuessSubTableObj.style.display == "none") {
                this.QuessSubTableObj.style.display = "block";
            }
            else {
                this.QuessSubTableObj.style.display = "none";
            }
        }

        this.ListSubLoad = function () {
            var RunFunc = new Array(this.ObjNameStr + ".ListSubLoadRunFunc();");
            parent.left.LoadDocFunc("读取听力子试题", "pagequestion?type=loadLinkSelectlist&subid=" + this.Linkid + "&pagenum=1", RunFunc);
        }

        this.ListSubLoadRunFunc = function () {
            this.SubPagesObj = new showPages(this.ObjNameStr + ".SubPagesObj");
            this.QuessSubTableObj.firstChild.innerHTML += ("<SPAN>" + this.SubPagesObj.returnHtml() + "</SPAN>");
            var DataArray = parent.left.DataArray;
            var PCount = parent.left.TmpStr;
            var TmpHTML = "";
            var SubQuesTmpStatus = [];
            this.SubQuesObjArray = [];
            for (var i = 0; i < DataArray.length; i++) {
                TmpHTML += this.createQuesItemFunc(DataArray[i]);
                var QuesStatus = "";
                QuesStatus += DataArray[i].ques_short + "'";
                QuesStatus += DataArray[i].ques_score + "'";
                QuesStatus += DataArray[i].ques_diff + "'";
                QuesStatus += DataArray[i].ques_text + "'";
                QuesStatus += DataArray[i].ques_item + "'";
                QuesStatus += DataArray[i].ques_answer;
                SubQuesTmpStatus[SubQuesTmpStatus.length] = { Status: QuesStatus, Id: DataArray[i].ques_id, Style: DataArray[i].ques_type };
            }
            if (TmpHTML != "") {
                TmpHTML = '<table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD">' + TmpHTML + '</table>';
                this.QuessSubTableObj.firstChild.childNodes[2].innerHTML = TmpHTML;
            }
            for (var i = 0; i < SubQuesTmpStatus.length; i++) {
                var TmpListBase = this.QuessSubTableObj.firstChild.childNodes[2].firstChild.rows[i].cells[0].firstChild.rows[2].cells[0];
                switch (parseInt(SubQuesTmpStatus[i].Style)) {
                    case 0:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new SingleSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                    case 1:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new MultiSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                    case 2:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new JudgeSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                }
            }
            this.SubPagesObj.pageCount = PCount;
            this.SubPagesObj.printHtml(2, 1);
        }

        this.createQuesItemFunc = function (ItemObj) {
            var QuesType = "单项选择题";
            var QuesId = "0";
            var QuesShort = "";
            var QuesStatus = "";
            switch (ItemObj.ques_type) {
                case "0":
                    QuesType = "单项选择题";
                    break;
                case "1":
                    QuesType = "多项选择题";
                    break;
                case "2":
                    QuesType = "对错判断题";
                    break;
            }
            QuesId = ItemObj.ques_id;
            QuesShort = unescape(ItemObj.ques_short);
            var CreateHTML = '<tr><td>';
            CreateHTML += '<table border="0" cellpadding="0" cellspacing="0" width="445" height="50">';
            CreateHTML += '<tr>';
            CreateHTML += '<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="' + QuesId + '" name="SubQuesItemName" title="同步操作复选框"></td>';
            CreateHTML += '<td align="center"><b>' + QuesShort + '</b></td>';
            CreateHTML += '</tr>';
            CreateHTML += '<tr>';
            CreateHTML += '<td align="left"><span style="width: 332px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 试题类型：' + QuesType + '</span><img src="images/edit.gif" alt="展开编辑试题" onClick="ShowEditDiv(this);"> &nbsp; <img src="images/del.gif" alt="删除试题" onClick="' + this.ObjNameStr + '.DelSubFunc(\'' + QuesId + '\',this);"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的试题操作选框" onClick="' + this.ObjNameStr + '.selectQuesItem();"></td>';
            CreateHTML += '</tr>';
            CreateHTML += '<tr>';
            CreateHTML += '<td colspan="2" align="center" style="display: none;">';
            CreateHTML += '</td>';
            CreateHTML += '</tr>';
            CreateHTML += '</table>';
            CreateHTML += '</td></tr>';

            return CreateHTML;
        }

        this.createSubQuesPages = function (PageNumT, Hide) {
            var DataArray = null;
            var PCount = null;
            if (Hide != null && Hide == true) {
                DataArray = parent.left.SDataArray;
                PCount = parent.left.STmpStr;
            }
            else {
                DataArray = parent.left.DataArray;
                PCount = parent.left.TmpStr;
            }

            var TmpHTML = "";
            var SubQuesTmpStatus = [];
            this.SubQuesObjArray = [];
            for (var i = 0; i < DataArray.length; i++) {
                TmpHTML += this.createQuesItemFunc(DataArray[i]);
                var QuesStatus = "";
                QuesStatus += DataArray[i].ques_short + "'";
                QuesStatus += DataArray[i].ques_score + "'";
                QuesStatus += DataArray[i].ques_diff + "'";
                QuesStatus += DataArray[i].ques_text + "'";
                QuesStatus += DataArray[i].ques_item + "'";
                QuesStatus += DataArray[i].ques_answer;
                SubQuesTmpStatus[SubQuesTmpStatus.length] = { Status: QuesStatus, Id: DataArray[i].ques_id, Style: DataArray[i].ques_type };
            }
            TmpHTML = '<table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD">' + TmpHTML + '</table>';
            this.QuessSubTableObj.firstChild.childNodes[2].innerHTML = TmpHTML;
            for (var i = 0; i < SubQuesTmpStatus.length; i++) {
                var TmpListBase = this.QuessSubTableObj.firstChild.childNodes[2].firstChild.rows[i].cells[0].firstChild.rows[2].cells[0];
                switch (parseInt(SubQuesTmpStatus[i].Style)) {
                    case 0:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new SingleSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                    case 1:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new MultiSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                    case 2:
                        this.SubQuesObjArray[this.SubQuesObjArray.length] = new JudgeSelect(this.ObjNameStr + ".SubQuesObjArray[" + this.SubQuesObjArray.length + "]", TmpListBase, SubQuesTmpStatus[i].Status, SubQuesTmpStatus[i].Id, this.Linkid);
                        break;
                }
            }
            if (PageNumT == null) PageNumT = this.SubPagesObj.page;
            this.SubPagesObj.changePageCount(PCount, PageNumT);
        }

        this.RefreshSubList = function () {
            if (this.QuessSubTableObj.firstChild.lastChild.tagName != "SPAN") return;
            var RunFunc = new Array(this.ObjNameStr + ".createSubQuesPages(1,true);");
            parent.left.SLoadDocFunc("pagequestion?type=loadLinkSelectlist&subid=" + this.Linkid + "&pagenum=1&other=1", RunFunc);
        }

        this.EndQuess = function () {
            this.ClearQuess();
            this.QuessNewSubObj.style.display = "none";
            this.QuessSubTableObj.style.display = "none";
            this.QuessSubTableObj.firstChild.innerHTML = '<legend>子试题列表</legend><br><span><table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD"></table></span><br>';
            this.QuessNewSubPanelObj.innerHTML = "";
            this.Linkid = -1;
            this.SubQuesResult = [];
        }

        this.ReSetQuess = function () {
            if (this.StatusStr == null || this.StatusStr == "") {
                this.ClearQuess();
                return;
            }
            var ContentArray = this.StatusStr.split("'");
            this.DescriptionObj.value = unescape(ContentArray[0]);
            this.DifficultObj.value = ContentArray[2];
            this.QuessMainObj.value = unescape(ContentArray[3]);
            this.PlayCount.value = ContentArray[4];
        }

        this.ReSetQuess();
    }



    function CreateSubFunc(linkid) {
        for (var i = 0; i < SelectSumObjArray.length; i++) {
            if ((SelectSumObjArray[i] instanceof ReadSelect) || (SelectSumObjArray[i] instanceof ListenSelect)) {
                if (SelectSumObjArray[i].Linkid == linkid) {
                    try {
                        SelectSumObjArray[i].RefreshSubList();
                    }
                    catch (e) { }
                }
            }
        }
    }


    function ShowEditDiv(that) {
        var TdObj = that.parentElement.parentElement.parentElement.rows[2].cells[0];
        var ImgButtonObj = TdObj.parentElement.parentElement.rows[1].cells[0].childNodes[1];
        if (TdObj.style.display == "none") {
            ImgButtonObj.alt = "收缩编辑试题";
            TdObj.style.display = "block";
        }
        else {
            ImgButtonObj.alt = "展开编辑试题";
            TdObj.style.display = "none";
        }
    }


    function CreateRunFunc() {
        CreateQuesList(1);
        parent.left.DataArray = [];
        changeTitle(1);
    }

    function CreateQuesList(PageNumT, PageSum) {
        if (PageNumT == null) PageNumT = SysQuesListPages.page;
        if (PageSum == null) PageSum = parent.left.TmpStr;
        SysQuesListPages.changePageCount(PageSum, PageNumT);
        var DataArray = parent.left.DataArray;
        var MangerListObjT = document.getElementById("queslist");
        for (var i = 0; i < MangerListObjT.childNodes.length; i++) {
            MangerListObjT.childNodes[i].removeNode(true);
        }
        SelectSumObjArray = [];
        for (var i = 0; i < DataArray.length; i++) {
            CreateQuesRecord(DataArray[i]);
        }
    }


    function CreateQuesRecord(ItemObj) {
        var QuesType = "单项选择题";
        var QuesId = "0";
        var QuesShort = "";
        var QuesStatus = "";
        var QuesFunc = 0;

        if (ItemObj.ques_link == "0") {
            switch (ItemObj.ques_type) {
                case "0":
                    QuesType = "单项选择题";
                    QuesFunc = 0;
                    break;
                case "1":
                    QuesType = "多项选择题";
                    QuesFunc = 1;
                    break;
                case "2":
                    QuesType = "对错判断题";
                    QuesFunc = 2;
                    break;
            }
            QuesId = ItemObj.ques_id;
            QuesShort = unescape(ItemObj.ques_short);
            QuesStatus += ItemObj.ques_short + "'";
            QuesStatus += ItemObj.ques_score + "'";
            QuesStatus += ItemObj.ques_diff + "'";
            QuesStatus += ItemObj.ques_text + "'";
            QuesStatus += ItemObj.ques_item + "'";
            QuesStatus += ItemObj.ques_answer;
        }
        else {
            switch (ItemObj.link_type) {
                case "0":
                    QuesType = "阅读理解题";
                    QuesFunc = 3;
                    break;
                case "1":
                    QuesType = "听力分析题";
                    QuesFunc = 4;
                    break;
            }
            QuesId = "_" + ItemObj.link_id;
            QuesShort = unescape(ItemObj.link_short);
            QuesStatus += ItemObj.link_short + "''";
            QuesStatus += ItemObj.link_diff + "'";
            QuesStatus += ItemObj.link_text + "'";
            QuesStatus += unescape(ItemObj.link_bak) + "'";
        }
        var CreateHTML = '';
        CreateHTML += '<table border="0" cellpadding="0" cellspacing="0" width="550" height="50">';
        CreateHTML += '<tr>';
        CreateHTML += '<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="' + QuesId + '" name="QuesItemName" title="同步操作复选框"></td>';
        CreateHTML += '<td align="center"><b>' + QuesShort + '</b></td>';
        CreateHTML += '</tr>';
        CreateHTML += '<tr>';
        CreateHTML += '<td align="left"><span style="width: 430px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 试题类型：' + QuesType + '</span><img src="images/edit.gif" alt="展开编辑试题" onClick="ShowEditDiv(this);"> &nbsp; <img src="images/del.gif" alt="删除试题" onClick="DelFunc(\'' + QuesId + '\');"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的试题操作选框" onClick="selectQuesItem();"></td>';
        CreateHTML += '</tr>';
        CreateHTML += '<tr>';
        CreateHTML += '<td colspan="2" align="center" style="display: none;">';
        CreateHTML += '</td>';
        CreateHTML += '</tr>';
        CreateHTML += '</table>';

        var MangerListObjT = document.getElementById("queslist");
        var RowNum = MangerListObjT.rows.length;
        var CurrentRow = MangerListObjT.insertRow(RowNum);
        var CurrentCell = CurrentRow.insertCell(0);
        CurrentCell.innerHTML = CreateHTML;
        var TmpListBase = CurrentCell.firstChild.rows[2].cells[0];
        switch (QuesFunc) {
            case 0:
                SelectSumObjArray[SelectSumObjArray.length] = new SingleSelect("SelectSumObjArray[" + SelectSumObjArray.length + "]", TmpListBase, QuesStatus, QuesId);
                break;
            case 1:
                SelectSumObjArray[SelectSumObjArray.length] = new MultiSelect("SelectSumObjArray[" + SelectSumObjArray.length + "]", TmpListBase, QuesStatus, QuesId);
                break;
            case 2:
                SelectSumObjArray[SelectSumObjArray.length] = new JudgeSelect("SelectSumObjArray[" + SelectSumObjArray.length + "]", TmpListBase, QuesStatus, QuesId);
                break;
            case 3:
                SelectSumObjArray[SelectSumObjArray.length] = new ReadSelect("SelectSumObjArray[" + SelectSumObjArray.length + "]", TmpListBase, QuesStatus, QuesId);
                break;
            case 4:
                SelectSumObjArray[SelectSumObjArray.length] = new ListenSelect("SelectSumObjArray[" + SelectSumObjArray.length + "]", TmpListBase, QuesStatus, QuesId);
                break;
        }
    }

    function DelFunc(Num, that) {
        if (confirm("您真的要删除此试题及其所选试题吗？\n删除后将无法恢复")) {
            var LinkIds = "";
            var QuesIds = "";
            Num = Num + "";
            if (Num.hchar() == Num) {
                QuesIds = Num;
            }
            else {
                LinkIds = Num.hchar();
            }
            var currentPageNum = SysQuesListPages.page;
            var InputObjS = document.getElementsByName("QuesItemName");
            for (var i = 0; i < InputObjS.length; i++) {
                if (InputObjS[i].type == "checkbox" && InputObjS[i].checked) {
                    if (InputObjS[i].value != Num) {
                        var CTNum = InputObjS[i].value;
                        if (CTNum.hchar() == CTNum) {
                            if (QuesIds == "") {
                                QuesIds = CTNum;
                            }
                            else {
                                QuesIds = QuesIds + "," + CTNum;
                            }
                        }
                        else {
                            if (LinkIds == "") {
                                LinkIds = CTNum.hchar();
                            }
                            else {
                                LinkIds = LinkIds + "," + CTNum.hchar();
                            }
                        }
                    }
                }
            }
            var RunFunc = new Array("DelRunFunc(" + currentPageNum + ");");
            parent.left.LoadDocFunc("删除试题", "pagequestion?type=delSelect&linkid=" + LinkIds + "&quesid=" + QuesIds + "&PageN=" + currentPageNum, RunFunc);
        }
    }

    function DelRunFunc(pageNum) {
        CreateQuesList(pageNum, parent.left.TmpStr);
        parent.left.DataArray = [];
    }

    function selectQuesItem() {
        var InputObjS = document.getElementsByName("QuesItemName");
        for (var i = 0; i < InputObjS.length; i++) {
            if (InputObjS[i].type == "checkbox") {
                InputObjS[i].checked = !InputObjS[i].checked;
            }
        }
    }


    function showCurrentPageFunc(nameStr, Num) {
        var ObjName = nameStr;
        var PageNum = Num;

        if (ObjName.indexOf("SysQuesListPages") == 0) {
            var RunFunc = ["CreateQuesList(" + PageNum + ");"];
            parent.left.LoadDocFunc("读取系统试题列表", "pagequestion?type=loadlist&pagenum=" + PageNum, RunFunc);
        }
        else if (ObjName.indexOf("SelectSumObjArray") == 0) {
            var TmpObj = ObjName.substring(0, ObjName.indexOf(".SubPagesObj"));
            var RunFunc = [TmpObj + ".createSubQuesPages(" + PageNum + ");"];
            parent.left.LoadDocFunc("读取子试题列表", "pagequestion?type=loadLinkSelectlist&subid=" + eval(TmpObj + ".Linkid") + "&pagenum=" + PageNum, RunFunc);
        }
    }

    function showAllContent(nameStr, that) {
        var ObjName = nameStr;

        if (ObjName.indexOf("SysQuesListPages") == 0) {
            if (that.value == "呈现全部") {
                var RunFunc = ["CreateQuesList();"];
                parent.left.LoadDocFunc("读取系统试题列表", "pagequestion?type=loadlist&pagenum=0", RunFunc);
                that.value = "恢复分页";
            }
            else {
                var TmpPageNumT = SysQuesListPages.page;
                var RunFunc = ["CreateQuesList(" + TmpPageNumT + ");"];
                parent.left.LoadDocFunc("读取系统试题列表", "pagequestion?type=loadlist&pagenum=" + TmpPageNumT, RunFunc);
                that.value = "呈现全部";
            }
        }
        else if (ObjName.indexOf("SelectSumObjArray") == 0) {
            var TmpObj = ObjName.substring(0, ObjName.indexOf(".SubPagesObj"));
            if (that.value == "呈现全部") {
                var RunFunc = [TmpObj + ".createSubQuesPages();"];
                parent.left.LoadDocFunc("读取子试题列表", "pagequestion?type=loadLinkSelectlist&subid=" + eval(TmpObj + ".Linkid") + "&pagenum=0", RunFunc);
                that.value = "恢复分页";
            }
            else {
                var TmpPageNumT = eval(ObjName + ".page");
                var RunFunc = [TmpObj + ".createSubQuesPages(" + TmpPageNumT + ");"];
                parent.left.LoadDocFunc("读取子试题列表", "pagequestion?type=loadLinkSelectlist&subid=" + eval(TmpObj + ".Linkid") + "&pagenum=" + TmpPageNumT, RunFunc);
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
  <table border="0" cellpadding="0" cellspacing="0" class="TableStyle">
    <tr>
      <td height="22" align="center"  class="HeadStyle">添加新试题</td>
    </tr>
    <tr>
      <td height="80" align="center" class="BodyStyle">
	  
	  <table border="0" cellpadding="0" cellspacing="0"  width="550">
	  <tr>
	   	<td align="center" valign="middle">
			添加试题类型选择：<select size="1" name="type" onChange="ChangeNewType(this);">
			<option value="0" selected>单项选择题</option>
			<option value="1">多项选择题</option>
			<option value="2">对错判断题</option>
			<option value="3">阅读理解题</option>
			<option value="4">听力分析题</option>
			</select>
		</td>
	  </tr>
	   <tr>
	   	<td align="center" valign="middle">
		<fieldset style="width: 100%; padding-top: 0px;" id="newquesspanel"><legend>试题添加区（单项选择）</legend><br><span>
			<script language="javascript">
			    newSelectObj = new SingleSelect("newSelectObj");
			</script>
		</span>
		</fieldset>
		</td>
	   </tr>
	  </table>

      </td>
    </tr>
	</table>
	
    <p></p>

<script language="javascript">
    function ChangeNewType(that) {
        var NewTypeVal = parseInt(that.value);
        var QuesPanelObj = document.getElementById("newquesspanel");
        switch (NewTypeVal) {
            case 0:
                QuesPanelObj.innerHTML = "<legend>试题添加区（单项选择）</legend><br><span></span>";
                var BasePanelObj = QuesPanelObj.childNodes[2];
                newSelectObj = new SingleSelect("newSelectObj", BasePanelObj);
                break;
            case 1:
                QuesPanelObj.innerHTML = "<legend>试题添加区（多项选择）</legend><br><span></span>";
                var BasePanelObj = QuesPanelObj.childNodes[2];
                newSelectObj = new MultiSelect("newSelectObj", BasePanelObj);
                break;
            case 2:
                QuesPanelObj.innerHTML = "<legend>试题添加区（对错判断）</legend><br><span></span>";
                var BasePanelObj = QuesPanelObj.childNodes[2];
                newSelectObj = new JudgeSelect("newSelectObj", BasePanelObj);
                break;
            case 3:
                QuesPanelObj.innerHTML = "<legend>试题添加区（阅读理解）</legend><br><span></span>";
                var BasePanelObj = QuesPanelObj.childNodes[2];
                newSelectObj = new ReadSelect("newSelectObj", BasePanelObj);
                break;
            case 4:
                QuesPanelObj.innerHTML = "<legend>试题添加区（听力分析）</legend><br><span></span>";
                var BasePanelObj = QuesPanelObj.childNodes[2];
                newSelectObj = new ListenSelect("newSelectObj", BasePanelObj);
                break;
        }
    }


    var StatusPage = 1;

    function ListPageFunc() {
        var Ttxt = document.getElementById("listname").innerHTML;
        if (Ttxt == "系统试题列表") {
            StatusPage = SysQuesListPages.page;
        }
    }

    function SearchFunc(sign) {
        var BackStr = "";
        var RunNum = 1;
        if (sign == "short") {
            var TxtObj = document.getElementById("fastword");
            var TxtStr = TxtObj.value;
            if (TxtStr == "") {
                alert("快速搜索框中不能为空，请更正");
                TxtObj.select();
                return;
            }
            BackStr = escape(TxtStr);
            RunNum = 1;
        }
        else {
            var SeleObj = document.getElementById("fastsele");
            BackStr = (SeleObj.value.trim() == "") ? 0 : SeleObj.value.trim();
            RunNum = 2;
        }
        ListPageFunc();
        var RunFunc = new Array("SearchFuncRun();");
        parent.left.LoadDocFunc("快速搜索试题", "pagequestion?type=search&sign=" + RunNum, RunFunc, null, BackStr);
    }

    function SearchFuncRun() {
        CreateQuesList(1);
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
        if (Ttxt == "系统试题列表") {
            alert("当前已是系统试题列表不用恢复！");
            return;
        }
        var RunFunc = ["changeTitle(1)", "CreateQuesList(" + StatusPage + ");"];
        parent.left.LoadDocFunc("恢复系统试题列表", "pagequestion?type=reviece&pagenum=" + StatusPage, RunFunc);
    }

    function changeTitle(sign) {
        if (sign == 1) {
            document.getElementById("listname").innerHTML = "系统试题列表";
        }
        else {
            document.getElementById("listname").innerHTML = "试题搜索列表";
        }
    }

    function ChangeSearchPanel(Sign) {
        if (Sign == 0) {
            document.getElementById("fastsele").style.display = "block";
            document.getElementById("fastword").style.display = "none";
        }
        else {
            document.getElementById("fastsele").style.display = "none";
            document.getElementById("fastword").style.display = "block";
        }
    }
</script>
			  <table border="0" cellpadding="0" cellspacing="0" class="TableStyle">
			    <tr>
      				<td height="22" align="center" class="HeadStyle">搜索条件设定</td>
    			</tr>
    			<tr>
      				<td class="BodyStyle" align="center">
						快速搜索: <span style="width: 150; text-align: center;"><select size="1" name="fastsele" id="fastsele" style="display:block;">
							<option value="0" checked>单项选择</option>
							<option value="1">多项选择</option>
							<option value="2">对错判断</option>
							<option value="3">阅读理解</option>
							<option value="4">听力分析</option>
							</select><input type="text" size="20" class="ip1" id="fastword" style="display:none;"></span>&nbsp;
						<input type="button" value="题型搜索" class="ip2" onMouseOver="ChangeSearchPanel(0);" onClick="SearchFunc('type');">&nbsp;
						<input type="button" value="描述搜索" class="ip2" onMouseOver="ChangeSearchPanel(1);" onClick="SearchFunc('short');">&nbsp;
						<input type="button" value="高级搜索" class="ip2" onClick="hiddenSearchArea();">&nbsp;
						<input type="button" value="恢复列表" class="ip2" onClick="changeSearchNormal();"><br><br>
						<fieldset style="width: 90%; display:none; padding-top: 0px;" id="BetterSearchArea"><legend>高级搜索条件设定区</legend><br>
							搜索字段：<select size="1" name="filed" id="filed" onChange="BetterSearchObj.ChangeFiled();">
							<option value="0" checked>检索描述</option>
							<option value="1">试题类型</option>
							<option value="2">创建日期</option>
							<option value="3">试题难度</option>
							<option value="4">试题分数</option>
							</select> &nbsp; 
							操作条件：<select size="1" name="opare" id="opare">
							<option value="0" checked>模糊查询</option>
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
                    this.AddOptions("0", "模糊查询");
                    break;
                case "1":
                    this.AddOptions("0", "单项选择");
                    this.AddOptions("1", "多项选择");
                    this.AddOptions("2", "对错判断");
                    this.AddOptions("3", "阅读理解");
                    this.AddOptions("4", "听力分析");
                    break;
                case "2":
                    this.AddOptions("0", "大于操作");
                    this.AddOptions("1", "小于操作");
                    this.AddOptions("2", "等于操作");
                    this.AddOptions("3", "不等操作");
                    break;
                case "3":
                    this.AddOptions("0", "容易级别");
                    this.AddOptions("1", "一般级别");
                    this.AddOptions("2", "较难级别");
                    this.AddOptions("3", "困难级别");
                    break;
                case "4":
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
            if (sign == 0) {
                this.FOSpanObj.style.display = "block";
                this.DataSpanObj.style.display = "none";
            }
            else if (sign == 1) {
                this.FOSpanObj.style.display = "none";
                this.DataSpanObj.style.display = "block";
            }
            else {
                this.FOSpanObj.style.display = "none";
                this.DataSpanObj.style.display = "none";
            }
        }
        this.ChangeFiled = function () {
            var t_filedval = this.FiledObj.value;
            switch (t_filedval) {
                case "0":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(0);
                    break;
                case "1":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(2);
                    break;
                case "2":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(1);
                    break;
                case "3":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(2);
                    break;
                case "4":
                    this.ChangeOpare(t_filedval);
                    this.ChangeFotxt(0);
                    break;
                default:
                    this.ChangeOpare("0");
                    this.ChangeFotxt(0);
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
                    txt += "检索描述";
                    break;
                case "1":
                    txt += "试题类型";
                    break;
                case "2":
                    txt += "创建日期";
                    break;
                case "3":
                    txt += "试题难度";
                    break;
                case "4":
                    txt += "试题分数";
                    break;
                default:
                    t_filedval = "0";
                    txt += "检索描述";
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
            if (t_filedval == "0" || t_filedval == "4") {
                if (T_Obj.value == "") {
                    alert("光标所在的文本框不能为空，请输入内容！");
                    T_Obj.focus();
                    return "";
                }
            }
            if (t_filedval == "0") {
                returntxt = escape(T_Obj.value);
            }
            if (t_filedval == "4") {
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
            parent.left.LoadDocFunc("搜索相关试题", "pagequestion?type=search&sign=3", RunFunc, null, BackTxt);
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
      <td height="22" align="center" class="HeadStyle"><span id="listname">系统试题列表</span></td>
    </tr>
    <tr>
      <td align="center" class="BodyStyle">
      <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#817FAD" id="queslist">
      <asp:Repeater runat="server" ID="repQuestionList">
      <ItemTemplate>
        <tr>
          <td>
			<table border="0" cellpadding="0" cellspacing="0" width="550" height="50">
		  		<tr>
		  			<td rowspan="2" align="left" width="20" valign="middle">&nbsp;<input type="checkbox" value="<%# Eval("QuestionId")%>" name="QuesItemName" title="同步操作复选框"></td>
		  			<td align="center"><b><script language="javascript">document.write(unescape("<%# Eval("QuestionShortDescription")%>"))</script></b></td>
		  		</tr>
		  		<tr>
		  			<td align="left"><span style="width: 430px;">&nbsp;<img src="images/admin.gif" width="11" height="14">&nbsp; 试题类型：<%# Eval("QuestionType")%></span><img src="images/edit.gif" alt="展开编辑试题" onClick="ShowEditDiv(this);"> &nbsp; <img src="images/del.gif" alt="删除试题" onClick="DelFunc('<%# Eval("QuestionId")%>',this);"> &nbsp; <img src="images/select.gif" alt="反向选择当前页面的试题操作选框" onClick="selectQuesItem();"></td>
		  		</tr>
				<tr>
					<td colspan="2" align="center" style="display: none;">
						<script language="javascript">
							SelectSumObjArray[<%# Container.ItemIndex%>] = new <%# Eval("QuestionClientFunction")%>("SelectSumObjArray[<%# Container.ItemIndex%>]",null,"<%# Eval("QuestionStatus")%>","<%# Eval("QuestionId")%>");
						</script>
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
			var SysQuesListPages = new showPages('SysQuesListPages');
			SysQuesListPages.pageCount = <%=this.QuestionCollectionPageCount%>; // 定义总页数(必要)
			SysQuesListPages.printHtml(1);
		</script>
	</span>
      </td>
    </tr>
	</table>

<p></p>
<% if (this.IsControlSettingParam)
   {%>
  <table border="0" cellpadding="0" cellspacing="0" align="center" class="TableStyle">
    <tr>
      <td height="22" align="center" class="HeadStyle">综合设置区</td>
    </tr>
	<tr>
	<td class="BodyStyle" align="center">
	<fieldset style="width: 95%; display: block; padding-top: 0px;"><legend>综合设置修改区</legend><br>
		<table border="0" cellspacing="0" cellpadding="0" id="SMPanelP">
			<tr><td>
			<b>上传音频功能允许上传音频文件的最大值(以KB计算)：</b><input type="text" size="31" class="ip1" ><br>
			<b>系统后台Session活动最长时间设置参数(以分钟计算)：</b><input type="text" size="30" class="ip1"><br>
            <b>网络考场中的考试总时间设定(以分钟计算)：</b><input type="text" size="30" class="ip1"><br>
            <b>网络考场中的每类体型的出题数量(以个计算)：</b><input type="text" size="30" class="ip1"><br>
			</td>
			</tr>
		</table>
<br>
	<input type="hidden" value="'" id="BackCSHidArea"><input type="button" value="保存设置" onClick="SMSaveFunc();" class="ip2"> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<input type="button" value="还原设置" onClick="SMBackFunc();" class="ip2"><br><br>
	</fieldset>
	</td>
	</tr>
  </table>
<%} %>
</div>

<!------------------------------------------------------------------>
<iframe scrolling="no" src="pageupfile.aspx" noResize id="upfilearea" name="upfilearea" style="display: none;"></iframe>

</body>
<script language="javascript">

    var SMDataAll = "<%= this.SystemSettingParamsString %>";

    function CheckSMData() {
        var SMPanelObjAlls = document.getElementById("SMPanelP").rows[0].cells[0].childNodes;
        var ReStrTT = "";
        var RegObj = /(\\|'|")/gi;
        var NumObj = new Array(1, 4, 7, 10);
        for (var i = 0; i < NumObj.length; i++) {
            var TestObj = SMPanelObjAlls[NumObj[i]];
            TestObj.value = TestObj.value.replace(/\\/g, "/");
            if (TestObj.value.trim() == "") {
                alert("光标所在的文本框中不能为空，请更正");
                TestObj.select();
                return "";
            }
            if (RegObj.test(TestObj.value)) {
                alert("光标所在的文本框中输入了非法字符，请更正");
                TestObj.select();
                return "";
            }
            if (/[^\x00-\xff]/.test(TestObj.value)) {
                alert("光标所在的文本框中输入了双角字符，请更正");
                TestObj.select();
                return "";
            }
            if (/[^\d]+/.test(TestObj.value)) {
                alert("光标所在的文本框中输入的不是数值，请更正");
                TestObj.select();
                return "";
            }
            if (/^0\d*$/.test(TestObj.value)) {
                alert("光标所在的文本框中输入的不是数学整数，请更正");
                TestObj.select();
                return "";
            }
            ReStrTT += TestObj.value + "'";
        }
        return ReStrTT;
    }


    function SMSaveFunc() {
        var SizeSMDTAr = CheckSMData().slice(0, -1);
        if (SizeSMDTAr == "") return;
        var SMDataT = "";
        var SMPanelObjAlls = document.getElementById("SMPanelP").rows[0].cells[0].childNodes;
        SMDataT += SMPanelObjAlls[1].value + "'";
        SMDataT += SMPanelObjAlls[4].value + "'";
        SMDataT += SMPanelObjAlls[7].value + "'";
        SMDataT += SMPanelObjAlls[10].value;
        document.getElementById("BackCSHidArea").value = SMDataT;
        var RunFunc = new Array("SMSaveRunFunc();");
        parent.left.LoadDocFunc("保存系统参数设置", "pagequestion?type=SaveParams", RunFunc, null, SMDataT);
    }

    function SMSaveRunFunc() {
        var newDBNameALLStr = document.getElementById("BackCSHidArea").value;
        SMDataAll = newDBNameALLStr;
        SMBackFunc();
    }

    function SMBackFunc() {
        var SMPanelObjAlls = document.getElementById("SMPanelP").rows[0].cells[0].childNodes;
        var SMDataAllArray = SMDataAll.split("'");
        SMPanelObjAlls[1].value = SMDataAllArray[0];
        SMPanelObjAlls[4].value = SMDataAllArray[1];
        SMPanelObjAlls[7].value = SMDataAllArray[2];
        SMPanelObjAlls[10].value = SMDataAllArray[3];
    }

<%if (this.IsControlSettingParam) { %>
    SMBackFunc();
<% } %>

    parent.parent.HideShowDivFunc(false);
</script>
</asp:Content>



