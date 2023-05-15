using SalesHelmet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesHelmet.Controllers
{
    public class HelmetController : Controller
    {
        dbSQL_HelmetsDataContext data = new dbSQL_HelmetsDataContext();


        private List<SANPHAM> laysanpham(int count)
        {
            return data.SANPHAMs.OrderBy(a => a.MaSP).Take(count).ToList();
        }


        // GET: Helmet
        public ActionResult Index()
        {
            var spmoi = laysanpham(12);
            return View(spmoi);
        }

        public ActionResult MatHang()
        {
            var mathang = from mh in data.MATHANGs select mh;
            return PartialView(mathang);
        }

        public ActionResult SPtheoMH(int id)
        {
            var sp = from s in data.SANPHAMs where s.MaMH == id select s;
            return View(sp);
        }

        public ActionResult Details(int id)
        {
            var sp = from s in data.SANPHAMs where s.MaSP == id select s;
            return View(sp.Single());
        }
    }
}