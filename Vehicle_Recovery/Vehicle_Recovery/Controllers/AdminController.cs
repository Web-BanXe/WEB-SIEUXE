using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Recovery.Models;

namespace Vehicle_Recovery.Controllers
{
    public class AdminController : Controller
    {
        VehicleDataContext db = new VehicleDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tenadmin = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tenadmin))
            {
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập tên password";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.id == tenadmin && n.password == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc password không đúng";
            }
            return View();
        }
    }
}