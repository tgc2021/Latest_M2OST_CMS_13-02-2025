//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace m2ostnext
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_cms_users
    {
        public int ID_USER { get; set; }
        public int ID_ROLE { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string STATUS { get; set; }
        public System.DateTime UPDATED_DATE_TIME { get; set; }
        public Nullable<int> cmd_user_type { get; set; }
        public string employee_id { get; set; }
        public string employee_name { get; set; }
    }
}
