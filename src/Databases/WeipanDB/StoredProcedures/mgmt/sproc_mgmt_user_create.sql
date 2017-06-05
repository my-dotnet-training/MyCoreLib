
DROP PROCEDURE IF EXISTS sproc_mgmt_user_create;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_create(
	_user_name          varchar(64) charset 'utf8mb4',
	_cell_phone         varchar(32) charset 'utf8mb4',
	_display_name       varchar(64) charset 'utf8mb4',
	_password           varchar(64) charset 'utf8mb4',
	_salt               varchar(16) charset 'utf8mb4',
	_created_by         integer,
	_global_permissions bigint,
	_center_id          integer,
	_center_permissions bigint,
	_member_id          integer,
	_member_permissions bigint,
	_attrs              text charset 'utf8mb4')
BEGIN
	DECLARE _user_id integer;
	DECLARE _num_rows integer;

	INSERT IGNORE INTO mgmt_users (`user_name`, cell_phone, display_name, `password`, salt, global_permissions, `enabled`, center_id, center_permissions, member_id, member_permissions, created_by, attributes)
	VALUES(_user_name, _cell_phone, _display_name, _password, _salt, _global_permissions, true, _center_id, _center_permissions, _member_id, _member_permissions, _created_by, _attrs);

	SELECT ROW_COUNT() INTO _num_rows;
	SELECT @@IDENTITY INTO _user_id;

	IF (_num_rows > 0) THEN
		SELECT _user_id AS user_id;
	END IF;
END//

DELIMITER ;