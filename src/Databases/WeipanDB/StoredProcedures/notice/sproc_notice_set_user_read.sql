DROP PROCEDURE IF EXISTS  sproc_notice_set_user_read;

DELIMITER //

CREATE PROCEDURE sproc_notice_set_user_read(
	_notice_Id   integer,
	_user_Id   integer
)
BEGIN	
	SELECT 1 FROM notice_read_record WHERE notice_id=_notice_Id AND  user_id= _user_Id;

	IF (FOUND_ROWS() <=0) THEN
		INSERT INTO notice_read_record(user_id,user_type,notice_id) VALUES(_user_Id,2,_notice_Id);
	END IF; 
END//

DELIMITER ;