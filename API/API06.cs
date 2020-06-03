using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WhosIn.Classes_DataModels;
using System.Text.Json;
using Newtonsoft.Json;
using WhosIn.Pages;

//====================================
//
// API06  - Chat 
//
//====================================

namespace WhosIn.API
{

    internal class API06
    {
        readonly cMain m;
        readonly APIRunner a;

        public API06(cMain mIn, APIRunner aIn)
        {
            m = mIn;
            a = aIn;
        }


        public void API06_GET_MESSAGES_LIST()
        {
            string sTTGUID;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sTTGUID = aFields[0];
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT * FROM [W_TeeTimeChat] ";
            sSQL = sSQL + " WHERE [TeeTimeGUID] = " + m.InQuote(sTTGUID);
            sSQL = sSQL + " ORDER BY DateTimeEntered DESC";
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            a.AppendTableToDataset(ds1);
        }


        public void API06_GET_SINGLE_MESSAGE()
        {
            string sChatGUID;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sChatGUID = aFields[0];
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT * FROM [W_TeeTimeChat] ";
            sSQL = sSQL + " WHERE [ChatGUID] = " + m.InQuote(sChatGUID);
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            a.AppendTableToDataset(ds1);
        }



        public void API06_ADD_MESSAGE()
        {
            string sJason;
            ChatDM gItem;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sJason = aFields[0];
            gItem = System.Text.Json.JsonSerializer.Deserialize<ChatDM>(sJason);
            AddChat2(gItem);
        }

        private void AddChat2(ChatDM gItem)
        {
            string sSQL;
            int i2;
            string sMessage;

            sMessage = m.Left2(gItem.Message,32) + " : " + gItem.Initials;

            sSQL = "INSERT INTO [dbo].[W_TeeTimeChat] ";
            sSQL = sSQL + " ([ChatGUID] ";
            sSQL = sSQL + " ,[TeeTimeGUID] ";
            sSQL = sSQL + " ,[DateTimeEntered] ";
            sSQL = sSQL + " ,[Message] ";
            sSQL = sSQL + " ,[IPAddress]) ";
            sSQL = sSQL + " VALUES ( ";
            sSQL = sSQL + m.InQuote(m.CreateGuid()) + ", ";           //  (<ChatGUID, varchar(50),>
            sSQL = sSQL + m.InQuote(gItem.TeeTimeGUID , 150) + ", ";       //  ,<TeeTimeGUID, nvarchar(max),>
            sSQL = sSQL + m.InQuoteT(m.Now2()) + ", ";   //  ,<DateTimeEntered, datetime,>
            sSQL = sSQL + m.InQuote(sMessage, 150) + ", ";       //  ,<Message, nvarchar(max),>
            sSQL = sSQL + m.InQuote(gItem.IPAddress, 50) + ") ";      //  ,<IPAddress, varchar(max),>)
            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

            SetResultCode(c.BATCH_RESULT_OK, "Message added.");
            return;
        }




        public void API06_DELETE_MESSAGE()
        {
            string sGuid;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sGuid = aFields[0];
            DeleteChat2(sGuid);
        }

        private void DeleteChat2(string sGuid)
        {
            string sSQL;
            int i2;

            sSQL = "DELETE [dbo].[W_TeeTimeChat] ";
            sSQL = sSQL + " WHERE [ChatGUID] = " + m.InQuote(sGuid);
            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);
            SetResultCode(c.BATCH_RESULT_OK, "Message deleted.");
            return;
        }



        private void SetResultCode(string sResultCode, string sResultMsg)
        {
            DataTable dt1;
            dt1 = m.CreateReturnDT(sResultCode, sResultMsg);
            a.AppendTableToDataset(dt1);
        }

    }

}
