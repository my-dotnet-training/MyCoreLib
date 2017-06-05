
INSERT INTO `orders_v2` (
	`order_id`, `user_id`, `center_id`, `p1`, `p2`, `p3`, `p4`,
	`status`, `error_code`, `buy_up`, `cid`, `order_duration`, `amount`, `charge_rate`, `profit_rate`,
	`create_price`, `settle_price`, `fee`, `profit`,
	`time_initiated`, `expected_settle_time`, `actual_settle_time`, `description`, `time_created`, `time_updated` )
SELECT
	d.`order_id`, d.`user_id`, d.`center_id`, m.`p1`, m.`p2`, m.`p3`, m.`p4`,
	d.`status`, d.`error_code`, d.`buy_up`, d.`cid`, d.`order_duration`, d.`amount`, d.`charge_rate`, d.`profit_rate`,
	d.`create_price`, d.`settle_price`, d.`fee`, d.`profit`,
	d.`time_initiated`, d.`expected_settle_time`, d.`actual_settle_time`, d.`description`, d.`time_created`, d.`time_updated`
FROM
	`orders` d
	INNER JOIN `users` u ON d.`user_id` = u.`user_id`
	INNER JOIN `members` m ON u.`parent_member` = m.`member_id` AND d.`member_id` = m.`p1`;