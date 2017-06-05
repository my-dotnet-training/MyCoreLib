
CREATE TABLE IF NOT EXISTS system_logs(
	log_id          integer NOT NULL AUTO_INCREMENT,
	`tag`           varchar(32) NULL,
	`type`          ENUM('error', 'warning', 'info') NOT NULL,
	`message`       text NOT NULL,
	time_created    datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,

	CONSTRAINT pk_system_logs PRIMARY KEY (log_id),
	INDEX ix_system_logs_tag_type (`tag`, `type`)
)
CHARACTER SET = 'utf8mb4';