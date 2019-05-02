using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CongNghePhanMemTienTien.DataContext;

namespace CongNghePhanMemTienTien.Controllers
{
    public class PhuHuynhsController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();

        // GET: PhuHuynhs
        public ActionResult Index()
        {
            return View(db.PhuHuynhs.ToList());
        }

        // GET: PhuHuynhs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuHuynh phuHuynh = db.PhuHuynhs.Find(id);
            if (phuHuynh == null)
            {
                return HttpNotFound();
            }
            return View(phuHuynh);
        }

        // GET: PhuHuynhs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhuHuynhs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,HoTenCha,HoTenMe,Email,DiDong,DienThoai,DiaChi,IsDelete,CreatedDate,UpdatedDate,DeletedDate,CreatedUser,UpdatedUser,DeletedUser")] PhuHuynh phuHuynh)
        {
            if (ModelState.IsValid)
            {
                db.PhuHuynhs.Add(phuHuynh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phuHuynh);
        }

        // GET: PhuHuynhs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuHuynh phuHuynh = db.PhuHuynhs.Find(id);
            if (phuHuynh == null)
            {
                return HttpNotFound();
            }
            return View(phuHuynh);
        }

        // POST: PhuHuynhs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,HoTenCha,HoTenMe,Email,DiDong,DienThoai,DiaChi,IsDelete,CreatedDate,UpdatedDate,DeletedDate,CreatedUser,UpdatedUser,DeletedUser")] PhuHuynh phuHuynh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phuHuynh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phuHuynh);
        }

        // GET: PhuHuynhs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuHuynh phuHuynh = db.PhuHuynhs.Find(id);
            if (phuHuynh == null)
            {
                return HttpNotFound();
            }
            return View(phuHuynh);
        }

        // POST: PhuHuynhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhuHuynh phuHuynh = db.PhuHuynhs.Find(id);
            db.PhuHuynhs.Remove(phuHuynh);
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
