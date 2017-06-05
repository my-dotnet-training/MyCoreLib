
DROP PROCEDURE IF EXISTS sproc_sys_log_clean;

DELIMITER //

CREATE PROCEDURE sproc_sys_log_clean(_start_id integer)
BEGIN

	-- delete all rows whose ids are smaller than or equal to _start_id;
	DELETE LOW_PRIORITY FROM system_logs WHERE log_id <= _start_id;
END//

DELIMITER ;