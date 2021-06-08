namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCommentPerGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompetenciesGroupingAnswers", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompetenciesGroupingAnswers", "Comment");
        }
    }
}
