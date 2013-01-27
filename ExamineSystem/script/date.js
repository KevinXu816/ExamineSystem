var optmp = !!(window.opera && document.getElementById);	
var ietype = !!(navigator.userAgent.toLowerCase().indexOf("msie") >= 0 && document.all && !optmp);
var fftype = !!(typeof window.getComputedStyle != "undefined" && typeof document.createRange != "undefined");

var txtTag ='innerHTML';
var eventTag = ((fftype)?'event.target':'event.srcElement')
var isDay = 'class';
var isDayVal = 'className';


function getTrueNodeArray(thatArray)
{
	var oldNodeArray = thatArray;
	var returnNodeArray = [];	
	for (var i=0; i<oldNodeArray.length; i++)
	{
		if (oldNodeArray[i].nodeType != 1) continue;
		returnNodeArray[returnNodeArray.length] = oldNodeArray[i];
	}
	return returnNodeArray;
}


function getTruePreviousSibling(thatNode)
{
	do
	{
		if(thatNode.previousSibling.nodeType == 1)
		{
			return thatNode.previousSibling;
		}
		thatNode = thatNode.previousSibling;
	}
	while(thatNode != null && thatNode !="");
	return thatNode;
}


function setTableStyle(tableobj,colorstr,bgcolorstr)
{
	var currentObj = tableobj;
	var currentRows = currentObj.rows;
	for (var i=0; i<currentRows.length; i++)
	{
		var currentCells = currentRows[i].cells;
		for (var j=0; j<currentCells.length; j++)
		{
			currentCells[j].style.backgroundColor = bgcolorstr;
			currentCells[j].style.color = colorstr;
			currentCells[j].style.cursor="default";
		}
	}
}


function compareSetTxt(inpNameObj)
{
	var inpSign = /^\S+last$/;
	var prevDate;
	var nextDate;
	var prevInpObj;
	var nextInpObj;
    if (inpSign.test(inpNameObj.id))
    {
		var prevInpName = inpNameObj.id.substring(0,inpNameObj.id.lastIndexOf("last"));
		prevInpObj = document.getElementById(prevInpName);
		nextInpObj = inpNameObj;
		var prevInpVal = prevInpObj.value;
		var nextInpVal = nextInpObj.value;
		var DateTrue = /^(0?\d|1[0-2])\/([0-2]?\d|3[01])\/(19[7-9]\d|20[0-5]\d)$/;
		
		if(DateTrue.test(prevInpVal))
		{
			var DateArray = prevInpVal.split("/");
			prevDate = new Date(DateArray[2],DateArray[0],DateArray[1]);
		}
		else
		{
			var todayS = new Date();
      		var yearS = todayS.getFullYear();
      		var monthS = todayS.getMonth()+1;
      		var mdayS = todayS.getDate();
			prevDate = new Date(yearS,monthS,mdayS);
		}
		if(DateTrue.test(nextInpVal))
		{
			var DateArray = nextInpVal.split("/");
			nextDate = new Date(DateArray[2],DateArray[0],DateArray[1]);
		}
		else
		{
			var todayS = new Date();
      		var yearS = todayS.getFullYear();
      		var monthS = todayS.getMonth()+1;
      		var mdayS = todayS.getDate();
			nextDate = new Date(yearS,monthS,mdayS);
		}
    }
	else
	{
		var nextInpName = inpNameObj.id+"last";
		prevInpObj = inpNameObj;
		nextInpObj = document.getElementById(nextInpName);
		var nextInpVal = nextInpObj.value;
		var prevInpVal = prevInpObj.value;
		var DateTrue = /^(0?\d|1[0-2])\/([0-2]?\d|3[01])\/(19[7-9]\d|20[0-5]\d)$/;
		
		if(DateTrue.test(prevInpVal))
		{
			var DateArray = prevInpVal.split("/");
			prevDate = new Date(DateArray[2],DateArray[0],DateArray[1]);
		}
		else
		{
			var todayS = new Date();
      		var yearS = todayS.getFullYear();
      		var monthS = todayS.getMonth()+1;
      		var mdayS = todayS.getDate();
			prevDate = new Date(yearS,monthS,mdayS);
		}
		if(DateTrue.test(nextInpVal))
		{
			var DateArray = nextInpVal.split("/");
			nextDate = new Date(DateArray[2],DateArray[0],DateArray[1]);
		}
		else
		{
			var todayS = new Date();
      		var yearS = todayS.getFullYear();
      		var monthS = todayS.getMonth()+1;
      		var mdayS = todayS.getDate();
			nextDate = new Date(yearS,monthS,mdayS);
		}
	}
	if (prevDate > nextDate)
	{
		nextInpObj.value = prevInpObj.value;
		reSetDateControl(nextInpObj,nextInpObj.value);
	}
}


function reSetDateControl(InpObj,DateObj)
{
	 var year = DateObj.split("/")[2];
     var month = DateObj.split("/")[0];
	 
     var firstDay=new Date(year,month-1,1);
	 firstDay=firstDay.getDay();
     var lastDay=new Date(year,month,0);
	 lastDay=lastDay.getDate();

     var tab = getTrueNodeArray(getTrueNodeArray(getTrueNodeArray(getTrueNodeArray(InpObj.parentNode.parentNode.childNodes)[1].childNodes)[0].childNodes)[1].childNodes)[0];
	 
	 tab.rows[0].cells[2].innerHTML = "<span>" + year + "</span> <span> - </span> <span>" + month + "</span>";
	 
	 var day = 1;
	 for(var row = 2; row < 8; row++)
	 {
		for(var col = 0; col < 7; col++)
		{
			if(row == 2 && col < firstDay)
			{
				tab.rows[row].cells[col].innerHTML = '&nbsp;';
				tab.rows[row].cells[col].className = '0';
			}
			else if(day <= lastDay)
			{
				tab.rows[row].cells[col].innerHTML = day;
				tab.rows[row].cells[col].className = '1';
				day ++;
			}
			else
			{
				tab.rows[row].cells[col].innerHTML = '';
				tab.rows[row].cells[col].className = '0';
			}
		}
	 }
}


function beginSetControl(InpObjName)
{
	 var InpObj = document.getElementById(InpObjName);
	 var year = InpObj.value.split("/")[2];
     var month = InpObj.value.split("/")[0];
	 
     var firstDay=new Date(year,month-1,1);
	 firstDay=firstDay.getDay();
     var lastDay=new Date(year,month,0);
	 lastDay=lastDay.getDate();

     var tab = getTrueNodeArray(getTrueNodeArray(getTrueNodeArray(getTrueNodeArray(InpObj.parentNode.parentNode.childNodes)[1].childNodes)[0].childNodes)[1].childNodes)[0];
	 
	 tab.rows[0].cells[2].innerHTML = "<span>" + year + "</span> <span> - </span> <span>" + month + "</span>";
	 
	 var day = 1;
	 for(var row = 2; row < 8; row++)
	 {
		for(var col = 0; col < 7; col++)
		{
			if(row == 2 && col < firstDay)
			{
				tab.rows[row].cells[col].innerHTML = '&nbsp;';
				tab.rows[row].cells[col].className = '0';
			}
			else if(day <= lastDay)
			{
				tab.rows[row].cells[col].innerHTML = day;
				tab.rows[row].cells[col].className = '1';
				day ++;
			}
			else
			{
				tab.rows[row].cells[col].innerHTML = '';
				tab.rows[row].cells[col].className = '0';
			}
		}
	 }
	
}



function UncCalendar (sName, sDate, sRange)
{ 
  this.dateSign = sRange || "0";
  this.inputName = sName || "uncDate";
  this.inputValue = sDate || "";
  this.inputSize = 10;
  this.inputClass = "";
  this.color = "#333333";
  this.bgColor = "#EEEEEE";
  this.buttonWidth = 80;
  this.buttonWords = "Select Date";
  this.canEdits = true;
  this.hidesSelects = true;
  /////////////////////////////////////////////////////////////////////////

  /////////////////////////////////////////////////////////////////////////

  this.display = function ()
  {
	var reDate = /^(0?\d|1[0-2])\/([0-2]?\d|3[01])\/(19[7-9]\d|20[0-5]\d)$/;
	
    if (reDate.test(this.inputValue))
    {
      var dates = this.inputValue.split("/");	  
	  var month = parseInt(dates[0], 10);
      var mday = parseInt(dates[1], 10);
      var year = parseInt(dates[2], 10);
    }
    else
    {
      var today = new Date();
      var year = today.getFullYear();
      var month = today.getMonth()+1;
      var mday = today.getDate();
    }
    if (this.inputValue == "today")
	  inputValue = month + "/" + mday + "/" + year;
    else
      inputValue = this.inputValue;
    var lastDay = new Date(year, month, 0);
    lastDay = lastDay.getDate();
    var firstDay = new Date(year, month-1, 1);
    firstDay = firstDay.getDay();
    
    var btnBorder =
      "border-left:1px solid " + this.color + ";" +
      "border-right:1px solid " + this.color + ";" +
      "border-top:1px solid " + this.color + ";" +
      "border-bottom:1px solid " + this.color + ";";
    var btnStyle =
      "padding-top:3px;cursor:default;width:" + this.buttonWidth + "px;text-align:center;height:18px;top:-9px;" +
      "font:normal 12px Arial, Helvetica, sans-serif;position:absolute;z-index:99;background-color:" + this.bgColor + ";" +
      "line-height:12px;" + btnBorder + "color:" + this.color + ";";
    var boardStyle = 
      "position:absolute;width:251px;background-color:" + this.bgColor + ";top:8px;border:1px solid "+
      this.color + ";display:none;padding:3px;";
    var buttonEvent =
      " onmouseover=\"getTrueNodeArray(this.childNodes)[0].style.borderBottom='0px';" + 
          "getTrueNodeArray(this.childNodes)[1].style.display='';this.style.zIndex=100;" +
          (this.hidesSelects ?
          "var slts=document.getElementsByTagName('SELECT');" +
          "for(var i=0;i<slts.length;i++)slts[i].style.visibility='hidden';"
          : "") + "\"" +
		  
      " onmouseout=\"getTrueNodeArray(this.childNodes)[0].style.borderBottom='1px solid " + this.color + "';" +
          "getTrueNodeArray(this.childNodes)[1].style.display='none';this.style.zIndex=99;" +
          (this.hidesSelects ?
          "var slts=document.getElementsByTagName('SELECT');" +
          "for(var i=0;i<slts.length;i++)slts[i].style.visibility='';"
          : "") + "\"" +
      " onselectstart=\"return false;\"";
    var mdayStyle = "font:normal 9px Verdana,Arial, Helvetica, sans-serif;line-height:12px;cursor:default;color:" + this.color;
    var weekStyle = "font:normal 12px Arial, Helvetica, sans-serif;line-height:15px;cursor:default;color:" + this.color;
    var arrowStyle = "font:bold 7px Verdana,Arial, Helvetica, sans-serif;cursor:pointer;line-height:16px;color:" + this.color;
    var ymStyle = "font:bold 12px Arial, Helvetica, sans-serif;line-height:16px;cursor:default;color:" + this.color;
    var changeMdays = 
      "var year=parseInt(getTrueNodeArray(this.parentNode.cells[2].childNodes)[0]."+txtTag+");" +
      "var month=parseInt(getTrueNodeArray(this.parentNode.cells[2].childNodes)[2]."+txtTag+");" +
      "var firstDay=new Date(year,month-1,1);firstDay=firstDay.getDay();" +
      "var lastDay=new Date(year,month,0);lastDay=lastDay.getDate();" +
      "var tab=this.parentNode.parentNode, day=1;" +
      "for(var row=2;row<8;row++)" +
      "  for(var col=0;col<7;col++){" +
      "    if(row==2&&col<firstDay){" +
      "      tab.rows[row].cells[col].innerHTML='&nbsp;';" +
      "      tab.rows[row].cells[col]."+isDayVal+"='0';}" +
      "    else if(day<=lastDay){" +
      "      tab.rows[row].cells[col].innerHTML=day;" +
      "      tab.rows[row].cells[col]."+isDayVal+"='1';day++;}" +
      "    else{" +
      "      tab.rows[row].cells[col].innerHTML='';" +
      "      tab.rows[row].cells[col]."+isDayVal+"='0';}" +
      "  }";
	  
    var pyEvent =
      " onclick=\"var y=getTrueNodeArray(this.parentNode.cells[2].childNodes)[0];y."+txtTag+"=parseInt(y."+txtTag+")-1;" +
                  changeMdays + "\"";
				  
    var pmEvent =
      " onclick=\"var y=getTrueNodeArray(this.parentNode.cells[2].childNodes)[0];m=getTrueNodeArray(this.parentNode.cells[2].childNodes)[2];" +
                 "m."+txtTag+"=parseInt(m."+txtTag+")-1;if(m."+txtTag+"=='0'){m."+txtTag+"=12;y."+txtTag+"=" +
                 "parseInt(y."+txtTag+")-1;}" + changeMdays + "\"";
				 
    var nmEvent =
      " onclick=\"var y=getTrueNodeArray(this.parentNode.cells[2].childNodes)[0];m=getTrueNodeArray(this.parentNode.cells[2].childNodes)[2];" +
                 "m."+txtTag+"=parseInt(m."+txtTag+")+1;if(m."+txtTag+"=='13'){m."+txtTag+"=1;y."+txtTag+"=" +
                 "parseInt(y."+txtTag+")+1;}" + changeMdays + "\"";
				 
    var nyEvent =
      " onclick=\"var y=getTrueNodeArray(this.parentNode.cells[2].childNodes)[0];y."+txtTag+"=parseInt(y."+txtTag+")+1;" +
                  changeMdays + "\"";
				  
    var mdayEvent =
      " onmouseover=\"var event=event||window.event;"+
	  	  "if("+eventTag+".tagName=='TD'&& "+eventTag+"."+isDayVal+"=='1'){" +
          ""+eventTag+".style.backgroundColor='" + this.color + "';" +
          ""+eventTag+".style.color='" + this.bgColor + "';" +
          ""+eventTag+".style.cursor='pointer';" +
          "var ym=getTrueNodeArray("+eventTag+".parentNode.parentNode.rows[0].cells[2].childNodes);" +
		  ""+eventTag+".title=ym[2]."+txtTag+"+'/'+"+eventTag+"."+txtTag+"+'/'+ym[0]."+txtTag+";}\"" +
		  
		  
      " onmouseout=\"var event=event||window.event;" +
	      "setTableStyle(this,'"+this.color+"','"+this.bgColor+"');"+
	  	  "if("+eventTag+".tagName=='TD'&& "+eventTag+"."+isDayVal+"=='1'){" +
          "var ym=getTrueNodeArray("+eventTag+".parentNode.parentNode.rows[0].cells[2].childNodes);" +
		  ""+eventTag+".title=ym[2]."+txtTag+"+'/'+"+eventTag+"."+txtTag+"+'/'+ym[0]."+txtTag+";}\"" +
		  
		  
      " onclick=\"var event=event||window.event;"+
	      "if("+eventTag+".tagName=='TD'&& "+eventTag+"."+isDayVal+"=='1'){" +
          "var inp=getTrueNodeArray(getTruePreviousSibling(this.parentNode.parentNode.parentNode).childNodes)[0];" +
		  "inp.value=getTrueNodeArray(this.rows[0].cells[2].childNodes)[2]."+txtTag+"+'/'+"+eventTag+"."+txtTag+"+'/'+getTrueNodeArray(this.rows[0].cells[2].childNodes)[0]."+txtTag+";" +
		  
		  ((this.dateSign =='1')?"compareSetTxt(inp);":"") +
		  
          "this.parentNode.style.display='none';this.parentNode.parentNode.style.zIndex=99;" +
          "getTruePreviousSibling(this.parentNode).style.borderBottom='1px solid " + this.color + "';" +
          (this.hidesSelects ?
          "var slts=document.getElementsByTagName('SELECT');" +
          "for(var i=0;i<slts.length;i++)slts[i].style.visibility='';"
          : "") + "}\"";

    var output = "";
    output += "<table cellpadding=0 cellspacing=1 style='display:inline;'><tr>";
    output += "  <td><input size=" + this.inputSize + " maxlength=10 value=\"" + inputValue + "\"";
    output +=    (this.canEdits ? "" : " readonly") + " name=\"" + this.inputName + "\" id=\"" + this.inputName + "\"></td>";
    output += "  <td width=" + this.buttonWidth + ">";
    output += "    <div style=\"position:absolute;overflow:visible;width:0px;height:0px;\"" + buttonEvent + ">";
    output += "      <div style=\"" + btnStyle + "\"><nobr>" + ((this.dateSign=='1')?"Start Time":this.buttonWords) + "</nobr></div>";
    output += "      <div style=\"" + boardStyle + "\">";
    output += "        <table cellspacing=1 cellpadding=1 width=250" + mdayEvent + ">";
    output += "          <tr height=20 align=center>";
    output += "            <td style=\"" + arrowStyle + "\" title=\"Prev. Year\"" + pyEvent + ">&lt;&lt;</td>";
    output += "            <td style=\"" + arrowStyle + "\" align=left title=\"Prev. Month\"" + pmEvent + ">&lt;</td>";
    output += "            <td colspan=3 style=\"" + ymStyle + "\" valign=bottom>";
    output += "              <span>" + year + "</span> <span> - </span> <span>" + month + "</span>";
    output += "            </td>";
    output += "            <td style=\"" + arrowStyle + "\" align=right title=\"Next. Month\"" + nmEvent + ">&gt;</td>";
    output += "            <td style=\"" + arrowStyle + "\" title=\"Next. Year\"" + nyEvent + ">&gt;&gt;</td>";
    output += "          </tr>";
    output += "          <tr height=20 align=center bgcolor=" + this.bgColor + ">";
    output += "            <td width=14% style=\"" + weekStyle + "\">Sun</td>";
    output += "            <td width=14% style=\"" + weekStyle + "\">Mon</td>";
    output += "            <td width=14% style=\"" + weekStyle + "\">Tue</td>";
    output += "            <td width=14% style=\"" + weekStyle + "\">Wed</td>";
    output += "            <td width=14% style=\"" + weekStyle + "\">Thu</td>";
    output += "            <td width=14% style=\"" + weekStyle + "\">Fri</td>";
    output += "            <td width=14% style=\"" + weekStyle + "\">Sat</td>";
    output += "          </tr>";
    var day = 1;
    for (var row=0; row<6; row++)
    {
      output += "<tr align=center>";
      for (var col=0; col<7; col++)
      {
        if (row == 0 && col < firstDay)
          output += "<td style=\"" + mdayStyle + "\">&nbsp;</td>";
        else if (day <= lastDay)
        {
          output += "<td style=\"" + mdayStyle + "\" "+isDay+"=\"1\">" + day + "</td>";
          day++;
        }
        else
          output += "<td style=\"" + mdayStyle + "\"></td>";
      }
      output += "</tr>";
    }
    output += "        </table>";
    output += "      </div>";
    output += "    </div>";
    output += "  </td>";
    output += "</tr></table>";
	
	if (this.dateSign == '1')
	{
		output += "&nbsp;&nbsp;&nbsp;&nbsp;";
		output += "<table cellpadding=0 cellspacing=1 style='display:inline;'><tr>";
    	output += "  <td><input size=" + this.inputSize + " maxlength=10 value=\"" + inputValue + "\"";
    	output +=    (this.canEdits ? "" : " readonly") + " name=\"" + this.inputName + "last \" id=\"" + this.inputName + "last\"></td>";
    	output += "  <td width=" + this.buttonWidth + ">";
		output += "    <div style=\"position:absolute;overflow:visible;width:0px;height:0px;\"" + buttonEvent + ">";
		output += "      <div style=\"" + btnStyle + "\"><nobr> End Time </nobr></div>";
		output += "      <div style=\"" + boardStyle + "\">";
		output += "        <table cellspacing=1 cellpadding=1 width=250" + mdayEvent + ">";
		output += "          <tr height=20 align=center>";
		output += "            <td style=\"" + arrowStyle + "\" title=\"Prev. Year\"" + pyEvent + ">&lt;&lt;</td>";
		output += "            <td style=\"" + arrowStyle + "\" align=left title=\"Prev. Month\"" + pmEvent + ">&lt;</td>";
		output += "            <td colspan=3 style=\"" + ymStyle + "\" valign=bottom>";
		output += "              <span>" + year + "</span> <span> - </span> <span>" + month + "</span>";
		output += "            </td>";
		output += "            <td style=\"" + arrowStyle + "\" align=right title=\"Next. Month\"" + nmEvent + ">&gt;</td>";
		output += "            <td style=\"" + arrowStyle + "\" title=\"Next. Year\"" + nyEvent + ">&gt;&gt;</td>";
		output += "          </tr>";
		output += "          <tr height=20 align=center bgcolor=" + this.bgColor + ">";
		output += "            <td width=14% style=\"" + weekStyle + "\">Sun</td>";
		output += "            <td width=14% style=\"" + weekStyle + "\">Mon</td>";
		output += "            <td width=14% style=\"" + weekStyle + "\">Tue</td>";
		output += "            <td width=14% style=\"" + weekStyle + "\">Wed</td>";
		output += "            <td width=14% style=\"" + weekStyle + "\">Thu</td>";
		output += "            <td width=14% style=\"" + weekStyle + "\">Fri</td>";
		output += "            <td width=14% style=\"" + weekStyle + "\">Sat</td>";
		output += "          </tr>";
		var day = 1;
		for (var row=0; row<6; row++)
		{
		  output += "<tr align=center>";
		  for (var col=0; col<7; col++)
		  {
			if (row == 0 && col < firstDay)
			  output += "<td style=\"" + mdayStyle + "\">&nbsp;</td>";
			else if (day <= lastDay)
			{
			  output += "<td style=\"" + mdayStyle + "\" "+isDay+"=\"1\">" + day + "</td>";
			  day++;
			}
			else
			  output += "<td style=\"" + mdayStyle + "\"></td>";
		  }
		  output += "</tr>";
		}
		output += "        </table>";
		output += "      </div>";
		output += "    </div>";
		output += "  </td>";
		output += "</tr></table>";
	}
    document.write(output);
  }
  /////////////////////////////////////////////////////////////////////////
}