//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CongNghePhanMemTienTien.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sv_Khoa_Detail
    {
        public int ID { get; set; }
        public Nullable<int> SinhVienID { get; set; }
        public Nullable<int> KhoaID { get; set; }
        public Nullable<int> Gv_KhoaID { get; set; }
    
        public virtual Gv_Khoa Gv_Khoa { get; set; }
        public virtual Khoa Khoa { get; set; }
    }
}