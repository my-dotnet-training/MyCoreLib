
DROP PROCEDURE IF EXISTS sproc_site_enable_disable;

DELIMITER //

CREATE PROCEDURE sproc_site_enable_disable(
	_site_id integer,
	_enabled boolean)
BEGIN

	UPDATE sites SET `enabled` = _enabled WHERE site_id = _site_id;

END//

DELIMITER ;