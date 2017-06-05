
DROP PROCEDURE IF EXISTS sproc_sitetoken_clean;

DELIMITER //

CREATE PROCEDURE sproc_sitetoken_clean()
BEGIN

	TRUNCATE TABLE site_tokens;

END//

DELIMITER ;