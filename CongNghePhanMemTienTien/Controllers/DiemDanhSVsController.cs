using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CongNghePhanMemTienTien.DataContext;
using OfficeOpenXml;
using System.Collections.Generic;

namespace CongNghePhanMemTienTien.Controllers
{
    public class DiemDanhSVsController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();

        public ActionResult Index()
        {
            var diemDanhSVs = db.DiemDanhSVs.Include(d => d.GiangVien).Include(d => d.Lop).Include(d => d.Ca1).Include(d => d.MonHoc).Where(s=>s.HocBu == false);
            return View(diemDanhSVs.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiemDanhSV diemDanhSV = db.DiemDanhSVs.Find(id);
            if (diemDanhSV == null)
            {
                return HttpNotFound();
            }
            return View(diemDanhSV);
        }

        public ActionResult Create()
        {
            ViewBag.MaGV = new SelectList(db.GiangViens, "ID", "TenGiangVien");
            ViewBag.MaLop = new SelectList(db.Lops, "ID", "MaLop");
            ViewBag.Ca = new SelectList(db.Cas, "ID", "TenCa");
            ViewBag.MonHocID = new SelectList(db.MonHocs, "ID", "TenMonHoc");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DiemDanhSV diemDanhSV,string Ngay)
        {
            try
            {
                string[] str = Ngay.ToString().Split('/');
                diemDanhSV.Ngay = new DateTime(int.Parse(str[2]),int.Parse(str[1]), int.Parse(str[0]));
                db.DiemDanhSVs.Add(diemDanhSV);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.MaGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", diemDanhSV.MaGV);
                ViewBag.MaLop = new SelectList(db.Lops, "ID", "MaLop", diemDanhSV.MaLop);
                ViewBag.Ca = new SelectList(db.Cas, "ID", "TenCa", diemDanhSV.Ca);
                ViewBag.MonHocID = new SelectList(db.MonHocs, "ID", "TenMonHoc", diemDanhSV.MonHocID);
                return View(diemDanhSV);
            } 
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiemDanhSV diemDanhSV = db.DiemDanhSVs.Find(id);
            if (diemDanhSV == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", diemDanhSV.MaGV);
            ViewBag.MaLop = new SelectList(db.Lops, "ID", "MaLop", diemDanhSV.MaLop);
            ViewBag.Ca = new SelectList(db.Cas, "ID", "TenCa", diemDanhSV.Ca);
            ViewBag.MonHocID = new SelectList(db.MonHocs, "ID", "TenMonHoc", diemDanhSV.MonHocID);
            return View(diemDanhSV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DiemDanhSV diemDanhSV, string Ngay)
        {
            try
            {
                string[] str = Ngay.ToString().Split('/');
                diemDanhSV.Ngay = new DateTime(int.Parse(str[2]), int.Parse(str[1]), int.Parse(str[0]));
                db.Entry(diemDanhSV).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.MaGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", diemDanhSV.MaGV);
                ViewBag.MaLop = new SelectList(db.Lops, "ID", "MaLop", diemDanhSV.MaLop);
                ViewBag.Ca = new SelectList(db.Cas, "ID", "TenCa", diemDanhSV.Ca);
                ViewBag.MonHocID = new SelectList(db.MonHocs, "ID", "TenMonHoc", diemDanhSV.MonHocID);
                return View(diemDanhSV);
            } 
        }


        //HỌC BÙ
        public ActionResult CreateHB()
        {
            ViewBag.MaGV = new SelectList(db.GiangViens, "ID", "TenGiangVien");
            ViewBag.MaLop = new SelectList(db.Lops, "ID", "MaLop");
            ViewBag.Ca = new SelectList(db.Cas, "ID", "TenCa");
            ViewBag.MonHocID = new SelectList(db.MonHocs, "ID", "TenMonHoc");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHB(DiemDanhSV diemDanhSV, string Ngay)
        {
            try
            {
                string[] str = Ngay.ToString().Split('/');
                diemDanhSV.Ngay = new DateTime(int.Parse(str[2]), int.Parse(str[1]), int.Parse(str[0]));
                diemDanhSV.HocBu = true;
                db.DiemDanhSVs.Add(diemDanhSV);
                db.SaveChanges();
                return RedirectToAction("IndexHB");
            }
            catch
            {
                ViewBag.MaGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", diemDanhSV.MaGV);
                ViewBag.MaLop = new SelectList(db.Lops, "ID", "MaLop", diemDanhSV.MaLop);
                ViewBag.Ca = new SelectList(db.Cas, "ID", "TenCa", diemDanhSV.Ca);
                ViewBag.MonHocID = new SelectList(db.MonHocs, "ID", "TenMonHoc", diemDanhSV.MonHocID);
                return View(diemDanhSV);
            }
        }

        public ActionResult EditHB(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiemDanhSV diemDanhSV = db.DiemDanhSVs.Find(id);
            if (diemDanhSV == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", diemDanhSV.MaGV);
            ViewBag.MaLop = new SelectList(db.Lops, "ID", "MaLop", diemDanhSV.MaLop);
            ViewBag.Ca = new SelectList(db.Cas, "ID", "TenCa", diemDanhSV.Ca);
            ViewBag.MonHocID = new SelectList(db.MonHocs, "ID", "TenMonHoc", diemDanhSV.MonHocID);
            return View(diemDanhSV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHB(DiemDanhSV diemDanhSV, string Ngay)
        {
            try
            {
                string[] str = Ngay.ToString().Split('/');
                diemDanhSV.Ngay = new DateTime(int.Parse(str[2]), int.Parse(str[1]), int.Parse(str[0]));
                diemDanhSV.HocBu = true;
                db.Entry(diemDanhSV).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexHB");
            }
            catch
            {
                ViewBag.MaGV = new SelectList(db.GiangViens, "ID", "TenGiangVien", diemDanhSV.MaGV);
                ViewBag.MaLop = new SelectList(db.Lops, "ID", "MaLop", diemDanhSV.MaLop);
                ViewBag.Ca = new SelectList(db.Cas, "ID", "TenCa", diemDanhSV.Ca);
                ViewBag.MonHocID = new SelectList(db.MonHocs, "ID", "TenMonHoc", diemDanhSV.MonHocID);
                return View(diemDanhSV);
            }
        }
        public ActionResult IndexHB()
        {
            var diemDanhSVs = db.DiemDanhSVs.Include(d => d.GiangVien).Include(d => d.Lop).Include(d => d.Ca1).Include(d => d.MonHoc).Where(s => s.HocBu == true);
            return View(diemDanhSVs.ToList());
        }

        public ActionResult ExportExcelTemplateCreate()
        {
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("DSSanPham_Data");
                    worksheet.Cells["A1:G1"].Merge = true;
                    worksheet.Cells[1, 1].Value = "DANH SÁCH SINH VIÊN THỰC TẬP";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheet.Cells[1, 1].Style.Font.Size = 18;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#607d8b"));


                    worksheet.Cells[2, 1].Value = "HỌ TÊN";
                    worksheet.Cells[2, 2].Value = "MÃ SV";
                    worksheet.Cells[2, 3].Value = "LỚP";
                    worksheet.Cells[2, 4].Value = "GVHD";
                    worksheet.Cells[2, 5].Value = "E-GVHD";
                    worksheet.Cells[2, 6].Value = "P-GVHD";
                    worksheet.Cells[2, 7].Value = "ĐVTT";
                    worksheet.Cells[2, 8].Value = "NGƯỜI QL";
                    worksheet.Cells[2, 9].Value = "EMAIL";
                    worksheet.Cells[2, 10].Value = "SĐT";
                    for (int i = 1; i < worksheet.Dimension.End.Column + 1; i++)
                    {
                        for(int j=1;j<11;j++)
                        {
                            worksheet.Cells[i, j].Style.Font.Bold = true;
                            worksheet.Cells[i, j].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            worksheet.Cells[i, j].Style.Font.Size = 13;
                            worksheet.Cells[i, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[i, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        } 
                    }
                    for (int i = 3; i <101; i++)
                    {
                        worksheet.Cells[i, 1].Value = "HOÀNG VĂN HIẾU" + i;
                        worksheet.Cells[i, 2].Value = "TÊN SINH VIÊN";
                        worksheet.Cells[i, 3].Value = "TÊN SINH VIÊN";
                        worksheet.Cells[i, 4].Value = "TÊN SINH VIÊN";
                        worksheet.Cells[i, 5].Value = "TÊN SINH VIÊN";
                        worksheet.Cells[i, 6].Value = "TÊN SINH VIÊN";
                        worksheet.Cells[i, 7].Value = "TÊN SINH VIÊN"; 
                    }
                    for(int k=1; k<8; k++)
                    {
                        worksheet.Column(k).AutoFit();
                    }
                    
                    string saveAsFileName = DateTime.Now.ToString("yyyyMMdd") + "_FileDatHang.xlsx";
                    Response.Clear();
                    Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                    Response.AddHeader("FileName", saveAsFileName);
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.Flush();
                    Response.End();
                }
                return new EmptyResult();
            }
            catch (Exception objEx)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var item = db.DiemDanhSVs.Find(id);
                db.DiemDanhSVs.Remove(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
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
