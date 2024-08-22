using SaFarHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaFarHub.Controllers
{
    public class PicturesLikeController : Controller
    {
        SaFarHubDatabaseEntities db = new SaFarHubDatabaseEntities();
        // GET: FriendRequest

        [HttpPost]
        public ActionResult PostLiked(int postId)
        {
            try
            {
                string myUsername = (string)Session["username"];
                PicturesLike picturesLike = new PicturesLike();
                picturesLike.postID = postId;
                picturesLike.username = myUsername;

                db.PicturesLikes.Add(picturesLike);
                db.SaveChanges();
                return RedirectToAction("ViewHome");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ViewHome");
            }
        }

        [HttpPost]
        public ActionResult PostDisLiked(int postId)
        {
            try
            {
                string myUsername = (string)Session["username"];
                PicturesLike picturesLike = db.PicturesLikes.Where(post => post.username == myUsername).FirstOrDefault();

                db.PicturesLikes.Remove(picturesLike);
                db.SaveChanges();
                return RedirectToAction("ViewHome");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ViewHome");
            }
        }


        //public JsonResult PostLiked(int postId)
        //{
        //    try
        //    {
        //        string myUsername = (string)Session["username"];
        //        PicturesLike picturesLike = new PicturesLike();
        //        picturesLike.postID = postId;
        //        picturesLike.username = myUsername;

        //        db.PicturesLikes.Add(picturesLike);
        //        db.SaveChanges();

        //        return Json(new { error = "Ok" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { error = "An Error Occurr" }, JsonRequestBehavior.AllowGet);
        //    }
        //}



        //public JsonResult PostDisLiked(int postId)
        //{
        //    //Same as request and Accept 
        //    //for the same button there will be two onclick functions 
        //    //like() and dislike()
        //    //at a time one functions will work
        //    //The ID for the post like button will be like or dislike


        //    try
        //    {
        //        string myUsername = (string)Session["username"];
        //        PicturesLike picturesLike = db.PicturesLikes.Where(post=>post.username ==  myUsername).FirstOrDefault();

        //        db.PicturesLikes.Remove(picturesLike);
        //        db.SaveChanges();

        //        return Json(new { error = "Ok" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { error = "An Error Occurr" }, JsonRequestBehavior.AllowGet);
        //    }
        //}


    }
}