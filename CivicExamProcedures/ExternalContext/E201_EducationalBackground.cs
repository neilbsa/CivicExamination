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
    
    public partial class E201_EducationalBackground
    {
        public long E201EducationalBackgroundID { get; set; }
        public long EmployeeID { get; set; }
        public Nullable<int> EducationLevel { get; set; }
        public string School { get; set; }
        public string SchoolAddress { get; set; }
        public string Course { get; set; }
        public string Honors { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
