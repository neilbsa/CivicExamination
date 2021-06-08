namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ConnectionStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ConnectionStatus");
        }
    }
}
