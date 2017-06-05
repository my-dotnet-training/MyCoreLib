
DROP PROCEDURE IF EXISTS sproc_auto_response_get;

DELIMITER //

CREATE PROCEDURE sproc_auto_response_get(
	_site_id integer,
	_response_id integer
)
BEGIN

	SELECT *
	FROM site_auto_responses
	WHERE site_id = _site_id AND response_id = _response_id;

END//

DELIMITER ;