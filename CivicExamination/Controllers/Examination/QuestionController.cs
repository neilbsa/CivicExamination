using CivicExamination.Models;
using CivicExamProcedures.Context;
using ExaminationEntity.ExaminationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CivicExamination.Controllers.Examination
{
    [Authorize(Roles = "ExaminationAdministrator,Administrator")]
    public class QuestionController : BaseController<CategoryQuestion>
    {

        public PartialViewResult CreateQuestion()
        {
            return PartialView();
        }

        public PartialViewResult GetPartialQuestionView(string viewName)
        {
            CategoryQuestion model = new CategoryQuestion();
            return PartialView(viewName, model);
        }



        [Authorize]
        public override ActionResult Details(int id)
        {

            return base.Details(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult DeleteConfirmed(CategoryQuestion ent)
        {

            base.DeleteConfirmed(ent);
            return RedirectToAction("Update","Category", new { Id = ent.CategoryId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateQuestion(CategoryQuestion model)
        {

            ModelState.Merge(ValidateViewModel(model));
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


            CategoryQuestion question = new CategoryQuestion();
            List<QuestionChoices> choices = new List<QuestionChoices>();
            FileRepositoryItem fileItem = new FileRepositoryItem();
            if (model.MainQuestionImageModel != null && model.MainQuestionImageModel.ContentLength > 0)
            {
                fileItem.byteContent = convertFileToByte(model.MainQuestionImageModel);
                fileItem.contentLenght = model.MainQuestionImageModel.ContentLength;
                fileItem.contentType = model.MainQuestionImageModel.ContentType;
            }
            if (fileItem == null || fileItem.contentLenght <= 0)
            {
                fileItem = null;
            }


            question.Question = model.Question;
            question.QuestionType = model.QuestionType;
            question.CategoryId = model.CategoryId;


            if (model.MainQuestionImageModel != null)
            {
                question.MainQuestionImage = fileItem != null ? fileItem : null;
            }

            if (model.Choices != null)
            {
            

                foreach (QuestionChoices item in model.Choices)
                {
                    FileRepositoryItem fileItemChoice = new FileRepositoryItem();

                    if (item.choiceImageModel != null && item.choiceImageModel.ContentLength > 0)
                    {
                        fileItemChoice.byteContent = convertFileToByte(item.choiceImageModel);
                        fileItemChoice.contentLenght = item.choiceImageModel.ContentLength;
                        fileItemChoice.contentType = item.choiceImageModel.ContentType;
                    }

                    if (fileItemChoice == null || fileItemChoice.contentLenght <= 0)
                    {
                        fileItemChoice = null;
                    }
                    QuestionChoices choice = new QuestionChoices() { ChoiceString = item.ChoiceString, IsCorrectAnswer = item.IsCorrectAnswer, choiceImage = fileItemChoice };
                    choices.Add(choice);
                }
            }
            else
            {
                if (model.QuestionType == "Essay")
                {
                    QuestionChoices choice = new QuestionChoices() { ChoiceString = "", IsCorrectAnswer = false };
                    choices.Add(choice);
                }
            }

            question.Choices = choices;
            BaseMethods.AddtoContext(question);

            return Json(new { success = true, Id = question.Id }, JsonRequestBehavior.AllowGet);
        }




        public override void UpdateExtraProcess(CategoryQuestion oldT, CategoryQuestion newT)
        {

            oldT.Question = newT.Question;
            FileRepositoryItem item = new FileRepositoryItem();
            if (newT.imageId == null && newT.MainQuestionImageModel == null)
            {
                if (!object.ReferenceEquals(oldT.MainQuestionImage, null))
                {
                    FileRepository.DeletetoContext(oldT.MainQuestionImage);
                }
                item = null;
                oldT.imageId = null;
                oldT.MainQuestionImage = item;

            }
            if (newT.imageId == null && newT.MainQuestionImageModel != null)
            {

                if (!object.ReferenceEquals(oldT.MainQuestionImage, null))
                {
                    FileRepository.DeletetoContext(oldT.MainQuestionImage);
                }
                item.byteContent = convertFileToByte(newT.MainQuestionImageModel);
                item.contentLenght = newT.MainQuestionImageModel.ContentLength;
                item.contentType = newT.MainQuestionImageModel.ContentType;
                oldT.MainQuestionImage = item;

            }


            foreach (var choices in oldT.Choices.Where(x => x.Id != 0).ToList())
            {
                var newChoice = newT.Choices.Where(x => x.Id == choices.Id).FirstOrDefault();


                if (newChoice == null)
                {
                    if (!object.ReferenceEquals(choices.choiceImage, null) && newChoice != null)
                    {
                        FileRepository.DeletetoContext(choices.choiceImage);
                    }

                    choices.IsDeleted = true;
                }
                else
                {
                    choices.IsCorrectAnswer = newChoice.IsCorrectAnswer;
                    choices.ChoiceString = newChoice.ChoiceString;
                    FileRepositoryItem itemChoice = new FileRepositoryItem();
                    if (newChoice.imageId == null && newChoice.choiceImageModel == null)
                    {
                        if (!object.ReferenceEquals(choices.choiceImage, null))
                        {
                            FileRepository.DeletetoContext(choices.choiceImage);
                        }
                        itemChoice = null;
                        choices.imageId = null;
                        //choices.choiceImage = itemChoice;
                    }
                    if (newChoice.imageId == null && newChoice.choiceImageModel != null)
                    {
                        if (!object.ReferenceEquals(choices.choiceImage, null))
                        {
                            FileRepository.DeletetoContext(choices.choiceImage);
                        }
                        itemChoice.byteContent = convertFileToByte(newChoice.choiceImageModel);
                        itemChoice.contentLenght = newChoice.choiceImageModel.ContentLength;
                        itemChoice.contentType = newChoice.choiceImageModel.ContentType;
                        choices.choiceImage = itemChoice;

                    }
                }

            }

            if (newT.Choices != null)
            {
                var forAddition = newT.Choices.Where(x => x.Id == 0).ToList();

                foreach (var addItem in forAddition)
                {

                    FileRepositoryItem itemChoice = new FileRepositoryItem();
                    if (addItem.choiceImageModel != null && addItem.choiceImageModel.ContentLength > 0)
                    {
                        itemChoice.byteContent = convertFileToByte(addItem.choiceImageModel);
                        itemChoice.contentLenght = addItem.choiceImageModel.ContentLength;
                        itemChoice.contentType = addItem.choiceImageModel.ContentType;
                    }
                    addItem.choiceImage = itemChoice;
                    oldT.Choices.Add(addItem);
                }
            }

            //oldT.Choices = newT.Choices;





            base.UpdateExtraProcess(oldT, newT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Update(CategoryQuestion ent)
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


        private ModelStateDictionary ValidateViewModel(CategoryQuestion model)
        {
            ModelStateDictionary modelState = new ModelStateDictionary();


            if (model.QuestionType == "Multiple_Choice" || model.QuestionType == "Matching")
            {
                if ((object.ReferenceEquals(model.MainQuestionImageModel, null) && model.imageId == null) && (String.IsNullOrEmpty(model.Question) || String.IsNullOrWhiteSpace(model.Question)))
                {
                    modelState.AddModelError(String.Empty, "Question is Required");
                }

                if (model.Choices == null || model.Choices.Count() <= 0)
                {
                    modelState.AddModelError(String.Empty, "Question Selection is require");
                }

                int error = 0;
                if (model.Choices != null)
                {

                    foreach (var item in model.Choices)
                    {
                        if ((object.ReferenceEquals(item.choiceImageModel, null) && item.imageId == null) && (String.IsNullOrEmpty(item.ChoiceString) || String.IsNullOrWhiteSpace(item.ChoiceString)))
                        {
                            error = error + 1;
                        }
                    }
                }
                if (error > 0)
                {
                    modelState.AddModelError(String.Empty, String.Format("You have {0} empty choices description", error));
                }
            }
            else if (model.QuestionType == "Essay")
            {
                if ((model.MainQuestionImageModel == null || model.MainQuestionImageModel.ContentLength <= 0) && (String.IsNullOrEmpty(model.Question) || String.IsNullOrWhiteSpace(model.Question)))
                {
                    modelState.AddModelError(String.Empty, "Question is Required");
                }
            }

            if (model.Choices != null)
            {
                int formatError = 0;

                foreach (var choices in model.Choices)
                {
                    if (choices.choiceImageModel != null)
                    {
                        if (!choices.choiceImageModel.ContentType.Contains("image"))
                        {
                            formatError = formatError + 1;
                        }
                    }

                }
                if (formatError > 0)
                {
                    modelState.AddModelError(String.Empty, String.Format("You have {0} invalid File Format in choices", formatError));
                }
            }




            return modelState;
        }


        //public ActionResult UpdateQuestion(int Id)
        //{
        //    ExaminationCategoryViewModel viewModel = new ExaminationCategoryViewModel();
        //    CategoryQuestion model = new CategoryQuestion();
        //    model = BaseMethods.GetDetails(x => x.Id == Id);
        //    viewModel.Id = model.Id;
        //    //viewModel.imageId = model.imageId;
        //    viewModel.MainQuestionImage = model.MainQuestionImage;
        //    viewModel.Question = model.Question;
        //    viewModel.QuestionType = model.QuestionType;

        //    foreach(var item in model.Choices)
        //    {
        //        CategoryQuestionChoicesViewModel choiceModel = new CategoryQuestionChoicesViewModel();
        //        choiceModel.Id = item.Id;
        //        choiceModel.choiceImage = item.choiceImage;
        //        choiceModel.ChoiceString = item.ChoiceString;
        //        choiceModel.IsCorrectAnswer = item.IsCorrectAnswer;
        //        viewModel.QuestionsChoices.Add(choiceModel);
        //    }
        //    return View(viewModel);
        //}




    }


    //public class QuestionChoicesContext : DbConnect<QuestionChoices>
    //{

    //}

}