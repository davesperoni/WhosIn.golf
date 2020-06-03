using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WhosIn
{
    public class APIErrorHandler
    {
        readonly private cMain m;
        public int APIActionCode{ get; set; } = 0;
        public string FriendlyMsg { get; set; } = "";
        private string RawStackTrace { get; set; } = "";
        private string CompanyNumber { get; set; } = "";
        private string UserName { get; set; } = "";
        private DateTime? TimeOfError { get; set; } = null;

        public APIErrorHandler(cMain mIn)
        {
            m = mIn;
        }

        public bool ErrorOccured()
        {
            return !m.IsEmpty(FriendlyMsg);
        }

        public void AddErrorFromException(Exception e)
        {
            if (e == null)
                return;
 
            ClearError();
            FriendlyMsg = e.Message;
            RawStackTrace = m.NoNull(e.StackTrace);

            FriendlyMsg = m.LeftOf(FriendlyMsg, ". See");
            CompanyNumber = "";   
            UserName = "";   
            TimeOfError = m.CurrentTime();

            WriteErrorToTextFile();  
        }

        public void AddErrorManually(string sErrMsg)
        {
            ClearError();
            FriendlyMsg = sErrMsg;
        }

        public string FormatedErrMsg(bool bFormatForHTML = true)
        {
            string sReturnMessage;
            string NEWLINE;

            if (bFormatForHTML)
            {
                NEWLINE = "<br />";
            }
            else
            {
                NEWLINE = m.NL();
            }

            sReturnMessage = "Error calling API.  API Constant: " + APIActionCode.ToString() + "." ;

            if (!m.IsEmpty(FriendlyMsg))
            {
                sReturnMessage = sReturnMessage + NEWLINE + FriendlyMsg;
            }

            return sReturnMessage;
        }

        public string FormatedSecret(bool bFormatForHTML = true)
        {
            string NEWLINE = "";

            if (bFormatForHTML)
            {
                NEWLINE = "<br />";
            }
            else
            {
                NEWLINE = m.NL();
            }
            return "Last SQL statement: " + m.LastUsedSQLStatement + NEWLINE + m.ShortenStack(RawStackTrace, bFormatForHTML);  
        }
   
        private string ErrorMsgForLogging()
        {
            string s1 = "";

            s1 = s1 + m.NL();
            s1 = s1 + m.NL();
            s1 = s1 + "-------------- Error Occurred --------------";
            s1 = s1 + m.NL();
            s1 = s1 + "ErrorMsg: " + FormatedErrMsg(false) + m.NL();
            s1 = s1 + "Company: " + CompanyNumber + m.NL();
            s1 = s1 + "UserName: " + UserName + m.NL();
            s1 = s1 + "DateOfError: " + string.Format("{0:" + c.DATE_FORMAT5 + "}", m.Now2()) + m.NL();
            s1 = s1 +  FormatedSecret(false);
            return s1;
        }

        private void ClearError()
        {
            FriendlyMsg = "";
            RawStackTrace = "";
            CompanyNumber = "";
            UserName = "";
            TimeOfError = null;
        }

        private void WriteErrorToTextFile()
        {
            if (!m.x_bWriteErrorsToTextFile)
            {
                return;
            }

            if (m.IsEmpty(m.x_sTextFileForErrors))
            {
                return;
            }

            m.WriteToTextFile(m.x_sTextFileForErrors, ErrorMsgForLogging());
        }
    }
}

