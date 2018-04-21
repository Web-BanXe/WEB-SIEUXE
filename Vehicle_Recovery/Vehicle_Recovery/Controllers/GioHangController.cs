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
        
        private List<GioHang> LayGioHang()
        {
            List<GioHang> giohangs = Session["GioHang"] as List<GioHang>;
            if(giohangs == null)
            {
                giohangs = new List<GioHang>();
            }
            return giohangs;
        }
        private int TongSoLuong()
        {
            int TongSoLuong = 0;
            List<GioHang> giohangs = Session["GioHang"] as List<GioHang>;
            if (giohangs != null)
            {
                return LayGioHang().Sum(n => n.SoLuong);
            }
            return TongSoLuong;
        }
        private long TongThanhTien()
        {
            long TongThanhTien = 0;
            List<GioHang> giohangs = Session["GioHang"] as List<GioHang>;
            if (giohangs != null)
            {
                return LayGioHang().Sum(n => n.TongTien);
            }
            return TongThanhTien;
        }
        public ActionResult GioHangPartial()
        {
            
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return PartialView(LayGioHang());
        }

        public ActionResult ThemGioHang(int maxe,string URL)
        {
            List<GioHang> list = LayGioHang();
            GioHang xe = list.Find(n => n.MaXe == maxe);
            if(xe == null)
            {
                xe = new GioHang(maxe);
                list.Add(xe);
            } else
            {
                xe.SoLuong++;
            }
            return Redirect(URL);
        }
        public ActionResult Giohang()
        {
            List<GioHang> giohangs = LayGioHang();
            if (giohangs.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return PartialView(LayGioHang());
        }

        public ActionResult Dathang(FormCollection collection)
        {
            DonDatHang ddh = new DonDatHang();
            User user = (User)Session["taikhoan"];
            List<GioHang> gh = LayGioHang();
            ddh.KhachHang = user.HoVaTen;
            ddh.NgayDat = DateTime.Now;
            var ngaygiao = string.Format("{0:dd/MM/yyyy}", collection["Ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.DaGiaoHang = false;
            ddh.DaThanhToan = false;
            db.DonDatHangs.InsertOnSubmit(ddh);
            db.SubmitChanges(); 
            foreach(var item in gh)
            {
                CTDDH ctdh = new CTDDH();
                ctdh.SoDDH = ddh.SoDDH;
                ctdh.Xe = item.MaXe;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = (int)item.DonGia;
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}