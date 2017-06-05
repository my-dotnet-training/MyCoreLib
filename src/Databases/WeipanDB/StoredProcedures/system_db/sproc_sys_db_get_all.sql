
DROP PROCEDURE IF EXISTS sproc_sys_db_get_all;

DELIMITER //

CREATE PROCEDURE sproc_sys_db_get_all()
BEGIN

SELECT * FROM system_dbs;

END//

DELIMITER ;