using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    public class MailHelper
    {
        public static bool MailSend(MyMailMessage mailMsg)
        {
            MailMessage message = WrapMailMessage(mailMsg);

            SmtpClient client = new SmtpClient();
            client.Host = mailMsg.MailDomain;
            client.Port = mailMsg.MailDomainPort;
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(mailMsg.MailServerUserName, mailMsg.MailServerPassWord);

            //(gmail:587)
            if (mailMsg.MailDomainPort != 25)
            {
                client.EnableSsl = true;
            }

            client.Send(message);

            foreach (Attachment at in mailMsg.AttachmentCollection)
            {
                at.ContentStream.Dispose();
            }

            return true;
        }

        public static bool MailSendAysnc(MyMailMessage mailMsg)
        {
            MailMessage message = WrapMailMessage(mailMsg);

            SmtpClient client = new SmtpClient(mailMsg.MailDomain, mailMsg.MailDomainPort);
            //client.Host = mailMsg.MailDomain;
            //client.Port = mailMsg.MailDomainPort;
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(mailMsg.MailServerUserName, mailMsg.MailServerPassWord);

            //(gmail:587)
            if (mailMsg.MailDomainPort != 25)
            {
                client.EnableSsl = true;
            }

            client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
            string status = "";
            client.SendAsync(message, status);

            return true;
        }

        private static MailMessage WrapMailMessage(MyMailMessage mailMsg)
        {
            MailMessage message = new MailMessage(mailMsg.From, mailMsg.From);
            message.From = new MailAddress(mailMsg.From, mailMsg.FromName);
            message.To.Clear();
            message.ReplyToList.Add(new MailAddress(mailMsg.From, mailMsg.FromName));
            if (mailMsg.To != null && mailMsg.To.Count > 0)
            {
                mailMsg.To.ForEach(p =>
                {
                    message.To.Add(new MailAddress(p.Value, p.Key));
                });
            }

            if (mailMsg.CC != null && mailMsg.CC.Count > 0)
            {
                mailMsg.CC.ForEach(p =>
                {
                    message.CC.Add(new MailAddress(p.Value, p.Key));
                });
            }

            message.SubjectEncoding = Encoding.UTF8;
            message.Subject = mailMsg.Subject;

            message.BodyEncoding = Encoding.UTF8;
            message.Body = mailMsg.Body;
            message.IsBodyHtml = mailMsg.IsHtml;
            message.Priority = mailMsg.Priority;

            if (mailMsg.AttachmentCollection != null)
            {
                foreach (Attachment at in mailMsg.AttachmentCollection)
                {
                    message.Attachments.Add(at);
                }
            }
            return message;
        }

        private static void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            return;
        }

        public static bool MailSendAysnc(EmailAccount account, MailMessage message)
        {
            SmtpClient client = new SmtpClient(account.EmailOutHost, account.EmailOutPort.Value);
            //client.Host = account.EmailOutHost;
            //client.Port = account.EmailOutPort.Value;
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(account.EmailOutLogin, account.EmailOutPassword);

            //(gmail:587)
            if (account.EmailOutPort.Value != 25)
            {
                client.EnableSsl = true;
            }

            client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
            string status = "";
            client.SendAsync(message, status);

            return true;
        }

        public static bool MailSend(EmailAccount account, MailMessage message)
        {
            SmtpClient client = new SmtpClient(account.EmailOutHost, account.EmailOutPort.Value);
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(account.EmailOutLogin, account.EmailOutPassword);
            //(gmail:587)
            if (account.EmailOutPort.Value != 25)
            {
                client.EnableSsl = true;
            }

            client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);

            client.Send(message);

            return true;
        }

    }



    #region 相关类
    public class MyMailMessage
    {
        public string From { get; set; }

        public string FromName { get; set; }

        public List<KeyValuePair<string, string>> To { get; set; }

        public List<KeyValuePair<string, string>> CC { get; set; }

        public List<string> Attachments { get; set; }

        public List<Attachment> AttachmentCollection { get; set; }

        public string AttachmentIDs { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public MailPriority Priority { get; set; }

        public bool IsHtml { get; set; }

        public string MailDomain { get; set; }

        public int MailDomainPort { get; set; }

        public string MailServerUserName { get; set; }

        public string MailServerPassWord { get; set; }

        public int BPID { get; set; }

        public int CreatedID { get; set; }

        public string CreatedName { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class EmailAccount
    {
        public int EmailAccountID { get; set; }
        public int BPID { get; set; }
        public int UserID { get; set; }
        public string EmailUserName { get; set; }
        public string Email { get; set; }
        public int EmailProtocalID { get; set; }
        public string EmailInHost { get; set; }
        public string EmailInLogin { get; set; }
        public string EmailInPassword { get; set; }
        public int? EmailInPort { get; set; }
        public string EmailOutHost { get; set; }
        public string EmailOutLogin { get; set; }
        public string EmailOutPassword { get; set; }
        public int? EmailOutPort { get; set; }
        public bool? HasSignature { get; set; }
        public string EmailSignature { get; set; }
        public bool? IsAutoResponse { get; set; }
        public int? ResponseMessageDataID { get; set; }
        public DateTime? ResponseStartDate { get; set; }
        public DateTime? ResponseEndDate { get; set; }
        public int CreatedID { get; set; }
        public string CreatedName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedID { get; set; }
        public string UpdatedName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsDefaultEmail { get; set; }
        public bool IsActive { get; set; }

    }
    #endregion


}
