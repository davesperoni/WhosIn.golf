using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace WhosIn
{
    //=========   Misc helper functions  ===================

    public partial class cMain
    {
 
        public bool IsEmpty(string s1)
        {
            if (s1 == null)
            {
                return true;
            }
            return string.IsNullOrEmpty(s1.Trim());
        }

        public bool IsEmptyOrZero(string s1)
        {
            if (s1 == null)
            {
                return true;
            }

            s1 = s1.Trim();

            if (Val(s1) == 0)
            {
                return true;
            }

            return string.IsNullOrEmpty(s1);
        }

        public bool StringContains(string s1, string sSearchFor)
        {

            if (s1.ToLower().Contains(sSearchFor.ToLower()))
            {
                return true;
            }

            return false;
        }

        public string Replace2(string sExp, string sFind, string sReplaceWith)
        {
            sExp = NoNull(sExp);
            sFind = NoNull(sFind);
            sReplaceWith = NoNull(sReplaceWith);
            if (IsEmpty(sExp) || IsEmpty(sFind))
            {
                return sExp;
            }
            return NoNull(sExp.Replace(sFind, sReplaceWith));
        }

        public bool ToBol(string s1)
        {
            bool iTrue = false;
            s1 = NoNull(s1);

            switch (s1.Trim().ToLower())
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
            if (Val2(s1) != 0)
            {
                iTrue = true;
                goto ExitRoutine;
            }
            ExitRoutine:
            return iTrue;
        }

        public bool ToBol(int i)
        {
            if (i != 0)
            {
                return true;
            }
            return false;
        }

        public string NoNull(string s1)
        {
            try
            {
                if (s1 == null)
                {
                    return "";
                }
                else
                {
                    return s1;
                }
            }
            catch
            {
                return "";
            }
        }

        public string NoNullObject(object o)
        {
            try
            {
                if (o == null)
                {
                    return "";
                }
                else
                {
                    return o.ToString();
                }
            }
            catch
            {
                return "";
            }
        }

        public DateTime NoNullDate(DateTime? dateTime)
        {
            return dateTime.GetValueOrDefault(DateTime.Parse(c.NODATE_STRING));
        }

        public bool DateIsEmpty(DateTime? dt)
        {
            if (dt == null)
            {
                return true;
            }
            else
            {
                return DateIsEmpty(NoNullDate(dt));
            }
        }

        public bool DateIsEmpty(DateTime dt)
        {
            if (dt == DateTime.MinValue || dt == StringToDate(c.NODATE_STRING))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string[] NoNullArray(string[] a1, int iMinimumIndex)
        {
            int i;

            if (a1.Length < iMinimumIndex + 1)
            {
                Array.Resize(ref a1, iMinimumIndex + 1);
            }

            for (i = 0; i <= a1.Length - 1; i++)
            {
                a1[i] = NoNull(a1[i]);
            }

            return a1;
        }

        public int ToInt(long l2)
        {
            return ToInt(l2.ToString());
        }

        public int ToInt(string s1)
        {
            return ToInt2(s1);
        }

        public string DateTimeToString(DateTime? dt)
        {
            if (dt == null)
            {
                return "";
            }
            return FormatDT(dt, c.DATE_FORMAT7);
        }


        public string FormatDT(string s1, string sFormat)
        {
            try
            {
                return String.Format("{0:" + sFormat + "}", StringToDate(s1));
            }
            catch
            {
                return "";
            }
        }

        public string FormatDT(DateTime? t1, string sFormat)
        {
            try
            {
                return String.Format("{0:" + sFormat + "}", t1);
            }
            catch
            {
                return "";
            }
        }

        public string FormatDT(DateTime? t1 )
        {
            string sFormat = c.DATE_FORMAT1;
            return FormatDT(t1, sFormat);
        }


        public string FormatTwoDecIfNumeric(string s1)
        {
            if (IsNumeric2(s1))
            {
                return Format2(s1, 2);
            }
            else
            {
                return s1;
            }
        }


        public string BolToStr(bool b1)
        {

            if (b1)
            {
                return "1";
            }
            else
            {
                return "0";
            }

        }

        public bool stringsAreEqual(string s1, string s2)
        {
            s1 = NoNull(s1);
            s2 = NoNull(s2);

            if (s1.Trim().ToLower() == s2.Trim().ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DateTimeStringsAreSame(string sDT1, string sDT2)
        {
            DateTime t1;
            DateTime t2;

            t1 = StringToDate(sDT1);
            t2 = StringToDate(sDT2);
            return (t1 == t2);
        }

        public string FileMustUseThisExtention(string sFileName, string sExtention)
        {

            if (IsEmpty(sExtention) || IsEmpty(sFileName))
            {
                //  f2.Open903ErrorOccured("Error calling FileMustUseThisExtention().  Empty parameter. ");
                return sFileName;
            }

            if (sExtention.Substring2(0, 1) != ".")
            {
                sExtention = "." + sExtention;
            }

            sFileName = FileNameWithoutExtension(sFileName) + sExtention;
            return sFileName;
        }

        public string FileNameWithoutExtension(string sFilename)
        {
            return System.IO.Path.GetFileNameWithoutExtension(sFilename);
        }

        public string ExtentionOfFile(string sFilename)
        {
            FileInfo fi = default;
            try
            {
                fi = new FileInfo(sFilename);
                return fi.Extension;
            }
            catch
            {
                return "";
            }
        }

        public byte[] GetPictureFileAsBytes(string sFileName)
        {
            byte[] b;

            FileStream fs1 = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs1);
            b = br.ReadBytes(ToInt(fs1.Length));
            br.Close();
            br = null;
            fs1.Close();
            fs1 = null;

            return b;
        }

        public bool DeleteThisFile(string sFilename, bool bUseKill = false)
        {
            FileInfo fi = default;

            try
            {
                if (bUseKill)
                {
                    HardDeleteFile(sFilename);
                }
                else
                {
                    fi = new FileInfo(sFilename);
                    fi.Delete();
                }
                return true;

            }
            catch
            {
                return false;
            }

        }

        public string Left2(string s1, int iCount)
        {
            int iLen;
            s1 = NoNull(s1).Trim();

            iLen = s1.Length;

            if (iCount > iLen)
            {
                return s1.Substring2(0, iLen);
            }
            else
            {
                return s1.Substring2(0, iCount);
            }

        }

        public bool ValidSSN(string s1)
        {
            s1 = s1.Trim();

            if (IsEmpty(s1))
            { return false; }

            if (s1.Length != 11)
            { return false; }

            if (Mid2(s1, 4, 1) != "-")
            { return false; }

            if (Mid2(s1, 7, 1) != "-")
            { return false; }

            if (!IsNumeric2(Mid2(s1, 1, 3)))
            { return false; }

            if (!IsNumeric2(Mid2(s1, 5, 2)))
            { return false; }

            if (!IsNumeric2(Mid2(s1, 8, 4)))
            { return false; }

            return true;
        }

        public string GetCustomCode(string sKey, string OptionalCustomCodes = "")
        {
            string[] codes;
            string sCodeName;
            string sCodeValue;


            if (!IsEmpty(OptionalCustomCodes))
            {
                codes = parseToArrayBySemiColon(OptionalCustomCodes);
            }
            else
            {
                codes = parseToArrayBySemiColon(AllCustomCodes());
            }

            foreach (string sSingleCode in codes)
            {
                sCodeName = PartBeforeEqualSign(sSingleCode);
                sCodeValue = PartAfterEqualSign(sSingleCode);
                if (stringsAreEqual(sCodeName, sKey))
                {
                    return sCodeValue;
                }
            }

            return "";
        }

        public bool GetCustomCodeBool(string sKey, string OptionalCustomCodes = "")
        {
            return ToBol(GetCustomCode(sKey, OptionalCustomCodes));
        }

        public string PartBeforeEqualSign(string s1)
        {
            char[] delimiterChars = { '=' };
            string[] words = s1.Split(delimiterChars);

            words = NoNullArray(words, 2);
            return words[0];
        }

        public string PartAfterEqualSign(string s1)
        {
            s1 = NoNull(s1);
            char[] delimiterChars = { '=' };
            string[] words = s1.Split(delimiterChars);

            words = NoNullArray(words, 2);
            return words[1];
        }

        public string PartAfterColin(string s1)
        {
            s1 = NoNull(s1);
            char[] delimiterChars = { ':' };
            string[] words = s1.Split(delimiterChars);

            words = NoNullArray(words, 2);
            return words[1];
        }

        public string PartBeforeDelimiter(string s1, string sDelimeter)
        {
            s1 = NoNull(s1);
            sDelimeter = NoNull(sDelimeter);
            string[] partsFromString = s1.Split(new string[] { sDelimeter }, StringSplitOptions.None);

            if (partsFromString.Length == 0)
            {
                return s1;
            }
            return partsFromString[0].Trim();
        }

        public void SplitByDelimter2(string s1, string sDelimeter, out string sPart1, out string sPart2)
        {
            s1 = NoNull(s1);
            sPart1 = "";
            sPart2 = "";
            sDelimeter = NoNull(sDelimeter);
            string[] partsFromString = s1.Split(new string[] { sDelimeter }, StringSplitOptions.None);
            if (partsFromString.Length == 0)
            {
                return;
            }

            if (partsFromString.Length == 1)
            {
                sPart1 = partsFromString[0].Trim();
                sPart2 = partsFromString[0].Trim();
                return;
            }
            else
            {
                sPart1 = partsFromString[0].Trim();
                sPart2 = partsFromString[1].Trim();
                return;
            }
        }

        public void SplitByDelimter3(string s1, string sDelimeter, out string sPart1, out string sPart2, out string sPart3)
        {
            s1 = NoNull(s1);
            sPart1 = "";
            sPart2 = "";
            sPart3 = "";
            sDelimeter = NoNull(sDelimeter);
            string[] partsFromString = s1.Split(new string[] { sDelimeter }, StringSplitOptions.None);
            if (partsFromString.Length == 0)
            {
                return;
            }

            if (partsFromString.Length == 1)
            {
                sPart1 = partsFromString[0].Trim();
                sPart2 = partsFromString[0].Trim();
                return;
            }
            else
            {
                if (partsFromString.Length == 2)
                {
                    sPart1 = partsFromString[0].Trim();
                    sPart2 = partsFromString[1].Trim();
                    return;
                }
                else
                {
                    sPart1 = partsFromString[0].Trim();
                    sPart2 = partsFromString[1].Trim();
                    sPart3 = partsFromString[2].Trim();
                    return;
                }
            }



        }

        public string[] parseToArrayByDelimter(string s1, string sDelimeter)
        {
            s1 = NoNull(s1);
            string[] partsFromString = s1.Split(new string[] { sDelimeter }, StringSplitOptions.None);

            for (int i = 0; i < partsFromString.Length; i++)
            {
                partsFromString[i] = partsFromString[i].Trim();
            }

            return partsFromString;
        }

        public List<string> parseToListByDelimterAndSubDelimter(string s1, string sDelimeter, string sSubDelimeter)
        {
            //  delimtes by string, then takes left side of second parse
            //  Name<`>LastName,FirstName,MidName <~>   EmpNo<`>EmpNo   <~>   Dept,Name<`>Dept,Lastname,Firstname,MidName   <~>   Dept,EmpNo<`>Dept,EmpNo  <~>    Returns--->  Name  EmpNo  Dept,Name  Dept,EmpNo

            s1 = NoNull(s1);
            List<string> returnList = new List<string>();
            string[] parts2;
            string[] partsFromString = s1.Split(new string[] { sDelimeter }, StringSplitOptions.None);

            for (int i = 0; i < partsFromString.Length; i++)
            {
                if (!IsEmpty(partsFromString[i].Trim()))
                {
                    parts2 = partsFromString[i].Trim().Split(new string[] { sSubDelimeter }, StringSplitOptions.None);
                    if (!IsEmpty(parts2[0]))
                    {
                        returnList.Add(parts2[0]);
                    }

                }

            }

            return returnList;
        }

        public string[] parseToArrayBySemiColon(string s1)
        {
            s1 = NoNull(s1);
            char[] delimiterChars = { ';' };
            string[] words = s1.Split(delimiterChars).Select(e => e.Trim()).ToArray();
            return words;
        }

        public string[] parseToArrayByComma(string s1)
        {
            s1 = NoNull(s1);
            char[] delimiterChars = { ',' };
            string[] words = s1.Split(delimiterChars).Select(e => e.Trim()).ToArray();
            return words;
        }

        public string[] parseToArrayByPipe(string s1)
        {
            s1 = NoNull(s1);
            char[] delimiterChars = { '|' };
            string[] words = s1.Split(delimiterChars).Select(e => e.Trim()).ToArray();
            return words;
        }

        public string GetPiece(string sText, int iPiece, string sDel= c.MULTI_FIELD_DELIMETER)
        {
            string[] temp1 = null;
            string s1 = null;
            temp1 = new string[iPiece + 1];

            ParseToArray(sText, ref temp1, sDel);
            s1 = temp1[iPiece - 1];

            return NoNull(s1);
        }    

        public void ParseToArray(string sLine, ref string[] a1, string sD)
        {
            int iLastFound = 0;
            int iPos = 0;
            int iMax = 0;
            int iLow = 0;
            int iLenDel = 0;
            int iIndex = 0;

            iMax = a1.GetUpperBound(0);
            iLow = a1.GetLowerBound(0);
            iLastFound = 0;
            iLenDel = sD.Length;

            for (iIndex = iLow; iIndex <= iMax; iIndex++)
            {

                if (iLastFound == 0)
                {
                    iPos = sLine.IndexOf(sD, iLastFound) + 1;
                }
                else
                {
                    iPos = sLine.IndexOf(sD, (iLastFound + iLenDel) - 1) + 1;
                }

                if (iPos == 0)
                {
                    if (iLastFound == 0)
                    {
                        a1[iIndex] = sLine.Trim();
                    }
                    else
                    {
                        a1[iIndex] = (sLine.Substring2((iLastFound + iLenDel) - 1)).Trim();
                    }
                    break;
                }

                if (iLastFound == 0)
                {
                    a1[iIndex] = (sLine.Substring2(0, iPos - 1)).Trim();
                }
                else
                {
                    a1[iIndex] = (sLine.Substring2((iLastFound + iLenDel) - 1, iPos - (iLastFound + iLenDel))).Trim();
                }


                iLastFound = iPos;
            }

        }  

        public string LeftOf(string s, string sDelimiter)
        {
            string[] res = s.Split(new string[] { sDelimiter }, StringSplitOptions.None);
            return res[0];
        }

        public string ValidDate(string s1, int iFormatType = 1)
        {
            string v1 = "";
            string sFormat = "";
            DateTime tDate;
            string sDate = "";
            int iYear;

            if (IsEmpty(s1))
            {
                return "";
            }

            switch (iFormatType)
            {
                case 1:
                    sFormat = c.DATE_FORMAT1;
                    break;
                case 2:
                    sFormat = c.DATE_FORMAT8;
                    break;
                case 3:
                    sFormat = c.DATE_FORMAT9;
                    break;
                case 4:
                    sFormat = c.DATE_FORMAT4;
                    break;
                case 5:
                    sFormat = c.DATE_FORMAT5;
                    break;
                case 12:
                    sFormat = c.DATE_FORMAT12;
                    break;
            }


            v1 = s1.Trim();
            if (IsNumeric2(v1) && v1.Length == 6)
            {
                v1 = Mid2(v1, 1, 2) + "/" + Mid2(v1, 3, 2) + "/" + Mid2(v1, 5, 2);
            }

            if (IsNumeric2(v1) && v1.Length == 8)
            {
                v1 = Mid2(v1, 1, 2) + "/" + Mid2(v1, 3, 2) + "/" + Mid2(v1, 5, 4);
            }


            if (!IsDate2(v1))
            {
                return "";
            }

            tDate = StringToDate(v1);

            // 'the minumum year is 1850
            iYear = ToInt(DatePart2(tDate, 7));
            if (iYear < 1850)
            {
                sDate = c.NODATE_STRING;
            }

            sDate = String.Format("{0:" + sFormat + "}", tDate);
            return sDate;
        }

        public DateTime Now2()
        {
            return DateTime.Now;
        }

        public string GetTempFileName(string sExt)
        {
            string s1 = "";
            string s2 = "";
            const string TEMP_INDICATOR = "EMTEMP";
            //put 'EMTEMP' in the filename all temp files created. So they can be identified and deleted later.

            if (sExt.Trim().Length == 0)
            {
                sExt = ".txt";
            }
            if (Left2(sExt, 1) != ".")
            {
                sExt = "." + sExt;
            }

            s1 = Path.GetTempFileName();
            s2 = Path.GetDirectoryName(s1) + "\\" + Path.GetFileNameWithoutExtension(s1) + TEMP_INDICATOR + sExt;
            DeleteThisFile(s2);
            return s2;

        }
   

        public string ReplaceDel(string sLine)
        {
            sLine = sLine.Replace(",", "");
            sLine = sLine.Replace(c.MULTI_FIELD_DELIMETER, ",");
            return sLine;
        }

        public string RemoveDoubleSpaces(string sCode)
        {
            sCode = sCode.Trim();
            while (sCode.IndexOf("  ") + 1 > 0)
            {
                sCode = sCode.Replace("  ", " ");
            }
            return sCode;
        }

        public void WriteToTextFile(string sFileName, string sAppendText)
        {
            System.IO.FileStream oFileStream;
            System.IO.StreamWriter oStreamWriter;

            try
            {
                if ((sFileName.Trim().Length == 0))
                {
                    return;
                }

                if ((SizeOfFile(sFileName) > 5000000))   // 5 meg max
                {
                    DeleteThisFile(sFileName);
                }

                oFileStream = new System.IO.FileStream(sFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                oStreamWriter = new System.IO.StreamWriter(oFileStream);
                oStreamWriter.WriteLine(sAppendText);
                oStreamWriter.Flush();
                oStreamWriter.Close();
                oFileStream.Close();

            }
            catch
            {

            }

        }

        public string GetFileExtention(int iOutputType)
        {
            if (iOutputType == c.REPORT_FORMAT_FILE)
            {
                return ".txt";
            }

            if (iOutputType == c.REPORT_FORMAT_EXCEL)
            {
                return ".csv";
            }

            if (iOutputType == c.REPORT_FORMAT_ACROBAT)
            {
                return ".pdf";
            }

            return ".pdf";
        }

        public string FormatName(string sFirstName, string sLastName, string sMidName, int iType)
        {
            switch (iType)
            {
                case 1:
                    return sFirstName + " " + sLastName;

                case 2:
                    return sLastName + ", " + sFirstName;

                case 3:
                    string sMid = Left2(sMidName, 1).Trim();
                    if (IsEmpty(sMid))
                    {
                        return sFirstName + " " + sLastName;
                    }
                    return sFirstName + " " + sMid + " " + sLastName;

                case 4:
                    return sLastName + ", " + Left2(sFirstName, 1).Trim();

                default:
                    return sFirstName + " " + sLastName;

            }

        }

        public string GetVersionNumber()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public string CreateGuid(bool bRemoveDashes = false)
        {
            string s1;
            s1 = Guid.NewGuid().ToString();
            if (bRemoveDashes)
            {
                s1 = s1.Replace("-", "");
            }
            return s1;
        }

        public void WriteDebug(string s1)
        {
            System.Diagnostics.Debug.WriteLine(s1);
        }

        public string YesNo(string s1)
        {
            if (ToBol(s1))
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

        public string YesNo2(bool b)
        {
            if (b)
            {
                return "Yes";
            }
            else
            {
                return "";
            }
        }


        public bool IsNumeric2(string s1)  //  better than Isnumeric()
        {
            s1 = s1.Trim();
            double d1 = 0;
            if (Double.TryParse(s1, out d1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double Val2(string s)
        {
            char systemSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            double result = 0;
            try
            {
                if (s != null)
                    if (!s.Contains(","))
                        result = double.Parse(s, CultureInfo.InvariantCulture);
                    else
                        result = Convert.ToDouble(s.Replace(".", systemSeparator.ToString()).Replace(",", systemSeparator.ToString()));
            }
            catch (Exception)
            {
                try
                {
                    result = Convert.ToDouble(s);
                }
                catch
                {
                    try
                    {
                        result = Convert.ToDouble(s.Replace(",", ";").Replace(".", ",").Replace(";", "."));
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            return result;
        }

        public int ToInt2(string s1)
        {
            int j;
            double d1;

            s1 = NoNull(s1);
            d1 = Val2(s1);
            d1 = Math.Round(d1, 0);

            if (Int32.TryParse(d1.ToString(), out j))
            {
                return j;
            }
            return 0;
        }

        public void HardDeleteFile(string sFilename)
        {
            System.IO.File.Delete(sFilename);
        }

        public bool IsDate2(Object obj)
        {
            string strDate = obj.ToString();
            try
            {
                DateTime dt = DateTime.Parse(strDate);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string ReplacePartOfString(string original, string replaceWith, int iPosition)
        {
            iPosition = iPosition - 1;    //postion is zero-based, user enters it as 1-based, so subtract 1.
            if (iPosition < 0)
            {
                return original;
            }

            if (original.Length >= (iPosition + replaceWith.Length))
            {
                StringBuilder rev = new StringBuilder(original);
                rev.Remove(iPosition, replaceWith.Length);
                rev.Insert(iPosition, replaceWith);
                return rev.ToString();
            }
            else
            {
                return Left2(original, iPosition) + replaceWith; ;
            }
        }

        public void MidStatement(ref string target, int oneBasedStart, string insert, int length)
        {
            //These 'MidStatement' method overloads replicate the behavior of the VB 'Mid' statement (which is unrelated to the VB 'Mid' function)

            if (target == null || insert == null)
                return;

            int minLength = Math.Min(insert.Length, length);
            target = target.PadRight(target.Length + insert.Length).Remove(oneBasedStart - 1, minLength).Insert(oneBasedStart - 1, insert.Substring2(0, minLength)).Substring2(0, target.Length);
        }

        public DateTime NoDate( )
        {
            return StringToDate(c.NODATE_STRING);
        }


        // this used to do a try/catch, but made debugging very slow.  Search for 'catch' and see if the try/catch can be taken out.
        public DateTime StringToDate(string s1)
        {
            if (s1.Trim().Length == 0)
            {
                return Convert.ToDateTime(c.NODATE_STRING);
            }

            DateTime dt1;
            if (!DateTime.TryParse(s1, out dt1))
            {
                return Convert.ToDateTime(c.NODATE_STRING);
            }
            return dt1;
        }


        public string Mid2(string tempString, int startIndex, int intLength)
        {
            if (startIndex < 0)
            {
                return "";
            }

            startIndex = startIndex - 1;    //postion is zero-based, user enters it as 1-based, so subtract 1.

            if ((startIndex + intLength) > tempString.Length)
            {
                return tempString.Substring2(startIndex);
            }

            return tempString.Substring2(startIndex, intLength);
        }

        public int GetWeekOfYear(DateTime time)
        {
            CultureInfo cul = CultureInfo.CurrentCulture; ;
            return cul.Calendar.GetWeekOfYear(time, cul.DateTimeFormat.CalendarWeekRule, cul.DateTimeFormat.FirstDayOfWeek);
        }

        public int DatePart2(DateTime t1, int iDatePartCode)
        {
            int i;
            Calendar c1 = CultureInfo.CurrentCulture.Calendar;

            switch (iDatePartCode)
            {
                case 1:
                    i = c1.GetDayOfMonth(t1);
                    break;

                case 2:
                    i = c1.GetDayOfYear(t1);
                    break;

                case 3:
                    i = c1.GetHour(t1);
                    break;

                case 4:
                    i = c1.GetMinute(t1);
                    break;

                case 5:
                    i = c1.GetMonth(t1);
                    break;

                case 6:
                    i = c1.GetSecond(t1);
                    break;

                case 7:
                    i = c1.GetYear(t1);
                    break;

                default:
                    i = c1.GetYear(t1);
                    break;
            }

            return i;
        }

        public int DateDiff2(DateTime d1, DateTime d2)
        {
            double dx;
            int i;
            dx = (d2.Date - d1.Date).TotalDays;
            i = Convert.ToInt32(dx);
            return i;
        }

        public string Format2(string s1, string sFormat)
        {
            return String.Format("{0:" + sFormat + "}", Val2(s1));
        }

        public string Format2(double d1, string sFormat)
        {
            return String.Format("{0:" + sFormat + "}", d1);
        }

        public string Format2(string s1, int DecimalPlaces)
        {
            return Format2(Val(s1), DecimalPlaces);
        }

        public string Format2(double d1, int DecimalPlaces)
        {
            string sFormat;
            switch (DecimalPlaces)
            {
                case 0:
                    sFormat = "{0:0}";
                    break;
                case 1:
                    sFormat = "{0:0.0}";
                    break;
                case 2:
                    sFormat = "{0:0.00}";
                    break;
                case 4:
                    sFormat = "{0:0.0000}";
                    break;
                case 5:
                    sFormat = "{0:00}";
                    break;
                case 6:
                    sFormat = "{0:0.00}";
                    break;
                case 7:
                    sFormat = "{0:0000}";
                    break;
                default:
                    sFormat = "{0:0.00}";
                    break;
            }
           return String.Format(sFormat, d1);
        }

        public string Format2(int i, string sFormat)
        {
            return String.Format("{0:" + sFormat + "}", i);
        }

        public string NL()
        {
            return Environment.NewLine.ToString();
        }

        public bool ValidateRoutingNumberDigits(string RoutingNumber)
        {
            if (!IsNineDgits(RoutingNumber))
            {
                return false;
            }

            //     Electronic Funds Transfer Routing Number Check    
            double Sum;
            Sum = ((3 * double.Parse(RoutingNumber.Substring2(0, 1)))
                        + ((7 * double.Parse(RoutingNumber.Substring2(1, 1)))
                        + (double.Parse(RoutingNumber.Substring2(2, 1))
                        + ((3 * double.Parse(RoutingNumber.Substring2(3, 1)))
                        + ((7 * double.Parse(RoutingNumber.Substring2(4, 1)))
                        + (double.Parse(RoutingNumber.Substring2(5, 1))
                        + ((3 * double.Parse(RoutingNumber.Substring2(6, 1)))
                        + ((7 * double.Parse(RoutingNumber.Substring2(7, 1)))
                        + double.Parse(RoutingNumber.Substring2(8, 1))))))))));
            return ((Sum % 10) == 0);
        }

        public bool IsNineDgits(string RoutingNumber)
        {
            string pattern = "[0-9]{9}";
            Match NewMatch = Regex.Match(RoutingNumber, pattern);
            if (NewMatch.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool IsTodayInDateRange(string sStartDate, string sEndDate)
        {
            DateTime tStartOfToday = default(DateTime);
            DateTime tEndOfToday = default(DateTime);
            DateTime tStartDate = default(DateTime);
            DateTime tEndDate = default(DateTime);
            string sFormat = null;

            try
            {
                if (sStartDate == null)
                {
                    sStartDate = "";
                }
                if (sEndDate == null)
                {
                    sEndDate = "";
                }

                if (sStartDate.Trim().Length == 0 && sEndDate.Trim().Length == 0)
                {
                    return true;
                }

                sFormat = "M/dd/yyyy  01:01";
                tStartOfToday = DateTime.Parse(string.Format("{0:" + sFormat + "}", CurrentTimeAdjustedToUser()));

                sFormat = "M/dd/yyyy  23:59";
                tEndOfToday = DateTime.Parse(string.Format("{0:" + sFormat + "}", CurrentTimeAdjustedToUser()));

                if (sStartDate.Trim().Length == 0)
                {
                    tStartDate = DateTime.Parse("1/1/1000");
                }
                else
                {
                    sFormat = "M/dd/yyyy  5:01";
                    tStartDate = DateTime.Parse(string.Format("{0:" + sFormat + "}", DateTime.Parse(sStartDate)));
                }

                if (sEndDate.Trim().Length == 0)
                {
                    tEndDate = DateTime.Parse("12/1/2500");
                }
                else
                {
                    sFormat = "M/dd/yyyy  5:01";
                    tEndDate = DateTime.Parse(string.Format("{0:" + sFormat + "}", DateTime.Parse(sEndDate)));
                }

                if ((tEndOfToday > tStartDate) && (tStartOfToday < tEndDate))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }


        }    

        public string ArrayToString(string[] aFields)
        {
            string s1 = "";

            foreach (string s in aFields)
            {
                s1 = s1 + NoNull(s) + c.MULTI_FIELD_DELIMETER;
            }

            return s1;
        }

        public double SizeOfFile(string sFilename)
        {
            FileInfo fi;
            try
            {
                fi = new FileInfo(sFilename);
                return fi.Length;
            }
            catch
            {
                return 0;
            }

        }

        public string InQuote(string s1, int iTrimLength = 0)
        {
            if (s1 is null)
            {
                return "Null";
            }

            if (iTrimLength > 0)
            {
                s1 = Left2(s1, iTrimLength);
            }

            s1 = MakeSingleQuotesDouble(s1);

            return "'" + s1 + "'"; ;
        }

        public string InQuoteN(string s1)
        {
            return Val2(s1).ToString();
        }

        public string InQuoteN(int i)
        {
            return i.ToString();
        }

        public string InQuoteN(bool b)
        {
            if (b)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }


        public string InQuoteD(string s1) //date only, no time.
        {
            string tempInQuoteD = null;
            s1 = ValidDate(s1, 1);

            if (s1.Trim().Length == 0)
            {
                tempInQuoteD = "Null";
            }
            else
            {
                tempInQuoteD = (char)39 + s1 + (char)39; //single quote
            }
            return tempInQuoteD;
        }



        public string InQuoteT(DateTime tDate1, string sFormat = "")
        {
            string s1;
            if ((sFormat.Trim().Length == 0))
            {
                sFormat = c.DATE_FORMAT4;
            }

            try
            {
                if (IsDate2(tDate1))
                {
                    s1 = string.Format(("{0:" + (sFormat + "}")), tDate1);
                }
                else
                {
                    s1 = "";
                }

            }
            catch (System.Exception)
            {
                return null;
            }

            return InQuoteT(s1, sFormat);
        }

        public string InQuoteT(string s1, string sFormat = "")
        {
            if ((sFormat.Trim().Length == 0))
            {
                sFormat = c.DATE_FORMAT4;
            }

            s1 = ValidDate(s1, 4);

            if ((s1.Trim().Length == 0))
            {
                return "Null";
            }
            else
            {
                return ('\'' + s1 + '\'');
            }

        }

        public string InQuoteB(bool b)
        {
            string tempInQuoteB = null;

            if (b)
            {
                tempInQuoteB = InQuoteN("1");
            }
            else
            {
                tempInQuoteB = InQuoteN("0");
            }

            return tempInQuoteB;
        }

        public string MakeSingleQuotesDouble(string sIncomingString)
        {
            return Replace2(sIncomingString, "'", "''");
        }

        public DataTable CreateReturnDT(string sResultCode, string sResultMsg)
        {
            DataTable dt1 = new DataTable();
            DataRow newrow;
            dt1.Columns.Add("ResultCode");
            dt1.Columns.Add("ResultMsg");

            newrow = dt1.NewRow();
            newrow["ResultCode"] = sResultCode;
            newrow["ResultMsg"] = sResultMsg;
            dt1.Rows.Add(newrow);

            return dt1;

        }


    }
}
