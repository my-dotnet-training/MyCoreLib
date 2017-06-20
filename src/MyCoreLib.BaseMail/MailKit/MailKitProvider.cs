using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseMail.MailKit
{
    public class MailKitProvider : IMailProvider
    {
        private MailSetting _setting;
        public MailSetting Setting
        {
            get { return _setting; }
        }

        public void Init(MailSetting setting)
        {
            _setting = setting;
        }

        public bool Send(MailEntity entity)
        {
            using (var _client = MailKitTool.CreatClient(_setting))
            {
                MailKitTool.Send(_client, MailKitTool.CreateMessage(entity));
                _client.Disconnect(true);
            }
            return false;
        }
    }
}
