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
    
    public partial class PayrollCommission
    {
        public long PayrollCommissionID { get; set; }
        public Nullable<long> EmployeeID { get; set; }
        public Nullable<long> PayrollID { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<int> CutOff { get; set; }
    }
}