
DROP PROCEDURE IF EXISTS sproc_mgmt_user_is_manage_member;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_is_manage_member(
	_user_id   integer,
	_member_id          integer
)
BEGIN

SELECT user_id FROM `mgmt_user_members`
WHERE `user_id` = _user_id AND `member_id` = _member_id;

END//

DELIMITER ;