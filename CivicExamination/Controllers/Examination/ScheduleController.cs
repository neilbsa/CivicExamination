using CivicExamination.AuthorizationLogics;
using CivicExamination.Models;
using ExaminationEntity.ExaminationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Rotativa;
//using Rotativa.Options;
using Microsoft.Ajax.Utilities;
using Rotativa;
using Rotativa.Options;

namespace CivicExamination.Controllers.Examination
{
    [AuthorizeRequireBranch(Roles = "ExamineeAdministrator,ExaminationAdministrator,Administrator")]
    public class ScheduleController : CompanyTransactionController<ScheduleTemplate>
    {
        public override ActionResult Create()
        {
            List<string> examList = new List<string>();
            List<string> jobPostingList = new List<string>();
            examList = templateDb.GetList(x => x.CompanyId == SelectedCompany.Id && x.IsDeleted != true && x.Status != "Obsolete").Select(x => x.ExamTemplateName).ToList();
            jobPostingList = JobPosting.GetList(x => x.CompanyId == SelectedCompany.Id && x.IsDeleted != true).Select(x => x.JobName).ToList();
            ViewBag.examinationList = examList;
            ViewBag.jobList = jobPostingList;
            return base.Create();
        }

        public override ActionResult Update(int Id)
        {
            List<string> examList = new List<string>();
            List<string> jobPostingList = new List<string>();

            examList = templateDb.GetList(x => x.CompanyId == SelectedCompany.Id && x.IsDeleted != true && x.Status != "Obsolete").Select(x => x.ExamTemplateName).ToList();
            jobPostingList = JobPosting.GetList(x => x.CompanyId == SelectedCompany.Id && x.IsDeleted != true).Select(x => x.JobName).ToList();

            ViewBag.examinationList = examList;
            ViewBag.jobList = jobPostingList;

            return base.Update(Id);
        }

        public override void DeleteExtraProcess(ScheduleTemplate temp)
        {
            foreach (var item in temp.ScheduledExaminers)
            {
                item.IsDeleted = true;
            }
        }

        [HttpPost]
        public override ActionResult DeleteConfirmed(ScheduleTemplate ent)
        {



            base.DeleteConfirmed(ent);

            return Json(new { deleteSuccess = true }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult getScheduleList()
        {

            List<ScheduleTemplate> model = new List<ScheduleTemplate>();
            model = BaseMethods.GetList(x => x.IsDeleted != true && x.CompanyId == SelectedCompany.Id).ToList();


            var schedList = from t in BaseMethods.GetList(x => x.IsDeleted != true && x.CompanyId == SelectedCompany.Id)
                            select new
                            {
                                schedId = t.Id,
                                color = t.ColorString,
                                //examName = $"{t.Examination.ExamTemplateName} for {t.JobPostingDetail.JobName}",
                                memberCount = t.ScheduledExaminers.Where(x => x.IsDeleted != true).Count(),
                                startDate = t.StartDate.ToLongDateString(),
                                endDate = t.ExpirationDate.ToLongDateString()
                            };
            return Json(schedList, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Update(ScheduleTemplate ent)
        {
            //ent.JobPostingDetail = null;
            //ent.JobPostingDetail = JobPosting.GetDetails(x=>x.Id == ent.JobPostingId);
            //ent.Examination = templateDb.GetDetails(x=>x.Id== ent.ExaminationId);

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

        public override void UpdateExtraProcess(ScheduleTemplate oldT, ScheduleTemplate newT)
        {
            //oldT.ExaminationId = newT.ExaminationId;
            //oldT.JobPostingId = newT.JobPostingId;
            oldT.ColorString = newT.ColorString;


            var forDeleteUsers = (from c in oldT.ScheduledExaminers.Where(x => x.IsDeleted == false)
                                       where !(from d in newT.ScheduledExaminers select d.Id).Contains(c.Id)
                                       select c);

            var forAdd = (from c in newT.ScheduledExaminers.Where(x => x.IsDeleted == false)
                                  where !(from d in oldT.ScheduledExaminers select d.Id).Contains(c.Id)
                                  select c);
            //.Union(from newId in newT.ScheduledExaminers
            //       where !(from oldId in oldT.ScheduledExaminers select oldId.Id).Contains(newId.Id)
            //       select newId).ToList();


            foreach(var item in forDeleteUsers)
            {
                var user = oldT.ScheduledExaminers.Where(x => x.Id == item.Id).FirstOrDefault();
                user.IsDeleted = true;
            }



            foreach (var item in forAdd)
            {
                oldT.ScheduledExaminers.Add(item);
            }















            


            base.UpdateExtraProcess(oldT, newT);
        }

        public JsonResult GetDetails(int id)
        {
            ScheduleTemplate temp = BaseMethods.GetDetails(x => x.Id == id);
            var item = new
            {
                schedId = temp.Id,
                color = temp.ColorString,
                examName = temp.Examination.ExamTemplateName,
                memberCount = temp.ScheduledExaminers.Where(x => x.IsDeleted != true).Count(),
                startDate = temp.StartDate.ToLongDateString(),
                endDate = temp.ExpirationDate.ToLongDateString()

            };
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create(ScheduleTemplate t)
        {
            t.CompanyId = SelectedCompany.Id;
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

            base.Create(t);

            ScheduleTemplate newT = BaseMethods.GetList(x => x.Id == t.Id).FirstOrDefault();


            return Json(new
            {
                success = true,
                successData = new
                {
                    schedId = newT.Id
                }
            }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult ViewResult(int schedId)
        {
            ScheduleTemplate model = new ScheduleTemplate();
            model = BaseMethods.GetDetails(x => x.Id == schedId);
            model.ScheduledExaminers = model.ScheduledExaminers.Where(x => x.IsDeleted != true).ToList();
            ScheduleTemplateResultViewModel viewModel = new ScheduleTemplateResultViewModel();
            if (model != null)
            {
                viewModel = createResultList(model);
            }
            return View(viewModel);
        }

        private ScheduleTemplateResultViewModel createResultList(ScheduleTemplate model)
        {
            ScheduleTemplateResultViewModel viewModel = new ScheduleTemplateResultViewModel();
            List<ExaminersResult> vmList = new List<ExaminersResult>();
            viewModel.Id = model.Id;
            viewModel.ColorString = model.ColorString;
            viewModel.Examination = model.Examination;
            viewModel.StartDate = model.StartDate;
            viewModel.EndDate = model.ExpirationDate;
            viewModel.CreateUser = model.CreateUser;
            viewModel.DateCreated = model.DateCreated;
            //viewModel.JobPostingDetail = model.JobPostingDetail;
            foreach (var item in model.ScheduledExaminers)
            {
                ExaminersResult examiner = new ExaminersResult();
                double examRes;
                examiner.User = item.User;
                examiner.UserId = item.UserId;
                examiner.Status = item.Status;
                examiner.Id = item.Id;

                examiner.PartialScore = ComputeScores(item.Questions.ToList());

                examRes = Convert.ToDouble(Convert.ToDouble(examiner.PartialScore) / Convert.ToDouble(item.Questions.Count()));
                examiner.TotalScore = (examRes * 100);
                //examiner.Remarks = examiner.TotalScore >= model.Examination.PassingScore ? "Passed" : "Failed";
                examiner.ItemsNotCheck = ComputeNotCheck(item.Questions.ToList());
                vmList.Add(examiner);
            }
            viewModel.Results = vmList;
            return viewModel;
        }

        public ActionResult RenderReport(int schedId)
        {
            ScheduleTemplate model = new ScheduleTemplate();
            model = BaseMethods.GetDetails(x => x.Id == schedId);
            ScheduleTemplateResultViewModel viewModel = new ScheduleTemplateResultViewModel();

            if (model != null)
            {
                viewModel = createResultList(model);
            }

            Dictionary<string, string> cookieCollection = new Dictionary<string, string>();
            foreach (var key in Request.Cookies.AllKeys)
            {
                cookieCollection.Add(key, Request.Cookies.Get(key).Value);
            }
            string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";

            return new ViewAsPdf("PrintResult", viewModel)
            {
                Cookies = cookieCollection,
                PageSize = Size.Letter,
                CustomSwitches = footer
            };
        }


        public ActionResult PrintResult()
        {
            return View();
        }

        private int ComputeNotCheck(List<ScheduleQuestion> quest)
        {
            int notcheck = 0;
            List<ScheduleQuestion> model = new List<ScheduleQuestion>();
            model = quest.Where(x => x.Question.QuestionType == "Essay").ToList();

            foreach (var item in model)
            {
                int correctCount = item.UserAnswer.Where(x => x.IsCorrectAnswer == false && x.isSelected == false && x.IsDeleted != true).Count();
                if (correctCount == 1)
                {
                    notcheck = notcheck + 1;
                }
            }
            return notcheck;
        }

        private int ComputeScores(List<ScheduleQuestion> quest)
        {
            int CorrectAns = 0;

            if (quest != null)
            {

                foreach (var item in quest.Where(x => x.IsDeleted != true))
                {
                    if (item.Question.QuestionType == "Multiple_Choice")
                    {
                        //logics for Multiple Choice
                        int insCorrect = item.UserAnswer.Where(x => x.isSelected == true && x.IsCorrectAnswer == true && x.IsDeleted != true).Count();

                        if (insCorrect == 1)
                        {
                            CorrectAns = CorrectAns + 1;
                        }
                    }
                    else if (item.Question.QuestionType == "Matching")
                    {
                        //logics for matching

                        int correctAnswerCount = item.Question.Choices.Where(x => x.IsCorrectAnswer == true && x.IsDeleted != true).Count();
                        int userCorrectAnswer = item.UserAnswer.Where(x => x.IsDeleted != true && x.isSelected == true && x.IsCorrectAnswer == true).Count();
                        if (userCorrectAnswer == correctAnswerCount)
                        {
                            CorrectAns = CorrectAns + 1;
                        }
                    }
                    else if (item.Question.QuestionType == "Essay")
                    {
                        //logics for Essay
                        int intcorrect = item.UserAnswer.Where(x => x.IsCorrectAnswer == true && x.isSelected == true && x.IsDeleted != true).Count();
                        if (intcorrect == 1)
                        {
                            CorrectAns = CorrectAns + 1;
                        }
                    }
                }
            }




            return CorrectAns;
        }

        private ModelStateDictionary ValidateViewModel(ScheduleTemplate model)
        {
            ModelStateDictionary modelState = new ModelStateDictionary();
            int usererror = 0;
            if (model.ScheduledExaminers != null)
            {
                List<int> includedId = new List<int>();

                foreach (var item in model.ScheduledExaminers.ToList())
                {
                    if (!includedId.Any(v => v == item.UserId))
                    {
                        includedId.Add(item.UserId);
                    }
                    else
                    {
                        model.ScheduledExaminers.Remove(item);
                    }




                    bool userExist = userDb.CheckIfExist(x => x.CompanyId == SelectedCompany.Id && x.Id == item.UserId && x.IsDeleted != true && x.IsAdministrator != true);

                    if (!userExist)
                    {
                        usererror = usererror + 1;
                    }
                }
            }
            else
            {
                modelState.AddModelError(String.Empty, String.Format("Invalid number of members", usererror));
            }

            //JobPostingModel jobmod = new JobPostingModel();
            //jobmod = JobPosting.GetDetails(x => x.Id == model.JobPostingId);

            //if (jobmod == null)
            //{
            //    modelState.AddModelError(String.Empty, "Job Posting doesn't exist");
            //}


            //bool examExist = templateDb.CheckIfExist(x => x.CompanyId == SelectedCompany.Id && x.Id == model.ExaminationId && x.IsDeleted != true);
            //if (!examExist)
            //{
            //    modelState.AddModelError(String.Empty, String.Format("Examination not Exist", usererror));
            //}

            if (usererror > 0)
            {
                modelState.AddModelError(String.Empty, String.Format("You have {0} unknown user", usererror));
            }



            return modelState;
        }
    }
}