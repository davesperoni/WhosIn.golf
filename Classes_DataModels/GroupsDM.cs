using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhosIn.Classes_DataModels
{
	public class GroupItemDM
	{
		public string GroupGUID { get; set; }
		public string GroupName { get; set; }
		public string GroupDescription { get; set; }
		public string GroupPin { get; set; }
		public string GroupPhone { get; set; }
		public string GroupEmail { get; set; }
		public bool GroupActive { get; set; }
		public bool GroupIsDefault { get; set; }
		public string ValidationFailMessage { get; set; }
		public bool GroupShowAllGroups { get; set; }
		public string GroupColor { get; set; }
		public bool GroupDefaultForNewTeeTime { get; set; }
	}
}