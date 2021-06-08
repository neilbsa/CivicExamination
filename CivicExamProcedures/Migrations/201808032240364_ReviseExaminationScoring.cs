namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviseExaminationScoring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamTemplates", "PassingScore", c => c.Int(nullable: false));
            DropColumn("dbo.JobPostingModels", "PassingScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobPostingModels", "PassingScore", c => c.Int(nullable: false));
            DropColumn("dbo.ExamTemplates", "PassingScore");
        }
    }
}
