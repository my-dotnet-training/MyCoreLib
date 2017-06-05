
DROP PROCEDURE IF EXISTS sproc_mgmt_user_change_password;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_change_password(
	_user_id               integer,
	_password_hash         varchar(64) charset 'utf8mb4',
	_updated_password_hash varchar(64) charset 'utf8mb4',
	_salt                  varchar(16) charset 'utf8mb4')
BEGIN
	UPDATE mgmt_users
	SET `password` = _updated_password_hash, salt = _salt
	WHERE `user_id` = _user_id AND `password` = _password_hash;

	SELECT ROW_COUNT();
END//

DELIMITER ;