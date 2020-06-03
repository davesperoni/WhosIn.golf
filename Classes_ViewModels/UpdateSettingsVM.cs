using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhosIn.Classes_DataModels;
using System.Data;
using System.Text.Json;

namespace WhosIn.Classes_ViewModels
{
    public class UpdateSettingsVM
    {
        private readonly cMain m;
        public string ErrorMessage { get; set; } = "";
        public string InfoMessage { get; set; } = "";
        public string SecretErrorDetails { get; set; } = "";
        public DataSet ds1;

        public UpdateSettingsVM(cMain mIn)
        {
            m = mIn;
        }


        public void ReadSettings(SettingsDM setting)
        {
            try
            {
                ReadSettings2(setting);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void ReadSettings2(SettingsDM setting)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API05_READ_SETTINGS;

            aParamData[0] = "";
            sAPIInput = m.ArrayToString(aParamData);

            oAPI.ActionCode = iAPINumber;
            oAPI.InputParameters = sAPIInput;
            oAPI.CallAPI(c.APIType.SENDDATA_RECEIVEDATA);
            if (oAPI.ErrorOccured)
            {
                ErrorMessage = oAPI.APIErrorMessage;
                SecretErrorDetails = oAPI.APIErrorSecretInfo;
                return;
            }
            ds1 = oAPI.DatasetReturned;

            if (m.EmptyTable(ds1))
            {
                ErrorMessage = "Settings table has no rows";
                return;
            }

            setting.SystemCustomCodes = m.GetFieldStr("SystemCustomCodes", ds1);
            setting.PinNumber1 = m.GetFieldStr("PinNumber1", ds1);
            setting.PinNumber2 = m.GetFieldStr("PinNumber2", ds1);
            setting.PinNumber3 = m.GetFieldStr("PinNumber3", ds1);
            setting.IncludeSecretInfoInErrMsgToUser = m.GetFieldBol("IncludeSecretInfoInErrMsgToUser", ds1);
            setting.CopywriteNotice = m.GetFieldStr("CopywriteNotice", ds1);
            setting.ProductName = m.GetFieldStr("ProductName", ds1);
            setting.ContactUrl = m.GetFieldStr("ContactUrl", ds1);
            setting.DisableSystem = m.GetFieldBol("DisableSystem", ds1);
            setting.DisableMessage = m.GetFieldStr("DisableMessage", ds1);
            setting.AboutLine1 = m.GetFieldStr("AboutLine1", ds1);
            setting.AboutLine2 = m.GetFieldStr("AboutLine2", ds1);
            setting.AboutLine3 = m.GetFieldStr("AboutLine3", ds1);
            setting.TextField1 = m.GetFieldStr("TextField1", ds1);
            setting.TextField2 = m.GetFieldStr("TextField2", ds1);
            setting.TextField3 = m.GetFieldStr("TextField3", ds1);
            setting.TextField4 = m.GetFieldStr("TextField4", ds1);
            setting.CheckBox1 = m.GetFieldBol("CheckBox1", ds1);
            setting.CheckBox2 = m.GetFieldBol("CheckBox2", ds1);
            setting.CheckBox3 = m.GetFieldBol("CheckBox3", ds1);
            setting.CheckBox4 = m.GetFieldBol("CheckBox4", ds1);
            setting.CheckBox5 = m.GetFieldBol("CheckBox5", ds1);
            setting.CheckBox6 = m.GetFieldBol("CheckBox6", ds1);
            setting.CheckBox7 = m.GetFieldBol("CheckBox7", ds1);
        }


        public void UpdateSettings(SettingsDM setting)
        {
            try
            {
                UpdateSettings2(setting);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void UpdateSettings2(SettingsDM setting)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API05_UPDATE_SETTINGS;

            aParamData[0] = JsonSerializer.Serialize<SettingsDM>(setting);
            sAPIInput = m.ArrayToString(aParamData);

            oAPI.ActionCode = iAPINumber;
            oAPI.InputParameters = sAPIInput;
            oAPI.CallAPI(c.APIType.SENDDATA_RECEIVEDATA);
            if (oAPI.ErrorOccured)
            {
                ErrorMessage = oAPI.APIErrorMessage;
                SecretErrorDetails = oAPI.APIErrorSecretInfo;
                return;
            }
            ds1 = oAPI.DatasetReturned;

            if (m.GetFieldStr("ResultCode", ds1) == c.BATCH_RESULT_OK)
            {
                InfoMessage = "Settings updated.";
            }
            else
            {
                ErrorMessage = m.GetFieldStr("ResultMsg", ds1);
            }

        }

    }
}
