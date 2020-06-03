using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhosIn.API
{
    public class APICommon
    {

        public void InsertLogItem(cMain m, string sTTGUID, string sLogMessage)
        {
            string sSQL;
            int i2;
            string sIPAddress = "";

            sSQL = "INSERT INTO [dbo].[W_TeeTimeLog] ";
            sSQL = sSQL + " ([LogGUID] ";
            sSQL = sSQL + " ,[TTGUID] ";
            sSQL = sSQL + " ,[LogDateTime] ";
            sSQL = sSQL + " ,[LogEvent] ";
            sSQL = sSQL + " ,[LogIPAddress]) ";
            sSQL = sSQL + " VALUES ( ";
            sSQL = sSQL + m.InQuote(m.CreateGuid()) + ", ";         //  (<LogGUID, varchar(50),>
            sSQL = sSQL + m.InQuote(sTTGUID, 150) + ", ";           //  ,<TTGUID, nvarchar(max),>
            sSQL = sSQL + m.InQuoteT(m.Now2()) + ", ";              //  ,<LogDateTime, datetime,>
            sSQL = sSQL + m.InQuote(sLogMessage, 300) + ", ";       //  ,<LogEvent, nvarchar(max),>
            sSQL = sSQL + m.InQuote(sIPAddress, 50) + ") ";         //  ,<LogIPAddress, varchar(max),>)
            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

        }

         

    }
}
