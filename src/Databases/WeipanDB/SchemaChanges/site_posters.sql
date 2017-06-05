
CREATE TABLE IF NOT EXISTS `site_posters`
(
	`site_id`         integer NOT NULL,
	`poster_name`     varchar(64) NOT NULL,
	`message`         varchar(128) NULL,
	`metadata`        text NULL,
	`poster_image`    mediumblob NOT NULL,
	`content_type`    varchar(32) NOT NULL,
	`created_by`      integer NOT NULL,
	`updated_by`      integer NOT NULL,
	`enabled`         boolean NOT NULL,
	`time_created`    timestamp NOT NULL DEFAULT NOW(),
	`time_updated`    timestamp NOT NULL DEFAULT NOW(),

	CONSTRAINT pk_site_posters PRIMARY KEY (site_id, poster_name),
	CONSTRAINT fk_site_posters_site_id FOREIGN KEY (site_id) REFERENCES sites(site_id)
)
CHARACTER SET = 'utf8mb4';