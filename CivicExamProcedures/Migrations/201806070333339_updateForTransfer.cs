namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateForTransfer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppraisalEmployeeAnswerModels", "ForTransfer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppraisalEmployeeAnswerModels", "ForTransfer", c => c.String());
        }
    }
}
