using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhosIn;
using WhosIn.Classes_DataModels;
using WhosIn.Classes_ViewModels;
using WhosIn.Classes_Helper;

namespace WhosIn.Pages
{
    public class AddGroupCode : WhosInComponentBase
    {
        public GroupItemDM groupItem = new GroupItemDM();

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => this.InitPage());
        }

        protected void InitPage()
        {
            
        }


        public void HandleValidSubmit()
        {
            GroupsVM VM = new GroupsVM(m);
            VM.AddGroup(groupItem);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }

        public void CancelForm(MouseEventArgs e)
        {
          //  NavigationManager.NavigateTo("");
        }

    }
}