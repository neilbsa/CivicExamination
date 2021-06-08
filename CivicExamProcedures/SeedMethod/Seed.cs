using CivicExamProcedures.Context;
using ExaminationEntity.ExaminationModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicExamProcedures.SeedMethod
{
    public class Seed
    {
        public void SeedSystem(CivicExamProcedures.Context.ApplicationContext context)
        {



            context.Companies.AddOrUpdate(p => p.Name,
                 new Company() { Name = "Civic Merchandising Inc.", Address = "77 Mindanao Ave. Bagong Pagasa", IsDeleted = false },
                 new Company() { Name = "PrimeQuest Transport Solutions Inc.", Address = "99 Mindanao Ave. brgy Tandang Sora", IsDeleted = false },
                 new Company() { Name = "Top Spot Heavy Equipt", Address = "77 Mindanao Ave. Bagong Pagasa", IsDeleted = false },
                 new Company() { Name = "CMI Equiptment", Address = "77 Mindanao Ave. Bagong Pagasa", IsDeleted = false }
                );


            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(context);
            var UserManager = new UserManager<ApplicationUser>(userStore);

            var role = roleManager.FindByName("ExaminationAdministrator");
            if (role == null)
            {
                role = new IdentityRole("ExaminationAdministrator");
                roleManager.Create(role);
            }

            var role2 = roleManager.FindByName("ExamineeAdministrator");
            if (role2 == null)
            {
                role2 = new IdentityRole("ExamineeAdministrator");
                roleManager.Create(role2);
            }


            var role4 = roleManager.FindByName("Administrator");
            if (role4 == null)
            {
                role4 = new IdentityRole("Administrator");
                roleManager.Create(role4);
            }


            var role3 = roleManager.FindByName("Common");
            if (role3 == null)
            {
                role3 = new IdentityRole("Common");
                roleManager.Create(role3);
            }



            var erole2 = roleManager.FindByName("EvaluationAdmin");
            if (erole2 == null)
            {
                role2 = new IdentityRole("EvaluationAdmin");
                roleManager.Create(role2);
            }


            var erole3 = roleManager.FindByName("EvaluatorUser");
            if (erole3 == null)
            {
                role2 = new IdentityRole("EvaluatorUser");
                roleManager.Create(role2);
            }


            context.SaveChanges();


            ApplicationUser user1 = new ApplicationUser()
            {
                UserName = "Administrator",
                Email = "nbsa@civicmdsg.com.ph",
                CompanyId = "1662",
                UserDetail = new UserProfile()
                {
                    Firstname = "Neil",
                    Middlename = "salarzon",
                    Lastname = "Abar2x",
                    CompanyId = 1,
                    EmailAddress = "nbsa@civicmdsg.com.ph",
                    PresentAddress = "myAddress",
                    PermanentAddress = "myAddress",
                    MobileContact = "Contact",
                    HomeContact = "Contact",
                    Gender = "Male",

                    BirthDate = new DateTime(1992, 04, 11),
                    IsAdministrator = true
                }
            };


            context.UserCompanies.Add(new ExaminationEntity.ExaminationModels.UserCompany() { UserId = user1.UserDetail.Id, CompanyId = 1 });
            context.UserCompanies.Add(new ExaminationEntity.ExaminationModels.UserCompany() { UserId = user1.UserDetail.Id, CompanyId = 2 });
            context.UserCompanies.Add(new ExaminationEntity.ExaminationModels.UserCompany() { UserId = user1.UserDetail.Id, CompanyId = 3 });
            context.UserCompanies.Add(new ExaminationEntity.ExaminationModels.UserCompany() { UserId = user1.UserDetail.Id, CompanyId = 4 });


            var result = UserManager.Create(user1, "defaultpasswordhere");
            var user = UserManager.FindByName("Administrator");

            UserManager.AddToRole(user.Id, "Administrator");
            UserManager.AddToRole(user.Id, "ExamineeAdministrator");
            UserManager.AddToRole(user.Id, "ExaminationAdministrator");
            UserManager.AddToRole(user.Id, "EvaluatorUser");
            UserManager.AddToRole(user.Id, "EvaluationAdmin");



            context.AppraisalTypesTbl.AddOrUpdate(p => p.Description,
                new AppraisalType()
                {
                    Type = "Officer",
                    KRAPercentage = 60,
                    Description = "Officer",
                    Competencies = new System.Collections.Generic.List<CompetenciesGrouping>() {
                        new CompetenciesGrouping()
                        {
                            GroupType ="Competencies",
                            Percentage = 20,
                            Description = "Competencies are defined as the ability, skill, knowledge, and motivation needed for success on the job. All relevant competencies (suggested minimum of five) should be evaluated.",
                              RatingScale = new List<RatingScale>(){
                                new RatingScale(){ Description="EXCELLENT (Consistently Exceeds Standards)", ScoreDetails="5.00" },
                                new RatingScale(){ Description="OUTSTANDING (Frequently  Exceeds Standards)", ScoreDetails="4.00 – 4.99 " },
                                new RatingScale(){ Description="SATISFACTORY (Generally Meets Standards)", ScoreDetails="3.00 – 3.99  " },
                                new RatingScale(){ Description="NEEDS IMPROVEMENT (Frequently Fails to Meet Standards)", ScoreDetails="2.00 – 2.99" },
                                new RatingScale(){ Description="UNACCEPTABLE (Fails to Meet Standards)", ScoreDetails="1.00 – 1.99" },

                             },

                            CompetenciesMember = new System.Collections.Generic.List<CompetenciesModels>()
                            {
                                new CompetenciesModels() { Title="ADAPTABILITY", Description="Maintaining effectiveness when experiencing major changes in work tasks or the work environment; adjusting effectively to work within new work structures, processes, requirements, or cultures." },
                                new CompetenciesModels() { Title="COMMUNICATION", Description="Clearly conveying and receiving information and ideas through a variety of media to individuals or groups in a manner that engages the audience, helps them understand and retain the message, and permits response and feedback from the audience." },
                                new CompetenciesModels() { Title="CUSTOMER FOCUS", Description="Makes customers and their needs a primary focus of one’s actions; developing and sustaining productive customer relationships." },
                                new CompetenciesModels() { Title="DECISION MAKING", Description="Identifying and understanding issues, problems, and opportunities; comparing data from different sources to draw conclusions; using effective approaches for choosing a course of action or developing appropriate solutions; taking action that is consistent with available facts, constraints, and probable consequences." },
                                new CompetenciesModels() { Title="INNOVATION", Description="Generates innovative solutions; tries different and novel ways to deal with work problems and opportunities." },
                                new CompetenciesModels() { Title="TECHNICAL / PROFESSIONAL KNOWLEDGE AND SKILLS", Description="Possesses, acquires, and maintains the technical/professional expertise required to do the job effectively and to create customer solutions. Technical/professional expertise is demonstrated through problem solving, applying technical knowledge, and product and service management for the functional area in which one operates." },
                                new CompetenciesModels() { Title="VALUING DIVERSITY", Description="Actively appreciating and including the diverse capabilities, insights, and ideas of others and working effectively and respectfully with individuals of diverse backgrounds, styles, abilities, and motivations." },
                                new CompetenciesModels() { Title="DELEGATING RESPONSIBILITY", Description="Allocates decision-making authority and/or task responsibility to appropriate others to maximize the organization’s and individual’s effectiveness." },
                                new CompetenciesModels() { Title="PLANNING AND ORGANIZING WORK", Description="Establishes courses of action for self and others to ensure that work is completed efficiently." },
                                new CompetenciesModels() { Title="ALIGNING PERFORMANCE FOR SUCCESS", Description="Focuses and guides others in accomplishing work objectives." },
                                new CompetenciesModels() { Title="BUILDING TRUST", Description="Interacts with others in a way that gives them confidence in one’s intentions and those of the organization." },
                                new CompetenciesModels() { Title="DEVELOPING A SUCCESSFUL TEAM", Description="Uses appropriate methods and a flexible, interpersonal style to help develop a cohesive team; facilitating the completion of team goals." },
                                new CompetenciesModels() { Title="MANAGING CONFLICT", Description="Deals effectively with others in antagonistic situations, using appropriate interpersonal styles and methods to reduce tension or conflict between two or more people." },
                                new CompetenciesModels() { Title="ATTITUDE", Description="The demeanor used in dealings with customers and co-workers." },
                                new CompetenciesModels() { Title="BUILDING PARTNERSHIPS", Description="Identifies opportunities and takes action to build strategic relationships between one’s area and other areas, teams, departments, units, or organizations to help achieve business goals." },
                                new CompetenciesModels() { Title="FACILITATING CHANGE", Description="Encourages others to seek opportunities for different and innovative approaches to addressing problems and opportunities, facilitating the implementation and acceptance of change within the workplace." },

                            }
                        },
                        new CompetenciesGrouping()
                        {
                            GroupType ="Values",
                                Percentage = 20,
                            Description="The Company's Corporate Values are the operating philosophies and principles that guide the Company's internal conduct as well as its relationship with its customers, partners, and shareholders Manifestation of these values is evaluated.",
                            RatingScale = new List<RatingScale>(){
                                new RatingScale(){ Description="Keenly shows Company Values and Influences others", ScoreDetails="5.00" },
                                new RatingScale(){ Description="Lives the Company Values", ScoreDetails="3.00" },
                                new RatingScale(){ Description="Inconsistent display of Company Values", ScoreDetails="1.00" },

                            },

                            CompetenciesMember = new System.Collections.Generic.List<CompetenciesModels>()
                            {
                                new CompetenciesModels() { Title="CONCERN FOR THE COMPANY", Description="The ability of an individual of instilling the company's welfare in his heart." },
                                new CompetenciesModels() { Title="INTEGRITY AND HONESTY", Description="Integrity is the quality of being honest and having strong moral principles; moral uprightness. It is generally a personal choice to uphold oneself to consistently moral and ethical standards" },
                                new CompetenciesModels() { Title="CONTRIBUTING TO TEAM SUCCESS", Description="An individual’s active participation as a member of the team to move the team toward the achievement of organizational goals." },
                                new CompetenciesModels() { Title="WILLINGNESS TO SHARE ", Description="An individual’s eagerness and strong internal drive to communicate his intellectual assets (time, knowledge, skills, talents) to others." }

                            }
                        }
                    }
                },
                  new AppraisalType()
                  {
                      Type = "Staff",
                      Description = "Staff",
                      KRAPercentage = 60,
                      Competencies = new System.Collections.Generic.List<CompetenciesGrouping>() {
                        new CompetenciesGrouping()
                        {
                            GroupType ="Competencies",
                            Percentage = 20,
                             Description = "Competencies are defined as the ability, skill, knowledge, and motivation needed for success on the job. All relevant competencies (suggested minimum of five) should be evaluated.",
                               RatingScale = new List<RatingScale>(){
                                new RatingScale(){ Description="EXCELLENT (Consistently Exceeds Standards)", ScoreDetails="5.00" },
                                new RatingScale(){ Description="OUTSTANDING (Frequently  Exceeds Standards)", ScoreDetails="4.00 – 4.99 " },
                                new RatingScale(){ Description="SATISFACTORY (Generally Meets Standards)", ScoreDetails="3.00 – 3.99  " },
                                new RatingScale(){ Description="NEEDS IMPROVEMENT (Frequently Fails to Meet Standards)", ScoreDetails="2.00 – 2.99" },
                                new RatingScale(){ Description="UNACCEPTABLE (Fails to Meet Standards)", ScoreDetails="1.00 – 1.99" },

                             },
                            CompetenciesMember = new System.Collections.Generic.List<CompetenciesModels>()
                            {
                                new CompetenciesModels() { Title="ADAPTABILITY", Description="Maintaining effectiveness when experiencing major changes in work tasks or the work environment; adjusting effectively to work within new work structures, processes, requirements, or cultures." },
                                new CompetenciesModels() { Title="APPLIED LEARNING", Description="Assimilating and applying new job-related information in a timely manner." },
                                new CompetenciesModels() { Title="BUILDING CUSTOMER LOYALTY", Description="Effectively meeting customer needs; building productive customer relationships; taking responsibility for customer satisfaction and loyalty." },
                                new CompetenciesModels() { Title="COMMUNICATION", Description="Clearly conveying and receiving information and ideas through a variety of media to individuals or groups in a manner that engages the audience, helps them understand and retain the message, and permits response and feedback from the audience." },
                                new CompetenciesModels() { Title="DECISION MAKING", Description="Identifying and understanding issues, problems, and opportunities; comparing data from different sources to draw conclusions; using effective approaches for choosing a course of action or developing appropriate solutions; taking action that is consistent with available facts, constraints, and probable consequences." },
                                new CompetenciesModels() { Title="IMPACT", Description="Creating a good first impression, commanding attention and respect, showing an air of confidence." },
                                new CompetenciesModels() { Title="INITIATING ACTION",Description="Taking prompt action to accomplish objectives; taking action to achieve goals beyond what is required; being proactive."  },
                                new CompetenciesModels() { Title="INNOVATION",  Description="Generating innovative solutions in work situations; trying different and novel ways to deal with work problems and opportunities."},
                                new CompetenciesModels() { Title="INTERPERSONAL SKILLS", Description="Considering and responding appropriately to the needs, feelings, and capabilities of others; adjusting approaches to suit different people and situations; and representing the agency to the public and other agencies in a courteous and pleasant manner." },
                                new CompetenciesModels() { Title="JOB KNOWLEDGE", Description="Understanding, absorbing, retaining, and correctly applying information, instructions, and procedures to complete the job assignments effectively and being skilled in the use and maintenance of the equipment related to the work." },
                                new CompetenciesModels() { Title="MANAGING WORK", Description="Effectively managing one’s time and resources to ensure that work is completed efficiently; makes timely requests for sick/annual leave time; utilizes sick leave appropriately; reporting for work and returning from breaks and lunch in a timely manner." },
                                new CompetenciesModels() { Title="QUALITY ORIENTATION", Description="Accomplishing tasks by considering all areas involved, no matter how small; showing concern for all aspects of the job; accurately checking processes and tasks; being watchful over a period of time." },
                                new CompetenciesModels() { Title="SAFETY AWARENESS", Description="Being aware of conditions that affect employee safety.Being aware of conditions that affect employee safety." },
                                new CompetenciesModels() { Title="STRESS TOLERANCE", Description="Maintaining stable performance under pressure or opposition (such as time pressure, or job ambiguity); handling stress in a manner that is acceptable to others and to the organization." },
                                new CompetenciesModels() { Title="VALUING DIVERSITY AND INCLUSION", Description="Actively appreciating and including the diverse capabilities, insights, and ideas of others and working effectively and respectfully with individuals of diverse backgrounds, styles, abilities, and motivations others and to the organization." },
                            }
                        },
                        new CompetenciesGrouping()
                        {
                            GroupType ="Values",
                            Percentage = 20,
                            Description="The Company's Corporate Values are the operating philosophies and principles that guide the Company's internal conduct as well as its relationship with its customers, partners, and shareholders Manifestation of these values is evaluated.",
                              RatingScale = new List<RatingScale>(){
                                new RatingScale(){ Description="Keenly shows Company Values and Influences others", ScoreDetails="5.00" },
                                new RatingScale(){ Description="Lives the Company Values", ScoreDetails="3.00" },
                                new RatingScale(){ Description="Inconsistent display of Company Values", ScoreDetails="1.00" },

                            },
                            CompetenciesMember = new System.Collections.Generic.List<CompetenciesModels>()
                            {
                                new CompetenciesModels() { Title="CONCERN FOR THE COMPANY", Description="The ability of an individual of instilling the company's welfare in his heart." },
                                new CompetenciesModels() { Title="INTEGRITY AND HONESTY", Description="Integrity is the quality of being honest and having strong moral principles; moral uprightness. It is generally a personal choice to uphold oneself to consistently moral and ethical standards" },
                                new CompetenciesModels() { Title="CONTRIBUTING TO TEAM SUCCESS", Description="An individual’s active participation as a member of the team to move the team toward the achievement of organizational goals." },
                                new CompetenciesModels() { Title="WILLINGNESS TO SHARE ", Description="An individual’s eagerness and strong internal drive to communicate his intellectual assets (time, knowledge, skills, talents) to others." }

                            }
                        }
                    }
                  }
                );


            context.AppraisalLookup.AddOrUpdate(p => p.PositionType,


                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "A1-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "A1-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "A2-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "A2-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "A3-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "A3-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "A4-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "A4-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "A5-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "A5-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "B1-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "B1-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "B2-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "B2-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "B3-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "B3-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "B4-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "B4-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "B5-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "B5-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "C1-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "C1-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "C2-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "C2-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "C3-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "C3-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "D1-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "D1-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "D2-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "D2-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "D3-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "D3-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "E1-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "E1-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "E2-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "E2-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "E3-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "E3-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "E4-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "E4-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F1-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F1-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F1-3" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F2-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F2-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F2-3" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F3-1" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F3-2" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F3-3" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F3-4" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F3-5" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F3-6" },
                new PostionAppraisalTypeLookup() { AppraisalId = 1, PositionType = "F3-7" },
                new PostionAppraisalTypeLookup() { AppraisalId = 2, PositionType = "PROBI" }
                );


            context.SaveChanges();

        }
    }
}
