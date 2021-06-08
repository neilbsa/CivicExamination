using CivicExamination.Models;
using ExaminationEntity.ExaminationModels;
using Microsoft.AspNet.Identity;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CivicExamination.Controllers.Examination
{
    [Authorize]
    public class MainExaminationController : BaseController<ScheduleExaminerModel>
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrepareExamination(int SchedId)
        {
            ScheduleExaminerModel model = new ScheduleExaminerModel();
            model = BaseMethods.GetDetails(x => x.Id == SchedId);

            if (model.Questions.Where(x => x.IsDeleted != true) != null)
            {
                foreach (var item in model.Questions.Where(x => x.IsDeleted != true).ToList())
                {
                    item.IsDeleted = true;


                    foreach (var items in item.UserAnswer)
                    {
                        items.IsDeleted = true;
                    }
                }
                BaseMethods.UpdatetoContext(x => x.Id, model);
            }
            if (model != null)
            {
                List<int> includedQuestionId = new List<int>();
                foreach (ExamCategoryTemplate item in model.Schedule.Examination.TemplateBuild.Where(x => x.IsDeleted != true))
                {
                    List<int> questionId = item.CategoryDetail.Questions.Where(x => x.IsDeleted != true).Select(x => x.Id).ToList();
                    int itemCount = item.Items;
                    for (int i = 1; i <= itemCount; i++)
                    {
                        questionId = questionId.Where(x => !includedQuestionId.Contains(x)).ToList();
                        int rndNumber = GenerateRandomNumber(0, questionId.Count());
                        int selectedQuestion = questionId[rndNumber];
                        includedQuestionId.Add(selectedQuestion);
                    }
                }
                List<ScheduleQuestion> userQuestion = new List<ScheduleQuestion>();
                foreach (var i in includedQuestionId)
                {
                    CategoryQuestion question = new CategoryQuestion();
                    List<UserAnswer> userAns = new List<UserAnswer>();
                    question = Questions.GetDetails(x => x.Id == i && x.IsDeleted != true);
                    foreach (var item in question.Choices)
                    {

                        userAns.Add(new UserAnswer() { IsCorrectAnswer = item.IsCorrectAnswer, ChoiceString = item.ChoiceString, imageId = item.imageId });
                    }

                    userQuestion.Add(new ScheduleQuestion { QuestionId = i, UserAnswer = userAns });
                }
                if (model.Questions == null)
                {
                    model.Questions = userQuestion;
                }
                else
                {
                    foreach (var item in userQuestion)
                    {
                        model.Questions.Add(item);
                    }
                }
                //  model.Status = "Prepared";
                model.RemainSeconds = model.Schedule.Examination.LimitSeconds * 60;
                BaseMethods.UpdatetoContext(x => x.Id, model);
                return Json(new { success = true }, JsonRequestBehavior.DenyGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.DenyGet);
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            Random rand = new Random();
            int item = 0;
            for (int i = 1; i <= max; i++)
            {
                item = rand.Next(min, max);
            }
            return item;
        }

        public ActionResult OnExamination(int schedId)
        {
            ScheduleExaminerModel model = new ScheduleExaminerModel();
            var user = UserManager.FindByName(User.Identity.Name);
            int userId = user.UserDetail.Id;
            string[] status = { "Finish" };

            model = BaseMethods.GetDetails(x => x.Id == schedId && x.UserId == userId && !status.Contains(x.Status));
            //ViewBag.IsFirstLoad = "true";
            ViewData["IsFirstLoad"] = "true";

            if (model != null && model.Status == "Prepared")
            {
                ViewData["IsFirstLoad"] = "false";
            }


            model.Status = "Prepared";
            BaseMethods.UpdatetoContext(x => x.Id, model);

            return View(model);
        }

        public ActionResult updateServerTime(int schedId, int Remaining)
        {
            ScheduleExaminerModel model = new ScheduleExaminerModel();
            var user = UserManager.FindByName(User.Identity.Name);
            int userId = user.UserDetail.Id;
            model = BaseMethods.GetDetails(x => x.Id == schedId && x.UserId == userId);
            if (model != null)
            {
                model.RemainSeconds = Remaining;
                BaseMethods.UpdatetoContext(x => x.Id, model);
            }

            return null;
        }

        [HttpPost]
        public ActionResult QuestionView(int schedId, int QuestionId)
        {
            ScheduleExaminerModel mod = new ScheduleExaminerModel();
            mod = BaseMethods.GetDetails(x => x.Id == schedId);
            ScheduleQuestion question = new ScheduleQuestion();

            question = mod.Questions.Where(x => x.QuestionId == QuestionId && x.IsDeleted != true).FirstOrDefault();
            if (question != null)
            {
                return View(question);
            }
            return null;
        }

        [HttpPost]
        public ActionResult updateSelected(int ansId, int quesId)
        {
            ScheduleQuestion question = new ScheduleQuestion();
            UserAnswer ans = new UserAnswer();
            question = SchedQuestion.GetDetails(x => x.Id == quesId);
            if (question != null)
            {
                foreach (var item in question.UserAnswer.Where(x => x.Id != ansId))
                {
                    item.isSelected = false;
                }
                foreach (var item in question.UserAnswer.Where(x => x.Id == ansId))
                {
                    item.isSelected = true;
                }
            }
            SchedQuestion.UpdatetoContext(x => x.Id, question);
            return null;
        }

        [HttpPost]
        public ActionResult updateChoosen(int ansId, int quesId, bool value)
        {
            ScheduleQuestion question = new ScheduleQuestion();
            UserAnswer ans = new UserAnswer();
            question = SchedQuestion.GetDetails(x => x.Id == quesId);
            if (question != null)
            {
                ans = question.UserAnswer.Where(x => x.Id == ansId).FirstOrDefault();
                ans.isSelected = value;
            }

            SchedQuestion.UpdatetoContext(x => x.Id, question);


            return null;
        }

        [Authorize(Roles = "ExamineeAdministrator,ExaminationAdministrator,Administrator")]
        public ActionResult ViewUserExamination(int schedId, int userId)
        {
            ScheduleExaminerModel model = new ScheduleExaminerModel();
            model = BaseMethods.GetDetails(x => x.ScheduleId == schedId && x.UserId == userId);

            ScheduleExaminerViewModel viewModel = new ScheduleExaminerViewModel();
            List<questionCorrectionViewModel> listQuestion = new List<questionCorrectionViewModel>();
            if (model != null)
            {
                viewModel.Id = model.Id;
                viewModel.User = model.User;
                viewModel.Schedule = model.Schedule;

                foreach (var item in model.Questions)
                {
                    questionCorrectionViewModel currenQuestionModel = new questionCorrectionViewModel();
                    bool iscorrect = checkAns(item);

                    currenQuestionModel.Question = item.Question;
                    currenQuestionModel.UserAnswer = item.UserAnswer;
                    currenQuestionModel.IsCorrect = iscorrect;

                    if (item.Question.QuestionType == "Essay")
                    {
                        currenQuestionModel.essayStatus = checkAnswerEssay(item);
                    }
                    listQuestion.Add(currenQuestionModel);
                }
                viewModel.CorrectedQuestion = listQuestion;
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("SystemMessage", new { message = "Record Not found" });
            }
        }

        [Authorize(Roles = "ExamineeAdministrator,ExaminationAdministrator,Administrator")]
        public ActionResult UpdateExamination(int schedId, int userId)
        {
            ScheduleExaminerModel model = new ScheduleExaminerModel();
            model = BaseMethods.GetDetails(x => x.ScheduleId == schedId && x.UserId == userId);
            ScheduleExaminerViewModel viewModel = new ScheduleExaminerViewModel();
            List<questionCorrectionViewModel> questions = new List<questionCorrectionViewModel>();

            if (model != null)
            {
                viewModel.Schedule = model.Schedule;
                viewModel.UserId = model.UserId;
                viewModel.Id = model.Id;

                foreach (var item in model.Questions.Where(x => x.Question.QuestionType == "Essay"))
                {


                    questionCorrectionViewModel currmod = new questionCorrectionViewModel();
                    string ans;
                    ans = checkAnswerEssay(item);
                    currmod.Id = item.Id;
                    currmod.Question = item.Question;
                    currmod.UserAnswer = item.UserAnswer;
                    currmod.essayStatus = ans;
                    questions.Add(currmod);
                }

                viewModel.CorrectedQuestion = questions;
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("SystemMessage", new { message = "Record Not found" });
            }





        }

        [HttpPost]
        [Authorize(Roles = "ExamineeAdministrator,ExaminationAdministrator,Administrator")]
        public ActionResult UpdateEssayAnswer(string type, int questionId, int schedId, int userId)
        {
            ScheduleExaminerModel model = new ScheduleExaminerModel();
            model = BaseMethods.GetDetails(x => x.ScheduleId == schedId && x.UserId == userId);

            if (model != null)
            {
                ScheduleQuestion question = new ScheduleQuestion();
                UserAnswer choices = new UserAnswer();
                question = model.Questions.Where(v => v.Id == questionId).FirstOrDefault();
                choices = question.UserAnswer.FirstOrDefault();
                if (type == "Correct")
                {
                    choices.isSelected = true;
                    choices.IsCorrectAnswer = true;
                }
                else if (type == "Wrong")
                {
                    choices.isSelected = true;
                    choices.IsCorrectAnswer = false;
                }
                else
                {
                    choices.isSelected = false;
                    choices.IsCorrectAnswer = false;
                }
                BaseMethods.UpdatetoContext(x => x.Id, model);

                return Json(new { success = true }, JsonRequestBehavior.DenyGet);
            }




            return Json(new { success = false }, JsonRequestBehavior.DenyGet); ;
        }

        public bool checkAns(ScheduleQuestion model)
        {

            bool ans = false;
            if (model.Question.QuestionType == "Multiple_Choice")
            {
                //logics for Multiple Choice
                var mod = model.UserAnswer.Where(x => x.isSelected == true && x.IsCorrectAnswer == true && x.IsDeleted != true);
                int insCorrect = 0;
                if (mod != null)
                {
                    insCorrect = mod.Count();
                }


                if (insCorrect == 1)
                {
                    ans = true;
                }
            }
            else if (model.Question.QuestionType == "Matching")
            {
                //logics for matching

                int correctAnswerCount = model.Question.Choices.Where(x => x.IsCorrectAnswer == true && x.IsDeleted != true).Count();
                int userCorrectAnswer = model.UserAnswer.Where(x => x.IsDeleted != true && x.isSelected == true && x.IsCorrectAnswer == true).Count();
                if (userCorrectAnswer == correctAnswerCount)
                {
                    ans = true;
                }
            }
            return ans;
        }

        public string checkAnswerEssay(ScheduleQuestion model)
        {
            string returnVal = "";

            if (model.Question.QuestionType == "Essay")
            {
                //logics for Essay
                // isselected true and iscorrect ans true - Correct Answer
                // iselected true and is Correct false - wrong answer
                // iselected false and iscorrect false - not check
                var ans = model.UserAnswer.FirstOrDefault();
                if (ans.isSelected && ans.IsCorrectAnswer)
                {
                    returnVal = "Correct";
                }
                else if (ans.isSelected && !ans.IsCorrectAnswer)
                {
                    returnVal = "Wrong";
                }
                else if (!ans.isSelected && !ans.IsCorrectAnswer)
                {
                    returnVal = "NotChecked";
                }
            }
            return returnVal;
        }

        public ActionResult RenderReportData(int schedId, int userId)
        {
            ScheduleExaminerModel model = new ScheduleExaminerModel();
            model = BaseMethods.GetDetails(x => x.ScheduleId == schedId && x.UserId == userId);

            ScheduleExaminerViewModel viewModel = new ScheduleExaminerViewModel();
            List<questionCorrectionViewModel> listQuestion = new List<questionCorrectionViewModel>();
            if (model != null)
            {
                viewModel.Id = model.Id;
                viewModel.User = model.User;
                viewModel.Schedule = model.Schedule;

                foreach (var item in model.Questions)
                {
                    questionCorrectionViewModel currenQuestionModel = new questionCorrectionViewModel();
                    bool iscorrect = checkAns(item);

                    currenQuestionModel.Question = item.Question;
                    currenQuestionModel.UserAnswer = item.UserAnswer;
                    currenQuestionModel.IsCorrect = iscorrect;
                    //convertArrayToImage(item.Question.MainQuestionImage);
                    if (item.Question.QuestionType == "Essay")
                    {
                        currenQuestionModel.essayStatus = checkAnswerEssay(item);
                    }
                    listQuestion.Add(currenQuestionModel);
                }
                viewModel.CorrectedQuestion = listQuestion;



                Dictionary<string, string> cookieCollection = new Dictionary<string, string>();
                foreach (var key in Request.Cookies.AllKeys)
                {
                    cookieCollection.Add(key, Request.Cookies.Get(key).Value);
                }
                string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";




                return new ViewAsPdf("UserExaminationReport", viewModel)
                {
                    Cookies = cookieCollection,
                    PageSize = Size.Letter,
                    CustomSwitches = footer

                };



            }
            else
            {
                return RedirectToAction("SystemMessage", new { message = "Record Not found" });
            }

        }

        public ActionResult UserExaminationReport()
        {
            return View();


        }

        [HttpPost]
        public ActionResult updateEssayAnswerOnExam(int quesId, int choiceId, string Answer)
        {
            ScheduleQuestion question = new ScheduleQuestion();
            question = SchedQuestion.GetDetails(x => x.Id == quesId);
            UserAnswer answer = new UserAnswer();
            if (question != null)
            {
                answer = question.UserAnswer.FirstOrDefault();
                answer.ChoiceString = Answer;
            }
            SchedQuestion.UpdatetoContext(x => x.Id, question);

            return null;
        }

        public ActionResult FinishExamination(int Id)
        {
            ScheduleExaminerModel model = new ScheduleExaminerModel();
            model = BaseMethods.GetDetails(x => x.Id == Id);
            model.Status = "Finish";
            model.FinishDate = DateTime.Now;
            BaseMethods.UpdatetoContext(x => x.Id, model);
            return RedirectToAction("ExaminerDashboard", "Home");
        }
    }
}