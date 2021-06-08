namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateIndexScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KRAModels", "IndexScore", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KRAModels", "IndexScore");
        }
    }
}
