using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhosIn.Classes_Helper
{
    public class WhosInComponentBase : ComponentBase
    {
        [Inject]
        protected cMain m { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }


        [Inject]
        public IMatToaster Toaster { get; set; }

        public string BaseErrorMessage { get; set; } = "";
        public string BaseInfoMessage { get; set; } = "";
        public string BaseSecretErrorDetails { get; set; } = "";
        public bool IsLoggedIn = false;


        public WhosInComponentBase()
        {
        }

        protected override void OnInitialized()
        {
            ReadConfig.ReadAppSettingFile(m);
        }

        protected async Task CheckIfLoggedInAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            IsLoggedIn = user.Identity.IsAuthenticated;
        }

        protected bool PutMessagesInView(string sErr, string sInfo = "", string sSecretErrorDetails = "")
        {
            BaseErrorMessage = m.NoNull(sErr);
            BaseInfoMessage = m.NoNull(sInfo);

            if (m.x_bIncludeSecretInfoInErrMsgToUser)
            {
                BaseSecretErrorDetails = m.NoNull(sSecretErrorDetails);
            }

            if ((!m.IsEmpty(BaseErrorMessage)))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected void LoadExceptionInfoMessage(Exception e)
        {
            PutMessagesInView(e.Message, "", m.ShortenStack(e.StackTrace, true));
        }

        protected bool HasErrorMsg()
        {
            if (m.IsEmpty(BaseErrorMessage))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        protected void ShowToastSuccess(string sMessage)
        {
            Toaster.Add(sMessage, MatToastType.Success, "", "", config =>
            {
                config.VisibleStateDuration = 2000;
            });
        }

        protected void ShowToastWarning(string sMessage)
        {
            Toaster.Add(sMessage, MatToastType.Warning, "", "", config =>
            {
                config.VisibleStateDuration = 5000;
            });
        }

    }

}
