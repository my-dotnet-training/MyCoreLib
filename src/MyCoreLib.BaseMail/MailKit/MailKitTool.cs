using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace MyCoreLib.BaseMail.MailKit
{
    /// <summary>
    /// https://github.com/jstedfast/MailKit/blob/master/Documentation/Examples/SmtpExamples.cs
    /// </summary>
    internal sealed class MailKitTool
    {
        private static MailSetting m_Setting;
        public static MailSetting Setting { get { return m_Setting; } }
        public static SmtpClient CreatClient(MailSetting setting = null)
        {
            if (setting != null)
                m_Setting = setting;
            else
                setting = m_Setting;
            if (setting == null)
                throw new ArgumentNullException("setting");

            var client = new SmtpClient();
            //client.QueryCapabilitiesAfterAuthenticating = false;
            client.Connect(setting.Host, setting.Port, setting.UseSsl);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            // Note: since we don't have an OAuth2 token, disable 	
            // the XOAUTH2 authentication mechanism.     
            client.Authenticate(setting.User, setting.Password);
            return client;
        }

        public static bool Send(SmtpClient client, MimeMessage message)
        {
            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception ee)
            {

            }
            return false;
        }

        public static MimeMessage CreateMessage(MailEntity entity)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(entity.FromName, entity.FromEmail));
            message.To.Add(new MailboxAddress(entity.ToName, entity.ToEmail));
            message.Subject = entity.Title;
            message.Body = new TextPart(entity.Subtype)
            {
                Text = entity.Body
            };
            return message;
        }

        public void SendDemo()
        {
            string user = "zjlbyron@gmail.com";//替换成你的GMAIL用户名
            string password = "1987629";//替换成你的GMAIL密码
                                        //
            string host = "smtp.gmail.com";
            //
            string mailAddress = "zjlbyron@gmail.com"; //替换成你的GMAIL账户
            string ToAddress = "zjl9494@163.com";//目标邮件地址。

            //.net formwork code
            //SmtpClient smtp = new SmtpClient(host);
            //smtp.EnableSsl = true; //开启安全连接。
            //smtp.Credentials = new NetworkCredential(user, password); //创建用户凭证
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //使用网络传送
            //                                                  //创建邮件
            //MailMessage message = new MailMessage(mailAddress, ToAddress, "Test", "This is a Test Message");
            //smtp.Send(message); //发送邮件


            // MailKit code
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("zjlbyron", mailAddress));
            message.To.Add(new MailboxAddress("zjl9494", ToAddress));
            message.Subject = "Hello World - A mail from ASPNET Core";
            message.Body = new TextPart("plain")
            {
                Text = "Hello World - A mail from ASPNET Core"
            };
            using (var client = new SmtpClient())
            {
                //client.QueryCapabilitiesAfterAuthenticating = false;
                client.Connect(host, 25, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: since we don't have an OAuth2 token, disable 	
                // the XOAUTH2 authentication mechanism.     
                client.Authenticate(user, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
