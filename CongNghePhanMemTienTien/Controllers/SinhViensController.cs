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
using CongNghePhanMemTienTien.Models;

namespace CongNghePhanMemTienTien.Controllers
{
    public class SinhViensController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();
        private string pathFile = "/Files/SINH-VIEN/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/Images/";
        private string fileName = "";

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

        public void ExportC(int? id)
        {
            List<SinhVien> list = new List<SinhVien>();
            string name = string.Empty;
            if (id != null)
            {
                name = db.Lops.Find(id).MaLop.ToUpper();
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
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("DSSinhVien_Data");
                    if (id == null)
                    {
                        worksheet.Cells["A1:E1"].Merge = true;
                    }
                    else
                    {
                        worksheet.Cells["A1:D1"].Merge = true;
                    }
                    if (id != null)
                    {
                        worksheet.Cells[1, 1].Value = "DANH SÁCH SINH VIÊN THUỘC LỚP " + name;
                    }
                    else
                    {
                        worksheet.Cells[1, 1].Value = "DANH SÁCH SINH VIÊN";
                    }
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 18;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#607d8b"));
                    worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);

                    for (int j = 1; j < (id != null ? 5 : 6); j++)
                    {
                        worksheet.Cells[2, j].Style.Font.Bold = true;
                        worksheet.Cells[2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[2, j].Style.Font.Size = 15;
                        worksheet.Cells[2, j].Style.Font.Color.SetColor(System.Drawing.Color.OrangeRed);
                        worksheet.Cells[2, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[2, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Transparent);
                    }

                    worksheet.Cells[2, 1].Value = "TÊN - SV";
                    worksheet.Cells[2, 2].Value = "MÃ - SV";
                    worksheet.Cells[2, 3].Value = "SĐT";
                    worksheet.Cells[2, 4].Value = "EMAIL";
                    if (id == null)
                    {
                        worksheet.Cells[2, 5].Value = "LỚP";
                    }
                    int total = list.Count();
                    for (int i = 3; i < total + 3; i++)
                    {
                        for (int j = 1; j < (id != null ? 5 : 6); j++)
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
                        if (id == null)
                        {
                            worksheet.Cells[i + 3, 5].Value = list[i].Lop.MaLop.ToUpper();
                        }
                    }
                    for (int k = 1; k < 6; k++)
                    {
                        worksheet.Column(k).AutoFit();
                    }


                    string saveAsFileName = DateTime.Now.ToString("yyyyMMdd") + "_Danh_Sach_Sinh_Vien" + name + ".xlsx";
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

        public void ExportF(int? id)
        {
            List<int> list = new List<int>();
            string tenKhoa = db.Khoas.Find(id).TenKhoa.ToUpper();
            if (id != null)
            {
                list = (from gv in db.Lops
                        orderby gv.ID descending
                        where gv.KhoaID == id
                        select gv.ID).ToList();
            }
            else
            {
                list = (from gv in db.Lops
                        orderby gv.ID descending
                        select gv.ID).ToList();
            }

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {

                    foreach (var item in list)
                    {
                        var lop = db.Lops.Find(item);

                        if (lop == null || lop.SinhViens.Count == 0)
                        {
                            continue;
                        }
                        var listSV = db.SinhViens.Where(a => a.LopID == lop.ID).ToList();
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(lop.MaLop.ToUpper());
                        worksheet.Cells["A1:E1"].Merge = true;
                        worksheet.Cells[1, 1].Value = "DANH SÁCH SINH VIÊN THUỘC LỚP " + lop.MaLop.ToUpper();

                        worksheet.Cells[1, 1].Style.Font.Bold = true;
                        worksheet.Cells[1, 1].Style.Font.Size = 18;
                        worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#607d8b"));
                        worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);

                        for (int j = 1; j < 6; j++)
                        {
                            worksheet.Cells[2, j].Style.Font.Bold = true;
                            worksheet.Cells[2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            worksheet.Cells[2, j].Style.Font.Size = 15;
                            worksheet.Cells[2, j].Style.Font.Color.SetColor(System.Drawing.Color.OrangeRed);
                            worksheet.Cells[2, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[2, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Transparent);
                        }

                        worksheet.Cells[2, 1].Value = "TÊN - SV";
                        worksheet.Cells[2, 2].Value = "MÃ - SV";
                        worksheet.Cells[2, 3].Value = "SĐT";
                        worksheet.Cells[2, 4].Value = "EMAIL";
                        worksheet.Cells[2, 5].Value = "KHOA";
                        int total = listSV.Count();
                        for (int i = 3; i < total + 3; i++)
                        {
                            for (int j = 1; j < 6; j++)
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
                            worksheet.Cells[i + 3, 1].Value = listSV[i].TenSinhVien;
                            worksheet.Cells[i + 3, 2].Value = listSV[i].MaSinhVien;
                            worksheet.Cells[i + 3, 3].Value = listSV[i].SoDienThoai;
                            worksheet.Cells[i + 3, 4].Value = listSV[i].Email;
                            worksheet.Cells[i + 3, 5].Value = listSV[i].Lop.Khoa.TenKhoa.ToUpper();
                        }
                        for (int k = 1; k < 7; k++)
                        {
                            worksheet.Column(k).AutoFit();
                        }

                    }

                    string saveAsFileName = DateTime.Now.ToString("yyyyMMdd") + "_DSSV_KHOA_"+tenKhoa+".xlsx";
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
