using System;
using System.Data;
using WhosIn.API;

namespace WhosIn
{
    public class APIRunner
    {
        private cMain m { get; set; }
        public APIErrorHandler errorHandler { get; set; }
        public DataSet DatasetReturned { get; set; } = new DataSet();
        private int iTableCount { get; set; } = 0;
        public int ActionCode { get; set; } = 0;
        public string InputParameters { get; set; } = "";

        public string APIErrorMessage { get; set; } = "";
        public string APIErrorSecretInfo { get; set; } = "";
        public string APISucessMessage { get; set; } = "";

        public byte[] InputByteArray { get; set; } = new byte[] { 0x20 };
        public byte[] OutputByteArray { get; set; }

        public APIRunner(cMain mIn)
        {
            m = mIn;
            errorHandler = new APIErrorHandler(mIn);
        }

        public void CallAPI(c.APIType iCallType)
        {
            errorHandler.APIActionCode = ActionCode;

            try
            {
                switch (iCallType)
                {
                    case c.APIType.SENDDATA_RECEIVEDATA:
                        SendDataRecieveData();
                        break;

                    case c.APIType.SENDDATA_RECEIVEIMAGE:
                        SendDataReceiveImage();
                        break;

                    case c.APIType.SENDIMAGE_RECEIVEDATA:
                        SendImageReceiveData();
                        break;

                    default:
                        APIErrorMessage = "API called with invalid type.";
                        break;
                }
            }
            catch (Exception e)
            {
                errorHandler.AddErrorFromException(e);
                APIErrorMessage =  errorHandler.FormatedErrMsg();
                APIErrorSecretInfo = errorHandler.FormatedSecret();
                return;
            }

            // This is to check for a manually entered error (not from an exception).
            // This error would be entered in the API using errorHandler.AddErrorManually
            if (!m.IsEmpty(errorHandler.FriendlyMsg))
            {
                APIErrorMessage = errorHandler.FriendlyMsg;
            }

        }

        private void SendDataRecieveData()
        {
            ExecuteTheAPI();
        }

        private void SendDataReceiveImage()
        {
            //    ApiSendDataReceiveImage api = new ApiSendDataReceiveImage();
            //    api.DatasetSend = _dsSend;
            //    api.Proxy = _Proxy;
            //    api.Start();
            //    ImageReturned = api.ImageReturned;
            //    _dsReturned = api.DatasetReturned;
        }

        private void SendImageReceiveData()
        {
            //    ApiSendImageReceiveData api = new ApiSendImageReceiveData();
            //    api.DatasetSend = _dsSend;
            //    api.ImageSend = ImageToSend;
            //    api.Proxy = _Proxy;
            //    api.Start();
            //    _dsReturned = api.DatasetReturned;
            //}
        }

        private void ExecuteTheAPI()
        {
            switch (ActionCode)
            {

                case c.API01_GET_TEETIMES:
                    API01 A1 = new API01(m, this);
                    A1.API01_GET_TEETIMES();
                    break;

                case c.API01_GET_SINGLE_TEETIME:
                    API01 A2 = new API01(m, this);
                    A2.API01_GET_SINGLE_TEETIME();
                    break;

                case c.API02_ADD_TEETIME:    
                    API02 A3 = new API02(m, this);
                    A3.API02_ADD_TEETIME();
                    break;

                case c.API03_UPDATE_TT_PLAYER:
                    API03 A4 = new API03(m, this);
                    A4.API03_UPDATE_TT_PLAYER();
                    break;

                case c.API03_UPDATE_TT_INFO:
                    API03 A5 = new API03(m, this);
                    A5.API03_UPDATE_TT_INFO();
                    break;

                case c.API03_MARK_TT_AS_DELETED:
                    API03 A6 = new API03(m, this);
                    A6.API03_MARK_TT_AS_DELETED();
                    break;

                case c.API04_GET_GROUPS:
                    API04 A8 = new API04(m, this);
                    A8.API04_GET_GROUPS();
                    break;

                case c.API04_GET_SINGLE_GROUP:
                    API04 A9 = new API04(m, this);
                    A9.API04_GET_SINGLE_GROUP();
                    break;

                case c.API04_UPDATE_GROUP:
                    API04 A10 = new API04(m, this);
                    A10.API04_UPDATE_GROUP();
                    break;

                case c.API04_DELETE_GROUP:
                    API04 A11 = new API04(m, this);
                    A11.API04_DELETE_GROUP();
                    break;

                case c.API04_ADD_GROUP:
                    API04 A12 = new API04(m, this);
                    A12.API04_ADD_GROUP();
                    break;

                case c.API05_UPDATE_SETTINGS:
                    API05 A13 = new API05(m, this);
                    A13.API05_UPDATE_SETTINGS();
                    break;

                case c.API05_READ_SETTINGS:
                    API05 A14 = new API05(m, this);
                    A14.API05_READ_SETTINGS();
                    break;

                case c.API06_GET_MESSAGES_LIST:
                    API06 A15 = new API06(m, this);
                    A15.API06_GET_MESSAGES_LIST();
                    break;

                case c.API06_GET_SINGLE_MESSAGE:
                    API06 A16 = new API06(m, this);
                    A16.API06_GET_SINGLE_MESSAGE();
                    break;

                case c.API06_ADD_MESSAGE:
                    API06 A17 = new API06(m, this);
                    A17.API06_ADD_MESSAGE();
                    break;

                case c.API06_DELETE_MESSAGE:
                    API06 A18 = new API06(m, this);
                    A18.API06_DELETE_MESSAGE();
                    break;

                case c.API07_GET_AUDITLOG:
                    API07 A19 = new API07(m, this);
                    A19.API07_GET_AUDITLOG();
                    break;
                    
                default:
                    APIErrorMessage = "An invalid API code was called. (" + ActionCode.ToString() + ")";
                    break;

            }
        }

        public bool ErrorOccured
        {
            get
            {
                return !m.IsEmpty(APIErrorMessage);
            }
        }

        public void AddErrorManually(string ErrMsg)
        {
            errorHandler.AddErrorManually(ErrMsg);
        }

        public void AppendTableToDataset(DataTable dtAdd)
        {
            DataTable dt1;
            iTableCount = (iTableCount + 1);
            dt1 = dtAdd.Copy();
            dt1.TableName = (ActionCode.ToString() + "_" + iTableCount.ToString());
            DatasetReturned.Tables.Add(dt1);
        }

        public void AppendTableToDataset(DataSet ds2)
        {
            DataTable dt1;
            iTableCount = (iTableCount + 1);
            dt1 = ds2.Tables[0].Copy();
            dt1.TableName = (ActionCode.ToString() + "_" + iTableCount.ToString());
            DatasetReturned.Tables.Add(dt1);
        }

    }
}
