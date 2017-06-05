
DROP PROCEDURE IF EXISTS sproc_sys_log_get_one_page;

DELIMITER //

CREATE PROCEDURE sproc_sys_log_get_one_page(_start_id integer, _limit integer)
BEGIN

	SELECT * FROM system_logs
		WHERE log_id <= _start_id
		ORDER BY log_id desc
		LIMIT _limit;

	-- Returns the total number of rows in the table
	SELECT COALESCE(MIN(log_id),0) as min_id, COALESCE(MAX(log_id),0) as max_id FROM system_logs;
END//

DELIMITER ;