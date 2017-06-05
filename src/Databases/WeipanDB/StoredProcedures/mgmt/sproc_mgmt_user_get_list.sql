
DROP PROCEDURE IF EXISTS sproc_mgmt_user_get_list;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_get_list(
	_offset  integer,
	_limit   integer)
BEGIN

	SELECT SQL_CALC_FOUND_ROWS *
	FROM `mgmt_users`
	LIMIT _offset, _limit;

	SELECT FOUND_ROWS() as total_rows;

END//

DELIMITER ;