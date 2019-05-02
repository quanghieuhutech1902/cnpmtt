using CongNghePhanMemTienTien.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CongNghePhanMemTienTien.Controllers
{
    public class DiemDanhsController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();
        public ActionResult Index(int id = 1)
        {
            var lst = new List<DiemDanhSV>();
            if (Session["Admin"] != null)
            {
                var user = Session["Admin"] as UserAdmin;
                if (user.Type == 1)
                {
                    var iuser = db.GiangViens.Where(s => s.Email == user.UserName).FirstOrDefault();
                    if (iuser != null)
                    {
                        id = iuser.ID;
                        ViewBag.tenGV = db.GiangViens.Find(id).TenGiangVien.ToUpper();
                        lst = db.DiemDanhSVs.Where(s => s.MaGV == id && s.Ngay.Value.Day == DateTime.Now.Day && s.Ngay.Value.Month == DateTime.Now.Month && s.Ngay.Value.Year == DateTime.Now.Year && s.IsCheck == false).ToList();
                    }
                }
            }
            else
            {
                lst = db.DiemDanhSVs.Where(s => s.Ngay.Value.Day == DateTime.Now.Day && s.Ngay.Value.Month == DateTime.Now.Month && s.Ngay.Value.Year == DateTime.Now.Year).ToList();
            }
            return View(lst);
        }
        public ActionResult ClassDetail(int id)
        {
            var item = db.DiemDanhSVs.Find(id);
            ViewBag.pr = item;
            var lst = db.SinhViens.Where(s => s.LopID == item.MaLop).ToList();
            return View(lst);
        }
        public void CapNhat(int? id, int? id_)
        {
            try
            {
                ChiTietDiemDanh it = db.ChiTietDiemDanhs.Where(s => s.MaSV == id && s.MaDiemDanhSV == id_).FirstOrDefault();
                if (it != null)
                {
                    if (it.TinhTrang == true)
                    {
                        it.TinhTrang = false;
                    }
                    else
                    {
                        it.TinhTrang = true;
                    }
                    db.Entry(it).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    ChiTietDiemDanh dt = new ChiTietDiemDanh();
                    dt.MaDiemDanhSV = id_;
                    dt.MaSV = id;
                    dt.TinhTrang = true;
                    db.ChiTietDiemDanhs.Add(dt);
                    db.SaveChanges();
                }

            }
            catch
            {
                //log
            }
        }
        public ActionResult Update(int id)
        {
            DiemDanhSV dd = db.DiemDanhSVs.Find(id);
            dd.IsCheck = true;
            db.Entry(dd).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}