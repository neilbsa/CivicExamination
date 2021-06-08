namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRatingScale : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnswerRatingScales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Description = c.String(),
                        ScoreDetails = c.String(),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompetenciesGroupingAnswers", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.RatingScales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Description = c.String(),
                        ScoreDetails = c.String(),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompetenciesGroupings", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            AddColumn("dbo.CompetenciesGroupingAnswers", "Description", c => c.String());
            AddColumn("dbo.CompetenciesGroupings", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RatingScales", "GroupId", "dbo.CompetenciesGroupings");
            DropForeignKey("dbo.AnswerRatingScales", "GroupId", "dbo.CompetenciesGroupingAnswers");
            DropIndex("dbo.RatingScales", new[] { "GroupId" });
            DropIndex("dbo.AnswerRatingScales", new[] { "GroupId" });
            DropColumn("dbo.CompetenciesGroupings", "Description");
            DropColumn("dbo.CompetenciesGroupingAnswers", "Description");
            DropTable("dbo.RatingScales");
            DropTable("dbo.AnswerRatingScales");
        }
    }
}
