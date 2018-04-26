using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Recovery.Models;
using PagedList;
using PagedList.Mvc;

namespace Vehicle_Recovery.Controllers
{
    public class HomeController : Controller
    {
        VehicleDataContext db = new VehicleDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Trang web được làm ra chỉ vì mục đích học tập không nhằm để kinh doanh.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Bạn có thể liên hệ chúng tôi theo thông tin";

            return View();
        }

        public ActionResult LoaiXePartial()
        {
            var loais = from loai in db.LoaiXes select loai;
            return PartialView(loais);
        }

        public ActionResult HangXePartial()
        {
            var hangxes = db.HangXes;
            return PartialView(hangxes);
        }

        [HttpGet]
        public ActionResult GiaXePartial()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult GiaXePatial(FormCollection form)
        {
            int giadau = 0;
            int giacuoi = 0;
            string strgiadau = form["giadau"];
            string strgiacuoi = form["giacuoi"];
            if (string.IsNullOrEmpty(strgiadau))
            {
                if (string.IsNullOrEmpty(strgiacuoi))
                {
                    var xes = db.Xes;
                    return PartialView(xes);
                } else
                {
                    giacuoi = int.Parse(strgiacuoi);
                    var xes = db.Xes.Where(n => n.ThanhTien <= giacuoi);
                    return PartialView(xes);
                }
            }
            else
            {
                giadau = int.Parse(strgiadau);
                if (string.IsNullOrEmpty(strgiacuoi))
                {
                    var xes = db.Xes.Where(n => n.ThanhTien >= giadau);
                    return PartialView(xes);
                }else
                {
                    giacuoi = int.Parse(strgiacuoi);
                    var xes = db.Xes.Where(n => n.ThanhTien >= giadau && n.ThanhTien <= giacuoi);
                    return PartialView(xes);
                }
            }
        }

        public ActionResult HangXe(int mahx,int? page)
        {
            var xes = db.Xes.Where(n => n.DongXe1.MaHangXe == mahx);
            if(xes == null)
            {
                return HttpNotFound();
            } else
            {
                int pageSize = 20;
                int pageNum = (page ?? 1);
                if (xes.Count() != 0)
                {
                    ViewBag.HX = xes.FirstOrDefault().DongXe1.HangXe.TenHX;
                    ViewBag.MaHX = mahx;
                }
                return View(xes.ToPagedList(pageNum,pageSize));
            }
        }
        public ActionResult DongXe(int maloai, int? page)
        {
            var xes = db.Xes.Where(n => n.DongXe1.Loai == maloai);
            if(xes == null)
            {
                return HttpNotFound();
            } else
            {
                int pageSize = 20;
                int pageNum = (page ?? 1);
                if (xes.Count() != 0)
                {
                    ViewBag.HX = xes.FirstOrDefault().DongXe1.LoaiXe.TenLoai;
                    ViewBag.MaHX = maloai;
                }
                return View(xes.ToPagedList(pageNum, pageSize));
            }
        }
    }
}