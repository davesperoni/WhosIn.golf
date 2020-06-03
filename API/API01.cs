using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

namespace WhosIn.API
{
    //====================================
    //
    // API01  - Get TeeTimes
    //
    //====================================

    internal class API01
    {
        readonly cMain m;
        readonly APIRunner a;

        public API01(cMain mIn, APIRunner aIn)
        {
            m = mIn;
            a = aIn;
        }

        public void API01_GET_TEETIMES()
        {
            string sGroupGUID;
            bool bShowAllTeeTimes;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 10);
            sGroupGUID = aFields[0];
            bShowAllTeeTimes = m.ToBol(aFields[1]);
            GetTeeTimesByGroup(sGroupGUID, bShowAllTeeTimes);
        }

        private void GetTeeTimesByGroup(string sGroupGUID, bool bShowAllTeeTimes)
        {
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT * ";
            sSQL = sSQL + " FROM  [W_Groups] ";
            sSQL = sSQL + " WHERE [GroupGUID] = " + m.InQuote(sGroupGUID);
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            if (!m.EmptyTable(ds1))
            {
                if (m.GetFieldBol("GroupShowAllGroups", ds1))
                {
                    sGroupGUID = "";
                }
            }

            if (bShowAllTeeTimes)
            {
                sSQL = "SELECT TOP 100 t.*, c.* ";
                sSQL = sSQL + " FROM  [W_TeeTimes] t ";
                sSQL = sSQL + " LEFT JOIN [W_Groups] c ";
                sSQL = sSQL + " ON c.[GroupGUID] = t.[GroupGUID] ";
                sSQL = sSQL + " Order by t.TeeTimeDate, t.TeeTimeTime ";
            }
            else
            {
                if (m.IsEmpty(sGroupGUID))  // show all groups if sGroupGUID is empty.
                {
                    sSQL = "SELECT t.*, c.* ";
                    sSQL = sSQL + " FROM  [W_TeeTimes] t ";
                    sSQL = sSQL + " LEFT JOIN [W_Groups] c ";
                    sSQL = sSQL + " ON c.[GroupGUID] = t.[GroupGUID] ";
                    sSQL = sSQL + " WHERE ( TeeTimeDate >= Convert(date, getdate()) )";
                    sSQL = sSQL + " AND (c.[GroupActive] = 1   )";
                    sSQL = sSQL + " AND (t.[IsDeleted] = 0 )";
                    sSQL = sSQL + " Order by t.TeeTimeDate, t.TeeTimeTime ";
                }
                else
                {
                    sSQL = "SELECT t.*, c.* ";
                    sSQL = sSQL + " FROM  [W_TeeTimes] t ";
                    sSQL = sSQL + " LEFT JOIN [W_Groups] c ";
                    sSQL = sSQL + " ON c.[GroupGUID] = t.[GroupGUID] ";
                    sSQL = sSQL + " WHERE ( TeeTimeDate >= Convert(date, getdate()) )";
                    sSQL = sSQL + " AND c.[GroupGUID] = " + m.InQuote(sGroupGUID);
                    sSQL = sSQL + " AND (t.[IsDeleted] = 0 )";
                    sSQL = sSQL + " Order by t.TeeTimeDate, t.TeeTimeTime ";
                }
            }

            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            a.AppendTableToDataset(ds1);
        }

        public void API01_GET_SINGLE_TEETIME()
        {
            string TTGUID;
            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 10);
            TTGUID = aFields[0];
            GetSingleTeeTime(TTGUID);
        }

        private void GetSingleTeeTime(string TTGUID)
        {
            string sSQL;
            DataSet ds1;

            sSQL = "SELECT t.*, c.* ";
            sSQL = sSQL + " FROM  [W_TeeTimes] t ";
            sSQL = sSQL + " LEFT JOIN [W_Groups] c ";
            sSQL = sSQL + " ON c.[GroupGUID] = t.[GroupGUID] ";
            sSQL = sSQL + " WHERE [TeeTimeGUID] = " + m.InQuote(TTGUID);

            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            a.AppendTableToDataset(ds1);

        }




    }
}

