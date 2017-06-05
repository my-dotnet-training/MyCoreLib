
SET autocommit = 0;

BEGIN;

/*
 * Import 1st level members
 */
INSERT INTO `userdb`.`members` (
	`center_id`, `parent_member_id`, `member_name`, `enabled`, `special`, 
	`num_dist_levels`, `incentive1`, `incentive2`, `incentive3`,
	`payout_ratio`, `alert_rate`, `stop_rate`, `level`, `display_name`, `attrs`, `ref_member_id`)
SELECT
	100, /*root member*/0, u.`userno`, /* enabled */true, /* non-special */false,
		/*disable dist*/0, 0, 0, 0,
		0, 40, 20, 1, m.`real_name`,
		CONCAT('{"Contact":"', TRIM(IFNULL(m.`person_in_charge`,'')), '","Company":"', TRIM(IFNULL(m.`Company`,'')), '", "Phone":"', TRIM(IFNULL(m.`mobile`,'')), '"}'),
		m.`id`
FROM `hmgj`.`wp_member` m INNER JOIN `hmgj`.`wp_user` u ON m.`id` = u.`id`
WHERE
	u.`rid` = 3
	AND m.`id` != 1 /* admin */
	AND m.`id` != 895 /* test member */
	AND m.`company` NOT LIKE '%测试%';


/*
 * Import 2nd level members
 */
INSERT INTO `userdb`.`members` (
	`center_id`, `parent_member_id`, `member_name`, `enabled`, `special`, 
	`num_dist_levels`, `incentive1`, `incentive2`, `incentive3`,
	`payout_ratio`, `alert_rate`, `stop_rate`, `level`, `display_name`, `attrs`, `ref_member_id`)
SELECT
	100, dest.`member_id`, u.`userno`, /* enabled */true, /* non-special */false,
		/*disable dist*/0, 0, 0, 0,
		0, 40, 20, 1, child.`real_name`,
		CONCAT('{"Contact":"', TRIM(IFNULL(child.`person_in_charge`,'')), '","Company":"', TRIM(IFNULL(child.`Company`,'')), '", "Phone":"', TRIM(IFNULL(child.`mobile`,'')), '"}'),
		child.`id`
FROM
	`hmgj`.`wp_member` parent
	INNER JOIN `hmgj`.`wp_member` child ON child.`parent_broker_id` = parent.`id`
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = child.`id`
	INNER JOIN `userdb`.`members` dest ON dest.`ref_member_id` = child.`parent_broker_id`
WHERE
	u.`rid` = 4;

/* Special case for 1920: 辉老板经济会员一 */
INSERT INTO `userdb`.`members` (`ref_member_id`, `center_id`, `parent_member_id`, `member_name`, `enabled`, `special`, `num_dist_levels`, `incentive1`, `incentive2`, `incentive3`, `payout_ratio`, `alert_rate`, `stop_rate`, `level`, `display_name`, `attrs`)
SELECT m.`id`, 100, dest.`member_id`, u.`userno`, /* enabled */true, /* non-special */false, /*disable dist*/0, 0, 0, 0, 0, 40, 20, 1, m.`real_name`,
		CONCAT('{"Contact":"', TRIM(IFNULL(m.`person_in_charge`,'')), '","Company":"', TRIM(IFNULL(m.`Company`,'')), '", "Phone":"', TRIM(IFNULL(m.`mobile`,'')), '"}')
FROM
	`hmgj`.`wp_member` m
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = m.`id`
	INNER JOIN `userdb`.`members` dest ON dest.`ref_member_id` = m.`settler_id`
WHERE u.`rid` = 4 AND m.`id` = 1920 AND m.`settler_id` = 1996;


/*
 * Import 3rd level members
 */
INSERT INTO `userdb`.`members` (
	`center_id`, `parent_member_id`, `member_name`, `enabled`, `special`, 
	`num_dist_levels`, `incentive1`, `incentive2`, `incentive3`,
	`payout_ratio`, `alert_rate`, `stop_rate`, `level`, `display_name`, `attrs`, `ref_member_id`)
SELECT
	100, dest.`member_id`, u.`userno`, /* enabled */true, /* non-special */false,
		/*disable dist*/0, 0, 0, 0,
		0, 40, 20, 1, child.`real_name`,
		CONCAT('{"Contact":"', TRIM(IFNULL(child.`person_in_charge`,'')), '","Company":"', TRIM(IFNULL(child.`Company`,'')), '", "Phone":"', TRIM(IFNULL(child.`mobile`,'')), '"}'),
		child.`id`
FROM
	`hmgj`.`wp_member` parent
	INNER JOIN `hmgj`.`wp_member` child ON child.`wechat_member_id` = parent.`id`
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = child.`id`
	INNER JOIN `userdb`.`members` dest ON dest.`ref_member_id` = child.`wechat_member_id`
WHERE
	u.`rid` = 5;

/* Special case for 2887: 修云坤 */
INSERT INTO `userdb`.`members` (`ref_member_id`, `center_id`, `parent_member_id`, `member_name`, `enabled`, `special`, `num_dist_levels`, `incentive1`, `incentive2`, `incentive3`, `payout_ratio`, `alert_rate`, `stop_rate`, `level`, `display_name`, `attrs`)
SELECT m.`id`, 100, dest.`member_id`, u.`userno`, /* enabled */true, /* non-special */false, /*disable dist*/0, 0, 0, 0, 0, 40, 20, 1, m.`real_name`,
		CONCAT('{"Contact":"', TRIM(IFNULL(m.`person_in_charge`,'')), '","Company":"', TRIM(IFNULL(m.`Company`,'')), '", "Phone":"', TRIM(IFNULL(m.`mobile`,'')), '"}')
FROM
	`hmgj`.`wp_member` m
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = m.`id`
	INNER JOIN `userdb`.`members` dest ON dest.`ref_member_id` = 1929
WHERE u.`rid` = 5 AND m.`id` = 2887 AND m.`parent_broker_id` = 2326 AND m.`wechat_member_id` = 2886 AND m.`settler_id` = 963;

/* Special case for 1199: 何鑫 */
INSERT INTO `userdb`.`members` (`ref_member_id`, `center_id`, `parent_member_id`, `member_name`, `enabled`, `special`, `num_dist_levels`, `incentive1`, `incentive2`, `incentive3`, `payout_ratio`, `alert_rate`, `stop_rate`, `level`, `display_name`, `attrs`)
SELECT child.`id`, 100, dest.`member_id`, u.`userno`, /* enabled */true, /* non-special */false, /*disable dist*/0, 0, 0, 0, 0, 40, 20, 1, child.`real_name`,
		CONCAT('{"Contact":"', TRIM(IFNULL(child.`person_in_charge`,'')), '","Company":"', TRIM(IFNULL(child.`Company`,'')), '", "Phone":"', TRIM(IFNULL(child.`mobile`,'')), '"}')
FROM
	`hmgj`.`wp_member` parent
	INNER JOIN `hmgj`.`wp_member` child ON child.`parent_broker_id` = parent.`id`
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = child.`id`
	INNER JOIN `userdb`.`members` dest ON dest.`ref_member_id` = child.`parent_broker_id`
WHERE u.`rid` = 5 AND child.`id` = 1199 AND child.`settler_id` = 963;


/*
 * Import normal users
 */
INSERT INTO `userdb`.`users` (
	`user_name`, `center_id`, `real_name`, `nick_name`, `id_number`, `password`, `salt`, `open_id`,
	`parent1`, `parent2`, `parent3`,
	`orig_root_member`, `current_root_member`, `parent_member`,
	`account_enabled`, `trade_enabled`, `qr_enabled`, `cash_out_enabled`, `avatar_url`, `attrs`
	)
SELECT
	m.`mobile`, 100, m.`real_name`, u.`nickname`, m.`identity_no`, 'NA', 'NA', null,
	/* no parent users */0, 0, 0, 
	root_mem.`member_id`, root_mem.`member_id`, parent_mem.`member_id`,
	true, true, false, true, u.`picture`, null
FROM
	`hmgj`.`wp_member` m
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = m.`id`
	INNER JOIN `userdb`.`members` root_mem on m.`settler_id` = root_mem.`ref_member_id` AND root_mem.`level` = 1
	INNER JOIN `userdb`.`members` parent_mem on m.`parent_broker_id` = parent_mem.`ref_member_id`
WHERE u.`rid` = 6;

/* Display the current member and users */
/*
SELECT COUNT(*) FROM `userdb`.`members`;
SELECT * FROM `userdb`.`members`;

SELECT COUNT(*) FROM `userdb`.`users`;
SELECT * FROM `userdb`.`users`;
*/

/* Look up the members which haven't been imported */
SELECT u.`rid`, m.* FROM `wp_member` m INNER JOIN `wp_user` u ON m.id = u.`id` 
WHERE m.`id` NOT IN (SELECT `ref_member_id` FROM `userdb`.`members` ) AND m.`mobile` NOT IN (SELECT `user_name` FROM `userdb`.`users` )
AND m.`id` != 1 AND m.`id` != 895 AND m.`settler_id` != 895 AND m.`company` NOT LIKE '%测试%';



/*
 * Import member balance
 */
INSERT INTO `userdb`.`member_balance_history` (`member_id`, `type`, `balance_change`, `frozen_change`, `current_balance`, `current_frozen`, `ref_order_id`, `commit`, `description`)
SELECT mem.`member_id`, /*充值*/1, y.`balance` * 10000, 0, y.`balance` * 10000, 0, CONCAT('系统升级-', u.`id`), true, '系统升级'
FROM `userdb`.`members` mem
	INNER JOIN `hmgj`.`wp_user` u ON mem.`ref_member_id` = u.`id`
	INNER JOIN `hmgj`.`wp_money` y ON u.`id` = y.`uid`
WHERE u.`rid` < 6;

INSERT INTO `userdb`.`member_recharge_history`(`member_id`, `amount`, `total_recharge`, `total_withdraw`, `source`, `ref_order_id`, `attrs`)
SELECT mem.`member_id`, y.`balance` * 10000, y.`balance` * 10000, 0, '系统升级', CONCAT('系统升级-', u.`id`), null
FROM `userdb`.`members` mem
	INNER JOIN `hmgj`.`wp_user` u ON mem.`ref_member_id` = u.`id`
	INNER JOIN `hmgj`.`wp_money` y ON u.`id` = y.`uid`
WHERE u.`rid` < 6;

INSERT INTO `userdb`.`member_balance` (`member_id`, `current_balance`, `current_frozen`, `total_recharge`, `total_withdraw`)
SELECT mem.`member_id`, y.`balance` * 10000, 0, y.`balance` * 10000, 0
FROM `userdb`.`members` mem
	INNER JOIN `hmgj`.`wp_user` u ON mem.`ref_member_id` = u.`id`
	INNER JOIN `hmgj`.`wp_money` y ON u.`id` = y.`uid`
WHERE u.`rid` < 6;


/*
 * Import user balance
 */
INSERT INTO `userdb`.`user_balance_history` (`user_id`, `type`, `balance_change`, `frozen_change`, `current_balance`, `current_frozen`, `ref_order_id`, `commit`, `description`)
SELECT mem.`user_id`, /*充值*/1, y.`balance` * 10000, 0, y.`balance` * 10000, 0, CONCAT('系统升级-', u.`id`), true, '系统升级'
FROM `userdb`.`users` mem
	INNER JOIN `hmgj`.`wp_member` m ON mem.`user_name` = m.`mobile`
	INNER JOIN `hmgj`.`wp_user` u ON m.`id` = u.`id`
	INNER JOIN `hmgj`.`wp_money` y ON u.`id` = y.`uid`
WHERE u.`rid` = 6;

INSERT INTO `userdb`.`user_recharge_history`(`user_id`, `amount`, `total_recharge`, `total_withdraw`, `source`, `ref_order_id`, `attrs`)
SELECT mem.`user_id`, y.`balance` * 10000, y.`balance` * 10000, 0, '系统升级', CONCAT('系统升级-', u.`id`), null
FROM `userdb`.`users` mem
	INNER JOIN `hmgj`.`wp_member` m ON mem.`user_name` = m.`mobile`
	INNER JOIN `hmgj`.`wp_user` u ON m.`id` = u.`id`
	INNER JOIN `hmgj`.`wp_money` y ON u.`id` = y.`uid`
WHERE u.`rid` = 6;

INSERT INTO `userdb`.`user_balance` (`user_id`, `current_balance`, `total_recharge`)
SELECT mem.`user_id`, y.`balance` * 10000, y.`balance` * 10000
FROM `userdb`.`users` mem
	INNER JOIN `hmgj`.`wp_member` m ON mem.`user_name` = m.`mobile`
	INNER JOIN `hmgj`.`wp_user` u ON m.`id` = u.`id`
	INNER JOIN `hmgj`.`wp_money` y ON u.`id` = y.`uid`
WHERE u.`rid` = 6;


/* Build member parents */
INSERT INTO `userdb`.`member_parents` (`member_id`, `parent_member_id`, `parent_level`)
	SELECT `member_id`, `member_id`, 1
	FROM `userdb`.`members`
	WHERE `level` = 1;
INSERT INTO `userdb`.`member_parents` (`member_id`, `parent_member_id`, `parent_level`)
	SELECT `member_id`, `parent_member_id`, 1
	FROM `userdb`.`members`
	WHERE `level` = 2;
INSERT INTO `userdb`.`member_parents` (`member_id`, `parent_member_id`, `parent_level`)
	SELECT `member_id`, `parent_member_id`, 2
	FROM `userdb`.`members`
	WHERE `level` = 3;
INSERT INTO `userdb`.`member_parents` (`member_id`, `parent_member_id`, `parent_level`)
	SELECT child.`member_id`, parent.`parent_member_id`, 1
	FROM `userdb`.`members` child INNER JOIN `userdb`.`members` parent ON child.`parent_member_id` = parent.`member_id`
	WHERE child.`level` = 3;


/* Create empty balance records for members */
INSERT INTO `userdb`.`member_balance` (`member_id`, `current_balance`, `current_frozen`, `total_recharge`, `total_withdraw`)
SELECT `member_id`, 0, 0, 0, 0
FROM `userdb`.`members`
WHERE `member_id` NOT IN (SELECT `member_id` FROM `userdb`.`member_balance`);


/*
 * Confirm sum of balance 
 */
SELECT SUM(`balance`) AS `sum_balance` FROM `hmgj`.`wp_money`;
SELECT SUM(`current_balance`) AS `sum_member_balance` FROM `userdb`.`member_balance`;
SELECT SUM(`current_balance`) AS `sum_user_balance` FROM `userdb`.`user_balance`;


ROLLBACK;