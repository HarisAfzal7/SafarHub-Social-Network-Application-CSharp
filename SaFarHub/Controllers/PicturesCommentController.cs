using SaFarHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SaFarHub.Controllers
{
    public class PicturesCommentController : Controller
    {   
        SaFarHubDatabaseEntities db = new SaFarHubDatabaseEntities();
        // GET: PicturesComment

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult PostComment(int postId, string commentValue)
        {
            try
            {
                string username = (string)Session["username"];
                PicturesComment comment = new PicturesComment();
                comment.pictureID = postId;
                comment.username = username;
                comment.commentText = commentValue;
                comment.commentedDateTime = DateTime.Now;

                db.PicturesComments.Add(comment);
                db.SaveChanges();
                return Json(new { error = username}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}