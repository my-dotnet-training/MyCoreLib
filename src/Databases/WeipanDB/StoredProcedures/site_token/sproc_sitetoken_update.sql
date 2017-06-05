
DROP PROCEDURE IF EXISTS sproc_sitetoken_update;

DELIMITER //

CREATE PROCEDURE sproc_sitetoken_update(
	_site_id        integer,
	_type           integer,
	_token          varchar(256) charset 'utf8mb4',
	_created_by     varchar(32)  charset 'utf8mb4',
	_time_expired   datetime)
BEGIN

	INSERT INTO site_tokens (site_id, `token_type`, `token`, created_by, time_expired)
		VALUES(_site_id, _type, _token, _created_by, _time_expired)
		ON DUPLICATE KEY UPDATE
			`token`      = _token,
			created_by   = _created_by,
			time_expired = _time_expired,
			time_updated = now();

END//

DELIMITER ;