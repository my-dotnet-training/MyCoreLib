
DROP PROCEDURE IF EXISTS sproc_site_get_all;

DELIMITER //

CREATE PROCEDURE sproc_site_get_all()
BEGIN

	SELECT * FROM sites;

END//

DELIMITER ;