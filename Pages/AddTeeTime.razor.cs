using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhosIn;
using WhosIn.Classes_DataModels;
using WhosIn.Classes_ViewModels;
using WhosIn.Classes_Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

namespace WhosIn.Pages
{
    public class AddTeeTimeCode : WhosInComponentBase
    {
        public string GroupGUIDSelected;
        public DateTime? Timevalue = DateTime.Now;
        public TeeTimeItem TT = new TeeTimeItem();
        public List<GroupItemDM> GroupList = new List<GroupItemDM>();

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => this.InitPage());
        }

        protected void InitPage()
        {
            TeeTimesVM VM = new TeeTimesVM(m);
            VM.TTDefaults(TT);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
            GetListOfGroups();
        }

        private void GetListOfGroups()
        {
            GroupsVM VM = new GroupsVM(m);
            GroupList = VM.GetGroups(false);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);


            GroupGUIDSelected = "";

            // clear the default flag for all items in list.
            foreach (var gr in GroupList)
            {
                gr.GroupDefaultForNewTeeTime = false;
            }

            // set default to item that has default flag on.
            foreach (var gr in GroupList)
            {
                if (!gr.GroupShowAllGroups)
                {
                    if (gr.GroupIsDefault)
                    {
                        gr.GroupDefaultForNewTeeTime = true;
                        GroupGUIDSelected = gr.GroupGUID;
                    }
                }
            }

            // if no default, take the first item.
            if (m.IsEmpty(GroupGUIDSelected))
            {
                foreach (var gr in GroupList)
                {
                    if (!gr.GroupShowAllGroups)
                    {
                        gr.GroupDefaultForNewTeeTime = true;
                        GroupGUIDSelected = gr.GroupGUID;
                        break;
                    }
                }
            }

        }

        public void HandleValidSubmit()
        {
            TeeTimesVM VM = new TeeTimesVM(m);

            TT.GroupGUID = GroupGUIDSelected;
            TT.ValidationFailMessage = "";
            VM.AddNewTeeTime(TT);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);

            if (m.IsEmpty(VM.ErrorMessage) & (m.IsEmpty(TT.ValidationFailMessage)))
            {
                ShowToastSuccess("Tee Time added.");
                NavigationManager.NavigateTo("");
            }
        }

        public void CancelForm(MouseEventArgs e)
        {
            NavigationManager.NavigateTo("");
        }
    }
}
