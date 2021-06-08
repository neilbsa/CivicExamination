using ExaminationEntity.ExaminationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CivicExamination.Models
{
  
    public class AppraisalBatchSelect : AppraisalBatch
    {
        public bool IsIncluded { get; set; }
    }

    public class BatchCreateViewModel
    {

      public AppraisalBatch batch { get; set; }
      public List<Ta3Employee> employees { get; set; }
      
    }

}