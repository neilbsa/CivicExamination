namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AcceptanceDateNulableandEmailAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ta3Employee", "EmailAddress", c => c.String());
            AlterColumn("dbo.Ta3Employee", "AcceptanceDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ta3Employee", "AcceptanceDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Ta3Employee", "EmailAddress");
        }
    }
}
