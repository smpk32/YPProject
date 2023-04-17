function move(){
	location.href ='/hello';
}

//file변수
var str;
var fileNm;  
var fileString;

function selectCnt(){
	$.ajax({
                type : "POST",
                url : "/selectMessge",
                data : JSON.stringify({
					"sender" : "psm",
					"rownum" : 10
				}),
				contentType: 'application/json',
                success : function(res){ 
                    // 응답코드 > 0000
                    //alert(res);
                    console.log("갯수 : "+res);
                },
                error : function(XMLHttpRequest, textStatus, errorThrown){ // 비동기 통신이 실패할경우 error 콜백으로 들어옵니다.
                    alert("통신 실패.")
                }
            });
}

var ws;
function Connect(){
	ws = new WebSocket("ws://192.168.1.142:8080/socket");

	ws.onopen = function(e){ 
			console.log("info : connection opened.");
		}
		
		ws.onmessage = function(e){ 
			console.log("받은 메세지 : "+e.data); 
			//unityInstance.SendMessage("SocketMng","ChatCheck",e.data);
		}
		
		ws.onclose = function(e){ 
			console.log("info : connection closed");
		};
		
		ws.onerror = function(e){
			console.log(e);
			console.log("error")
		};
}

function clickImgMsgUploadButton() {
	document.getElementById("sendImgMsg").value = "";
	document.getElementById("sendImgMsg").click();
}

function imgfileOnchange(fis) {
	if(fis == null){
		return;
	}
	str = fis.files[0];
   	console.log(str.name+" ,"+ str.size +" ,"+ str.type);
	//unityInstance.SendMessage("MainCanvas","SendImgMsg",str.name);
	unityInstance.SendMessage("MainCanvas","SendImgMsg",str.name);
	
	/*var filePath = document.getElementById("sendImgMsg").value
    var formData = new FormData();
    formData.append("file",str);
    
    var xhr = new XMLHttpRequest();
    	   xhr.open("POST", "http://192.168.1.142:8080/filePreview",true);  
    	   xhr.onload =  function(result){      
    	       console.log(result);
    	      if(xhr.status === 200){
    	        
    	           console.log("start preview!");
    	           unityInstance.SendMessage("MainCanvas","SetPreView2"); 
    	        
    	        }         
    	      
    	    }
    	   
    	     xhr.send(formData);  */
}


function clickImgSubmitBtn() {
   	    
	var formData = new FormData();
  	     
    formData.append("file",str);
    var xhr = new XMLHttpRequest();
  	   	 
    xhr.open("POST", "http://192.168.1.142:8080/sendImg",true);  
    //xhr.setRequestHeader('Content-Type','text/html;charset=utf-8');
	xhr.onload =  function(result){
  	       
   		filePath = xhr.responseText;
  	        
    	if(xhr.status === 200){
			console.log(filePath);
			unityInstance.SendMessage("MainCanvas","ReceiveImgFilePath",filePath);	   	        	   	        	
    	}
    }   
	xhr.send(formData);
}

(function () {
    //selectCnt();
})();