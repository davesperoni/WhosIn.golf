using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhosIn.Classes_DataModels
{
	public class ChatDM
	{
		public string ChatGUID { get; set; }
		public string TeeTimeGUID{ get; set; }
		public DateTime DateTimeEntered { get; set; }
		public string Message { get; set; }
		public string Initials { get; set; }
		public string IPAddress { get; set; }
		public string DisplayLine { get; set; }
		public string ValidationFailMessage { get; set; } = "";
	}
}
