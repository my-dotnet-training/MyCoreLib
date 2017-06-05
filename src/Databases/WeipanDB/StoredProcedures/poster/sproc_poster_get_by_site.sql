
DROP PROCEDURE IF EXISTS sproc_poster_get_by_site;

DELIMITER //

CREATE PROCEDURE sproc_poster_get_by_site(
	_site_id integer
)
BEGIN

	-- do not return poster_image column as it might be huge!!!
	SELECT
		site_id, poster_name, message, metadata,
		created_by, updated_by, enabled, time_created, time_updated
	FROM site_posters
	WHERE site_id = _site_id;

END//

DELIMITER ;