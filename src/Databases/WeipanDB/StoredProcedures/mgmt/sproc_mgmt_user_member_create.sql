
DROP PROCEDURE IF EXISTS sproc_mgmt_user_member_create;

DELIMITER //

CREATE PROCEDURE sproc_mgmt_user_member_create(
	_user_id          integer,
	_member_id        integer,
	_center_id        integer,
	_parent_member_id integer,
	_member_name      varchar(64),
	_level integer)
BEGIN

	INSERT IGNORE INTO mgmt_user_members (`user_id`,  member_id, center_id, parent_member_id, member_name, level)
	VALUES(_user_id, _member_id, _center_id, _parent_member_id, _member_name, _level);

	SELECT ROW_COUNT() as row_num;

END//

DELIMITER ;