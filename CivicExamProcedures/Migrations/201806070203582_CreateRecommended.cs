namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRecommended : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecomendedTrainings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        MyProperty = c.Int(nullable: false),
                        TrainingDescription = c.String(),
                        Timetable = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ta3Employee", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            AddColumn("dbo.Ta3Employee", "Comment", c => c.String());
            AddColumn("dbo.Ta3Employee", "PositionRetained", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ta3Employee", "MeritIncrease", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ta3Employee", "ForPomotion", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ta3Employee", "ForTransfer", c => c.String());
            AddColumn("dbo.Ta3Employee", "Reasons", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecomendedTrainings", "EmployeeId", "dbo.Ta3Employee");
            DropIndex("dbo.RecomendedTrainings", new[] { "EmployeeId" });
            DropColumn("dbo.Ta3Employee", "Reasons");
            DropColumn("dbo.Ta3Employee", "ForTransfer");
            DropColumn("dbo.Ta3Employee", "ForPomotion");
            DropColumn("dbo.Ta3Employee", "MeritIncrease");
            DropColumn("dbo.Ta3Employee", "PositionRetained");
            DropColumn("dbo.Ta3Employee", "Comment");
            DropTable("dbo.RecomendedTrainings");
        }
    }
}
