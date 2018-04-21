using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Recovery.Models;

namespace Vehicle_Recovery.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        public static bool FlagUser { get; set; } = true;
        VehicleDataContext db = new VehicleDataContext();
        
        private User SearchUser (string taikhoan, string matkhau)
        {
            User kh = db.Users.SingleOrDefault(n => n.User1 == taikhoan);
            return kh;
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            if(Session["User"] == null)
            {
                return PartialView();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection form)
        {
            ViewBag.Code = 1;
            string taikhoan = form["taikhoan"];
            string matkhau = form["matkhau"];
            string matkhaunhaplai = form["matkhaunhaplai"];
            string quequan = form["quequan"];
            string dienthoai = form["dienthoai"];
            string ngaysinh = string.Format("{0:MM/dd/yyyy}", form["ngaysinh"]);
            string diachi = form["diachi"];
            string gioitinh = form["gioitinh"];
            string hoten = form["hoten"];
            string email = form["email"];
            DateTime bd;
            bool err = false;

            if (taikhoan.Length < 6)
            {
                ViewData["taikhoan"] = "Tài khoản không đủ độ dài";
                err = true;
            }

            if (matkhau.Length < 6)
            {
                ViewData["matkhau"] = "Mật khẩu không đủ độ dài";
                err = true;
            }

            if (string.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["matkhaunhaplai"] = "Mật khẩu nhập lại không được để trống";
                err = true;
            }

            if (string.IsNullOrEmpty(quequan))
            {
                ViewData["quequan"] = "Quê quán không được để trống";
                err = true;
            }

            if (dienthoai.Length < 9 || dienthoai.Length > 11)
            {
                ViewData["dienthoai"] = "Số điện thoại không hợp lệ";
                err = true;
            }

            if (!DateTime.TryParse(ngaysinh,out bd))
            {
                ViewData["ngaysinh"] = "Bạn chưa chọn ngày sinh";
                err = true;
            }

            if (string.IsNullOrEmpty(diachi))
            {
                ViewData["diachi"] = "Địa chỉ không được để trống";
                err = true;
            }

            if (string.IsNullOrEmpty(email))
            {
                ViewData["email"] = "E-mail không được để trống";
                err = true;
            }

            //if (string.IsNullOrEmpty(hoten))
            //{
            //    ViewData["hoten"] = "Họ tên không đươc để trống";
            //    err = true;
            //}
            
            if (err) return DangKy();

            User kh = db.Users.SingleOrDefault(n => n.User1 == taikhoan);
            if(kh != null)
            {
                ViewData["taikhoan"] = "Tài khoản này đã có người sử dụng";
                return DangKy();
            }
            kh = db.Users.SingleOrDefault(n => n.Email == email);
            if (kh != null)
            {
                ViewData["email"] = "E-mail này đã có người sử dụng";
                return DangKy();
            }
            kh = new User();
            kh.User1 = taikhoan;
            kh.Password = matkhau;
            kh.NgayDK = DateTime.Now;
            kh.QueQuan = quequan;
            kh.SDT = dienthoai;
            kh.Email = email;
            kh.HoVaTen = "AT";
            kh.NgaySinh = bd;
            kh.AnhDaiDien = "generic-user.png";
            db.Users.InsertOnSubmit(kh);
            db.SubmitChanges();
            ViewBag.Code = 0;
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public ActionResult DangNhapPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult DangNhapPartial(FormCollection form)
        {
            if(Session["User"] != null)
            {
                return PartialView();
            }
            ViewBag.Code = 0;
            string taikhoan = form["taikhoan"];
            string matkhau = form["matkhau"];
            Session["User"] = SearchUser(taikhoan,matkhau);
            if(Session["User"] == null)
            {
                ViewBag.ThongBao = "Tài khoản hoặc mật khẩu không đúng";
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection form)
        {
            string taikhoan = form["taikhoan"];
            string matkhau = form["matkhau"];
            Session["User"] = SearchUser(taikhoan, matkhau);
            if (Session["User"] == null)
            {
                ViewBag.ThongBao = "Tài khoản hoặc mật khẩu không đúng";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DangNhapDangKy(string URL)
        {
            ViewBag.URL = URL;
            return PartialView();
        }

        public ActionResult DieuKhien(string URL)
        {
            ViewBag.URL = URL;
            return PartialView();
        }

        public ActionResult DangXuat(string URL)
        {
            Session["User"] = null;
            return Redirect(URL);
        }
        [HttpGet]
        public ActionResult ThongTinCaNhan(string URL)
        {
            if(Session["User"] == null)
            {
                return Redirect(URL);
            }
            ViewBag.URL = URL;
            ViewData["hoten"] = "";
            ViewData["ngaysinh"] = "";
            ViewData["quequan"] = "";
            ViewData["dienthoai"] = "";
            ViewData["diachi"] = "";
            ViewData["email"] = "";
            User kh = (User)Session["User"];
            return View(kh);

        }
        [HttpPost]
        public ActionResult ThongTinCaNhan(FormCollection form,string URL)
        {
            User kh = (User)Session["User"];
            bool err = false;
            ViewBag.URL = URL;
            ViewData["hoten"] = "";
            ViewData["ngaysinh"] = "";
            ViewData["quequan"] = "";
            ViewData["dienthoai"] = "";
            ViewData["diachi"] = "";
            ViewData["email"] = "";
            DateTime birthday;
            string hoten = form["HoVaTen"];
            string ngaysinh = string.Format("{0:MM/dd/yyyy}",form["NgaySinh"]);
            string gioitinh = form["GioiTinh"];
            string quequan = form["QueQuan"];
            string dienthoai = form["SDT"];
            string diachi = form["DiaChiThuongTru"];
            string email = form["Email"];
            if (string.IsNullOrEmpty(hoten))
            {
                err = true;
                ViewData["hoten"] = "Họ và tên không được để trống";
            }
            if (string.IsNullOrEmpty(ngaysinh))
            {
                err = true;
                ViewData["ngaysinh"] = "Ngày sinh không được để trống";
            }
            if (string.IsNullOrEmpty(quequan))
            {
                err = true;
                ViewData["quequan"] = "Quê quán không được để trống";
            }
            if (string.IsNullOrEmpty(dienthoai))
            {
                err = true;
                ViewData["dienthoai"] = "Điện thoại không được để trống";
            }
            if (string.IsNullOrEmpty(diachi))
            {
                err = true;
                ViewData["diachi"] = "Địa chỉ không được để trống";
            }
            if (string.IsNullOrEmpty(email))
            {
                err = true;
                ViewData["email"] = "Email không được để trống";
            }
            if (err)
            {
                return View(kh);
            }
            if (!DateTime.TryParse(ngaysinh, out birthday))
            {
                ViewData["ngaysinh"] = "Ngày sinh không hợp lệ";
                return View(kh);
            }
            kh = db.Users.SingleOrDefault(n => n.User1 == kh.User1);
            kh.HoVaTen = hoten;
            kh.NgaySinh = birthday;
            kh.QueQuan = quequan;
            kh.GioiTinh = gioitinh == "1"?true:false;
            kh.Email = email;
            kh.DiaChiThuongTru = diachi;
            kh.SDT = dienthoai;
            db.SubmitChanges();
            Session["User"] = kh;
            return View(kh);
        }
        public ActionResult XemDonHang (string taikhoan)
        {
            User user = db.Users.SingleOrDefault(n => n.User1 == taikhoan);
            return View();
        }

        public ActionResult XemSanPhamYeuThich(string taikhoan)
        {
            User user = db.Users.SingleOrDefault(n => n.User1 == taikhoan);
            return View(user);
        }
    }
}