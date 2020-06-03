using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhosIn.Classes_DataModels
{

	public class TeeTimeDM
	{
		public string GroupGUID { get; set; }
		public List<TeeTimeItem> TeeTimesList { get; set; } = new List<TeeTimeItem>();
	}
	

	public class TeeTimeItem
	{
		public string TTGUID { get; set; } = "";
		public string GroupGUID { get; set; } = "";
		public string GroupName { get; set; } = "";

		public DateTime? TTDate { get; set; }
		public string TTDateFormated { get; set; } = "";

		public DateTime? TTTime { get; set; }
		public string TTTimeFormated { get; set; } = "";

		public string DaysFromNow { get; set; } = "";
		public string Owner { get; set; } = "";
		public string OwnerComment { get; set; } = "";
		public string Location { get; set; } = "";
		public string Player1 { get; set; } = "";
		public string Player1Comment { get; set; } = "";
		public string Player2 { get; set; } = "";
		public string Player2Comment { get; set; } = "";
		public string Player3 { get; set; } = "";
		public string Player3Comment { get; set; } = "";
		public string Player4 { get; set; } = "";
		public string Player4Comment { get; set; } = "";
		public string WaitList1 { get; set; } = "";
		public string WaitList2 { get; set; } = "";
		public string WaitList3 { get; set; } = "";
		public bool IsDeleted { get; set; }
		public bool LockPlayer1 { get; set; }
		public bool LockPlayer2 { get; set; }
		public bool LockPlayer3 { get; set; }
		public bool LockPlayer4 { get; set; }
		public bool LockMessages { get; set; }
		public bool HideMessages { get; set; }
		public string PinNumberEntered { get; set; } = "";
		public string ValidationFailMessage { get; set; } = "";
		public bool ShowAllTeeTimes { get; set; }
		public int PlayerNumberBeingEdited { get; set; }
		public string PlayerNameBeingEdited { get; set; } = "";
		public string PlayerCommentBeingEdited { get; set; } = "";
		public string SpotsAvailable { get; set; } = "";

		public List<ChatDM> ChatList { get; set; } = new List<ChatDM>();

	}

}
