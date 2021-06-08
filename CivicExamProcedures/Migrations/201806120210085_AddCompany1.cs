namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompany1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ta3Employee", "CompanyName", c => c.String());
            DropColumn("dbo.Ta3Employee", "Company");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ta3Employee", "Company", c => c.String());
            DropColumn("dbo.Ta3Employee", "CompanyName");
        }
    }
}
