using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace WhosIn
{
    //=========   Misc helper functions  ===================



    public partial class cMain
    {

        public Task DoNothingTask()
        {
            return Task.CompletedTask;
        }

        public DateTime CurrentTime()
        {
            return DateTime.Now;
        }

        public DateTime CurrentTimeAdjustedToUser()   
        {
            return CurrentTime();
        }

        public DateTime CurrentTimeOnly(int HoursFromServerTime)
        {
            //  Returns current time for the employee.  Sample return:  1850-01-01 05:15:00.000

            DateTime a = CurrentTime();
            DateTime b = DateWithTimeOnly(a);
            return b.AddHours(HoursFromServerTime);
        }

        public DateTime? DateWithTimeOnly(DateTime? dt)
        {
            DateTime dt2;
            if (dt == null)
            {
                return null;
            }
            dt2 = dt.GetValueOrDefault(DateTime.Parse(c.NODATE_STRING));
            dt2 = DateWithTimeOnly(dt2);
            return dt2;
        }

        public DateTime DateWithTimeOnly(DateTime a)
        {
            DateTime b = new DateTime(1850, 1, 1, a.Hour, a.Minute, 0);
            return b;
        }

        public DateTime DateWithTimeOnly(string sDateTime)
        {
            return DateWithTimeOnly(StringToDate(sDateTime));
        }

        public string RemoveCCSSFromUserName(string sLogonName)
        {
            string s1 = "";
            s1 = PartAfterColin(sLogonName);
            if (IsEmpty(s1))
            {
                return sLogonName;
            }
            else
            {
                return s1;
            }
        }

        public string RemoveSemiColon(string s1)
        {
            return s1.Replace(";", "$$$");
        }

        public string RightPad(string sText, int iPadTo)
        {
            string s1 = sText;

            if (iPadTo < 1)
            {
                return "";
            }

            s1 = s1.Trim();

            if (s1.Length >= iPadTo)
            {
                s1 = s1.Substring2(0, iPadTo);
            }
            else
            {
                s1 = s1 + new string(' ', iPadTo - s1.Length);
            }

            return s1;

        }

        public bool TrueFalse(string s1)
        {
            bool iTrue = false;

            if (s1 == null)
            {
                return false;
            }

            switch (s1.ToLower().Trim())
            {
                case "true":
                case "yes":
                case "-1":
                case "ok":
                case "1":
                case "y":
                    iTrue = true;
                    goto ExitRoutine;
            }

            if (Val(s1) > 0)
            {
                iTrue = true;
                goto ExitRoutine;
            }

            ExitRoutine:
            return iTrue;

        }

        public bool IsNumeric(object expression)
        {
            if (expression == null)
                return false;

            double testDouble;
            if (expression is string)
            {
                CultureInfo provider;
                if (((string)expression).StartsWith("$"))
                    provider = new CultureInfo("en-US");
                else
                    provider = CultureInfo.InvariantCulture;

                if (double.TryParse((string)expression, NumberStyles.Any, provider, out testDouble))
                    return true;
            }
            else
            {
                if (double.TryParse(expression.ToString(), out testDouble))
                    return true;
            }

            //VB's 'IsNumeric' returns true for any boolean value:
            bool testBool;
            if (bool.TryParse(expression.ToString(), out testBool))
                return true;

            return false;
        }

        public double Val(string expression)
        {
            if (expression == null)
                return 0;

            //try the entire string, then progressively smaller substrings to replicate the behavior of VB's 'Val', which ignores trailing characters after a recognizable value:
            for (int size = expression.Length; size > 0; size--)
            {
                double testDouble;
                if (double.TryParse(expression.Substring2(0, size), out testDouble))
                    return testDouble;
            }

            //no value is recognized, so return 0:
            return 0;
        }  //  val2 vs Val  ????

        public double Val(object expression)
        {
            if (expression == null)
                return 0;

            double testDouble;
            if (double.TryParse(expression.ToString(), out testDouble))
                return testDouble;

            //VB's 'Val' function returns -1 for 'true':
            bool testBool;
            if (bool.TryParse(expression.ToString(), out testBool))
                return testBool ? -1 : 0;

            //VB's 'Val' function returns the day of the month for dates:
            DateTime testDate;
            if (DateTime.TryParse(expression.ToString(), out testDate))
                return testDate.Day;

            //no value is recognized, so return 0:
            return 0;
        }

        public int Val(char expression)
        {
            int testInt;
            if (int.TryParse(expression.ToString(), out testInt))
                return testInt;
            else
                return 0;
        }

        public bool IsDate(object expression)
        {
            if (expression == null)
                return false;

            DateTime testDate;
            return DateTime.TryParse(expression.ToString(), out testDate);
        }

        public long DateDiff(c.DateInterval intervalType, string sDateOne, string sDateTwo)
        {
            DateTime t1;
            DateTime t2;

            if (!IsDate2(sDateOne) || !IsDate2(sDateTwo))
            {
                return 0;
            }

            t1 = StringToDate(sDateOne);
            t2 = StringToDate(sDateTwo);

            return DateDiff(intervalType, t1, t2);
        }

        public long DateDiff(c.DateInterval intervalType, DateTime dateOne, DateTime dateTwo)
        {
            switch (intervalType)
            {
                case c.DateInterval.Day:
                case c.DateInterval.DayOfYear:
                    TimeSpan spanForDays = dateTwo - dateOne;
                    return (long)spanForDays.TotalDays;
                case c.DateInterval.Hour:
                    TimeSpan spanForHours = dateTwo - dateOne;
                    return (long)spanForHours.TotalHours;
                case c.DateInterval.Minute:
                    TimeSpan spanForMinutes = dateTwo - dateOne;
                    return (long)spanForMinutes.TotalMinutes;
                case c.DateInterval.Month:
                    return ((dateTwo.Year - dateOne.Year) * 12) + (dateTwo.Month - dateOne.Month);
                case c.DateInterval.Quarter:
                    long dateOneQuarter = (long)Math.Ceiling(dateOne.Month / 3.0);
                    long dateTwoQuarter = (long)Math.Ceiling(dateTwo.Month / 3.0);
                    return (4 * (dateTwo.Year - dateOne.Year)) + dateTwoQuarter - dateOneQuarter;
                case c.DateInterval.Second:
                    TimeSpan spanForSeconds = dateTwo - dateOne;
                    return (long)spanForSeconds.TotalSeconds;
                case c.DateInterval.Weekday:
                    TimeSpan spanForWeekdays = dateTwo - dateOne;
                    return (long)(spanForWeekdays.TotalDays / 7.0);
                case c.DateInterval.WeekOfYear:
                    DateTime dateOneModified = dateOne;
                    DateTime dateTwoModified = dateTwo;
                    while (dateTwoModified.DayOfWeek != DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
                    {
                        dateTwoModified = dateTwoModified.AddDays(-1);
                    }
                    while (dateOneModified.DayOfWeek != DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
                    {
                        dateOneModified = dateOneModified.AddDays(-1);
                    }
                    TimeSpan spanForWeekOfYear = dateTwoModified - dateOneModified;
                    return (long)(spanForWeekOfYear.TotalDays / 7.0);
                case c.DateInterval.Year:
                    return dateTwo.Year - dateOne.Year;
                default:
                    return 0;
            }
        }

        public string BeginningOfDay(string sDate)
        {
            string sFormat = null;
            DateTime t1 = default(DateTime);
            string sNewDateTime = null;

            sDate = ValidDate(sDate);
            if (sDate.Trim().Length == 0)
            {
                return "";
            }
            sFormat = "M/dd/yyyy 00:00:00";
            t1 = DateTime.Parse(sDate);
            sNewDateTime = string.Format("{0:" + sFormat + "}", t1);

            return sNewDateTime;
        }

        public string EndOfDay(string sDate)
        {
            string sFormat = null;
            DateTime t1 = default(DateTime);
            string sNewDateTime = null;

            sDate = ValidDate(sDate);
            if (sDate.Trim().Length == 0)
            {
                return "";
            }
            sFormat = "M/dd/yyyy 23:59:59";
            t1 = DateTime.Parse(sDate);
            sNewDateTime = string.Format("{0:" + sFormat + "}", t1);

            return sNewDateTime;
        }

        public DateTime EndOfDay(DateTime tDate)
        {
            string sFormat = "M/dd/yyyy 23:59:59";

            return DateTime.Parse(string.Format("{0:" + sFormat + "}", tDate));
        }

        public DateTime BeginningOfDay(DateTime tDate)
        {
            string sFormat = "M/dd/yyyy 00:00:00";

            return DateTime.Parse(string.Format("{0:" + sFormat + "}", tDate));
        }

        public string FixAM(string s1)
        {

            s1 = s1.Replace(" AM", " am");
            s1 = s1.Replace(" PM", " pm");

            return s1;

        }

        public string InQuoteTNoTime(string s1)
        {
            string tempInQuoteTNoTime = null;

            s1 = ValidDate(s1, 1);

            if (s1.Trim().Length == 0)
            {
                tempInQuoteTNoTime = "Null";
            }
            else
            {
                tempInQuoteTNoTime = (char)39 + s1 + (char)39; //single quote
            }

            return tempInQuoteTNoTime;
        }

        public string InQuoteTNoTime(DateTime tDate1)
        {
            string s1 = null;

            try
            {
                if (IsDate(tDate1))
                {
                    s1 = string.Format("{0:M/dd/yyyy}", tDate1);
                }
                else
                {
                    s1 = "";
                }
            }
            catch
            {
                s1 = "";
            }

            return InQuoteT(s1);

        }

        private string RandomTwoCharacters()
        {
            int iValue = 0;
            string s1 = null;
            string s2 = null;

            iValue = GetRandomNumber(66, 89);
            s1 = Char.ConvertFromUtf32(iValue);

            iValue = GetRandomNumber(66, 89);
            s2 = Char.ConvertFromUtf32(iValue);

            return s1.ToLower() + s2.ToLower();
        }

        public int GetRandomNumber(int iMin, int iMax)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int rInt = r.Next(iMin, iMax + 1);
            return rInt;
        }

        public bool FolderExists(string sFolder)
        {
            bool tempVar = false;
            return FolderExists(sFolder, tempVar);
        }

        public bool FolderExists(string sFolder, bool bCreate)
        {
            bool bOk = false;
            try
            {
                if (Directory.Exists(sFolder))
                {
                    bOk = true;
                }
                else
                {
                    if (bCreate)
                    {
                        Directory.CreateDirectory(sFolder);
                        bOk = true;
                    }
                    else
                    {
                        bOk = false;
                    }
                }
            }
            catch
            {
            }
            return bOk;
        }

        public bool SysOption(int i, string sSwitchesFromSetup)
        {
            bool b = false;

            try
            {
                b = ToBol(sSwitchesFromSetup.Substring2(i - 1, 1));
            }
            catch
            {
                b = false;
            }

            return b;

        }

        public DataTable SortDataTable(DataTable dt1, string sSortField, string sFilter)
        {
            DataView dv = null;
            DataRow newRow = null;
            DataTable dtTemp = null;
            int i = 0;

            dv = new DataView(dt1);
            dv.RowFilter = sFilter;
            dv.Sort = sSortField;

            dtTemp = (DataTable)dt1.Clone();

            foreach (DataRowView drv in dv)
            {
                newRow = dtTemp.NewRow();
                for (i = 0; i < dtTemp.Columns.Count; i++)
                {
                    newRow[i] = drv[i];
                }
                dtTemp.Rows.Add(newRow);
            }

            return dtTemp;
        }

        public string TrueFalse3(string s1)
        {
            string tempTrueFalse3 = null;
            bool bTrue = false;


            if (s1 == null)
            {
                s1 = "0";
            }

            switch (s1.ToLower().Trim())
            {
                case "true":
                case "yes":
                case "-1":
                case "ok":
                case "1":
                    bTrue = true;
                    goto ExitRoutine;
            }

            if (Val(s1) > 0)
            {
                bTrue = true;
                goto ExitRoutine;
            }

            ExitRoutine:
            if (bTrue)
            {
                tempTrueFalse3 = "-1";
            }
            else
            {
                tempTrueFalse3 = "0";
            }

            return tempTrueFalse3;
        }

        public bool DeleteOldFilesInTempFolder(int iMaxCount)
        {
            DirectoryInfo di = null;
            ArrayList al = new ArrayList();
            FileInfo[] afi = null;
            int iDeletedCount = 0;
            int iDaysToKeep = 10; // delete only files that are older than 10 days.

            di = new DirectoryInfo(System.IO.Path.GetTempPath());
            afi = di.GetFiles();

            foreach (FileInfo fi in afi)
            {
                try
                {
                    if (DateDiff(c.DateInterval.Day, fi.LastWriteTime, DateTime.Now) > iDaysToKeep)
                    {
                        fi.Delete();
                        iDeletedCount = iDeletedCount + 1;
                    }
                }
                catch
                {
                }

                if (iDeletedCount >= iMaxCount)
                {
                    break;
                }
            }
            return false;
        }

        public bool FileExists(string FileFullPath)
        {
            try
            {
                System.IO.FileInfo f = new System.IO.FileInfo(FileFullPath);
                return f.Exists;
            }
            catch
            {
                return false;
            }
        }

        public string GetATempFileName(string sExt = "")
        {
            string s1 = null;
            string s2 = null;
            const string TEMP_INDICATOR = "temp5"; //put 'temp5' in the filename all temp files created. So they can be identified and deleted later.

            //Returns a name only, the file will not exist.

            if (sExt.Trim().Length == 0)
            {
                sExt = ".tmp";
            }
            if (sExt.Substring2(0, 1) != ".")
            {
                sExt = "." + sExt;
            }

            s1 = Path.GetTempFileName();
            s2 = Path.GetDirectoryName(s1) + "\\" + Path.GetFileNameWithoutExtension(s1) + TEMP_INDICATOR + sExt;
            DeleteThisFile(s1);
            DeleteThisFile(s2);
            return s2;
        }

        public string NameOnly(string sFilename)
        {
            FileInfo fi = null;
            try
            {
                fi = new FileInfo(sFilename);
                return fi.Name;
            }
            catch
            {
                return "";
            }
        }

        public string ConvertDataTableToHTML(DataTable dt)
        {

            if (EmptyTable(dt))
            {
                return "";
            }

            string html = "<table border='1px' cellpadding='1' cellspacing='1' bgcolor='lightyellow' style='font-family:Garamond; font-size:smaller'>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }

        public string CombineParametes(string sParam1, string sParam2, string sParam3 = "")
        {
            string s1;
            s1 = sParam1 + c.MULTI_FIELD_DELIMETER + sParam2 + c.MULTI_FIELD_DELIMETER + sParam3 + c.MULTI_FIELD_DELIMETER + sParam3 + c.MULTI_FIELD_DELIMETER;
            return s1;
        }

        public string GetParamItem(string sData, int iPart)
        {
            return GetPiece(sData, iPart);
        }

    }
}

