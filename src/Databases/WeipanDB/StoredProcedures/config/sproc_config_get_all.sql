
DROP PROCEDURE IF EXISTS sproc_config_get_all;

DELIMITER //

CREATE PROCEDURE sproc_config_get_all()
BEGIN

	SELECT `key`, `value` FROM configs;

END//

DELIMITER ;