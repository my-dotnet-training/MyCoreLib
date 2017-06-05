
DROP PROCEDURE IF EXISTS sproc_site_upsert_V_2;

DELIMITER //

CREATE PROCEDURE sproc_site_upsert_V_2(
	_site_id        integer,
	_site_name      varchar(128) charset 'utf8mb4',
	_site_host      varchar(32)  charset 'utf8mb4',
	_app_id         varchar(64)  charset 'utf8mb4',
	_app_secret     varchar(128) charset 'utf8mb4',
	_token          varchar(64)  charset 'utf8mb4',
	_encoding_key   varchar(64)  charset 'utf8mb4',
	_avatar_url     varchar(256) charset 'utf8mb4',
	_wxpayid        varchar(16)  charset 'utf8mb4',
	_wxpaykey       varchar(32)  charset 'utf8mb4',
	_enabled        boolean,
	_attributes     text charset 'utf8mb4'
)
BEGIN
	DECLARE num_rows integer;

	IF (_site_id = 0) THEN
		-- create a new site
		INSERT INTO sites(site_name, site_host, app_id, app_secret, `token`, encoding_key, `enabled`, avatar_url, attributes, wxpayid, wxpaykey)
		VALUES (_site_name, _site_host, _app_id, _app_secret, _token, _encoding_key, _enabled, _avatar_url, _attributes, _wxpayid, _wxpaykey);

		SELECT ROW_COUNT(), @@IDENTITY INTO num_rows, _site_id;
		IF (num_rows = 0) THEN
			SET _site_id = 0;
		ELSE
			-- create an empty token so that it could be refreshed by token manager.
			INSERT IGNORE INTO site_tokens (site_id, token_type, `token`, created_by)
			VALUES (_site_id, /* access token */1, 'initial invalid token', 'system');
		END IF;
	ELSE
		UPDATE sites SET
			site_name    = _site_name,
			site_host    = _site_host,
			app_id       = _app_id,
			app_secret   = _app_secret,
			`token`      = _token,
			encoding_key = _encoding_key,
			avatar_url   = _avatar_url,
			`enabled`    = _enabled,
			attributes   = _attributes,
			wxpayid      = _wxpayid,
			wxpaykey     = _wxpaykey,
			time_updated = now()
		WHERE site_id = _site_id;
	END IF;

	SELECT _site_id AS site_id;
END//

DELIMITER ;