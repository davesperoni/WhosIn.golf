using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhosIn
{
    public class c
    {
        public const string PAGEMESSAGE = "PageMessage";
        public const string PAGEERROR = "PageError";
        public const string PAGEEXTRAINFO1 = "PageExtraInfo1";

        //  ---------------------------   API 
        public const int API01_GET_TEETIMES = 10;    
        public const int API01_GET_SINGLE_TEETIME = 20;   
        public const int API02_ADD_TEETIME = 30;
        public const int API03_UPDATE_TT_PLAYER = 40;    
        public const int API03_UPDATE_TT_INFO = 50;   
        public const int API03_MARK_TT_AS_DELETED = 60;
        public const int API04_GET_GROUPS = 80;
        public const int API04_GET_SINGLE_GROUP = 90;
        public const int API04_UPDATE_GROUP = 100;
        public const int API04_DELETE_GROUP = 110;
        public const int API04_ADD_GROUP = 120;
        public const int API05_UPDATE_SETTINGS = 130;
        public const int API05_READ_SETTINGS = 140;
        public const int API06_GET_MESSAGES_LIST = 150;
        public const int API06_ADD_MESSAGE = 160;
        public const int API06_DELETE_MESSAGE = 170;
        public const int API06_GET_SINGLE_MESSAGE = 180;
        public const int API07_GET_AUDITLOG = 190;
        //  ---------------------------    

        public const string MULTI_FIELD_DELIMETER = "<~>";
        public const string MULTI_FIELD_DELIMETER2 = "<`>";
        public const string MULTI_FIELD_DELIMETER3 = "<^>";
        public const string MULTI_FIELD_DELIMETER4 = "<=>";

        public const string DATE_FORMAT1 = "MM/dd/yyyy";
        public const string DATE_FORMAT2 = "MM/dd/yyyy  (ddd)";
        public const string DATE_FORMAT3 = "M/dd/yyyy";
        public const string DATE_FORMAT4 = "MM/dd/yyyy HH:mm:ss";
        public const string DATE_FORMAT5 = "M/dd/yyyy h:mm tt";
        public const string DATE_FORMAT6 = "ddd  M/dd/yyyy h:mm tt";
        public const string DATE_FORMAT7 = "M/dd/yyyy h:mm tt";
        public const string DATE_FORMAT8 = "MM/dd/yyyy HH:mm";
        public const string DATE_FORMAT9 = "M/dd/yy";
        public const string DATE_FORMAT10 = "h:mm tt";
        public const string DATE_FORMAT11 = "dddd  M/dd/yyyy";
        public const string DATE_FORMAT12 = "dddd, MMMM d, yyyy";
        public const string DATE_FORMAT13 = "yyyy-MM-dd h:mm tt";
        public const string DATE_FORMAT14 = "ddd, MMM d, h:mm tt";
        public const string DATE_FORMAT15 = "MMMM-d";
        public const string DATE_FORMAT16 = "dddd  M/dd/yyyy h:mm tt";
        public const string DATE_FORMAT17 = "MMM d";
        public const string DATE_FORMAT18 = "ddd, MMM d, yyyy";
        public const string DATE_FORMAT19 = "MMM d, h:mm tt";
        public const string DATE_FORMAT20 = "dddd, MMM dd";
        public const string DATE_FORMAT21 = "M/dd h:mm tt";
        public const string DATE_FORMAT22 = "ddd, MMM dd";

        public const string BATCH_RESULT_OK = "-1";
        public const string BATCH_RESULT_FAIL = "FAIL";
        public const string BATCH_RESULT_FAIL_QUIT = "FAIL_QUIT";
        public const string BATCH_RESULT_WARNING = "WARN";

        public const string NODATE_STRING = "1/1/1900";
        public const string RESULT_OK = "OK";
        public const string RESULT_FAIL = "FAIL";


        public enum DateInterval
        {
            Day,
            DayOfYear,
            Hour,
            Minute,
            Month,
            Quarter,
            Second,
            Weekday,
            WeekOfYear,
            Year
        }


        public enum APIType : int
        {
            SENDDATA_RECEIVEDATA = 1,
            SENDDATA_RECEIVEIMAGE = 2,
            SENDIMAGE_RECEIVEDATA = 3
        }

        public enum DB : int
        {
            WhosIn = 1,
            NotUsed = 2
        }

        public const int REPORT_FORMAT_REGULAR = 1;
        public const int REPORT_FORMAT_HTML = 2;
        public const int REPORT_FORMAT_FILE = 3;
        public const int REPORT_FORMAT_EXCEL = 4;
        public const int REPORT_FORMAT_XML = 5;
        public const int REPORT_FORMAT_WORD = 6;
        public const int REPORT_FORMAT_ACROBAT = 7;

    }

}
