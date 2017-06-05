
DROP PROCEDURE IF EXISTS sproc_site_check_host;

DELIMITER //

CREATE PROCEDURE sproc_site_check_host(_host varchar(32))
BEGIN

	SELECT site_id FROM sites WHERE site_host = _host LIMIT 1;

END//

DELIMITER ;