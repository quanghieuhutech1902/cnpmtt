using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CongNghePhanMemTienTien.DataContext;
using System.Net.Mail;
using System.Text;
using System.Net.Mime;

namespace CongNghePhanMemTienTien.Controllers
{
    public class GiangVienThucTapsController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();
        public ActionResult Index()
        {
            var sinhVienThucTaps = db.SinhVienThucTaps.Include(s => s.GiangVien).Select(p => p.GiangVien).Distinct(); ;
            return View(sinhVienThucTaps.ToList());
        }
        public ActionResult GetAll(int id)
        {
            var item = db.GiangViens.Find(id);
            ViewBag.GiangVien = item.TenGiangVien + " - " + item.MaGiangVien;
            var sinhVienThucTaps = db.SinhVienThucTaps.Include(s => s.DonViThucTap).Include(s => s.SinhVien).Include(s => s.DVTTQuanLy).Where(s=>s.MGV==id);
            return View(sinhVienThucTaps.ToList());
        }
        public ActionResult SendMaiGV(int id)
        {

            try
            {
                List<SinhVienThucTap> item = db.SinhVienThucTaps.Where(s => s.MGV == id).ToList();
                GiangVien gv = db.GiangViens.Find(id);
                if (SendMail(item, gv) ==true)
                {
                    return RedirectToAction("GuiMailThanhCong");
                }
                else
                {
                    return RedirectToAction("GuiMailThatBai");
                }
            }
            catch
            {
                return RedirectToAction("GuiMailThatBai");
            } 
        }
        public bool SendMail(List<SinhVienThucTap> svtp, GiangVien gv)
        {

            string html = "";

            html = "<h2><a href='#'>KHOA CNTT GỬI BẠN THÔNG TIN THỰC TẬP<a/></h2>";
            html += "<div style='font-size:14px; line-height:2; color:#000;background:#fff'>";
            html += "<p style='padding: 5px; font - style:italic; color:#900'>NGÀY GỬI: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "</p>";
            html += "<h2 style='border-bottom:2px solid #d58003;padding-bottom:0px'>THÔNG TIN CHI TIẾT</h2>";
            html += "<p style='margin-bottom:10px'>Gửi Quý Thầy/Cô: " + gv.TenGiangVien.ToUpper() +" - "+ gv.MaGiangVien.ToUpper() + "</p>";
            html += "<b>Khoa CNTT</b> gửi Quý Thầy/Cô thông tin và danh sách sinh viên thực tập như sau:<br/>";
            html += "<table width='100%' bgcolor='#F1F1F1' cellpadding='0' cellspacing='0' style='color:#000000'>";
            html += "<thead>";
            html += "<tr style='background:#d58003;font-weight:bold;color:#ffffff'>";
            html += "<td style='padding:5px'>Đơn vị TT</td>";
            html += "<td align='center' style='padding:5px'>Người LH</td>";
            html += "<td align='right' style='padding:5px'>Email LH</td>";
            html += "<td align='right' style='padding:5px'>SDT LH</td>";
            html += "<td align='center' style='padding:5px'>SV</td>";
            html += "<td align='center' style='padding:5px'>E-SV</td>";
            html += "<td align='center' style='padding:5px'>SDT-SV</td>";
            html += "<td align='right' style='padding:5px'>Ngày BĐ</td>";
            html += "<td align='right' style='padding:5px'>Ngày KT</td>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";
            foreach(var sinhvientp in svtp)
            {
                html += "<tr>";
                html += "<td style='padding:5px;border-bottom:1px solid #e3d2a5'>" + sinhvientp.DVTTQuanLy.DonViThucTap.TenDonVi + "</td>";
                html += "<td align='center' style='padding:5px;border-bottom:1px solid #e3d2a5'>" + sinhvientp.DVTTQuanLy.TenQuanLy + "</td>";
                html += "<td align='center' style='padding:5px;border-bottom:1px solid #e3d2a5'>" + sinhvientp.DVTTQuanLy.Email + "</td>";
                html += "<td align='center' style='padding:5px;border-bottom:1px solid #e3d2a5'>" + sinhvientp.DVTTQuanLy.SoDienThoai + "</td>";
                html += "<td align='center' style='padding:5px;border-bottom:1px solid #e3d2a5'>" + sinhvientp.GiangVien.TenGiangVien + "</td>";
                html += "<td align='center' style='padding:5px;border-bottom:1px solid #e3d2a5'>" + sinhvientp.GiangVien.Email + "</td>";
                html += "<td align='center' style='padding:5px;border-bottom:1px solid #e3d2a5'>" + sinhvientp.GiangVien.SoDienThoai + "</td>";
                html += "<td align='center' style='padding:5px;border-bottom:1px solid #e3d2a5'>" + sinhvientp.NgayBD.Value.ToString("dd/MM/yyyy") + "</td>";
                html += "<td align='center' style='padding:5px;border-bottom:1px solid #e3d2a5'>" + sinhvientp.NgayKT.Value.ToString("dd/MM/yyyy") + "</td>";
                html += "</tr>";
            }
            html += "</tbody>";
            html += "</table>";
            html += "<div style='float:left;width:100%;color:#900;font-size:14px;margin-left:1px;font-weight:600;margin-top:-43px'>CHÚC EM CÓ KỲ THỰC TẬP THÀNH CÔNG </div>";
            html += "</div>";


            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("kinhdoanh.bepnhabo@gmail.com", "bepnhabo");

            MailMessage mailMsg = new MailMessage();

            mailMsg.To.Add(new MailAddress(gv.Email,  gv.MaGiangVien));
            mailMsg.From = new MailAddress("kinhdoanh.bepnhabo@gmail.com", "KHOA CNTT");

            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            mailMsg.BodyEncoding = UTF8Encoding.UTF8;
            mailMsg.Subject = "THÔNG TIN THỰC TẬP";
            mailMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mailMsg);

            return true;
        }

        public ActionResult GuiMailThanhCong()
        {
            return View();
        }
        public ActionResult GuiMailThatBai()
        {
            return View();
        }
    }
   
}