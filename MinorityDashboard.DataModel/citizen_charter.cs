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
    
    public partial class citizen_charter
    {
        public int cc_id { get; set; }
        public string name_e { get; set; }
        public string name_m { get; set; }
        public string file_path { get; set; }
        public bool isactive { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<System.DateTime> updated_date { get; set; }
    }
}
