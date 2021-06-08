using CivicExamination.Models;
using CivicExamProcedures.Context;
using ExaminationEntity.Communication;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CivicExamination.Controllers
{



    public class ChatMemberViewModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public bool IsVisible { get; set; }
    }
    [Authorize]
    public class CommunicationController : BaseController<ChatDetails>
    {
        //public override ActionResult Index()
        //{          
        //    var listOnlineUser = UserManager.Users.Where(x => x.UserName != User.Identity.Name);
        //    List<ChatHeadViewModel> mod = new List<ChatHeadViewModel>();
        //    foreach (var user in listOnlineUser)
        //    {
        //        //check if has offline Message
        //        var listofMessage = BaseMethods.GetList(x => (x.MessageFrom == user.UserName && x.MessageTo == User.Identity.Name) || (x.MessageTo == user.UserName && x.MessageFrom == User.Identity.Name));
        //        bool hasUnread = listofMessage.Where(x => x.IsRead == false).Count() > 0 ? true : false;
        //        mod.Add(new ChatHeadViewModel() { HasNew = hasUnread, senderName = user.UserName, NewMessageCount = listofMessage.Where(x => x.MessageTo == User.Identity.Name && x.IsRead == false).Count() });
        //    }

        //    return View(mod);
        //}

        
        [HttpPost]
        public JsonResult GetUserUpdate()
        {
           
            if (!User.IsInRole("Common"))
            {
                List<ChatMemberViewModel> listOnlineUser = UserManager.Users.Where(x => x.UserName != User.Identity.Name).Select(x=> new ChatMemberViewModel () {  Firstname = x.UserDetail.Firstname,  Middlename = x.UserDetail.Middlename, Lastname= x.UserDetail.Lastname, Id= x.Id, IsVisible= true }).ToList();
                return Json(listOnlineUser, JsonRequestBehavior.DenyGet);
            }
            else
            {
                List<ChatMemberViewModel> listOnlineUser = UserManager.Users.Where(x => x.UserName != User.Identity.Name).Select(x=> new ChatMemberViewModel() { Firstname = x.UserDetail.Firstname, Middlename = x.UserDetail.Middlename, Lastname = x.UserDetail.Lastname, Id = x.Id, IsVisible = true }).ToList();
                foreach(var item in listOnlineUser)
                {
                    List<string> userRoles = UserManager.GetRoles(item.Id).ToList();
                   
                    if (userRoles.Contains("Common"))
                    {
                        item.IsVisible = false;
                    }
                }

                return Json(listOnlineUser, JsonRequestBehavior.DenyGet);

            }
        }

        [HttpPost]
        public ActionResult GetPopChatContainer(string userFrom)
        {
            var currentUser = User.Identity.Name;
            var userFromDetail = UserManager.Users.Where(x => x.Id == userFrom).FirstOrDefault();
            List<ChatDetails> chats = new List<ChatDetails>();
            if (userFromDetail != null)
            {
                chats = BaseMethods.GetList(x => (x.MessageFrom == userFromDetail.UserName && x.MessageTo == currentUser) || (x.MessageFrom == currentUser && x.MessageTo == userFromDetail.UserName)).OrderBy(x => x.Id).ToList();
                ViewBag.userFrom = userFromDetail.UserDetail.Firstname;
                ViewBag.userId = userFromDetail.Id;
            } 
            return View("PopChatContainer", chats);
        }

        public ActionResult GetMessageContainer (string type, string message)
        {
            if(type == "Sent")
            {
                return PartialView("PopChatItemDarker",message);
            }
            else
            {
                return PartialView("PopChatItem", message);
            }
        }

    }
}