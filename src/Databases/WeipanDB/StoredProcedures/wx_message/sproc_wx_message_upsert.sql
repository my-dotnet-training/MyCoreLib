
DROP PROCEDURE IF EXISTS sproc_wx_message_upsert;

DELIMITER //

CREATE PROCEDURE sproc_wx_message_upsert(
	_site_id    integer,
	_msg_type   varchar(32),
	_content    text charset 'utf8mb4',
	_created_by integer,
	_is_template_message boolean,
	_enabled    boolean)
BEGIN

	INSERT INTO site_wx_messages (site_id, msg_type, content, created_by, updated_by, is_template_message, `enabled`)
		VALUES (_site_id, _msg_type, _content, _created_by, _created_by, _is_template_message, _enabled)
		ON DUPLICATE KEY UPDATE
			content      = _content,
			updated_by   = _created_by,
			`enabled`    = _enabled,
			is_template_message = _is_template_message,
			time_updated = now();

END//

DELIMITER ;