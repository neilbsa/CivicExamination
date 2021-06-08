using CivicExamProcedures.ExternalContext;
using ExaminationEntity.ExaminationModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CivicExamination.Controllers.Evaluation
{

    [Authorize(Roles = "EvaluationAdmin,EvaluatorUser,Administrator")]
    public class EvaluationAnswerSheetController : Ta3BaseController<AppraisalEmployeeAnswerModel>
    {




        public ActionResult AcceptanceCodeView(int id)
        {
            ViewBag.AnsId = id;


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserAccept(int AnsId, string AcceptanceCode)
        {
            var model = BaseMethods.GetDetails(x => x.Id == AnsId);
            bool isValid = true;
            if (model != null && model.EmployeeDetail.AcceptanceCode == AcceptanceCode)
            {
                isValid = true;
                model.EmployeeDetail.Status = "Accepted";
                model.EmployeeDetail.AcceptanceDate = DateTime.Now;
                model.EmployeeDetail.EvaluatorEmployeeId = UserManager.FindByName(User.Identity.Name).CompanyId;
                BaseMethods.UpdatetoContext(x => x.Id, model);
            }
            else
            {
                isValid = false;
            }
            return Json(new { success = isValid }, JsonRequestBehavior.DenyGet);
        }

        public override ActionResult Details(int id)
        {
            AppraisalEmployeeAnswerModel mod = new AppraisalEmployeeAnswerModel();
            mod = BaseMethods.GetDetails(x => x.EmployeeDetail.Id == id);
            if(mod == null)
            {
                return SystemMessage("The record trying to browse is not existing");
            }



            return View(mod);
        }

        public PartialViewResult KRAItemView()
        {
            KRAModels mod = new KRAModels();

            return PartialView(mod);
        }

        public PartialViewResult RecomendedTrainings()
        {
            RecomendedTraining mod = new RecomendedTraining();
            return PartialView(mod);
        }

        public override void UpdateExtraProcess(AppraisalEmployeeAnswerModel oldT, AppraisalEmployeeAnswerModel newT)
        {
            oldT.MeritIncrease = newT.MeritIncrease;
            oldT.PositionRetained = newT.PositionRetained;
            oldT.Reasons = newT.Reasons;
            oldT.ForPomotion = newT.ForPomotion;
            oldT.ForTransfer = newT.ForTransfer;
            oldT.Comment = newT.Comment;






            foreach (var scores in newT.CompetencygroupAnswer)
            {
                CompetenciesGroupingAnswer group = new CompetenciesGroupingAnswer();
                group = oldT.CompetencygroupAnswer.Where(x => x.Id == scores.Id).FirstOrDefault();
                group.Comment = scores.Comment;

                foreach (var q in scores.CompetenciesMemberAnswers)
                {
                    CompetenciesAnswerSheets grp = new CompetenciesAnswerSheets();
                    grp = group.CompetenciesMemberAnswers.Where(x => x.Id == q.Id).FirstOrDefault();
                    grp.Rating = q.Rating;
                    grp.Remarks = q.Remarks;
                }
            }



            if (newT.KRAs != null)
            {
                var deleteKRA = (from c in oldT.KRAs.Where(x => x.IsDeleted == false).ToList()
                                 where !(from d in newT.KRAs select d.Id).Contains(c.Id)
                                 select c).ToList();

                foreach (var KRA in deleteKRA)
                {
                    KRA.IsDeleted = true;
                }

                foreach (var kra in newT.KRAs)
                {
                    KRAModels mod = new KRAModels();
                    mod = oldT.KRAs.Where(x => x.Id == kra.Id && x.Id != 0).FirstOrDefault();
                    if (mod != null)
                    {
                        mod.RawScore = kra.RawScore;
                        mod.Weight = kra.Weight;
                        mod.Description = kra.Description;
                    }
                    else
                    {
                        oldT.KRAs.Add(kra);
                    }

                }
            }
            else
            {
                foreach(var krs in oldT.KRAs)
                {
                    krs.IsDeleted = true;
                }
            }


            if (newT.RecommendedTrainings != null)
            {
                var deleteTrain = (from c in oldT.RecommendedTrainings.Where(x => x.IsDeleted == false).ToList()
                                 where !(from d in newT.RecommendedTrainings select d.Id).Contains(c.Id)
                                 select c).ToList();

                foreach (var train in deleteTrain)
                {
                    train.IsDeleted = true;
                }

                foreach (var train in newT.RecommendedTrainings)
                {
                    RecomendedTraining mod = new RecomendedTraining();
                    mod = oldT.RecommendedTrainings.Where(x => x.Id == train.Id && x.Id != 0).FirstOrDefault();
                    if (mod != null)
                    {
                        mod.TrainingDescription = train.TrainingDescription;
                        mod.Timetable = train.Timetable;
                        mod.Remarks = train.Remarks;
                    }
                    else
                    {
                        oldT.RecommendedTrainings.Add(train);
                    }

                }
            }
            else
            {
                foreach (var tranings in oldT.RecommendedTrainings)
                {
                    tranings.IsDeleted = true;
                }
            }       


            base.UpdateExtraProcess(oldT, newT);
        }

        public override ActionResult Update(AppraisalEmployeeAnswerModel ent)
        {

            bool issuccess = true;
            try
            {
                base.Update(ent);
            }
            catch
            {
                issuccess = false;
            }
         


            return Json(new { success = issuccess });
        }

        public ActionResult GenerateAnswerSheet(int EmpAppId)
        {
            AppraisalEmployeeAnswerModel tempModel = new AppraisalEmployeeAnswerModel();
            string ManagerId = UserManager.FindByName(User.Identity.Name).CompanyId;
           
            var managerdetail = Employees.GetDetails(c => c.EmployeeNo == ManagerId && c.IsDeleted != true);
         
            if (managerdetail != null)
            {
                AppraisalEmployeeAnswerModel model2 = new AppraisalEmployeeAnswerModel();
                long managerTa3Id = managerdetail.EmployeeID;
                model2 = BaseMethods.GetDetails(x => x.EmployeeDetail.Id == EmpAppId);
                //CivicExamProcedures.ExternalContext.Employee emps = new CivicExamProcedures.ExternalContext.Employee();
                //emps = Employees.GetDetails(x => x.EmployeeID == EmpAppId);
                bool m = Subordinates.CheckIfExist(x => x.SupervisorID == managerTa3Id && x.EmployeeID == model2.EmployeeDetail.Ta3Id);

                //if user is under sub of manager
                if (m)
                {

                    tempModel = BaseMethods.GetDetails(x => x.EmployeeDetail.Id == EmpAppId);
                }
                else
                {
                    return SystemMessage("Dont have access to this entity");
                }
            } 
            return View(tempModel);
        }

        public JsonResult PrepareAnswerSheets(int EmpAppId)
        {
            bool isSuccess = false;
            AppraisalEmployeeAnswerModel mod = new AppraisalEmployeeAnswerModel();
            try
            {

                mod = BaseMethods.GetDetails(x => x.EmployeeId == EmpAppId);
                if (mod == null)
                {
                    Ta3Employee emp = new Ta3Employee();
                    emp = EvaluationEmployee.GetDetails(x => x.Id == EmpAppId);
                    AppraisalEmployeeAnswerModel mod2 = new AppraisalEmployeeAnswerModel();
                    List<CompetenciesGroupingAnswer> AnswerSheets = new List<CompetenciesGroupingAnswer>();
                    List<AnswerRatingScale> ansrateScale = new List<AnswerRatingScale>();
                    mod2.EmployeeId = emp.Id;
                    mod2.KRAPercentage = emp.AppraisalDetails.KRAPercentage;

                    foreach (var item in emp.AppraisalDetails.Competencies)
                    {
                        CompetenciesGroupingAnswer modAns = new CompetenciesGroupingAnswer();
                        List<CompetenciesAnswerSheets> ansSheet = new List<CompetenciesAnswerSheets>();
                        List<AnswerRatingScale> rateScales = new List<AnswerRatingScale>();
                        modAns.GroupType = item.GroupType;
                        modAns.Description = item.Description;
                        modAns.Percentage = item.Percentage;

                        foreach(var grp in item.RatingScale)
                        {
                            AnswerRatingScale rate = new AnswerRatingScale() { Description = grp.Description, ScoreDetails = grp.ScoreDetails };
                            rateScales.Add(rate);
                          
                        }
                        modAns.RatingScales = rateScales;


                        foreach (var comp in item.CompetenciesMember)
                        {
                            ansSheet.Add(new CompetenciesAnswerSheets() { Title = comp.Title, Description = comp.Description });
                           
                        }
                        modAns.CompetenciesMemberAnswers = ansSheet;
                        AnswerSheets.Add(modAns);
                    }
                    mod2.CompetencygroupAnswer = AnswerSheets;
                    BaseMethods.AddtoContext(mod2);
                mod = BaseMethods.GetDetails(c => c.Id == mod2.Id);
            }
      
            isSuccess = true;

        }
            catch (Exception ex)
            {

                isSuccess = false;
            }



            return Json(new { success = isSuccess, id = mod.EmployeeId }, JsonRequestBehavior.AllowGet);
        }


    }
}