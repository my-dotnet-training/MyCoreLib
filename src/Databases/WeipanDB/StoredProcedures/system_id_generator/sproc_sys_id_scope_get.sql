
DROP PROCEDURE IF EXISTS sproc_sys_id_scope_get;

DELIMITER //

CREATE PROCEDURE sproc_sys_id_scope_get(
	_machine            varchar(128) charset 'utf8mb4',
	_process            varchar(64)  charset 'utf8mb4',
	_type               integer,
	_max_scope          integer,
	_valid_until        datetime,
	_current_time       datetime
)
BEGIN

DECLARE _scope integer;
DECLARE _time_expired datetime;

SELECT IFNULL(MAX(`scope`), 0) + 1 INTO _scope
	FROM `system_id_generators`
	WHERE `type` = _type
	FOR UPDATE; -- Make sure the table is locked for the session.

IF (_scope <= _max_scope) THEN
	-- Create a record
	INSERT INTO `system_id_generators` (`process`, `type`, `scope`, `machine`, `valid_until`, `time_created`)
	VALUES (_process, _type, _scope, _machine, _valid_until, _current_time);
ELSE

	-- We need to reuse one which are already expired.
	SELECT `scope` INTO _scope FROM `system_id_generators`
		WHERE `type` = _type AND `valid_until` < _current_time
		ORDER BY `valid_until` asc
		LIMIT 1;

	IF (0 = ROW_COUNT()) THEN
		-- In this case, it's a critial fail. All scopes are in use.
		-- The application layer MUST raise the situation to the operation team.
		SET _scope = -1;
	ELSE

		UPDATE system_id_generators SET
			`process`     = _process,
			`machine`     = _machine,
			`valid_until` = _valid_until
		WHERE `type` = _type AND `scope` = _scope;

	END IF;

END IF;

SELECT _scope;

END//

DELIMITER ;