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
    
    public partial class GiangVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiangVien()
        {
            this.DiemDanhSVs = new HashSet<DiemDanhSV>();
            this.SinhVienThucTaps = new HashSet<SinhVienThucTap>();
            this.GIangVien_MonHoc = new HashSet<GIangVien_MonHoc>();
            this.Gv_Khoa_Detail = new HashSet<Gv_Khoa_Detail>();
            this.Sv_Gv_Detail = new HashSet<Sv_Gv_Detail>();
        }
    
        public int ID { get; set; }
        public string TenGiangVien { get; set; }
        public string MaGiangVien { get; set; }
        public string Link { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public Nullable<int> KhoaID { get; set; }
        public Nullable<int> SoLuongSinhVien { get; set; }
        public string HinhDaiDien { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> CreatedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<int> DeletedUser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemDanhSV> DiemDanhSVs { get; set; }
        public virtual Khoa Khoa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVienThucTap> SinhVienThucTaps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GIangVien_MonHoc> GIangVien_MonHoc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gv_Khoa_Detail> Gv_Khoa_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sv_Gv_Detail> Sv_Gv_Detail { get; set; }
    }
}