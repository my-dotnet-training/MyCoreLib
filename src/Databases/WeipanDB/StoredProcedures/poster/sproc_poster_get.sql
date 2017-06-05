
DROP PROCEDURE IF EXISTS sproc_poster_get;

DELIMITER //

CREATE PROCEDURE sproc_poster_get(
	_site_id integer,
	_poster_name varchar(64) charset 'utf8mb4'
)
BEGIN

	SELECT *
	FROM site_posters
	WHERE site_id = _site_id AND poster_name = _poster_name;

END//

DELIMITER ;