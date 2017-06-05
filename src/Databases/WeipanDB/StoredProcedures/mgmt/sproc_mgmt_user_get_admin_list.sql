
DROP PROCEDURE IF EXISTS sproc_mgmt_user_get_admin_list;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_get_admin_list(
	_offset  integer,
	_limit   integer)
BEGIN

SELECT * FROM `mgmt_users`
WHERE `global_permissions` != 0
LIMIT _offset, _limit;

END//

DELIMITER ;