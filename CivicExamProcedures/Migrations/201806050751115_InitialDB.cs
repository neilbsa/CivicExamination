namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppraisalEmployeeAnswerModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        AnswerSheetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ta3Employee", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.CompetenciesGroupingAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        GroupType = c.String(),
                        AppraisalEmployeeAnswerModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalEmployeeAnswerModels", t => t.AppraisalEmployeeAnswerModel_Id)
                .Index(t => t.AppraisalEmployeeAnswerModel_Id);
            
            CreateTable(
                "dbo.CompetenciesAnswerSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Remarks = c.String(),
                        Rating = c.Single(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                        CompetenciesGroupingAnswer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompetenciesGroupingAnswers", t => t.CompetenciesGroupingAnswer_Id)
                .Index(t => t.CompetenciesGroupingAnswer_Id);
            
            CreateTable(
                "dbo.Ta3Employee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Ta3Id = c.Long(nullable: false),
                        Firstname = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        EmployeeNo = c.String(),
                        EmployeePositionLevel = c.String(),
                        AcceptanceCode = c.String(),
                        Status = c.String(),
                        AppraisalId = c.Int(nullable: false),
                        AppraisalBatchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalBatches", t => t.AppraisalBatchId, cascadeDelete: true)
                .ForeignKey("dbo.AppraisalTypes", t => t.AppraisalId, cascadeDelete: true)
                .Index(t => t.AppraisalId)
                .Index(t => t.AppraisalBatchId);
            
            CreateTable(
                "dbo.AppraisalBatches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Description = c.String(),
                        Remarks = c.String(),
                        Period = c.String(),
                        AppraisalDate = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppraisalTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Type = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompetenciesGroupings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        GroupType = c.String(),
                        AppraisalType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalTypes", t => t.AppraisalType_Id)
                .Index(t => t.AppraisalType_Id);
            
            CreateTable(
                "dbo.CompetenciesModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        GroupId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompetenciesGroupings", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.KRAModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Description = c.String(),
                        RawScore = c.Single(nullable: false),
                        Weight = c.Single(nullable: false),
                        AppraisalId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalEmployeeAnswerModels", t => t.AppraisalId, cascadeDelete: true)
                .Index(t => t.AppraisalId);
            
            CreateTable(
                "dbo.PostionAppraisalTypeLookups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        AppraisalId = c.Int(nullable: false),
                        PositionType = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppraisalTypes", t => t.AppraisalId, cascadeDelete: true)
                .Index(t => t.AppraisalId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CategoryName = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastDateUpdated = c.DateTime(nullable: false),
                        LastUpdateUser = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        QuestionType = c.String(nullable: false),
                        Question = c.String(),
                        CategoryId = c.Int(nullable: false),
                        imageId = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.FileRepositoryItems", t => t.imageId)
                .Index(t => t.CategoryId)
                .Index(t => t.imageId);
            
            CreateTable(
                "dbo.QuestionChoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        ChoiceString = c.String(),
                        IsCorrectAnswer = c.Boolean(nullable: false),
                        imageId = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                        ScheduleQuestionId = c.Int(),
                        isSelected = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        CategoryQuestion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileRepositoryItems", t => t.imageId)
                .ForeignKey("dbo.ScheduleQuestions", t => t.ScheduleQuestionId, cascadeDelete: true)
                .ForeignKey("dbo.CategoryQuestions", t => t.CategoryQuestion_Id)
                .Index(t => t.imageId)
                .Index(t => t.ScheduleQuestionId)
                .Index(t => t.CategoryQuestion_Id);
            
            CreateTable(
                "dbo.FileRepositoryItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        byteContent = c.Binary(),
                        contentType = c.String(),
                        contentLenght = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScheduleQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        ScheduleExamId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryQuestions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.ScheduleExaminerModels", t => t.ScheduleExamId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.ScheduleExamId);
            
            CreateTable(
                "dbo.ScheduleExaminerModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        ScheduleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Status = c.String(),
                        RemainSeconds = c.Int(nullable: false),
                        FinishDate = c.DateTime(),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ScheduleTemplates", t => t.ScheduleId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ScheduleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ScheduleTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        ExaminationId = c.Int(nullable: false),
                        ColorString = c.String(),
                        JobPostingId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .ForeignKey("dbo.ExamTemplates", t => t.ExaminationId, cascadeDelete: true)
                .ForeignKey("dbo.JobPostingModels", t => t.JobPostingId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.ExaminationId)
                .Index(t => t.JobPostingId);
            
            CreateTable(
                "dbo.ExamTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        ExamTemplateName = c.String(),
                        TemplateDescription = c.String(),
                        AvatarText = c.String(),
                        Status = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                        LimitSeconds = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.ExamCategoryTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        TemplateId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Items = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.ExamTemplates", t => t.TemplateId, cascadeDelete: true)
                .Index(t => t.TemplateId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.JobPostingModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        JobName = c.String(nullable: false),
                        JobDescription = c.String(),
                        PassingScore = c.Int(nullable: false),
                        Status = c.String(),
                        RequestedBy = c.String(nullable: false),
                        Remarks = c.String(),
                        CompanyId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Firstname = c.String(nullable: false),
                        Middlename = c.String(nullable: false),
                        Lastname = c.String(nullable: false),
                        Nickname = c.String(),
                        EmailAddress = c.String(nullable: false),
                        PresentAddress = c.String(),
                        PermanentAddress = c.String(),
                        HomeContact = c.String(),
                        MobileContact = c.String(),
                        Gender = c.String(),
                        UserStatus = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        BirthPlace = c.String(),
                        Citizenship = c.String(),
                        Age = c.Int(nullable: false),
                        Height = c.Double(nullable: false),
                        Weight = c.Double(nullable: false),
                        Religion = c.String(),
                        CivilStatus = c.String(),
                        SpouseName = c.String(),
                        DependentNo = c.Int(nullable: false),
                        SpouseOccupation = c.String(),
                        Tin = c.String(),
                        SSS = c.String(),
                        Philhealth = c.String(),
                        Pagibig = c.String(),
                        IsLicensedDriver = c.Boolean(nullable: false),
                        DriverLicenseValidity = c.DateTime(),
                        IsHasPassport = c.Boolean(nullable: false),
                        PassportNo = c.String(),
                        PassportValitdity = c.DateTime(),
                        IsHasCriminalCase = c.Boolean(nullable: false),
                        CriminalCaseNature = c.String(),
                        ApplyingReason = c.String(),
                        DesiredEmploymentId = c.Int(),
                        EmergencyContactId = c.Int(),
                        IsAdministrator = c.Boolean(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .ForeignKey("dbo.DesiredEmployments", t => t.DesiredEmploymentId)
                .ForeignKey("dbo.EmergencyNotifications", t => t.EmergencyContactId)
                .Index(t => t.DesiredEmploymentId)
                .Index(t => t.EmergencyContactId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.CharacterReferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        Fullname = c.String(nullable: false),
                        ContactNumber = c.String(nullable: false),
                        Company = c.String(nullable: false),
                        Position = c.String(nullable: false),
                        EmailAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DesiredEmployments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        PositionAppliedFor = c.String(),
                        DesiredSalary = c.Double(nullable: false),
                        OpeningSource = c.String(),
                        IsPrevApplicant = c.Boolean(nullable: false),
                        PrevApplication = c.DateTime(),
                        PrevPositionApplied = c.String(),
                        IsPrevHired = c.Boolean(nullable: false),
                        PrevDateHired = c.DateTime(),
                        PreDateLeaved = c.DateTime(),
                        PrevPosition = c.String(),
                        CanAssignedToProvince = c.Boolean(nullable: false),
                        DesiredProvince = c.String(),
                        IsPresentlyEmployed = c.Boolean(nullable: false),
                        DateAvailable = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EducationAttainments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Type = c.String(nullable: false),
                        DateAttendedFrom = c.DateTime(nullable: false),
                        DateAttendedTo = c.DateTime(nullable: false),
                        NameOfSchool = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EmergencyNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Name = c.String(),
                        Relationship = c.String(),
                        Address = c.String(),
                        ContactNumber = c.String(),
                        MobileNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmploymentHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        CompanyName = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        ContactNumber = c.String(nullable: false),
                        InitialPosition = c.String(nullable: false),
                        LastPosition = c.String(nullable: false),
                        StartingSalary = c.Double(nullable: false),
                        LastSalary = c.Double(nullable: false),
                        EmploymentDate = c.DateTime(nullable: false),
                        Resposibilities = c.String(nullable: false),
                        ReasonForLeaving = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LicenseExaminations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        LicenseNumber = c.String(nullable: false),
                        LicenseName = c.String(nullable: false),
                        Expiration = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                        CompanyId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserProfileExamTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                        ExamTemplateId = c.Int(nullable: false),
                        DateTaken = c.DateTime(nullable: false),
                        ScheduledDate = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExamTemplates", t => t.ExamTemplateId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.ExamTemplateId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserDetailId = c.Int(),
                        CompanyId = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserDetailId)
                .Index(t => t.UserDetailId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserDetailId", "dbo.UserProfiles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserProfileExamTemplates", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.UserProfileExamTemplates", "ExamTemplateId", "dbo.ExamTemplates");
            DropForeignKey("dbo.UserCompanies", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.UserCompanies", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CategoryQuestions", "imageId", "dbo.FileRepositoryItems");
            DropForeignKey("dbo.QuestionChoices", "CategoryQuestion_Id", "dbo.CategoryQuestions");
            DropForeignKey("dbo.QuestionChoices", "ScheduleQuestionId", "dbo.ScheduleQuestions");
            DropForeignKey("dbo.ScheduleExaminerModels", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.LicenseExaminations", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.EmploymentHistories", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.UserProfiles", "EmergencyContactId", "dbo.EmergencyNotifications");
            DropForeignKey("dbo.EducationAttainments", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.UserProfiles", "DesiredEmploymentId", "dbo.DesiredEmployments");
            DropForeignKey("dbo.UserProfiles", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CharacterReferences", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.ScheduleExaminerModels", "ScheduleId", "dbo.ScheduleTemplates");
            DropForeignKey("dbo.ScheduleTemplates", "JobPostingId", "dbo.JobPostingModels");
            DropForeignKey("dbo.JobPostingModels", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.ScheduleTemplates", "ExaminationId", "dbo.ExamTemplates");
            DropForeignKey("dbo.ExamCategoryTemplates", "TemplateId", "dbo.ExamTemplates");
            DropForeignKey("dbo.ExamCategoryTemplates", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.ExamTemplates", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.ScheduleTemplates", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.ScheduleQuestions", "ScheduleExamId", "dbo.ScheduleExaminerModels");
            DropForeignKey("dbo.ScheduleQuestions", "QuestionId", "dbo.CategoryQuestions");
            DropForeignKey("dbo.QuestionChoices", "imageId", "dbo.FileRepositoryItems");
            DropForeignKey("dbo.CategoryQuestions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.PostionAppraisalTypeLookups", "AppraisalId", "dbo.AppraisalTypes");
            DropForeignKey("dbo.KRAModels", "AppraisalId", "dbo.AppraisalEmployeeAnswerModels");
            DropForeignKey("dbo.AppraisalEmployeeAnswerModels", "EmployeeId", "dbo.Ta3Employee");
            DropForeignKey("dbo.Ta3Employee", "AppraisalId", "dbo.AppraisalTypes");
            DropForeignKey("dbo.CompetenciesGroupings", "AppraisalType_Id", "dbo.AppraisalTypes");
            DropForeignKey("dbo.CompetenciesModels", "GroupId", "dbo.CompetenciesGroupings");
            DropForeignKey("dbo.Ta3Employee", "AppraisalBatchId", "dbo.AppraisalBatches");
            DropForeignKey("dbo.CompetenciesGroupingAnswers", "AppraisalEmployeeAnswerModel_Id", "dbo.AppraisalEmployeeAnswerModels");
            DropForeignKey("dbo.CompetenciesAnswerSheets", "CompetenciesGroupingAnswer_Id", "dbo.CompetenciesGroupingAnswers");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "UserDetailId" });
            DropIndex("dbo.UserProfileExamTemplates", new[] { "ExamTemplateId" });
            DropIndex("dbo.UserProfileExamTemplates", new[] { "UserProfileId" });
            DropIndex("dbo.UserCompanies", new[] { "UserId" });
            DropIndex("dbo.UserCompanies", new[] { "CompanyId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.LicenseExaminations", new[] { "UserId" });
            DropIndex("dbo.EmploymentHistories", new[] { "UserId" });
            DropIndex("dbo.EducationAttainments", new[] { "UserId" });
            DropIndex("dbo.CharacterReferences", new[] { "UserId" });
            DropIndex("dbo.UserProfiles", new[] { "CompanyId" });
            DropIndex("dbo.UserProfiles", new[] { "EmergencyContactId" });
            DropIndex("dbo.UserProfiles", new[] { "DesiredEmploymentId" });
            DropIndex("dbo.JobPostingModels", new[] { "CompanyId" });
            DropIndex("dbo.ExamCategoryTemplates", new[] { "CategoryId" });
            DropIndex("dbo.ExamCategoryTemplates", new[] { "TemplateId" });
            DropIndex("dbo.ExamTemplates", new[] { "CompanyId" });
            DropIndex("dbo.ScheduleTemplates", new[] { "JobPostingId" });
            DropIndex("dbo.ScheduleTemplates", new[] { "ExaminationId" });
            DropIndex("dbo.ScheduleTemplates", new[] { "CompanyId" });
            DropIndex("dbo.ScheduleExaminerModels", new[] { "UserId" });
            DropIndex("dbo.ScheduleExaminerModels", new[] { "ScheduleId" });
            DropIndex("dbo.ScheduleQuestions", new[] { "ScheduleExamId" });
            DropIndex("dbo.ScheduleQuestions", new[] { "QuestionId" });
            DropIndex("dbo.QuestionChoices", new[] { "CategoryQuestion_Id" });
            DropIndex("dbo.QuestionChoices", new[] { "ScheduleQuestionId" });
            DropIndex("dbo.QuestionChoices", new[] { "imageId" });
            DropIndex("dbo.CategoryQuestions", new[] { "imageId" });
            DropIndex("dbo.CategoryQuestions", new[] { "CategoryId" });
            DropIndex("dbo.Categories", new[] { "CompanyId" });
            DropIndex("dbo.PostionAppraisalTypeLookups", new[] { "AppraisalId" });
            DropIndex("dbo.KRAModels", new[] { "AppraisalId" });
            DropIndex("dbo.CompetenciesModels", new[] { "GroupId" });
            DropIndex("dbo.CompetenciesGroupings", new[] { "AppraisalType_Id" });
            DropIndex("dbo.Ta3Employee", new[] { "AppraisalBatchId" });
            DropIndex("dbo.Ta3Employee", new[] { "AppraisalId" });
            DropIndex("dbo.CompetenciesAnswerSheets", new[] { "CompetenciesGroupingAnswer_Id" });
            DropIndex("dbo.CompetenciesGroupingAnswers", new[] { "AppraisalEmployeeAnswerModel_Id" });
            DropIndex("dbo.AppraisalEmployeeAnswerModels", new[] { "EmployeeId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserProfileExamTemplates");
            DropTable("dbo.UserCompanies");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.LicenseExaminations");
            DropTable("dbo.EmploymentHistories");
            DropTable("dbo.EmergencyNotifications");
            DropTable("dbo.EducationAttainments");
            DropTable("dbo.DesiredEmployments");
            DropTable("dbo.CharacterReferences");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.JobPostingModels");
            DropTable("dbo.ExamCategoryTemplates");
            DropTable("dbo.ExamTemplates");
            DropTable("dbo.ScheduleTemplates");
            DropTable("dbo.ScheduleExaminerModels");
            DropTable("dbo.ScheduleQuestions");
            DropTable("dbo.FileRepositoryItems");
            DropTable("dbo.QuestionChoices");
            DropTable("dbo.CategoryQuestions");
            DropTable("dbo.Companies");
            DropTable("dbo.Categories");
            DropTable("dbo.PostionAppraisalTypeLookups");
            DropTable("dbo.KRAModels");
            DropTable("dbo.CompetenciesModels");
            DropTable("dbo.CompetenciesGroupings");
            DropTable("dbo.AppraisalTypes");
            DropTable("dbo.AppraisalBatches");
            DropTable("dbo.Ta3Employee");
            DropTable("dbo.CompetenciesAnswerSheets");
            DropTable("dbo.CompetenciesGroupingAnswers");
            DropTable("dbo.AppraisalEmployeeAnswerModels");
        }
    }
}
