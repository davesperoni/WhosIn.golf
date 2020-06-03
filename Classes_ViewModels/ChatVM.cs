using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhosIn.Classes_DataModels;
using System.Data;
using System.Text.Json;

namespace WhosIn.Classes_ViewModels
{
    public class ChatVM
    {
        readonly private cMain m;
        public string ErrorMessage { get; set; } = "";
        public string InfoMessage { get; set; } = "";
        public string SecretErrorDetails { get; set; } = "";
        public DataSet ds1;

        public ChatVM(cMain mIn)
        {
            m = mIn;
        }

        public List<ChatDM> GetChatList(string sTTGUID)
        {
            List<ChatDM> ccList = new List<ChatDM>();
            try
            {
                ccList = GetChatList2(sTTGUID);
                return ccList;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
                return ccList;
            }
        }

        private List<ChatDM> GetChatList2(string sTTGUID)
        {
            List<ChatDM> ccList = new List<ChatDM>();
            ChatDM ccItem;
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            aParamData[0] = sTTGUID;
            int iAPINumber = c.API06_GET_MESSAGES_LIST;
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
                ccItem = new ChatDM();
                ccItem.ChatGUID = m.GetFieldStr("ChatGUID", r);
                ccItem.DateTimeEntered = m.GetFieldDate("DateTimeEntered", r);
                ccItem.Message = m.GetFieldStr("Message", r);
                ccItem.IPAddress = m.GetFieldStr("IPAddress", r);
                ccItem.DisplayLine = m.GetPrettyDate(ccItem.DateTimeEntered) + " -- " + ccItem.Message;
                ccList.Add(ccItem);
            }

            return ccList;
        }



        public ChatDM GetSingleChatItem(string chatGUID)
        {
            ChatDM chatItem = new ChatDM();
            chatItem.ChatGUID = chatGUID;

            try
            {
                chatItem = GetSingleChatItem2(chatItem);
                return chatItem;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
                return chatItem;
            }
        }

        private ChatDM GetSingleChatItem2(ChatDM chatItem)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            aParamData[0] = chatItem.ChatGUID;
            int iAPINumber = c.API06_GET_SINGLE_MESSAGE;
            sAPIInput = m.ArrayToString(aParamData);

            oAPI.ActionCode = iAPINumber;
            oAPI.InputParameters = sAPIInput;
            oAPI.CallAPI(c.APIType.SENDDATA_RECEIVEDATA);
            if (oAPI.ErrorOccured)
            {
                ErrorMessage = oAPI.APIErrorMessage;
                SecretErrorDetails = oAPI.APIErrorSecretInfo;
                return chatItem;
            }
            ds1 = oAPI.DatasetReturned;

            if (!m.EmptyTable(ds1))
            {
                chatItem.DisplayLine = m.GetFieldStr("Message", ds1);
                chatItem.IPAddress = m.GetFieldStr("IPAddress", ds1);
                chatItem.DateTimeEntered = m.GetFieldDate("DateTimeEntered", ds1);
            }

            return chatItem;
        }



        public void AddChatItem(ChatDM cc)
        {
            try
            {
                AddChatItem2(cc);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void AddChatItem2(ChatDM cc)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API06_ADD_MESSAGE;


            if (m.IsEmpty(cc.Message))
            {
                cc.ValidationFailMessage = "Message cannot be blank.";
                return;
            }

            if (m.IsEmpty(cc.Initials))
            {
                cc.ValidationFailMessage = "Initials cannot be blank.";
                return;
            }

            aParamData[0] = JsonSerializer.Serialize<ChatDM>(cc);
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


        public void DeleteChatItem(string sGuid)
        {
            try
            {
                DeleteChatItem2(sGuid);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                SecretErrorDetails = m.ShortenStack(e.StackTrace, true);
            }
        }

        private void DeleteChatItem2(string sGuid)
        {
            string sAPIInput = "";
            APIRunner oAPI = new APIRunner(m);
            string[] aParamData = new string[10];
            int iAPINumber = c.API06_DELETE_MESSAGE;

            aParamData[0] = sGuid;
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
