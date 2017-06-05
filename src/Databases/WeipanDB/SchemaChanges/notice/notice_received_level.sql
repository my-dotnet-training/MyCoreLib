CREATE TABLE IF NOT EXISTS `notice_received_level` (
	`notice_id`		integer NOT NULL AUTO_INCREMENT COMMENT '编号',
	`received_notice_level` integer NOT NULL COMMENT '接收消息层级 [99] admin，[0] center admin，[1~4] member',
	`time_created`	datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY (`notice_id`,`received_notice_level`)
) 
COMMENT '公告',
CHARACTER SET = 'utf8mb4';