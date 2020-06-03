using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WhosIn
{
    public partial class cMain
    {

        public string GetQueryStringValue(string sFullURI, string sKey)   
        {
            try
            {
                var uri = new Uri(sFullURI);
                var baseUri = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
                var query = QueryHelpers.ParseQuery(uri.Query);
                var items = query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();

                for (int i1 = 0; i1 < items.Count; i1++)
                {
                    if (items[i1].Key.Trim().ToLower() == sKey.Trim().ToLower())
                    {
                        return items[i1].Value;
                    }
                }
                return "";
            }
            catch (Exception)
            {

                return "";
            }
        }

        public string ShortenStack(string StackTrace, bool bFormatForHTML)
        {
            string NEWLINE = "";
            string sNewStack = "";
            string[] lines;

            StackTrace = NoNull(StackTrace);

            lines = parseToArrayByDelimter(StackTrace, "at ");
            lines = NoNullArray(lines, 1);

            if (bFormatForHTML)
            {
                NEWLINE = "<br />";
            }
            else
            {
                NEWLINE = NL();
            }

            sNewStack = NEWLINE;

            foreach (string s in lines)
            {

                if (!StringContains(s, "System.Data."))
                {
                    if (!IsEmpty(s))
                    {
                        sNewStack = sNewStack + NEWLINE + "at: " + NoNull(s) + NEWLINE;
                    }
                }

            }




            return sNewStack;
        }

    }
}


 