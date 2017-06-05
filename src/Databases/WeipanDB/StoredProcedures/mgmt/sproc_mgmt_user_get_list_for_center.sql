
DROP PROCEDURE IF EXISTS sproc_mgmt_user_get_list_for_center;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_get_list_for_center(
	_center_id integer,
	_offset    integer,
	_limit     integer)
BEGIN

	SELECT SQL_CALC_FOUND_ROWS * FROM `mgmt_users`
	WHERE `center_id` = _center_id
	LIMIT _offset, _limit;

	SELECT FOUND_ROWS() as total_rows;

END//

DELIMITER ;