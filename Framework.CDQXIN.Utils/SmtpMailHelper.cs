using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    /// <summary>
    /// 采用Smtp协议发送邮件
    /// </summary>
    public class SmtpMailHelper
    {

        public async static void SendMail(string host, int port, string account, string password, MailMessage message)
        {
            SmtpClient client = new SmtpClient(host, port);
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(account, password);
            //(gmail:587)
            if (port != 25)
            {
                client.EnableSsl = true;
            }

            client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);

            await client.SendMailAsync(message);
        }

        /// <summary>
        /// 邮件发送完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {

            return;
        }
    }
}
