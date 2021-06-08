
using CivicExamProcedures.Context;
using ExaminationEntity.ExaminationModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CivicExamination.Controllers
{

    [Authorize]
    public class HomeController : BaseController<UserProfile>
    { 




        public UserProfile CurrentUser { get; set; }
        public HomeController()
        {
            CurrentUser = new UserProfile();
        }

        public ActionResult ExaminerDashboard()
        {
            var user = UserManager.FindByName(User.Identity.Name);
            DateTime Datenow = DateTime.Today;
        
            
            //Datenow = DateTime.Today.AddHours(-1);
            
            CurrentUser = BaseMethods.GetDetails(x => x.Id == user.UserDetailId);
         var list = CurrentUser.ScheduledExaminations.Where(x => x.IsDeleted == false &&
                        x.Schedule.IsDeleted ==false &&
                        ( x.Schedule.ExpirationDate.Date >=Datenow &&
                         x.Schedule.StartDate.Date.Date <= Datenow)
                        && x.Schedule.StartDate.Month == DateTime.Now.Month
            );

            CurrentUser.ScheduledExaminations = list.ToList();
            return View(CurrentUser);
        }



    }
}