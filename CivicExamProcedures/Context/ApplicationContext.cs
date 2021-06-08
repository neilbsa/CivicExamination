using ExaminationEntity.BaseModels;
using ExaminationEntity.ExaminationModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using ExaminationEntity.Communication;

namespace CivicExamProcedures.Context
{
    public class ApplicationUser : IdentityUser
    {
        public int? UserDetailId { get; set; }
        [ForeignKey("UserDetailId")]
        public virtual UserProfile UserDetail { get; set; }
        [Required]
        public string CompanyId { get; set; }
        public string ConnectionStatus { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext()
            : base("MainConnection", throwIfV1Schema: false)
        {

        }




        public DbSet<Company> Companies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryQuestion> Questions { get; set; }
        public DbSet<QuestionChoices> QuestionChoices { get; set; }
        public DbSet<ExamTemplate> ExamTemplates { get; set; }
        public DbSet<ExamCategoryTemplate> ExamCategoryTemplates { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserProfileExamTemplate> UserProfileExamTemplate { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<FileRepositoryItem> FileRepository { get; set; }
        public DbSet<ScheduleExaminerModel> ScheduleExaminationModels { get; set; }
        public DbSet<ScheduleTemplate> ScheduleTemplates { get; set; }
        public DbSet<ScheduleQuestion> ScheduleUserQuestions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<JobPostingModel> JobPostings { get; set; }
        public DbSet<CharacterReference> CharacterReferences{ get; set; }
        public DbSet<EmergencyNotification> EmergencyContacts { get; set; }
        public DbSet<DesiredEmployment> DesiredEmployments { get; set; }
        public DbSet<EmploymentHistory> EmploymentHistories { get; set; }
        public DbSet<LicenseExamination> LicenseExaminations { get; set; }
        public DbSet<EducationAttainment> EducationHistories { get; set; }
        public DbSet<JobPostingExamTemplate> JobPostingExamTemplates { get; set;}


        //EvaluationDb

        public DbSet<AppraisalEmployeeAnswerModel> AppraisalEmployeeTbl { get; set; }
        public DbSet<CompetenciesAnswerSheets> CompetenciesAnswerSheetsTbl { get; set; }
        public DbSet<AppraisalType> AppraisalTypesTbl { get; set; }
        public DbSet<KRAModels> KRATbl { get; set; }
        public DbSet<CompetenciesGrouping> CompetenciesGroupTbl { get; set; }
        public DbSet<CompetenciesModels> CompetenciesTbl { get; set; }

        public DbSet<AppraisalBatch> Batches { get; set; }
        public DbSet<Ta3Employee> BatchEmployee { get; set; }
        public DbSet<PostionAppraisalTypeLookup> AppraisalLookup { get; set; }

    
        public DbSet<CompetenciesGroupingAnswer> CompetenciesGroupingAnswerSheet { get; set; }
        public DbSet<AnswerRatingScale> AnswerRatingScale { get; set; }
        public DbSet<RatingScale> GroupRatingScale { get; set; }
        public DbSet<RecomendedTraining> RecommendedTrainings { get; set; }


        //communication

        public DbSet<ChatDetails> Chats { get; set; }



        public override int SaveChanges()
        {
            AddDateStamp();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            AddDateStamp();
            return base.SaveChangesAsync(cancellationToken);
        }
    

       
        private void AddDateStamp()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IDataChangeTracker && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var currentUsername = !string.IsNullOrEmpty(System.Web.HttpContext.Current?.User?.Identity?.Name)
                ? HttpContext.Current.User.Identity.Name
                : "Anonymous";

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {


                    //Company currentCompany = new Company();
                    //if ((Company)System.Web.HttpContext.Current.Session["COMPANY"] != null)
                    //{
                    //    currentCompany = (Company)System.Web.HttpContext.Current.Session["COMPANY"];
                    //}
                    //int companyId = currentCompany != null ? currentCompany.Id : 10000;


                    ((IDataChangeTracker)entity.Entity).DateCreated = DateTime.Now;
                    ((IDataChangeTracker)entity.Entity).CreateUser= currentUsername;
          
                }

                ((IDataChangeTracker)entity.Entity).LastDateUpdated = DateTime.Now;
                ((IDataChangeTracker)entity.Entity).LastUpdateUser = currentUsername;
            }
        }


       


        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

       
    }
}
