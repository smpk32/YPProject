package com.stsprojSample.home.TemplateService;

import org.apache.ibatis.session.SqlSession;
import org.mybatis.spring.SqlSessionTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;

import com.stsprojSample.home.MessageVo.MessageVo;
import com.stsprojSample.home.TemplateMapper.TemplateMapper;


@Service
public class TemplateServiceImpl implements TemplateService{

	@Autowired
	private SqlSessionTemplate sqlSession;

	/*
	 * public TemplateServiceImpl(SqlSession ss) { this.sqlSession = ss; }
	 */
	
	@Override
	public List SelectMessge(MessageVo params) {
		TemplateMapper tm = sqlSession.getMapper(TemplateMapper.class);
	
		return tm.SelectMessge(params);
	}

}
