//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CivicExamProcedures.ExternalContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public long EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string EmployeeNo { get; set; }
        public byte[] Picture { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> ScheduleID { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> DateHired { get; set; }
        public Nullable<System.DateTime> RegularizationDate { get; set; }
        public Nullable<long> UserRoleID { get; set; }
        public Nullable<System.DateTime> ResignationDate { get; set; }
        public string EMAIL { get; set; }
        public Nullable<long> LeaveProfileID { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<short> Gender { get; set; }
        public Nullable<int> Classification { get; set; }
        public Nullable<System.DateTime> DateSeniorityReference { get; set; }
    }
}