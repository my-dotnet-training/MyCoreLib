
DROP PROCEDURE IF EXISTS sproc_mgmt_user_update;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_update(
	_user_id            integer,
	_enabled            boolean,
	_display_name       varchar(64) charset 'utf8mb4',
	_cell_phone         varchar(32) charset 'utf8mb4',
	_password           varchar(64) charset 'utf8mb4',
	_salt               varchar(16) charset 'utf8mb4',
	_global_permissions bigint,
	_center_id          integer,
	_center_permissions bigint,
	_member_id          integer,
	_member_permissions bigint,
	_attrs              text charset 'utf8mb4')
BEGIN
	UPDATE mgmt_users SET
		`cell_phone`   = _cell_phone,
		`display_name` = _display_name,
		`enabled`      = _enabled,
		`attributes`   = _attrs,
		`global_permissions` = _global_permissions,
		`center_id`          = _center_id,
		`center_permissions` = _center_permissions,
		`member_id`          = _member_id,
		`member_permissions` = _member_permissions,
		`time_updated` = now()
	WHERE `user_id` = _user_id;

	IF ( _password IS NOT NULL ) THEN
		UPDATE mgmt_users SET `password` = _password, salt = _salt WHERE user_id = _user_id;
	END IF;
END//

DELIMITER ;