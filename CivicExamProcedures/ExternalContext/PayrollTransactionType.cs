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
    
    public partial class PayrollTransactionType
    {
        public long PayrollTransactionTypeID { get; set; }
        public string Description { get; set; }
        public Nullable<int> TransactionType { get; set; }
        public string ClassObject { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string Library { get; set; }
        public Nullable<int> DefinitionLocation { get; set; }
        public Nullable<int> InputType { get; set; }
    }
}
