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
    
    public partial class GIangVien_MonHoc
    {
        public int ID { get; set; }
        public Nullable<int> GiangVienID { get; set; }
        public Nullable<int> MonHocID { get; set; }
    
        public virtual GiangVien GiangVien { get; set; }
        public virtual MonHoc MonHoc { get; set; }
    }
}
