package com.stsprojSample.home.HelloController;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.net.URLEncoder;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;
import java.util.Map;

import org.json.simple.JSONArray;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.multipart.MultipartHttpServletRequest;

import com.stsprojSample.home.MessageVo.MessageVo;
import com.stsprojSample.home.TemplateService.TemplateService;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;


@Tag(name="HelloController", description="HelloController API")
@Controller
public class HelloController {
	
	private MultipartFile multipartFile ; // 사용자가 받아온 파일
	private File saveFile; 
	private String fPath;
	private MultipartFile tempFile; // 임시파일
	
	private final TemplateService templateService;

    public HelloController(TemplateService ts) {
        this.templateService = ts;
    }

	@RequestMapping("/hello")
	public String main() {
		return "index";
	}
	
	@GetMapping("/unity")
	public String Unity() {
		
		return "metaverse";
	}
	
	@Operation(summary="테스트용", description="테스트용 api입니다.")
	@PostMapping("/selectMessge")
	@ResponseBody
	public String SelectMessge(@RequestBody MessageVo params) {
		
		List selList = templateService.SelectMessge(params);
		
		return JSONArray.toJSONString(selList);
		
	}
	

	@ResponseBody
	@RequestMapping(value = "/sendImg", method = RequestMethod.POST, produces="application/json;charset=UTF-8")
	public String MsgImgfileupload(MultipartHttpServletRequest req, HttpServletRequest request) throws Exception {

		//String savePath = request.getSession().getServletContext().getRealPath("/resources/uploadImg");
		
		MultipartFile file = req.getFile("file");
		// String fileRealName2 = file.getOriginalFilename(); // 파일명을 얻어낼 수 있는 메서드!
		String fileRealName = new String(file.getOriginalFilename().getBytes(), "UTF-8");
		String fileNm =  System.currentTimeMillis() + fileRealName;
		
		String savePath = "c:\\Temp\\"+File.separator+fileRealName;
		//String fileExtsn = FilenameUtils.getExtension(fileNm);
		long fileSize = file.getSize();  // bytes로 파일사이즈 가져오기
		//File saveFiles = new File(savePath + File.separator +fileRealName);
		File saveFiles = new File(savePath);

		try {
			file.transferTo(saveFiles);

		} catch (IllegalStateException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}

		return saveFiles.toString();
	}
	
	@GetMapping(value="/imgDown")
	public void imgDown(HttpServletResponse response, HttpServletRequest request,@RequestParam("file_nm") String fileNm) throws IOException{
		
		String filePath = "c:\\Temp\\";
		String fileName = fileNm;
		
		String saveFileName = filePath + fileNm;
        File file = new File(saveFileName);
        Path source = Paths.get(saveFileName);
        String contentType = Files.probeContentType(source);
        long fileLength = file.length();
        
        response.setHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\";");
        response.setHeader("Content-Transfer-Encoding", "binary");
        response.setHeader("Content-Type", contentType);
        response.setHeader("Content-Length", "" + fileLength);
        response.setHeader("Pragma", "no-cache;");
        response.setHeader("Expires", "-1;");
        
        try(
                FileInputStream fis = new FileInputStream(saveFileName);
                OutputStream out = response.getOutputStream();
        ){
                int readCount = 0;
                byte[] buffer = new byte[1024];
            while((readCount = fis.read(buffer)) != -1){
                    out.write(buffer,0,readCount);
            }
        }catch(Exception ex){
            throw new RuntimeException("file Save Error");
        }
	}
		
		

}
