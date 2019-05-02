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
    public class NganhsController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();
        private string pathFile = "/Files/NGANH/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/Images/";
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
                ViewBag.TenKhoa = db.Khoas.Find(id).TenKhoa.ToUpper() + " - ";
                var list = (from nganh in db.Nganhs
                            orderby nganh.ID descending
                            where nganh.KhoaID == id
                            select nganh).ToPagedList(pageNum, pageSize);
                return View(list);
            }
            var listAll = (from nganh in db.Nganhs
                           orderby nganh.ID descending
                           select nganh).ToPagedList(pageNum, pageSize);
            return View(listAll);
        }
        public void Export(int? id)
        {
            List<Nganh> list = new List<Nganh>();
            if (id != null)
            {
                list = (from nganh in db.Nganhs
                        orderby nganh.ID descending
                        where nganh.KhoaID == id
                        select nganh).ToList();
            }
            else
            {
                list = (from nganh in db.Nganhs
                        orderby nganh.ID descending
                        select nganh).ToList();
            }

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("DSSanPham_Data");
                    if (id == null)
                    {
                        worksheet.Cells["A1:D1"].Merge = true;
                    }
                    else
                    {
                        worksheet.Cells["A1:C1"].Merge = true;
                    }
                    if (id != null)
                    {
                        string name = db.Khoas.Find(id).TenKhoa.ToUpper();
                        worksheet.Cells[1, 1].Value = "NGÀNH THUỘC " + name;
                    }
                    else
                    {
                        worksheet.Cells[1, 1].Value = "DANH SÁCH NGÀNH";
                    }
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 18;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#607d8b"));
                    worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.White);

                    for (int j = 1; j < (id != null ? 4 : 5); j++)
                    {
                        worksheet.Cells[2, j].Style.Font.Bold = true;
                        worksheet.Cells[2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[2, j].Style.Font.Size = 15;
                        worksheet.Cells[2, j].Style.Font.Color.SetColor(System.Drawing.Color.OrangeRed);
                        worksheet.Cells[2, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[2, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Transparent);
                    }

                    worksheet.Cells[2, 1].Value = "TÊN NGÀNH";
                    worksheet.Cells[2, 2].Value = "MÃ NGÀNH";
                    worksheet.Cells[2, 3].Value = "SỐ LƯỢNG CHUYÊN NGÀNH";
                    if (id == null)
                    {
                        worksheet.Cells[2, 4].Value = "KHOA";
                    }
                    int total = list.Count();
                    for (int i = 3; i < total + 3; i++)
                    {
                        for (int j = 1; j < (id != null ? 4 : 5); j++)
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
                        worksheet.Cells[i + 3, 1].Value = list[i].TenNganh;
                        worksheet.Cells[i + 3, 2].Value = list[i].MaNganh;
                        worksheet.Cells[i + 3, 3].Value = list[i].ChuyenNganhs.Count;
                        if (id == null)
                        {
                            worksheet.Cells[i + 3, 4].Value = list[i].Khoa.TenKhoa.ToUpper();
                        }
                    }
                    for (int k = 1; k < 6; k++)
                    {
                        worksheet.Column(k).AutoFit();
                    }


                    string saveAsFileName = DateTime.Now.ToString("yyyyMMdd") + "_Danh_Sach_Nganh.xlsx";
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
        public ActionResult Create()
        {
            ViewBag.KhoaID = new SelectList(db.Khoas, "ID", "TenKhoa");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Nganh nganh, HttpPostedFileBase Logo)
        {
            try
            {
                if (Logo != null)
                {
                    nganh.Logo = UploadCreateDir(Logo, "");
                }
                db.Nganhs.Add(nganh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.KhoaID = new SelectList(db.Khoas, "ID", "TenKhoa", nganh.KhoaID);
                return View(nganh);
            } 
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nganh nganh = db.Nganhs.Find(id);
            if (nganh == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhoaID = new SelectList(db.Khoas, "ID", "TenKhoa", nganh.KhoaID);
            return View(nganh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Nganh nganh, HttpPostedFileBase Logo)
        {
            try
            {
                if (Logo != null)
                {
                    nganh.Logo = UploadCreateDir(Logo, "");
                }
                db.Entry(nganh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.KhoaID = new SelectList(db.Khoas, "ID", "TenKhoa", nganh.KhoaID);
                return View(nganh);
            }
           
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var item = db.Nganhs.Find(id);
                db.Nganhs.Remove(item);
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
