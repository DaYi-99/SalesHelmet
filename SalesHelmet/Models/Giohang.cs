using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesHelmet.Models
{
    public class Giohang
    {
        dbSQL_HelmetsDataContext data = new dbSQL_HelmetsDataContext();

        public int iMaSP { set; get; }
        public string iTenSP { set; get; }
        public string iAnhbia { set; get; }
        public Double iDongia { set; get; }

        public int iSoluong { set; get; }
        public Double iThanhtien { 
            get { return iSoluong * iDongia; }
        }


        //khởi tạo giỏ hàng theo mã sách với số lượng mặc định là 1
        public Giohang(int MaSP)
        {
            iMaSP = MaSP;
            SANPHAM sp = data.SANPHAMs.Single(n => n.MaSP == iMaSP);
            iTenSP = sp.TenSP;
            iAnhbia = sp.Anhbia;
            iDongia = double.Parse(sp.Giaban.ToString());
            iSoluong = 1;
        }

    }
}