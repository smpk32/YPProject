mergeInto(LibraryManager.library,{

	// 강당씬 동영상 경로
	GetVideoRoot : function(){
		returnVideoUrl();
	},

	// baseUrl 리턴
	GetBaseURL: function (index){
		returnBaseUrl();
	},

	//군수 클릭후 url 버튼 클릭 이벤트
	OpenUseURL: function (index){
		openUrl(UTF8ToString(index));
	},

	// 전체화면 토글
	ChangeFullScreen: function (){
		changeFullScreenMode();
	},

	// 캐릭터 선택 후 전체화면으로 전환
	OpenFullScreen: function (){
		openFullScreenMode();
	}

})