
DROP PROCEDURE IF EXISTS sproc_mgmt_user_get_member_list;

DELIMITER //
CREATE PROCEDURE sproc_mgmt_user_get_member_list(
	_user_id	integer,
 	_offset  	integer,
	_limit   	integer)
BEGIN
	SELECT  SQL_CALC_FOUND_ROWS mum.*,mu.user_name
    FROM `mgmt_user_members` mum 
    inner join `mgmt_users` mu on  mum.user_id = mu.user_id 
    where mum.user_id = _user_id
    LIMIT _offset,_limit;
    
	SELECT FOUND_ROWS() as total_rows;
    
END//
DELIMITER ;
