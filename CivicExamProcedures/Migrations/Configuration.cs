namespace CivicExamProcedures.Migrations
{
    using CivicExamProcedures.SeedMethod;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CivicExamProcedures.Context.ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CivicExamProcedures.Context.ApplicationContext context)
        {
            Seed sid = new Seed();
            sid.SeedSystem(context);
        }
    }
}
