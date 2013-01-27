function SeleMultObj(SeleObj,MainItem,SlaveItem,Plan,WordTxt,StyleSize,StyleWidth)
{

	var SeleObjHTML = '';
	if (Plan == null || Plan == "0"){
		SeleObjHTML += '<table border="0" cellspacing="0" cellpDoAdding="0">';
		SeleObjHTML += '<tr>';
    	SeleObjHTML += '<td>被选'+((WordTxt==null)?'权限':WordTxt)+'：<br>';
		SeleObjHTML += '<select id="'+ SeleObj +'" name="'+ SeleObj +'" size="'+((StyleSize==null)?'6':StyleSize)+'" multiple style="width: '+((StyleWidth==null)?'100':StyleWidth)+'px" ondblclick ="'+ SeleObj +'.DoAdd();">';
    	SeleObjHTML += '</select>';
		SeleObjHTML += '</td>';
		SeleObjHTML += '<td width="28" align="center">';
		SeleObjHTML += '<input type="button" value=">>" onClick="'+ SeleObj +'.DoAdd();" class="ip2"><br><br>';
		SeleObjHTML += '<input type="button" value="<<" onClick="'+ SeleObj +'.DoDel();" class="ip2">';
		SeleObjHTML += '</td>';
    	SeleObjHTML += '<td>选中'+((WordTxt==null)?'权限':WordTxt)+'：<br>';
		SeleObjHTML += '<select id="'+ SeleObj +'Last" name="'+ SeleObj +'Last" size="'+((StyleSize==null)?'6':StyleSize)+'" multiple style="width: '+((StyleWidth==null)?'100':StyleWidth)+'px" ondblclick ="'+ SeleObj +'.DoDel();">';
		SeleObjHTML += '</select>';
		SeleObjHTML += '</td>';
  		SeleObjHTML += '</tr>';
		SeleObjHTML += '</table>';
		
		document.write(SeleObjHTML);
	}

	this.MainSel  = document.getElementById(SeleObj);
	this.SlaveSel = document.getElementById(SeleObj+"Last");
	this.Item_org = new Array();
	
	
	this.DoTestFunc = function(){
		try{
			if (this.SlaveSel.options.length>-1){
				return true;
			}
			else{
				return true;
			}
		}
		catch(e){
			return false;	
		}
	};
	
	this.SetMainItem = function(ItemArray){
		for (var i=0; i<ItemArray.length; i++){
			this.MainSel.options.add(new Option(ItemArray[i][0],ItemArray[i][1]));
		}
	};
	
	this.SetSlaveItem = function(ItemArray){
		for (var i=0; i<ItemArray.length; i++){
			this.SlaveSel.options.add(new Option(ItemArray[i][0],ItemArray[i][1]));
		}
	};
	
	this.ClearItem = function(){
		for (var i=0; i<this.MainSel.options.length; i++){
			this.MainSel.options.remove(i);
			i--;
		}
		for (var i=0; i<this.SlaveSel.options.length; i++){
			this.SlaveSel.options.remove(i);
			i--;
		}
	};
	
	this.ReSetItem = function(){
		for (var i=0; i<this.MainSel.options.length; i++){
			var TmpOption = this.MainSel.options[i];
			if (!this.HavItemInItem_Org(TmpOption)){
				this.SlaveSel.appendChild(TmpOption);
				i--;
			}
		}
		
		for (var i=0; i<this.SlaveSel.options.length; i++){
			var TmpOption = this.SlaveSel.options[i];
			if (this.HavItemInItem_Org(TmpOption)){
				this.MainSel.appendChild(TmpOption);
				i--;
			}
		}
		this.sort_Main(this.MainSel);
		this.sort_Main(this.SlaveSel);
	};
	
	
	this.HavItemInItem_Org = function(OptionObj){
		for (var i=0; i<this.Item_org.length; i++){
			if(OptionObj.value==this.Item_org[i][0] && OptionObj.text==this.Item_org[i][1]){
				return true;
			}
		}
		return false;
	};
	
	
	if (MainItem != null) this.SetMainItem(MainItem);
	if (SlaveItem != null) this.SetSlaveItem(SlaveItem);
	
	this.DoAdd = function(){
		var this_sel = null;
		for(var i=0;i<this.MainSel.options.length;i++){
			this_sel = this.MainSel.options[i];
			if(this_sel.selected){
				this.SlaveSel.appendChild(this_sel);
				i--;
			}
		}
		this.sort_Main(this.SlaveSel);
	};

	
	this.DoDel = function(){
		var this_sel = null;
		for(var i=0;i<this.SlaveSel.options.length;i++){
			this_sel = this.SlaveSel.options[i];
			if(this_sel.selected){
				this.MainSel.appendChild(this_sel);
				i--;
			}
		}
		this.sort_Main(this.MainSel);
	};
	
	this.sort_Main = function(the_Sel){
		var this_sel = null;
		for(var i=0;i<this.Item_org.length;i++){
			for(var j=0;j<the_Sel.options.length;j++){
				this_sel = the_Sel.options[j];
				if(this_sel.value==this.Item_org[i][0] && this_sel.text==this.Item_org[i][1]){
					the_Sel.appendChild(this_sel);
				}
			}
		}
	};
	
	
	var M_this_sel = null;
	for(var i=0;i<this.MainSel.options.length;i++) {
		M_this_sel = this.MainSel.options[i];
		this.Item_org.push(new Array(M_this_sel.value,M_this_sel.text));
	}
	
}
