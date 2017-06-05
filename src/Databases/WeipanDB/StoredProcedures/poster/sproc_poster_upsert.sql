
DROP PROCEDURE IF EXISTS sproc_poster_upsert;

DELIMITER //

CREATE PROCEDURE sproc_poster_upsert(
	_site_id        integer,
	_poster_name    varchar(64)  charset 'utf8mb4',
	_message        varchar(128) charset 'utf8mb4',
	_metadata       text         charset 'utf8mb4',
	_poster_image   mediumblob,
	_content_type   varchar(32)  charset 'utf8mb4',
	_current_user   integer,
	_enabled        boolean
)
BEGIN
	INSERT INTO site_posters(site_id, poster_name, `message`, `metadata`, poster_image, content_type, created_by, updated_by, `enabled`)
		VALUES (_site_id, _poster_name, _message, _metadata, _poster_image, _content_type, _current_user, _current_user, _enabled)
		ON DUPLICATE KEY UPDATE
			`message`    = _message,
			`metadata`   = _metadata,
			poster_image = _poster_image,
			content_type = _content_type,
			updated_by   = _current_user,
			`enabled`    = _enabled,
			time_updated = now();
END//

DELIMITER ;