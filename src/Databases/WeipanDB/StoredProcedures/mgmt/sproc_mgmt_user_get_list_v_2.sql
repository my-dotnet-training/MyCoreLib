DROP PROCEDURE IF EXISTS sproc_mgmt_user_get_list_v_2;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_get_list_v_2(
	_user_name  varchar(64),
	_member_name  varchar(64),
	_level  integer,
	_offset  integer,
	_limit   integer)
BEGIN
    IF( LENGTH(_user_name)>0 && LENGTH(_member_name)>0 ) THEN
		IF _level>=0 Then
			SELECT distinct SQL_CALC_FOUND_ROWS mu.* 
			FROM `mgmt_user_members` mum 
			inner join `mgmt_users` mu on  mum.user_id = mu.user_id 
			where (mu.user_name = _user_name or instr(mu.user_name ,_user_name) >0 )
			and (mum.member_name = _member_name or instr(mum.member_name ,_member_name) >0 )
            and mum.level = _level;
		ELSE
			SELECT distinct SQL_CALC_FOUND_ROWS mu.* 
			FROM `mgmt_user_members` mum 
			inner join `mgmt_users` mu on  mum.user_id = mu.user_id 
			where (mu.user_name = _user_name or instr(mu.user_name ,_user_name) >0 )
			and (mum.member_name = _member_name or instr(mum.member_name ,_member_name) >0 );
		END IF;
	ELSE
		IF( LENGTH(_user_name)>0 ) THEN
			IF _level>=0 Then
				SELECT distinct SQL_CALC_FOUND_ROWS mu.* 
				FROM `mgmt_user_members` mum 
				inner join `mgmt_users` mu on  mum.user_id = mu.user_id 
				where (mu.user_name = _user_name or instr(mu.user_name ,_user_name) >0 )
				and mum.level = _level;        
			ELSE
				SELECT distinct SQL_CALC_FOUND_ROWS mu.* 
				FROM `mgmt_user_members` mum 
				inner join `mgmt_users` mu on  mum.user_id = mu.user_id 
				where (mu.user_name = _user_name or instr(mu.user_name ,_user_name) >0 );
			END IF;
		ELSE
			IF( LENGTH(_member_name)>0 ) THEN
				IF _level>=0 Then
					SELECT distinct SQL_CALC_FOUND_ROWS mu.* 
					FROM `mgmt_user_members` mum 
					inner join `mgmt_users` mu on  mum.user_id = mu.user_id 
					where (mum.member_name = _member_name or instr(mum.member_name ,_member_name) >0 )
					and mum.level = _level;        
				ELSE
					SELECT distinct SQL_CALC_FOUND_ROWS mu.* 
					FROM `mgmt_user_members` mum 
					inner join `mgmt_users` mu on  mum.user_id = mu.user_id 
					where (mum.member_name = _member_name or instr(mum.member_name ,_member_name) >0 );
				END IF;
			ELSE            
				IF _level>=0 Then
					SELECT distinct SQL_CALC_FOUND_ROWS mu.* 
					FROM `mgmt_user_members` mum 
					inner join `mgmt_users` mu on  mum.user_id = mu.user_id 
					where mum.level = _level;        
				ELSE
					SELECT distinct SQL_CALC_FOUND_ROWS mu.* 
					FROM `mgmt_user_members` mum 
					inner join `mgmt_users` mu on  mum.user_id = mu.user_id ;
				END IF;
			END IF;
		END IF;
	END IF;
	SELECT FOUND_ROWS() as total_rows;   
END//

DELIMITER ;