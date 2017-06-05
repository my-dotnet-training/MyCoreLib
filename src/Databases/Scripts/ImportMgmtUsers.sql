
SET autocommit = 0;

BEGIN;

INSERT INTO `weipandb`.`mgmt_users` (
	`user_name`, `cell_phone`, `display_name`, `password`, `salt`,
	`global_permissions`, `member_id`, `member_permissions`, `created_by`)
SELECT
	u.`username`, m.`mobile`,
	IFNULL(IFNULL( m.`person_in_charge`, IFNULL(m.`real_name`, m.`company`)), u.`userno`),
	'NA', 'NA',
	0,
	r.`member_id`,
	0x7fffffffffffffff,
	1
FROM `hmgj`.`wp_user` u
	INNER JOIN `hmgj`.`wp_member` m ON u.`id` = m.`id`
	INNER JOIN `userdb`.`member_refs` r ON m.`id` = r.`member_ref_id`
WHERE u.`rid` < 6;

SELECT * FROM `weipandb`.`mgmt_users`;

ROLLBACK;