﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Recovery.Models;
using PagedList;
using PagedList.Mvc;
namespace Vehicle_Recovery.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        VehicleDataContext db = new VehicleDataContext();

        public ActionResult SanPham (int masp)
        {
            Xe xe = db.Xes.SingleOrDefault(n => n.MaXe == masp);
            return PartialView(xe);
        }
        public ActionResult SanPhamMoiPartial ()
        {
            var xes = (from xe in db.Xes
                       where xe.NgayBan <= DateTime.Now
                       orderby xe.NgayBan descending
                       select xe).Take(4);
            return PartialView(xes);
        }
        
        public ActionResult ThongTin (int masp)
        {
            var xe = db.Xes.SingleOrDefault(n => n.MaXe == masp);
            return View(xe);
        }

        public ActionResult ThongSoXe (int masp)
        {
            var thongsos = db.ThongSos.Where(n => n.MaXe == masp);
            return PartialView(thongsos);
        }

        public ActionResult SanPhamMoi (int? page)
        {
            int pagesize = 40;
            int pagenum = (page ?? 1);
            var xes = db.Xes.OrderByDescending(n => n.NgayBan);
            return View(xes.ToPagedList(pagenum,pagesize));
        }
        

    }
}