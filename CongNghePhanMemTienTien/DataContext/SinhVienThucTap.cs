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
    
    public partial class SinhVienThucTap
    {
        public int ID { get; set; }
        public Nullable<int> DVTTID { get; set; }
        public Nullable<int> MGV { get; set; }
        public Nullable<int> MSV { get; set; }
        public Nullable<double> ThoiGianTT { get; set; }
        public Nullable<System.DateTime> NgayBD { get; set; }
        public Nullable<System.DateTime> NgayKT { get; set; }
        public string NoiDungThucTap { get; set; }
        public Nullable<int> MQL { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> CreatedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<int> DeletedUser { get; set; }
    
        public virtual DonViThucTap DonViThucTap { get; set; }
        public virtual GiangVien GiangVien { get; set; }
        public virtual SinhVien SinhVien { get; set; }
        public virtual DVTTQuanLy DVTTQuanLy { get; set; }
    }
}
