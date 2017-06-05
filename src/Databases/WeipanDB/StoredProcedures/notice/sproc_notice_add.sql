
DROP PROCEDURE IF EXISTS sproc_notice_add;

DELIMITER //

CREATE PROCEDURE sproc_notice_add(
	_user_type		integer,
	_received_notice_levels    varchar(100)  charset 'utf8mb4',
	_title			varchar(200)  charset 'utf8mb4',
	_content		text  charset 'utf8mb4',
	_created_user_id integer
)
BEGIN
	declare _notice_Id integer;
	declare _next_pos integer  DEFAULT 0;
	declare _curr_pos integer DEFAULT 1;
	declare _curr_level_str varchar(200)  charset 'utf8mb4' DEFAULT '';
	declare _curr_level integer;

-- 	添加一条消息 
	INSERT INTO notice(user_type,received_notice_levels,title,content,created_user_id) VALUE(_user_type,_received_notice_levels,_title,_content,_created_user_id);
 	SET  _notice_Id = @@IDENTITY;

-- 如果是后台用户，根据层级，以逗号","遍历接收消息层级,逐级插入表 notice_received_level
	IF (_user_type=2 AND _received_notice_levels<>'') THEN
		LEVEL_ADD:WHILE(true)	
		DO 
			set _next_pos= LOCATE(',',_received_notice_levels,_curr_pos);
			IF(_next_pos>0)THEN			
				set _curr_level_str= substring(_received_notice_levels,_curr_pos,_next_pos-_curr_pos);
				set _curr_pos=_next_pos+1;
				set _curr_level= CAST(_curr_level_str  AS SIGNED);

				INSERT INTO notice_received_level(notice_id,received_notice_level) values(_notice_Id,_curr_level);
			ELSE
				set _curr_pos=_curr_pos-1;
				set _curr_level_str= right(_received_notice_levels,LENGTH(_received_notice_levels)-_curr_pos);
				set _curr_level= CAST(_curr_level_str  AS SIGNED);

				INSERT INTO notice_received_level(notice_id,received_notice_level) values(_notice_Id,_curr_level);

-- 			结束遍历
				LEAVE LEVEL_ADD;

			END IF;
		END WHILE;
	END IF;


END//

DELIMITER ;