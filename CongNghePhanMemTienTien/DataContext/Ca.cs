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
    
    public partial class Ca
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ca()
        {
            this.DiemDanhSVs = new HashSet<DiemDanhSV>();
        }
    
        public int ID { get; set; }
        public string TenCa { get; set; }
        public Nullable<System.TimeSpan> ThoiGianBatDay { get; set; }
        public Nullable<System.TimeSpan> ThoiGianKetThuc { get; set; }
        public string MoTa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemDanhSV> DiemDanhSVs { get; set; }
    }
}