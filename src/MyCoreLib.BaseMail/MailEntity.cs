using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseMail
{
    public class MailEntity
    {
        public string No { get; set; }
        public string Title { get; set; }
        public string Subtype { get; set; }
        public DateTime SendTime { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Body { get; set; }
    }
}
