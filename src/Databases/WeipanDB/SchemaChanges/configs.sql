
CREATE TABLE IF NOT EXISTS configs(
	`key`           varchar(32) NOT NULL,
	`value`         text NOT NULL,
	`time_created`  timestamp NOT NULL DEFAULT now(),
	`time_updated`  timestamp NOT NULL DEFAULT now(),

	CONSTRAINT pk_configs PRIMARY KEY (`key`)
)
CHARACTER SET = 'utf8mb4';

INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:WithdrawEnabled', 'true');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:WechatEnabled', 'false');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:QuotationServer', '121.43.157.62');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:QuotationServerPort', '8200');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:MaxNumberOfQuotationData', '2000');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:MaxNumberOfKLines', '160');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:MaxNumberOfKLinesForDisplay', '40');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:QuotationDataIntervalInSeconds', '10');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:SupportedAmounts', '[100, 200, 400, 800, 1000]');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:MemberPromoteLinkFormat', 'http://wp.huayueshunhe.com/create/{0}');

INSERT IGNORE INTO configs(`key`, `value`) VALUES ('userlimit_configs', '{"wechat_withdraw_quota":1000, "wechat_recharge_quota":20000, "max_profit":5000, "max_loss":-5000, "max_withdraw_times":5, "max_number_of_holdings":5, "max_holding_amount":10000}');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('netease_sms_configs', '{"app_key":"aed3017383549570cbb41d44ab5bfe30", "app_secret":"7e8caf16e13e"}');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wxpay_configs', '{"app_id":"wxd10d2f7b373e642e", "app_secret":"4ba1d66554deb34f48bee5794d427434", "wxpay_mchid":"1264063101", "wxpay_signkey":"80a3003ad215416da1a942458c5e8ce8"}');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('aliyun_configs', '{"access_key_id":"NA", "access_key_secret":"NA", "oss_endpoint":"http://oss-cn-hangzhou.aliyuncs.com", "oss_cdn_host":"NA", "oss_bucket_name":"NA", "mns_endpoint":"NA", "mns_queue":"NA"}');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('baofoo_configs', '{"member_id":"100000178", "terminal_id":"10000001", "withdraw_terminal_id":"10000002", "pay_secret":"abcdefg", "recharge_fee_rate": 0.32}');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('openepay_configs', '{"merchant_id":"test_merchant_id", "sign_key":"test_sign_key", "recharge_fee_rate": 0.32}');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('useroffline_configs', '{"seconds":180}');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('juhe_sms_configs', '{ "enabled": true,"app_key": "71f5abc36dfce0ef7ae0f2892007ac1a", "app_tpl_id": "32780" ,"app_url":"http://v.juhe.cn/sms/send"}');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('sms_configs', '{"enabled": true, "enable_signup_sms": true}');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('storage_connection_string', 'DefaultEndpointsProtocol=https;AccountName=queuewp;AccountKey=z2Nv0/brhxyTm7LReTFnJEamKNKFd5C8PkFLklfltdO++ejz0lZ/glVVnRCpOKd32F58NBktVqepM8xlq+tevw==;EndpointSuffix=core.windows.net');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:DemoMode', 'false');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:SupportedVipAmounts', '[50,100,200,300,400,500,600,700,800,900,1000,2000,3000,4000,5000]');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp:QuotationServiceHost', 'qs.hmibex.com');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wp_configs', '{"withdraw_enabled":true,"auto_payout":true,"default_special_member":119159,"sign_up_enabled":true}');
INSERT IGNORE INTO configs(`key`, `value`) VALUES ('wxpay_daily_quota_configs', '{"overall_recharge_quota":300000,"overall_withdraw_quota":20000,"individual_recharge_quota":1000,"individual_withdraw_quota":1000}');
