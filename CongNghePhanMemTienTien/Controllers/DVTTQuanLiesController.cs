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
    public class DVTTQuanLiesController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();
        private string pathFile = "/Files/NHAN-VIEN-QUAN-LY-THUC-TAP/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/Images/";
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
            var dVTTQuanLies = db.DVTTQuanLies.Include(d => d.DonViThucTap);
            return View(dVTTQuanLies.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DVTTQuanLy dVTTQuanLy = db.DVTTQuanLies.Find(id);
            if (dVTTQuanLy == null)
            {
                return HttpNotFound();
            }
            return View(dVTTQuanLy);
        }

        public ActionResult Create()
        {
            ViewBag.DVTTID = new SelectList(db.DonViThucTaps, "ID", "TenDonVi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DVTTQuanLy dVTTQuanLy)
        {
            try
            {
                dVTTQuanLy.CreatedDate = DateTime.Now;
                db.DVTTQuanLies.Add(dVTTQuanLy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.DVTTID = new SelectList(db.DonViThucTaps, "ID", "TenDonVi", dVTTQuanLy.DVTTID);
                return View(dVTTQuanLy);
            }

        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DVTTQuanLy dVTTQuanLy = db.DVTTQuanLies.Find(id);
            if (dVTTQuanLy == null)
            {
                return HttpNotFound();
            }
            ViewBag.DVTTID = new SelectList(db.DonViThucTaps, "ID", "TenDonVi", dVTTQuanLy.DVTTID);
            return View(dVTTQuanLy);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenQuanLy,Link,SoDienThoai,Email,DVTTID,IsDelete,CreatedDate,UpdatedDate,DeletedDate,CreatedUser,UpdatedUser,DeletedUser")] DVTTQuanLy dVTTQuanLy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dVTTQuanLy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DVTTID = new SelectList(db.DonViThucTaps, "ID", "TenDonVi", dVTTQuanLy.DVTTID);
            return View(dVTTQuanLy);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DVTTQuanLy dVTTQuanLy = db.DVTTQuanLies.Find(id);
            if (dVTTQuanLy == null)
            {
                return HttpNotFound();
            }
            return View(dVTTQuanLy);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DVTTQuanLy dVTTQuanLy = db.DVTTQuanLies.Find(id);
            db.DVTTQuanLies.Remove(dVTTQuanLy);
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
