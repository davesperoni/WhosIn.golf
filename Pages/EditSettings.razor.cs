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
    public class EditSettingsCode : WhosInComponentBase
    {
        public SettingsDM settingItem = new SettingsDM();

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => this.InitPage());
        }

        protected void InitPage()
        {
            UpdateSettingsVM VM = new UpdateSettingsVM(m);
            VM.ReadSettings(settingItem);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }

        public void HandleValidSubmit()
        {
            UpdateSettingsVM VM = new UpdateSettingsVM(m);
            VM.UpdateSettings(settingItem);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }

        public void CancelForm(MouseEventArgs e)
        {
           // NavigationManager.NavigateTo("");
        }
    }
}
