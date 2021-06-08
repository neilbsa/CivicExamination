namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDeptDateHired : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ta3Employee", "Dept", c => c.String());
            AddColumn("dbo.Ta3Employee", "DateHired", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ta3Employee", "DateHired");
            DropColumn("dbo.Ta3Employee", "Dept");
        }
    }
}
