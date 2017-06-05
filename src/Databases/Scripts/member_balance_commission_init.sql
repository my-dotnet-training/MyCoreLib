update member_balance mb set total_commission =  ifnull(
(
	select SUM(o2.amount) * 0.02
	from orders_v2 o2 
	INNER JOIN users u on o2.user_id = u.user_id
	where u.current_root_member = mb.member_id
	group by u.current_root_member
), 0);

update member_balance set current_balance = current_balance + total_commission;