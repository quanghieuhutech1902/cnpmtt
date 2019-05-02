using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CongNghePhanMemTienTien.DataContext;
using System.IO;
using ImageResizer;
using PagedList;
using OfficeOpenXml;

namespace CongNghePhanMemTienTien.Controllers
{
    public class SinhViensController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();
        private string pathFile = "/Files/SINH-VIEN/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/Images/";
        private string fileName = "";
        public string UploadCreateDir(HttpPostedFileBase upload, string name)
        {
            var versions = new Dictionary<string, string>();
            versions.Add("Cat", "width=200&height=200&format=jpeg&quality=60");
            fileName = Path.GetFileName(upload.FileName);

            bool exsits = System.IO.Directory.Exists(Server.MapPath(pathFile));
            if (!exsits)
                System.IO.Directory.CreateDirectory(Server.MapPath(pathFile));
            var path = Path.Combine(Server.MapPath(pathFile), name + fileName);
            upload.SaveAs(path);
            ImageBuilder.Current.Build(path, path, new ResizeSettings(versions["Cat"]));
            return pathFile + name + fileName;
        }
        public ActionResult Index(int? page, int? id)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            if (id != null)
            {
                ViewBag.ID = id;
                ViewBag.TenLop = db.Lops.Find(id).MaLop.ToUpper() + " - ";
                var list = (from sinhvien in db.SinhViens
                            orderby sinhvien.ID descending
                            where sinhvien.LopID == id
                            select sinhvien).ToPagedList(pageNum, pageSize);
                return View(list);
            }
            var listAll = (from sinhvien in db.SinhViens
                           orderby sinhvien.ID descending
                           select sinhvien).ToPagedList(pageNum, pageSize);
            return View(listAll);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sinhVien = db.SinhViens.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            return View(sinhVien);
        }

        public ActionResult Create()
        {
            ViewBag.PhuHuynhID = new SelectList(db.PhuHuynhs, "ID", "HoTenCha");
            ViewBag.ChuyenNganhID = new SelectList(db.ChuyenNganhs, "ID", "TenChuyenNganh");
            ViewBag.LopID = new SelectList(db.Lops, "ID", "MaLop");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SinhVien sinhVien, HttpPostedFileBase Logo)
        {
            try
            {
                if (Logo != null)
                {
                    sinhVien.HinhDaiDien = UploadCreateDir(Logo, "");
                }
                db.SinhViens.Add(sinhVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.PhuHuynhID = new SelectList(db.PhuHuynhs, "ID", "HoTenCha", sinhVien.PhuHuynhID);
                ViewBag.LopID = new SelectList(db.Lops, "ID", "MaLop", sinhVien.LopID);
                ViewBag.ChuyenNganhID = new SelectList(db.ChuyenNganhs, "ID", "TenChuyenNganh", sinhVien.ChuyenNganhID);
                return View(sinhVien);
            }
            
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sinhVien = db.SinhViens.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhuHuynhID = new SelectList(db.PhuHuynhs, "ID", "HoTenCha", sinhVien.PhuHuynhID);
            ViewBag.LopID = new SelectList(db.Lops, "ID", "MaLop", sinhVien.LopID);
            ViewBag.ChuyenNganhID = new SelectList(db.ChuyenNganhs, "ID", "TenChuyenNganh", sinhVien.ChuyenNganhID);
            return View(sinhVien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SinhVien sinhVien, HttpPostedFileBase Logo)
        {
            try
            {
                if (Logo != null)
                {
                    sinhVien.HinhDaiDien = UploadCreateDir(Logo, "");
                }
                db.Entry(sinhVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.PhuHuynhID = new SelectList(db.PhuHuynhs, "ID", "HoTenCha", sinhVien.PhuHuynhID);
                ViewBag.LopID = new SelectList(db.Lops, "ID", "MaLop", sinhVien.LopID);
                ViewBag.ChuyenNganhID = new SelectList(db.ChuyenNganhs, "ID", "TenChuyenNganh", sinhVien.ChuyenNganhID);
                return View(sinhVien);
            }
            
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var item = db.SinhViens.Find(id);
                db.SinhViens.Remove(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public void Export(int? id)
        {
            List<SinhVien> list = new List<SinhVien>();
            if (id != null)
            {
                list = (from gv in db.SinhViens
                        orderby gv.ID descending
                        where gv.LopID == id
                        select gv).ToList();
            }
            else
            {
                list = (from gv in db.SinhViens
                        orderby gv.ID descending
                        select gv).ToList();
            }

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("DSGiangVien_Data");
                    if (id == null)
                    {
                        worksheet.Cells["A1:F1"].Merge = true;
                    }
                    else
                    {
                        worksheet.Cells["A1:E1"].Merge = true;
                    }
                    if (id != null)
                    {
                        string name = db.Khoas.Find(id).TenKhoa.ToUpper();
                        worksheet.Cells[1, 1].Value = "DANH SÁCH GIẢNG THUỘC " + name;
                    }
                    else
                    {
                        worksheet.Cells[1, 1].Value = "DANH SÁCH GIẢNG VIÊN";
                    }
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 18;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#607d8b"));
                    worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);

                    for (int j = 1; j < (id != null ? 6 : 7); j++)
                    {
                        worksheet.Cells[2, j].Style.Font.Bold = true;
                        worksheet.Cells[2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[2, j].Style.Font.Size = 15;
                        worksheet.Cells[2, j].Style.Font.Color.SetColor(System.Drawing.Color.OrangeRed);
                        worksheet.Cells[2, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[2, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Transparent);
                    }

                    worksheet.Cells[2, 1].Value = "TÊN - GV";
                    worksheet.Cells[2, 2].Value = "MÃ - GV";
                    worksheet.Cells[2, 3].Value = "SĐT";
                    worksheet.Cells[2, 4].Value = "EMAIL";
                    worksheet.Cells[2, 5].Value = "SỐ LƯỢNG SVTT";
                    if (id == null)
                    {
                        worksheet.Cells[2, 6].Value = "KHOA";
                    }
                    int total = list.Count();
                    for (int i = 3; i < total + 3; i++)
                    {
                        for (int j = 1; j < (id != null ? 6 : 7); j++)
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
                        worksheet.Cells[i + 3, 1].Value = list[i].TenSinhVien;
                        worksheet.Cells[i + 3, 2].Value = list[i].MaSinhVien;
                        worksheet.Cells[i + 3, 3].Value = list[i].SoDienThoai;
                        worksheet.Cells[i + 3, 4].Value = list[i].Email;
                        worksheet.Cells[i + 3, 5].Value = list[i].SinhVienThucTaps.Count();
                        if (id == null)
                        {
                            worksheet.Cells[i + 3, 6].Value = list[i].Lop.TenLop.ToUpper();
                        }
                    }
                    for (int k = 1; k < 7; k++)
                    {
                        worksheet.Column(k).AutoFit();
                    }


                    string saveAsFileName = DateTime.Now.ToString("yyyyMMdd") + "_Danh_Sach_Giang_Vien.xlsx";
                    Response.Clear();
                    Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                    Response.AddHeader("FileName", saveAsFileName);
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ecc)
            {
            }
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
