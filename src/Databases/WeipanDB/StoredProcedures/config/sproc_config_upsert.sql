
DROP PROCEDURE IF EXISTS sproc_config_upsert;

DELIMITER //

CREATE PROCEDURE sproc_config_upsert(
	_key   varchar(32) charset 'utf8mb4',
	_value text        charset 'utf8mb4'
)
BEGIN

	INSERT INTO configs (`key`, `value`)
		VALUES (_key, _value)
		ON DUPLICATE KEY UPDATE
			`value` = _value,
			time_updated = now();

END//

DELIMITER ;