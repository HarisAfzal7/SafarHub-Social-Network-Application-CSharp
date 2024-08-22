using Newtonsoft.Json;
using SaFarHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SaFarHub.Controllers
{
    public class FriendRequestController : Controller
    {
        SaFarHubDatabaseEntities db = new SaFarHubDatabaseEntities();
        // GET: FriendRequest

        public JsonResult SendFriendRequest(string username)
        {
            try
            {
                string myUsername = (string)Session["username"];
                FriendRequest friendRequest = new FriendRequest();
                friendRequest.friendUsername = myUsername;
                friendRequest.toWhomeRequestUsername = username;
                friendRequest.requestDateAndTime = DateTime.Now;

                db.FriendRequests.Add(friendRequest);
                db.SaveChanges();

                return Json(new { error = "Ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "An Error Occurr" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AcceptFriendRquest(string username)
        {
            //In CSHTML using JavaScript
            //I have also need to handle the button of accept user and cancle for this action
            //also need to look for sending request
            //because all these event is handled by only one button.
            //I have to set onclick function for request as request(), accept() for accept, cancel() for cancel
            //in request and accept only one function will be there onclick functionality


            try
            {
                string myUsername = (string)Session["username"];
                FriendRequest friendRequest = db.FriendRequests.Where(user => (user.toWhomeRequestUsername == myUsername && user.friendUsername==username)).FirstOrDefault();
                
                if(friendRequest != null)
                {
                    UsersFriend usersFriend = new UsersFriend();
                    usersFriend.username = myUsername;
                    usersFriend.friendsUsername = username;

                    db.FriendRequests.Remove(friendRequest);
                    db.UsersFriends.Add(usersFriend);
                    db.SaveChanges();

                    return Json(new { error = "Ok" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { error = "Request Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = "An Error Occurr" }, JsonRequestBehavior.AllowGet);

            }

        }



        public JsonResult CancelFriendRquest(string username)
        {
            try
            {
                string myUsername = (string)Session["username"];
                FriendRequest friendRequest = db.FriendRequests.Where(user => ((user.toWhomeRequestUsername == myUsername && user.friendUsername == username) || (user.toWhomeRequestUsername == username && user.friendUsername == myUsername))).FirstOrDefault();

                if (friendRequest!=null){


                    db.FriendRequests.Remove(friendRequest);
                    db.SaveChanges() ;

                    return Json(new { error = "Ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "Request Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = "An Error Occurr" }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}