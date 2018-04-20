using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vehicle_Recovery.Models
{
    public class GioHang
    {
        private VehicleDataContext db = new VehicleDataContext();
        public int MaXe { get; set; }
        public string TenXe { get; set; }
        public int KhuyenMai { get; set; }
        public long DonGia { get; set; }
        public int SoLuong { get; set; }
        public string HinhAnh { get; set; }

        public GioHang(int maxe)
        {
            Xe xe = db.Xes.SingleOrDefault(n => n.MaXe == maxe);
            MaXe = xe.MaXe;
            TenXe = xe.TenXe;
            KhuyenMai = (xe.KhuyenMai ?? 0);
            DonGia = xe.ThanhTien;
            SoLuong = 1;
            HinhAnh = xe.HinhAnh;
        }
        public long TongTien
        {
            get
            {
                return DonGia * SoLuong * (100 - KhuyenMai)/100;
            }
        }
    }
}