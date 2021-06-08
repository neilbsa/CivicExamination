namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDatachangeTrackerTa3Employee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ta3Employee", "AcceptanceDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Ta3Employee", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Ta3Employee", "LastDateUpdated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Ta3Employee", "CreateUser", c => c.String());
            AddColumn("dbo.Ta3Employee", "LastUpdateUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ta3Employee", "LastUpdateUser");
            DropColumn("dbo.Ta3Employee", "CreateUser");
            DropColumn("dbo.Ta3Employee", "LastDateUpdated");
            DropColumn("dbo.Ta3Employee", "DateCreated");
            DropColumn("dbo.Ta3Employee", "AcceptanceDate");
        }
    }
}
