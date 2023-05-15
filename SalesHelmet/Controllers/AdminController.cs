using SalesHelmet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using PagedList.Mvc;

namespace SalesHelmet.Controllers
{
    public class AdminController : Controller
    {

        dbSQL_HelmetsDataContext db = new dbSQL_HelmetsDataContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //gan gia tri nguoi dung nhap vao
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.UN == tendn && n.PW == matkhau);
                if (ad != null)
                {
                    Session["Admin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.ThongBao = "Sai thông tin đăng nhập";
            }
            return View();
        }

        public ActionResult SanPham(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.SANPHAMs.ToList().OrderBy(n=>n.MaSP).ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        public ActionResult ThemSP()
        {
            ViewBag.MAMH = new SelectList(db.MATHANGs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSP(SANPHAM sp, HttpPostedFileBase up)
        {
            ViewBag.MAMH = new SelectList(db.MATHANGs.ToList().OrderBy(n => n.TenMH), "MaMH", "TenMH");

            //kiem tra duong dan
            if(up == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //them vao co so du lieu
            else
            {
                //luu ten file
                var filename = Path.GetFileName(up.FileName);
                //luu duong dan cua file
                var path = Path.Combine(Server.MapPath("~/Assets/images/product/"), filename);
                //kiem tra hinh anh ton tai chua
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    //luu hinh anh vao duong dan
                    up.SaveAs(path);
                }
                sp.Anhbia = filename;
                //luu vao csdl
                db.SANPHAMs.InsertOnSubmit(sp);
                db.SubmitChanges();
            }
            return RedirectToAction("SanPham");
        }
    }
}