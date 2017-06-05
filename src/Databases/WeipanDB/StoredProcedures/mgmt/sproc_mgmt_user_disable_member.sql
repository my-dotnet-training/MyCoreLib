
DROP PROCEDURE IF EXISTS sproc_mgmt_user_disable_member;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_disable_member(
	_member_id  integer
)
BEGIN
	UPDATE `mgmt_users` SET `enabled` = false WHERE `member_id` = _member_id;
END//

DELIMITER ;