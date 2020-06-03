using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WhosIn.Classes_DataModels;
using System.Text.Json;
using Newtonsoft.Json;
using WhosIn.Pages;
using Microsoft.AspNetCore.Diagnostics;

namespace WhosIn.API
{
    //====================================
    //
    // API02  - Add TeeTime 
    //
    //====================================

    internal class API02
    {
        readonly cMain m;
        readonly APIRunner a;

        public API02(cMain mIn, APIRunner aIn)
        {
            m = mIn;
            a = aIn;
        }

        public void API02_ADD_TEETIME()
        {
            TeeTimeItem TT;
            string sJason;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sJason = aFields[0];
            TT = System.Text.Json.JsonSerializer.Deserialize<TeeTimeItem>(sJason);
            AddTeeTime(TT);
        }

        private void AddTeeTime(TeeTimeItem TT)
        {
            string sSQL;
            int i2;
            APICommon ApiCommon = new APICommon();
            TT.TTGUID = m.CreateGuid();


            if (m.IsEmpty(TT.Owner))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Owner cannot be blank.");
                return;
            }

            if (m.IsEmpty(TT.Location))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Location cannot be blank.");
                return;
            }

            if (m.IsEmpty(TT.GroupGUID))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Group cannot be blank.");
                return;
            }


            if (TT.TTTime == null)
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Time cannot be blank.");
                return;
            }

            if (TT.TTDate == null)
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Date cannot be blank.");
                return;
            }

            if (m.BeforeToday(m.NoNullDate(TT.TTDate)))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Date cannot be before today.");
                return;
            }

            if (m.FormatDT(TT.TTTime, "HH:mm") == "00:00")
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Time cannot be blank.");
                return;
            }

            if (TTAleadyExists(TT))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "This teetime already exists.");
                return;
            }

            sSQL = "INSERT INTO [dbo].[W_TeeTimes] ";
            sSQL = sSQL + "     ([TeeTimeGUID] ";
            sSQL = sSQL + "    ,[GroupGUID] ";
            sSQL = sSQL + "    ,[TeeTimeOwner] ";
            sSQL = sSQL + "    ,[TeeTimeLocation] ";
            sSQL = sSQL + "    ,[TeeTimeDate] ";
            sSQL = sSQL + "    ,[TeeTimeTime] ";
            sSQL = sSQL + "    ,[OwnerComment] ";
            sSQL = sSQL + "    ,[LockPlayer1] ";
            sSQL = sSQL + "    ,[LockPlayer2] ";
            sSQL = sSQL + "    ,[LockPlayer3] ";
            sSQL = sSQL + "    ,[LockPlayer4] ";
            sSQL = sSQL + "    ,[Player1] ";
            sSQL = sSQL + "    ,[Player2] ";
            sSQL = sSQL + "    ,[Player3] ";
            sSQL = sSQL + "    ,[Player4] ";
            sSQL = sSQL + "    ,[WaitList1] ";
            sSQL = sSQL + "    ,[WaitList2] ";
            sSQL = sSQL + "    ,[WaitList3] ";
            sSQL = sSQL + "    ,[LockMessages] ";
            sSQL = sSQL + "    ,[HideMessages] ";
            sSQL = sSQL + "    ,[IsDeleted] )";
            sSQL = sSQL + "  VALUES (";
            sSQL = sSQL + m.InQuote(TT.TTGUID, 50) + ", ";    //  (<TeeTimeGUID, varchar(50),> ";
            sSQL = sSQL + m.InQuote(TT.GroupGUID, 50) + ", ";   //      ,<GroupGUID, varchar(50),> ";
            sSQL = sSQL + m.InQuote(TT.Owner, 50) + ", ";   //        ,<TeeTimeOwner, nvarchar(max),> ";
            sSQL = sSQL + m.InQuote(TT.Location, 100) + ", ";   //        ,<TeeTimeLocation, nvarchar(max),> ";
            sSQL = sSQL + m.InQuoteT(m.NoNullDate(TT.TTDate)) + ", ";   //        ,<TeeTimeDate, date,> ";
            sSQL = sSQL + m.InQuoteT(m.NoNullDate(TT.TTTime)) + ", ";   //       ,<TeeTimeTime, time(0),> ";
            sSQL = sSQL + m.InQuote(TT.OwnerComment, 500) + ", ";   //       ,<OwnerComment, nvarchar(max),> ";
            sSQL = sSQL + m.InQuoteN(TT.LockPlayer1) + ", ";   //       ,<LockPlayer1, smallint,> ";
            sSQL = sSQL + m.InQuoteN(TT.LockPlayer2) + ", ";   //       ,<LockPlayer2, smallint,> ";
            sSQL = sSQL + m.InQuoteN(TT.LockPlayer3) + ", ";   //       ,<LockPlayer3, smallint,> ";
            sSQL = sSQL + m.InQuoteN(TT.LockPlayer4) + ", ";   //       ,<LockPlayer4, smallint,> ";
            sSQL = sSQL + m.InQuote(TT.Player1, 100) + ", ";   //       ,<Player1, nvarchar(max),> ";
            sSQL = sSQL + m.InQuote(TT.Player2, 100) + ", ";   //       ,<Player2, nvarchar(max),> ";
            sSQL = sSQL + m.InQuote(TT.Player3, 100) + ", ";   //       ,<Player3, nvarchar(max),> ";
            sSQL = sSQL + m.InQuote(TT.Player4, 100) + ", ";   //      ,<Player4, nvarchar(max),> ";
            sSQL = sSQL + m.InQuote(TT.WaitList1, 100) + ", ";   //      ,<WaitList1, nvarchar(max),> ";
            sSQL = sSQL + m.InQuote(TT.WaitList2, 100) + ", ";   //      ,<WaitList2, nvarchar(max),> ";
            sSQL = sSQL + m.InQuote(TT.WaitList3, 100) + ", ";   //  "    ,<WaitList3, nvarchar(max),>) ";
            sSQL = sSQL + m.InQuoteN(TT.LockMessages) + ", ";   //       ,<LockMessages, smallint,> ";
            sSQL = sSQL + m.InQuoteN(TT.HideMessages) + ", ";   //       ,<HideMessages, smallint,> ";
            sSQL = sSQL + m.InQuoteN(0) + ") ";   //  "    ,<IsDeleted, int>) ";

            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

            ApiCommon.InsertLogItem(m, TT.TTGUID, "Tee Time created.");

            SetResultCode(c.BATCH_RESULT_OK, "TeeTime added.");

            DeleteOldTeeTimes();
            return;
        }


        private void SetResultCode(string sResultCode, string sResultMsg)
        {
            DataTable dt1;
            dt1 = m.CreateReturnDT(sResultCode, sResultMsg);
            a.AppendTableToDataset(dt1);
        }

        private bool TTAleadyExists(TeeTimeItem TT)
        {
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT TeeTimeGUID FROM [W_TeeTimes] ";
            sSQL = sSQL + " WHERE [TeeTimeDate] = " + m.InQuoteT(m.NoNullDate(TT.TTDate));
            sSQL = sSQL + " AND [TeeTimeTime] = " + m.InQuoteT(m.NoNullDate(TT.TTTime));
            sSQL = sSQL + " AND [IsDeleted] = 0";
            sSQL = sSQL + " AND [GroupGUID] = " + m.InQuote(TT.GroupGUID);
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);

            if (m.EmptyTable(ds1))
            {
                return false;
            }
            else
            {
                return true;
            }

        }



        private void DeleteOldTeeTimes()
        {
            int i2;
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT * FROM [W_TeeTimes] ";
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            if (m.EmptyTable(ds1))
            {
                return;
            }

            foreach (DataRow r in ds1.Tables[0].Rows)
            {
                if (CanBeDeleted(r))
                {
                    sSQL = "DELETE FROM W_TeeTimeChat ";
                    sSQL = sSQL + " WHERE [TeeTimeGUID] = " + m.InQuote(m.GetFieldStr("TeeTimeGUID", r));
                    i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

                    sSQL = "DELETE FROM W_TeeTimeLog ";
                    sSQL = sSQL + " WHERE [TTGUID] = " + m.InQuote(m.GetFieldStr("TeeTimeGUID", r));
                    i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

                    sSQL = "DELETE FROM W_TeeTimes ";
                    sSQL = sSQL + " WHERE [TeeTimeGUID] = " + m.InQuote(m.GetFieldStr("TeeTimeGUID", r));
                    i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

                }
            }

        }

        private bool CanBeDeleted(DataRow r)
        {
            int i;

            // string sDate = m.GetFieldDateF("TeeTimeDate", r, c.DATE_FORMAT1);

            i = m.DateDiff2(m.GetFieldDate("TeeTimeDate", r), m.Now2());

            if (i > 90)
            {
                return true;
            }

            return false;
        }

    }
}




