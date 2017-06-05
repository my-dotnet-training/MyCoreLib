
DROP PROCEDURE IF EXISTS sproc_sitetoken_get;

DELIMITER //

CREATE PROCEDURE sproc_sitetoken_get(
	_site_id   integer,
	_type      integer
)
BEGIN

	SELECT * FROM site_tokens WHERE site_id = _site_id AND `token_type` = _type;

END//

DELIMITER ;