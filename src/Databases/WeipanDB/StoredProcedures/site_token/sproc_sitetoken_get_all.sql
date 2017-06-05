
DROP PROCEDURE IF EXISTS sproc_sitetoken_get_all;

DELIMITER //

CREATE PROCEDURE sproc_sitetoken_get_all()
BEGIN

	SELECT * FROM site_tokens;

END//

DELIMITER ;