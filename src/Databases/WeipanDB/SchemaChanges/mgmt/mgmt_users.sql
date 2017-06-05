
CREATE TABLE IF NOT EXISTS mgmt_users(
	`user_id`               integer     NOT NULL AUTO_INCREMENT,
	`user_name`             varchar(64) NOT NULL,
	`cell_phone`            varchar(32) NULL,
	`display_name`          varchar(64) NOT NULL,
	`password`              varchar(64) NOT NULL, -- SHA256 hashed result
	`salt`                  varchar(16) NOT NULL, -- Password salt for SHA256 hashing.
	`enabled`               boolean     NOT NULL DEFAULT true,
	`next_login_time`	    datetime    NOT NULL DEFAULT now(), -- The date and time when the account could be logged in again.
	`global_permissions`    bigint      NOT NULL,
	`center_id`             integer     NOT NULL DEFAULT 0,
	`member_id`             integer     NOT NULL DEFAULT 0,
	`center_permissions`    bigint      NOT NULL DEFAULT 0,
	`member_permissions`    bigint      NOT NULL DEFAULT 0,
	`created_by`            integer     NOT NULL,
	`time_created`          datetime    NOT NULL DEFAULT now(),
	`time_updated`          datetime    NOT NULL DEFAULT now(),	
	`attributes`            text        NULL,

	CONSTRAINT pk_mgmt_users PRIMARY KEY (user_id),
	CONSTRAINT fk_mgmt_users_created_by FOREIGN KEY (created_by) REFERENCES mgmt_users(user_id),
	CONSTRAINT uk_mgmt_users_user_name UNIQUE (user_name)
)
AUTO_INCREMENT = 1000,
CHARACTER SET = 'utf8mb4';


/* default site */
INSERT IGNORE INTO mgmt_users(user_id, user_name, display_name, `password`, salt, global_permissions, created_by)
VALUES(1, 'admin', '系统管理员', '9eb6c64708fa40f227414d83dc7d80d8a24785c9904628c722630c5ca12534fb', 'salt', 0x7fffffff, 1) -- created by admin himself (**v*w*)
ON DUPLICATE KEY UPDATE
time_updated = now();