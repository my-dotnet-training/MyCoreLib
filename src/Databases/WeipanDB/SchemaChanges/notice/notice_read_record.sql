
CREATE TABLE IF NOT EXISTS `notice_read_record` (
  `notice_read_record_id` integer NOT NULL AUTO_INCREMENT COMMENT '自增编号',
  `user_id`				integer NOT NULL COMMENT '用户ID',
  `user_type`			integer NOT NULL COMMENT '前后端用户 1-前端 2-后端',
  `notice_id`			integer NOT NULL COMMENT '公告ID',
  `time_created`		datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建日期',
  PRIMARY KEY (`notice_read_record_id`),
  UNIQUE KEY `ix_notice_readcount_userid_noticeid` (`user_id`,`notice_id`) ,
  CONSTRAINT `fk_noticereadcount_mgntuser` FOREIGN KEY (`user_id`) REFERENCES `mgmt_users` (`user_id`),
  CONSTRAINT `fk_noticereadcount_notice` FOREIGN KEY (`notice_id`) REFERENCES `notice` (`notice_id`),  
  INDEX ix_notice_readcount_user_id (`user_id`) ,
  INDEX ix_notice_readcount_notice_id (`notice_id`)
)
COMMENT='公告阅读记录',
CHARSET=utf8mb4 ;