using CivicExamProcedures.Context;
using ExaminationEntity.Communication;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CivicExamination.Hubs
{
    [Authorize]
    public class SystemComm : Hub
    {
        private ApplicationContext context { get; set; }
     
        public SystemComm()
        {
            context = new ApplicationContext();
          
        }
 

        public void SendMessage(string recieverId,string message)
        {
            ApplicationUser senderDetail = context.Users.Where(x=>x.UserName == Context.User.Identity.Name).FirstOrDefault() ;
            ApplicationUser recieverDetail = context.Users.Where(x => x.Id == recieverId).FirstOrDefault();
            if(recieverDetail != null)
            {
                string recieverUser = recieverDetail.UserName;
                ChatDetails chat = new ChatDetails { MessageTo = recieverUser, MessageFrom = senderDetail.UserName, Message = message, IsRead = false };
                context.Chats.Add(chat);
                context.SaveChanges();
                Clients.Users(new List<string>() { recieverUser }).sendChat(chat.Message, senderDetail.Id);
            }
        }

        public override Task OnConnected()
        {
            var currentUser = context.Users.Where(x => x.UserName == Context.User.Identity.Name).FirstOrDefault();
            currentUser.ConnectionStatus = "Online";
            context.SaveChanges();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var currentUser = context.Users.Where(x => x.UserName == Context.User.Identity.Name).FirstOrDefault();
            currentUser.ConnectionStatus = "Offline";
            context.SaveChanges();
            return base.OnDisconnected(stopCalled);
        }

    }
}