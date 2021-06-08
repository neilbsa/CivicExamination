using ExaminationEntity.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationEntity.Communication
{
    public class ChatDetails : IDataChangeTracker
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string MessageFrom { get; set; }
        public string MessageTo { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
 

      


        public DateTime DateCreated {get;set; }
        public DateTime LastDateUpdated {get;set; }
        public string CreateUser {get;set; }
        public string LastUpdateUser {get;set; }

    }
}
