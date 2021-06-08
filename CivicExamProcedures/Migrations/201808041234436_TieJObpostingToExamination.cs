namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TieJObpostingToExamination : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ScheduleTemplates", "JobPostingId", "dbo.JobPostingModels");
            DropIndex("dbo.ScheduleTemplates", new[] { "JobPostingId" });
            CreateTable(
                "dbo.JobPostingExamTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        JobPostingId = c.Int(nullable: false),
                        ExamTemplateId = c.Int(nullable: false),
                        PassingScore = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExamTemplates", t => t.ExamTemplateId, cascadeDelete: true)
                .ForeignKey("dbo.JobPostingModels", t => t.JobPostingId, cascadeDelete: true)
                .Index(t => t.JobPostingId)
                .Index(t => t.ExamTemplateId);
            
            DropColumn("dbo.ScheduleTemplates", "JobPostingId");
            DropColumn("dbo.ExamTemplates", "PassingScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExamTemplates", "PassingScore", c => c.Int(nullable: false));
            AddColumn("dbo.ScheduleTemplates", "JobPostingId", c => c.Int(nullable: false));
            DropForeignKey("dbo.JobPostingExamTemplates", "JobPostingId", "dbo.JobPostingModels");
            DropForeignKey("dbo.JobPostingExamTemplates", "ExamTemplateId", "dbo.ExamTemplates");
            DropIndex("dbo.JobPostingExamTemplates", new[] { "ExamTemplateId" });
            DropIndex("dbo.JobPostingExamTemplates", new[] { "JobPostingId" });
            DropTable("dbo.JobPostingExamTemplates");
            CreateIndex("dbo.ScheduleTemplates", "JobPostingId");
            AddForeignKey("dbo.ScheduleTemplates", "JobPostingId", "dbo.JobPostingModels", "Id", cascadeDelete: true);
        }
    }
}
