
CREATE TABLE IF NOT EXISTS `site_wx_messages`
(
	`site_id`         integer NOT NULL,
	`msg_type`        varchar(32) NOT NULL,
	`content`         text NOT NULL,
	`created_by`      integer NOT NULL,
	`updated_by`      integer NOT NULL,
	`enabled`         boolean NOT NULL,
	`is_template_message` boolean NOT NULL,
	`time_created`    timestamp NOT NULL DEFAULT NOW(),
	`time_updated`    timestamp NOT NULL DEFAULT NOW(),

	CONSTRAINT pk_site_wx_messages PRIMARY KEY (site_id, msg_type),
	CONSTRAINT fk_site_wx_messages_site_id FOREIGN KEY (site_id) REFERENCES sites(site_id)
)
CHARACTER SET = 'utf8mb4';