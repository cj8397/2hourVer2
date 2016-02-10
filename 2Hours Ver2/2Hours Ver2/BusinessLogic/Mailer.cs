using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using _2Hours_Ver2.ViewModel;

namespace inclass2_aspnet2.BusinessLogic
{
    public class Mailer
    {
        public const string SUCCESS
        = "Your email has been sent.  Please allow up to 48 hrs for a reply.";
        public const string FAILURE = "Failure sending mail.";

        public const string MAIL_SUBJECT = "Confirm Registeration";
        public const string CONFIRM_BODY = "Please click here to confirm your registeration";

        //const string TO = "hassanhosseinpoor@yahoo.com"; // Specify where you want this email sent.
        // This value may/may not be constant.
        // To get started use one of your email 
        // addresses.
        public string EmailFromArvixe(Message message)
        {

            // Use credentials of the Mail account that you created with the steps above.
            const string FROM = "Hassan1@h-hosseinpour.com";
            const string FROM_PWD = "tv20021809";
            const bool USE_HTML = true;
            string TO = message.Sender;

            // Get the mail server obtained in the steps described above.
            const string SMTP_SERVER = "143.95.249.35";
            try
            {
                MailMessage mailMsg = new MailMessage(FROM, TO);
                //mailMsg.Subject = message.Subject;
                mailMsg.Subject = MAIL_SUBJECT;
                //mailMsg.Body = message.Body + "<br/>sent by: " + message.Sender;
                mailMsg.Body = CONFIRM_BODY + "<br/>sent by: " + message.Sender;
                mailMsg.IsBodyHtml = USE_HTML;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 25;
                smtp.Host = SMTP_SERVER;
                smtp.Credentials = new System.Net.NetworkCredential(FROM, FROM_PWD);
                smtp.Send(mailMsg);


            }
            catch//(System.Exception ex)
            {
                //return ex.Message;
                return FAILURE;
            }
            return SUCCESS;
        }

    }
}