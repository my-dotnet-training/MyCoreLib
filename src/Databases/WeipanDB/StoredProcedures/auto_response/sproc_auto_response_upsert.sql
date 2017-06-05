
DROP PROCEDURE IF EXISTS sproc_auto_response_upsert;

DELIMITER //

CREATE PROCEDURE sproc_auto_response_upsert(
	_site_id        integer,
	_response_id    integer,
	_response_type  varchar(16)  charset 'utf8mb4',
	_title          varchar(128) charset 'utf8mb4',
	_content        text         charset 'utf8mb4',
	_keywords       text         charset 'utf8mb4',
	_created_by     integer,
	_enabled        boolean
)
BEGIN

	IF ( _response_id = 0 ) THEN
		INSERT INTO site_auto_responses(site_id, response_type, `title`, content, keywords, created_by, updated_by, `enabled`)
		VALUES (_site_id, _response_type, _title, _content, _keywords, _created_by, _created_by, _enabled);

		SELECT LAST_INSERT_ID() AS response_id;
	ELSE
		UPDATE site_auto_responses
		SET
			response_type = _response_type,
			title       = _title,
			content     = _content,
			keywords    = _keywords,
			updated_by  = _created_by,
			`enabled`   = _enabled,
			time_updated = now()
		WHERE site_id = _site_id AND response_id = _response_id;

		-- Returns the id of the updated item
		SELECT _response_id AS response_id;
	END IF;

	

END//

DELIMITER ;