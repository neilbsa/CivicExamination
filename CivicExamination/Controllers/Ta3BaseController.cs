using CivicExamProcedures.Context;
using CivicExamProcedures.ExternalContext;
using ExaminationEntity.BaseModels;
using ExaminationEntity.ExaminationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CivicExamination.Controllers
{
    public abstract class Ta3BaseController<TEntity> : BaseController<TEntity> where TEntity : class,IBaseEntity 
    {

        public Ta3DbConnect<Subordinate> Subordinates { get; set; }
        public Ta3DbConnect<Employee> Employees { get; set; }
        public Ta3DbConnect<Group> Group { get; set; }
        public Ta3DbConnect<GroupType> GroupType { get; set; }
        public Ta3DbConnect<GroupMember> GroupMember { get; set; }
        public Ta3DbConnect<CompanyPayrollInfo> CompanyPayInfo { get; set; }
        public DbConnect<AppraisalType> appraisalType { get; set; }
        public DbConnect<PostionAppraisalTypeLookup> AppraisalLookup { get; set; }
        public DbConnect<AppraisalEmployeeAnswerModel> AppraisalEmpModel { get; set; }
        public DbConnect<Ta3Employee> EvaluationEmployee { get; set; }
        public Ta3BaseController()
        {
            CompanyPayInfo = new Ta3DbConnect<CompanyPayrollInfo>();
            EvaluationEmployee = new DbConnect<Ta3Employee>();
            AppraisalLookup = new DbConnect<PostionAppraisalTypeLookup>();
            appraisalType = new DbConnect<AppraisalType>();
            Subordinates = new Ta3DbConnect<Subordinate>();
            Employees = new Ta3DbConnect<Employee>();
            Group = new Ta3DbConnect<Group>();
            GroupType = new Ta3DbConnect<GroupType>();
            GroupMember = new Ta3DbConnect<GroupMember>();
            AppraisalEmpModel = new DbConnect<AppraisalEmployeeAnswerModel>();
        }
    
    }
}