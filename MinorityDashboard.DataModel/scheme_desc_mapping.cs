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
    
    public partial class scheme_desc_mapping
    {
        public int scheme_des_id { get; set; }
        public int parent_scheme_id { get; set; }
        public int scheme_id_child1 { get; set; }
        public int scheme_id_child2 { get; set; }
        public Nullable<int> scheme_id_child3 { get; set; }
        public string scheme_description_e { get; set; }
        public string scheme_description_m { get; set; }
        public Nullable<bool> isactive { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<System.DateTime> updated_date { get; set; }
    }
}