package com.stsprojSample.home.WebSocket;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.json.JSONObject;
import org.json.simple.JSONArray;
import org.springframework.stereotype.Component;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

import com.fasterxml.jackson.databind.ObjectMapper;

@Component
public class SocketHandler extends TextWebSocketHandler{

	HashMap<String, WebSocketSession> sessionList = new HashMap<>(); //웹소켓 세션을 담아둘 맵
	
	HashMap<String, String> userList = new HashMap<>(); //웹소켓 세션을 담아둘 맵
	
	
	
	@Override
	public void handleTextMessage(WebSocketSession session, TextMessage message) {
		//메시지 발송
		String msg = message.getPayload();
		System.out.println(msg);
		
		try{
			ObjectMapper mapper = new ObjectMapper();
	        Map<String, String> map = mapper.readValue(msg, Map.class);
	        
	        if(map.get("type").toString().equals("sendUsrInfo")) {
	        	String nickNm =  NickNmCheck(map.get("sender").toString());
	        	map.put("sender", nickNm);
	        	
	        	JSONObject jsonObject = new JSONObject(map);
	        	msg = jsonObject.toString();
	        	
	        	System.out.println("중복체크한 닉네임 : " + nickNm);
	        	userList.put(map.get("sessionId").toString(), nickNm);
	        	
	        	WebSocketSession wss = sessionList.get(session.getId());
	        	wss.sendMessage(new TextMessage(msg));
	        	return;
	        	
	        }

	    } catch (IOException e){
	        e.printStackTrace();
	    }
		
		for(String key : sessionList.keySet()) {
			WebSocketSession wss = sessionList.get(key);
			try {
				synchronized(wss) {
					wss.sendMessage(new TextMessage(msg));
				}
				
			}catch(Exception e) {
				e.printStackTrace();
			}
		}
	}
	
	@Override
	public void afterConnectionEstablished(WebSocketSession session) throws Exception {
		//소켓 연결
		super.afterConnectionEstablished(session);
		sessionList.put(session.getId(), session);
		
		
		HashMap<String,String> useInfo = new HashMap<>();
		
		useInfo.put("type", "sessionId");
		useInfo.put("sessionId", session.getId());
		
		WebSocketSession wss = sessionList.get(session.getId());
		
		JSONObject jsonObject = new JSONObject(useInfo);
		try {
			wss.sendMessage(new TextMessage(jsonObject.toString()));
		}catch(Exception e) {
			e.printStackTrace();
		}
	}
	
	@Override
	public void afterConnectionClosed(WebSocketSession session, CloseStatus status) throws Exception {
		
		//소켓 종료
		sessionList.remove(session.getId());
		
		
		/*
		 * if(sessionList.size()>0) { HashMap<String,String> useInfo = new HashMap<>();
		 * 
		 * useInfo.put("type", "quitUserInfo"); useInfo.put("sender",
		 * userList.get(session.getId()));
		 * 
		 * JSONObject jsonObject = new JSONObject(useInfo); for(String key :
		 * sessionList.keySet()) { WebSocketSession wss = sessionList.get(key); try {
		 * wss.sendMessage(new TextMessage(jsonObject.toString())); }catch(Exception e)
		 * { e.printStackTrace(); } } }
		 */
		
		
		
		
		userList.remove(session.getId());
		
		super.afterConnectionClosed(session, status);
	}
	
	
	
	// 닉네임 중복 체크
	public String NickNmCheck(String nickNm) {
		int idx = 1;
		
		String resultNick = nickNm;
		
		List<String> keyList = new ArrayList<>();
		
		for(String key : userList.keySet()) {
			keyList.add(key);
		}

		for(int i = 0; i < userList.size(); i++) {
			System.out.println(i);
			System.out.println(userList.get(keyList.get(i)));
			if(resultNick.equals(userList.get(keyList.get(i)))) {
				//System.out.println(resultNick);
				resultNick = nickNm + "_"+idx;
				System.out.println("바뀐 닉네임 : "+resultNick);
				idx++;
				// 한바퀴돌고 for문의 i++이 실행되기때문에 -1로 세팅
				i = -1;
			}
		}
		
		
		return resultNick;
	}
}

