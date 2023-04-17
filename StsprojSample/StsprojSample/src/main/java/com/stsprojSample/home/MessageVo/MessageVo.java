package com.stsprojSample.home.MessageVo;

import io.swagger.v3.oas.annotations.media.Schema;

public class MessageVo {
	
	@Schema(description="보낸사람", example="psm")
	public String sender;
	
	@Schema(description="row갯수", example="10")
	public int rownum;
	
	
}
