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
    
    public partial class LeaveCredit
    {
        public long LeaveCreditID { get; set; }
        public Nullable<long> EmployeeID { get; set; }
        public Nullable<long> LeaveTypeID { get; set; }
        public Nullable<System.DateTime> EffectiveStartDate { get; set; }
        public Nullable<System.DateTime> EffectiveEndDate { get; set; }
        public Nullable<decimal> LeaveTotal { get; set; }
        public string Remarks { get; set; }
    }
}
