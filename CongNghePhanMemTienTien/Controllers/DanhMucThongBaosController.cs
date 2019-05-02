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

namespace CongNghePhanMemTienTien.Controllers
{
    public class DanhMucThongBaosController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();

        private string pathFile = "/Files/LOAI-THONG-BAO/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/Images/";
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
        public ActionResult Index()
        {
            return View(db.DanhMucThongBaos.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucThongBao danhMucThongBao = db.DanhMucThongBaos.Find(id);
            if (danhMucThongBao == null)
            {
                return HttpNotFound();
            }
            return View(danhMucThongBao);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DanhMucThongBao danhMucThongBao, HttpPostedFileBase Logo)
        {
            try
            {
                if (Logo != null)
                {
                    danhMucThongBao.Logo = UploadCreateDir(Logo, "");
                }
                db.DanhMucThongBaos.Add(danhMucThongBao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(danhMucThongBao);
            }
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucThongBao danhMucThongBao = db.DanhMucThongBaos.Find(id);
            if (danhMucThongBao == null)
            {
                return HttpNotFound();
            }
            return View(danhMucThongBao);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DanhMucThongBao danhMucThongBao, HttpPostedFileBase Logo)
        {
            try
            {
                if (Logo != null)
                {
                    danhMucThongBao.Logo = UploadCreateDir(Logo, "");
                }
                db.Entry(danhMucThongBao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(danhMucThongBao);
            } 
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var item = db.DanhMucThongBaos.Find(id);
                db.DanhMucThongBaos.Remove(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
