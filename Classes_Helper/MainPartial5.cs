using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WhosIn
{
    public partial class cMain
    {
        public bool BeforeToday(DateTime dt1)
        {
            int daysOld;

            daysOld = DateDiff2(dt1, Now2());

            if (daysOld > 0)
            {
                return true;
            }
            {
                return false;
            }
        }

        public bool IsDateBeforeOrToday(string input)
        {
            DateTime pDate;
            if (!DateTime.TryParseExact(input, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out pDate))
            {
                return false;
            }
            return DateTime.Today <= pDate;
        }

        public string DaysFromNow(DateTime dt1)
        {
            int daysOld;
            daysOld = DateDiff2(dt1,Now2());


            if (daysOld == 0)
            {
                return "Today";
            }


            if (daysOld == -1)
            {
                return "Tomorrow";
            }

            if (daysOld == 1)
            {
                return "Yesterday";
            }

            if (daysOld > 0)
            {
                return Math.Abs(daysOld).ToString() + " days ago.";
            }


            if (daysOld < 0)
            {
                return Math.Abs(daysOld).ToString() + " days from now.";
            }

            return "";
        }

        public string TimeFromDate(DateTime dt1)
        {
            int daysOld;
            daysOld = DateDiff3(dt1, Now2());


            if (daysOld == 0)
            {
                return "Today";
            }


            if (daysOld == -1)
            {
                return "Tomorrow";
            }

            if (daysOld == 1)
            {
                return "Yesterday";
            }

            if (daysOld > 0)
            {
                return Math.Abs(daysOld).ToString() + " days ago.";
            }


            if (daysOld < 0)
            {
                return Math.Abs(daysOld).ToString() + " days from now.";
            }

            return "";
        }

        private int DateDiff3(DateTime d1, DateTime d2)
        {
            double dx;
            double dx2;
            double dx3;
            int i;
            dx = (d2.Date - d1.Date).TotalDays;
            dx2 = (d2.Date - d1.Date).TotalHours;
            dx3 = (d2.Date - d1.Date).Minutes;
            i = Convert.ToInt32(dx);
            return i;
        }

        public string GetPrettyDate(DateTime d)
        {
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            if (dayDiff < 0)
            {
                return "In future";
            }


            if (dayDiff >= 31)
            {
                return "30+ days ago";
            }


            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return null;
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "just now";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "1 minute ago";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} minutes ago",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 h ago";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} hours ago",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "yesterday";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} days ago",
                    dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("{0} weeks ago",
                    Math.Ceiling((double)dayDiff / 7));
            }
            return null;
        }

        public bool IsPinCorrect(string sPinToCheck, string sGroupGUID)    
        {
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT GroupPin FROM [W_Groups] ";
            sSQL = sSQL + " WHERE [GroupGUID] = " + InQuote(sGroupGUID);
            ds1 = SQLRunQuery(sSQL, c.DB.WhosIn);
            if (!EmptyTable(ds1))
            {
                if ((!IsEmpty(GetFieldStr("GroupPin", ds1))) && (GetFieldStr("GroupPin", ds1).Trim().ToLower() == sPinToCheck.Trim().ToLower()))
                {
                    return true;
                }
            }



            sSQL = "SELECT PinNumber1, PinNumber2, PinNumber3 FROM [W_SystemSettings] ";
            ds1 = SQLRunQuery(sSQL, c.DB.WhosIn);
            if (EmptyTable(ds1))
            {
                return true;
            }

            if ( IsEmpty(GetFieldStr("PinNumber1", ds1)) && IsEmpty(GetFieldStr("PinNumber2", ds1)) && IsEmpty(GetFieldStr("PinNumber3", ds1))) 
            {
                return true;
            }


            if ((!IsEmpty(GetFieldStr("PinNumber1", ds1))) && (GetFieldStr("PinNumber1", ds1).Trim().ToLower() == sPinToCheck.Trim().ToLower()))
            {
                return true;
            }

            if ((!IsEmpty(GetFieldStr("PinNumber2", ds1))) && (GetFieldStr("PinNumber2", ds1).Trim().ToLower() == sPinToCheck.Trim().ToLower()))
            {
                return true;
            }

            if ((!IsEmpty(GetFieldStr("PinNumber3", ds1))) && (GetFieldStr("PinNumber3", ds1).Trim().ToLower() == sPinToCheck.Trim().ToLower()))
            {
                return true;
            }

            return false;
        }
    }

}
