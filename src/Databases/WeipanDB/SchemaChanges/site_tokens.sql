
CREATE TABLE IF NOT EXISTS `site_tokens`
(
	`site_id`         integer NOT NULL,
	`token_type`      integer NOT NULL COMMENT 'token type, 1: access token, 2: JS api ticket, 3: wx_card',
	`token`           varchar(256) NOT NULL,
	`created_by`      varchar(32) NOT NULL,
	`time_updated`    timestamp NOT NULL DEFAULT NOW(),
	`time_expired`    timestamp NOT NULL DEFAULT NOW(),

	CONSTRAINT pk_site_tokens PRIMARY KEY (site_id, `token_type`),
	CONSTRAINT fk_site_tokens_site_id FOREIGN KEY (site_id) REFERENCES sites(site_id)
)
ENGINE MEMORY,
CHARACTER SET = 'utf8mb4';