
DROP PROCEDURE IF EXISTS sproc_notice_update_status;

DELIMITER //

CREATE PROCEDURE sproc_notice_update_status(
	_notice_id integer,
	_status integer
)
BEGIN
	UPDATE notice SET STATUS=_status where notice_id=_notice_id;
END//

DELIMITER ;