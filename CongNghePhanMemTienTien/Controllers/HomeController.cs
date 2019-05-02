using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CongNghePhanMemTienTien.DataContext;
namespace CongNghePhanMemTienTien.Controllers
{
    public class HomeController : Controller
    {
        private CNPMTTEntities db = new CNPMTTEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            var item = db.UserAdmins.Where(s => s.UserName == userName && s.Password == password).FirstOrDefault();
            if (item != null)
            {
                Session["Admin"] = item;
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            } 
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}