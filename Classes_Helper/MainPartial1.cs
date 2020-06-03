using Microsoft.AspNetCore.Http;
using System;
using System.Data;

namespace WhosIn
{

    public partial class cMain  
    {

        // ==================  From appsettings.json or W_SystemSettings table ==================
        public string x_sConnectionStringMain{ get; set; } = "";
        public string x_sConnectionStringNOTUSED { get; set; } = "";
        public bool x_bUseLogging { get; set; } = false;
        public bool x_bIncludeDebugInLog { get; set; } = false;
        public bool x_bIncludeSecretInfoInErrMsgToUser { get; set; } = false;
        public bool x_bDisableSystem { get; set; } = false;
        public string x_sDisableMsg { get; set; } = "";
        public string x_sCopywriteNotice { get; set; } = "";
        public string x_sProductName { get; set; } = "";
        public int x_iProductCode { get; set; } = 1;
        public string x_sContactUrl { get; set; } = "";
        public bool x_bWriteErrorsToTextFile { get; set; } = false;
        public string x_sTextFileForErrors { get; set; } = "";
        public string x_SystemCustomCodes { get; set; } = "";
        public string x_AboutLine1 { get; set; } = "";
        public string x_AboutLine2 { get; set; } = "";
        public string x_AboutLine3 { get; set; } = "";



        //Email
        public string x_SendGridApiKey { get; set; } = "";
        public string x_FromEmailAddress { get; set; } = "";
        public string x_FromName { get; set; } = "";
        public string x_ToAddressForErrors { get; set; } = "";
        public bool x_DisableAllEmail { get; set; } = false;
        public bool x_SendEmailOnError { get; set; } = false;
        // ====================================

        public string LastUsedSQLStatement { get; set; } = "";


        public string AllCustomCodes()
        {
            return x_SystemCustomCodes;
        }


    }
}
