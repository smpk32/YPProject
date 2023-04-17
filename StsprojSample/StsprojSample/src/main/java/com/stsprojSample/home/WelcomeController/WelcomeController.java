package com.stsprojSample.home.WelcomeController;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
public class WelcomeController {
	
	@GetMapping("/welcome")
	@ResponseBody
	public String Welcome(@RequestParam("id") String id) {
		System.out.println(id);
		return id;
	}

}
