
var BeginCode='';
BeginCode +='<div id="exp" style="position:absolute; left:300px; top:-62px; width:200px; height:60px; z-index:999; border: 1px solid #000000;">';
BeginCode +='<table border="0" cellspacing="0" cellpadding="0" width="100%" height="100%" bgcolor="#99CCFF">';
BeginCode +='<tr><td height="100%" width="100%" align="center" id="Warn_txt"></td></tr>';
BeginCode +='<tr><td nowrap height="1" id="expfunc"></td></tr>';
BeginCode +='<tr><td nowrap height="1" id="expmove"></td></tr>';
BeginCode +='</table>';
BeginCode +='</div>';

document.write(BeginCode);




//移动层对象构造函数//////////////////////

function divMoveClass(namestr,objn,clioo,moveoo,direstr,ifdrag,su1,su2)  
// namestr: 实例名称  objn: 伸缩层  clioo: 控制伸缩  moveoo: 拖拉层  direstr: 方向标示(left/right/up/down)  ifdragstrs: 是否允许拖拽(false/true)
{
  this.obj = document.getElementById(objn);
  this.co = document.getElementById(clioo); 
  this.mo = document.getElementById(moveoo); 
  this.fx = -1;
  this.sx = 0;
  this.mdt = null;
  this.moing = false;
  this.funcname=namestr;
  this.bs = su1;			//层每次移动的象素
  this.sd = su2;			//层移动的速度
  this.padd = (direstr=="left" || direstr=="right")?(this.co.offsetWidth+this.mo.offsetWidth+1):(this.co.offsetHeight+this.mo.offsetHeight-2);		//层关闭时预留尺寸  
  this.dtMax = 588;		//拖动最大宽度
  this.dtMin = 300;		//拖动最小宽度
  
  this.statustr="in";  //ining 正在向里走 outing正在向外走  in 里面  out 外面

  if (ifdrag) 
  {
  	this.mo.style.cursor=(direstr=="left" || direstr=="right")?("e-resize"):("n-resize");
	this.mo.onmousedown = function(e) { eval(namestr+".hd_down(e)") };
  }
  this.mo.ondragstart = function() { return false };
  this.mo.onselectstart = function() { return false };
  
  switch(direstr)
  {
	case "up":
		this.tmpcs= 0-this.obj.offsetHeight+this.padd; 
		this.divMoveing = divMoveingUp;
		this.hd_move = hd_moveup;
		this.hd_down = hd_downup;
	break;
	case "down":
		this.tmpcs= document.body.clientHeight-this.padd;
		this.divMoveing = divMoveingDown;
		this.hd_move = hd_movedown;
		this.hd_down = hd_downdown;
	break;
	case "left":
		this.tmpcs= 0-this.obj.offsetWidth+this.padd;
		this.divMoveing = divMoveingLeft;
		this.hd_move = hd_moveleft;
		this.hd_down = hd_downleft;
	break;
	case "right":
		this.tmpcs= document.body.clientWidth-this.padd;
		this.divMoveing = divMoveingRight;
		this.hd_move = hd_moveright;
		this.hd_down = hd_downright;
	break;
  }
  
  this.doDivMove = function ()
  {
    clearTimeout(this.mdt);
    this.fx = this.fx*(-1);
    this.divMoveing(this.fx)
  };

  this.hd_up = function (e)
  {
    if(!this.moing) return;
    var boo = e?document:this.mo;
    boo.onmouseup = null;
    boo.onmousemove = null;
    try {
      this.mo.releaseCapture();
    } catch(hh){}
    this.moing = false;
  };


}



///////////////////////////////////////////////////

function divMoveingLeft(f)
{
  var divw = 0-this.obj.offsetWidth+this.padd;
  var wx = this.obj.offsetLeft + f * this.bs;
  var jx = true;
  var ttmp=0;
 
  if(f==1)
  {
    if(wx>=ttmp) { wx=ttmp; jx=false; this.statustr="out"; }
  }
  else
  {
    if(wx<=divw) { wx=divw; jx=false; this.statustr="in"; }
  }
  
  if(this.tmpcs<wx)
  {
  	if(wx!=ttmp)
  	{
		this.statustr="outing";
	}
	this.tmpcs=wx;
  }
  if(this.tmpcs>wx)
  {
  	if(wx!=divw)
  	{
		this.statustr="ining";
	}
	this.tmpcs=wx;
  }
    
  this.obj.style.left = wx+"px";
  if(jx)
    this.mdt = setTimeout(this.funcname+".divMoveing("+f+")",this.sd); 
}

function divMoveingRight(f)
{
  var divw = document.body.clientWidth-this.padd;
  var wx = this.obj.offsetLeft - f * this.bs;
  var jx = true;
  var ttmp=document.body.clientWidth-this.obj.offsetWidth;

  if(f==1)
  {
    if(wx<=ttmp) { wx=ttmp; jx=false; this.statustr="out"; }
  }
  else
  {
    if(wx>=divw) { wx=divw; jx=false; this.statustr="in"; }
  }
      
  if(this.tmpcs>wx)
  {
  	if(wx!=ttmp)
  	{
		this.statustr="outing";
	}
	this.tmpcs=wx;
  }
  if(this.tmpcs<wx)
  {
  	if(wx!=divw)
  	{
		this.statustr="ining";
	}
	this.tmpcs=wx;
  }
    
  this.obj.style.left = wx+"px";
  if(jx)
    this.mdt = setTimeout(this.funcname+".divMoveing("+f+")",this.sd); 
}

function divMoveingUp(f)
{
  var divw = 0-this.obj.offsetHeight+this.padd;
  var wx = this.obj.offsetTop + f * this.bs;
  var jx = true;
  var ttmp=0;
  
  if(f==1)
  {
    if(wx>=ttmp) { wx=ttmp; jx=false; this.statustr="out"; }
  }
  else
  {
    if(wx<=divw) { wx=divw; jx=false; this.statustr="in"; }
  }
  
  if(this.tmpcs<wx)
  {
  	if(wx!=ttmp)
  	{
		this.statustr="outing";
	}
	this.tmpcs=wx;
  }
  if(this.tmpcs>wx)
  {
  	if(wx!=divw)
  	{
		this.statustr="ining";
	}
	this.tmpcs=wx;
  }

  
  this.obj.style.top = wx+"px";
  if(jx)
    this.mdt = setTimeout(this.funcname+".divMoveing("+f+")",this.sd); 
}

function divMoveingDown(f)
{
  var divw = document.body.clientHeight-this.padd ;
  var wx = this.obj.offsetTop - f * this.bs; 
  var jx = true;
  var ttmp=document.body.clientHeight-this.obj.offsetHeight;
  
  if(f==1)
  {
    if(wx<=ttmp) { wx=ttmp; jx=false; this.statustr="out"; }
  }
  else
  {
    if(wx>=divw) { wx=divw; jx=false; this.statustr="in"; }
  }
  
  if(this.tmpcs>wx)
  {
  	if(wx!=ttmp)
  	{
		this.statustr="outing";
	}
	this.tmpcs=wx;
  }
  if(this.tmpcs<wx)
  {
  	if(wx!=divw)
  	{
		this.statustr="ining";
	}
	this.tmpcs=wx;
  }
  
  this.obj.style.top = wx+"px";
  if(jx)
    this.mdt = setTimeout(this.funcname+".divMoveing("+f+")",this.sd); 
}


function hd_downleft(e)
{
  if(this.obj.offsetLeft<0) return;
  if(this.moing) return;
  var ex = e?e.pageX:event.clientX;
  this.sx = this.obj.offsetWidth - ex;
  var boo = e?document:this.mo;
  var fnameleft=this.funcname;
  boo.onmouseup = function(e) { eval(fnameleft+".hd_up(e)") };
  boo.onmousemove = function(e) { eval(fnameleft+".hd_move(e)") };
  try {
    this.mo.setCapture(); 
  } catch(hh){}
  this.moing = true;
}

function hd_downright(e)
{
  if(this.obj.offsetLeft>(document.body.clientWidth-this.obj.offsetWidth)) return;
  if(this.moing) return;
  var ex = e?e.pageX:event.clientX;
  this.sx = this.obj.offsetWidth - (document.body.clientWidth-ex);
  var boo = e?document:this.mo;
  var fnameright=this.funcname;
  boo.onmouseup = function(e) { eval(fnameright+".hd_up(e)") };
  boo.onmousemove = function(e) { eval(fnameright+".hd_move(e)") };
  try {
    this.mo.setCapture(); 
  } catch(hh){}
  this.moing = true;
}

function hd_downup(e)
{
  if(this.obj.offsetTop<0) return;
  if(this.moing) return;
  var ex = e?e.pageY:event.clientY;
  this.sx = this.obj.offsetHeight-ex;
  var boo = e?document:this.mo;
  var fnameup=this.funcname;
  boo.onmouseup = function(e) { eval(fnameup+".hd_up(e)") };
  boo.onmousemove = function(e) { eval(fnameup+".hd_move(e)") }; 
  try {
    this.mo.setCapture(); 
  } catch(hh){}
  this.moing = true;
}

function hd_downdown(e)
{
  if(this.obj.offsetTop>(document.body.clientHeight-this.obj.offsetHeight)) return;
  if(this.moing) return;
  var ex = e?e.pageY:event.clientY;
  this.sx = this.obj.offsetHeight - (document.body.clientHeight-ex);  
  var boo = e?document:this.mo;
  var fnamedown=this.funcname;
  boo.onmouseup = function(e) { eval(fnamedown+".hd_up(e)") };
  boo.onmousemove = function(e) { eval(fnamedown+".hd_move(e)") };
  try {
    this.mo.setCapture(); 
  } catch(hh){}
  this.moing = true;
}

function hd_moveleft(e)
{
  if(!this.moing) return;
  var ex = e?e.pageX:event.clientX;
  var gcx = ex+this.sx;
  if(gcx<this.dtMin)
    gcx = this.dtMin;
  if(gcx>this.dtMax)
    gcx = this.dtMax;
  this.obj.style.width = gcx+"px";
}

function hd_moveright(e)
{
  if(!this.moing) return;
  var ex = e?e.pageX:event.clientX;
  var gcx = ex+this.sx;
  if ((document.body.clientWidth-gcx)<this.dtMin)
    gcx = document.body.clientWidth-this.dtMin; 
  if ((document.body.clientWidth-gcx)>this.dtMax) 
    gcx = document.body.clientWidth-this.dtMax; 	
 this.obj.style.width = (document.body.clientWidth-gcx)+"px"; 
 this.obj.style.left = gcx+"px";
}

function hd_moveup(e)
{
  if(!this.moing) return;
  var ex = e?e.pageY:event.clientY;
  var gcx = ex+this.sx;
  if(gcx<this.dtMin)
    gcx = this.dtMin;
  if(gcx>this.dtMax)
    gcx = this.dtMax;
  this.obj.style.height = gcx+"px";
}

function hd_movedown(e)
{
  if(!this.moing) return;
  var ex = e?e.pageY:event.clientY;
  var gcx = ex+this.sx;
  if ((document.body.clientHeight-gcx)<this.dtMin)
    gcx = document.body.clientHeight-this.dtMin;
  if ((document.body.clientHeight-gcx)>this.dtMax)
    gcx = document.body.clientHeight-this.dtMax;
 this.obj.style.height = (document.body.clientHeight-gcx)+"px";
 this.obj.style.top = gcx+"px";
}




//警告提示框动态提示函数/////////////////////////////////////
var warndiv = new divMoveClass("warndiv","exp","expfunc","expmove","up",false,3,12);
var ipfunc;
function warn_Win(txt,sign)
{
	hhbackpage.left.document.getElementById("InfoSpan").innerHTML=txt;
	document.getElementById("Warn_txt").innerHTML=txt;
	clearTimeout(ipfunc);
	switch(warndiv.statustr)
	{
		case "ining":
			warndiv.doDivMove();
		break;
		case "outing":
			
		break;
		case "in":
			warndiv.doDivMove();
		break;
		case "out":
			
		break;
	}
	if (sign != null && sign != 0) ipfunc = setTimeout("warndiv.doDivMove()",6000);
}


function controlPos()
{
	document.getElementById("exp").style.left = ((document.body.clientWidth/2)-30)+"px";
}

controlPos();
