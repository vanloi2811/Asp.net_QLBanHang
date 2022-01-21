using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanHang.Models;
using System.Collections;

namespace QLBanHang.Controllers
{
    public class MenuController : Controller
    {
        private QLBHEntities db = new QLBHEntities();
        // GET: Menu
        public ActionResult Index()
        {
            var loaisp = db.LoaiSPs.ToList();
            Hashtable arrLoaiSP = new Hashtable();
            foreach (var item in loaisp)
            {
                arrLoaiSP.Add(item.MaLoaiSP, item.TenLoaiSP);
            }
            ViewBag.LoaiSP = arrLoaiSP;
            return PartialView("Index");
        }
    }
}