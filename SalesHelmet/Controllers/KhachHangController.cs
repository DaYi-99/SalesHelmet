using SalesHelmet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesHelmet.Controllers
{
    public class KhachHangController : Controller
    {

        dbSQL_HelmetsDataContext db = new dbSQL_HelmetsDataContext();

        // GET: KhachHang
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult User()
        {
            return View();
        }

        [HttpPost]
        public ActionResult User(string tendn, string matkhau)
        {
            //tác vụ đăng ký
            string un = tendn;
            string pw = matkhau;
            if (un.ToString()!="" && pw.ToString()!="")
            {
                //gán các giá trị cho đối tượng
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.UN == tendn && n.PW == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "Helmet");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}