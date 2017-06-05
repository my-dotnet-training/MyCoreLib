
DROP PROCEDURE IF EXISTS sproc_mgmt_user_reset_password;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_reset_password(
	_user_name             varchar(64),
	_cell_phone            varchar(32),
	_updated_password_hash varchar(64) charset 'utf8mb4',
	_salt                  varchar(16) charset 'utf8mb4')
BEGIN
	UPDATE `mgmt_users`
	SET `password` = _updated_password_hash, salt = _salt
	WHERE `user_name` = _user_name
		AND `cell_phone` IS NOT NULL
		AND `cell_phone` = _cell_phone;

	SELECT ROW_COUNT();
END//

DELIMITER ;