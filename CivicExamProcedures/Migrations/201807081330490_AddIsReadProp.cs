namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsReadProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatDetails", "IsRead", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatDetails", "IsRead");
        }
    }
}
