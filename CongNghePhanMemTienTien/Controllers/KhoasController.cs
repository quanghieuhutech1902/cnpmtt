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
    public class KhoasController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();
        private string pathFile = "/Files/KHOA/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/Images/";
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
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var listAll = (from khoa in db.Khoas
                           orderby khoa.ID descending
                           select khoa).ToPagedList(pageNum, pageSize);
            return View(listAll);
        }
        public ActionResult Export()
        {
            try
            {
                var khoas = db.Khoas.ToList();
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("DSSanPham_Data");
                    worksheet.Cells["A1:G1"].Merge = true;
                    worksheet.Cells[1, 1].Value = "DANH SÁCH KHOA";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 18;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#607d8b"));
                    worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);

                    for (int j = 1; j < 8; j++)
                    {
                        worksheet.Cells[2, j].Style.Font.Bold = true;
                        worksheet.Cells[2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[2, j].Style.Font.Size = 15;
                        worksheet.Cells[2, j].Style.Font.Color.SetColor(System.Drawing.Color.OrangeRed);
                        worksheet.Cells[2, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[2, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Transparent);
                    }

                    worksheet.Cells[2, 1].Value = "TÊN KHOA";
                    worksheet.Cells[2, 2].Value = "MÃ KHOA";
                    worksheet.Cells[2, 3].Value = "SỐ LƯỢNG LỚP";
                    worksheet.Cells[2, 4].Value = "SỐ LƯỢNG NGÀNH";
                    worksheet.Cells[2, 5].Value = "SỐ LƯỢNG CHUYÊN NGÀNH";
                    worksheet.Cells[2, 6].Value = "SỐ LƯỢNG GIẢNG VIÊN";
                    worksheet.Cells[2, 7].Value = "SỐ LƯỢNG SINH VIÊN";
                    int total = khoas.Count();
                    for (int i = 3; i < total + 3; i++)
                    {
                        for (int j = 1; j < 8; j++)
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
                        int id = khoas[i].ID;
                        worksheet.Cells[i + 3, 1].Value = khoas[i].TenKhoa;
                        worksheet.Cells[i + 3, 2].Value = khoas[i].MaKhoa;
                        worksheet.Cells[i + 3, 3].Value = khoas[i].Lops.Count;
                        worksheet.Cells[i + 3, 4].Value = khoas[i].Nganhs.Count;

                        List<int> nganhID = (from ng in db.Nganhs where ng.KhoaID == id select ng.ID).ToList();
                        int totalCN = 0;
                        foreach (int idNganh in nganhID)
                        {
                            totalCN += db.ChuyenNganhs.Where(s => s.NganhID == idNganh).Count();
                        }
                        worksheet.Cells[i + 3, 5].Value = totalCN; 
                        worksheet.Cells[i + 3, 6].Value = khoas[i].GiangViens.Count; 
                        List<int> lop = (from ng in db.Lops where ng.KhoaID == id select ng.ID).ToList();
                        int totalSV = 0;
                        foreach (int idLop in lop)
                        {
                            totalSV += db.SinhViens.Where(s => s.LopID == idLop).Count();
                        }
                        worksheet.Cells[i + 3, 7].Value = totalSV;
                    }
                    for (int k = 1; k < 10; k++)
                    {
                        worksheet.Column(k).AutoFit();
                    }


                    string saveAsFileName = DateTime.Now.ToString("yyyyMMdd") + "_KHOA.xlsx";
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
            catch (Exception ecc)
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
            Khoa khoa = db.Khoas.Find(id);
            if (khoa == null)
            {
                return HttpNotFound();
            }
            return View(khoa);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Khoa khoa, HttpPostedFileBase Logo)
        {
            try
            {
                if (Logo != null)
                {
                    khoa.Logo = UploadCreateDir(Logo, "");
                }
                else
                {
                    return View(khoa);
                }
                khoa.UpdatedDate = DateTime.Now;
                khoa.CreateDate = DateTime.Now;
                db.Khoas.Add(khoa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(khoa);
            }
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Khoa khoa = db.Khoas.Find(id);
            if (khoa == null)
            {
                return HttpNotFound();
            }
            return View(khoa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Khoa khoa, HttpPostedFileBase Logo)
        {
            try
            {
                if (Logo != null)
                {
                    khoa.Logo = UploadCreateDir(Logo, "");
                }
                khoa.UpdatedDate = DateTime.Now;
                db.Entry(khoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(khoa);
            }
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Khoa khoa = db.Khoas.Find(id);
            if (khoa == null)
            {
                return HttpNotFound();
            }
            return View(khoa);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Khoa khoa = db.Khoas.Find(id);
            db.Khoas.Remove(khoa);
            db.SaveChanges();
            return RedirectToAction("Index");
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
