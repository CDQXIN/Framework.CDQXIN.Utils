using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    /**/
    /// <summary>  
    /// 发送邮件[you jian]的类  
    /// </summary>  
    public class SendMail
    {
        private MailMessage mailMessage;
        private SmtpClient smtpClient;
        private string password;//发件人密码[mi ma]  
        /**/
        /// <summary>  
        /// 以cacti账号发送邮件
        /// </summary>  
        /// <param name="To">收件人地址[di zhi]，多个用";"隔开</param>  
        /// <param name="Body">邮件[you jian]正文[zheng wen]</param>  
        /// <param name="Title">邮件[you jian]的主题</param>  
        public SendMail(string To, string Body, string Title)
        {
            mailMessage = new MailMessage();
            /*  这里这样写是因为可能发给多个联系人，每个地址用;号隔开*/
            string[] mailaddress = To.Split(';');
            foreach (string address in mailaddress)
            {
                if (address.Trim() != string.Empty)
                    mailMessage.To.Add(address.Trim());
            }
            //mailMessage.To.Add(To);            
            mailMessage.From = new System.Net.Mail.MailAddress("cacti@uzai.com");
            mailMessage.Subject = Title;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            this.password = "cacti.uzai";
            Console.WriteLine("send mail sucssesful");
        }


        /// <summary>  
        /// 处审核后类的实例  
        /// </summary>  
        /// <param name="To">收件人地址[di zhi]</param>  
        /// <param name="From">发件人地址[di zhi]</param>  
        /// <param name="Body">邮件[you jian]正文[zheng wen]</param>  
        /// <param name="Title">邮件[you jian]的主题</param>  
        /// <param name="Password">发件人密码[mi ma]</param>  
        public SendMail(string To, string From, string Body, string Title, string Password)
        {
            mailMessage = new MailMessage();
            /*  这里这样写是因为可能发给多个联系人，每个地址用;号隔开*/
            string[] mailaddress = To.Split(';');
            foreach (string address in mailaddress)
            {
                if (address.Trim() != string.Empty)
                    mailMessage.To.Add(address.Trim());
            }
            //mailMessage.To.Add(To);            
            mailMessage.From = new System.Net.Mail.MailAddress(From);
            mailMessage.Subject = Title;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            this.password = Password;
            Console.WriteLine("send mail sucssesful");
        }

        public SendMail(string To, string Cc, string From, string Body, string Title, string Password)
        {
            mailMessage = new MailMessage();
            mailMessage.To.Add(To);
            mailMessage.CC.Add(Cc);
            mailMessage.From = new System.Net.Mail.MailAddress(From);
            mailMessage.Subject = Title;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            this.password = Password;
            Console.WriteLine("send mail sucssesful");
        }
        /**/
        /// <summary>  
        /// 添加附件  
        /// </summary>  
        public void Attachments(string Path)
        {
            string[] path = Path.Split(',');
            Attachment data;
            ContentDisposition disposition;
            for (int i = 0; i < path.Length; i++)
            {
                data = new Attachment(path[i], MediaTypeNames.Application.Octet);//实例化[shi li hua]附件  
                disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(path[i]);//获取附件的创建日期[chuang jian ri qi]  
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(path[i]);//获取附件的修改[xiu gai]日期  
                disposition.ReadDate = System.IO.File.GetLastAccessTime(path[i]);//获取附件的读取[du qu]日期  
                mailMessage.Attachments.Add(data);//添加到附件中  
            }
        }
        /**/
        /// <summary>  
        /// 异步[yi bu]发送邮件[you jian]  
        /// </summary>  
        /// <param name="CompletedMethod"></param>  
        public void SendAsync(SendCompletedEventHandler CompletedMethod)
        {
            if (mailMessage != null)
            {
                smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, password);//设置[she zhi]发件人身份[shen fen]的票据  
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpClient.Host = "smtp." + mailMessage.From.Host;
                smtpClient.SendCompleted += new SendCompletedEventHandler(CompletedMethod);//注册[zhu ce]异步[yi bu]发送邮件[you jian]完成时的事件[shi jian]  
                smtpClient.SendAsync(mailMessage, mailMessage.Body);
            }
        }
        /**/
        /// <summary>  
        /// 发送邮件[you jian]  
        /// </summary>  
        public void Send()
        {
            if (mailMessage != null)
            {
                smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, password);//设置[she zhi]发件人身份[shen fen]的票据  
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpClient.Host = "mail." + mailMessage.From.Host;
                smtpClient.Send(mailMessage);
            }
        }
    }
}
