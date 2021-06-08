using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CivicExamination.Models
{
    public class ExistingAccountViewModel
    {
        public double Months { get; set; }
        public string ExamName { get; set; }
        public DateTime DateScheduled { get; set; }
        public DateTime EndScheduled { get; set; }
        public DateTime? DateFinish { get; set; }
    }
}