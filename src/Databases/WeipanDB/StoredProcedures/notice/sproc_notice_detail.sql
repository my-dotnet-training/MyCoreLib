
DROP PROCEDURE IF EXISTS sproc_notice_detail;

DELIMITER //

CREATE PROCEDURE sproc_notice_detail( 
	 _notice_id		integer,
	 _user_type		integer,
	 _received_notice_levels  varchar(64)  charset 'utf8mb4'
)
BEGIN
	DECLARE _sql varchar(500);
	 
	IF (_user_type>0 AND _received_notice_levels<>'') THEN -- get the detail notice for center admins,or other members
		set _sql=CONCAT('SELECT DISTINCT n.* FROM notice n INNER JOIN notice_received_level r ON n.notice_id=r.notice_id '); 
		set _sql=CONCAT(_sql,'	WHERE  n.notice_id=',_notice_id,' AND  `status`=0 AND n.user_type=2 AND r.received_notice_level in(',_received_notice_levels,') ORDER BY n.time_created DESC ');	
	ELSE -- get the detal notice for admin
		set _sql=CONCAT('SELECT * FROM  notice  WHERE notice_id=',_notice_id,' AND `status`=0 ');  

	END IF;

	set @sql = _sql;
	prepare s2 from @sql;
	execute s2;

END//

DELIMITER ;