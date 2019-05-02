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
using System.Net.Mime;
using System.Text;
using OfficeOpenXml;
using CongNghePhanMemTienTien.Models;

namespace CongNghePhanMemTienTien.Controllers
{
    public class SinhVienThucTapsController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();
        public ActionResult GetAllKhoa()
        {
            List<KhoaModal> list = new List<KhoaModal>();
            list = (from kh in db.Khoas
                    select new KhoaModal
                    {
                        ID = kh.ID,
                        FName = kh.TenKhoa

                    }).ToList();

            return PartialView(list);
        }
        public ActionResult Index()
        {
            var sinhVienThucTaps = db.SinhVienThucTaps.Include(s => s.DonViThucTap).Include(s => s.GiangVien).Include(s => s.SinhVien).Include(s => s.DVTTQuanLy);
            return View(sinhVienThucTaps.ToList());
        }

        public ActionResult ExportSVF(int? id)
        {
            try
            {
                string FName = string.Empty;
                List<SinhVienThucTap> sinhVienThucTaps = new List<SinhVienThucTap>();
                if (id == null)
                {
                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {

                        List<int> list = new List<int>();

                        list = (from kh in db.Khoas select kh.ID).ToList();

                        foreach(var item in list)
                        {
                            FName = db.Khoas.Find(item).TenKhoa.ToUpper();
                            sinhVienThucTaps = db.SinhVienThucTaps.Include(s => s.DonViThucTap).Include(s => s.GiangVien).Include(s => s.SinhVien).Include(s => s.DVTTQuanLy).Where(s => s.GiangVien.KhoaID == item).ToList();
                            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(FName);
                            worksheet.Cells["A1:K1"].Merge = true;
                            worksheet.Cells[1, 1].Value = "DANH SÁCH SINH VIÊN THỰC TẬP THUỘC " + FName;
                            worksheet.Cells[1, 1].Style.Font.Bold = true;
                            worksheet.Cells[1, 1].Style.Font.Size = 18;
                            worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#607d8b"));
                            worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);

                            for (int j = 1; j < 12; j++)
                            {
                                worksheet.Cells[2, j].Style.Font.Bold = true;
                                worksheet.Cells[2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                worksheet.Cells[2, j].Style.Font.Size = 15;
                                worksheet.Cells[2, j].Style.Font.Color.SetColor(System.Drawing.Color.OrangeRed);
                                worksheet.Cells[2, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[2, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Transparent);
                            }

                            worksheet.Cells[2, 1].Value = "HỌ TÊN";
                            worksheet.Cells[2, 2].Value = "MÃ SV";
                            worksheet.Cells[2, 3].Value = "LỚP";
                            worksheet.Cells[2, 4].Value = "EMAIL";
                            worksheet.Cells[2, 5].Value = "SĐT";
                            worksheet.Cells[2, 6].Value = "GVHD";
                            worksheet.Cells[2, 7].Value = "E-GVHD";
                            worksheet.Cells[2, 8].Value = "P-GVHD";
                            worksheet.Cells[2, 9].Value = "ĐVTT";
                            worksheet.Cells[2, 10].Value = "NGƯỜI QL";
                            worksheet.Cells[2, 11].Value = "EMAIL";
                            int total = sinhVienThucTaps.Count();
                            for (int i = 3; i < total + 1; i++)
                            {
                                for (int j = 1; j < 12; j++)
                                {
                                    worksheet.Cells[i, j].Style.Font.Bold = true;
                                    worksheet.Cells[i, j].Style.Font.Color.SetColor(System.Drawing.Color.CadetBlue);
                                    worksheet.Cells[i, j].Style.Font.Size = 13;
                                    worksheet.Cells[i, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#f8f9fa"));
                                    worksheet.Cells[i, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                }
                            }

                            for (int i = 0; i < total; i++)
                            {
                                worksheet.Cells[i + 3, 1].Value = sinhVienThucTaps[i].SinhVien.TenSinhVien;
                                worksheet.Cells[i + 3, 2].Value = sinhVienThucTaps[i].SinhVien.MaSinhVien;
                                worksheet.Cells[i + 3, 3].Value = sinhVienThucTaps[i].SinhVien.Lop.MaLop;
                                worksheet.Cells[i + 3, 4].Value = sinhVienThucTaps[i].SinhVien.Email;
                                worksheet.Cells[i + 3, 5].Value = sinhVienThucTaps[i].SinhVien.SoDienThoai;
                                worksheet.Cells[i + 3, 6].Value = sinhVienThucTaps[i].GiangVien.TenGiangVien;
                                worksheet.Cells[i + 3, 7].Value = sinhVienThucTaps[i].GiangVien.Email;
                                worksheet.Cells[i + 3, 8].Value = sinhVienThucTaps[i].GiangVien.SoDienThoai;
                                worksheet.Cells[i + 3, 9].Value = sinhVienThucTaps[i].DVTTQuanLy.DonViThucTap.TenDonVi;
                                worksheet.Cells[i + 3, 10].Value = sinhVienThucTaps[i].DVTTQuanLy.TenQuanLy;
                                worksheet.Cells[i + 3, 11].Value = sinhVienThucTaps[i].DVTTQuanLy.Email;
                            }
                            for (int k = 1; k < 12; k++)
                            {
                                worksheet.Column(k).AutoFit();
                            }

                        }

                        string saveAsFileName = DateTime.Now.ToString("yyyyMMdd") + "_DSSinhVienThucTap.xlsx";
                        Response.Clear();
                        Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                        Response.AddHeader("FileName", saveAsFileName);
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {
                    FName = db.Khoas.Find(id).TenKhoa.ToUpper();
                    sinhVienThucTaps = db.SinhVienThucTaps.Include(s => s.DonViThucTap).Include(s => s.GiangVien).Include(s => s.SinhVien).Include(s => s.DVTTQuanLy).Where(s => s.GiangVien.KhoaID == id).ToList();

                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("DSSVTT_DATA");
                        worksheet.Cells["A1:K1"].Merge = true;
                        worksheet.Cells[1, 1].Value = "DANH SÁCH SINH VIÊN THỰC TẬP THUỘC " + FName;
                        worksheet.Cells[1, 1].Style.Font.Bold = true;
                        worksheet.Cells[1, 1].Style.Font.Size = 18;
                        worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#607d8b"));
                        worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);

                        for (int j = 1; j < 12; j++)
                        {
                            worksheet.Cells[2, j].Style.Font.Bold = true;
                            worksheet.Cells[2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            worksheet.Cells[2, j].Style.Font.Size = 15;
                            worksheet.Cells[2, j].Style.Font.Color.SetColor(System.Drawing.Color.OrangeRed);
                            worksheet.Cells[2, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[2, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Transparent);
                        }

                        worksheet.Cells[2, 1].Value = "HỌ TÊN";
                        worksheet.Cells[2, 2].Value = "MÃ SV";
                        worksheet.Cells[2, 3].Value = "LỚP";
                        worksheet.Cells[2, 4].Value = "EMAIL";
                        worksheet.Cells[2, 5].Value = "SĐT";
                        worksheet.Cells[2, 6].Value = "GVHD";
                        worksheet.Cells[2, 7].Value = "E-GVHD";
                        worksheet.Cells[2, 8].Value = "P-GVHD";
                        worksheet.Cells[2, 9].Value = "ĐVTT";
                        worksheet.Cells[2, 10].Value = "NGƯỜI QL";
                        worksheet.Cells[2, 11].Value = "EMAIL";
                        int total = sinhVienThucTaps.Count();
                        for (int i = 3; i < total + 1; i++)
                        {
                            for (int j = 1; j < 12; j++)
                            {
                                worksheet.Cells[i, j].Style.Font.Bold = true;
                                worksheet.Cells[i, j].Style.Font.Color.SetColor(System.Drawing.Color.CadetBlue);
                                worksheet.Cells[i, j].Style.Font.Size = 13;
                                worksheet.Cells[i, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#f8f9fa"));
                                worksheet.Cells[i, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            }
                        }

                        for (int i = 0; i < total; i++)
                        {
                            worksheet.Cells[i + 3, 1].Value = sinhVienThucTaps[i].SinhVien.TenSinhVien;
                            worksheet.Cells[i + 3, 2].Value = sinhVienThucTaps[i].SinhVien.MaSinhVien;
                            worksheet.Cells[i + 3, 3].Value = sinhVienThucTaps[i].SinhVien.Lop.MaLop;
                            worksheet.Cells[i + 3, 4].Value = sinhVienThucTaps[i].SinhVien.Email;
                            worksheet.Cells[i + 3, 5].Value = sinhVienThucTaps[i].SinhVien.SoDienThoai;
                            worksheet.Cells[i + 3, 6].Value = sinhVienThucTaps[i].GiangVien.TenGiangVien;
                            worksheet.Cells[i + 3, 7].Value = sinhVienThucTaps[i].GiangVien.Email;
                            worksheet.Cells[i + 3, 8].Value = sinhVienThucTaps[i].GiangVien.SoDienThoai;
                            worksheet.Cells[i + 3, 9].Value = sinhVienThucTaps[i].DVTTQuanLy.DonViThucTap.TenDonVi;
                            worksheet.Cells[i + 3, 10].Value = sinhVienThucTaps[i].DVTTQuanLy.TenQuanLy;
                            worksheet.Cells[i + 3, 11].Value = sinhVienThucTaps[i].DVTTQuanLy.Email;
                        }
                        for (int k = 1; k < 12; k++)
                        {
                            worksheet.Column(k).AutoFit();
                        }


                        string saveAsFileName = DateTime.Now.ToString("yyyyMMdd") + "_DSSinhVienThucTap_"+FName+".xlsx";
                        Response.Clear();
                        Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                        Response.AddHeader("FileName", saveAsFileName);
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.Flush();
                        Response.End();
                    }
                }
                return new EmptyResult();
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVienThucTap sinhVienThucTap = db.SinhVienThucTaps.Find(id);
            if (sinhVienThucTap == null)
            {
                return HttpNotFound();
            }
            return View(sinhVienThucTap);
        }
        public ActionResult Create()
        {
            ViewBag.MGV = new SelectList(db.GiangViens, "ID", "TenGiangVien");
            ViewBag.MSV = new SelectList(db.SinhViens, "ID", "TenSinhVien");
            ViewBag.MQL = new SelectList(db.DVTTQuanLies, "ID", "TenQuanLy");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SinhVienThucTap sinhVienThucTap, string NgayBD, string NgayKT)
        {
            try
            {
                SinhVienThucTap sv = db.SinhVienThucTaps.Where(s => s.SinhVien.ID == sinhVienThucTap.MSV).FirstOrDefault();
                if (sv != null)
                {
                    return RedirectToAction("Index");
                }
                string[] str = NgayBD.ToString().Split('/');
                sinhVienThucTap.NgayBD = new DateTime(int.Parse(str[2]), int.Parse(str[1]), int.Parse(str[0]));
                str = NgayKT.ToString().Split('/');
                sinhVienThucTap.NgayKT = new DateTime(int.Parse(str[2]), int.Parse(str[1]), int.Parse(str[0]));
                db.SinhVienThucTaps.Add(sinhVienThucTap);
                db.SaveChanges();
                SinhVienThucTap item = db.SinhVienThucTaps.Include(s => s.DonViThucTap).Include(s => s.GiangVien).Include(s => s.SinhVien).Include(s => s.DVTTQuanLy).Where(q => q.ID == sinhVienThucTap.ID).FirstOrDefault();
                if (SendMail(item))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", sinhVienThucTap.MGV);
                    ViewBag.MSV = new SelectList(db.SinhViens, "ID", "TenSinhVien", sinhVienThucTap.MSV);
                    ViewBag.MQL = new SelectList(db.DVTTQuanLies, "ID", "TenQuanLy", sinhVienThucTap.MQL);
                    return View(sinhVienThucTap);
                }
            }
            catch
            {
                ViewBag.MGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", sinhVienThucTap.MGV);
                ViewBag.MSV = new SelectList(db.SinhViens, "ID", "TenSinhVien", sinhVienThucTap.MSV);
                ViewBag.MQL = new SelectList(db.DVTTQuanLies, "ID", "TenQuanLy", sinhVienThucTap.MQL);
                return View(sinhVienThucTap);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVienThucTap sinhVienThucTap = db.SinhVienThucTaps.Find(id);
            if (sinhVienThucTap == null)
            {
                return HttpNotFound();
            }
            ViewBag.DVTTID = new SelectList(db.DonViThucTaps, "ID", "TenDonVi", sinhVienThucTap.DVTTID);
            ViewBag.MGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", sinhVienThucTap.MGV);
            ViewBag.MSV = new SelectList(db.SinhViens, "ID", "TenSinhVien", sinhVienThucTap.MSV);
            ViewBag.MQL = new SelectList(db.DVTTQuanLies, "ID", "TenQuanLy", sinhVienThucTap.MQL);
            return View(sinhVienThucTap);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SinhVienThucTap sinhVienThucTap, string NgayBD, string NgayKT)
        {
            try
            {
                string[] str = NgayBD.ToString().Split('/');
                sinhVienThucTap.NgayBD = new DateTime(int.Parse(str[2]), int.Parse(str[1]), int.Parse(str[0]));
                str = NgayKT.ToString().Split('/');
                sinhVienThucTap.NgayKT = new DateTime(int.Parse(str[2]), int.Parse(str[1]), int.Parse(str[0]));
                db.Entry(sinhVienThucTap).State = EntityState.Modified;
                db.SaveChanges();
                SinhVienThucTap item = db.SinhVienThucTaps.Include(s => s.DonViThucTap).Include(s => s.GiangVien).Include(s => s.SinhVien).Include(s => s.DVTTQuanLy).Where(q => q.ID == sinhVienThucTap.ID).FirstOrDefault();
                if (SendMail(item))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.DVTTID = new SelectList(db.DonViThucTaps, "ID", "TenDonVi", sinhVienThucTap.DVTTID);
                    ViewBag.MGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", sinhVienThucTap.MGV);
                    ViewBag.MSV = new SelectList(db.SinhViens, "ID", "TenSinhVien", sinhVienThucTap.MSV);
                    ViewBag.MQL = new SelectList(db.DVTTQuanLies, "ID", "TenQuanLy", sinhVienThucTap.MQL);
                    return View(sinhVienThucTap);
                }
            }
            catch
            {
                ViewBag.DVTTID = new SelectList(db.DonViThucTaps, "ID", "TenDonVi", sinhVienThucTap.DVTTID);
                ViewBag.MGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", sinhVienThucTap.MGV);
                ViewBag.MSV = new SelectList(db.SinhViens, "ID", "TenSinhVien", sinhVienThucTap.MSV);
                ViewBag.MQL = new SelectList(db.DVTTQuanLies, "ID", "TenQuanLy", sinhVienThucTap.MQL);
                return View(sinhVienThucTap);
            }
        }


        public ActionResult Delete(int id)
        {
            SinhVienThucTap sinhVienThucTap = db.SinhVienThucTaps.Find(id);
            db.SinhVienThucTaps.Remove(sinhVienThucTap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public bool SendMail(SinhVienThucTap sinhvientp)
        {

            string html = "";

            html = "<h2><a href='#'>KHOA CNTT GỬI BẠN THÔNG TIN THỰC TẬP<a/></h2>";
            html += "<div style='font-size:14px; line-height:2; color:#000;background:#fff'>";
            html += "<p style='padding: 5px; font - style:italic; color:#900'>NGÀY GỬI: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "</p>";
            html += "<h2 style='border-bottom:2px solid #d58003;padding-bottom:0px'>THÔNG TIN CHI TIẾT</h2>";
            html += "<p style='margin-bottom:10px'>Gửi sinh viên: " + sinhvientp.SinhVien.TenSinhVien + "</p>";
            html += "<b>KHOA CNTT</b> gửi bạn thông tin thực tập như sau:<br/>";
            html += "<table width='100%' bgcolor='#F1F1F1' cellpadding='0' cellspacing='0' style='color:#000000'>";
            html += "<thead>";
            html += "<tr style='background:#d58003;font-weight:bold;color:#ffffff'>";
            html += "<td style='padding:5px'>Đơn vị TT</td>";
            html += "<td align='center' style='padding:5px'>Người LH</td>";
            html += "<td align='right' style='padding:5px'>Email LH</td>";
            html += "<td align='right' style='padding:5px'>SDT LH</td>";
            html += "<td align='center' style='padding:5px'>GVHD</td>";
            html += "<td align='center' style='padding:5px'>E-GVHD</td>";
            html += "<td align='center' style='padding:5px'>SDT-GVHD</td>";
            html += "<td align='right' style='padding:5px'>Ngày BĐ</td>";
            html += "<td align='right' style='padding:5px'>Ngày KT</td>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";
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

            mailMsg.To.Add(new MailAddress(sinhvientp.SinhVien.Email, sinhvientp.SinhVien.MaSinhVien));
            mailMsg.From = new MailAddress("kinhdoanh.bepnhabo@gmail.com", "KHOA CNTT");

            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            mailMsg.BodyEncoding = UTF8Encoding.UTF8;
            mailMsg.Subject = "THÔNG TIN THỰC TẬP";
            mailMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mailMsg);

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
