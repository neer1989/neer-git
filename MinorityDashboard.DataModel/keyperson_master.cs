//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MinorityDashboard.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class keyperson_master
    {
        public int keyperson_id { get; set; }
        public string person_name_e { get; set; }
        public string person_name_m { get; set; }
        public string designation_e { get; set; }
        public string designation_m { get; set; }
        public string person_image { get; set; }
        public Nullable<int> display_order { get; set; }
        public Nullable<bool> isactive { get; set; }
        public int created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<System.DateTime> updated_date { get; set; }
    }
}
