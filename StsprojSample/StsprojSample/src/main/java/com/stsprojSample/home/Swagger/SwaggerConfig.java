package com.stsprojSample.home.Swagger;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import io.swagger.v3.oas.models.Components;
import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.info.Info;

@Configuration
public class SwaggerConfig {

	@Bean
	  public OpenAPI openAPI(@Value("v0.1") String springdocVersion) {
	    Info info = new Info()
	        .title("양평샘플프로젝트")
	        .version(springdocVersion)
	        .description("스웨거 샘플 제작");

	    return new OpenAPI()
	        .components(new Components())
	        .info(info);
	  }

}
