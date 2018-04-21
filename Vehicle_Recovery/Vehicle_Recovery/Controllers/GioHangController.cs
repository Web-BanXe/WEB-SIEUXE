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
            if(Session["User"] != null)
            {
                List<GioHang> giohangs = LayGioHang();
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
                return View(giohangs);
            } else
            {
                return RedirectToAction("Index","Home");
            }
        }
    }
}