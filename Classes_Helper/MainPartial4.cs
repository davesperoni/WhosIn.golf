using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace WhosIn
{
    public partial class cMain
    {
        //=========  SQL Read Datasets   ===================


        public DataSet SQLRunQuery(string sSql, c.DB e)
        {
            SqlConnection cnn;

            DataSet ds = new DataSet();
            SqlDataAdapter sqlAdp;

            LastUsedSQLStatement = sSql;

            switch (e)
            {
                case c.DB.WhosIn:
                    cnn = new SqlConnection(x_sConnectionStringMain);
                    break;

                case c.DB.NotUsed:
                    cnn = new SqlConnection(x_sConnectionStringNOTUSED);
                    break;

                default:
                    cnn = new SqlConnection(x_sConnectionStringMain);
                    break;
            }
            cnn.Open();
            sqlAdp = new SqlDataAdapter(sSql, cnn);
            cnn.Close();
            sqlAdp.Fill(ds);
            return ds;
        }

        public int SQLExecuteCommand(string sSql, c.DB e)
        {
            SqlConnection cnn;
            SqlCommand cmd = new SqlCommand();
            int i;

            LastUsedSQLStatement = sSql;

            switch (e)
            {
                case c.DB.WhosIn:
                    cnn = new SqlConnection(x_sConnectionStringMain);
                    break;

                case c.DB.NotUsed:
                    cnn = new SqlConnection(x_sConnectionStringNOTUSED);
                    break;

                default:
                    cnn = new SqlConnection(x_sConnectionStringMain);
                    break;
            }

            cmd.CommandText = sSql;
            cnn.Open();
            cmd.Connection = cnn;  
            i = cmd.ExecuteNonQuery();
            cnn.Close();
            return i;
        }

        public bool TimeToStop()
        {
            return false;
        }

        public void WriteToEventLog(string sEventItemGUID, string sParticipantGUID, string sChange, string sIP)
        {
            string sSQL = "";
            DataSet ds1;
            string sNewGUID;

            sNewGUID = CreateGuid();

            if (IsEmpty(sEventItemGUID) && !IsEmpty(sParticipantGUID))
            {
                sSQL = "SELECT PaEventGUID FROM Participants WHERE ParticipantGUID = " + InQuote(sParticipantGUID);
                ds1 = SQLRunQuery(sSQL, c.DB.WhosIn);
                sEventItemGUID = GetFieldStr("PaEventGUID", ds1);
            }

            sSQL = "INSERT INTO [EventLog] ";
            sSQL = sSQL + " ([EventLogGUID] ";
            sSQL = sSQL + "  ,[EventItemGUID] ";
            sSQL = sSQL + "  ,[ParticipantGUID] ";
            sSQL = sSQL + "  ,[LogAction] ";
            sSQL = sSQL + "  ,[LogDate] ";
            sSQL = sSQL + " ,[LogIP]) ";
            sSQL = sSQL + "  VALUES ";
            sSQL = sSQL + "  (" + InQuote(sNewGUID) + ", ";
            sSQL = sSQL + "  " + InQuote(sEventItemGUID) + ", ";
            sSQL = sSQL + "  " + InQuote(sParticipantGUID) + ", ";
            sSQL = sSQL + "  " + InQuote(sChange, 250) + ", ";
            sSQL = sSQL + "  " + InQuoteT(Now2()) + ", ";
            sSQL = sSQL + "  " + InQuote(sIP, 100) + " ) ";
            SQLRunQuery(sSQL, c.DB.WhosIn);
        }

        //------------------ Return string  ------------------------------------
        public string GetFieldStr(string sField, DataSet ds1)
        {
            return GetFieldStr(sField, 0, ds1.Tables[0]);
        }

        public string GetFieldStr(string sField, int iRow, DataSet ds1, int iTableNo)
        {
            return GetFieldStr(sField, iRow, ds1.Tables[iTableNo]);
        }

        public string GetFieldStr(string sField, DataSet ds1, int iTableNo)
        {
            return GetFieldStr(sField, 0, ds1.Tables[iTableNo]);
        }

        public string GetFieldStr(string sField, DataSet ds1, string sTableName)
        {
            return GetFieldStr(sField, 0, ds1.Tables[sTableName]);
        }

        public string GetFieldStr(string sField, int iRow, DataSet ds1, string sTableName)
        {
            return GetFieldStr(sField, iRow, ds1.Tables[sTableName]);
        }

        public string GetFieldStr(string sField, DataRow r)
        {
            // add this to all other stuff
            string s1 = null;
            s1 = r[sField].ToString();

            if (s1 == null)
            {
                s1 = "";
            }
            return s1.Trim();

        }

        public string GetFieldStr(string sField, DataTable dt1)
        {
            // add this to all other stuff
            string s1 = null;

            s1 = dt1.Rows[0][sField].ToString();

            if (s1 == null)
            {
                s1 = "";
            }
            return s1.Trim();

        }

        public string GetFieldStr(string sField, int iRow, DataTable dt1)
        {
            string s1 = null;

            if (dt1.Rows.Count < 1)
            {
                return "";
            }

            s1 = dt1.Rows[iRow][sField].ToString();
            if (s1 == null)
            {
                s1 = "";
            }
            return s1.Trim();
        }


        //------------------ Return Integer ------------------------------------
        public int GetFieldInt(string sField, DataSet ds1)
        {
            return GetFieldInt(sField, 0, ds1.Tables[0]);
        }

        public int GetFieldInt(string sField, int iRow, DataSet ds1, int iTableNo)
        {
            return GetFieldInt(sField, iRow, ds1.Tables[iTableNo]);
        }

        public int GetFieldInt(string sField, DataSet ds1, int iTableNo)
        {
            return GetFieldInt(sField, 0, ds1.Tables[iTableNo]);
        }

        public int GetFieldInt(string sField, DataSet ds1, string sTableName)
        {
            return GetFieldInt(sField, 0, ds1.Tables[sTableName]);
        }

        public int GetFieldInt(string sField, int iRow, DataSet ds1, string sTableName)
        {
            return GetFieldInt(sField, iRow, ds1.Tables[sTableName]);
        }

        public int GetFieldInt(string sField, DataRow r)
        {
            string s1 = null;

            s1 = r[sField].ToString();
            return ToInt(s1);
        }

        public int GetFieldInt(string sField, DataTable dt1)
        {
            return GetFieldInt(sField, 0, dt1);
        }

        public int GetFieldInt(string sField, int iRow, DataTable dt1)
        {
            string s1 = null;

            if (dt1.Rows.Count < 1)
            {
                return 0;
            }

            s1 = dt1.Rows[iRow][sField].ToString();
            if (s1 == null)
            {
                s1 = "";
            }
            return ToInt(s1);
        }


        //------------------ Return Double ------------------------------------

        public double GetFieldDbl(string sField, DataSet ds1)
        {
            return GetFieldDbl(sField, 0, ds1.Tables[0]);
        }

        public double GetFieldDbl(string sField, int iRow, DataSet ds1, int iTableNo)
        {
            return GetFieldDbl(sField, iRow, ds1.Tables[iTableNo]);
        }

        public double GetFieldDbl(string sField, DataSet ds1, int iTableNo)
        {
            return GetFieldDbl(sField, 0, ds1.Tables[iTableNo]);
        }

        public double GetFieldDbl(string sField, DataSet ds1, string sTableName)
        {
            return GetFieldDbl(sField, 0, ds1.Tables[sTableName]);
        }

        public double GetFieldDbl(string sField, int iRow, DataSet ds1, string sTableName)
        {
            return GetFieldDbl(sField, iRow, ds1.Tables[sTableName]);
        }

        public double GetFieldDbl(string sField, DataTable dt1)
        {
            return GetFieldDbl(sField, 0, dt1);
        }

        public double GetFieldDbl(string sField, int iRow, DataTable dt1)
        {
            string s1 = null;

            if (dt1.Rows.Count < 1)
            {
                return 0;
            }


            s1 = dt1.Rows[iRow][sField].ToString();
            if (s1 == null)
            {
                return 0;
            }

            return Val2(s1);

        }

        public double GetFieldDbl(string sField, DataRow r)
        {
            string s1 = null;

            s1 = r[sField].ToString();
            if (s1 == null)
            {
                return 0;
            }

            return Val2(s1);
        }


        //------------------ Return Double - Formatted as string  ------------------------------------

        public string GetFieldDblF(string sField, DataSet ds1, int iDec)
        {
            return GetFieldDblF(sField, 0, ds1.Tables[0], iDec);
        }

        public string GetFieldDblF(string sField, int iRow, DataSet ds1, int iTableNo, int iDec)
        {
            return GetFieldDblF(sField, iRow, ds1.Tables[iTableNo], iDec);
        }

        public string GetFieldDblF(string sField, DataSet ds1, int iTableNo, int iDec)
        {
            return GetFieldDblF(sField, 0, ds1.Tables[iTableNo], iDec);
        }

        public string GetFieldDblF(string sField, DataSet ds1, string sTableName, int iDec)
        {
            return GetFieldDblF(sField, 0, ds1.Tables[sTableName], iDec);
        }

        public string GetFieldDblF(string sField, int iRow, DataSet ds1, string sTableName, int iDec)
        {
            return GetFieldDblF(sField, iRow, ds1.Tables[sTableName], iDec);
        }

        public string GetFieldDblF(string sField, DataTable dt1, int iDec)
        {
            return GetFieldDblF(sField, 0, dt1, iDec);
        }

        public string GetFieldDblF(string sField, int iRow, DataTable dt1, int iDec)
        {
            string s1 = null;
            string sFormat = null;

            switch (iDec)
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

            if (dt1.Rows.Count < 1)
            {
                return string.Format(sFormat, 0);
            }


            s1 = dt1.Rows[iRow][sField].ToString();
            if (s1 == null)
            {
                return string.Format(sFormat, 0);
            }

            return string.Format(sFormat, Val2(s1));
        }

        public string GetFieldDblF(string sField, DataRow r, int iDec)
        {
            string s1 = null;
            string sFormat = null;
            bool bZeroIsBlank = false;

            if (iDec == 6)
            {
                bZeroIsBlank = true;
            }

            switch (iDec)
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
                default:
                    sFormat = "{0:0.00}";
                    break;
            }

            s1 = NoNull(r[sField].ToString());
            if (Val2(s1) == 0 & bZeroIsBlank)
            {
                return "";
            }
            else
            {
                return string.Format(sFormat, Val2(s1));
            }

        }


        //------------------ Return Boolean ------------------------------------

        public bool GetFieldBol(string sField, DataSet ds1)
        {
            return GetFieldBol(sField, 0, ds1.Tables[0]);
        }

        public bool GetFieldBol(string sField, int iRow, DataSet ds1, int iTableNo)
        {
            return GetFieldBol(sField, iRow, ds1.Tables[iTableNo]);
        }

        public bool GetFieldBol(string sField, DataSet ds1, int iTableNo)
        {
            return GetFieldBol(sField, 0, ds1.Tables[iTableNo]);
        }

        public bool GetFieldBol(string sField, DataSet ds1, string sTableName)
        {
            return GetFieldBol(sField, 0, ds1.Tables[sTableName]);
        }

        public bool GetFieldBol(string sField, int iRow, DataSet ds1, string sTableName)
        {
            return GetFieldBol(sField, iRow, ds1.Tables[sTableName]);
        }

        public bool GetFieldBol(string sField, DataTable dt1)
        {
            return GetFieldBol(sField, 0, dt1);
        }

        public bool GetFieldBol(string sField, DataRow r)
        {
            string s1 = null;

            s1 = r[sField].ToString();
            if (s1 == null)
            {
                s1 = "";
            }
            return ToBol(s1);
        }

        public bool GetFieldBol(string sField, int iRow, DataTable dt1)
        {
            string s1 = null;

            if (dt1.Rows.Count < 1)
            {
                return false;
            }

            s1 = dt1.Rows[iRow][sField].ToString();
            if (s1 == null)
            {
                s1 = "";
            }
            return ToBol(s1);

        }


        //------------------ Return Date ------------------------------------

        public System.DateTime GetFieldDate(string sField, DataSet ds1)
        {
            return GetFieldDate(sField, 0, ds1.Tables[0]);
        }

        public System.DateTime GetFieldDate(string sField, DataTable dt1)
        {
            return GetFieldDate(sField, 0, dt1);
        }

        public System.DateTime GetFieldDate(string sField, int iRow, DataSet ds1, int iTableNo)
        {
            return GetFieldDate(sField, iRow, ds1.Tables[iTableNo]);
        }

        public System.DateTime GetFieldDate(string sField, DataSet ds1, int iTableNo)
        {
            return GetFieldDate(sField, 0, ds1.Tables[iTableNo]);
        }

        public System.DateTime GetFieldDate(string sField, DataSet ds1, string sTableName)
        {
            return GetFieldDate(sField, 0, ds1.Tables[sTableName]);
        }

        public System.DateTime GetFieldDate(string sField, int iRow, DataSet ds1, string sTableName)
        {
            return GetFieldDate(sField, iRow, ds1.Tables[sTableName]);
        }

        public System.DateTime GetFieldDate(string sField, int iRow, DataTable dt1)
        {
            return GetFieldDate(sField, dt1.Rows[iRow]);
        }

        public System.DateTime GetFieldDate(string sField, DataRow oRow)
        {
            string s1 = null;
            DateTime tNoDate = default(DateTime);
            DateTime t1 = default(DateTime);

            tNoDate = Convert.ToDateTime(c.NODATE_STRING);

            if (oRow == null)
            {
                return tNoDate;
            }

            s1 = oRow[sField].ToString();

            if (s1.Length == 0)
            {
                return tNoDate;
            }
            t1 = Convert.ToDateTime(s1);
            return t1;
        }


        //------------------ Return Date - Formatted String ------------------------------------

        public string GetFieldDateF(string sField, int iRow, DataSet ds1, int iTableNo, string sFormat)
        {
            return GetFieldDateF(sField, iRow, ds1.Tables[iTableNo], sFormat);
        }

        public string GetFieldDateF(string sField, DataSet ds1, int iTableNo, string sFormat)
        {
            return GetFieldDateF(sField, 0, ds1.Tables[iTableNo], sFormat);
        }

        public string GetFieldDateF(string sField, DataSet ds1, string sTableName, string sFormat)
        {
            return GetFieldDateF(sField, 0, ds1.Tables[sTableName], sFormat);
        }

        public string GetFieldDateF(string sField, int iRow, DataSet ds1, string sTableName, string sFormat)
        {
            return GetFieldDateF(sField, iRow, ds1.Tables[sTableName], sFormat);
        }

        public string GetFieldDateF(string sField, int iRow, DataTable dt1, string sFormat)
        {
            return GetFieldDateF(sField, dt1.Rows[iRow], sFormat);
        }

        public string GetFieldDateF(string sField, DataTable dt1, string sFormat)
        {
            return GetFieldDateF(sField, dt1.Rows[0], sFormat);
        }

        public string GetFieldDateF(string sField, DataSet ds1, string sFormat)
        {
            return GetFieldDateF(sField, ds1.Tables[0].Rows[0], sFormat);
        }

        public string GetFieldDateF(string sField, DataRow oRow, string sFormat, bool bShowNoDateAsBlank = false)
        {
            string s1 = null;
            string s2 = null;
            System.DateTime t1 = default(System.DateTime);
            TimeSpan tsTimeSpan = new TimeSpan(0);

            if (sFormat.Trim().Length == 0)
            {
                sFormat = "M/dd/yyyy";
            }

            s1 = oRow[sField].ToString();

            if (s1.Length == 0)
            {
                return "";
            }

            if (!IsDate2(s1))
            {
                return "";
            }

            t1 = Convert.ToDateTime(s1);

            if (bShowNoDateAsBlank && t1 == Convert.ToDateTime(c.NODATE_STRING))
            {
                return "";
            }

            t1 = t1.Add(tsTimeSpan);

            s2 = string.Format("{0:" + sFormat + "}", t1);
            return s2;

        }

        public int RowsCount(DataSet ds1)
        {
            if (ds1 == null)
            {
                return 0;
            }
            return ds1.Tables[0].Rows.Count;
        }

        public bool EmptyTable(DataSet ds1, string sTableName)
        {

            if (ds1 == null)
            {
                return true;
            }

            if (ds1.Tables[sTableName].Rows.Count < 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool EmptyTable(DataSet ds1)
        {
            if (ds1 == null)
            {
                return true;
            }

            if (ds1.Tables[0].Rows.Count < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EmptyTable(DataTable dt1)
        {
            if (dt1 == null)
            {
                return true;
            }

            if (dt1.Rows.Count < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string AddColin(string s1)
        {
            if (IsEmpty(s1))
            {
                return "";
            }
            return s1 + ":";
        }

        public string GetTableName(int iActionCode, int iSeqNo)
        {
            return iActionCode.ToString().Trim() + "_" + iSeqNo.ToString().Trim();
        }

    }

}
