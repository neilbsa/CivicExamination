using CivicExamination.AuthorizationLogics;
using CivicExamination.Models;
using ClosedXML.Excel;
using ExaminationEntity.ExaminationModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CivicExamination.Controllers.Evaluation
{
    [Authorize]
    public class EvaluationController : Ta3BaseController<AppraisalBatch>
    {
        public string[] notIncludedPosition = { "F3-1", "F3-2", "F3-3", "F3-4", "F3-5", "F3-6", "F3-7" };
        private static Random random = new Random();
        public static string RandomPassword(int lenght)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            return new string(Enumerable.Repeat(chars, lenght)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public int GetDefaultType(string PositionLevel)
        {
            int id = 0;
            id = AppraisalLookup.GetDetails(x => x.PositionType == PositionLevel).AppraisalId;
            return id;
        }
        public string GetDept(long empNum)
        {
            var T = GroupMember.GetDetails(x => x.GroupTypeID == 1 && x.EmployeeID == empNum);
            if (T == null)
            {
                return "NO DEPT";
            }

            return Group.GetDetails(x => x.GroupID == T.GroupID).GroupName;
        }
        public string GetCompanyName(long empnum)
        {
            var compId = GroupMember.GetList(x => x.EmployeeID == empnum && x.GroupTypeID == 3).OrderByDescending(x => x.DateApplicable).FirstOrDefault();
            string comp = null;
            if (compId != null)
            {
                comp = CompanyPayInfo.GetDetails(x => x.CompanyPayrollInfoID == compId.GroupID).CompanyName;
            }
            else
            {
                comp = "NO COMPANY SET";
            }







            return comp;
        }
        [Authorize(Roles = "EvaluationAdmin,Administrator")]
        public override ActionResult Create()
        {
            List<Ta3Employee> ListOfEmployee = new List<Ta3Employee>();
            AppraisalBatch model = new AppraisalBatch();
            model.AppraisalDate = DateTime.Now;
            //ListOfEmployee = (from emp in Employees.GetList(x => x.IsDeleted == false)
            //                  join grpMem in GroupMember.GetList(x => x.GroupTypeID == 8).GroupBy(x => x.EmployeeID) on emp.EmployeeID equals grpMem.Key
            //                  join grp in Group.GetList(x => x.GroupTypeID == 8 && !notIncludedPosition.Contains(x.GroupName)) on grpMem.OrderByDescending(x => x.DateApplicable).FirstOrDefault().GroupID equals grp.GroupID

            //                  select new Ta3Employee()
            //                  {
            //                      EmployeeNo = emp.EmployeeNo,
            //                      EmployeePositionLevel = grp.GroupName,
            //                      Firstname = emp.FirstName,
            //                      MiddleName = emp.MiddleName,
            //                      LastName = emp.LastName,
            //                      Ta3Id = emp.EmployeeID,
            //                      IsIncluded = true,
            //                      AppraisalId = GetDefaultType(grp.GroupName)
            //                      ,AcceptanceCode = RandomPassword(5),
            //                      Status = "Pending",
            //                      Dept = GetDept(emp.EmployeeID),
            //                      DateHired = emp.DateHired

            //                  }).ToList();

            ListOfEmployee = (from emp in Employees.GetList(x => x.IsDeleted == false)
                              join grpMem in GroupMember.GetList(x => x.GroupTypeID == 8).GroupBy(x => x.EmployeeID) on emp.EmployeeID equals grpMem.Key
                              join grp in Group.GetList(x => x.GroupTypeID == 8 && !notIncludedPosition.Contains(x.GroupName)) on grpMem.OrderByDescending(x => x.DateApplicable).FirstOrDefault().GroupID equals grp.GroupID

                              select new Ta3Employee()
                              {
                                  EmployeeNo = emp.EmployeeNo,
                                  EmployeePositionLevel = grp.GroupName,
                                  Firstname = emp.FirstName,
                                  MiddleName = emp.MiddleName,
                                  LastName = emp.LastName,
                                  Ta3Id = emp.EmployeeID,
                                  IsIncluded = true,
                                  EmailAddress = emp.EMAIL,
                                  AppraisalId = GetDefaultType(grp.GroupName)
                                  ,
                                  CompanyName = GetCompanyName(emp.EmployeeID)
                                  ,
                                  AcceptanceCode = RandomPassword(5),
                                  Status = "Pending",
                                  Dept = GetDept(emp.EmployeeID),
                                  DateHired = emp.DateHired

                              }).ToList();


            var AppraisalTypes = appraisalType.GetList(x => x.IsDeleted == false).ToList();
            SelectList Selectmod = new SelectList(AppraisalTypes, "Id", "Type");
            ViewBag.AppraisalType = Selectmod;
            model.Employees = ListOfEmployee.OrderBy(x => x.EmployeeNo).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EvaluationAdmin,Administrator")]
        public override ActionResult Create(AppraisalBatch t)
        {
            t.Employees = t.Employees.Where(x => x.IsIncluded == true).ToList();
            bool IsAdded = false;

            try
            {

                base.Create(t);
                IsAdded = true;
            }
            catch
            {
                IsAdded = false;
            }
            return Json(new { status = IsAdded }, JsonRequestBehavior.DenyGet);
        }


        [Authorize(Roles = "EvaluationAdmin,Administrator")]
        public ActionResult CreateReport()
        {
            var ListOfAppraisal = from c in BaseMethods.GetList(x => x.IsDeleted != true)
                                  select new AppraisalBatchSelect()
                                  {
                                      AppraisalDate = c.AppraisalDate,
                                      Id = c.Id,
                                      Description = c.Description,
                                      IsIncluded = false
                                  };



            return View(ListOfAppraisal);
        }


        [Authorize(Roles = "EvaluationAdmin,Administrator")]
        public ActionResult CreateReportExcelExtract(int AppraisalId)
        {
            var appraisal = BaseMethods.GetDetails(x => x.Id == AppraisalId);
         
            //var includedReport = from c in items.Employees.Select(x => x.AppraisalDetails.Type).Distinct()
            //                     select c;
            var IncAppraisalTypes = from d in appraisal.Employees.Select(x => x.AppraisalDetails.Id).Distinct()
                                    select d;

            //int reportCount = includedReport.Count();



            var NewWorkbook = new XLWorkbook();
            Stream MyStream = new MemoryStream();

            //var NewWorksheet = NewWorkbook.AddWorksheet("AppraisalResultReport");


            foreach (var app in IncAppraisalTypes)
            {
                var currentAppraisal = appraisalType.GetDetails(x => x.Id == app);
                int comp = currentAppraisal.Competencies.Count() + 1;
                var NewWorksheet = NewWorkbook.AddWorksheet(currentAppraisal.Type);
                NewWorksheet.PageSetup.PagesTall = -1;
                NewWorksheet.PageSetup.FitToPages(1, 1);


                NewWorksheet.Cell(1, 9).Value=appraisal.Description;
                NewWorksheet.Range(1, 9, 1,(comp + 9)).Merge().Style.Font.SetBold();
                NewWorksheet.Cell(1, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                NewWorksheet.Cell(1, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                NewWorksheet.Cell(2, 9).Value = "Key Result Area";
                NewWorksheet.Cell(3, 9).Value = currentAppraisal.KRAPercentage + "%";
                int compCount = 10;


                foreach (var competencies in currentAppraisal.Competencies.OrderBy(x=>x.Id))
                {
                    NewWorksheet.Cell(2, compCount).Value = competencies.GroupType;
                    NewWorksheet.Cell(3, compCount).Value = competencies.Percentage + "%";

                    compCount++;
                }

                NewWorksheet.Cell(2, (9+comp)).Value = "Total Rating";
                NewWorksheet.Cell(3, (9 + comp)).Value = "100%";
                NewWorksheet.Cell(3, 1).Value = "NO";
                NewWorksheet.Cell(3, 2).Value = "CO_CODE";
                NewWorksheet.Cell(3, 3).Value = "EMP_NO";
                NewWorksheet.Cell(3, 4).Value = "BRANCH";
                NewWorksheet.Cell(3, 5).Value = "LAST_NAME";
                NewWorksheet.Cell(3, 6).Value = "FIRST_NAME";
                NewWorksheet.Cell(3, 7).Value = "MI";
                NewWorksheet.Cell(3, 8).Value = "POSITION";

                int userStartIndex = 4;
                int userNo = 1;
                foreach(var user in appraisal.Employees.Where(x=>x.AppraisalId == app))
                {
                    NewWorksheet.Cell(userStartIndex, 1).Value = userNo;
                    NewWorksheet.Cell(userStartIndex, 2).Value = user.CompanyName;
                    NewWorksheet.Cell(userStartIndex, 3).Value = user.EmployeeNo;
                    NewWorksheet.Cell(userStartIndex, 4).Value = "";
                    NewWorksheet.Cell(userStartIndex, 5).Value = user.LastName;
                    NewWorksheet.Cell(userStartIndex, 6).Value = user.Firstname;
                    NewWorksheet.Cell(userStartIndex, 7).Value = user.MiddleName != null ? user.MiddleName.Substring(0,1):"";
                    NewWorksheet.Cell(userStartIndex, 8).Value = user.EmployeePositionLevel;


                    var appraUserModel = AppraisalEmpModel.GetDetails(x=>x.EmployeeId == user.Id);

                    if(appraUserModel != null)
                    {


                        float totalKraScore = appraUserModel.KRAs.Where(c=>c.IsDeleted != true).Select(x => x.IndexScore).Sum();

                        NewWorksheet.Cell(userStartIndex, 9).Value = totalKraScore; //KRA



                        int compStart = 10;
                        float grandTotal = totalKraScore;
                        float totalCompScore = 0;
                        int totalCompetencyCount = appraUserModel.CompetencygroupAnswer.Count() + 1;
                        foreach (var competencies in appraUserModel.CompetencygroupAnswer.OrderBy(x=>x.Id))
                        {
                            float computedFlo = 0;
                              var currentCo = competencies.CompetenciesMemberAnswers.Where(x => x.IsDeleted != true && x.Rating != 0);
                            computedFlo = currentCo.Count() > 1? currentCo.Select(x => x.Rating).Average():0;
                            NewWorksheet.Cell(userStartIndex, compStart).Value = computedFlo; //competencies
                            totalCompScore += computedFlo;
                            compStart++;
                        }
                        grandTotal += totalCompScore ;
                        NewWorksheet.Cell(userStartIndex, 12).Value = (grandTotal / totalCompetencyCount); // TOTAL RATING

                    }
                    else
                    {
                        NewWorksheet.Cell(userStartIndex, 9).Value = "0";
                        NewWorksheet.Cell(userStartIndex, 10).Value = "0";
                        NewWorksheet.Cell(userStartIndex, 11).Value = "0";
                        NewWorksheet.Cell(userStartIndex, 12).Value = "0";
                    }

                    userNo++;
                    userStartIndex++;
                }
            }



            NewWorkbook.SaveAs(MyStream);
            MyStream.Position = 0;

            return File(MyStream, "application/vnd.ms-excel", "AppraisalReport.xlsx");
        }


        [Authorize(Roles = "EvaluatorUser")]
        public ActionResult EvaluatorIndex()
        {
            List<AppraisalBatch> appraisal = new List<AppraisalBatch>();
            appraisal = BaseMethods.GetList(x => x.IsDeleted != true).ToList();
            return View(appraisal);
        }
        [Authorize(Roles = "EvaluatorUser")]
        public ActionResult EvaluateeIndex(int AppraisalId)
        {
            string ManagerId = UserManager.FindByName(User.Identity.Name).CompanyId;
            List<Ta3Employee> ListOfSubordinates = new List<Ta3Employee>();
            if (!String.IsNullOrEmpty(ManagerId) || !String.IsNullOrWhiteSpace(ManagerId))
            {


                CivicExamProcedures.ExternalContext.Employee emps = new CivicExamProcedures.ExternalContext.Employee();
                emps = Employees.GetDetails(x => x.EmployeeNo == ManagerId);
                ListOfSubordinates = (from sub in Subordinates.GetList(x => x.SupervisorID == emps.EmployeeID)
                                      join emp in EvaluationEmployee.GetList(x => x.IsDeleted == false && x.AppraisalBatchId == AppraisalId) on sub.EmployeeID equals emp.Ta3Id

                                      select new Ta3Employee()
                                      {
                                          Id = emp.Id,
                                          Ta3Id = emp.Ta3Id,
                                          Firstname = emp.Firstname,
                                          MiddleName = emp.MiddleName,
                                          LastName = emp.LastName,
                                          EmployeeNo = emp.EmployeeNo,
                                          EmployeePositionLevel = emp.EmployeePositionLevel,
                                          Dept = emp.Dept,
                                          Status = emp.Status,
                                          DateHired = emp.DateHired
                                      }).ToList();
            }

            return View(ListOfSubordinates);
        }



    }
}