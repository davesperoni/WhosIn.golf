using System.Data;
using WhosIn.Classes_DataModels;

namespace WhosIn.API
{
    //====================================
    //
    // API05  - Settings 
    //
    //====================================

    internal class API05
    {
        readonly cMain m;
        readonly APIRunner a;

        public API05(cMain mIn, APIRunner aIn)
        {
            m = mIn;
            a = aIn;
        }


        public void API05_READ_SETTINGS()
        {
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT * FROM [W_SystemSettings] ";
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            a.AppendTableToDataset(ds1);
        }


        public void API05_UPDATE_SETTINGS()
        {
            string sJason;
            SettingsDM gItem;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            sJason = aFields[0];
            gItem = System.Text.Json.JsonSerializer.Deserialize<SettingsDM>(sJason);
            UpdateSettings2(gItem);
        }

        private void UpdateSettings2(SettingsDM gItem)
        {
            string sSQL;
            int i2;

            //if (m.IsEmpty(gItem.GroupName))
            //{
            //    SetResultCode(c.BATCH_RESULT_FAIL, "Group Name cannot be blank.");
            //    return;
            //}



            sSQL = "UPDATE [dbo].[W_SystemSettings] ";
            sSQL = sSQL + "  SET ";
            sSQL = sSQL + "  [SystemCustomCodes] = " + m.InQuote(gItem.SystemCustomCodes, 500);
            sSQL = sSQL + " , [PinNumber1] = " + m.InQuote(gItem.PinNumber1, 500);
            sSQL = sSQL + " , [PinNumber2] = " + m.InQuote(gItem.PinNumber2, 500);
            sSQL = sSQL + " , [PinNumber3] = " + m.InQuote(gItem.PinNumber3, 500);
            sSQL = sSQL + " , [TextField1] = " + m.InQuote(gItem.TextField1, 500);
            sSQL = sSQL + " , [TextField2] = " + m.InQuote(gItem.TextField2, 500);
            sSQL = sSQL + " , [TextField3] = " + m.InQuote(gItem.TextField3, 500);
            sSQL = sSQL + " , [TextField4] = " + m.InQuote(gItem.TextField4, 500);
            sSQL = sSQL + " , [CheckBox1] = " + m.InQuoteN(gItem.CheckBox1);
            sSQL = sSQL + " , [CheckBox2] = " + m.InQuoteN(gItem.CheckBox2);
            sSQL = sSQL + " , [CheckBox3] = " + m.InQuoteN(gItem.CheckBox3);
            sSQL = sSQL + " , [CheckBox4] = " + m.InQuoteN(gItem.CheckBox4);
            sSQL = sSQL + " , [CheckBox5] = " + m.InQuoteN(gItem.CheckBox5);
            sSQL = sSQL + " , [CheckBox6] = " + m.InQuoteN(gItem.CheckBox6);
            sSQL = sSQL + " , [CheckBox7] = " + m.InQuoteN(gItem.CheckBox7);
            sSQL = sSQL + " , [IncludeSecretInfoInErrMsgToUser] = " + m.InQuoteN(gItem.IncludeSecretInfoInErrMsgToUser);
            sSQL = sSQL + " , [CopywriteNotice] = " + m.InQuote(gItem.CopywriteNotice, 500);
            sSQL = sSQL + " , [ProductName] = " + m.InQuote(gItem.ProductName, 500);
            sSQL = sSQL + " , [ContactUrl] = " + m.InQuote(gItem.ContactUrl, 500);
            sSQL = sSQL + " , [DisableSystem] = " + m.InQuoteN(gItem.DisableSystem);
            sSQL = sSQL + " , [DisableMessage] = " + m.InQuote(gItem.DisableMessage, 500);
            sSQL = sSQL + " , [AboutLine1] = " + m.InQuote(gItem.AboutLine1, 500);
            sSQL = sSQL + " , [AboutLine2] = " + m.InQuote(gItem.AboutLine2, 500);
            sSQL = sSQL + " , [AboutLine3] = " + m.InQuote(gItem.AboutLine3, 500);

            i2 = m.SQLExecuteCommand(sSQL, c.DB.WhosIn);

            SetResultCode(c.BATCH_RESULT_OK, "Settings updated.");
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





