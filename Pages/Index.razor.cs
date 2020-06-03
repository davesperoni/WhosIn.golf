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
using Radzen.Blazor;
using MatBlazor;
using Radzen;
using Microsoft.AspNetCore.Components.Authorization;

namespace WhosIn.Pages
{
    public class IndexCode : WhosInComponentBase
    {

        public BaseMatButton MenuButtonB;
        public BaseMatMenu MatMenu;

        public string GroupGUIDSelectedMain = "";
        public string GroupGUIDSelectedDialog = "";

        public string ChatGUIDSelected;
        public bool ShowAllTeeTimes = false;
        public int NumberOfTTs;

        protected bool dialogAddPlayerIsOpen = false;
        protected bool dialogEditTTisOpen = false;
        protected bool dialogDeleteTTisOpen = false;
        protected bool dialogViewLogIsOpen = false;
        protected bool dialogLogIsOpen = false;
        protected bool dialogAddChatIsOpen = false;
        protected bool dialogDeleteChatisOpen = false;

        protected List<TeeTimeItem> TTList { get; set; } = new List<TeeTimeItem>();
        protected TeeTimeItem TTDialog = new TeeTimeItem();
        protected List<AuditLogDM> AuditLogList { get; set; } = new List<AuditLogDM>();
        protected List<ChatDM> ChatList;
        protected ChatDM chatItem = new ChatDM();
        public List<GroupItemDM> GroupList = new List<GroupItemDM>();


        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => this.InitPage());
            await CheckIfLoggedInAsync();
        }

        protected void InitPage()
        {
            GetListOfGroups();
            GetListOfTT();
        }

        private void GetListOfTT()
        {
            TeeTimesVM VM = new TeeTimesVM(m);
            TTList = VM.GetListOfTeeTimes(GroupGUIDSelectedMain, ShowAllTeeTimes);
            NumberOfTTs = TTList.Count;
            ChatGUIDSelected = null;
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }

        private void GetListOfGroups()
        {
            GroupsVM VM = new GroupsVM(m);
            GroupList = VM.GetGroups(false);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);


            // this is needed so the @bind-Value of the dropdown has an initial value, which should be the same as the item shown in the dropdow.
            foreach (var gr in GroupList)
            {
                if (m.IsEmpty(GroupGUIDSelectedMain))
                {
                    GroupGUIDSelectedMain = gr.GroupGUID;
                    GroupGUIDSelectedDialog = gr.GroupGUID;
                }

                if (gr.GroupIsDefault)
                {
                    GroupGUIDSelectedMain = gr.GroupGUID;
                    GroupGUIDSelectedDialog = gr.GroupGUID;

                }
            }
        }


        protected void OpenAddPlayerDialogMatButton(int iPlayerNumber, string sTTGuid, string sCurrentPlayerName, string sCurrentPlayerComment, bool PlayerIsLocked)
        {
            if (PlayerIsLocked)
            {
                return;
            }

            TTDialog = new TeeTimeItem();
            dialogAddPlayerIsOpen = true;

            TTDialog.TTGUID = sTTGuid;
            TTDialog.PlayerNumberBeingEdited = iPlayerNumber;
            TTDialog.PlayerNameBeingEdited = sCurrentPlayerName;
            TTDialog.PlayerCommentBeingEdited = sCurrentPlayerComment;
        }

        protected void SaveAddPlayerDialog()
        {
            TeeTimesVM VM = new TeeTimesVM(m);

            dialogAddPlayerIsOpen = false;

            switch (TTDialog.PlayerNumberBeingEdited)
            {
                case 1:
                    TTDialog.Player1 = TTDialog.PlayerNameBeingEdited;
                    TTDialog.Player1Comment = TTDialog.PlayerCommentBeingEdited;
                    break;
                case 2:
                    TTDialog.Player2 = TTDialog.PlayerNameBeingEdited;
                    TTDialog.Player2Comment = TTDialog.PlayerCommentBeingEdited;
                    break;
                case 3:
                    TTDialog.Player3 = TTDialog.PlayerNameBeingEdited;
                    TTDialog.Player3Comment = TTDialog.PlayerCommentBeingEdited;
                    break;
                case 4:
                    TTDialog.Player4 = TTDialog.PlayerNameBeingEdited;
                    TTDialog.Player4Comment = TTDialog.PlayerCommentBeingEdited;
                    break;
                default:
                    break;
            }

            VM.UpdateTTPlayer(TTDialog);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);

            GetListOfTT();

            if (!m.IsEmpty(TTDialog.ValidationFailMessage))
            {
                dialogEditTTisOpen = true;
            }

        }



        protected void OpenEditDialog(string sTTGuid)
        {
            TTDialog = new TeeTimeItem();
            TeeTimesVM VM = new TeeTimesVM(m);

            dialogEditTTisOpen = true;
            TTDialog.TTGUID = sTTGuid;
            VM.GetSingleTeeTime(TTDialog);
            GroupGUIDSelectedDialog = TTDialog.GroupGUID;
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }

        protected void SaveEditDialog()
        {
            TeeTimesVM VM = new TeeTimesVM(m);

            dialogEditTTisOpen = false;
            TTDialog.ValidationFailMessage = "";
            TTDialog.GroupGUID = GroupGUIDSelectedDialog;
            VM.UpdateTTInfo(TTDialog);
            GetListOfTT();
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);

            if (!m.IsEmpty(TTDialog.ValidationFailMessage))
            {
                dialogEditTTisOpen = true;
            }

        }

        protected void CancelEditDialog()
        {
            dialogEditTTisOpen = false;
        }



        protected void OpenDeleteDialog(string sTTGuid)
        {
            TeeTimeItem tItem = new TeeTimeItem();
            TeeTimesVM VM = new TeeTimesVM(m);

            dialogDeleteTTisOpen = true;
            tItem.TTGUID = sTTGuid;
            VM.GetSingleTeeTime(tItem);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);

            TTDialog = new TeeTimeItem();
            TTDialog.TTGUID = tItem.TTGUID;
            TTDialog.GroupGUID = tItem.GroupGUID;
            TTDialog.TTDate = tItem.TTDate;
            TTDialog.TTTime = tItem.TTTime;
            TTDialog.DaysFromNow = tItem.DaysFromNow;
            TTDialog.GroupName = tItem.GroupName;
            TTDialog.Location = tItem.Location;
            TTDialog.Owner = tItem.Owner;
            TTDialog.OwnerComment = tItem.OwnerComment;
            TTDialog.IsDeleted = tItem.IsDeleted;
            TTDialog.PinNumberEntered = "";
            TTDialog.LockPlayer1 = tItem.LockPlayer1;
            TTDialog.LockPlayer2 = tItem.LockPlayer2;
            TTDialog.LockPlayer3 = tItem.LockPlayer3;
            TTDialog.LockPlayer4 = tItem.LockPlayer4;
            TTDialog.LockMessages = tItem.LockMessages;
            TTDialog.HideMessages = tItem.HideMessages;

        }

        protected void SaveDeleteDialog(bool bUndelete)
        {
            dialogDeleteTTisOpen = false;

            TeeTimesVM VM = new TeeTimesVM(m);
            TTDialog.ValidationFailMessage = "";

            VM.MarkTTasDeleted(TTDialog, bUndelete);

            if (m.IsEmpty(VM.ErrorMessage))
            {
                GetListOfTT();
            }

            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);

            if (!m.IsEmpty(TTDialog.ValidationFailMessage))
            {
                dialogDeleteTTisOpen = true;
            }

        }

        protected void CancelDeleteDialog()
        {
            dialogDeleteTTisOpen = false;
        }



        protected void GetChatItem(string sTTGUID)
        {
            ChatVM VM = new ChatVM(m);
            ChatList = VM.GetChatList(sTTGUID);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }


        protected void OpenDialogAddChat(string sTTGuid)
        {
            chatItem = new ChatDM();
            dialogAddChatIsOpen = true;
            chatItem.TeeTimeGUID = sTTGuid;
        }




        protected void SaveDialogAddChat()
        {
            ChatVM VM = new ChatVM(m);
            chatItem.ValidationFailMessage = "";
            dialogAddChatIsOpen = false;
            VM.AddChatItem(chatItem);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);

            if (!m.IsEmpty(chatItem.ValidationFailMessage))
            {
                dialogAddChatIsOpen = true;
                return;
            }

            if (m.IsEmpty(VM.ErrorMessage))
            {
                GetListOfTT();
            }
        }



        protected void CancelDialogAddChat()
        {
            dialogAddChatIsOpen = false;
        }


        protected void OpenDialogLog(string sTTGuid)
        {
            dialogLogIsOpen = true;
            AuditLogVM VM = new AuditLogVM(m);
            TeeTimesVM VM2 = new TeeTimesVM(m);
            TTDialog = new TeeTimeItem();
            TTDialog.TTGUID = sTTGuid;

            AuditLogList = VM.GetAuditLog(sTTGuid);
            VM2.GetSingleTeeTime(TTDialog);

            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }

        protected void CloseDialogAuditLog()
        {
            dialogLogIsOpen = false;
        }


        protected void OpenDialogDeleteChat()
        {
            ChatVM VM = new ChatVM(m);
            chatItem = new ChatDM();

            if (m.IsEmpty(ChatGUIDSelected))
            {
                return;
            }

            dialogDeleteChatisOpen = true;
            chatItem = VM.GetSingleChatItem(ChatGUIDSelected);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }

        protected void SaveDialogDeleteChat()
        {
            ChatVM VM = new ChatVM(m);
            dialogDeleteChatisOpen = false;
            VM.DeleteChatItem(chatItem.ChatGUID);
            if (m.IsEmpty(VM.ErrorMessage))
            {
                GetListOfTT();
            }
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }

        protected void CancelDialogDeleteChat()
        {
            dialogDeleteChatisOpen = false;
        }


        protected void OnShowAllChanged()
        {
            GetListOfTT();
        }


        protected void EditMenuClicked(MenuItemEventArgs args, string sTTGuid)
        {
            if (args.Text == "Edit")
            {
                OpenEditDialog(sTTGuid);
            }

            if (args.Text == "Audit Log")
            {
                OpenDialogLog(sTTGuid);
            }

            if (args.Text == "Delete")
            {
                OpenDeleteDialog(sTTGuid);
            }

            if (args.Text == "Undelete")
            {
                OpenDeleteDialog(sTTGuid);
            }


        }

        protected void GoToAddNew()
        {
            NavigationManager.NavigateTo("AddTeeTime");
        }


        protected void DropDownChanged()
        {
            GetListOfTT();
        }



        public void OnOpenMenu(MouseEventArgs e)
        {
            this.MatMenu.OpenAsync(MenuButtonB.Ref);
        }

        protected void MenuEditClicked(string sTTGuid)
        {
            OpenEditDialog(sTTGuid);
        }

        protected void MenuAuditLogClicked(string sTTGuid)
        {
            OpenDialogLog(sTTGuid);
        }

        protected void MenuClickDelete(string sTTGuid)
        {
            OpenDeleteDialog(sTTGuid);
        }



        private async Task GetClaimsPrincipalData()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                IsLoggedIn = true;
            }
        }



    }

}


