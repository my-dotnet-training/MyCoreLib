
CREATE TABLE IF NOT EXISTS mgmt_user_members(
	`user_id`               integer     NOT NULL COMMENT '后台管理员内部编号',
	`center_id`             integer     NOT NULL COMMENT '运营中心编号',
	`member_id`             integer     NOT NULL COMMENT '会员内部编号',
	`member_name`           varchar(64) NULL COMMENT '会员登陆名',
	`parent_member_id`      integer     NULL COMMENT '上级会员编号，0代表综合会员',
	`level`                 integer     NULL DEFAULT 0 COMMENT '层级',
	PRIMARY KEY (`user_id`,`center_id`, `member_id`)
)
AUTO_INCREMENT = 1000,
CHARACTER SET = 'utf8mb4';