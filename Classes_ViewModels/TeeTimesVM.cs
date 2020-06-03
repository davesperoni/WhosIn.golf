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

    public class TeeTimesVM
    {
        private readonly cMain m;
        public string ErrorMessage { get; set; } = "";
        public string InfoMessage { get; set; } = "";
        public string SecretErrorDetails { get; set; } = "";
        public DataSet ds1;

        public TeeTimesVM(cMain mIn)
        {
            m = mIn;
        }

        public void InitializeNewTTime(TeeTimeItem TT)
        {
            TT.Location = "";
        }


        public List<TeeTimeItem> GetListOfTeeTimes(string GroupGUID, bool ShowAllTeeTimes)
        {
            List<TeeTimeItem> UIList = new List<TeeTimeItem>();

            try
            {
                UIList = GetListOfTeeTimes2(GroupGUID, ShowAllTeeTimes);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
            return UIList;
        }

        private List<TeeTimeItem> GetListOfTeeTimes2(string GroupGUID, bool ShowAllTeeTimes)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API01_GET_TEETIMES;
            TeeTimeItem ttItem;
            List<TeeTimeItem> TTList = new List<TeeTimeItem>();

            if (m.IsEmpty(GroupGUID))
            {
                GroupGUID = LookupDefaultGroupGUID();
            }

            aParamData[0] = GroupGUID;
            aParamData[1] = m.BolToStr(ShowAllTeeTimes);
            sAPIInput = m.ArrayToString(aParamData);

            oAPI.ActionCode = iAPINumber;
            oAPI.InputParameters = sAPIInput;
            oAPI.CallAPI(c.APIType.SENDDATA_RECEIVEDATA);
            if (oAPI.ErrorOccured)
            {
                ErrorMessage = oAPI.APIErrorMessage;
                SecretErrorDetails = oAPI.APIErrorSecretInfo;
                return TTList;
            }
            ds1 = oAPI.DatasetReturned;

            foreach (DataRow r in ds1.Tables[0].Rows)
            {
                ttItem = new TeeTimeItem();

                ttItem.TTGUID = m.GetFieldStr("TeeTimeGUID", r);
                ttItem.GroupGUID = m.GetFieldStr("GroupGUID", r);
                ttItem.GroupName = m.GetFieldStr("GroupName", r);
                ttItem.TTDate = m.GetFieldDate("TeeTimeDate", r);
                ttItem.TTTime = m.GetFieldDate("TeeTimeTime", r);
                ttItem.TTDateFormated = m.GetFieldDateF("TeeTimeDate", r, c.DATE_FORMAT20);
                ttItem.TTTimeFormated = m.GetFieldDateF("TeeTimeTime", r, c.DATE_FORMAT10).ToLower();
                ttItem.DaysFromNow = m.DaysFromNow(m.GetFieldDate("TeeTimeDate", r));
                ttItem.Owner = m.GetFieldStr("TeeTimeOwner", r);
                ttItem.OwnerComment = m.GetFieldStr("OwnerComment", r);
                ttItem.Location = m.GetFieldStr("TeeTimeLocation", r);
                ttItem.Player1 = m.GetFieldStr("Player1", r);
                ttItem.Player2 = m.GetFieldStr("Player2", r);
                ttItem.Player3 = m.GetFieldStr("Player3", r);
                ttItem.Player4 = m.GetFieldStr("Player4", r);
                ttItem.Player1Comment = m.GetFieldStr("Player1Comment", r);
                ttItem.Player2Comment = m.GetFieldStr("Player2Comment", r);
                ttItem.Player3Comment = m.GetFieldStr("Player3Comment", r);
                ttItem.Player4Comment = m.GetFieldStr("Player4Comment", r);
                ttItem.WaitList1 = m.GetFieldStr("WaitList1", r);
                ttItem.WaitList2 = m.GetFieldStr("WaitList2", r);
                ttItem.WaitList3 = m.GetFieldStr("WaitList3", r);
                ttItem.IsDeleted = m.GetFieldBol("IsDeleted", r);
                ttItem.LockPlayer1 = m.GetFieldBol("LockPlayer1", r);
                ttItem.LockPlayer2 = m.GetFieldBol("LockPlayer2", r);
                ttItem.LockPlayer3 = m.GetFieldBol("LockPlayer3", r);
                ttItem.LockPlayer4 = m.GetFieldBol("LockPlayer4", r);
                ttItem.HideMessages = m.GetFieldBol("HideMessages", r);
                ttItem.LockMessages = m.GetFieldBol("LockMessages", r);
                ttItem.SpotsAvailable = SpotsLeft(ttItem);     
                AddChatToTTItem(ttItem);

                TTList.Add(ttItem);
            }
            return TTList;
        }


        private string SpotsLeft(TeeTimeItem ttItem)
        {
            int iCount = 0;

            if (m.IsEmpty (ttItem.Player1))
            {
                iCount += 1;
            }

            if (m.IsEmpty(ttItem.Player2))
            {
                iCount += 1;
            }

            if (m.IsEmpty(ttItem.Player3))
            {
                iCount += 1;
            }

            if (m.IsEmpty(ttItem.Player4))
            {
                iCount += 1;
            }

            return iCount.ToString() + " spots avail.";
        }


        private void AddChatToTTItem(TeeTimeItem ttItem)
        {
            ChatVM cc = new ChatVM(m);
            ttItem.ChatList = cc.GetChatList(ttItem.TTGUID);
        }


        public void AddNewTeeTime(TeeTimeItem TT)
        {
            try
            {
                AddNewTeeTime2(TT);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void AddNewTeeTime2(TeeTimeItem TT)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API02_ADD_TEETIME;

            aParamData[0] = JsonSerializer.Serialize<TeeTimeItem>(TT);
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
                TT.ValidationFailMessage = m.GetFieldStr("ResultMsg", ds1);
            }

        }


        public void GetSingleTeeTime(TeeTimeItem TT)
        {
            try
            {
                GetSingleTeeTime2(TT);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void GetSingleTeeTime2(TeeTimeItem ttItem)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API01_GET_SINGLE_TEETIME;

            aParamData[0] = ttItem.TTGUID;
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
                ErrorMessage = "Error. TeeTime not found. (" + ttItem.TTGUID + ")";
                return;
            }

            ttItem.TTGUID = m.GetFieldStr("TeeTimeGUID", ds1);
            ttItem.GroupGUID = m.GetFieldStr("GroupGUID", ds1);
            ttItem.GroupName = m.GetFieldStr("GroupName", ds1);
            ttItem.TTDate = m.GetFieldDate("TeeTimeDate", ds1);
            ttItem.TTTime = m.GetFieldDate("TeeTimeTime", ds1);
            ttItem.TTDateFormated = m.GetFieldDateF("TeeTimeDate", ds1, c.DATE_FORMAT20);
            ttItem.TTTimeFormated = m.GetFieldDateF("TeeTimeTime", ds1, c.DATE_FORMAT10);
            ttItem.DaysFromNow = m.DaysFromNow(m.GetFieldDate("TeeTimeDate", ds1));
            ttItem.Owner = m.GetFieldStr("TeeTimeOwner", ds1);
            ttItem.OwnerComment = m.GetFieldStr("OwnerComment", ds1);
            ttItem.Location = m.GetFieldStr("TeeTimeLocation", ds1);
            ttItem.Player1 = m.GetFieldStr("Player1", ds1);
            ttItem.Player2 = m.GetFieldStr("Player2", ds1);
            ttItem.Player3 = m.GetFieldStr("Player3", ds1);
            ttItem.Player4 = m.GetFieldStr("Player4", ds1);
            ttItem.Player1Comment = m.GetFieldStr("Player1", ds1);
            ttItem.Player2Comment = m.GetFieldStr("Player2", ds1);
            ttItem.Player3Comment = m.GetFieldStr("Player3", ds1);
            ttItem.Player4Comment = m.GetFieldStr("Player4", ds1);
            ttItem.WaitList1 = m.GetFieldStr("WaitList1", ds1);
            ttItem.WaitList2 = m.GetFieldStr("WaitList2", ds1);
            ttItem.WaitList3 = m.GetFieldStr("WaitList3", ds1);
            ttItem.IsDeleted = m.GetFieldBol("IsDeleted", ds1);
            ttItem.LockPlayer1 = m.GetFieldBol("LockPlayer1", ds1);
            ttItem.LockPlayer2 = m.GetFieldBol("LockPlayer2", ds1);
            ttItem.LockPlayer3 = m.GetFieldBol("LockPlayer3", ds1);
            ttItem.LockPlayer4 = m.GetFieldBol("LockPlayer4", ds1);
            ttItem.HideMessages = m.GetFieldBol("HideMessages", ds1);
            ttItem.LockMessages = m.GetFieldBol("LockMessages", ds1);

        }



        public void UpdateTTPlayer(TeeTimeItem TT)
        {
            try
            {
                UpdateTTPlaye2(TT);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void UpdateTTPlaye2(TeeTimeItem TT)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API03_UPDATE_TT_PLAYER;

            aParamData[0] = JsonSerializer.Serialize<TeeTimeItem>(TT);
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



        public void TTDefaults(TeeTimeItem TT)
        {
            TT.TTDate = m.Now2();
            TT.TTTime = m.StringToDate("1/1/2000 8:00 AM") ;

        }



        public void UpdateTTInfo(TeeTimeItem TT)
        {
            try
            {
                UpdateTTInfo2(TT);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void UpdateTTInfo2(TeeTimeItem TT)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API03_UPDATE_TT_INFO;


            if (!m.IsPinCorrect(TT.PinNumberEntered, TT.GroupGUID))
            {
                TT.ValidationFailMessage = "Incorrect pin.";
                return;
            }

            aParamData[0] = JsonSerializer.Serialize<TeeTimeItem>(TT);
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
                TT.ValidationFailMessage = m.GetFieldStr("ResultMsg", ds1);
            }

        }



        public void MarkTTasDeleted(TeeTimeItem TT, bool bUndelete )
        {
            try
            {
                MardTTasDeleted2(TT, bUndelete);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void MardTTasDeleted2(TeeTimeItem TT, bool bUndelete)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API03_MARK_TT_AS_DELETED;

            if (!m.IsPinCorrect(TT.PinNumberEntered, TT.GroupGUID))
            {
                TT.ValidationFailMessage = "Incorrect pin.";
                return;
            }

            aParamData[0]  = JsonSerializer.Serialize<TeeTimeItem>(TT);
            aParamData[1]  = m.BolToStr(bUndelete);
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
                if (bUndelete)
                {
                    InfoMessage = "";
                }
                else
                {
                    InfoMessage = "";
                }
            }
            else
            {
                ErrorMessage = m.GetFieldStr("ResultMsg", ds1);
            }

        }





        public string LookupDefaultGroupGUID( )
        {
            string sSQL; 
            DataSet ds1;
            string sFirstRecGUID = "";
            bool bFirstRec = true;

            sSQL = "SELECT * FROM [W_Groups] ";
            sSQL = sSQL + " ORDER BY GroupName"  ;
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);

            foreach (DataRow r in ds1.Tables[0].Rows)
            {
                if (bFirstRec)
                {
                    sFirstRecGUID = m.GetFieldStr("GroupGUID", r);
                    bFirstRec = false;
                }

                if (m.GetFieldBol("GroupIsDefault", r))
                {
                    return m.GetFieldStr("GroupGUID", r);
                }
            }
            return sFirstRecGUID;
        }


    }
}

