
CREATE TABLE IF NOT EXISTS system_dbs(
	`name`          varchar(32) NOT NULL    COMMENT 'The unique name of a database instance.',
	`db_type`       int NOT NULL            COMMENT 'Database type. 1: WeipanDB, 2: WeipanDB_Slave, 3: UserDB, 4: UserDB_Slave, 5: QuotationDB.',
	`server`        varchar(64) NOT NULL    COMMENT 'Server address',
	`db_name`       varchar(64) NOT NULL    COMMENT 'Database name',
	`enabled`       boolean NOT NULL DEFAULT true,
	`time_created`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	`time_updated`  timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

	CONSTRAINT pk_system_dbs PRIMARY KEY (`name`)
)
CHARACTER SET = 'utf8mb4';


-- Add default databases
INSERT INTO `system_dbs` (`name`, `db_type`, `server`, `db_name`) VALUES
 ('wpdb',               /* weipandb           */ 1, 'localhost', 'weipandb_unittest')
,('wpdb_slave',         /* weipandb_slave     */ 2, 'localhost', 'weipandb_unittest')
,('userdb',             /* userdb             */ 3, 'localhost', 'userdb_unittest')
,('userdb_slave',       /* userdb_slave       */ 4, 'localhost', 'userdb_unittest')
,('quotation_db',       /* quotation_db       */ 5, 'localhost', 'quotationdb_unittest')
,('quotation_db_slave', /* quotation_db_slave */ 6, 'localhost', 'quotationdb_unittest')
ON DUPLICATE KEY UPDATE `time_updated` = now();