namespace CivicExamProcedures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateChatFet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        MessageFrom = c.String(),
                        MessageTo = c.String(),
                        Message = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        LastDateUpdated = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        LastUpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChatDetails");
        }
    }
}
