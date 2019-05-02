using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CongNghePhanMemTienTien.DataContext;
using System.IO;
using ImageResizer;

namespace CongNghePhanMemTienTien.Controllers
{
    public class DonViThucTapController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();
        private string pathFile = "/Files/DON-VI-THUC-TAP/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/Images/";
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
            var danhsachDVTT = db.DonViThucTaps.ToList();
            return View(danhsachDVTT);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(DonViThucTap dvtt, HttpPostedFileBase Logo)
        {
            try
            {
                if(Logo !=null)
                {
                    dvtt.Logo = UploadCreateDir(Logo, "");
                }
                else
                {
                    return View(dvtt);
                }
                dvtt.CreatedDate = DateTime.Now; 
                dvtt.UpdatedDate = DateTime.Now;
                dvtt.IsDelete = false; 
                db.DonViThucTaps.Add(dvtt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(dvtt);
            }
        }

        public ActionResult Edit(int id)
        {
            var dvtt = db.DonViThucTaps.Find(id);
            return View(dvtt);
        }

        [HttpPost]
        public ActionResult Edit(DonViThucTap dvtt, HttpPostedFileBase Logo)
        {
            try
            {
                if (Logo != null)
                {
                    dvtt.Logo = UploadCreateDir(Logo, "");
                }
                dvtt.UpdatedDate = DateTime.Now;
                db.Entry(dvtt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges(); //Lưu dữ liệu vào db
                return RedirectToAction("Index"); //Lưu thành công sẽ trả về danh sách
            }
            catch
            {
                return View(dvtt); //Thất bại trả về view edit
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var item = db.DonViThucTaps.Find(id);
                db.DonViThucTaps.Remove(item);
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