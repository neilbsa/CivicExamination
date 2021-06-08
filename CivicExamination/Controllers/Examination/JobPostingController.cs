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
    [AuthorizeRequireBranch(Roles = "ExamineeAdministrator,ExaminationAdministrator,Administrator")]
    public class JobPostingController : CompanyTransactionController<JobPostingModel>
    {


        public ActionResult JobpostingExaminationItem()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create(JobPostingModel t)
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
                return Json(new { success = false, errors = errors }, JsonRequestBehavior.DenyGet);
            }
            t.Status = "Open";
            base.Create(t);
           
            return Json(new { success = true, id=t.Id }, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        [AuthorizeRequireBranch()]
        public JsonResult jsonGetJobPostingList()
        {
            var items = from item in BaseMethods.GetList(x => x.IsDeleted != true && x.CompanyId == SelectedCompany.Id)
                        select new { jobId = item.Id, jobName = item.JobName };
            return Json(items, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Update(JobPostingModel ent)
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
                return Json(new { success = false, errors = errors }, JsonRequestBehavior.DenyGet);
            }
            base.Update(ent);
            return Json(new { success = true }, JsonRequestBehavior.DenyGet);
        }
        public ActionResult JobPostingList()
        {
            List<JobPostingModel> mod = new List<JobPostingModel>();
            mod = BaseMethods.GetList(x => x.IsDeleted != true && x.CompanyId == SelectedCompany.Id).ToList();
            return View(mod);
        }
        public override void UpdateExtraProcess(JobPostingModel oldT, JobPostingModel newT)
        {
            oldT.JobName = newT.JobName;
            oldT.JobDescription = newT.JobDescription;
            //oldT.PassingScore = newT.PassingScore;
            oldT.RequestedBy = newT.RequestedBy;
            oldT.Remarks = newT.Remarks;
            base.UpdateExtraProcess(oldT, newT);
        }
        [HttpPost]
        public ActionResult IndexViewItem(int Id)
        {
            JobPostingModel mod = new JobPostingModel();
            mod = BaseMethods.GetDetails(x => x.Id == Id);
            return View(mod);
        }
        private ModelStateDictionary ValidateViewModel(JobPostingModel model)
        {

            ModelStateDictionary modelState = new ModelStateDictionary();


            return modelState;

        }
        public ActionResult RenderExaminerReport(int PostingId)
        {
            JobPostingSearchViewModel model = new JobPostingSearchViewModel();
            var posting = BaseMethods.GetDetails(x => x.Id == PostingId);
            if(posting != null)
            {
                ViewBag.PostingName = posting.JobName;
                model.DateFrom = DateTime.Now;
                model.DateTo = DateTime.Now;
                model.JobPostingId = PostingId;
                return View(model);
            }
            else
            {
                return SystemMessage("Error Accessing Job Posting Details");
            }
        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenderExaminerReport(JobPostingSearchViewModel model)
        {
            //var ScheduledExaminer = from jobPosting in TemplateSchedule.GetList(x => x.JobPostingId == model.JobPostingId)
            //                join examinerSched in SchedExaminer.GetList() on jobPosting.Id equals examinerSched.ScheduleId
            //                select examinerSched.User;






            return View();
        }


    }
}