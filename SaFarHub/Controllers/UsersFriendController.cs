using SaFarHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaFarHub.Controllers
{
    public class UsersFriendController : Controller
    {
        SaFarHubDatabaseEntities db = new SaFarHubDatabaseEntities();
        // GET: UsersFriends



        public ActionResult UnFriendUser(string username)
        {
            string myUsername = (string)Session["username"];
            UsersFriend myFriend = db.UsersFriends.Where(friend => ((friend.username == username && friend.friendsUsername == myUsername) || (friend.username == myUsername && friend.friendsUsername == username))).FirstOrDefault();
            if (myFriend != null)
            {
                db.UsersFriends.Remove(myFriend);
                db.SaveChanges();
                return RedirectToAction("Friends","User");
            }
            return RedirectToAction("Friends", "User");
        }

        public ActionResult Profile(string username)
        {
            // Retrieve the user profile based on the provided username
            // TODO: Implement your logic to fetch the user profile from the database or any other source

            // Example: Fetch the user profile using the provided username
            var userProfile = db.Users.FirstOrDefault(u => u.username == username);

            if (userProfile != null)
            {
                // TODO: Pass the user profile to the view and display it
                var userRole = db.Roles.FirstOrDefault(u => u.roleID == userProfile.role);
                ViewBag.profilingUser = userProfile;
                ViewBag.myRole = userRole;
                return View();
            }
            else
            {
                // Handle the case when the user profile is not found
                return RedirectToAction("ViewHome", "User"); // Redirect to the homepage or an error page
            }
        }




        public ActionResult ShowAllFriends()
        {

            //Here work is required 
            //Fetch all user data and select only friends in cshtml and other by if condition

            User user = (User)Session["User"];
            ViewBag.userFriends = db.UsersFriends.Where(uFriend => ((uFriend.username == user.username) || (uFriend.friendsUsername == user.username))).ToList();
            ViewBag.otherUsers = db.UsersFriends.Where(otherUser => otherUser.username != user.username).ToList();
            return View();
        }

        //Adding friend request to the database
        [HttpPost]
        public ActionResult FriendRequest(UsersFriend friend)
        {
            //Need to code further here
            int iResult = 0;
            User userFriend = db.Users.Find(friend.username);
            if (userFriend != null)
            {
                User user = (User)Session["User"];
                FriendRequest friendRequest = new FriendRequest
                {
                    friendUsername = user.username,
                    toWhomeRequestUsername = userFriend.username
                };
                db.FriendRequests.Add(friendRequest);
                iResult = db.SaveChanges();
            }
            return PartialView(iResult);
        }

        //Adding friend and deleting the friend request
        [HttpPost]
        public ActionResult FriendResponse(FriendRequest friendRequest)
        {
            int iResult = 0;
            FriendRequest friendRequestTem = db.FriendRequests.Find(friendRequest.friendRequestID);
            if (friendRequestTem != null)
            {
                User user = (User)Session["User"];
                UsersFriend usersFriend = new UsersFriend
                {
                    username = user.username,
                    friendsUsername = friendRequest.friendUsername
                };
                db.UsersFriends.Add(usersFriend);
                db.FriendRequests.Remove(friendRequestTem);
                iResult = db.SaveChanges();
            }
            return PartialView(iResult);
        }

        //Cancelling friend request
        [HttpPost]
        public ActionResult FriendResponseCancel(FriendRequest friendRequest)
        {
            int iResult = 0;
            FriendRequest friendRequestTem = db.FriendRequests.Find(friendRequest.friendRequestID);
            if (friendRequestTem != null)
            {
                db.FriendRequests.Remove(friendRequestTem);
                iResult = db.SaveChanges();
            }
            return PartialView(iResult);
        }


        [HttpPost]
        public JsonResult Like(int id)
        {
            return Json(new { id });
        }
    }
}