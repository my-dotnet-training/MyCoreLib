
DROP PROCEDURE IF EXISTS sproc_mgmt_user_get_list_for_member;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_get_list_for_member(
	_member_id integer,
	_offset    integer,
	_limit     integer)
BEGIN

	SELECT SQL_CALC_FOUND_ROWS * FROM `mgmt_users`
	WHERE `member_id` = _member_id
	LIMIT _offset, _limit;

	SELECT FOUND_ROWS() as total_rows;

END//

DELIMITER ;