using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WhosIn.Classes_DataModels;
using System.Text.Json;
using Newtonsoft.Json;
using WhosIn.Pages;

//====================================
//
// API07  - Audit Log
//
//====================================

namespace WhosIn.API
{ 
    internal class API07
    {
        readonly cMain m;
        readonly APIRunner a;

        public API07(cMain mIn, APIRunner aIn)
        {
            m = mIn;
            a = aIn;
        }

        public void API07_GET_AUDITLOG()
        {
            string sSQL;
            DataSet ds1;
            string ttGUID;

            string[] aFields = m.parseToArrayByDelimter(a.InputParameters, c.MULTI_FIELD_DELIMETER);
            aFields = m.NoNullArray(aFields, 1);
            ttGUID = aFields[0];

            sSQL = "SELECT * FROM [W_TeeTimeLog] ";
            sSQL = sSQL + " WHERE [TTGUID] = " + m.InQuote(ttGUID);
            sSQL = sSQL + "  ORDER BY LogDateTime";
            ds1 = m.SQLRunQuery(sSQL, c.DB.WhosIn);
            a.AppendTableToDataset(ds1);
        }
        
    }

}
