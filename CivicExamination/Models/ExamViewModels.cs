using ExaminationEntity.ExaminationModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CivicExamination.Models
{
    public class ScheduleTemplateResultViewModel : ScheduleTemplate
    {


        public ICollection<ExaminersResult> Results{ get; set; }
    }

    public class ExaminersResult : ScheduleExaminerModel
    {
        public int PartialScore { get; set; }
        public double TotalScore { get; set; }
        public string Remarks { get; set; }
        public int ItemsNotCheck { get; set; }
    }





    public class ScheduleExaminerViewModel : ScheduleExaminerModel
    {
        public List<questionCorrectionViewModel> CorrectedQuestion { get; set; }
    }


    public class questionCorrectionViewModel : ScheduleQuestion
    {
       
        public bool IsCorrect { get; set; }
        public string essayStatus { get; set; }
    }




}