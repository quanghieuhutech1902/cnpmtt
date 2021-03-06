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
    
    public partial class DiemDanhSV
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DiemDanhSV()
        {
            this.ChiTietDiemDanhs = new HashSet<ChiTietDiemDanh>();
        }
    
        public int ID { get; set; }
        public Nullable<int> MaGV { get; set; }
        public Nullable<int> MaLop { get; set; }
        public Nullable<System.DateTime> Ngay { get; set; }
        public Nullable<int> Ca { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> CreatedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<int> DeletedUser { get; set; }
        public Nullable<int> MonHocID { get; set; }
        public bool HocBu { get; set; }
        public bool IsCheck { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDiemDanh> ChiTietDiemDanhs { get; set; }
        public virtual GiangVien GiangVien { get; set; }
        public virtual Lop Lop { get; set; }
        public virtual Ca Ca1 { get; set; }
        public virtual MonHoc MonHoc { get; set; }
    }
}
