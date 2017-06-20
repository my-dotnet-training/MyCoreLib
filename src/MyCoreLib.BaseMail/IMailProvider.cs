using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseMail
{
    public interface IMailProvider
    {
        MailSetting Setting { get; }
        bool Send(MailEntity entity);
        void Init(MailSetting setting);
    }
}
