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
    
    public partial class Leaf
    {
        public long LeaveID { get; set; }
        public Nullable<long> LeaveTypeID { get; set; }
        public Nullable<long> EmployeeID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> MinutesLeave { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<long> RequestedBy { get; set; }
        public string RequestRemarks { get; set; }
        public Nullable<System.DateTime> ChangeStatusDate { get; set; }
        public Nullable<long> ChangeStatusBy { get; set; }
        public string ChangeStatusRemarks { get; set; }
        public Nullable<short> LeaveApplication { get; set; }
        public Nullable<bool> WithPay { get; set; }
        public Nullable<short> Status { get; set; }
        public Nullable<long> LeaveGrantID { get; set; }
        public Nullable<int> YearDeducted { get; set; }
        public Nullable<short> LeaveCount { get; set; }
    }
}