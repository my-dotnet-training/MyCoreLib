
SET autocommit = 0;

BEGIN;

/*
 * Set to invalid firstly.
 */
UPDATE `userdb`.`members` SET `level` = 0 WHERE `member_id` > 100000;


/*
 * Import 1st level members
 */
INSERT INTO `userdb`.`members` (`member_name`, `ref_member_id_v2`)
SELECT u.`userno`, m.`id`
FROM `hmgj`.`wp_member` m INNER JOIN `hmgj`.`wp_user` u ON m.`id` = u.`id`
WHERE
	u.`rid` = 3
	AND m.`id` != 1 /* admin */
	AND m.`id` != 895 /* test member */
	AND m.`company` NOT LIKE '%测试%'
ON DUPLICATE KEY UPDATE `level` = 1, `ref_member_id_v2` = m.`id`;


/*
 * Import 2nd level members
 */
INSERT INTO `userdb`.`members` (`member_name`, `ref_member_id_v2`)
SELECT u.`userno`, child.`id`
FROM
	`hmgj`.`wp_member` parent
	INNER JOIN `hmgj`.`wp_member` child ON child.`parent_broker_id` = parent.`id`
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = child.`id`
	INNER JOIN `userdb`.`members` dest ON dest.`ref_member_id_v2` = child.`parent_broker_id`
WHERE u.`rid` = 4
ON DUPLICATE KEY UPDATE `level` = 2, `ref_member_id_v2` = child.`id`;

/* Special case for 1920: 辉老板经济会员一 */
INSERT INTO `userdb`.`members` (`ref_member_id_v2`, `member_name`)
SELECT m.`id`, u.`userno`
FROM
	`hmgj`.`wp_member` m
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = m.`id`
	INNER JOIN `userdb`.`members` dest ON dest.`ref_member_id_v2` = m.`settler_id`
WHERE u.`rid` = 4 AND m.`id` = 1920 AND m.`settler_id` = 1996
ON DUPLICATE KEY UPDATE `level` = 2, `ref_member_id_v2` = m.`id`;


/*
 * Import 3rd level members
 */
INSERT INTO `userdb`.`members` (	 `member_name`,  `ref_member_id_v2`)
SELECT
	u.`userno`,child.`id`
FROM
	`hmgj`.`wp_member` parent
	INNER JOIN `hmgj`.`wp_member` child ON child.`wechat_member_id` = parent.`id`
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = child.`id`
	INNER JOIN `userdb`.`members` dest ON dest.`ref_member_id_v2` = child.`wechat_member_id`
WHERE u.`rid` = 5
ON DUPLICATE KEY UPDATE `level` = 3, `ref_member_id_v2` = child.`id`;

/* Special case for 2887: 修云坤 */
INSERT INTO `userdb`.`members` (`ref_member_id_v2`, `member_name`)
SELECT m.`id`, u.`userno`
FROM
	`hmgj`.`wp_member` m
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = m.`id`
	INNER JOIN `userdb`.`members` dest ON dest.`ref_member_id_v2` = 1929
WHERE u.`rid` = 5 AND m.`id` = 2887 AND m.`parent_broker_id` = 2326 AND m.`wechat_member_id` = 2886 AND m.`settler_id` = 963
ON DUPLICATE KEY UPDATE `level` = 3, `ref_member_id_v2` = m.`id`;

/* Special case for 1199: 何鑫 */
INSERT INTO `userdb`.`members` (`ref_member_id_v2`, `member_name`)
SELECT child.`id`, u.`userno`
FROM
	`hmgj`.`wp_member` parent
	INNER JOIN `hmgj`.`wp_member` child ON child.`parent_broker_id` = parent.`id`
	INNER JOIN `hmgj`.`wp_user` u ON u.`id` = child.`id`
	INNER JOIN `userdb`.`members` dest ON dest.`ref_member_id_v2` = child.`parent_broker_id`
WHERE u.`rid` = 5 AND child.`id` = 1199 AND child.`settler_id` = 963
ON DUPLICATE KEY UPDATE `level` = 3, `ref_member_id_v2` = child.`id`;



/* Build member parents */
DELETE FROM `member_parents`;

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

SELECT count(*)
FROM `userdb`.`members`;

SELECT count(*)
FROM `userdb`.`member_refs`;

SELECT count(*)
FROM `userdb`.`members` m
INNER JOIN `userdb`.`member_refs` r ON m.`member_id` = r.`member_id` AND m.`ref_member_id_v2` = r.`member_ref_id`;


SELECT `level`, count(*) FROM `userdb`.`members` GROUP BY `level`;


ROLLBACK;