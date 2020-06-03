using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WhosIn.Classes_DataModels;
using System.Text.Json;
using Newtonsoft.Json;
using WhosIn.Pages;

namespace WhosIn.API
{
    //====================================
    //
    // API04  - Groups 
    //
    //====================================

    internal class API04
    {
        readonly cMain m;
        readonly APIRunner a;

        public API04(cMain mIn, APIRunner aIn)
        {
            m = mIn;
            a = aIn;
        }

        public void API04_GET_GROUPS()
        {
            bool bAll;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            bAll = m.ToBol(aFields[0]);
            GetGroups(bAll);
        }

        private void GetGroups(bool bAll)
        {
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT * FROM [W_Groups] ";
            if (!bAll)
            {
                sSQL = sSQL + " WHERE [GroupActive] = 1";

            }
            sSQL = sSQL + " ORDER BY [GroupName]";

            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            a.AppendTableToDataset(ds1);

        }


        public void API04_GET_SINGLE_GROUP()
        {
            string sGroupGUID;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sGroupGUID = aFields[0];
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT * FROM [W_Groups] ";
            sSQL = sSQL + " WHERE [GroupGUID] = " + m.InQuote(sGroupGUID);

            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            a.AppendTableToDataset(ds1);
        }


        public void API04_UPDATE_GROUP()
        {
            GroupItemDM gItem;
            string sJason;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sJason = aFields[0];
            gItem = System.Text.Json.JsonSerializer.Deserialize<GroupItemDM>(sJason);
            UpdateGroup(gItem);
        }

        private void UpdateGroup(GroupItemDM gItem)
        {
            string sSQL;
            int i2;


            if (m.IsEmpty(gItem.GroupName))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Group Name cannot be blank.");
                return;
            }

            if (m.IsEmpty(gItem.GroupDescription))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Group Description cannot be blank.");
                return;
            }

            sSQL = "UPDATE [dbo].[W_Groups] ";
            sSQL = sSQL + "  SET ";
            sSQL = sSQL + "   [GroupName] = " + m.InQuote(gItem.GroupName, 50);
            sSQL = sSQL + "  ,[GroupDescription] = " + m.InQuote(gItem.GroupDescription, 100);
            sSQL = sSQL + "  ,[GroupPin] = " + m.InQuote(gItem.GroupPin);
            sSQL = sSQL + "  ,[GroupPhone] = " + m.InQuote(gItem.GroupPhone);
            sSQL = sSQL + "  ,[GroupEmail] = " + m.InQuote(gItem.GroupEmail, 500);
            sSQL = sSQL + "  ,[GroupActive] = " + m.InQuoteN(gItem.GroupActive);
            sSQL = sSQL + "  ,[GroupIsDefault] = " + m.InQuoteN(gItem.GroupIsDefault);
            sSQL = sSQL + "  ,[GroupShowAllGroups] = " + m.InQuoteN(gItem.GroupShowAllGroups);
            sSQL = sSQL + "  ,[GroupColor] = " + m.InQuoteN(gItem.GroupColor);
            sSQL = sSQL + "  WHERE GroupGUID = " + m.InQuote(gItem.GroupGUID);
            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

            SetResultCode(c.BATCH_RESULT_OK, "TeeTime updated.");
            return;
        }


        public void API04_DELETE_GROUP()
        {
            string sGroupGUID;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sGroupGUID = aFields[0];
            DeleteGroup(sGroupGUID);
        }

        private void DeleteGroup(string sGroupGUID)
        {
            string sSQL;
            int i2;
            DataSet ds1;

            sSQL = "SELECT * FROM [W_Groups] ";
            sSQL = sSQL + " WHERE [GroupGUID] = " + m.InQuote(sGroupGUID);
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            if (m.EmptyTable(ds1))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Group not found.");
                return;
            }

            if (m.GetFieldBol("GroupShowAllGroups", ds1))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "This group cannot be deleted. It is the 'Show all' group.");
                return;
            }


            sSQL = "SELECT GroupGUID FROM [W_TeeTimes] ";
            sSQL = sSQL + " WHERE [GroupGUID] = " + m.InQuote(sGroupGUID);
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            if (!m.EmptyTable(ds1))
            {
                int iCount = ds1.Tables[0].Rows.Count;
                SetResultCode(c.BATCH_RESULT_FAIL, "Cannot delete. There are " + iCount.ToString() + " Tee Times associated with this Group.");
                return;
            }

            sSQL = "DELETE FROM [W_Groups] ";
            sSQL = sSQL + " WHERE [GroupGUID] = " + m.InQuote(sGroupGUID);
            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

            SetResultCode(c.BATCH_RESULT_OK, "");
            return;
        }


        public void API04_ADD_GROUP()
        {
            GroupItemDM gItem;
            string sJason;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sJason = aFields[0];
            gItem = System.Text.Json.JsonSerializer.Deserialize<GroupItemDM>(sJason);
            AddGroup(gItem);
        }

        private void AddGroup(GroupItemDM gItem)
        {
            string sSQL;
            int i2;

            if (m.IsEmpty(gItem.GroupName))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Group Name cannot be blank.");
                return;
            }

            if (m.IsEmpty(gItem.GroupDescription))
            {
                SetResultCode(c.BATCH_RESULT_FAIL, "Group Description cannot be blank.");
                return;
            }

            sSQL = " INSERT INTO [dbo].[W_Groups] ";
            sSQL = sSQL + " ([GroupGUID] ";
            sSQL = sSQL + " ,[GroupName] ";
            sSQL = sSQL + " ,[GroupDescription] ";
            sSQL = sSQL + " ,[GroupPin] ";
            sSQL = sSQL + " ,[GroupPhone] ";
            sSQL = sSQL + " ,[GroupEmail] ";
            sSQL = sSQL + " ,[GroupActive] ";
            sSQL = sSQL + " ,[GroupIsDefault] ";
            sSQL = sSQL + " ,[GroupShowAllGroups] ";
            sSQL = sSQL + " ,[GroupColor]) ";
            sSQL = sSQL + " VALUES (";

            sSQL = sSQL + m.InQuote(m.CreateGuid()) + ", ";
            sSQL = sSQL + m.InQuote(gItem.GroupName, 50) + ", ";
            sSQL = sSQL + m.InQuote(gItem.GroupDescription, 100) + ", ";    
            sSQL = sSQL + m.InQuote(gItem.GroupPin) + ", ";
            sSQL = sSQL + m.InQuote(gItem.GroupPhone) + ", ";                 
            sSQL = sSQL + m.InQuote(gItem.GroupEmail, 500) + ", ";
            sSQL = sSQL + m.InQuoteN(gItem.GroupActive) + ", ";
            sSQL = sSQL + m.InQuoteN(gItem.GroupIsDefault) + ", ";
            sSQL = sSQL + m.InQuoteN(gItem.GroupShowAllGroups) + ", ";
            sSQL = sSQL + m.InQuote(gItem.GroupColor, 100);
            sSQL = sSQL + " )";

            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

            SetResultCode(c.BATCH_RESULT_OK, "TeeTime added.");
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





