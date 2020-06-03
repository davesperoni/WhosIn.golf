using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhosIn.Classes_DataModels;
using WhosIn.Classes_ViewModels;
using WhosIn.Classes_Helper;
using Microsoft.AspNetCore.Components.Web;

namespace WhosIn.Pages
{

    public class GroupsCode : WhosInComponentBase
    {
        protected bool dialogEditIsOpen = false;
        protected bool dialogDeleteIsOpen = false;
        protected bool ModalInAddMode = false;

        public GroupItemDM gItem { get; set; } = new GroupItemDM();
        public List<GroupItemDM> GroupList;

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => this.InitClass());
        }

        protected void InitClass()
        {
            GetGroupList();
        }

        private void  GetGroupList()
        {
            GroupsVM VM = new GroupsVM(m);
            GroupList = VM.GetGroups(true);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }


        protected void OpenEditDialog(string sGroupGuid)
        {
            GroupsVM VM = new GroupsVM(m);
            dialogEditIsOpen = true;
            ModalInAddMode = false;
            gItem = new GroupItemDM();
            gItem.GroupGUID = sGroupGuid;
            VM.GetSingleGroup(gItem);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }


        protected void SaveEditDialog()
        {
            GroupsVM VM = new GroupsVM(m);
            dialogEditIsOpen = false;
            gItem.ValidationFailMessage = "";

            if (ModalInAddMode)
            {
                VM.AddGroup(gItem);
            }
            else
            {
                VM.UpdateGroup(gItem);
            }


            if (m.IsEmpty(VM.ErrorMessage))
            {
                GetGroupList();
            }
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);

            if (!m.IsEmpty(gItem.ValidationFailMessage))
            {
                dialogEditIsOpen = true;
            }


        }

        protected void CancelEditDialog()
        {
            dialogEditIsOpen = false;
        }


        protected void OpenAddDialog()
        {
            GroupsVM VM = new GroupsVM(m);
            gItem = new GroupItemDM();
            dialogEditIsOpen = true;
            ModalInAddMode = true;
        }


        protected void OpenDeleteDialog(string sGroupGuid)
        {
            GroupsVM VM = new GroupsVM(m);
            dialogDeleteIsOpen = true;
            gItem = new GroupItemDM();
            gItem.GroupGUID = sGroupGuid;
            VM.GetSingleGroup(gItem);
            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);
        }

        protected void SaveDeleteDialog()
        {
            dialogDeleteIsOpen = false;

            GroupsVM VM = new GroupsVM(m);
            gItem.ValidationFailMessage = "";

            VM.DeleteGroup(gItem.GroupGUID);

            if (m.IsEmpty(VM.ErrorMessage))
            {
                GetGroupList();
            }

            PutMessagesInView(VM.ErrorMessage, VM.InfoMessage, VM.SecretErrorDetails);

            if (!m.IsEmpty(gItem.ValidationFailMessage))
            {
                dialogDeleteIsOpen = true;
            }

        }

        protected void CancelDeleteDialog()
        {
            dialogDeleteIsOpen = false;
        }


    }
}
