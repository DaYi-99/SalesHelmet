using SalesHelmet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesHelmet.Controllers
{
    public class GioHangController : Controller
    {
        dbSQL_HelmetsDataContext data = new dbSQL_HelmetsDataContext();


        public List<Giohang> laygiohang()
        {
            List<Giohang> lstgh = Session["Giohang"] as List<Giohang>;
            if(lstgh==null)
            {
                // nếu  giỏ hàng chưa tồn tại thì khởi tạo
                lstgh = new List<Giohang>();
                Session["Giohang"] = lstgh;
            }
            return lstgh;
        }

        // Thêm sản phẩm vào giỏ hàng
        public ActionResult ThemGioHang(int imasp, string url)
        {
            // Lấy ra session giỏ hàng
            List<Giohang> lstgh = laygiohang();

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            Giohang sp = lstgh.Find(n => n.iMaSP == imasp);
            if(sp==null)
            {
                sp = new Giohang(imasp);
                lstgh.Add(sp);
                return Redirect(url);
            }
            else
            {   // nếu có rồi thì cộng thêm
                sp.iSoluong++;
                return Redirect(url);
            }
        }

        // tính tồng số lượng
        private int TongSL()
        {
            int iTongSL = 0;
            List<Giohang> lstgh = Session["Giohang"] as List<Giohang>;
            if(lstgh!=null)
            {
                iTongSL = lstgh.Sum(n => n.iSoluong);
            }
            return iTongSL;
        }

        // tính tỗng
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstgh = Session["Giohang"] as List<Giohang>;
            if (lstgh != null)
            {
                iTongTien = lstgh.Sum(n => n.iThanhtien);
            }
            return iTongTien;
        }


        // Xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            // Lấy ra session giỏ hàng
            List<Giohang> lstgh = laygiohang();
            if(lstgh.Count==0)
            {
                return RedirectToAction("Index", "Helmet");
            }
            ViewBag.TongSL = TongSL();
            ViewBag.TongTien = TongTien();
            return View(lstgh);
        }


        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TongSL();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult XoaSP(int imasp)
        {
            // Lấy ra session giỏ hàng
            List<Giohang> lstgh = laygiohang();

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            Giohang sp = lstgh.Find(n => n.iMaSP == imasp);

            
            if(sp!=null)
            {
                lstgh.RemoveAll(n => n.iMaSP == imasp);
                return RedirectToAction("GioHang");
            }
            if (lstgh.Count == 0)
            {
                return RedirectToAction("GioHang", "GioHang");
            }
            return RedirectToAction("GioHang");
        }


        public ActionResult CapNhatSP(int imasp, FormCollection f)
        {
            // Lấy ra session giỏ hàng
            List<Giohang> lstgh = laygiohang();

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            Giohang sp = lstgh.Find(n => n.iMaSP == imasp);

            // Nếu tồn tại thì cho sửa số lượng
            if (sp != null)
            {
                sp.iSoluong  = int.Parse(f["txtSL"].ToString());
            }
            return RedirectToAction("GioHang");
        }


        public ActionResult XoaGH()
        {
            // Lấy ra session giỏ hàng
            List<Giohang> lstgh = laygiohang();
            lstgh.Clear();
            return RedirectToAction("GioHang", "GioHang");
        }

        private double Total()
        {
            double gh = 30000;
            double total = TongTien() + gh;
            return total;
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            //kiểm tra đăng nhập
            //if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            //{
            //    return RedirectToAction("User", "KhachHang");
            //}
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Helmet");
            }
            List<Giohang> lstgh = laygiohang();
            ViewBag.TongSL = TongSL();
            ViewBag.TongTien = TongTien();
            ViewBag.Total = Total();
            return View(lstgh);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            //Them don hang
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<Giohang> gh = laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.TTGH = false;
            ddh.ThanhToan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();

            //them chi tiet don hang
            foreach(var item in gh)
            {
                CTDH ctdh = new CTDH();
                ctdh.MaDH = ddh.MaDH;
                ctdh.MaSP = item.iMaSP;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.iDongia;
                data.CTDHs.InsertOnSubmit(ctdh);
            }

            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("index", "Helmet");

        }







        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
    }
}