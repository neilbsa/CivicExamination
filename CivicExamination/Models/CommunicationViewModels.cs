using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CivicExamination.Models
{
    public class ChatHeadViewModel
    {
        public string senderName { get; set; }
        public int NewMessageCount { get; set; }
        public bool HasNew { get; set; }
    }
}