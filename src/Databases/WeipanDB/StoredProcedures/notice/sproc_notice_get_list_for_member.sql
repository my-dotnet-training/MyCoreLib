DROP PROCEDURE IF EXISTS  sproc_notice_get_list_for_member;

DELIMITER //

CREATE PROCEDURE sproc_notice_get_list_for_member(
	_user_Id				integer,
	_received_notice_levels varchar(64)  charset 'utf8mb4'
)
BEGIN	
	DECLARE _sql varchar(500);
	set _sql=CONCAT('select * from  ( ');

	set _sql=CONCAT(_sql,'SELECT DISTINCT n.* FROM notice n INNER JOIN notice_received_level r ON n.notice_id=r.notice_id AND r.received_notice_level in(',_received_notice_levels,')');
	set _sql=CONCAT(_sql,' LEFT OUTER JOIN (SELECT * FROM notice_read_record WHERE user_id=',_user_Id,') d ');	
	set _sql=CONCAT(_sql,'	ON n.notice_id=d.notice_id 	');
	set _sql=CONCAT(_sql,'	WHERE `status`=0 AND n.user_type=2 AND d.notice_read_record_id IS NULL ORDER BY n.time_created DESC ');	
	
	set _sql=CONCAT(_sql,' ) x where x.created_user_id<>',_user_Id);
	set @sql = _sql;
	prepare s2 from @sql;
	execute s2;

END//

DELIMITER ;