using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WhosIn.Classes_Helper;

namespace WhosIn.Pages
{

    public class SecureTestCode : WhosInComponentBase
    {

        protected string _authMessage;
        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();


        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => this.GetClaimsPrincipalData());
        }
 
        private async Task GetClaimsPrincipalData()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                _authMessage = $"{user.Identity.Name} is authenticated.";
            }
            else
            {
                _authMessage = "The user is NOT authenticated.";
            }
        }


    }
}
