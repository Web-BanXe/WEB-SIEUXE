using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Recovery.Models;

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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LoaiXePartial()
        {
            var loais = (from loai in db.DongXes select loai.TenDongXe).Distinct();
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
    }
}