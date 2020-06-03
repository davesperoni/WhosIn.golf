using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhosIn.Classes_DataModels;
using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

namespace WhosIn.Classes_ViewModels
{ 
    public class AuditLogVM
    {
        private readonly cMain m;
        public string ErrorMessage { get; set; } = "";
        public string InfoMessage { get; set; } = "";
        public string SecretErrorDetails { get; set; } = "";
        public DataSet ds1;

        public AuditLogVM(cMain mIn)
        {
            m = mIn;
        }
        
        public List<AuditLogDM> GetAuditLog(string ttGUID)
        {
            List<AuditLogDM> ccList = new List<AuditLogDM>();
            try
            {
                ccList = GetAuditLog2(ttGUID);
                return ccList;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
                return ccList;
            }
        }

        private List<AuditLogDM> GetAuditLog2(string ttGUID)
        {
            List<AuditLogDM> ccList = new List<AuditLogDM>();
            AuditLogDM ccItem;
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            aParamData[0] = ttGUID;
            int iAPINumber = c.API07_GET_AUDITLOG;
            sAPIInput = m.ArrayToString(aParamData);

            oAPI.ActionCode = iAPINumber;
            oAPI.InputParameters = sAPIInput;
            oAPI.CallAPI(c.APIType.SENDDATA_RECEIVEDATA);
            if (oAPI.ErrorOccured)
            {
                ErrorMessage = oAPI.APIErrorMessage;
                SecretErrorDetails = oAPI.APIErrorSecretInfo;
                return ccList;
            }
            ds1 = oAPI.DatasetReturned;

            foreach (DataRow r in ds1.Tables[0].Rows)
            {
                ccItem = new AuditLogDM();
                ccItem.LogGUID  = m.GetFieldStr("LogGUID", r);
                ccItem.TTGUID = m.GetFieldStr("TTGUID", r);
                ccItem.LogDateTime = m.GetFieldDate("LogDateTime", r);
                ccItem.LogEvent = m.GetFieldStr("LogEvent", r);
                ccItem.LogIPAddress = m.GetFieldStr("LogIPAddress", r);
                ccItem.DisplayLine1 = m.FormatDT(ccItem.LogDateTime, c.DATE_FORMAT21).ToLower();
                ccItem.DisplayLine2 = ccItem.LogEvent;
                ccList.Add(ccItem);
            }

            return ccList;
        }

    }

}
