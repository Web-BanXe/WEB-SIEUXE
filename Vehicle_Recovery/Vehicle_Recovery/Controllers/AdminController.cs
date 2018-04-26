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
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        public ActionResult Xe(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.Xes.ToList().OrderBy(n => n.MaXe).ToPagedList(pageNumber, pageSize));
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
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc password không đúng";
            }
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public ActionResult ThemmoiXe()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaDongXe = new SelectList(db.LoaiXes.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.HangSX = new SelectList(db.HangXes.ToList().OrderBy(n => n.TenHX), "MaHX", "TenHX");
            Xe xe = new Xe();
            xe.TenXe = "";
            xe.ThanhTien = 0;
            xe.NamSX = DateTime.Now;
            xe.KhuyenMai = 0;
            xe.HinhAnh = "";
            xe.ThanhTien = 0;
            return View(xe);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiXe(Xe xe, HttpPostedFileBase fileupload, FormCollection form)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaDongXe = new SelectList(db.LoaiXes.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.HangSX = new SelectList(db.HangXes.ToList().OrderBy(n => n.TenHX), "MaHX", "TenHX");
            int hangxe = int.Parse(form["HangSX"]);
            int loaixe = int.Parse(form["MaDongXe"]);
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn hình ảnh";
                return View(xe);
            }
            else
            {
                if (ModelState.IsValid)
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
                    DongXe dx = db.DongXes.SingleOrDefault(n => n.Loai == loaixe && n.MaHangXe == hangxe);
                    if (dx != null)
                    {
                        xe.DongXe = dx.MaDongXe;
                        db.Xes.InsertOnSubmit(xe);
                    } else
                    {
                        dx = new DongXe();
                        dx.MaHangXe = hangxe;
                        dx.Loai = loaixe;
                        dx.Xes.Add(xe);
                        db.DongXes.InsertOnSubmit(dx);
                    }
                    xe.HinhAnh = filename;
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Xe");
        }

        public ActionResult Chitietxe(int id)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            Xe xe = db.Xes.SingleOrDefault(n => n.MaXe == id);
            ViewBag.MaXe = xe.MaXe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xe);
        }
        public ActionResult Xoaxe(int id)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            Xe xe = db.Xes.SingleOrDefault(n => n.MaXe == id);
            ViewBag.MaXe = xe.MaXe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xe);
        }
        [HttpPost, ActionName("Xoaxe")]
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
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

        public ActionResult Suaxe(int maxe)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaDongXe = new SelectList(db.LoaiXes.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.HangSX = new SelectList(db.HangXes.ToList().OrderBy(n => n.TenHX), "MaHX", "TenHX");
            Xe xe = db.Xes.SingleOrDefault(n => n.MaXe == maxe);
            return View(xe);
        }

        [HttpPost]
        public ActionResult Suaxe(Xe xe, HttpPostedFileBase fileupload, FormCollection form)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaDongXe = new SelectList(db.LoaiXes.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.HangSX = new SelectList(db.HangXes.ToList().OrderBy(n => n.TenHX), "MaHX", "TenLoai");
            int loaixe = xe.DongXe1.Loai;
            int hangxe = xe.DongXe1.MaHangXe;
            if (fileupload == null)
            {
                Xe xe1 = db.Xes.SingleOrDefault(n => n.MaXe == xe.MaXe);
                DongXe dongxe = db.DongXes.SingleOrDefault(n => n.MaHangXe == hangxe && n.Loai == loaixe);
                if(dongxe == null)
                {
                    dongxe = new DongXe();
                    dongxe.Loai = loaixe;
                    dongxe.MaHangXe = hangxe;
                    db.DongXes.InsertOnSubmit(dongxe);
                    db.SubmitChanges();
                }
                xe1.TenXe = xe.TenXe;
                xe1.DongXe = dongxe.MaDongXe;
                xe1.ThanhTien = xe.ThanhTien;
                xe1.NamSX = xe.NamSX;
                xe1.NgayBan = xe.NgayBan;
                xe1.NgayDang = xe.NgayDang;
                xe1.MoTa = xe.MoTa;
                xe1.KhuyenMai = xe.KhuyenMai;
                db.SubmitChanges();
                return RedirectToAction("Xe", "Admin");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileupload.FileName);

                    var path = Path.Combine(Server.MapPath("~/Img"), filename);

                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình đã tồn tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }

                    Xe xe1 = db.Xes.SingleOrDefault(n => n.MaXe == xe.MaXe);
                    xe1.HinhAnh = filename;
                    xe1.TenXe = xe.TenXe;
                    xe1.ThanhTien = xe.ThanhTien;
                    xe1.NamSX = xe.NamSX;
                    xe1.NgayBan = xe.NgayBan;
                    xe1.NgayDang = xe.NgayDang;
                    xe1.MoTa = xe.MoTa;
                    xe1.KhuyenMai = xe.KhuyenMai;
                    db.SubmitChanges();
                }
                return RedirectToAction("Xe");
            }

        }

        public ActionResult DangXuat()
        {
            Session["Taikhoanadmin"] = null;
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult HangXe(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pagesize = 20;
            int pagenum = (page ?? 1);
            var hx = db.HangXes.OrderBy(n => n.TenHX);
            return View(hx.ToPagedList(pagenum, pagesize));
        }

        public ActionResult ThemHangXe()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(new HangXe());
        }

        [HttpPost]
        public ActionResult ThemHangXe(HangXe hx)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            db.HangXes.InsertOnSubmit(hx);
            db.SubmitChanges();
            return RedirectToAction("HangXe", "Admin");
        }

        public ActionResult SuaHangXe(int mahx)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            HangXe hx = db.HangXes.SingleOrDefault(n => n.MaHX == mahx);
            return View(hx);
        }

        [HttpPost]
        public ActionResult SuaHangXe(HangXe hx)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            HangXe hangxe = db.HangXes.SingleOrDefault(n => n.MaHX == hx.MaHX);
            hangxe.TenHX = hx.TenHX;
            hangxe.QuocGia = hx.QuocGia;
            db.SubmitChanges();
            return RedirectToAction("HangXe","Admin");
        }

        public ActionResult DongXe(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageSize = 40;
            int pageNum = (page ?? 1);
            var dx = db.LoaiXes.OrderBy(n => n.TenLoai);
            return View(dx.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ThemDongXe()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(new LoaiXe());
        }
        [HttpPost]
        public ActionResult ThemDongXe(LoaiXe loai)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            db.LoaiXes.InsertOnSubmit(loai);
            db.SubmitChanges();
            return RedirectToAction("DongXe");
        }

        public ActionResult SuaDongXe(int madongxe)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            LoaiXe loai = db.LoaiXes.SingleOrDefault(n => n.MaLoai == madongxe);
            return View(loai);
        }
        [HttpPost]
        public ActionResult SuaDongXe(LoaiXe loai)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            if (string.IsNullOrEmpty(loai.TenLoai))
            {
                ViewData["Loi"] = "Tên dòng xe không được bỏ trống";
                return View(loai);
            }
            LoaiXe loaixe = db.LoaiXes.SingleOrDefault(n => n.MaLoai == loai.MaLoai);
            loaixe.TenLoai = loai.TenLoai;
            db.SubmitChanges();
            return RedirectToAction("DongXe", "Admin");
        }
        
        public ActionResult DonDatHang(int ?page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageSize = 20;
            int pageNum = (page ?? 1);
            var donhang = db.DonDatHangs.OrderByDescending(n => n.NgayDat);
            return View(donhang.ToPagedList(pageNum, pageSize));
        }

        public ActionResult SuaDonDatHang(int soddh)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var donhang = db.DonDatHangs.SingleOrDefault(n => n.SoDDH == soddh);
            return View(donhang);
        }

        [HttpPost]
        public ActionResult SuaDonDatHang(FormCollection form)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int soddh = int.Parse(form["SoDDH"]);
            bool dathanhtoan = bool.Parse(form["DaThanhToan"]);
            bool dagiaohang = bool.Parse(form["DaGiaoHang"]);
            DonDatHang ddh = db.DonDatHangs.SingleOrDefault(n => n.SoDDH == soddh);
            ddh.DaGiaoHang = dagiaohang;
            ddh.DaThanhToan = dathanhtoan;
            db.SubmitChanges();
            return RedirectToAction("DonDatHang","Admin");
        }

        public ActionResult ThongTinDonHang(int soddh)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            DonDatHang ddh = db.DonDatHangs.SingleOrDefault(n => n.SoDDH == soddh);
            return View(ddh);
        }

        public ActionResult ChiTietDDH(int soddh)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var ctddt = db.CTDDHs.Where(n => n.SoDDH == soddh);
            return PartialView(ctddt);
        }

        public ActionResult DoanhThu()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var xes = db.Xes;
            ViewBag.TongSoLuong = db.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == DateTime.Now.Month && n.DonDatHang.NgayGiao.Year == DateTime.Now.Year).Sum(n => n.SoLuong);
            ViewBag.TongTien = db.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == DateTime.Now.Month && n.DonDatHang.NgayGiao.Year == DateTime.Now.Year).Sum(n => n.DonGia * n.SoLuong * ( 1 - (float)n.KhuyenMai/100));
            foreach(var xe in xes)
            {
                ViewData["sl " + xe.MaXe] = xe.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == DateTime.Now.Month && n.DonDatHang.NgayGiao.Year == DateTime.Now.Year).Sum(n => n.SoLuong);
                ViewData["tsl " + xe.MaXe] = string.Format("{0:0.00}%",(float)xe.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == DateTime.Now.Month && n.DonDatHang.NgayGiao.Year == DateTime.Now.Year).Sum(n => n.SoLuong) / ViewBag.TongSoLuong * 100);
                ViewData["tt "+xe.MaXe] = string.Format("{0:0,000} VNĐ", xe.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == DateTime.Now.Month && n.DonDatHang.NgayGiao.Year == DateTime.Now.Year).Sum(n => n.DonGia * n.SoLuong * (1 - (float)n.KhuyenMai / 100)));
                ViewData["ttt " + xe.MaXe] = string.Format("{0:0.00}%",((float)xe.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == DateTime.Now.Month && n.DonDatHang.NgayGiao.Year == DateTime.Now.Year).Sum(n => n.DonGia * n.SoLuong * (1 - (float)n.KhuyenMai / 100)) / ViewBag.TongTien) * 100);
            }
            ViewBag.Month = DateTime.Now.Month;
            ViewBag.Year = DateTime.Now.Year;
            ViewBag.Err = false;
            return View(xes);
        }
        [HttpPost]
        public ActionResult DoanhThu(FormCollection form)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            string[] time = form["date"].Split('/');
            int month, year;
            if(!int.TryParse(time[0], out month) || !int.TryParse(time[1],out year))
            {
                ViewData["loi"] = "Thời gian không hợp lệ";
                return DoanhThu();
            } else if(month < 1 || month > 12 || year < 2010 || year > DateTime.Now.Year)
            {
                ViewData["loi"] = "Thời gian không hợp lệ";
                return DoanhThu();
            } else
            {
                try
                {
                    var xes = db.Xes;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                    ViewBag.TongSoLuong = db.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == month && n.DonDatHang.NgayGiao.Year == year).Sum(n => n.SoLuong);
                    ViewBag.TongTien = db.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == month && n.DonDatHang.NgayGiao.Year == year).Sum(n => n.DonGia * n.SoLuong * (1 - (float)n.KhuyenMai / 100));
                    foreach (var xe in xes)
                    {
                        ViewData["sl " + xe.MaXe] = xe.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == month && n.DonDatHang.NgayGiao.Year == year).Sum(n => n.SoLuong);
                        ViewData["tsl " + xe.MaXe] = string.Format("{0:0.00}%", (float)xe.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == month && n.DonDatHang.NgayGiao.Year == year).Sum(n => n.SoLuong) / ViewBag.TongSoLuong * 100);
                        ViewData["tt " + xe.MaXe] = string.Format("{0:0,000} VNĐ", xe.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == month && n.DonDatHang.NgayGiao.Year == year).Sum(n => n.DonGia * n.SoLuong * (1 - (float)n.KhuyenMai / 100)));
                        ViewData["ttt " + xe.MaXe] = string.Format("{0:0.00}%", ((float)xe.CTDDHs.Where(n => n.DonDatHang.NgayGiao.Month == month && n.DonDatHang.NgayGiao.Year == year).Sum(n => n.DonGia * n.SoLuong * (1 - (float)n.KhuyenMai / 100)) / ViewBag.TongTien) * 100);
                    }
                    ViewBag.Err = false;
                    return View(xes);
                } catch(Exception)
                {
                    ViewBag.Err = true;
                    ViewData["ThongBaoLoi"] = "Không có báo cáo nào trong thời gian này";
                    return View();
                }
            }
        }
    }
}