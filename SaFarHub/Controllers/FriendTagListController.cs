using SaFarHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SaFarHub.Controllers
{
    public class FriendTagListController : Controller
    {
        SaFarHubDatabaseEntities db = new SaFarHubDatabaseEntities();

        // GET: FriendTagList
        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult FetchTagingUsers(string partialUsername)
        //{
        //    //var tagingUsers = db.Users.Where(tagUser => tagUser.username.Contains(partialUsername)).ToList();
        //    var user = db.Users.Find("f2020065010");
        //    //return Json(tagingUsers, JsonRequestBehavior.AllowGet);
        //    return Json(user, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetAllMyTags()
        {
            try
            {
                string username = (string)Session["username"];

                List<FriendTagList> friendTagList = db.FriendTagLists.Where(tag=>tag.friendUsernameWhoIsTagged==username).ToList();
                if(friendTagList.Count>0)
                {
                    FetchedTaggedNotifications fetchedTaggedNotifications = new FetchedTaggedNotifications();
                    foreach (var item in friendTagList)
                    {
                        if (item != null)
                        {
                            fetchedTaggedNotifications.TaggingID.Add(item.taggingID);
                            fetchedTaggedNotifications.PostID.Add(item.postID);
                            fetchedTaggedNotifications.FriendUserName.Add(item.friendUsername);
                            fetchedTaggedNotifications.FriendUserNameWhoIsTagged.Add(item.friendUsernameWhoIsTagged);
                            fetchedTaggedNotifications.NotificationSeen.Add(item.notificationSeen);
                            fetchedTaggedNotifications.ClickedAndSeen.Add(item.clickedAndSeen);
                            fetchedTaggedNotifications.TaggedDateTime.Add(item.taggedDateTime);
                        }
                    }
                    var json = JsonConvert.SerializeObject(fetchedTaggedNotifications);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "Tag not found" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { error = "Tag not found" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FetchTagingUsers(string partialUsername)
        {
            try
            {

                var user = db.Users.Where(tagUser => tagUser.username.Contains(partialUsername)).ToList();
                //var user = db.Users.ToList();
                if (user != null)
                {
                    FetchTaggedUsernames fetchTaggedUsernames = new FetchTaggedUsernames();
                    foreach (var item in user)
                    {
                        if(item != null)
                        {
                            fetchTaggedUsernames.SimilerUsernames.Add(item.username);
                        }
                    }
                    var json = JsonConvert.SerializeObject(fetchTaggedUsernames);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "User not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = partialUsername }, JsonRequestBehavior.AllowGet);
            }
        }


        public HttpStatusCodeResult InsertTaggings(string[] taggedUsers)
        {
            string errorMessage = "";
            if (taggedUsers.Length > 0)
            {

                try
                {
                    foreach (var singleUser in taggedUsers)
                    {
                        if (singleUser != null)
                        {

                            FriendTagList friendTagList = new FriendTagList();
                            friendTagList.friendUsername = (string)Session["username"];
                            friendTagList.friendUsernameWhoIsTagged = Regex.Unescape(singleUser);
                            friendTagList.postID = db.Pictures.Max(p_id => p_id.pictureID);
                            friendTagList.taggedDateTime = DateTime.Now;
                            friendTagList.notificationSeen = false;
                            friendTagList.clickedAndSeen = false;

                            db.FriendTagLists.Add(friendTagList);
                            db.SaveChanges();
                        }
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.OK);

                }
                catch (Exception ex)
                {
                    errorMessage = "An error occurred."; // Provide an appropriate error message

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorMessage);
                }

            }

            errorMessage = "No Friend Is Tagged."; // Provide an appropriate error message

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}