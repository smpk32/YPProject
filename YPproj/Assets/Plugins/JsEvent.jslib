mergeInto(LibraryManager.library,{

	// 강당씬 동영상 경로
	GetVideoRoot : function(){
		return;
	},

	//군수 클릭후 url 버튼 클릭 이벤트
	OpenUseURL: function (index){
		OpenUrl(UTF8ToString(index));
	}

})