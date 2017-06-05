
DROP PROCEDURE IF EXISTS sproc_mgmt_user_get_levels;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_get_levels(
	_user_id integer
)
BEGIN
	select user_id,`level` from mgmt_user_members where user_id=_user_id GROUP BY `LEVEL`;
END//

DELIMITER ;