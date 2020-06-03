using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WhosIn.Classes_DataModels;
using System.Text.Json;
using Newtonsoft.Json;
using WhosIn.Pages;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

namespace WhosIn.API
{
    //====================================
    //
    // API03  - Update TeeTime  - Delete TeeTime 
    //
    //====================================

    internal class API03
    {
        readonly cMain m;
        readonly APIRunner a;

        public API03(cMain mIn, APIRunner aIn)
        {
            m = mIn;
            a = aIn;
        }

        public void API03_UPDATE_TT_PLAYER()
        {
            TeeTimeItem TT;
            string sJason;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sJason = aFields[0];
            TT = System.Text.Json.JsonSerializer.Deserialize<TeeTimeItem>(sJason);
            UpdateTTPlayer(TT);
        }

        private void UpdateTTPlayer(TeeTimeItem TT)
        {
            string sSQL;
            string sSQL2 = "";
            int i2;

            switch (TT.PlayerNumberBeingEdited)
            {
                case 1:
                    sSQL2 = "  Player1 = " + m.InQuote(TT.Player1, 50);
                    sSQL2 = sSQL2 + " , Player1Comment = " + m.InQuote(TT.Player1Comment, 150);
                    break;
                case 2:
                    sSQL2 = "  Player2 = " + m.InQuote(TT.Player2, 50);
                    sSQL2 = sSQL2 + " , Player2Comment = " + m.InQuote(TT.Player2Comment, 150);
                    break;
                case 3:
                    sSQL2 = "  Player3 = " + m.InQuote(TT.Player3, 50);
                    sSQL2 = sSQL2 + " , Player3Comment = " + m.InQuote(TT.Player3Comment, 150);
                    break;
                case 4:
                    sSQL2 = "  Player4 = " + m.InQuote(TT.Player4, 50);
                    sSQL2 = sSQL2 + " , Player4Comment = " + m.InQuote(TT.Player4Comment, 150);
                    break;
                default:
                    break;
            }

            WriteToAuditLog1(TT);

            sSQL = " UPDATE [dbo].[W_TeeTimes] ";
            sSQL += "  SET ";
            sSQL += sSQL2;
            sSQL += "  WHERE TeeTimeGUID = " + m.InQuote(TT.TTGUID);
            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

            SetResultCode(c.BATCH_RESULT_OK, "Player" + TT.PlayerNumberBeingEdited.ToString() + " updated.");
            return;
        }




        private void WriteToAuditLog1(TeeTimeItem TT)
        {
            string sSQL;
            DataSet ds1;
            APICommon ApiCommon = new APICommon();

            sSQL = "SELECT * ";
            sSQL = sSQL + " FROM  [W_TeeTimes] ";
            sSQL = sSQL + " WHERE [TeeTimeGUID] = " + m.InQuote(TT.TTGUID);
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            if (m.EmptyTable(ds1))
            {
                return;
            }

            if (TT.PlayerNumberBeingEdited == 1)
            {
                if (m.GetFieldStr("Player1", ds1) != TT.Player1)
                {
                    if (m.IsEmpty(TT.Player1))
                    {
                        ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 1 deleted.");
                    }
                    else
                    {
                        if (m.IsEmpty(m.GetFieldStr("Player1", ds1)))
                        {
                            ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 1 set to " + TT.Player1);
                        }
                        else
                        {
                            ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 1 changed to " + TT.Player1);
                        }
                    }
                }
            }




            if (TT.PlayerNumberBeingEdited == 2)
            {
                if (m.GetFieldStr("Player2", ds1) != TT.Player2)
                {
                    if (m.IsEmpty(TT.Player2))
                    {
                        ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 2 deleted.");
                    }
                    else
                    {
                        if (m.IsEmpty(m.GetFieldStr("Player2", ds1)))
                        {
                            ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 2 set to " + TT.Player2);
                        }
                        else
                        {
                            ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 2 changed to " + TT.Player2);
                        }
                    }
                }
            }




            if (TT.PlayerNumberBeingEdited == 3)
            {
                if (m.GetFieldStr("Player3", ds1) != TT.Player3)
                {
                    if (m.IsEmpty(TT.Player3))
                    {
                        ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 3 deleted.");
                    }
                    else
                    {
                        if (m.IsEmpty(m.GetFieldStr("Player3", ds1)))
                        {
                            ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 3 set to " + TT.Player3);
                        }
                        else
                        {
                            ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 3 changed to " + TT.Player3);
                        }
                    }
                }
            }

            if (TT.PlayerNumberBeingEdited == 4)
            {
                if (m.GetFieldStr("Player4", ds1) != TT.Player4)
                {
                    if (m.IsEmpty(TT.Player4))
                    {
                        ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 4 deleted.");
                    }
                    else
                    {
                        if (m.IsEmpty(m.GetFieldStr("Player4", ds1)))
                        {
                            ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 4 set to " + TT.Player4);
                        }
                        else
                        {
                            ApiCommon.InsertLogItem(m, TT.TTGUID, "Player 4 changed to " + TT.Player4);
                        }
                    }
                }
            }
        }





        public void API03_UPDATE_TT_INFO()
        {
            TeeTimeItem TT;
            string sJason;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sJason = aFields[0];
            TT = System.Text.Json.JsonSerializer.Deserialize<TeeTimeItem>(sJason);
            UpdateInfo2(TT);
        }

        private void UpdateInfo2(TeeTimeItem TT)
        {
            string sSQL;
            int i2;


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

            WriteToAuditLog2(TT);

            sSQL = "UPDATE [dbo].[W_TeeTimes] ";
            sSQL += "  SET [GroupGUID] =" + m.InQuote(TT.GroupGUID);
            sSQL += ", [TeeTimeDate] =" + m.InQuoteT(m.NoNullDate(TT.TTDate));
            sSQL += ", [TeeTimeTime] =" + m.InQuoteT(m.NoNullDate(TT.TTTime));
            sSQL += ", [OwnerComment] =" + m.InQuote(TT.OwnerComment);
            sSQL += ", [LockPlayer1] =" + m.InQuoteN(TT.LockPlayer1);
            sSQL += ", [LockPlayer2] =" + m.InQuoteN(TT.LockPlayer2);
            sSQL += ", [LockPlayer3] =" + m.InQuoteN(TT.LockPlayer3);
            sSQL += ", [LockPlayer4] =" + m.InQuoteN(TT.LockPlayer4);
            sSQL += ", [TeeTimeOwner] =" + m.InQuote(TT.Owner);
            sSQL += ", [TeeTimeLocation] =" + m.InQuote(TT.Location);
            sSQL += ", [LockMessages] =" + m.InQuoteN(TT.LockMessages);
            sSQL += ", [HideMessages] =" + m.InQuoteN(TT.HideMessages);
            sSQL += "  WHERE TeeTimeGUID = " + m.InQuote(TT.TTGUID);
            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);
            SetResultCode(c.BATCH_RESULT_OK, "");
            return;
        }

        private void WriteToAuditLog2(TeeTimeItem TT)
        {
            string sSQL;
            DataSet ds1;
            APICommon ApiCommon = new APICommon();

            sSQL = "SELECT * ";
            sSQL = sSQL + " FROM  [W_TeeTimes] ";
            sSQL = sSQL + " WHERE [TeeTimeGUID] = " + m.InQuote(TT.TTGUID);
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            if (m.EmptyTable(ds1))
            {
                return;
            }

            if (m.GetFieldDateF("TeeTimeDate", ds1, c.DATE_FORMAT1) != m.FormatDT(TT.TTDate, c.DATE_FORMAT1))
            {
                ApiCommon.InsertLogItem(m, TT.TTGUID, "Date changed to " + m.FormatDT(TT.TTDate, c.DATE_FORMAT1));
            }

            if (m.GetFieldDateF("TeeTimeTime", ds1, c.DATE_FORMAT10) != m.FormatDT(TT.TTTime, c.DATE_FORMAT10))
            {
                ApiCommon.InsertLogItem(m, TT.TTGUID, "Time changed to " + m.FormatDT(TT.TTTime, c.DATE_FORMAT10));
            }

            if (m.GetFieldStr("OwnerComment", ds1) != TT.OwnerComment)
            {
                ApiCommon.InsertLogItem(m, TT.TTGUID, "Owner's comment changed.");
            }

            if (m.GetFieldStr("TeeTimeOwner", ds1) != TT.Owner)
            {
                ApiCommon.InsertLogItem(m, TT.TTGUID, "Owner changed to " + TT.Owner);
            }

            if (m.GetFieldStr("TeeTimeLocation", ds1) != TT.Location)
            {
                ApiCommon.InsertLogItem(m, TT.TTGUID, "Location to " + TT.Location);
            }

            if (m.GetFieldStr("GroupGUID", ds1) != TT.GroupGUID)
            {
                ApiCommon.InsertLogItem(m, TT.TTGUID, "Group changed.");
            }

        }



        public void API03_MARK_TT_AS_DELETED()
        {
            TeeTimeItem TT;
            bool bUndelete;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            TT = System.Text.Json.JsonSerializer.Deserialize<TeeTimeItem>(aFields[0]);
            bUndelete = m.ToBol(aFields[1]);

            if (bUndelete)
            {
                MarkTTasUnDeleted(TT.TTGUID);
            }
            else
            {
                MarkTTasDeleted2(TT);
            }
        }

        private void MarkTTasDeleted2(TeeTimeItem TT)
        {
            string sSQL;
            int i2;

            if (!m.IsPinCorrect(TT.PinNumberEntered, TT.GroupGUID))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Pin number is not correct.");
                return;
            }

            sSQL = "UPDATE [dbo].[W_TeeTimes] ";
            sSQL += "  SET [IsDeleted] =" + m.InQuoteN(1);
            sSQL += "  WHERE TeeTimeGUID = " + m.InQuote(TT.TTGUID);
            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

            SetResultCode(c.BATCH_RESULT_OK, "TeeTime deleted.");
            return;
        }

        private void MarkTTasUnDeleted(string ttGUID)
        {
            string sSQL;
            int i2;

            sSQL = "UPDATE [dbo].[W_TeeTimes] ";
            sSQL += "  SET [IsDeleted] =" + m.InQuoteN(0);
            sSQL += "  WHERE TeeTimeGUID = " + m.InQuote(ttGUID);
            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

            SetResultCode(c.BATCH_RESULT_OK, "");
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





