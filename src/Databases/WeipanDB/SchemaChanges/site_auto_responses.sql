
CREATE TABLE IF NOT EXISTS `site_auto_responses`
(
	`site_id`         integer NOT NULL,
	`response_id`     int NOT NULL AUTO_INCREMENT,
	`response_type`   varchar(16) NOT NULL,
	`title`           varchar(64) NOT NULL,
	`content`         text NOT NULL,
	`keywords`        text NULL,
	`created_by`      integer NOT NULL,
	`updated_by`      integer NOT NULL,
	`enabled`         boolean NOT NULL,
	`time_created`    timestamp NOT NULL DEFAULT NOW(),
	`time_updated`    timestamp NOT NULL DEFAULT NOW(),

	CONSTRAINT pk_site_auto_responses PRIMARY KEY (response_id),
	CONSTRAINT fk_site_auto_responses_site_id FOREIGN KEY (site_id) REFERENCES sites(site_id),
	INDEX ix_site_auto_responses_site_id_time_updated (site_id, time_updated desc)
)
CHARACTER SET = 'utf8mb4';