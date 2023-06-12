mergeInto(LibraryManager.library,{

	// 강당씬 동영상 경로
	GetVideoRoot : function(){
		return;
	},

    // 메세지 전송
    SendMsg : function(msg){

		var msgObj = JSON.parse(UTF8ToString(msg));
		msgObj.message = BadWordFilter.Replace(msgObj.message,'ko');
	    //this.ws.send(UTF8ToString(msg));
		this.ws.send(JSON.stringify(msgObj));
    },

	// id 필터링
	BadWordCheck : function(usrId){
		console.log(UTF8ToString(usrId));
		console.log(BadWordFilter.Check(UTF8ToString(usrId),'ko'));
        return BadWordFilter.Check(UTF8ToString(usrId),'ko');
	},

  ShowSeatPanel : function(panelId){
		console.log(UTF8ToString(panelId));
		InitSeatPanel(UTF8ToString(panelId));
	},
	//군수 클릭후 url 버튼 클릭 이벤트
	OpenUseURL: function (index){
		OpenUrl(UTF8ToString(index));
	}

})