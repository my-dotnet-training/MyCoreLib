
DROP PROCEDURE IF EXISTS sproc_mgmt_user_get_by_id;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_get_by_id(_user_id integer)
BEGIN
	SELECT * FROM mgmt_users WHERE user_id = _user_id;
END//

DELIMITER ;