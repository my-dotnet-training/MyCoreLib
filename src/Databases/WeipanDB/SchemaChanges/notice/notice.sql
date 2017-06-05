CREATE TABLE IF NOT EXISTS `notice` (
  `notice_id`	integer NOT NULL AUTO_INCREMENT COMMENT '编号',
  `user_type`	integer NOT NULL COMMENT '前后端用户 1-前端用户 2-后端用户',
  `title`		varchar(200) NOT NULL COMMENT '标题',
  `received_notice_levels`varchar(100) NOT NULL COMMENT '接收消息层级',
  `content`		text NOT NULL COMMENT '内容',
  `created_user_id`	integer DEFAULT '0',
  `read_count`	integer DEFAULT '0',
  `status`		integer NOT NULL DEFAULT '0' COMMENT '状态 0-正常 1-删除 ',
  `time_created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `time_updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`notice_id`),
  INDEX ix_notice_noticeId_status_usertype (`notice_id`,`status`,user_type) ,
  INDEX ix_notice_status_usertype_timecreated (`status`,user_type,time_created ) 
) 
COMMENT '公告',
CHARACTER SET = 'utf8mb4';