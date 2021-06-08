using CivicExamProcedures.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CivicExamProcedures.Context;
using ExaminationEntity.BaseModels;
using System.Web.Routing;
using CivicExamination.AuthorizationLogics;
using Microsoft.AspNet.Identity.Owin;
using ExaminationEntity.ExaminationModels;
using Microsoft.AspNet.Identity;

namespace CivicExamination.Controllers
{
    [AuthorizeRequireBranch]
    public abstract class CompanyTransactionController<TCompEntity> : BaseController<TCompEntity> where TCompEntity :class, ICompanyTransaction
    {
        //public DbConnect<TCompEntity> BaseMethods { get; set; }

        public DbConnect<UserCompany> dbUserComp { get; set; }


        public Company SelectedCompany { get; set; }
    
        public override bool HasDetailAccess(TCompEntity t)
        {
            TCompEntity tent;
            tent = t;
            bool allows = AssignedCompanies().Any(x=>x.Id == t.CompanyDetails.Id);
           // bool res = allows != null ? true : false;

            return allows;
        }

        public CompanyTransactionController()
        {
            SelectedCompany = new Company();
            dbUserComp = new DbConnect<UserCompany>();
           SelectedCompany = System.Web.HttpContext.Current.Session["COMPANY"] as Company;
        }


        public override ActionResult Index()
        {
            List<TCompEntity> compList = new List<TCompEntity>();
            compList = BaseMethods.GetList(x => x.CompanyDetails.Id == SelectedCompany.Id).ToList();
            return View(compList);
        }

        protected List<Company> AssignedCompanies()
        {
            var user = UserManager.FindByName(User.Identity.Name);
            List<Company> AllowedCompanies = new List<Company>();
            if (user != null)
            {
                AllowedCompanies = dbUserComp.GetList(x => x.UserDetail.Id == user.UserDetailId).Select(x => x.CompanyDetails).ToList();
            }
            return AllowedCompanies;
        }

        public override ActionResult Create(TCompEntity t)
        {
            t.CompanyId = SelectedCompany.Id;
            return base.Create(t);
        }

    }
}