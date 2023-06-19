mergeInto(LibraryManager.library,{

    ConnectStart : function(){
		connect();
	
    },
	ConnectClose : function(){
		closeConnect();
	
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

	// id 필터링
	BadWordCheck : function(usrId){
		console.log(UTF8ToString(usrId));
		console.log(BadWordFilter.Check(UTF8ToString(usrId),'ko'));
        return BadWordFilter.Check(UTF8ToString(usrId),'ko');
	},

  ShowSeatPanel : function(panelId){
		console.log(UTF8ToString(panelId));
		initSeatPanel(UTF8ToString(panelId));
	},

})