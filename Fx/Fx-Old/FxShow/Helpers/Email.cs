using System;
using System.Collections.Generic;
//using System.DirectoryServices;
using System.Net.Mail;

using FxShow.Data;

namespace FxShow.Helpers
{
    public class Email
    {
        private static int CLASS_BASE_ERRORNUMBER = FxShow.ErrorNumbers.FXSHOW;
        private const string LOG_APPNAME = Common.APP_NAME;

//        public static void SendBackdatedEmail(Helper.Request requestBackdated)
//        {
//#if TRACE
//#endif
            
//            Data.DB.UpdateProcessed(requestBackdated.RequestID);
            
//            string str = Helper.GetMembershipProperty(requestBackdated.Requestor, "givenName");
//            string str2 = Helper.GetMembershipProperty(requestBackdated.Requestor, "sn");
//            string str3 = str + " " + str2;

//            if(str3.Trim() == "")
//            {
//                str3 = "Requestor's full name not found";
//            }

//            if(requestBackdated.Email.Trim().Length < 1)
//            {
//                requestBackdated.Email = "Requestor's e-mail not found";
//            }

//            string[] strArray = new string[requestBackdated.Reports.Count];
//            Array.Copy(requestBackdated.Reports.ToArray(), strArray, requestBackdated.Reports.Count);

//            StringBuilder emailBody = new StringBuilder();

//            emailBody.Append("The following request could not be handled by MCR system and needs to be generated immediately:");
//            emailBody.Append("\r\n\r\n");
//            emailBody.Append(requestBackdated.ProductFamily);
//            emailBody.Append("\r\n");
//            emailBody.Append(str3);
//            emailBody.Append(", ");
//            emailBody.Append(requestBackdated.Email);
//            emailBody.Append(", ");
//            emailBody.Append(requestBackdated.Listbill);
//            emailBody.Append(", ");
//            emailBody.Append(requestBackdated.Office);
//            emailBody.Append(", ");

//            if ((requestBackdated.ProductFamily == "Variable Universal Life") 
//                || (requestBackdated.ProductFamily == "Indexed Universal Life"))
//            {
//                emailBody.Append(requestBackdated.PeriodStart.ToShortDateString());
//                emailBody.Append(" - ");
//            }

//            emailBody.Append(requestBackdated.PeriodEnd.ToShortDateString());
//            emailBody.Append(", ");
//            emailBody.Append("Reports: " + string.Join(",", strArray));
//            emailBody.Append(", ");
//            emailBody.Append(requestBackdated.Frequency);
//            emailBody.Append(requestBackdated.AssumePremiums ? ", Assume premiums" : "");
//            emailBody.Append("\r\n");

//            Email.SendEmail(
//                Config.Email_To, 
//                Config.Email_From, 
//                Config.Email_Subject_Backdated_Requests,
//                emailBody.ToString(), 
//                Config.Email_CC);
//#if TRACE
//#endif
//        }

//        /// <summary>
//        /// Sends an email containing the requests that need to be run Manually
//        /// </summary>
//        public static void SendManualRequestsEmail(DataTable dtRequests)
//        {
//#if TRACE
//#endif

//            try
//            {
//                SendEmail(
//                    Config.Email_To,
//                    Config.Email_From,
//                    Config.Email_Subject_ManualRequests,
//                    BuildManualRequestsEmailBody(dtRequests).ToString(),
//                    Config.Email_CC);
//            }
//            catch(Exception ex)
//            {
//                //throw new ApplicationException("Cannot send Manual Requests Email");
//            }
//#if TRACE
//#endif
//        }

//        /// <summary>
//        /// Builds an email to be sent to Client Services notifying them
//        /// of the reports they need to generate manually.
//        /// </summary>
//        private static StringBuilder BuildManualRequestsEmailBody(DataTable dtRequests)
//        {
//#if TRACE
//#endif
        
//            //Get an array of all of the request records sorted by Product Family
//            DataRow[] drRequests = dtRequests.Select("", "ProductFamily");

//            StringBuilder emailBody = new StringBuilder();
//            emailBody.Append("The following requests could not be processed by the MCR system and need to be generated manually:");
//            emailBody.Append("\r\n\r\n");

//            try
//            {
//                //Iterate through all of the requests
//                string sOldFamily = "";

//                foreach (DataRow drRequest in drRequests)
//                {
//                    //Put the product family at the top of all the requests for that family
//                    if (drRequest["ProductFamily"].ToString() != sOldFamily)
//                    {
//                        emailBody.Append("\r\n\r\n");
//                        emailBody.Append(drRequest["ProductFamily"].ToString());
//                        emailBody.Append("\r\n");
//                        sOldFamily = drRequest["ProductFamily"].ToString();
//                    }

//                    Helper.Request requestManual = (Helper.Request)drRequest["Request"];

//                    //Append the details of the request
//                    emailBody.Append(requestManual.RequestID.ToString());
//                    emailBody.Append(",");
//                    emailBody.Append(requestManual.Requestor);
//                    emailBody.Append(", ");
//                    emailBody.Append(requestManual.Email);
//                    emailBody.Append(", ");
//                    emailBody.Append(requestManual.Listbill.Trim());
//                    emailBody.Append(", ");
//                    emailBody.Append(requestManual.Office.Trim());
//                    emailBody.Append(", ");

//                    //Loop through the reports and add a comma
//                    StringBuilder sbReports = new StringBuilder();
//                    foreach(string sReport in requestManual.Reports)
//                    {
//                        sbReports.Append(sReport + ", ");
//                    }

//                    emailBody.Append(sbReports.ToString());

//                    //Only use the period starting date for VUL requests
//                    if (requestManual.ProductFamily == "Variable Universal Life")
//                    {
//                        emailBody.Append(requestManual.PeriodStart.ToShortDateString());
//                        emailBody.Append(" - ");
//                    }

//                    emailBody.Append(requestManual.PeriodEnd.ToShortDateString());
//                    emailBody.Append(", ");
//                    emailBody.Append(requestManual.Frequency);
//                    emailBody.Append(requestManual.AssumePremiums ? ", Assume premiums" : "");
//                    emailBody.Append("\r\n");
//                }

//            }
//            catch (Exception ex)
//            {
//            }

//#if TRACE
//#endif
            
//            return emailBody;       
//        }


        public static void SendEmail(string sTo, string sFrom, string sSubject, string sBody, string sCC)
        {
            using(MailMessage mailMessage = new MailMessage(sFrom, sTo, sSubject, sBody))
            {
                // TODO(crhodes): Figure out how to do format types.
                //message.BodyFormat = MailFormat.Text;
                if(sCC != "")
                {
                    mailMessage.CC.Add(sCC);                       
                }

                SendEmail(mailMessage);
            } 
        }

        public static void SendEmail(string sTo, string sFrom, string sSubject, string sBody, string sCC, List<string> attachments)
        {
            using(MailMessage mailMessage = new MailMessage(sFrom, sTo, sSubject, sBody))
            {
                //message.BodyFormat = MailFormat.Text;
                if(sCC != "")
                {
                    mailMessage.CC.Add(sCC);                       
                }
             
                foreach (string attachment in attachments)
                {
                    mailMessage.Attachments.Add(new Attachment(attachment));
                }

                SendEmail(mailMessage);
            } 
        }

        public static void SendEmail(MailMessage message)
        {
            if(Config.SMTP_Server != "")
            {
                SmtpClient smtp = new SmtpClient(Config.SMTP_Server);

                try
                {
                    smtp.Send(message);
                }
                catch(Exception ex)
                {
                    throw new ApplicationException("Cannot Send Email");
                }
            }
            else
            {
                // TODO(crhodes): Something about no SMTP
                throw new ApplicationException("SMTP Server not configured");
            }
        }

    }
}
