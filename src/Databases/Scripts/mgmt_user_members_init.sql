insert into `weipandb`.`mgmt_user_members`(user_id, member_id, center_id) select user_id, member_id, center_id from `weipandb`.`mgmt_users`;


UPDATE `weipandb`.`mgmt_user_members` mum INNER JOIN `userdb`.`members` m ON mum.member_id=m.member_id
SET mum.center_id=m.center_id,mum.member_name=m.member_name;


UPDATE `weipandb`.`mgmt_user_members` mum INNER JOIN `userdb`.`members` m ON mum.member_id=m.member_id
SET mum.parent_member_id=m.parent_member_id,mum.level=m.level;

UPDATE `weipandb`.`mgmt_user_members` SET level=99 where center_id=0 and member_id=0;