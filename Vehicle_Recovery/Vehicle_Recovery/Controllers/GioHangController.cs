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
    }
}