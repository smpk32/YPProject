package com.stsprojSample.home.TemplateMapper;
import java.util.List;
import org.springframework.stereotype.Repository;

import com.stsprojSample.home.MessageVo.MessageVo;

@Repository
public interface TemplateMapper {

	List SelectMessge(MessageVo rownum);
}
