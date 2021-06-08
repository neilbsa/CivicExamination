namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddKRaPErcentageGroupPercentage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RecomendedTrainings", "EmployeeId", "dbo.Ta3Employee");
            DropIndex("dbo.RecomendedTrainings", new[] { "EmployeeId" });
            AddColumn("dbo.CompetenciesGroupingAnswers", "Percentage", c => c.Int(nullable: false));
            AddColumn("dbo.AppraisalEmployeeAnswerModels", "Comment", c => c.String());
            AddColumn("dbo.AppraisalEmployeeAnswerModels", "PositionRetained", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppraisalEmployeeAnswerModels", "MeritIncrease", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppraisalEmployeeAnswerModels", "ForPomotion", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppraisalEmployeeAnswerModels", "ForTransfer", c => c.String());
            AddColumn("dbo.AppraisalEmployeeAnswerModels", "Reasons", c => c.String());
            AddColumn("dbo.AppraisalEmployeeAnswerModels", "KRAPercentage", c => c.Int(nullable: false));
            AddColumn("dbo.AppraisalTypes", "KRAPercentage", c => c.Int(nullable: false));
            AddColumn("dbo.CompetenciesGroupings", "Percentage", c => c.Int(nullable: false));
            AddColumn("dbo.RecomendedTrainings", "AppraisalEmployeeAnswerModel_Id", c => c.Int());
            CreateIndex("dbo.RecomendedTrainings", "AppraisalEmployeeAnswerModel_Id");
            AddForeignKey("dbo.RecomendedTrainings", "AppraisalEmployeeAnswerModel_Id", "dbo.AppraisalEmployeeAnswerModels", "Id");
            DropColumn("dbo.Ta3Employee", "Comment");
            DropColumn("dbo.Ta3Employee", "PositionRetained");
            DropColumn("dbo.Ta3Employee", "MeritIncrease");
            DropColumn("dbo.Ta3Employee", "ForPomotion");
            DropColumn("dbo.Ta3Employee", "ForTransfer");
            DropColumn("dbo.Ta3Employee", "Reasons");
            DropColumn("dbo.RecomendedTrainings", "MyProperty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RecomendedTrainings", "MyProperty", c => c.Int(nullable: false));
            AddColumn("dbo.Ta3Employee", "Reasons", c => c.String());
            AddColumn("dbo.Ta3Employee", "ForTransfer", c => c.String());
            AddColumn("dbo.Ta3Employee", "ForPomotion", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ta3Employee", "MeritIncrease", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ta3Employee", "PositionRetained", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ta3Employee", "Comment", c => c.String());
            DropForeignKey("dbo.RecomendedTrainings", "AppraisalEmployeeAnswerModel_Id", "dbo.AppraisalEmployeeAnswerModels");
            DropIndex("dbo.RecomendedTrainings", new[] { "AppraisalEmployeeAnswerModel_Id" });
            DropColumn("dbo.RecomendedTrainings", "AppraisalEmployeeAnswerModel_Id");
            DropColumn("dbo.CompetenciesGroupings", "Percentage");
            DropColumn("dbo.AppraisalTypes", "KRAPercentage");
            DropColumn("dbo.AppraisalEmployeeAnswerModels", "KRAPercentage");
            DropColumn("dbo.AppraisalEmployeeAnswerModels", "Reasons");
            DropColumn("dbo.AppraisalEmployeeAnswerModels", "ForTransfer");
            DropColumn("dbo.AppraisalEmployeeAnswerModels", "ForPomotion");
            DropColumn("dbo.AppraisalEmployeeAnswerModels", "MeritIncrease");
            DropColumn("dbo.AppraisalEmployeeAnswerModels", "PositionRetained");
            DropColumn("dbo.AppraisalEmployeeAnswerModels", "Comment");
            DropColumn("dbo.CompetenciesGroupingAnswers", "Percentage");
            CreateIndex("dbo.RecomendedTrainings", "EmployeeId");
            AddForeignKey("dbo.RecomendedTrainings", "EmployeeId", "dbo.Ta3Employee", "Id", cascadeDelete: true);
        }
    }
}
