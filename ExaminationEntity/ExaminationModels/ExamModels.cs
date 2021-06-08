using ExaminationEntity.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExaminationEntity.ExaminationModels
{
    public class Company : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

    }
    public class CategoryQuestion : IDataChangeTracker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string QuestionType { get; set; }

        public string Question { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category CategoryDetail { get; set; }
        public virtual ICollection<QuestionChoices> Choices { get; set; }

        public int? imageId { get; set; }
        [ForeignKey("imageId")]
        public virtual FileRepositoryItem MainQuestionImage { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }

        [NotMapped]
        public HttpPostedFileBase MainQuestionImageModel { get; set; }

    }
    public class FileRepositoryItem : IDataChangeTracker
    {
        public int Id { get; set; }

        public byte[] byteContent { get; set; }
        public string contentType { get; set; }
        public decimal contentLenght { get; set; }


        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public string content64base
        {
            get
            {
                return Convert.ToBase64String(byteContent, 0, byteContent.Length);
            }
        }
    }
    public class QuestionChoices : IDataChangeTracker
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string ChoiceString { get; set; }
        public bool IsCorrectAnswer { get; set; }

        public int? imageId { get; set; }
        [ForeignKey("imageId")]
        public virtual FileRepositoryItem choiceImage { get; set; }


        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }

        [NotMapped]
        public HttpPostedFileBase choiceImageModel { get; set; }
    }
    public class Category : ICompanyTransaction
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        public string CategoryName { get; set; }
        public virtual ICollection<CategoryQuestion> Questions { get; set; }

        public DateTime DateCreated { get; set; }
        public string CreateUser { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string LastUpdateUser { get; set; }

        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company CompanyDetails { get; set; }

    }


    public class ExamTemplate : ICompanyTransaction
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string ExamTemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public string AvatarText { get; set; }
        public virtual ICollection<ExamCategoryTemplate> TemplateBuild { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }
        [Required]
        public int LimitSeconds { get; set; }
        //[Required]
        //public int PassingScore { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company CompanyDetails { get; set; }
        public int CompanyId { get; set; }
    }
    public class ExamCategoryTemplate : IDataChangeTracker
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public int TemplateId { get; set; }
        [ForeignKey("TemplateId")]
        public virtual ExamTemplate TemplateDetails { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category CategoryDetail { get; set; }

        public int Items { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }
    }


    public class UserCompany : ICompanyTransaction
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }


        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }

        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company CompanyDetails { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserDetail { get; set; }


    }

    public class ScheduleTemplate : ICompanyTransaction
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company CompanyDetails { get; set; }
        public int CompanyId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? EndDate { get; set; }
        [ForeignKey("ExaminationId")]
        public virtual ExamTemplate Examination { get; set; }
        public int ExaminationId { get; set; }
        public string ColorString { get; set; }

        public virtual ICollection<ScheduleExaminerModel> ScheduledExaminers { get; set; }



        //[ForeignKey("JobPostingId")]
        //public virtual JobPostingModel JobPostingDetail { get; set; }
        //public int JobPostingId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }

    }


    public class JobPostingExamTemplate : IBaseEntity
    {
        public int Id {get;set;}
        public bool IsDeleted {get;set;}

        public int JobPostingId { get; set; }
        [ForeignKey("JobPostingId")]
        public virtual JobPostingModel JobPostingDetails { get; set; }
        public int ExamTemplateId { get; set; }
        [ForeignKey("ExamTemplateId")]
        public virtual ExamTemplate ExamTemplateDetails { get; set; }
        [Required]
        public float PassingScore { get; set; }

    }




    public class JobPostingModel : ICompanyTransaction
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        //[Required]
        //public int PassingScore { get; set; }
        public string Status { get; set; }
        [Required]
        public string RequestedBy { get; set; }
        public string Remarks { get; set; }

        public ICollection<JobPostingExamTemplate> Examinations { get; set; }

        //public virtual ICollection<ScheduleTemplate> ScheduledExamination { get; set; }



        public virtual Company CompanyDetails { get; set; }
        public int CompanyId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }

    }

    public class ScheduleQuestion : IDataChangeTracker
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("QuestionId")]
        public virtual CategoryQuestion Question { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey("ScheduleExamId")]
        public virtual ScheduleExaminerModel ScheduledExamination { get; set; }
        public int ScheduleExamId { get; set; }

        public virtual ICollection<UserAnswer> UserAnswer { get; set; }


        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }

    }

    public class UserAnswer : QuestionChoices
    {
        [ForeignKey("ScheduleQuestionId")]
        public virtual ScheduleQuestion ScheduledQuestionDetail { get; set; }
        public int ScheduleQuestionId { get; set; }
        //public int OriginalId { get; set; }

        public bool isSelected { get; set; }
    }


    public class ScheduleExaminerModel : IDataChangeTracker
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("ScheduleId")]
        public virtual ScheduleTemplate Schedule { get; set; }
        public int ScheduleId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public int RemainSeconds { get; set; }
        public virtual ICollection<ScheduleQuestion> Questions { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }
    }


    public class UserProfile : ICompanyTransaction
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Middlename { get; set; }
        [Required]
        public string Lastname { get; set; }
        public string Nickname { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public string PresentAddress { get; set; }

        public string PermanentAddress { get; set; }

        public string HomeContact { get; set; }

        public string MobileContact { get; set; }

        public string Gender { get; set; }
        public string UserStatus { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Citizenship { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Religion { get; set; }
        public string CivilStatus { get; set; }
        public string SpouseName { get; set; }
        public int DependentNo { get; set; }
        public string SpouseOccupation { get; set; }
        public string Tin { get; set; }
        public string SSS { get; set; }
        public string Philhealth { get; set; }
        public string Pagibig { get; set; }



        public bool IsLicensedDriver { get; set; }
        public DateTime? DriverLicenseValidity { get; set; }
        public bool IsHasPassport { get; set; }
        public string PassportNo { get; set; }
        public DateTime? PassportValitdity { get; set; }
        public bool IsHasCriminalCase { get; set; }
        public string CriminalCaseNature { get; set; }
        public string ApplyingReason { get; set; }



        public virtual ICollection<EducationAttainment> EducationHistories { get; set; }
        public virtual ICollection<LicenseExamination> LicenseExaminations { get; set; }
        public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; }
        public virtual ICollection<CharacterReference> CharacterReferences { get; set; }


        [ForeignKey("DesiredEmploymentId")]
        public virtual DesiredEmployment DesiredEmploymentDetails { get; set; }
        public int? DesiredEmploymentId { get; set; }

        [ForeignKey("EmergencyContactId")]
        public virtual EmergencyNotification EmergencyContactDetails { get; set; }
        public int? EmergencyContactId { get; set; }



        public bool IsAdministrator { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company CompanyDetails { get; set; }
        public int CompanyId { get; set; }

        public virtual ICollection<ScheduleExaminerModel> ScheduledExaminations { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }
        [NotMapped]
        public string Fullname { get { return String.Format("{0} {1} {2}", Firstname, Middlename, Lastname); } }
    }

    public class CharacterReference : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserDetails { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Fullname { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string Position { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
    }

    public class EmergencyNotification : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        //[ForeignKey("UserId")]
        //public virtual UserProfile UserDetails { get; set; }
        //public int UserId { get; set; }

        public string Name { get; set; }

        public string Relationship { get; set; }

        public string Address { get; set; }

        public string ContactNumber { get; set; }

        public string MobileNumber { get; set; }
    }

    public class DesiredEmployment : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        //[ForeignKey("UserId")]
        //public virtual UserProfile UserDetails { get; set; }
        //public int UserId { get; set; }

        public string PositionAppliedFor { get; set; }
        public double DesiredSalary { get; set; }
        public string OpeningSource { get; set; }
        public bool IsPrevApplicant { get; set; }
        public DateTime? PrevApplication { get; set; }
        public string PrevPositionApplied { get; set; }
        public bool IsPrevHired { get; set; }
        public DateTime? PrevDateHired { get; set; }
        public DateTime? PreDateLeaved { get; set; }
        public string PrevPosition { get; set; }
        public bool CanAssignedToProvince { get; set; }
        public string DesiredProvince { get; set; }
        public bool IsPresentlyEmployed { get; set; }
        public DateTime? DateAvailable { get; set; }

    }

    public class EmploymentHistory : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserDetails { get; set; }
        public int UserId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string InitialPosition { get; set; }
        [Required]
        public string LastPosition { get; set; }
        [Required]
        public double StartingSalary { get; set; }
        [Required]
        public double LastSalary { get; set; }
        public DateTime EmploymentDate { get; set; }
        [Required]
        public string Resposibilities { get; set; }
        [Required]
        public string ReasonForLeaving { get; set; }
    }

    public class LicenseExamination : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserDetails { get; set; }
        public int UserId { get; set; }
        [Required]
        public string LicenseNumber { get; set; }
        [Required]
        public string LicenseName { get; set; }

        public DateTime Expiration { get; set; }

    }

    public class EducationAttainment : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string Type { get; set; }
        public DateTime DateAttendedFrom { get; set; }
        public DateTime DateAttendedTo { get; set; }
        [Required]
        public string NameOfSchool { get; set; }
        [Required]
        public string Address { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserDetails { get; set; }
        public int UserId { get; set; }

    }

    public class UserProfileExamTemplate : IDataChangeTracker
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("UserProfileId")]
        public virtual UserProfile User { get; set; }
        public int UserProfileId { get; set; }

        [ForeignKey("ExamTemplateId")]
        public virtual ExamTemplate ExamTemplate { get; set; }
        public int ExamTemplateId { get; set; }

        public DateTime DateTaken { get; set; }
        public DateTime ScheduledDate { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateUser { get; set; }
    }
}
