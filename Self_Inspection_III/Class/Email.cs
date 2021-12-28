using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;

namespace Self_Inspection.Class
{
    [Serializable]
    public class Email
    {
        private string m_Active;
        private string m_Name;
        private string m_Title;
        private string m_Address;

        public Email() { }
        public Email(string name, string title, string address, string active)
        {
            Name = name;
            Title = title;
            Address = address;
            m_Active = active;
        }

        public bool Active
        {
            get
            {
                try { return Convert.ToBoolean(m_Active); } catch { return false; }
            }
            set => m_Active = value.ToString();
        }
        public string Name { get => m_Name ?? string.Empty; set => m_Name = value; }
        public string Title
        {
            get => m_Title ?? string.Empty;
            set
            {
                switch (value)
                {
                    case "MR": m_Title = "經理"; break;
                    case "CL": m_Title = "課長"; break;
                    case "TL": m_Title = "組長"; break;
                    case "PE": m_Title = "產品工程師"; break;
                    case "SE": m_Title = "軟體工程師"; break;
                    case "TE": m_Title = "設備工程師"; break;
                    default: m_Title = value; break;
                }
            }
        }
        public string Address { get => m_Address ?? string.Empty; set => m_Address = value; }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static void SendErrorEmail(string Function, string ErrorMsg, string ErrorDetails)
        {
            return;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
            string smtpAddress = "aprelay.chroma.com.tw";
            int portNumber = 25;
            bool enableSSL = false;
            string emailFrom = "Self_Inspection@chroma.com.tw";
            string password = "";
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add("gary.huang@chroma.com.tw");
                mail.Subject = "Device Monitor Error :" + DateTime.Now.ToString();
                mail.Body = Function + ": " + ErrorMsg + "\n\n" + ErrorDetails;
                mail.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.UseDefaultCredentials = true;
                    smtp.EnableSsl = enableSSL;
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString(), "信件異常");
                    }
                }
            }
        }
    }
}
