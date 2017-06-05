
SET autocommit = 0;

BEGIN;

UPDATE `members` SET `p1` = `member_id` WHERE `parent_member_id` = 0 AND `level` = 1;

-- update 2nd level members
UPDATE `members` c INNER JOIN `members` p ON c.`parent_member_id` = p.`member_id`
SET `c`.`p1` = p.`p1`, c.`p2` = c.`member_id`
WHERE c.`level` = 2 AND p.`level` = 1 AND p.`parent_member_id` = 0 AND p.`p1` > 0 AND p.`p2` = 0;

-- update 3rd level members
UPDATE `members` c INNER JOIN `members` p ON c.`parent_member_id` = p.`member_id`
SET `c`.`p1` = p.`p1`, c.`p2` = p.`p2`, c.`p3` = c.`member_id`
WHERE c.`level` = 3 AND p.`level` = 2 AND p.`p1` > 0 AND p.`p2` > 0 AND p.`p3` = 0;

-- update 4th level members
UPDATE `members` c INNER JOIN `members` p ON c.`parent_member_id` = p.`member_id`
SET `c`.`p1` = p.`p1`, c.`p2` = p.`p2`, c.`p3` = p.`p3`, c.`p4` = c.`member_id`
WHERE c.`level` = 4 AND p.`level` = 3 AND p.`p1` > 0 AND p.`p2` > 0 AND p.`p3` > 0 AND p.`p4` = 0;

-- finally display stats data
SELECT 'total' as `name`, COUNT(1) as `count` FROM `members`
UNION ALL SELECT '1st members',      COUNT(1) FROM `members` WHERE `level` = 1
UNION ALL SELECT '1st members (p1)', COUNT(1) FROM `members` WHERE `p1` = `member_id` AND `p2` = 0
UNION ALL SELECT '2nd members',      COUNT(1) FROM `members` WHERE `level` = 2
UNION ALL SELECT '2nd members (p2)', COUNT(1) FROM `members` WHERE `p2` = `member_id` AND `p1` > 0 AND `p3` = 0
UNION ALL SELECT '3rd members',      COUNT(1) FROM `members` WHERE `level` = 3
UNION ALL SELECT '3rd members (p3)', COUNT(1) FROM `members` WHERE `p3` = `member_id` AND `p1` > 0 AND `p2` > 0 AND `p4` = 0
UNION ALL SELECT '4th members',      COUNT(1) FROM `members` WHERE `level` = 4
UNION ALL SELECT '4th members (p4)', COUNT(1) FROM `members` WHERE `p4` = `member_id` AND `p1` > 0 AND `p2` > 0 AND `p3` > 0 AND `p5` = 0
UNION ALL SELECT '5th members',      COUNT(1) FROM `members` WHERE `level` = 5
UNION ALL SELECT '5th members (p5)', COUNT(1) FROM `members` WHERE `p5` = `member_id`;

ROLLBACK;