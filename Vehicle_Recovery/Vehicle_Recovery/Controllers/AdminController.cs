using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Recovery.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

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
        public ActionResult Xe(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.Xes.ToList().OrderBy(n=>n.MaXe).ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tenadmin = collection["tenadmin"];
            var matkhau = collection["matkhau"];
            if (String.IsNullOrEmpty(tenadmin))
            {
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập password";
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
            return RedirectToAction("Index","Admin");
        }
        [HttpGet]
        public ActionResult ThemmoiXe()
        {
            ViewBag.MaDongXe = new SelectList(db.DongXes.ToList().OrderBy(n => n.TenDongXe), "MaDongXe", "TenDongXe");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiXe(Xe xe,HttpPostedFileBase fileupload)
        {
            ViewBag.MaDongXe = new SelectList(db.DongXes.ToList().OrderBy(n => n.TenDongXe), "MaDongXe", "TenDongXe");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn hình ảnh";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileupload.FileName);

                    var path = Path.Combine(Server.MapPath("~/Img"), filename);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    xe.HinhAnh = filename;
                    db.Xes.InsertOnSubmit(xe);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Xe");
        }

        public ActionResult Chitietxe(int id)
        {
            Xe xe = db.Xes.SingleOrDefault(n => n.MaXe == id);
            ViewBag.MaXe = xe.MaXe;
            if(xe==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xe);
        }
        public ActionResult Xoaxe(int id)
        {
            Xe xe = db.Xes.SingleOrDefault(n => n.MaXe == id);
            ViewBag.MaXe = xe.MaXe;
            if(xe==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xe);
        }
        [HttpPost,ActionName("Xoaxe")]
        public ActionResult Xacnhanxoa(int id)
        {
            Xe xe = db.Xes.SingleOrDefault(n => n.MaXe == id);
            ViewBag.MaXe = xe.MaXe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Xes.DeleteOnSubmit(xe);
            db.SubmitChanges();
            return RedirectToAction("Xe");
        }

        public ActionResult Suaxe(Xe xe,HttpPostedFileBase fileupload)
        {
            ViewBag.MaDongXe = new SelectList(db.DongXes.ToList().OrderBy(n => n.TenDongXe), "MaDongXe", "TenDongXe");
            if(fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn hình ảnh";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileupload.FileName);

                    var path = Path.Combine(Server.MapPath("~/Img"), filename);

                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình đã tồn tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    xe.HinhAnh = filename;
                    UpdateModel(xe);
                    db.SubmitChanges();
                }
                return RedirectToAction("Xe");
            }
            
        }
    }
}