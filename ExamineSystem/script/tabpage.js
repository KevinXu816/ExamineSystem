    var oTabPage = new Object();

    var TabPage = function(_container) {
        this.author = '51JS.COM-ZMM';
        this.version = 'TabPage 1.0';
        this.container = _container;
        this.barHeight = 22;
        this.put_barHeight = function(_height) { return _height; };
        this.get_barHeight = function() { return this.barHeight; }; 
        this.guideWidth = 82;
        this.put_guideWidth = function(_width) { return _width; };
        this.get_guideWidth = function() { return this.guideWidth; }; 
    }
    
    TabPage.prototype = {
        boxInit : function() {
            this.box = (function(_object) {
                 var _box = document.createElement('DIV');
                 with (_box) {
                       style.width = _object.offsetWidth;
                       style.height = _object.offsetHeight;
                       style.padding = '0px';   
                 }
                 return _box;
            })(this.container);
            this.box.appendChild(this.bar = (function(_height) {
                 var _bar = document.createElement('DIV');
                 with (_bar) {
                       align = 'left';
                       style.width = '90%';
                       style.height = _height;
                       style.padding = '0px';
                       attachEvent('oncontextmenu', new Function('return false'));
                       attachEvent('onselectstart', new Function('return false'));               
                 }
                 return _bar;
            })(this.barHeight));
            this.box.appendChild(this.page = (function(_height) {
                 var _page = document.createElement('DIV');
                 with (_page) {
                       style.width = '90%';
                       style.height = _height;
                       style.fontSize = '12px';
                       style.textAlign = 'left';           
                       style.padding = '5px';
                       style.borderLeft = '1px solid white';
                       style.borderRight = '2px outset';
                       style.borderBottom = '2px outset';
                       style.backgroundColor = '#e6ecf9';
                 }
                 return _page;             
            })(parseInt(this.box.style.height) - this.barHeight));     
            this.container.appendChild(this.box);
        },
    
        addTabPage : function(_guides, _contents) {
            this.guides = [];
            this.contents = [];
            for (var i = 0; i < _contents.length; i ++) {
                 var _page = document.createElement('DIV');
                 with (_page) {
                       style.display = (i == 0) ? 'inline' : 'none' ;
                       innerHTML = _contents[i].innerHTML;
                       _contents[i].removeNode(true);
                 }
                 this.page.appendChild(_page);
                 this.contents[this.contents.length] = _page;
            }
            this.bar.appendChild((function(_object) {
                 var _table = document.createElement('TABLE');
                 with (_table) {
                       width = '90%';
                       cellSpacing = 0;
                       cellPadding = 0;
                       style.tableLayout = 'fixed';
                 }
                 var _tbody = document.createElement('TBODY');
                 var _tr = document.createElement('TR');
                 for (var i = 0; i < _guides.length; i ++) {
                      var _td = document.createElement('TD');
                      with (_td) {
                            vAlign = 'bottom';
                            style.width = _object.guideWidth;
                            if (i != 0) style.borderBottom = '1px solid white';
                            appendChild (_object.guides[_object.guides.length] = (function(n) {
                                var _guide = document.createElement('SPAN');
                                with (_guide) {
                                      style.width = _object.guideWidth - 2;
                                      style.fontSize = '12px';
                                      style.textAlign = 'center';
                                      style.cursor = 'default';
                                      style.paddingTop = '3px';
                                      style.borderLeft = '1px solid white';
                                      style.borderTop = '1px solid white';
                                      style.backgroundColor = '#e6ecf9';
                                      if (i == 0) {
                                          style.height = _object.barHeight;
                                          _contents[i].style.display = 'inline';
                                      } else {
                                          style.height = _object.barHeight - 4;
                                      }
                                      innerText = _guides[i];
                                      attachEvent('onselectstart', new Function('return false;'));
                                      attachEvent('onmousedown', function() {                                      
                                          with (oTabPage) {
                                                for (var j = 0; j < guides.length; j ++) {
                                                     if (guides[j] == event.srcElement) {
                                                         guides[j].style.height = _object.barHeight; 
                                                         guides[j].nextSibling.style.height = _object.barHeight;
                                                         guides[j].parentNode.style.borderBottom = '0px';
                                                         contents[j].style.display = 'inline';
                                                     } else {
                                                         guides[j].style.height = _object.barHeight - 4; 
                                                         guides[j].nextSibling.style.height = _object.barHeight - 4;
                                                         guides[j].parentNode.style.borderBottom = '1px solid white';
                                                         contents[j].style.display = 'none';
                                                     }
                                                }
                                          }
                                      });
                                }
                                return _guide;
                            })(i));                        
                      }
                      _td.appendChild((function(o, n) {
                          var _line = document.createElement('SPAN');
                          with (_line) {
                                style.width = '2px';
                                style.height = (n == 0) ? o.barHeight : o.barHeight - 4 ;
                                style.borderLeft = '1px solid dimgray';
                                style.borderRight = '1px solid black';
                          }
                          return _line;
                      })(_object, i));
                      _tr.appendChild(_td);
                 }
                 _tr.appendChild((function() {
                      var _surplus = document.createElement('TD');
                      with (_surplus) {
                            style.width = '90%';
                            style.borderBottom = '1px solid white';
                            innerHTML = '&nbsp;';
                      }
                      return _surplus;
                 })());             
                 _tbody.appendChild(_tr);
                 _table.appendChild(_tbody); 
                 return _table;           
            })(this));
        }
    }
        
    attachEvent('onload', function() {
        var _guideArr = ['听力分析', '单项选择', '多项选择', '对错判断', '阅读理解'];
        var _contentArr = [Content1, Content2, Content3, Content4, Content5];
        oTabPage = new TabPage(self.oContainer);
        oTabPage.boxInit();
        oTabPage.addTabPage(_guideArr, _contentArr);
    });