(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([[5],{EWZL:function(e,a,t){"use strict";t.r(a);var i=t("k1fw"),r=(t("2qtc"),t("kLXV")),n=t("0Owb"),s=t("fWQN"),o=t("mtLc"),l=t("yKVA"),c=t("879j"),d=(t("+L6B"),t("2/Rp")),h=(t("7Kak"),t("9yH6")),u=t("tJVT"),m=t("q1tI"),v=t.n(m),p=t("Mn5C"),f=t("BuAN"),E=t("Ty5D"),_=t("9kvl"),g=t("f7Sm"),y=t.n(g),b=(t("5NDa"),t("5rEg")),N=(t("y8nQ"),t("Vl3Y")),S=t("YB8P"),k=t("Jgw9"),C=t("kSOO"),O=t.n(C),F=t("4i/J"),M=t("XkFa"),I=N["a"].create()((function(e){var a=e.intl,t=e.onCancel,i=e.onCreate,r=e.currentPath,n=e.hasSameFolder,s=void 0!==n&&n,o=e.onClearCreateError,l=e.form,c=Object(m["useState"])(!1),h=Object(u["a"])(c,2),p=h[0],E=h[1],_=function(e){e.preventDefault(),l.validateFields((function(e,a){if(!e){var t=a.name,r=a.currentPath;i&&i(t,r)}}))},g=l.getFieldDecorator;return v.a.createElement(v.a.Fragment,null,v.a.createElement(f["a"],{title:a.formatMessage({id:"UI_HEADER_NEWFOLDER"}),canBack:!0,onBack:t}),v.a.createElement(N["a"],{form:l,onSubmit:_},v.a.createElement(N["a"].Item,{style:{display:"none"}},g("currentPath",{initialValue:r})(v.a.createElement(b["a"],{type:"hidden"}))),v.a.createElement(N["a"].Item,{label:a.formatMessage({id:"UI_FORM_FOLDERNAME"}),validateStatus:s?"error":void 0,help:s?a.formatMessage({id:"UI_FORM_FOLDERNAMEEXIST"}):null},g("name",{rules:[{max:255,type:"string",message:a.formatMessage({id:"UI_FORM_FOLDERNAMEMAX255"})},{validator:function(e,t,i){/[\\/\\:\\*\\?\\"\\<\\>\\|\\]/g.test(t)&&i(a.formatMessage({id:"UI_FORM_FOLDERNAMEREGERROR"})),i()}}]})(v.a.createElement(b["a"],{placeholder:a.formatMessage({id:"UI_FORM_FOLDERNAMEPLACEHOLDER"}),onChange:function(e){var a=""!=e.target.value.trim();E(a),o&&o()}}))),v.a.createElement(N["a"].Item,{style:{margin:"0px",position:"absolute",bottom:"10px",width:"100%"}},v.a.createElement(d["a"],{htmlType:"submit",type:"primary",disabled:!p},a.formatMessage({id:"UI_BUTTON_CONFIRM"})),v.a.createElement(d["a"],{onClick:t,style:{marginLeft:"8px"}},a.formatMessage({id:"UI_BUTTON_CANCEL"})))))})),R=function(e){Object(l["a"])(t,e);var a=Object(c["a"])(t);function t(e){var i,r;return Object(s["a"])(this,t),r=a.call(this,e),r.state={fileName:null===(i=e.footer)||void 0===i?void 0:i.fileName},r}return Object(o["a"])(t,[{key:"render",value:function(){var e=this,a=this.props,t=a.breadcrumbsProps,i=a.fileExplorerProps,r=a.intl,s=a.footer,o=a.onCreate,l=a.results,c=a.opened,h=void 0!==c&&c,u=a.files,m=a.loading,E=void 0!==m&&m,_=a.showCreateForm,g=void 0!==_&&_,y=a.onCreateFormVisibleChange,N=a.onClearCreateError,C=a.hasSameFolder,R=void 0!==C&&C,T=s||{},U=T.visible,A=void 0!==U&&U,L=T.onCancel,D=this.state,w=D.fileName,B=D.searchStarted,P=void 0!==B&&B,H=D.searchText,j=A;P&&(j=h);var V=void 0,x=t||{},z=x.paths;return z&&z.length>0&&(V=z[z.length-1].id),v.a.createElement("div",{className:O.a.container},g?v.a.createElement(I,{intl:r,currentPath:V,onCreate:o,onClearCreateError:N,onCancel:function(){return y&&y(!1)},hasSameFolder:R}):v.a.createElement(v.a.Fragment,null,v.a.createElement(f["a"],{title:r.formatMessage({id:"UI_HEADER_SELECTSAVEPATH"}),canBack:P,onBack:this.back.bind(this)}),h?v.a.createElement(p["a"],{loading:E},v.a.createElement(k["a"],{emptyType:2,intl:r,files:u,height:"calc(100vh - 144px)",displayMenu:!1,onOpen:this.openSearchResult.bind(this)})):v.a.createElement(v.a.Fragment,null,v.a.createElement(F["a"],{className:O.a.search,placeholder:r.formatMessage({id:"UI_SEARCH_PLACEHOLDER"}),onFocus:function(){return e.setState({searchStarted:!0})},value:H,onSearch:this.search.bind(this),onClear:this.clear.bind(this)}),P?v.a.createElement(v.a.Fragment,null,v.a.createElement(M["a"],{displayMenu:!1,onOpen:this.openSearchResult.bind(this),results:l})):v.a.createElement(v.a.Fragment,null,v.a.createElement(S["a"],t),v.a.createElement(k["a"],Object(n["a"])({},i,{height:"calc(100vh - 225px)"})))),j&&v.a.createElement("div",{className:O.a.footer},v.a.createElement("div",{className:O.a.inputContent},v.a.createElement("span",{className:O.a.label},r.formatMessage({id:"UI_FORM_FOLDER"}),":"),v.a.createElement(b["a"],{className:O.a.input,value:w,onChange:function(a){return e.setState({fileName:a.target.value})}})),v.a.createElement("div",{className:O.a.buttons},v.a.createElement(d["a"],{className:O.a.button,type:"primary",disabled:!w||""===w.trim(),onClick:this.save.bind(this)},r.formatMessage({id:"UI_BUTTON_SAVE"})),v.a.createElement(d["a"],{className:O.a.button,onClick:L},r.formatMessage({id:"UI_BUTTON_CANCEL"})),v.a.createElement(d["a"],{className:O.a.button,disabled:!A||P,type:"link",onClick:function(){return y&&y(!0)}},r.formatMessage({id:"UI_HEADER_NEWFOLDER"}))))))}},{key:"save",value:function(){var e=this.props,a=e.footer,t=e.pathHistory,i=e.intl,r=a||{},n=r.onSave,s=void 0;t&&t.length>0&&(s=t[t.length-1]);var o=this.state.fileName;if(n&&o){var l,c;if(o.length>255)return void anyshare.system.notice(i.formatMessage({id:"UI_FORM_FILENAMEMAX255"}));if(/[\\/\\:\\*\\?\\"\\<\\>\\|\\]/g.test(o))return void anyshare.system.notice(i.formatMessage({id:"UI_FORM_FILENAMEREGERROR"}));n(o,null===(l=s)||void 0===l?void 0:l.id,null===(c=s)||void 0===c?void 0:c.name)}}},{key:"search",value:function(e){this.setState({searchText:e});var a=this.props.onSearch;a&&a(e)}},{key:"openSearchResult",value:function(e,a){var t=this.props.onOpenSearchResult;t&&t(e,a)}},{key:"clear",value:function(){this.setState({searchText:""});var e=this.props.onClear;e&&e()}},{key:"back",value:function(){var e=this.props,a=e.opened,t=void 0!==a&&a,i=e.onClear,r=e.onBack;console.log(t),t?r&&r():(this.setState({searchStarted:!1,searchText:""}),i&&i())}}]),t}(m["PureComponent"]),T=R,U=t("TSYQ"),A=t.n(U),L=t("iHd1"),D=t("SLhE"),w=function(e){var a=e.file,t=e.intl,i=e.onCancel,r=e.onConfirm,n=e.disabled,s=Number(localStorage.getItem("attachmentSize")||void 0),o=anyshare.outlook.isSupportType(),l=anyshare.sharelink,c=l.initialized,p=l.strategy,f=!1,E=11;if(c&&p){var _=p.config,g=p.switches,b=_||{},N=b.allowperm,S=void 0===N?25:N,k=g||{},C=k.enable_user_doc_out_link_share,O=void 0!==C&&C;f=25!=S&&1==O}f||(E=12);var F=void 0,M=void 0;o?isNaN(s)?(F=1,M=E):a.size>1024*s*1024?(F=void 0,M=E):(F=2,M=void 0):(F=void 0,M=E);var I=Object(m["useState"])(F),R=Object(u["a"])(I,2),T=R[0],U=R[1],L=Object(m["useState"])(M),D=Object(u["a"])(L,2),w=D[0],B=D[1],P=function(){var e=T&&1!=T?T:w;r&&r(e)};return v.a.createElement("div",{className:y.a.content},v.a.createElement("div",{className:y.a.body},v.a.createElement("p",null,t.formatMessage({id:"UI_CONTENT_SELECTEDFILE"})),!o||!isNaN(s)&&1024*s*1024<a.size||31457280<a.size?v.a.createElement(h["a"].Group,{disabled:n,className:y.a.radioGroup,value:w,onChange:function(e){return B(e.target.value)}},v.a.createElement(h["a"],{className:y.a.radio,value:11,disabled:!f},t.formatMessage({id:"UI_MENUITEM_ANONYMOUSSHARE"})),v.a.createElement(h["a"],{className:y.a.radio,value:12},t.formatMessage({id:"UI_MENUITEM_COPYSHARELINK"}))):v.a.createElement(h["a"].Group,{disabled:n,className:y.a.radioGroup,value:T,onChange:function(e){var a=e.target.value;U(a),1==a&&B(w||E)}},v.a.createElement(h["a"],{className:y.a.radio,value:1},t.formatMessage({id:"UI_MENUITEM_ASCLOUDATTACHMENT"})),1===T&&v.a.createElement(h["a"].Group,{disabled:n,className:A()(y.a.radioGroup,y.a.secondly),value:w,onChange:function(e){return B(e.target.value)}},v.a.createElement(h["a"],{className:y.a.radio,value:11,disabled:!f},t.formatMessage({id:"UI_MENUITEM_ANONYMOUSSHARE"})),v.a.createElement(h["a"],{className:y.a.radio,value:12},t.formatMessage({id:"UI_MENUITEM_COPYSHARELINK"}))),v.a.createElement(h["a"],{className:y.a.radio,value:2},t.formatMessage({id:"UI_MENUITEM_ASORDINARYATTACHMENT"})))),v.a.createElement("div",{className:y.a.footer},v.a.createElement(d["a"],{className:y.a.button,type:"primary",onClick:P,disabled:n},t.formatMessage({id:"UI_BUTTON_CONFIRM"})),v.a.createElement(d["a"],{className:y.a.button,onClick:i,disabled:n},t.formatMessage({id:"UI_BUTTON_CANCEL"}))))},B=function(e){Object(l["a"])(t,e);var a=Object(c["a"])(t);function t(){var e;Object(s["a"])(this,t);for(var i=arguments.length,r=new Array(i),n=0;n<i;n++)r[n]=arguments[n];return e=a.call.apply(a,[this].concat(r)),e.state={},e.__selectedValue=void 0,e.__file=void 0,e}return Object(o["a"])(t,[{key:"componentDidMount",value:function(){var e=this.props.dispatch;e&&e({type:"local/initLibs"})}},{key:"componentWillUnmount",value:function(){var e=this.props.dispatch;e&&e({type:"local/clear"});var a=document.getElementById("__fileInput");a&&document.body.removeChild(a)}},{key:"render",value:function(){var e=this,a=this.props,t=a.intl,i=a.pathHistory,s=a.files,o=a.loading,l=a.footerVisible,c=void 0!==l&&l,h=a.search,u=void 0===h?{}:h,m=this.state,_=m.showSave,g=void 0!==_&&_,b=m.checking,N=void 0!==b&&b,S=m.uploading,k=void 0!==S&&S,C=m.modalVisible,O=void 0!==C&&C,F=m.newFileName,M=m.showCreateForm,I=void 0!==M&&M,R=m.hasSameFolder,U=void 0!==R&&R,A=m.copying,L=void 0!==A&&A,D=this.getFile();return D?v.a.createElement(p["a"],{loading:o||N||k||L},g?v.a.createElement(T,Object(n["a"])({showCreateForm:I,hasSameFolder:U},u,{loading:u.loading||k,onCreate:this.createDir.bind(this),intl:t,breadcrumbsProps:{paths:i,onBack:this.back.bind(this),onItemClick:this.goto.bind(this)},fileExplorerProps:{emptyType:2,intl:t,files:s,displayMenu:!1,onOpen:this.openDir.bind(this)},footer:{fileName:D.name,visible:c,onSave:this.save.bind(this),onCancel:this.cancelSave.bind(this)},onSearch:this.search.bind(this),onOpenSearchResult:this.openSearchResult.bind(this),onClear:this.clearSearchResults.bind(this),onBack:this.searchBack.bind(this),onClearCreateError:function(){return e.setState({hasSameFolder:!1})},onCreateFormVisibleChange:function(a){return e.setState({showCreateForm:a,hasSameFolder:!1})}})):v.a.createElement(v.a.Fragment,null,v.a.createElement(f["a"],{title:t.formatMessage({id:"UI_HEADER_CLOUDATTACHMENTALERT"})}),v.a.createElement(w,{file:D,intl:t,onCancel:this.cancel.bind(this),onConfirm:this.confirm.bind(this)})),v.a.createElement(r["a"],{visible:O,closable:!1,maskClosable:!1,footer:[v.a.createElement(d["a"],{key:1,onClick:this.keepBoth.bind(this)},t.formatMessage({id:"UI_BUTTON_KEEPBOTH"})),v.a.createElement(d["a"],{key:2,onClick:this.replace.bind(this)},t.formatMessage({id:"UI_BUTTON_REPLACE"})),v.a.createElement(d["a"],{key:3,onClick:this.skip.bind(this)},t.formatMessage({id:"UI_BUTTON_SKIP"}))]},v.a.createElement("div",{className:y.a.dialog},v.a.createElement("img",{className:y.a.icon,src:"../../Images/Icons/SaveAndOpen/notice.png"}),v.a.createElement("div",{className:y.a.content},v.a.createElement("h2",null,t.formatMessage({id:"UI_NOTICE_HASSAMEFILETITLE"})),v.a.createElement("p",null,t.formatMessage({id:"UI_NOTICE_HASSAMEFILECONTENT"},{fileName:F})))))):v.a.createElement(E["c"],{to:"/"})}},{key:"confirm",value:function(e){switch(this.__selectedValue=e,e){case 11:this.saveFile();break;case 12:this.saveFile();break;case 2:this.addAttachment();break;default:break}}},{key:"addAttachment",value:function(){var e=this.getFile();if(e){var a=e.name,t=new FileReader;t.readAsDataURL(e),t.onload=function(e){var t,i,r=(null===(t=e.target)||void 0===t||null===(i=t.result)||void 0===i?void 0:i.toString())||"",n=r.indexOf("base64,"),s=r;n>=0&&(s=r.substr(n+7)),anyshare.outlook.addAttachment(a,s,(function(e){e?_["d"].goBack():D["a"].alertError(-1)}))},t.onerror=function(){}}}},{key:"saveFile",value:function(){var e=localStorage.getItem("defaultAttachmentFolderName"),a=localStorage.getItem("defaultAttachmentFolderUrl"),t=localStorage.getItem("defaultAttachmentFolderCKB");if(e&&a&&t&&"1"==t){var i=this.getFile();i&&this.save(null===i||void 0===i?void 0:i.name,a,e,1)}else this.setState({showSave:!0})}},{key:"cancel",value:function(){var e=this.props.dispatch;e&&e({type:"search/clear"}),_["d"].push({pathname:"/"})}},{key:"cancelSave",value:function(){this.cancel()}},{key:"createDir",value:function(e,a){var t=this,i=this.props.dispatch;L["a"].createDir(e.trim(),a).then((function(e){var r=e.Success,n=e.StatusCode;r&&0==n?(t.setState({showCreateForm:!1}),i&&i({type:"local/reload",payload:a})):403002039==n?t.setState({hasSameFolder:!0}):403001002!=n&&403002056!=n||D["a"].alertError(n)})).catch((function(e){D["a"].alertError(-1)}))}},{key:"keepBoth",value:function(){this.setState({modalVisible:!1});var e=this.state,a=e.newFileName,t=e.filePath,i=e.saveFolderName;a&&this.save(a,t,i,2)}},{key:"replace",value:function(){this.setState({modalVisible:!1});var e=this.state,a=e.fileName,t=e.filePath,i=e.saveFolderName;a&&this.save(a,t,i,3)}},{key:"skip",value:function(){this.setState({modalVisible:!1,newFileName:void 0,fileName:void 0,filePath:void 0,saveFolderName:void 0})}},{key:"save",value:function(e,a,t){var i=this,r=arguments.length>3&&void 0!==arguments[3]?arguments[3]:1,n=12===this.__selectedValue,s=this.props,o=s.pathHistory,l=s.intl,c=a,d=t;if(!c&&o&&o.length>0){var h=o[o.length-1];c=h.id,d=h.name}var u=this.getFile();u&&c&&(this.setState({uploading:!0}),L["a"].uploadFile(u,e,c,r,(function(a){if(i.setState({uploading:!1}),a){var t=a.Success,r=a.StatusCode,s=a.Data;if(t){var o=new Function("return "+s)();if(0===r){var h=o.FileId;console.log("\u4fdd\u5b58\u6210\u529f",h),n?(console.log("\u5b9e\u540d\u5171\u4eab"),i.copyShareLink(e,u.size,h)):_["d"].push({pathname:"/shared/create",query:{gnsId:h,fileType:"file",name:e,anonymoussharing:!0,size:u.size}})}else if(403002039===r){var m=o.FileName;i.setState({modalVisible:!0,fileName:e,filePath:c,saveFolderName:d,newFileName:m})}else if(403001031==r){var v=new Function("return "+o.ErrorDetail)();anyshare.system.alert(l.formatMessage({id:"ERROR_CODE_403001031"},{fileName:e,locker:v.locker}))}else if(403002070==r){v=new Function("return "+o.ErrorDetail)();anyshare.system.alert(l.formatMessage({id:"ERROR_CODE_403002070"},{fileName:e,size:D["a"].getFileSize(v.file_limit_size)}))}else D["a"].alertFolderError(r,d,e);return}}D["a"].alertError(-1)})))}},{key:"copyShareLink",value:function(e,a,t){var i=this;this.setState({copying:!0});var r=this.props.intl;anyshare.outlook.getRealNameShareLink(t,"file",(function(n){if(n){var s=n.Success,o=n.StatusCode,l=n.Data;if(s&&0==o)try{return anyshare.outlook.addCloudFiles(e,a,l,t),i.setState({copying:!1}),anyshare.system.success(r.formatMessage({id:"UI_NOTICE_SHARELINKCOPIED"})),void _["d"].push({pathname:"/"})}catch(c){console.error(c)}if(0!=o)return i.setState({copying:!1}),void D["a"].alertFileError(o,e)}i.setState({copying:!1}),D["a"].alertError(-1)}))}},{key:"openDir",value:function(e,a){var t=this.props.dispatch;t&&t({type:"local/opendir",payload:{id:e,name:a}})}},{key:"back",value:function(){var e=this.props.dispatch;e&&e({type:"local/back"})}},{key:"goto",value:function(e){var a=this.props.dispatch;a&&a({type:"local/goto",payload:e})}},{key:"search",value:function(e){if(e&&""!==e.trim()){var a,t=null===(a=this.getCurrentPath())||void 0===a?void 0:a.id,i=this.props.dispatch;i&&i({type:"search/search",payload:{text:e,gnsId:t,doctype:2}})}else this.clearSearchResults()}},{key:"openSearchResult",value:function(e,a){var t=this.props.dispatch;t&&t({type:"search/opendir",payload:{id:e,name:a,includeFiles:!1}})}},{key:"clearSearchResults",value:function(){var e=this.props.dispatch;e&&e({type:"search/clear"})}},{key:"searchBack",value:function(){var e=this.props.dispatch;e&&e({type:"search/searchBack"})}},{key:"getCurrentPath",value:function(){var e=this.props.pathHistory;if(e&&e.length>0)return e[e.length-1]}},{key:"getFile",value:function(){if(!this.__file){var e=document.querySelector("#__fileInput"),a=null===e||void 0===e?void 0:e.files;this.__file=a&&a.length>0?a[0]:void 0}return this.__file}}]),t}(v.a.PureComponent);a["default"]=Object(_["b"])((function(e){var a=e.loading,t=e.local,r=e.search;return{pathHistory:t.pathHistory,files:t.files,loading:a.models.local||a.effects["search/search"],footerVisible:t.footerVisible,search:Object(i["a"])(Object(i["a"])({},r),{},{loading:a.models.search})}}))(Object(_["e"])(B))},f7Sm:function(e,a,t){e.exports={content:"content___vFA4N",body:"body___1lkIb",footer:"footer___26VUY",button:"button___3P60m",radioGroup:"radioGroup___1M76l",secondly:"secondly___1v_cB",radio:"radio___3U5FF",dialog:"dialog___2OXka",icon:"icon___3cbi6"}},kSOO:function(e,a,t){e.exports={container:"container___3c7Je",search:"search___3S3Oe",footer:"footer___2UU6E",inputContent:"inputContent___1JkW_",label:"label___-Mr5x",input:"input___wux9G",buttons:"buttons___BPAzJ",button:"button___1X1ds"}}}]);