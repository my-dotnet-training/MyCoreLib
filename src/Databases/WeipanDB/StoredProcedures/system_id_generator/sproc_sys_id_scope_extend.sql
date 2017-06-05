
DROP PROCEDURE IF EXISTS sproc_sys_id_scope_extend;

DELIMITER //

CREATE PROCEDURE sproc_sys_id_scope_extend(
	_process            varchar(64)  charset 'utf8mb4',
	_type               integer,
	_scope              integer,
	_valid_until        datetime
)
BEGIN

	UPDATE `system_id_generators`
	SET `valid_until` = _valid_until
	WHERE `process` = _process AND `type` = _type AND `scope` = _scope;

END//

DELIMITER ;