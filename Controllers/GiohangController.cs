using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanHang.Models;
using System.Net.Mail;
using System.Net;

namespace QLBanHang.Controllers
{
    public class GiohangController : Controller
    {
        private QLBHEntities db = new QLBHEntities();
        // GET: Giohang
        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return View(giohang);
        }
        public RedirectToRouteResult AddToCart(string MaSP)
        {
            if(Session["giohang"] == null) //chưa có trong giỏ hàng
            {
                Session["giohang"] = new List<CartItem>(); //tạo mới
            }
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            if(giohang.FirstOrDefault(m=>m.MaSP == MaSP) == null)
            {
                SanPham sp = db.SanPhams.Find(MaSP);
                CartItem item = new CartItem();
                item.MaSP = MaSP;
                item.TenSP = sp.TenSP;
                item.DonGia = Convert.ToDouble(sp.Dongia);
                item.SoLuong = 1;
                giohang.Add(item);

            }
            else // vì sản phẩm đã có -> tăng 1 số lượng
            {
                CartItem item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
                item.SoLuong++;
            }
            Session["giohang"] = giohang;
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Update(string MaSP, int txtSoluong)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            
            CartItem item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
            if(item != null)
            {
                item.SoLuong = txtSoluong;
                Session["giohang"] = giohang;
            }    
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Delete(string MaSP)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;

            CartItem item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
            if (item != null)
            {
                giohang.Remove(item);
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Order(string Email, string Phone)
        {
            // gửi mail cho khách hàng về thông tin đặt hàng
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            string sMsg = "<html><body><table><caption>Thông tin đặt hàng</caption><tr><th>STT</th><th>Tên hàng</th><th>Số lượng</th><th>Đơn giá</th><th>Thành tiền</th></tr>";
            int i = 0;
            double tongTien = 0;
            foreach (CartItem item in giohang)
            {
                i++;
                sMsg += "<tr>";
                sMsg += "<td>" + i.ToString() + "</td>";
                sMsg += "<td>" + item.TenSP + "</td>";
                sMsg += "<td>" + item.SoLuong.ToString() + "</td>";
                sMsg += "<td>" + item.DonGia.ToString() + "</td>";
                sMsg += "<td>" + String.Format("{0:#,###}", item.SoLuong * item.DonGia) + "</td>";
                sMsg += "</tr>";
                tongTien += item.ThanhTien;
            }
            sMsg += "<tr><th colspan='5'>Tổng cộng: " + String.Format("{0:#,### vnđ}", tongTien) + "</th></tr></table>";
            MailMessage mail = new MailMessage("qlbh01@gmail.com", Email, "Thông tin đơn hàng", sMsg);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("qlbh01", "Qlbanhang");
            mail.IsBodyHtml = true;
            client.Send(mail);
            return RedirectToAction("Index", "Home");
        }
    }
}