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
    
    public partial class PayrollJournalVoucher
    {
        public long PayrollJVID { get; set; }
        public string JournalNo { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Cutoff { get; set; }
        public Nullable<long> CompanyID { get; set; }
        public Nullable<int> PayrollFrequency { get; set; }
        public string Particulars { get; set; }
        public string PreparedBy { get; set; }
        public string CheckedBy { get; set; }
        public string ApprovedBy { get; set; }
    }
}
