using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CongNghePhanMemTienTien
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //Sinh viên -> điểm danh
            routes.MapRoute(
                name: "sinh-vien/diem-danh",
                url: "sinh-vien/diem-danh",
                defaults: new { controller = "DiemDanhs", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "sinh-vien/diem-danh-lop",
                url: "sinh-vien/diem-danh-lop-{metatitle}-{id}",
                defaults: new { controller = "DiemDanhs", action = "ClassDetail", id = UrlParameter.Optional }
            );

            //Thực tập -> đơn vị thực tập
            routes.MapRoute(
                name: "sua-don-vi-thuc-tap",
                url: "thuc-tap/sua-don-vi-thuc-tap-{id}",
                defaults: new { controller = "DonViThucTap", action = "Edit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "them-moi-don-vi-thuc-tap",
                url: "thuc-tap/them-moi-don-vi-thuc-tap",
                defaults: new { controller = "DonViThucTap", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "don-vi-thuc-tap",
                url: "thuc-tap/don-vi-thuc-tap",
                defaults: new { controller = "DonViThucTap", action = "Index", id = UrlParameter.Optional }
            );

            //Thực tập -> nhan vien quan ly thuc tap
            routes.MapRoute(
                name: "sua-thong-tin-nhan-vien-quan-ly-thuc-tap",
                url: "thuc-tap/sua-thong-tin-nhan-vien-quan-ly-thuc-tap-{id}",
                defaults: new { controller = "DVTTQuanLies", action = "Edit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "them-moi-nhan-vien-quan-ly-thuc-tap",
                url: "thuc-tap/them-moi-nhan-vien-quan-ly-thuc-tap",
                defaults: new { controller = "DVTTQuanLies", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "nhan-vien-quan-ly-thuc-tap",
                url: "thuc-tap/nhan-vien-quan-ly-thuc-tap",
                defaults: new { controller = "DVTTQuanLies", action = "Index", id = UrlParameter.Optional }
            );

            //Thực tập -> Sinh viên thực tập
            routes.MapRoute(
                name: "sua-thong-tin-sinh-vien-thuc-tap",
                url: "thuc-tap/sua-thong-tin-sinh-vien-thuc-tap-{id}",
                defaults: new { controller = "SinhVienThucTaps", action = "Edit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "thuc-tap/them-moi-sinh-vien-thuc-tap",
                url: "thuc-tap/them-moi-sinh-vien-thuc-tap",
                defaults: new { controller = "SinhVienThucTaps", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "thuc-tap/sinh-vien-thuc-tap",
                url: "thuc-tap/sinh-vien-thuc-tap",
                defaults: new { controller = "SinhVienThucTaps", action = "Index", id = UrlParameter.Optional }
            );
            

            //Thực tập -> Giảng viên HD TT
            routes.MapRoute(
                name: "GetAllSVTT",
                url: "thuc-tap/{metatitle}/danh-sach-sinh-vien-{id}",
                defaults: new { controller = "GiangVienThucTaps", action = "GetAll", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "thuc-tap/them-moi-sinh-vien-thuc-tap",
            //    url: "thuc-tap/them-moi-sinh-vien-thuc-tap",
            //    defaults: new { controller = "GiangVienThucTaps", action = "Create", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
                name: "thuc-tap/giang-vien-huong-dan",
                url: "thuc-tap/giang-vien-phu-trach-thuc-tap",
                defaults: new { controller = "GiangVienThucTaps", action = "Index", id = UrlParameter.Optional }
            );

            //Quan Ly -> Khoa
            routes.MapRoute(
                name: "sua-khoa",
                url: "quan-ly/sua-khoa-{id}",
                defaults: new { controller = "Khoas", action = "Edit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "them-moi-khoa",
                url: "quan-ly/them-moi-khoa",
                defaults: new { controller = "Khoas", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "khoa",
                url: "quan-ly/khoa",
                defaults: new { controller = "Khoas", action = "Index", id = UrlParameter.Optional }
            );

            //Quan ly -> Phụ Huynh
            routes.MapRoute(
               name: "sua-thong-tin-phu-huynh",
               url: "quan-ly/sua-thong-tin-phu-huynh-{id}",
               defaults: new { controller = "PhuHuynhs", action = "Edit", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-phu-hunh",
                url: "quan-ly/them-moi-phu-huynh",
                defaults: new { controller = "PhuHuynhs", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "phu-huynh",
                url: "quan-ly/phu-huynh",
                defaults: new { controller = "PhuHuynhs", action = "Index", id = UrlParameter.Optional }
            );

            //Quan ly -> GIẢNG VIÊN
            routes.MapRoute(
               name: "sua-thong-tin-giang-vien",
               url: "quan-ly/sua-thong-tin-giang-vien-{id}",
               defaults: new { controller = "GiangViens", action = "Edit", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-giang-vien",
                url: "quan-ly/them-moi-giang-vien",
                defaults: new { controller = "GiangViens", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "gian-vien",
                url: "quan-ly/giang-vien",
                defaults: new { controller = "GiangViens", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "khoa/gian-vien",
                url: "khoa/{metatile}/danh-sach-giang-vien-{id}",
                defaults: new { controller = "GiangViens", action = "Index", id = UrlParameter.Optional }
            );

            //Quan ly -> lớp
            routes.MapRoute(
               name: "sua-thong-tin-lop",
               url: "quan-ly/sua-thong-tin-lop-{id}",
               defaults: new { controller = "Lops", action = "Edit", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-lop",
                url: "quan-ly/them-moi-lop",
                defaults: new { controller = "Lops", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "lop",
                url: "quan-ly/lop",
                defaults: new { controller = "Lops", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "danh-sach-lop-theo-khoa",
                url: "khoa/{metatile}/danh-sach-lop-{id}",
                defaults: new { controller = "Lops", action = "Index", id = UrlParameter.Optional }
            );

            //Quan ly -> nganh
            routes.MapRoute(
               name: "sua-thong-tin-nganh",
               url: "quan-ly/sua-thong-tin-nganh-{id}",
               defaults: new { controller = "Nganhs", action = "Edit", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-nganh",
                url: "quan-ly/them-moi-nganh",
                defaults: new { controller = "Nganhs", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "nganh",
                url: "quan-ly/nganh",
                defaults: new { controller = "Nganhs", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "khoa/nganh",
                url: "khoa/{metatile}/danh-sach-nganh-{id}",
                defaults: new { controller = "Nganhs", action = "Index", id = UrlParameter.Optional }
            );

            //Quan ly -> chuyenh nganh
            routes.MapRoute(
               name: "sua-thong-tin-chuyen-nganh",
               url: "quan-ly/sua-thong-tin-chuyen-nganh-{id}",
               defaults: new { controller = "ChuyenNganhs", action = "Edit", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-chuyen-nganh",
                url: "quan-ly/them-moi-chuyen-nganh",
                defaults: new { controller = "ChuyenNganhs", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "chuyen-nganh",
                url: "quan-ly/chuyen-nganh",
                defaults: new { controller = "ChuyenNganhs", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "nganh/chuyen-nganh",
               url: "nganh/{metatile}/danh-sach-chuyen-nganh-{id}",
               defaults: new { controller = "ChuyenNganhs", action = "Index", id = UrlParameter.Optional }
           );

            //Quan ly -> sinh viên
            routes.MapRoute(
               name: "sua-thong-tin-sinh-vien",
               url: "quan-ly/sua-thong-tin-sinh-vien-{id}",
               defaults: new { controller = "SinhViens", action = "Edit", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-sinh-vien",
                url: "quan-ly/them-moi-sinh-vien",
                defaults: new { controller = "SinhViens", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "sinh-vien",
                url: "quan-ly/sinh-vien",
                defaults: new { controller = "SinhViens", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "lop/danh-sach-sinh-vien",
                url: "lop/{metatile}/danh-sach-sinh-vien-{id}",
                defaults: new { controller = "SinhViens", action = "Index", id = UrlParameter.Optional }
            );


            //Quan ly -> Môn học
            routes.MapRoute(
               name: "sua-thong-tin-mon-hoc",
               url: "quan-ly/sua-thong-tin-mon-hoc-{id}",
               defaults: new { controller = "MonHocs", action = "Edit", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-mon-hoc",
                url: "quan-ly/them-moi-mon-hoc",
                defaults: new { controller = "MonHocs", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "mon-hoc",
                url: "quan-ly/mon-hoc",
                defaults: new { controller = "MonHocs", action = "Index", id = UrlParameter.Optional }
            );


            //THỜI KHOA BIỂU -> THỜI KHÓA BIỂU
            routes.MapRoute(
               name: "sua-thong-tin-thoi-khoa-bieu",
               url: "thoi-khoa-bieu/sua-thong-tin-thoi-khoa-bieu-{id}",
               defaults: new { controller = "DiemDanhSVs", action = "Edit", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-thoi-khoa-bieu",
                url: "thoi-khoa-bieu/them-moi-thoi-khoa-bieu",
                defaults: new { controller = "DiemDanhSVs", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "thoi-khoa-bieu",
                url: "thoi-khoa-bieu/thoi-khoa-bieu",
                defaults: new { controller = "DiemDanhSVs", action = "Index", id = UrlParameter.Optional }
            );

            //THỜI KHOA BIỂU -> THỜI KHÓA BIỂU HỌC BÙ
            routes.MapRoute(
               name: "sua-thong-tin-thoi-khoa-bieu-hoc-bu",
               url: "thoi-khoa-bieu/sua-thoi-khoa-bieu-hoc-bu-{id}",
               defaults: new { controller = "DiemDanhSVs", action = "EditHB", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-thoi-khoa-bieu-hoc-bu",
                url: "thoi-khoa-bieu/them-moi-thoi-khoa-bieu-hoc-bu",
                defaults: new { controller = "DiemDanhSVs", action = "CreateHB", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "thoi-khoa-bieu-hoc-bu",
                url: "thoi-khoa-bieu/thoi-khoa-bieu-hoc-bu",
                defaults: new { controller = "DiemDanhSVs", action = "IndexHB", id = UrlParameter.Optional }
            );

            //THỜI KHOA BIỂU -> CA HỌC
            routes.MapRoute(
               name: "sua-thong-tin-ca-hoc",
               url: "thoi-khoa-bieu/sua-thong-tin-ca-hoc-{id}",
               defaults: new { controller = "Cas", action = "Edit", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-ca-hoc",
                url: "thoi-khoa-bieu/them-moi-ca-hoc",
                defaults: new { controller = "Cas", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ca-hoc",
                url: "thoi-khoa-bieu/ca-hoc",
                defaults: new { controller = "Cas", action = "Index", id = UrlParameter.Optional }
            );



            //THÔNG BÁO -> DANH MỤC THÔNG BÁO
            routes.MapRoute(
               name: "sua-thong-tin-loai-thong-bao",
               url: "thong-bao/sua-thong-tin-loai-thong-bao-{id}",
               defaults: new { controller = "DanhMucThongBaos", action = "Edit", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "them-moi-loai-thong-bao",
                url: "thong-bao/them-moi-loai-thong-bao",
                defaults: new { controller = "DanhMucThongBaos", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "loai-thong-bao",
                url: "thong-bao/loai-thong-bao",
                defaults: new { controller = "DanhMucThongBaos", action = "Index", id = UrlParameter.Optional }
            );




            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
