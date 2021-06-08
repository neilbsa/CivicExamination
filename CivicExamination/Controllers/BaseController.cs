
using CivicExamProcedures.Methods;
using ExaminationEntity.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CivicExamProcedures.Context;
using ExaminationEntity.ExaminationModels;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
//using Rotativa;
//using Rotativa.Options;

namespace CivicExamination.Controllers
{

    public abstract class BaseController<TEntity> : Controller, IBaseControlMethod<TEntity> where TEntity : class, IBaseEntity
    {


        public DbConnect<TEntity> BaseMethods { get; set; }
        public DbConnect<FileRepositoryItem> FileRepository { get; set; }
        public DbConnect<QuestionChoices> QuestionsChoices { get; set; }
        public DbConnect<CategoryQuestion> Questions { get; set; }
        public DbConnect<Category> CategoriesDb { get; set; }
        public DbConnect<ExamTemplate> templateDb { get; set; }
        public DbConnect<UserProfile> userDb { get; set; }
        public DbConnect<ScheduleQuestion> SchedQuestion { get; set; }
        public DbConnect<JobPostingModel> JobPosting { get; set; }
        public DbConnect<ScheduleTemplate> TemplateSchedule { get; set; }
        public DbConnect<ScheduleExaminerModel> SchedExaminer { get; set; }
        private ApplicationUserManager _userManager;
        public BaseController()
        {
            JobPosting = new DbConnect<JobPostingModel>();
            Questions = new DbConnect<CategoryQuestion>();
            SchedQuestion = new DbConnect<ScheduleQuestion>();
            FileRepository = new DbConnect<FileRepositoryItem>();
            CategoriesDb = new DbConnect<Category>();
            templateDb = new DbConnect<ExamTemplate>();
            userDb = new DbConnect<UserProfile>();
            TemplateSchedule = new DbConnect<ScheduleTemplate>();
            SchedExaminer = new DbConnect<ScheduleExaminerModel>();
            BaseMethods = new DbConnect<TEntity>();

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }


        }

        public virtual bool HasDetailAccess(TEntity t) { return true; }
        public virtual void UpdateExtraProcess(TEntity oldT, TEntity newT) { }

        // GET: Base
        public virtual ActionResult Index()
        {
            return View(BaseMethods.GetList());
        }


        //public void convertArrayToImage(FileRepositoryItem image)
        //{
        //    if (image.contentLenght > 0)
        //    {
        //        using (MemoryStream ms = new MemoryStream(image.byteContent))
        //        {
        //            Image img = Image.FromStream(ms);
        //            string path = Path.Combine(Server.MapPath("~/Images/"),$"{image.Id}.jpeg");
        //            img.Save(path, ImageFormat.Jpeg);
        //        }
        //    }

        //}

        public virtual ActionResult GetViewWithModel(string viewName, int? Id = null)
        {
            TEntity model = null;

            if (Id != null)
            {
                model = BaseMethods.GetDetails(x => x.Id == Id);
            }

            if (model == null)
            {
                return View(viewName);
            }
            else
            {
                return View(viewName, model);
            }
        }

        protected byte[] convertFileToByte(HttpPostedFileBase item)
        {

            byte[] value = new byte[item.ContentLength];
            if (item.ContentLength > 0 && item != null)
            {
                //this return arrayed Image
                item.InputStream.Read(value, 0, item.ContentLength);
            }
            return value;
        }

        public virtual ActionResult Update(int Id)
        {
            TEntity t;
            t = BaseMethods.GetDetails(Id);

            if (!HasDetailAccess(t))
            {
                return RedirectToAction("SystemMessage", new { mess = "You Dont Have Access" });
            }
            return View(t);
        }

        public virtual ActionResult GetView(string viewName)
        {

            return View(viewName);
        }



        public virtual ActionResult SystemMessage(string message)
        {
            ViewData["SystemMessage"] = message;
            return View();
        }



        public virtual ActionResult Delete(int id)
        {
            TEntity ent = BaseMethods.GetDetails(x => x.Id == id);
            return View(ent);
        }

        [HttpPost]
        public virtual ActionResult DeleteConfirmed(TEntity ent)
        {
            TEntity temp = BaseMethods.GetDetails(x => x.Id == ent.Id);

            DeleteExtraProcess(temp);
            BaseMethods.DeletetoContext(temp);
            return View();
        }

        public virtual void DeleteExtraProcess(TEntity temp)
        {

        }

        [HttpPost]
        public virtual ActionResult Update(TEntity ent)
        {
            TEntity oldT = BaseMethods.GetDetails(ent.Id);
            UpdateExtraProcess(oldT, ent);
            BaseMethods.UpdatetoContext(t => t.Id, oldT);
            return View(ent);
        }

        [HttpPost]
        public virtual JsonResult UpdateJson(TEntity ent)
        {
            bool isUpdateSuccess = false;
            TEntity newEntity = BaseMethods.GetDetails(ent.Id);
            if (newEntity != null)
            {
                isUpdateSuccess = true;
                UpdateExtraProcess(newEntity, ent);
                BaseMethods.UpdatetoContext(t => t.Id, newEntity);
            }

            return Json(isUpdateSuccess, JsonRequestBehavior.DenyGet);
        }


        public virtual ActionResult Details(int id)
        {

            TEntity t;
            t = BaseMethods.GetDetails(id);

            if (!HasDetailAccess(t))
            {
                return RedirectToAction("SystemMessage", new { mess = "You Dont Have Access" });
            }
            return View(t);
        }

        public virtual ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public virtual ActionResult Create(TEntity t)
        {
            BaseMethods.AddtoContext(t);
            return View();
        }



        public virtual ActionResult ErrorPage(string t)
        {
            return View(t);
        }


    }
}