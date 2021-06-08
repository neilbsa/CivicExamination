namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequireIdNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "CompanyId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "CompanyId", c => c.String());
        }
    }
}
