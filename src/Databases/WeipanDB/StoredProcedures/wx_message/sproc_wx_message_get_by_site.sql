
DROP PROCEDURE IF EXISTS sproc_wx_message_get_by_site;

DELIMITER //

CREATE PROCEDURE sproc_wx_message_get_by_site(
	_site_id integer)
BEGIN

	SELECT * FROM site_wx_messages WHERE site_id = _site_id;

END//

DELIMITER ;