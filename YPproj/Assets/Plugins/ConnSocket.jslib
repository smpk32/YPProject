mergeInto(LibraryManager.library,{

    Connect : function(){

	this.ws = new WebSocket("ws://192.168.1.113:8080/socket");

	this.ws.onopen = function(e){ 
			console.log("info : connection opened.");
			//unityInstance.SendMessage("MainCanvas","MasterCheck",e.data);
		}
		
		this.ws.onmessage = function(e){ 
			unityInstance.SendMessage("MainCanvas","ChatCheck",e.data);
		}
		
		this.ws.onclose = function(e){ 
			console.log("info : connection closed");
			//unityInstance.SendMessage("MainCanvas","QuitUserInfo",e.data);
		};
		
		this.ws.onerror = function(e){
			console.log("error")
		};
    },
    // 메세지 전송
    SendMsg : function(msg){

		var msgObj = JSON.parse(UTF8ToString(msg));
		msgObj.message = BadWordFilter.Replace(msgObj.message,'ko');
	    //this.ws.send(UTF8ToString(msg));
		this.ws.send(JSON.stringify(msgObj));
    },

    // 이미지 파일 업로드
    ImgFileOpen : function(){
        clickImgMsgUploadButton();
    },

    ImgFileSubmit : function(){
        clickImgSubmitBtn();
},


})