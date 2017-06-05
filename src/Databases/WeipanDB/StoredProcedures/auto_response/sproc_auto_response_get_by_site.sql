
DROP PROCEDURE IF EXISTS sproc_auto_response_get_by_site;

DELIMITER //

CREATE PROCEDURE sproc_auto_response_get_by_site(
	_site_id integer
)
BEGIN

	SELECT *
	FROM site_auto_responses
	WHERE site_id = _site_id;

END//

DELIMITER ;