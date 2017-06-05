
CREATE TABLE IF NOT EXISTS system_id_generators (
	`process`       varchar(64)  NOT NULL,
	`type`          integer      NOT NULL,
	`scope`         integer      NOT NULL,
	`machine`       varchar(128) NOT NULL,
	`valid_until`   datetime     NOT NULL,
	`time_created`	datetime     NOT NULL,

	CONSTRAINT pk_system_id_generators PRIMARY KEY (`process`),
	CONSTRAINT uk_system_id_generators_scope UNIQUE (`type`, `scope`),
	INDEX ix_system_id_generators_valid_until (`type`, `valid_until`)
)
CHARACTER SET = 'utf8mb4';
