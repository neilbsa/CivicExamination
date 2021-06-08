using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CivicExamination.Models;
using CivicExamProcedures.Context;
using ExaminationEntity.ExaminationModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Web.Security;
using CivicExamination.AuthorizationLogics;

namespace CivicExamination.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
       private DbConnect<Company> DbComp { get; set; }
        private DbConnect<UserCompany> userComp { get; set; }
        private DbConnect<UserProfile> userProf { get; set; }
        //  private ApplicationContext db = new ApplicationContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private Company SelectedCompany { get; set; }
        public ApplicationContext context { get; set; }
        public AccountController()
        {
            SelectedCompany = new Company();
            SelectedCompany = System.Web.HttpContext.Current.Session["COMPANY"] as Company;
            userProf = new DbConnect<UserProfile>();
            userComp = new DbConnect<UserCompany>();
            DbComp = new DbConnect<Company>();
            context = new ApplicationContext();
        }
   


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

    
        public ActionResult SelectCompany(string returnUrl)
        {
            var companies = AssignedCompanies();
            if(companies.Where(x=>x.IsDeleted != true).ToList().Count <= 1){
                var comp = companies.FirstOrDefault();
                return SetCompany(comp.Id, returnUrl);
            }
            ViewBag.returnUrl = returnUrl;
            return View(companies);
        }

        public ActionResult UpdatePassword(int Id)
        {
            UpdatePassword model = new UpdatePassword();
            var user = context.Users.Where(x => x.UserDetailId == Id).First();
            model.User = user;
            model.Username = user.UserName;
            model.User.Id = user.Id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdatePassword(UpdatePassword ent)
        {
            UpdatePassword model = new UpdatePassword();
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
            
                model = ent;
                //var user = context.Users.Where(x => x.Id == model.User.Id);
                var result = UserManager.RemovePassword(model.User.Id);
                if (result.Succeeded)
                {
                    UserManager.AddPassword(model.User.Id, model.Password);
                    isSuccess = true;
                }
            }


            return Json(isSuccess, JsonRequestBehavior.DenyGet);
        }




        
        public ActionResult UpdateAdminPassword(int Id)
        {
            UpdatePassword model = new UpdatePassword();
            var user = context.Users.Where(x => x.UserDetailId == Id).First();
            model.User = user;
            model.Username = user.UserName;
            model.User.Id = user.Id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateAdminPassword(UpdatePassword ent)
        {
            UpdatePassword model = new UpdatePassword();
            bool isSuccess = false;
           
            if (ModelState.IsValid)
            {
            
                model = ent;
                //var user = context.Users.Where(x => x.Id == model.User.Id);
                var result = UserManager.RemovePassword(model.User.Id);
                if (result.Succeeded)
                {
                    UserManager.AddPassword(model.User.Id, model.Password);
                    isSuccess = true;
                }
            }
            return Json(isSuccess, JsonRequestBehavior.DenyGet);
        }





        private static Random random = new Random();
        public static string RandomPassword(int lenght)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+=-";

            return new string(Enumerable.Repeat(chars, lenght)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        [HttpPost]
        public JsonResult GenerateRandomPassword()
        {
            string password = null;
            password = RandomPassword(10);
            return Json(password,JsonRequestBehavior.DenyGet);
        }


        public ActionResult SetCompany(int id, string returnUrl)
        {
            var user = UserManager.FindByName(User.Identity.Name);
            Company selectedComp = DbComp.GetDetails(id);
            bool isHasAccess = userComp.CheckIfExist(x=> x.IsDeleted==false && x.UserId == user.UserDetailId && x.CompanyId == selectedComp.Id);

            if (selectedComp != null && isHasAccess)
            {
                Session["COMPANY"] = selectedComp;
                if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
          
            }
            else
            {
                return RedirectToAction("Account", "SelectCompany");
            }       
        }

        private List<Company> AssignedCompanies()
        {
            var user = UserManager.FindByName(User.Identity.Name);
            List<Company> AllowedCompanies = new List<Company>();
            if (user != null)
            {
                AllowedCompanies = userComp.GetList(x => x.UserDetail.Id == user.UserDetailId && x.IsDeleted != true).Select(x => x.CompanyDetails).ToList();
            }
            return AllowedCompanies;
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    Session["COMPANY"] = null;
                    Session["USERPROFILE"] = null;
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                    var user = UserManager.FindByName(model.Username);

                    UserProfile profile = user.UserDetail;

                    //Session["USERPROFILE"] = profile;

                    if (profile.IsAdministrator && returnUrl != null)
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else if(!profile.IsAdministrator)
                    {
                        return RedirectToAction("ExaminerDashboard", "Home");
                       
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                   
                case SignInStatus.Failure:
                    ViewBag.LoginError = "Username or Password is incorrect";
                    return View("Login");
                case SignInStatus.LockedOut:
                    ViewBag.LoginError = "User is Locked out";
                    return View("Login");
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

     
        public ActionResult AdminAccountIndex() {
            List<UserProfile> Adminlist = new List<UserProfile>();
            Adminlist = context.UserProfiles.Where(x => x.IsAdministrator == true).ToList();
            return View(Adminlist);
        }

        public ActionResult AdminDetails(int Id)
        {
            UserProfile model = new UserProfile();
            model = userProf.GetDetails(Id);
            return View(model);
        }
  

        public ActionResult UpdateAdmin(int Id)
        {
            UpdateViewModel model = new UpdateViewModel();
            IEnumerable<Company> companies = new List<Company>();
            List<CompanySelectionViewModel> companyView = new List<CompanySelectionViewModel>();
            List<UserRoleSelection> selectedRoles = new List<UserRoleSelection>();
            model.User=context.Users.Where(x => x.UserDetailId == Id).First();
            var credentialDetail = UserManager.FindById(model.User.Id);
            model.Username = credentialDetail.UserName;
            model.User.UserDetail = context.UserProfiles.Where(x => x.Id == model.User.UserDetailId).First();
            companies = DbComp.GetList();
            var assComp = userComp.GetList(x => x.UserId == Id && x.IsDeleted == false);
            var assRoles = UserManager.GetRoles(model.User.Id);
            var roles = context.Roles.ToList();
            foreach(var item in companies)
            {
                string status = assComp.Any(x=>x.CompanyId == item.Id) == true ? "true":"false";
                companyView.Add(new CompanySelectionViewModel() {  CompanyId = item.Id, CompanyName = item.Name, IsSelected =status});
            }

            foreach (var item in roles)
            {
                string status = assRoles.Contains(item.Name) == true ? "true" : "false";
                selectedRoles.Add(new UserRoleSelection() {  Id = item.Id,  Name = item.Name, IsSelected = status });
            }

            model.SelectedRoles = selectedRoles;
            model.SelectedCompany = companyView;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateAdmin(UpdateViewModel model)
        {
            bool isValid = false;
            UserProfile newUser = userProf.GetDetails(model.User.UserDetail.Id);
            newUser.Firstname = model.User.UserDetail.Firstname;
            newUser.Middlename = model.User.UserDetail.Middlename;
            newUser.Lastname = model.User.UserDetail.Lastname;
            newUser.PresentAddress = model.User.UserDetail.PresentAddress;
            newUser.HomeContact = model.User.UserDetail.HomeContact;
            newUser.Gender = model.User.UserDetail.Gender;
            userProf.UpdatetoContext(x => x.Id, newUser);


            foreach(var item in model.SelectedCompany)
            {
                bool update = item.IsSelected == "true" ? true : false;
                UserCompany record = userComp.GetDetails(x => x.UserId == model.User.UserDetail.Id && x.CompanyId == item.CompanyId);

                if (record != null)
                {
                    record.IsDeleted = !update;
                    userComp.UpdatetoContext(x => x.Id, record);
                }
                else
                {
                    if (update)
                    {
                        userComp.AddtoContext(new UserCompany() { CompanyId = item.CompanyId, UserId = model.User.UserDetail.Id });
                    }
                }
            }


            foreach (var item in model.SelectedRoles)
            {
                bool update = item.IsSelected == "true" ? true : false;
             
                if (update)
                {
                    UserManager.AddToRole(model.User.Id, item.Name);
                }
                else
                {
                    UserManager.RemoveFromRole(model.User.Id, item.Name);
                }
            }


            isValid = true;

            return Json(isValid, JsonRequestBehavior.DenyGet);

        }


           public ActionResult RegisterAdmin()
           {
            RegisterViewModel model = new RegisterViewModel();
            
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            IEnumerable<Company> companies = new List<Company>();
            List<CompanySelectionViewModel> companyView = new List<CompanySelectionViewModel>();
            List<UserRoleSelection> selectedRoles = new List<UserRoleSelection>();
            companies = DbComp.GetList();


            foreach (var item in companies)
            {
                
                companyView.Add(new CompanySelectionViewModel() { CompanyId = item.Id, CompanyName = item.Name, IsSelected = "false" });
            }
            
            foreach ( var item in roleManager.Roles.Where(x => x.Name != "Common").ToList())
            {
                selectedRoles.Add(new UserRoleSelection() { Id = item.Id, Name = item.Name, IsSelected = "false" });
            }

            model.SelectedCompany = companyView;
            model.SelectedRoles = selectedRoles;
               
            ViewBag.Roles = roleManager.Roles.Where(x=>x.Name != "Common").Select(x => x.Name).ToList();

            return View(model);
        }

        //
        // POST: /Account/Register
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterAdmin(RegisterViewModel model)
        {
            model.User.UserDetail.HomeContact = "N/A";
            model.User.UserDetail.MobileContact = "N/A";
            model.User.UserDetail.PresentAddress = "N/A";
            model.User.UserDetail.PermanentAddress = "N/A";
            model.User.UserDetail.BirthDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                IEnumerable<Company> companies = new List<Company>();
                model.User.UserDetail.CompanyId = 1;
                model.User.UserDetail.IsAdministrator = true;
                
                var user = new ApplicationUser { UserName = model.Username, Email = model.User.UserDetail.EmailAddress,CompanyId = model.User.CompanyId };
              
                var result = UserManager.Create(user, model.Password);

            
                if (result.Succeeded)
                {
                    var td = userProf.AddtoContext(model.User.UserDetail);
                    user.UserDetailId = td.Id;
                    var updateResult = UserManager.Update(user);



                    foreach (var trueComp in model.SelectedCompany.Where(x => x.IsSelected == "true"))
                    {
                        userComp.AddtoContext(new UserCompany() { CompanyId = trueComp.CompanyId, UserId = td.Id, IsDeleted = false });
                    }


                    foreach (var trueRoles in model.SelectedRoles.Where(x => x.IsSelected == "true"))
                    {
                        roleManager.RoleExists(trueRoles.Name);
                        UserManager.AddToRole(user.Id, trueRoles.Name);
                    }


                    return RedirectToAction("AdminAccountIndex", "Account");
                }
                AddErrors(result);
            }
            return View(model);
        }



        






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

   

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}