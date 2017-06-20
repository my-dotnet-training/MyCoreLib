using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseMail
{
    public class MailSetting
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
