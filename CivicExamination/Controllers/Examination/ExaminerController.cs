using CivicExamination.AuthorizationLogics;
using CivicExamination.Models;
using CivicExamProcedures.Context;
using ExaminationEntity.ExaminationModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CivicExamination.Controllers.Examination
{
    [AuthorizeRequireBranch(Roles = "ExamineeAdministrator,ExaminationAdministrator,Administrator")]
    public class ExaminerController : CompanyTransactionController<UserProfile>
    {
 
        public ApplicationContext context { get; set; }

        [AuthorizeRequireBranch]
        public override ActionResult GetView(string viewName)
        {
            return base.GetView(viewName);
        }

        public ExaminerController()
        {
            context = new ApplicationContext();
        }



        public ActionResult EmploymentDatasheetReport()
        {
            UserProfile mod = new UserProfile();
            mod = BaseMethods.GetDetails(x => x.Id == 4);
            return View(mod);
        }


        public ActionResult RenderEmploymentDatasheet()
        {


            Dictionary<string, string> cookieCollection = new Dictionary<string, string>();
            foreach (var key in Request.Cookies.AllKeys)
            {
                cookieCollection.Add(key, Request.Cookies.Get(key).Value);
            }
            string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";

            return new ViewAsPdf("EmploymentDatasheetReport")
            {
                Cookies = cookieCollection,
                PageSize = Size.Letter,
                CustomSwitches = footer

            };
        }


        public ActionResult RegisterExaminer()
        {
            RegisterViewModel mod = new RegisterViewModel();
           
            ApplicationUser user = new ApplicationUser();

            mod.BirthDate = DateTime.Now;
            //user.BirthDate = DateTime.Now;
       
            user.CompanyId = "FIXEDUSEREXAMINER";
            mod.User = user;
            return View(mod);
        }

        [AuthorizeRequireBranch]
        public ActionResult UserEmploymentDataSheet()
        {
            UserProfile mod = new UserProfile();
            mod = UserManager.FindByName(User.Identity.Name).UserDetail;
            return View("EmploymentDataSheet", mod);
        }


        public ActionResult EmploymentDataSheet(int Id)
        {
            UserProfile mod = new UserProfile();
            mod = BaseMethods.GetDetails(x => x.Id == Id);
            return View(mod);
        }

     


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRequireBranch]
        public ActionResult EmploymentDataSheet(UserProfile ent)
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
                return View(ent);
            }
            else
            {
                UserProfile currentMod = new UserProfile();
                currentMod = BaseMethods.GetDetails(x => x.Id == ent.Id);

                currentMod.Firstname = ent.Firstname;
                currentMod.Middlename = ent.Middlename;
                currentMod.Lastname = ent.Lastname;
                currentMod.EmailAddress = ent.EmailAddress;
                currentMod.HomeContact = ent.HomeContact;
                currentMod.Gender = ent.Gender;

                currentMod.Nickname = ent.Nickname;
                currentMod.PresentAddress = ent.PresentAddress;
                currentMod.PermanentAddress = ent.PermanentAddress;
                currentMod.MobileContact = ent.MobileContact;
                currentMod.BirthDate = ent.BirthDate;
                currentMod.BirthPlace = ent.BirthPlace;
                currentMod.Citizenship = ent.Citizenship;
                currentMod.Age = ent.Age;
                currentMod.Height = ent.Height;
                currentMod.Weight = ent.Weight;
                currentMod.Religion = ent.Religion;
                currentMod.CivilStatus = ent.CivilStatus;
                currentMod.SpouseName = ent.SpouseName;
                currentMod.DependentNo = ent.DependentNo;
                currentMod.SpouseOccupation = ent.SpouseOccupation;
                currentMod.Tin = ent.Tin;
                currentMod.SSS = ent.SSS;
                currentMod.Philhealth = ent.Philhealth;
                currentMod.Pagibig = ent.Pagibig;
                currentMod.IsLicensedDriver = ent.IsLicensedDriver;
                currentMod.DriverLicenseValidity = ent.DriverLicenseValidity;
                currentMod.IsHasPassport = ent.IsHasPassport;
                currentMod.PassportNo = ent.PassportNo;
                currentMod.PassportValitdity = ent.PassportValitdity;
                currentMod.IsHasCriminalCase = ent.IsHasCriminalCase;
                currentMod.CriminalCaseNature = ent.CriminalCaseNature;
                currentMod.ApplyingReason = ent.ApplyingReason;


                if (currentMod.DesiredEmploymentDetails == null)
                {
                    DesiredEmployment desireMod = new DesiredEmployment();
                    currentMod.DesiredEmploymentDetails = desireMod;
                }

                currentMod.DesiredEmploymentDetails.PositionAppliedFor = ent.DesiredEmploymentDetails.PositionAppliedFor;
                currentMod.DesiredEmploymentDetails.DesiredSalary = ent.DesiredEmploymentDetails.DesiredSalary;
                currentMod.DesiredEmploymentDetails.OpeningSource = ent.DesiredEmploymentDetails.OpeningSource;
                currentMod.DesiredEmploymentDetails.IsPrevApplicant = ent.DesiredEmploymentDetails.IsPrevApplicant;
                currentMod.DesiredEmploymentDetails.PrevApplication = ent.DesiredEmploymentDetails.PrevApplication;
                currentMod.DesiredEmploymentDetails.PrevPositionApplied = ent.DesiredEmploymentDetails.PrevPositionApplied;
                currentMod.DesiredEmploymentDetails.IsPrevHired = ent.DesiredEmploymentDetails.IsPrevHired;
                currentMod.DesiredEmploymentDetails.PreDateLeaved = ent.DesiredEmploymentDetails.PreDateLeaved;
                currentMod.DesiredEmploymentDetails.PrevDateHired = ent.DesiredEmploymentDetails.PrevDateHired;
                currentMod.DesiredEmploymentDetails.PrevPosition = ent.DesiredEmploymentDetails.PrevPosition;
                currentMod.DesiredEmploymentDetails.CanAssignedToProvince = ent.DesiredEmploymentDetails.CanAssignedToProvince;
                currentMod.DesiredEmploymentDetails.DesiredProvince = ent.DesiredEmploymentDetails.DesiredProvince;
                currentMod.DesiredEmploymentDetails.IsPresentlyEmployed = ent.DesiredEmploymentDetails.IsPresentlyEmployed;
                currentMod.DesiredEmploymentDetails.DateAvailable = ent.DesiredEmploymentDetails.DateAvailable;



                if (currentMod.EmergencyContactDetails == null)
                {
                    EmergencyNotification emerMod = new EmergencyNotification();
                    currentMod.EmergencyContactDetails = emerMod;
                }


                currentMod.EmergencyContactDetails.Name = ent.EmergencyContactDetails.Name;
                currentMod.EmergencyContactDetails.Relationship = ent.EmergencyContactDetails.Relationship;
                currentMod.EmergencyContactDetails.Address = ent.EmergencyContactDetails.Address;
                currentMod.EmergencyContactDetails.ContactNumber = ent.EmergencyContactDetails.ContactNumber;
                currentMod.EmergencyContactDetails.MobileNumber = ent.EmergencyContactDetails.MobileNumber;


                if (ent.CharacterReferences != null)
                {
                    foreach (var item in ent.CharacterReferences.ToList())
                    {
                        CharacterReference charRef = new CharacterReference();
                        charRef = currentMod.CharacterReferences.Where(x => x.Id == item.Id).FirstOrDefault();

                        if (charRef != null)
                        {
                            charRef.Fullname = item.Fullname;
                            charRef.Company = item.Company;
                            charRef.ContactNumber = item.ContactNumber;
                            charRef.EmailAddress = item.EmailAddress;
                        }
                        else
                        {
                            currentMod.CharacterReferences.Add(item);
                        }

                    }


                    var qu = from c in currentMod.CharacterReferences.Where(x => x.IsDeleted != true).ToList()
                             where !(from o in ent.CharacterReferences
                                     select o.Id).Contains(c.Id)
                             select c;
                    foreach (var foredelete in qu.ToList())
                    {
                        foredelete.IsDeleted = true;
                    }
                }



                if (ent.EmploymentHistories != null)
                {
                    foreach (var item in ent.EmploymentHistories.ToList())
                    {
                        EmploymentHistory empHist = new EmploymentHistory();
                        empHist = currentMod.EmploymentHistories.Where(x => x.Id == item.Id).FirstOrDefault();

                        if (empHist != null)
                        {
                            empHist.CompanyName = item.CompanyName;
                            empHist.Address = item.Address;
                            empHist.ContactNumber = item.ContactNumber;
                            empHist.InitialPosition = item.InitialPosition;


                            empHist.LastPosition = item.LastPosition;
                            empHist.StartingSalary = item.StartingSalary;

                            empHist.LastSalary = item.LastSalary;
                            empHist.EmploymentDate = item.EmploymentDate;
                            empHist.Resposibilities = item.Resposibilities;
                            empHist.ReasonForLeaving = item.ReasonForLeaving;

                        }
                        else
                        {
                            currentMod.EmploymentHistories.Add(item);
                        }

                    }


                    var qu = from c in currentMod.EmploymentHistories.Where(x => x.IsDeleted != true).ToList()
                             where !(from o in ent.EmploymentHistories
                                     select o.Id).Contains(c.Id)
                             select c;
                    foreach (var foredelete in qu.ToList())
                    {
                        foredelete.IsDeleted = true;
                    }
                }



                if (ent.EducationHistories != null)
                {
                    foreach (var item in ent.EducationHistories.ToList())
                    {
                        EducationAttainment educHist = new EducationAttainment();
                        educHist = currentMod.EducationHistories.Where(x => x.Id == item.Id).FirstOrDefault();

                        if (educHist != null)
                        {
                            educHist.Type = item.Type;
                            educHist.DateAttendedFrom = item.DateAttendedFrom;
                            educHist.DateAttendedTo = item.DateAttendedTo;
                            educHist.NameOfSchool = item.NameOfSchool;
                            educHist.Address = item.Address;


                        }
                        else
                        {
                            currentMod.EducationHistories.Add(item);
                        }

                    }


                    var qu = from c in currentMod.EducationHistories.Where(x => x.IsDeleted != true).ToList()
                             where !(from o in ent.EducationHistories
                                     select o.Id).Contains(c.Id)
                             select c;
                    foreach (var foredelete in qu.ToList())
                    {
                        foredelete.IsDeleted = true;
                    }
                }



                if (ent.LicenseExaminations != null)
                {
                    foreach (var item in ent.LicenseExaminations.ToList())
                    {
                        LicenseExamination licHistory = new LicenseExamination();
                        licHistory = currentMod.LicenseExaminations.Where(x => x.Id == item.Id).FirstOrDefault();

                        if (licHistory != null)
                        {
                            licHistory.LicenseNumber = item.LicenseNumber;
                            licHistory.LicenseName = item.LicenseName;
                            licHistory.Expiration = item.Expiration;
                        }
                        else
                        {
                            currentMod.LicenseExaminations.Add(item);
                        }

                    }


                    var qu = from c in currentMod.LicenseExaminations.Where(x => x.IsDeleted != true).ToList()
                             where !(from o in ent.LicenseExaminations
                                     select o.Id).Contains(c.Id)
                             select c;
                    foreach (var foredelete in qu.ToList())
                    {
                        foredelete.IsDeleted = true;
                    }
                }
                BaseMethods.UpdatetoContext(x => x.Id, currentMod);
            }



            return View(ent);
        }


        public override JsonResult UpdateJson(UserProfile ent)
        {
            return base.UpdateJson(ent);
        }

        [HttpPost]
        public JsonResult UpdateUserStatus(int UserId, string Status)
        {
            var mod = BaseMethods.GetDetails(x => x.Id == UserId);
            mod.UserStatus = Status;
            BaseMethods.UpdatetoContext(x => x.Id, mod);
            return Json(new { success = true }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterExaminer(RegisterViewModel model)
        {
            //model.User.CompanyId = "FOREXAMINERUSER";
            if (ModelState.IsValid)
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                IEnumerable<Company> companies = new List<Company>();
                model.User.UserDetail.CompanyId = SelectedCompany.Id;
                model.User.UserDetail.IsAdministrator = false;
                model.User.UserDetail.BirthDate = model.BirthDate;
                model.User.UserDetail.UserStatus = "Active";
               
                var user = new ApplicationUser { UserName = model.Username, Email = model.User.UserDetail.EmailAddress,CompanyId = model.User.CompanyId };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    var td = BaseMethods.AddtoContext(model.User.UserDetail);
                    user.UserDetailId = td.Id;
                    var updateResult = UserManager.Update(user);
                    dbUserComp.AddtoContext(new UserCompany() { UserId = int.Parse(user.UserDetailId.ToString()), CompanyId = SelectedCompany.Id, IsDeleted = false });
                    UserManager.AddToRole(user.Id, "Common");
                    return RedirectToAction("Index", "Examiner");
                }
                else
                {
                    AddErrors(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ValidateUserForRegistration(RegisterViewModel model)
        {


            var d = BaseMethods.GetList(x =>
                x.BirthDate == model.BirthDate &&
                x.Firstname.ToLower().Trim() == model.User.UserDetail.Firstname.ToLower().Trim() &&
                x.Lastname.ToLower().Trim() == model.User.UserDetail.Lastname.ToLower().Trim()
                && x.Middlename.ToLower().Trim() == model.User.UserDetail.Middlename.ToLower().Trim())
                .Select(x => x.ScheduledExaminations);


            List<ExistingAccountViewModel> ListOfExams = new List<ExistingAccountViewModel>();


            foreach (var item in d)
            {
                foreach (var exams in item)
                {
                    ExistingAccountViewModel mod = new ExistingAccountViewModel()
                    {
                        DateFinish = exams.FinishDate,
                        DateScheduled = exams.Schedule.StartDate,
                        EndScheduled = exams.Schedule.ExpirationDate,
                        ExamName = exams.Schedule.Examination.ExamTemplateName,
                        Months = exams.FinishDate != null ? Math.Truncate(DateTime.Now.Subtract(exams.FinishDate.GetValueOrDefault()).TotalDays / 30) : 0
                    };
                    ListOfExams.Add(mod);
                }
            }

            if (ListOfExams.Count() > 0)
            {
                return View(ListOfExams);
            }
            else
            {
                return null;
            }
        }



        [HttpPost]
        public JsonResult getUserFullnames()
        {
            List<UserProfile> users = new List<UserProfile>();
            users = BaseMethods.GetList(x => x.IsDeleted != true && x.CompanyId == SelectedCompany.Id && x.IsAdministrator == false).ToList();
            var json = from t in users.Where(x => "Active".Equals(x.UserStatus))
                       select new { userId = t.Id, FullName = String.Format("{0}", t.Fullname) };
            return Json(json, JsonRequestBehavior.DenyGet);
        }



        public override void UpdateExtraProcess(UserProfile oldT, UserProfile newT)
        {
            oldT.Firstname = newT.Firstname;
            oldT.Middlename = newT.Middlename;
            oldT.Lastname = newT.Lastname;
            oldT.EmailAddress = newT.EmailAddress;
            oldT.HomeContact = newT.HomeContact;
            oldT.Gender = newT.Gender;
            oldT.BirthDate = newT.BirthDate;

            base.UpdateExtraProcess(oldT, newT);
        }

        public override ActionResult Index()
        {
            List<UserProfile> compList = new List<UserProfile>();
            compList = BaseMethods.GetList(x => x.CompanyDetails.Id == SelectedCompany.Id && x.IsAdministrator == false).ToList();
            return View(compList);
        }





        private ModelStateDictionary ValidateViewModel(UserProfile model)
        {

            ModelStateDictionary modelState = new ModelStateDictionary();

            if (String.IsNullOrEmpty(model.PresentAddress) || String.IsNullOrWhiteSpace(model.PresentAddress))
            {
                modelState.AddModelError("PresentAddress", "Present address is required");
            }
            if (String.IsNullOrEmpty(model.PermanentAddress) || String.IsNullOrWhiteSpace(model.PermanentAddress))
            {
                modelState.AddModelError("PermanentAddress", "Permanent address is required");
            }

            if (String.IsNullOrEmpty(model.HomeContact) || String.IsNullOrWhiteSpace(model.HomeContact))
            {
                modelState.AddModelError("HomeContact", "HomeContact is required");
            }
            if (String.IsNullOrEmpty(model.MobileContact) || String.IsNullOrWhiteSpace(model.MobileContact))
            {
                modelState.AddModelError("MobileContact", "MobileContact is required");
            }
            if (String.IsNullOrEmpty(model.BirthPlace) || String.IsNullOrWhiteSpace(model.BirthPlace))
            {
                modelState.AddModelError("BirthPlace", "BirthPlace is required");
            }

            if (String.IsNullOrEmpty(model.Citizenship) || String.IsNullOrWhiteSpace(model.Citizenship))
            {
                modelState.AddModelError("Citizenship", "Citizenship is required");
            }
            if (String.IsNullOrEmpty(model.Religion) || String.IsNullOrWhiteSpace(model.Religion))
            {
                modelState.AddModelError("Religion", "Religion is required");
            }
            if (String.IsNullOrEmpty(model.CivilStatus) || String.IsNullOrWhiteSpace(model.CivilStatus))
            {
                modelState.AddModelError("CivilStatus", "CivilStatus is required");
            }

            if (model.CivilStatus == "Married")
            {
                if (String.IsNullOrEmpty(model.SpouseName) || String.IsNullOrWhiteSpace(model.SpouseName))
                {
                    modelState.AddModelError("SpouseName", "SpouseName is required");
                }
                if (String.IsNullOrEmpty(model.SpouseOccupation) || String.IsNullOrWhiteSpace(model.SpouseOccupation))
                {
                    modelState.AddModelError("SpouseOccupation", "SpouseOccupation is required");
                }
            }


            if (model.IsLicensedDriver)
            {
                if (!model.DriverLicenseValidity.HasValue)
                {
                    modelState.AddModelError("DriverLicenseValidity", "Driver License Validity is required");
                }
            }

            if (model.IsHasPassport)
            {
                if (String.IsNullOrEmpty(model.PassportNo) || String.IsNullOrWhiteSpace(model.PassportNo))
                {
                    modelState.AddModelError("PassportNo", "Passport Number is required");
                }
                if (!model.PassportValitdity.HasValue)
                {
                    modelState.AddModelError("PassportValitdity", "Passport Valitdity is required");
                }
            }



            if (model.IsHasCriminalCase)
            {
                if (String.IsNullOrEmpty(model.CriminalCaseNature) || String.IsNullOrWhiteSpace(model.CriminalCaseNature))
                {
                    modelState.AddModelError("CriminalCaseNature", "Please State your Criminal case");
                }
            }




            if (String.IsNullOrEmpty(model.DesiredEmploymentDetails.PositionAppliedFor) || String.IsNullOrWhiteSpace(model.DesiredEmploymentDetails.PositionAppliedFor))
            {
                modelState.AddModelError("DesiredEmploymentDetails.PositionAppliedFor", "Applying Position is required");
            }
            if (String.IsNullOrEmpty(model.DesiredEmploymentDetails.OpeningSource) || String.IsNullOrWhiteSpace(model.DesiredEmploymentDetails.OpeningSource))
            {
                modelState.AddModelError("DesiredEmploymentDetails.OpeningSource", "Opening Source is required");
            }
            if (model.DesiredEmploymentDetails.IsPrevApplicant)
            {
                if (!model.DesiredEmploymentDetails.PrevApplication.HasValue)
                {
                    modelState.AddModelError("DesiredEmploymentDetails.PrevApplication", "Application Date is required");
                }
                if (String.IsNullOrEmpty(model.DesiredEmploymentDetails.PrevPositionApplied) || String.IsNullOrWhiteSpace(model.DesiredEmploymentDetails.PrevPositionApplied))
                {
                    modelState.AddModelError("DesiredEmploymentDetails.PrevPositionApplied", "Previous Position Applied is required");
                }
            }

            if (model.DesiredEmploymentDetails.IsPrevHired)
            {
                if (!model.DesiredEmploymentDetails.PrevDateHired.HasValue)
                {
                    modelState.AddModelError("DesiredEmploymentDetails.PrevDateHired", "Date is required");
                }
                if (!model.DesiredEmploymentDetails.PreDateLeaved.HasValue)
                {
                    modelState.AddModelError("DesiredEmploymentDetails.PreDateLeaved", "Date is required");
                }
                if (String.IsNullOrEmpty(model.DesiredEmploymentDetails.PrevPosition) || String.IsNullOrWhiteSpace(model.DesiredEmploymentDetails.PrevPosition))
                {
                    modelState.AddModelError("DesiredEmploymentDetails.PrevPosition", "Previous Position is required");
                }
            }


            if (model.DesiredEmploymentDetails.CanAssignedToProvince)
            {
                if (String.IsNullOrEmpty(model.DesiredEmploymentDetails.DesiredProvince) || String.IsNullOrWhiteSpace(model.DesiredEmploymentDetails.DesiredProvince))
                {
                    modelState.AddModelError("DesiredEmploymentDetails.PrevPosition", "Previous Position is required");
                }
            }

            if (model.DesiredEmploymentDetails.IsPresentlyEmployed)
            {
                if (!model.DesiredEmploymentDetails.DateAvailable.HasValue)
                {
                    modelState.AddModelError("DesiredEmploymentDetails.DateAvailable", "Date is required");
                }
            }

            if (String.IsNullOrEmpty(model.EmergencyContactDetails.Name) || String.IsNullOrWhiteSpace(model.EmergencyContactDetails.Name))
            {
                modelState.AddModelError("EmergencyContactDetails.Name", "Name is required");
            }
            if (String.IsNullOrEmpty(model.EmergencyContactDetails.Relationship) || String.IsNullOrWhiteSpace(model.EmergencyContactDetails.Relationship))
            {
                modelState.AddModelError("EmergencyContactDetails.Relationship", "Relationship is required");
            }
            if (String.IsNullOrEmpty(model.EmergencyContactDetails.Address) || String.IsNullOrWhiteSpace(model.EmergencyContactDetails.Address))
            {
                modelState.AddModelError("EmergencyContactDetails.Address", "Address is required");
            }
            if (String.IsNullOrEmpty(model.EmergencyContactDetails.ContactNumber) || String.IsNullOrWhiteSpace(model.EmergencyContactDetails.ContactNumber))
            {
                modelState.AddModelError("EmergencyContactDetails.ContactNumber", "ContactNumber is required");
            }
            if (String.IsNullOrEmpty(model.EmergencyContactDetails.MobileNumber) || String.IsNullOrWhiteSpace(model.EmergencyContactDetails.MobileNumber))
            {
                modelState.AddModelError("EmergencyContactDetails.MobileNumber", "MobileNumber is required");
            }


            return modelState;

        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }



}