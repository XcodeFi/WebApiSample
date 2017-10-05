using System.Net.Mail;
using System.Net;
using System.Text;
namespace ShCore.Utility
{
    /// <summary>
    /// Mailer
    /// </summary>
    public class ShMailer
    {
        #region Properties
        private SmtpClient smtp = new SmtpClient();
        public SmtpClient Smtp
        {
            set { this.smtp = value; }
            get { return this.smtp; }
        }

        private MailMessage mail = new MailMessage();
        public MailMessage Mail
        {
            set { this.mail = value; }
            get { return this.mail; }
        }

        private string emailFrom = "linkit.vn@gmail.com";
        public string EmailFrom
        {
            set { this.emailFrom = value; }
            get { return this.emailFrom; }
        }

        private string passwordEmailFrom = "sonpcdct";
        public string PasswordEmailFrom
        {
            set { this.passwordEmailFrom = value; }
            get { return this.passwordEmailFrom; }
        }

        private string sendBy = "Mạng xã hội tin học LinkIT";
        public string SendBy
        {
            set { this.sendBy = value; }
            get { return this.sendBy; }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ShMailer()
        {
            this.Smtp.EnableSsl = true;
            this.Smtp.Host = "smtp.gmail.com";
            this.Smtp.Credentials = new NetworkCredential(emailFrom, passwordEmailFrom);

            mail.From = new MailAddress(emailFrom, sendBy);
            mail.BodyEncoding = mail.SubjectEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
        }

        /// <summary>
        /// Gửi mail
        /// </summary>
        /// <returns></returns>
        public bool Send()
        {
            try
            {
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Thực hiện gửi mail
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static bool SendMail(string title, string content,params string[] to)
        {
            var shmailer = new ShMailer();
            for (var i = 0; i < to.Length;i++ )
                shmailer.Mail.To.Add(to[i]);
            shmailer.Mail.Subject = title;
            shmailer.Mail.Body = content;
            return shmailer.Send();
        }
    }
}
