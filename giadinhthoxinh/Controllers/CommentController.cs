using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;  // ✅ Quan trọng
using giadinhthoxinh.Models;

namespace giadinhthoxinh.Controllers
{
    public class CommentController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // Load danh sách bình luận cho sản phẩm
        public PartialViewResult LoadComment(int productId)
        {
            var comments = db.tblComments
                .Include("tblUser") // lấy luôn thông tin người dùng
                .Where(c => c.FK_iProductID == productId && c.bStatus == true)
                .OrderByDescending(c => c.dtCreateDate)
                .ToList();

            return PartialView("_CommentList", comments);
        }


        // Thêm bình luận mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(int productId, string content)
        {
            if (Session["idUser"] == null)
                return RedirectToAction("Login", "User");

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "⚠️ Nội dung bình luận không được để trống!";
                return RedirectToAction("ProductDetail", "Home", new { id = productId });
            }

            var cmt = new tblComment
            {
                FK_iProductID = productId,
                FK_iAccountID = (int)Session["idUser"],
                sContent = content,
                dtCreateDate = DateTime.Now,
                bStatus = true
            };

            db.tblComments.Add(cmt);
            db.SaveChanges();

            TempData["Success"] = "✅ Bình luận thành công!";
            return RedirectToAction("ProductDetail", "Home", new { id = productId });
        }
    }
}