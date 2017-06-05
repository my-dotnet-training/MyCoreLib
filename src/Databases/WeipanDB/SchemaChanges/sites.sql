
CREATE TABLE IF NOT EXISTS sites(
	`site_id`       integer NOT NULL AUTO_INCREMENT,
	`site_name`     varchar(128) NOT NULL,
	`site_host`     varchar(32) NOT NULL,
	`app_id`        varchar(64) NOT NULL,
	`app_secret`    varchar(128) NOT NULL,
	`token`         varchar(64) NOT NULL,
	`encoding_key`  varchar(64) NOT NULL,
	`enabled`       boolean NOT NULL,
	`avatar_url`    varchar(256) NULL,
	`wxpayid`       varchar(16) NULL,   -- If no data in this column, then all payments will be redirected to our default one.
	`wxpaykey`      varchar(32) NULL,
	`time_created`  timestamp NOT NULL DEFAULT now(),
	`time_updated`  timestamp NOT NULL DEFAULT now(),
	`attributes`    text NULL,

	CONSTRAINT pk_sites PRIMARY KEY (site_id),
	CONSTRAINT uk_sites_host UNIQUE (site_host)
)
AUTO_INCREMENT = 100,
CHARACTER SET = 'utf8mb4';


/* default site */
INSERT INTO sites(site_id, site_name, site_host, app_id, app_secret, `enabled`, time_created, time_updated,
	`token`, encoding_key, avatar_url, wxpayid, wxpaykey)
VALUES(100, '测试公众号', 'default', 'invalid app_id', 'invalid app_secret', false, now(), now(),
	'invalid token', 'invalid encoding key', '~/images/headimg_7.jpg', null, null)
ON DUPLICATE KEY UPDATE
	time_updated = now();