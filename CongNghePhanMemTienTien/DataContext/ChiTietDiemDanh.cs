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
    
    public partial class ChiTietDiemDanh
    {
        public int ID { get; set; }
        public Nullable<int> MaDiemDanhSV { get; set; }
        public Nullable<int> MaSV { get; set; }
        public Nullable<bool> TinhTrang { get; set; }
    
        public virtual DiemDanhSV DiemDanhSV { get; set; }
        public virtual SinhVien SinhVien { get; set; }
    }
}
