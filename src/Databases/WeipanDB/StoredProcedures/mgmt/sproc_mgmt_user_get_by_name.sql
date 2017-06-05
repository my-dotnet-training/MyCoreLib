
DROP PROCEDURE IF EXISTS sproc_mgmt_user_get_by_name;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_get_by_name(
	_user_name varchar(64) charset 'utf8mb4',
	_freeze_seconds integer)
BEGIN
	IF ( _freeze_seconds > 0 ) THEN
		-- If _freeze_seconds is greater than zero, then the account will be froze for _freeze_seconds seconds.
		
		SELECT *
			FROM mgmt_users
			WHERE user_name = _user_name AND next_login_time <= now();

		IF ( FOUND_ROWS() > 0 ) THEN
			-- freeze the account then
			UPDATE mgmt_users
			SET next_login_time = DATE_ADD(now(), interval _freeze_seconds second)
			WHERE user_name = _user_name;
		END IF;

	ELSE
		SELECT * FROM mgmt_users WHERE user_name = _user_name;
	END IF;
END//

DELIMITER ;