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
    
    public partial class PhuHuynh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhuHuynh()
        {
            this.SinhViens = new HashSet<SinhVien>();
        }
    
        public int ID { get; set; }
        public string HoTenCha { get; set; }
        public string HoTenMe { get; set; }
        public string Email { get; set; }
        public string DiDong { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> CreatedUser { get; set; }
        public Nullable<int> UpdatedUser { get; set; }
        public Nullable<int> DeletedUser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVien> SinhViens { get; set; }
    }
}
