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
    
    public partial class DVTTQuanLy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DVTTQuanLy()
        {
            this.SinhVienThucTaps = new HashSet<SinhVienThucTap>();
        }
    
        public int ID { get; set; }
        public string TenQuanLy { get; set; }
        public string Link { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public Nullable<int> DVTTID { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> CreatedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<int> DeletedUser { get; set; }
    
        public virtual DonViThucTap DonViThucTap { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVienThucTap> SinhVienThucTaps { get; set; }
    }
}