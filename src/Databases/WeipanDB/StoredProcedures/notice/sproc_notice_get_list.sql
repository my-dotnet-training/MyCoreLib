
DROP PROCEDURE IF EXISTS sproc_notice_get_list;

DELIMITER //

CREATE PROCEDURE sproc_notice_get_list(
	_user_type	integer,
	_received_notice_levels  varchar(64)  charset 'utf8mb4',
	_offset		integer,
	_limit		integer,
	_start_Date varchar(24),
	_end_Date	 varchar(24)
)
BEGIN
	DECLARE _sql varchar(500);
	DECLARE _condition varchar(100);
	
	set _sql=' SELECT  SQL_CALC_FOUND_ROWS * FROM notice';
	IF (_start_Date IS NULL OR _end_Date IS NULL OR _start_Date<'2000-01-01' OR _end_Date <'2000-01-01') THEN -- do not include the date 

			IF( _user_type=2 AND _received_notice_levels<>'') THEN  -- the backstage query
					set _sql=CONCAT(' SELECT  SQL_CALC_FOUND_ROWS  DISTINCT n.* FROM notice  n 
									  inner join notice_received_level r on n.notice_id=r.notice_id   
									  WHERE  n.STATUS=0  and n.user_type=',_user_type,' AND  r.received_notice_level in(',_received_notice_levels,')
									  ORDER BY n.time_created desc LIMIT ',_offset,',',_limit,' ');

			ELSEIF(_user_type=1 OR _user_type=2 ) THEN -- the frontend user or admin  query
			 	set _sql=CONCAT(_sql,' WHERE  STATUS=0 AND user_type=',_user_type,' ORDER BY time_created DESC LIMIT ',_offset,',',_limit);
			 
			ELSE -- the admin query
				 	set _sql=CONCAT(_sql,' WHERE  STATUS=0  ORDER BY time_created DESC LIMIT ',_offset,',',_limit);
			END IF;
	ELSE
			set _condition=concat(' WHERE  time_created BETWEEN ',_start_Date,' and ',_end_Date,' and STATUS=0 ');
			IF(_user_type=2 and _received_notice_levels<>'') THEN  -- the backstage query
					set _sql=CONCAT(' SELECT  SQL_CALC_FOUND_ROWS  DISTINCT  n inner join notice_received_level r on n.notice_id=r.notice_id   
												WHERE n.time_created BETWEEN ',_start_Date,' and ',_end_Date,' n.STATUS=0  and n.user_type=',_user_type,' 
												AND  r.received_notice_level in(',_received_notice_levels,')	ORDER BY n.time_created desc LIMIT ',_offset,',',_limit,' ');

			ELSEIF(_user_type=1 OR _user_type=2 ) THEN -- the admin query
			 	set _sql=CONCAT(_sql,_condition,' AND user_type=',_user_type,' ORDER BY time_created DESC LIMIT ',_offset,',',_limit);
			 
			ELSE -- the admin query
				 	set _sql=CONCAT(_sql,_condition,' ORDER BY time_created DESC LIMIT ',_offset,',',_limit);
			END IF;
	END IF;

	set @sql = _sql;
	prepare s2 from @sql;
	execute s2;

	SELECT FOUND_ROWS();
END//

DELIMITER ;