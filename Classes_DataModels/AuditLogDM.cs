using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhosIn.Classes_DataModels
{
    public class AuditLogDM
    {
        public string LogGUID { get; set; }
        public string TTGUID { get; set; }
        public DateTime LogDateTime { get; set; }
        public string LogEvent { get; set; }
        public string LogIPAddress { get; set; }
        public string DisplayLine1 { get; set; }
        public string DisplayLine2 { get; set; }
    }
}

 