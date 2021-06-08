using CivicExamination.AuthorizationLogics;
using ExaminationEntity.ExaminationModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CivicExamination.Controllers.Examination
{
    [AuthorizeRequireBranch(Roles = "ExaminationAdministrator,Administrator")]
    public class ExaminationController : CompanyTransactionController<ExamTemplate>
    {

        public override ActionResult Index()
        {

            var items = BaseMethods.GetList(x => x.Status != "Obsolete" && x.CompanyId==SelectedCompany.Id);
            return View(items);
        }

        [HttpPost]
        [AuthorizeRequireBranch()]
        public JsonResult GetExaminationList() {
            var exams = from c in BaseMethods.GetList(x => x.IsDeleted != true && x.CompanyId == SelectedCompany.Id)
                        select new { entityId = c.Id, ExaminationName = c.ExamTemplateName };
            return Json(exams.ToList(), JsonRequestBehavior.DenyGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create(ExamTemplate t)
        {
            ModelState.Merge(ValidateViewModel(t));
            if (!ModelState.IsValid)
            {
                List<string> errors = new List<string>();

                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                ViewBag.error = errors;
                return Json(new { success = false, errors = errors }, JsonRequestBehavior.AllowGet);
            }
            t.AvatarText = GetAvatarChar(t.ExamTemplateName);
            t.LimitSeconds = t.LimitSeconds;
            base.Create(t);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        private string GetAvatarChar(string str)
        {
            char[] newStr = str.ToCharArray();
            newStr[0] = char.ToUpper(newStr[0]);
            Regex regex = new Regex("[^A-Z]");
            string output = regex.Replace(new string(newStr), "");
            return String.Format("{0}{1}", output[0], output.Length > 1 ? output[1].ToString(): "");
        }


        [HttpPost]
        [AuthorizeRequireBranch()]
        public JsonResult jsonGetExamList()
        {
            var items = from item in BaseMethods.GetList(x => x.IsDeleted != true && x.CompanyId == SelectedCompany.Id && x.Status != "Obsolete")
                        select new { examId = item.Id, examName = item.ExamTemplateName };
            return Json(items, JsonRequestBehavior.DenyGet);
        }


        public override void UpdateExtraProcess(ExamTemplate oldT, ExamTemplate newT)
        {
            ExamTemplate newTemplate = new ExamTemplate();
            List<ExamCategoryTemplate> build = new List<ExamCategoryTemplate>();
            newTemplate.ExamTemplateName = newT.ExamTemplateName;
            newTemplate.TemplateDescription = newT.TemplateDescription;
            foreach (var item in newT.TemplateBuild.Where(x => x.CategoryId != 0).ToList())
            {
                build.Add(item);
            }
            newTemplate.TemplateBuild = build;
            newTemplate.AvatarText = GetAvatarChar(oldT.ExamTemplateName);
            newTemplate.LimitSeconds = newT.LimitSeconds;
            oldT.Status = "Obsolete";
            base.UpdateExtraProcess(oldT, newT);
            base.Create(newTemplate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Update(ExamTemplate ent)
        {
            ModelState.Merge(ValidateViewModel(ent));
            if (!ModelState.IsValid)
            {
                List<string> errors = new List<string>();

                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                ViewBag.error = errors;
                return Json(new { success = false, errors = errors }, JsonRequestBehavior.AllowGet);
            }
            base.Update(ent);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
       
        }

        public JsonResult getCurrentId(string examName)
        {
            var item = BaseMethods.GetDetails(x => examName.Equals(x.ExamTemplateName) && x.Status != "Obsolete");
            return Json(new { newId = item.Id }, JsonRequestBehavior.AllowGet);
        }

        private ModelStateDictionary ValidateViewModel(ExamTemplate model)
        {
            ModelStateDictionary modelState = new ModelStateDictionary();
            ExamTemplate modelDb = new ExamTemplate();
            modelDb = BaseMethods.GetDetails(x => x.Id == model.Id);
            DateTime Datenow = DateTime.Today;
            if (String.IsNullOrEmpty(model.ExamTemplateName) || String.IsNullOrWhiteSpace(model.ExamTemplateName))
            {
                modelState.AddModelError(String.Empty, "Template name is Required");
            }

            bool isExist = false;
            isExist = BaseMethods.CheckIfExist(x => x.ExamTemplateName == model.ExamTemplateName && x.CompanyId == SelectedCompany.Id && x.IsDeleted == false);
            if(isExist && (object.ReferenceEquals(modelDb,null) || modelDb.ExamTemplateName != model.ExamTemplateName))
            {
                modelState.AddModelError(String.Empty, "Template name is already Exist");
            }

           




           var userScheduledWithTemplate = from template in TemplateSchedule.GetList((x => (x.ExaminationId== model.Id && x.IsDeleted == false) && (DbFunctions.TruncateTime(x.StartDate) >= DbFunctions.TruncateTime(Datenow))))
                                             join scheduleExaminerModel in SchedExaminer.GetList(x=> x.IsDeleted ==false && x.Status != "Finish") on template.Id equals scheduleExaminerModel.ScheduleId
                                             select new { scheduleExaminerModel.User,template.StartDate,template.ExpirationDate };
            if (userScheduledWithTemplate.Count() > 0)
            {
                modelState.AddModelError(String.Empty, "This template is currently on used, delete users that is not finish this template");
            }
            if (model.LimitSeconds <= 0)
            {
                modelState.AddModelError(String.Empty, "Time limit is not valid");
            }
            if (model.TemplateBuild == null || model.TemplateBuild.Count() <= 0)
            {
                
                modelState.AddModelError(String.Empty, "Please select atleast 1 body is Required");
            }


            List<string> strExistingBuild = new List<string>();

            foreach(var item in model.TemplateBuild)
            {
                if (!strExistingBuild.Contains(item.CategoryDetail.CategoryName))
                {
                    strExistingBuild.Add(item.CategoryDetail.CategoryName);
                }
                else
                {
                    modelState.AddModelError(String.Empty,String.Format("Category named : {0} is duplicated", item.CategoryDetail.CategoryName));
                }
            }



            int totalOverQuote = 0;
            int invalidCAtegory = 0;
            if (!object.ReferenceEquals(model.TemplateBuild,null))
            {
                foreach (var item in model.TemplateBuild.ToList())
                {
                    if (object.ReferenceEquals(item.CategoryDetail, null) || item.CategoryId == 0)
                    {
                        invalidCAtegory = invalidCAtegory + 1;
                    }
                    else
                    {
                        Category noitems = new Category();
                        noitems = CategoriesDb.GetList(x => x.Id == item.CategoryId).FirstOrDefault();
                        if (item.Items > noitems.Questions.Count())
                        {
                            totalOverQuote = totalOverQuote + 1;
                        }
                        item.CategoryDetail = null;
                    }
                }
            }

            if(totalOverQuote != 0)
            {
                modelState.AddModelError(String.Empty, String.Format("You have {0} over quote category items",totalOverQuote));
            }

            if (invalidCAtegory != 0)
            {
                modelState.AddModelError(String.Empty, String.Format("You have {0} invalid Category", invalidCAtegory));
            }

            return modelState;
        }
   }
}