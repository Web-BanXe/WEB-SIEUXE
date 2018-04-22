using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Recovery.Models;
namespace Vehicle_Recovery.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        VehicleDataContext db = new VehicleDataContext();
        private static List<GioHang> giohangs;
        private List<GioHang> LayGioHang()
        {
            if(giohangs == null)
            {
                giohangs = new List<GioHang>();
            }
            return giohangs;
        }
        private int TongSoLuong()
        {
            return LayGioHang().Sum(n => n.SoLuong);
        }
        private long TongThanhTien()
        {
            return LayGioHang().Sum(n => n.TongTien);
        }
        public ActionResult GioHangPartial(string URL)
        {
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView(LayGioHang());
        }

        public ActionResult ThemGioHang(int maxe,string URL)
        {
            if(Session["User"] != null)
            {
                List<GioHang> list = LayGioHang();
                GioHang xe = list.Find(n => n.MaXe == maxe);
                if (xe == null)
                {
                    xe = new GioHang(maxe);
                    list.Add(xe);
                }
                else
                {
                    xe.SoLuong++;
                }
            } else
            {
                KhachHangController.FlagUser = false;
            }
            return Redirect(URL);
        }
        public ActionResult GioHang()
        {
            if(Session["User"] != null && TongSoLuong() > 0)
            {
                List<GioHang> giohangs = LayGioHang();
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongTien = TongThanhTien();
                return View(giohangs);
            } else
            {
                return RedirectToAction("Index","Home");
            }
        }
        [HttpPost]
        public ActionResult GioHang(FormCollection form)
        {
            if(Session["User"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            List<GioHang> giohangs = LayGioHang();
            int sl = giohangs.Count;
            for(int i=0; i < sl; i++)
            {
                GioHang gh = giohangs.ElementAt(i);
                if(!string.IsNullOrEmpty(form["xoa " + gh.MaXe]))
                {
                    giohangs.RemoveAt(i);
                    i--;
                    sl--;
                } else
                {
                    string soluong = form["sl " + gh.MaXe];
                    int sol = 0;
                    if(int.TryParse(soluong,out sol) && sol > 0)
                    {
                        gh.SoLuong = sol;
                    } else
                    {
                        ViewData["sl " + gh.MaXe] = "Số lượng không hợp lệ";
                    }
                }
            }
            if(sl > 0)
            {
                return GioHang();
            } else
            {
                return RedirectToAction("Index","Home");
            }
        }

        public ActionResult XoaTatCa()
        {
            if(Session["User"] != null)
            {
                LayGioHang().Clear();
            }
            return GioHang();
        }

        public ActionResult ThanhToan ()
        {
            if(Session["User"] != null && TongSoLuong() > 0)
            {
                ViewBag.Flag = 0;
                return GioHang();
            } else
            {
                return RedirectToAction("Index","Home");
            }
        }
        [HttpPost]
        public ActionResult ThanhToan(FormCollection form)
        {
            User kh = (User)Session["User"];
            if(kh != null && TongSoLuong() > 0)
            {
                List<GioHang> giohangs = LayGioHang();
                DonDatHang ddh = new DonDatHang();
                ddh.KhachHang = kh.User1;
                ddh.NgayDat = DateTime.Now;
                ddh.NgayGiao = DateTime.Now.AddDays(2);
                ddh.DaGiaoHang = false;
                ddh.DaThanhToan = false;
                foreach(var giohang in giohangs)
                {
                    CTDDH ct = new CTDDH();
                    ct.SoLuong = giohang.SoLuong;
                    ct.Xe = giohang.MaXe;
                    ct.KhuyenMai = giohang.KhuyenMai;
                    ct.DonGia = giohang.DonGia;
                    ddh.CTDDHs.Add(ct);
                }
                db.DonDatHangs.InsertOnSubmit(ddh);
                db.SubmitChanges();
                ViewBag.Flag = 1;
                return View();
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
    }
}