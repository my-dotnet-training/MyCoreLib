insert into `mgmt_user_members`(user_id, member_id, center_id) 
	select user_id, member_id, center_id from `mgmt_users`;

UPDATE `mgmt_user_members` SET level=99 where center_id=0 and member_id=0;