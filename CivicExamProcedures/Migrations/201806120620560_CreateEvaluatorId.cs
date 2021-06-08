namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateEvaluatorId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ta3Employee", "EvaluatorEmployeeId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ta3Employee", "EvaluatorEmployeeId");
        }
    }
}
