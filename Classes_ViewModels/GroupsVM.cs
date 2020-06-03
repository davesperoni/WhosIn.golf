using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhosIn.Classes_DataModels;
using System.Data;
using System.Text.Json;

namespace WhosIn.Classes_ViewModels
{
    public class GroupsVM
    {
        private cMain m;
        public string ErrorMessage { get; set; } = "";
        public string InfoMessage { get; set; } = "";
        public string SecretErrorDetails { get; set; } = "";
        public DataSet ds1;

        public GroupsVM(cMain mIn)
        {
            m = mIn;
        }



        public List<GroupItemDM> GetGroups(bool bAll)
        {
            List<GroupItemDM> GroupList = new List<GroupItemDM>();
            try
            {
                GroupList = GetGroups2(bAll);
                return GroupList;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
                return GroupList;
            }
        }

        private List<GroupItemDM> GetGroups2(bool bAll)
        {

            List<GroupItemDM> GroupList = new List<GroupItemDM>();
            GroupItemDM GroupItem;
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API04_GET_GROUPS;

            if (bAll)
            {
                aParamData[0] = "1";      
            }
            else
            {
                aParamData[0] = "0";     
            }

            sAPIInput = m.ArrayToString(aParamData);

            oAPI.ActionCode = iAPINumber;
            oAPI.InputParameters = sAPIInput;
            oAPI.CallAPI(c.APIType.SENDDATA_RECEIVEDATA);
            if (oAPI.ErrorOccured)
            {
                ErrorMessage = oAPI.APIErrorMessage;
                SecretErrorDetails = oAPI.APIErrorSecretInfo;
                return GroupList;
            }
            ds1 = oAPI.DatasetReturned;

            foreach (DataRow r in ds1.Tables[0].Rows)
            {
                GroupItem = new GroupItemDM();
                GroupItem.GroupGUID = m.GetFieldStr("GroupGUID", r);
                GroupItem.GroupName = m.GetFieldStr("GroupName", r);
                GroupItem.GroupDescription = m.GetFieldStr("GroupDescription", r);
                GroupItem.GroupPin = m.GetFieldStr("GroupPin", r);
                GroupItem.GroupPhone = m.GetFieldStr("GroupPhone", r);
                GroupItem.GroupEmail = m.GetFieldStr("GroupEmail", r);
                GroupItem.GroupActive = m.GetFieldBol("GroupActive", r);
                GroupItem.GroupIsDefault = m.GetFieldBol("GroupIsDefault", r);
                GroupItem.GroupShowAllGroups = m.GetFieldBol("GroupShowAllGroups", r);
                GroupItem.GroupColor = m.GetFieldStr("GroupColor", r);
                GroupList.Add(GroupItem);
            }





            return GroupList;
        }






        public void GetSingleGroup(GroupItemDM groupItem)
        {
            try
            {
                GetSingleGroup2(groupItem);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void GetSingleGroup2(GroupItemDM groupItem)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API04_GET_SINGLE_GROUP;

            aParamData[0] = groupItem.GroupGUID;
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
                ErrorMessage = "Error. Group not found. (" + groupItem.GroupGUID + ")";
                return;
            }

            groupItem.GroupGUID = m.GetFieldStr("GroupGUID", ds1);
            groupItem.GroupName = m.GetFieldStr("GroupName", ds1);
            groupItem.GroupDescription = m.GetFieldStr("GroupDescription", ds1);
            groupItem.GroupPin = m.GetFieldStr("GroupPin", ds1);
            groupItem.GroupPhone = m.GetFieldStr("GroupPhone", ds1);
            groupItem.GroupEmail = m.GetFieldStr("GroupEmail", ds1);
            groupItem.GroupActive = m.GetFieldBol("GroupActive", ds1);
            groupItem.GroupIsDefault = m.GetFieldBol("GroupIsDefault", ds1);
            groupItem.GroupShowAllGroups = m.GetFieldBol("GroupShowAllGroups", ds1);
            groupItem.GroupColor = m.GetFieldStr("GroupColor", ds1);

        }



        public void UpdateGroup(GroupItemDM groupItem)
        {
            try
            {
                UpdateGroup2(groupItem);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void UpdateGroup2(GroupItemDM groupItem)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API04_UPDATE_GROUP;

            aParamData[0] = JsonSerializer.Serialize<GroupItemDM>(groupItem);
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
                InfoMessage = "";
            }
            else
            {
                groupItem.ValidationFailMessage = m.GetFieldStr("ResultMsg", ds1);
            }




        }



        public void AddGroup(GroupItemDM groupItem)
        {
            try
            {
                AddGroup2(groupItem);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void AddGroup2(GroupItemDM groupItem)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API04_ADD_GROUP;

            aParamData[0] = JsonSerializer.Serialize<GroupItemDM>(groupItem);
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
                InfoMessage = "";
            }
            else
            {
                groupItem.ValidationFailMessage = m.GetFieldStr("ResultMsg", ds1);
            }

        }



        public void DeleteGroup(string sGroupGuid)
        {
            try
            {
                DeleteGroup2(sGroupGuid);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void DeleteGroup2(string sGroupGuid)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API04_DELETE_GROUP;

            aParamData[0] = sGroupGuid;
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
                InfoMessage = "";
            }
            else
            {
                ErrorMessage = m.GetFieldStr("ResultMsg", ds1);
            }

        }

    }

}
