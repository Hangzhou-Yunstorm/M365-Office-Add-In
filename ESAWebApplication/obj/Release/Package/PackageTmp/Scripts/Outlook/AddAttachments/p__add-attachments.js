(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([[4],{g321:function(e,t,a){"use strict";a.r(t);a("+L6B");var i=a("2/Rp"),n=a("fWQN"),s=a("mtLc"),r=a("yKVA"),o=a("879j"),c=a("q1tI"),h=a.n(c),l=a("9kvl"),p=a("4i/J"),d=a("BuAN"),u=a("YB8P"),y=a("Jgw9"),f=a("Mn5C"),v=a("XkFa"),g=a("SLhE"),k=function(e){Object(r["a"])(a,e);var t=Object(o["a"])(a);function a(){var e;Object(n["a"])(this,a);for(var i=arguments.length,s=new Array(i),r=0;r<i;r++)s[r]=arguments[r];return e=t.call.apply(t,[this].concat(s)),e.mounted=!0,e.FileInputId="__fileInput",e.state={checking:!1,copying:!1},e}return Object(s["a"])(a,[{key:"componentDidMount",value:function(){var e=this.props.dispatch;e&&e({type:"file/initLibs"}),this.mounted=!0,console.log("mount",this.mounted)}},{key:"componentWillUnmount",value:function(){this.setState=function(){}}},{key:"render",value:function(){var e=this.props,t=e.intl,a=e.files,n=e.pathHistory,s=e.loading,r=void 0!==s&&s,o=e.searchResults,c=e.searchStarted,l=void 0!==c&&c,g=e.searchText,k=this.state,m=k.checking,E=void 0!==m&&m,S=k.copying,R=void 0!==S&&S;return console.log(o),h.a.createElement(f["a"],{loading:r||E||R},h.a.createElement(d["a"],{canBack:l,onBack:this.pageBack.bind(this),title:t.formatMessage({id:"UI_HEADER_TITLE"})}),h.a.createElement(p["a"],{placeholder:t.formatMessage({id:"UI_SEARCH_PLACEHOLDER"}),value:g,onSearch:this.search.bind(this),onFocus:this.searchBefore.bind(this),onClear:this.clearSearchResults.bind(this)}),l?h.a.createElement(v["a"],{results:o,displayMenu:!0,onOpenShareLink:this.openShareLink.bind(this),onCopyShareLink:this.copyShareLink.bind(this),onOpen:this.openSearchResult.bind(this)}):h.a.createElement(h.a.Fragment,null,h.a.createElement(u["a"],{paths:n,onBack:this.back.bind(this),onItemClick:this.goto.bind(this)}),h.a.createElement(y["a"],{emptyType:1,intl:t,height:"calc(100vh - 172px)",files:a,onOpen:this.openDir.bind(this),onOpenShareLink:this.openShareLink.bind(this),onCopyShareLink:this.copyShareLink.bind(this)}),h.a.createElement("div",{style:{height:"42px",paddingBottom:"10px"}},h.a.createElement(i["a"],{style:{padding:"0px"},icon:"plus",type:"link",onClick:this.openFileDialog.bind(this)},t.formatMessage({id:"UI_BUTTON_ADDFROMLOCAL"})))))}},{key:"openFileDialog",value:function(){var e=this,t=document.getElementById(this.FileInputId);t||(t=document.createElement("input"),t.setAttribute("type","file"),t.style.display="none",t.id=this.FileInputId,t.addEventListener("change",(function(){e.onSelectFile()})),document.body.appendChild(t)),t.click()}},{key:"onSelectFile",value:function(){var e=this,t=this.props.intl,a=document.querySelector("#"+this.FileInputId);if(a){var i=a.files;if(i&&i.length>0){var n=i[0];if(n.size<=0)return void anyshare.system.alert(t.formatMessage({id:"UI_NOTICE_EMPTYFILE"}));this.setState({checking:!0}),anyshare.sharelink.initialize((function(a){var i=a.switches,n=void 0===i?{}:i,s=a.config,r=void 0===s?{}:s;e.setState({checking:!1});var o=n.enable_user_doc_out_link_share,c=void 0!==o&&o;if(c){r.allowperm;l["d"].push("/local")}else anyshare.system.alert(t.formatMessage({id:"ERROR_USERDOC_SHAREDIABLED"}))}),(function(t){e.setState({checking:!1}),g["a"].alertError(t)}))}}}},{key:"openDir",value:function(e,t){var a=this.props.dispatch;a&&a({type:"file/opendir",payload:{id:e,name:t}})}},{key:"copyShareLink",value:function(e){var t=this;console.log("\u5b9e\u540d\u5171\u4eab",e),this.setState({copying:!0});var a=this.props.intl,i=e.id,n=e.isDir,s=e.size,r=e.name,o=n?"folder":"file";anyshare.sharelink.initialize((function(n){var c=n.switches,h=void 0===c?{}:c,l=(n.config,h.enable_user_doc_inner_link_share),p=void 0!==l&&l;if(!p)return t.setState({copying:!1}),void anyshare.system.alert(a.formatMessage({id:"ERROR_USERDOC_DISABLEINNERLINKSHARE"}));anyshare.outlook.getRealNameShareLink(i,o,(function(n){if(n){var o=n.Success,c=n.Data,h=n.StatusCode;if(o&&0==h)try{return anyshare.outlook.addCloudFiles(r,s,c,i),t.setState({copying:!1}),void anyshare.system.success(a.formatMessage({id:"UI_NOTICE_SHARELINKCOPIED"}))}catch(l){console.error(l)}if(0!=h)return t.setState({copying:!1}),void(e.isDir?g["a"].alertFolderError(h,r):g["a"].alertFileError(h,r))}t.setState({copying:!1}),g["a"].alertError(-1)}))}),(function(e){t.setState({copying:!1}),g["a"].alertError(e)}))}},{key:"openShareLink",value:function(e){var t=this,a=Object(l["c"])();this.setState({checking:!0}),anyshare.sharelink.check(e.id,(function(i,n){return i?(t.setState({checking:!1}),void anyshare.system.alert(a.formatMessage({id:"ERROR_USER_FREEZE"}))):n?void anyshare.sharelink.initialize((function(i){var n=i.switches,s=void 0===n?{}:n,r=i.config,o=void 0===r?{}:r;t.setState({checking:!1});var c=s.enable_user_doc_out_link_share,h=void 0!==c&&c;if(h){var p=o.allowperm;25!=p||e.isDir?l["d"].push({pathname:"/shared",query:{gnsId:e.id,fileType:e.isDir?"folder":"file",name:e.name,size:e.size}}):anyshare.system.alert(a.formatMessage({id:"ERROR_USER_NOHAVEPERMS"}))}else anyshare.system.alert(a.formatMessage({id:"ERROR_USERDOC_SHAREDIABLED"}))}),(function(e){t.setState({checking:!1}),g["a"].alertError(e)})):(t.setState({checking:!1}),void anyshare.system.alert(a.formatMessage({id:"ERROR_USER_NOTOWNER"})))}),(function(e){g["a"].alertError(e)}))}},{key:"pageBack",value:function(){var e=this.props.dispatch;e&&e({type:"search/end"}),this.clearSearchResults()}},{key:"back",value:function(){var e=this.props.dispatch;e&&e({type:"file/back"})}},{key:"goto",value:function(e){var t=this.props.dispatch;t&&t({type:"file/goto",payload:e})}},{key:"searchBefore",value:function(){var e=this.props.dispatch;e&&e({type:"search/start"})}},{key:"search",value:function(e){var t=this.props.dispatch;if(t&&t({type:"search/input",payload:e}),""==e.trim())this.clearSearchResults();else{var a=this.getCurrentDir();t&&t({type:"search/search",payload:{text:e,gnsId:a,doctype:3}})}}},{key:"clearSearchResults",value:function(){var e=this.props.dispatch;e&&e({type:"search/clear"})}},{key:"openSearchResult",value:function(e,t){var a=this.props.dispatch;a&&a({type:"search/opendir",payload:{id:e,name:t}}),l["d"].push({pathname:"/search/result",query:{id:e,name:t}})}},{key:"getCurrentDir",value:function(){var e=this.props.pathHistory;if(e&&e.length>0){var t=e[e.length-1];return t.id}}}]),a}(c["Component"]);t["default"]=Object(l["b"])((function(e){var t=e.loading,a=e.file,i=e.search;return{pathHistory:a.pathHistory,files:a.files,searchResults:i.results,searchStarted:i.searchStarted,searchText:i.searchText,loading:t.models.file||t.effects["search/search"]}}))(Object(l["e"])(k))}}]);