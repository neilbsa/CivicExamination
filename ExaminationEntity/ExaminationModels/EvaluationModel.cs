using ExaminationEntity.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationEntity.ExaminationModels
{
    //Table for default AppraisalType VS Position
    public class PostionAppraisalTypeLookup : IBaseEntity
    {
        public int Id { get;set; }
        public bool IsDeleted { get;set; }
        public int AppraisalId { get; set; }
        [ForeignKey("AppraisalId")]
        public virtual AppraisalType AppraisalDetail { get; set; }
        public string PositionType { get; set; }
    }






    //Creating AppraisalBatch
    public class AppraisalBatch : IDataChangeTracker
    {

        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Description { get; set; }
        public string Remarks { get; set; }
        public string Period { get; set; }
        public DateTime AppraisalDate { get; set; }
        public virtual List<Ta3Employee> Employees { get; set; }

        public DateTime DateCreated { get;set; }
        public DateTime LastDateUpdated { get;set; }
        public string CreateUser { get;set; }
        public string LastUpdateUser { get;set; }

    }


    public class Ta3Employee : IDataChangeTracker
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public long Ta3Id { get; set; }
        public string Firstname { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeePositionLevel { get; set; }
        public string AcceptanceCode { get; set; }
        public string Dept { get; set; }
        public string EmailAddress { get; set; }
        public string EvaluatorEmployeeId { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public DateTime? DateHired{ get; set; }
        public string Status { get; set; }
        public string CompanyName { get; set; }
        [NotMapped]
        public bool IsIncluded { get; set; }
        public int AppraisalId { get; set; }
        [ForeignKey("AppraisalId")]
        public virtual AppraisalType AppraisalDetails { get; set; }
        public int AppraisalBatchId { get; set; }
        [ForeignKey("AppraisalBatchId")]
        public virtual AppraisalBatch AppraisalBatchDetails { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }

    }


    public class RecomendedTraining : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int EmployeeId { get; set; }
    
        public string TrainingDescription { get; set; }
        public string Timetable { get; set; }
        public string Remarks { get; set; }
    }


    //Eto yung groupings ng appraisal by period
    public class AppraisalEmployeeAnswerModel : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Ta3Employee EmployeeDetail { get; set; }
        public int AnswerSheetId { get; set; }


        public string Comment { get; set; }
        public bool PositionRetained { get; set; }
        public bool MeritIncrease { get; set; }
        public bool ForPomotion { get; set; }
        public bool ForTransfer { get; set; }
        public string Reasons { get; set; }
        public int KRAPercentage { get; set; }
        public virtual List<RecomendedTraining> RecommendedTrainings { get; set; }


        public virtual List<KRAModels> KRAs { get; set; }
        public virtual List<CompetenciesGroupingAnswer> CompetencygroupAnswer { get; set; }

    }

 


    public class CompetenciesGroupingAnswer : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string GroupType { get; set; } // Part1 Part2 Part3
        public string Description { get; set; }
        public string Comment { get; set; }
        public int Percentage { get; set; }
        public virtual List<AnswerRatingScale> RatingScales { get; set; }
        public virtual List<CompetenciesAnswerSheets> CompetenciesMemberAnswers { get; set; }
    }

    public class AnswerRatingScale : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public string ScoreDetails { get; set; }
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual CompetenciesGroupingAnswer GroupDetails { get; set; }
    }



    //dito mmag store lahat ng sagot sa Appraisal
    public class CompetenciesAnswerSheets : IDataChangeTracker
    {

        public int Id { get; set; }
        public bool IsDeleted { get; set; }


        public string Title { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public float Rating { get; set; }
      


        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }

    }
    //eto yung groupings apraisal type like Staff or Officer
    public class AppraisalType : IDataChangeTracker
    {

        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Type { get; set; } // Staff or Officer
        public string Description { get; set; }
        public int KRAPercentage { get; set; }
        public virtual List<CompetenciesGrouping> Competencies { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }



    }
    //Dito Mag store lahat ng KRA ng employee
    public class KRAModels : IDataChangeTracker
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Description { get; set; }
        public float RawScore { get; set; }
        public float Weight { get; set; }
        public float IndexScore { get; set; }
        public int AppraisalId { get; set; }
        [ForeignKey("AppraisalId")]
        public virtual AppraisalEmployeeAnswerModel AppraisalDetails { get; set; }



        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }
    }

    //Dito ma sstore lahat ng Appraisal Groups
    public class CompetenciesGrouping : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string GroupType { get; set; } // Part1 Part2 Part3
        public string Description { get; set; }
        public int Percentage { get; set; }
        public virtual List<RatingScale> RatingScale { get; set; }
        public virtual List<CompetenciesModels> CompetenciesMember { get; set; }
    }


    public class RatingScale : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public string ScoreDetails { get; set; }
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual CompetenciesGrouping GroupDetails { get; set; }
    }


    //Dito magstore lahat ng Question sa Competencies per group
    public class CompetenciesModels : IDataChangeTracker
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual CompetenciesGrouping group { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }
    }




}
