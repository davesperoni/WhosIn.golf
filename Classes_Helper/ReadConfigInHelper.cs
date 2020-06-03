using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace WhosIn
{
    public static class ReadConfig
    {
        public static void ReadAppSettingFile(cMain m)
        {
            if (!m.IsEmpty(m.x_sConnectionStringMain))
            {
                return;
            }

            //ConnectionStrings Section
            m.x_sConnectionStringMain = GetConfigItem("ConnectionStrings", "DefaultConnection");
            m.x_sConnectionStringNOTUSED = GetConfigItem("ConnectionStrings", "BadConnection");

            //OtherSettings Section
            m.x_bUseLogging = m.ToBol(GetConfigItem("OtherSettings", "UseLogging"));
            m.x_bIncludeDebugInLog = m.ToBol(GetConfigItem("OtherSettings", "IncludeDebugInLog"));
            m.x_iProductCode = m.ToInt(GetConfigItem("OtherSettings", "ProductCode"));
            m.x_bWriteErrorsToTextFile = m.ToBol(GetConfigItem("OtherSettings", "WriteErrorsToTextFile"));
            m.x_sTextFileForErrors = GetConfigItem("OtherSettings", "TextFileForErrors");

            // Email Section
            m.x_SendGridApiKey = GetConfigItem("Email", "SendGridApiKey");
            m.x_FromEmailAddress = GetConfigItem("Email", "FromEmailAddress");
            m.x_FromName = GetConfigItem("Email", "FromName");
            m.x_ToAddressForErrors = GetConfigItem("Email", "ToAddressForErrors");
            m.x_SendEmailOnError = m.ToBol(GetConfigItem("Email", "SendEmailOnErrors"));
            m.x_DisableAllEmail = m.ToBol(GetConfigItem("Email", "DisableAllEmail"));

            ReadSettingsFromDatabase(m);
        }

        public static void ReadSettingsFromDatabase(cMain m)
        {
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT * FROM [W_SystemSettings] ";
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);

            if (!m.EmptyTable(ds1))
            {
                m.x_bIncludeSecretInfoInErrMsgToUser = m.GetFieldBol("IncludeSecretInfoInErrMsgToUser", ds1);
                m.x_sCopywriteNotice = m.GetFieldStr("CopywriteNotice", ds1);
                m.x_sProductName = m.GetFieldStr("ProductName", ds1);
                m.x_sContactUrl = m.GetFieldStr("ContactUrl", ds1);
                m.x_bDisableSystem = m.GetFieldBol("DisableSystem", ds1);
                m.x_sDisableMsg = m.GetFieldStr("DisableMessage", ds1);
                m.x_AboutLine1 = m.GetFieldStr("AboutLine1", ds1);
                m.x_AboutLine2 = m.GetFieldStr("AboutLine2", ds1);
                m.x_AboutLine3 = m.GetFieldStr("AboutLine3", ds1);
                m.x_SystemCustomCodes = m.GetFieldStr("SystemCustomCodes", ds1);
            }

        }

        private static string GetConfigItem(string sSection, string sKey)
        {
            string sKeyPair = "";
            string sTemp = "";

            sKeyPair = sSection + ":" + sKey;
            sKeyPair = sKeyPair.Trim().ToLower();

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            foreach (var kvp in configuration.GetSection(sSection).AsEnumerable())
            {
                if (kvp.Key.Trim().ToLower() == sKeyPair)
                {
                    sTemp = kvp.Value;
                    return sTemp; 
                }
            }

            return "";
        }
    }
}
