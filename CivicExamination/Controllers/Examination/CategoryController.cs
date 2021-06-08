using CivicExamination.AuthorizationLogics;
using CivicExamination.Models;
using ExaminationEntity.ExaminationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CivicExamination.Controllers.Examination
{
    [AuthorizeRequireBranch(Roles = "ExaminationAdministrator,Administrator")]
    public class CategoryController : CompanyTransactionController<Category>
    {

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create(Category t)
        {
            ModelState.Merge(ValidateCategory(t));

            if (!ModelState.IsValid)
            {
                return PartialView(t);
            }
            base.Create(t);

            //RedirectToRoute("Index");
            return null;
        }

        [HttpPost]
        [AuthorizeRequireBranch()]
        public JsonResult GetCategoryList()
        {
            List<string> listOfCategory = new List<string>();
            List<Category> item = new List<Category>();
            var items = from c in BaseMethods.GetList(x => x.IsDeleted != true && x.CompanyId == SelectedCompany.Id)
                        select new { entityId = c.Id, CategoryName = c.CategoryName, count = c.Questions.Where(x=>x.IsDeleted == false).Count() };

           
            return Json(items.ToList(), JsonRequestBehavior.DenyGet);
        }


        public PartialViewResult IndexItemView(string CategoryName)
        {
            Category item = BaseMethods.GetDetails(x => x.CategoryName == CategoryName && x.CompanyId == SelectedCompany.Id);
            return PartialView(item);
        }

        public override void UpdateExtraProcess(Category oldT, Category newT)
        {
            oldT.CategoryName = newT.CategoryName;
            base.UpdateExtraProcess(oldT, newT);
        }

        //public override ActionResult Update(Category ent)
        //{
        //    base.Update(ent);
        //    return RedirectToAction("Index");
        //}

        private ModelStateDictionary ValidateCategory(Category cat)
        {
            ModelStateDictionary model = new ModelStateDictionary();

            bool isExist = BaseMethods.CheckIfExist(p => p.CategoryName == cat.CategoryName && p.CompanyId == SelectedCompany.Id);
            if (isExist)
            {
                model.AddModelError("CategoryName", "The Category already Exist");
            }
            return model;
        }



    }
}