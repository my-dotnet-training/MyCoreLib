
DROP PROCEDURE IF EXISTS sproc_sys_log_add;

DELIMITER //

CREATE PROCEDURE sproc_sys_log_add(
	_tag        varchar(32) charset 'utf8mb4',
	_type       varchar(16) charset 'utf8mb4',
	_message    text charset 'utf8mb4')
BEGIN

	INSERT INTO system_logs(`tag`, `type`, `message`)
	VALUES(_tag, _type, _message);

END//

DELIMITER ;