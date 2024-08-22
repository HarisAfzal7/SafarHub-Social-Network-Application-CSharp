using Newtonsoft.Json;
using SaFarHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaFarHub.Controllers
{
    public class UserController : Controller
    {
        // GET: Users
        private readonly SaFarHubDatabaseEntities db = new SaFarHubDatabaseEntities();

        public ActionResult OtherUsersProfile(string username)
        {
            //username = "f2020065010";
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
                return RedirectToAction("ShowAllUsers", "User"); // Redirect to the homepage or an error page
            }
        }

        public ActionResult ShowFriendRequests()
        {
            string currentUsername = (string)Session["username"];
            List<FriendRequest> sendRequest = db.FriendRequests.Where(send=>send.friendUsername == currentUsername).ToList();
            List<FriendRequest> receiveRequest = db.FriendRequests.Where(receive=>receive.toWhomeRequestUsername == currentUsername).ToList();
            if (sendRequest != null)
            {
                List<User> sentRequestUser = new List<User>();
                foreach (FriendRequest item in sendRequest)
                {
                    if (item != null)
                    {
                        User temUser = db.Users.Where(user => user.username == item.toWhomeRequestUsername).FirstOrDefault();
                        sentRequestUser.Add(temUser);
                    }
                }
                if (sentRequestUser.Count > 0) {
                    ViewBag.sentRequestUser = sentRequestUser;
                }
            }
            if (receiveRequest != null)
            {
                List<User> receiveRequestUser = new List<User>();
                foreach (FriendRequest item in receiveRequest)
                {
                    if (item != null)
                    {
                        User temUser = db.Users.Where(user => user.username == item.friendUsername).FirstOrDefault();
                        receiveRequestUser.Add(temUser);
                    }
                }
                if (receiveRequestUser.Count > 0)
                {
                    ViewBag.receiveRequestUser = receiveRequestUser;
                }
            }
            return View();
        }


        public ActionResult ShowAllUsers()
        {
            string currentUsername = (string)Session["username"];
            // Retrieve the usernames of the user's friends from the UsersFriends table
            var friendUsernamesQuery = db.UsersFriends
                .Where(uf => uf.username == currentUsername || uf.friendsUsername == currentUsername)
                .Select(uf => uf.username == currentUsername ? uf.friendsUsername : uf.username)
                .ToList();

            // Retrieve the friend users from the Users table
            var friendsQuery = db.Users
                .Where(u => friendUsernamesQuery.Contains(u.username))
                .ToList();

            var allUsers = db.Users.ToList();

            if (friendUsernamesQuery.Count>0) { 
                ViewBag.friends = friendsQuery;
            }
            if (allUsers.Count > 0) { 
                ViewBag.allUsers = allUsers;
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddFriend(string username)
        {

            // Add the friend logic here
            // Retrieve the user and friend based on the username
            string currentUsername = (string)Session["username"];
            User friend = db.Users.Where(user=>user.username==username).FirstOrDefault();
            if (friend != null)
            {
                FriendRequest friendRequest= new FriendRequest();
                friendRequest.friendUsername = currentUsername;
                friendRequest.toWhomeRequestUsername = username;
                friendRequest.requestDateAndTime = DateTime.Now;
                db.FriendRequests.Add(friendRequest);
                db.SaveChanges();

                //return RedirectToAction("Profile", "User", new { username = friendUser.Username });
                return RedirectToAction("ShowAllUsers");
            }
            else
            {
                return RedirectToAction("ShowAllUsers");
            }

        }
        [HttpPost]
        public ActionResult CancelRequest(string username) {
            string currentUsername = (string)Session["username"];
            FriendRequest friendRequest = db.FriendRequests.Where(request => (request.friendUsername == currentUsername && request.toWhomeRequestUsername == username) || (request.toWhomeRequestUsername == currentUsername && request.friendUsername == username)).FirstOrDefault();
            if (friendRequest != null)
            {
                db.FriendRequests.Remove(friendRequest);
                db.SaveChanges();
                return RedirectToAction("ViewHome");
            }
            else
            {
                return RedirectToAction("ViewHome");
            }
        }
        [HttpPost]
        public ActionResult AcceptRequest(string username)
        {
            string currentUsername = (string)Session["username"];
            FriendRequest friendRequest = db.FriendRequests.Where(request => (request.friendUsername == currentUsername && request.toWhomeRequestUsername == username) || (request.toWhomeRequestUsername == currentUsername && request.friendUsername == username)).FirstOrDefault();
            if (friendRequest != null)
            {
                UsersFriend usersFriend = new UsersFriend();
                usersFriend.username = currentUsername;
                usersFriend.friendsUsername = username;

                db.UsersFriends.Add(usersFriend);
                db.FriendRequests.Remove(friendRequest);
                db.SaveChanges();
                return RedirectToAction("ViewHome");
            }
            else
            {
                return RedirectToAction("ViewHome");
            }
        }

        //Tem
        public ActionResult Friends()
        {
            // Retrieve the username of the current user from the session or authentication context
            string currentUsername = (string)Session["username"];
            //string currentUsername = "f2020065010";

            // Retrieve the usernames of the user's friends from the UsersFriends table
            var friendUsernamesQuery = db.UsersFriends
                .Where(uf => uf.username == currentUsername || uf.friendsUsername == currentUsername)
                .Select(uf => uf.username == currentUsername ? uf.friendsUsername : uf.username)
                .ToList();

            // Retrieve the friend users from the Users table
            var friendsQuery = db.Users
                .Where(u => friendUsernamesQuery.Contains(u.username))
                .ToList();

            if (friendsQuery.Count > 0) { 
                ViewBag.friends = friendsQuery;
            }

            return View();
        }


        //Worked but only retrienved my name:

        //public ActionResult Friends()
        //{
        //    // Retrieve the username of the current user from the session or authentication context
        //    string currentUsername = "f2020065010";

        //    // Retrieve the user's friends and users who added the user as a friend from the database
        //    var friendsQuery = (from u in db.Users
        //                        join uf in db.UsersFriends on u.username equals uf.username
        //                        where u.username == currentUsername || uf.friendsUsername == currentUsername
        //                        select new
        //                        {
        //                            u.username,
        //                            u.firstName,
        //                            u.lastName,
        //                            u.profilePicture,
        //                            u.role,
        //                            u.registrationDate
        //                        }).ToList();

        //    // Create instances of the User class using the fetched data
        //    var friends = friendsQuery.Select(f => new User
        //    {
        //        username = f.username,
        //        firstName = f.firstName,
        //        lastName = f.lastName,
        //        profilePicture = f.profilePicture,
        //        role = f.role,
        //        registrationDate = f.registrationDate
        //    }).ToList();

        //    ViewBag.friends = friends;

        //    return View();
        //}



        //this one worked


        //public ActionResult Friends()
        //{
        //    // Retrieve the username of the current user from the session or authentication context
        //    string currentUsername = "f2020065010";

        //    // Retrieve the user's friends and users who added the user as a friend from the database
        //    var friendsQuery = (from u in db.Users
        //                        join uf in db.UsersFriends on u.username equals uf.username
        //                        where u.username == currentUsername || uf.friendsUsername == currentUsername
        //                        select new
        //                        {
        //                            Username = (u.username == currentUsername) ? uf.friendsUsername : u.username,
        //                            // Include other necessary properties
        //                        }).ToList();

        //    // Create instances of the User class using the fetched data
        //    var friends = friendsQuery.Select(f => new User
        //    {
        //        username = f.Username,
        //        // Include other necessary properties
        //    }).ToList();

        //    ViewBag.friends = friends;

        //    return View();
        //}




        //public ActionResult Friends()
        //{
        //    // Retrieve the username of the current user from the session or authentication context
        //    string currentUsername = "f2020065010";

        //    // TODO: Initialize your database context

        //    // Retrieve the user's friends and users who added the user as a friend from the database
        //    var friends = db.Users
        //        .Join(db.UsersFriends, u => u.username, uf => uf.username, (u, uf) => new { User = u, Friend = uf.friendsUsername })
        //        .Where(uf => uf.User.username == currentUsername || uf.Friend == currentUsername)
        //        .Select(uf => new User
        //        {
        //            username = uf.User.username == currentUsername ? uf.Friend : uf.User.username,
        //            // Include other necessary properties
        //        })
        //        .ToList();


        //    //FriendsViewModel viewModel = new FriendsViewModel
        //    //{
        //    //    Friends = friends
        //    //};

        //    ViewBag.friends = friends;
        //    //return View(viewModel);
        //    return View();
        //}




        public ActionResult ViewHome()
        {
            string username = (string)Session["username"];


            // Retrieve the username of the current user from the session or authentication context
            string currentUsername = username;

            // Retrieve the usernames of the user's friends from the UsersFriends table
            var friendUsernamesQuery = db.UsersFriends
                .Where(uf => uf.username == currentUsername || uf.friendsUsername == currentUsername)
                .Select(uf => uf.username == currentUsername ? uf.friendsUsername : uf.username).ToList();

            // Retrieve the friend users from the Users table
            var friendsQuery = db.Users.Where(u => friendUsernamesQuery.Contains(u.username)).ToList();



            //User IAm = (User)Session["User"];
            //friendProfileData.Add(IAm);
            //ViewBag.friends = db.UsersFriends.Where(friend => (friend.username == username || friend.friendsUsername == username));
            ViewBag.friends = friendsQuery;
            ViewBag.friendRequestRecieve = db.FriendRequests.Where(friend => (friend.toWhomeRequestUsername == username));
            ViewBag.friendRequestSend = db.FriendRequests.Where((friend) => (friend.friendUsername == username));

            ViewBag.tagList = db.FriendTagLists.Where(taglist => taglist.friendUsernameWhoIsTagged == username);

            List<List<Picture>> PictureList = new List<List<Picture>>();
            List<List<PicturesComment>> PicturesComment = new List<List<PicturesComment>>();
            List<PicturesLike> PicturesLike = new List<PicturesLike>();

            foreach (var item in friendsQuery)
            {
                if (item != null)
                {
                    List<Picture> temPicture = db.Pictures.Where(picture => (picture.username == item.username)).ToList();
                    PictureList.Add(temPicture);
                }
            }

            List<Picture> pictures = db.Pictures.Where(p => p.username == username).ToList();
            PictureList.Add(pictures);
            //List<User> usersFriends = db.UsersFriends.Include("Users").ToList();
            //ViewBag.userFriends = db.UsersFriends.Include("Users").ToList();

            foreach (List<Picture> pictureListForComment in PictureList)
            {
                if (pictureListForComment.Count>0)
                {
                    foreach (var item in pictureListForComment)
                    {
                        if (item != null)
                        {
                            List<PicturesComment> singlePictureComments = db.PicturesComments.Where(comment => comment.pictureID == item.pictureID).ToList();

                            PicturesLike temUsersLikes = db.PicturesLikes.Where(like => like.postID == item.pictureID && like.username == username).FirstOrDefault();
                            if(temUsersLikes!=null)
                            {
                                PicturesLike.Add(temUsersLikes);
                            }
                            if (singlePictureComments!= null)
                            {
                                PicturesComment.Add(singlePictureComments);
                            }
                        }
                    }
                }
            }


            if (PictureList != null)
            {
                ViewBag.PostList = PictureList;
                if (PicturesComment != null)
                {
                    ViewBag.PostCommentList = PicturesComment;
                }
                if (PicturesLike != null)
                {
                    ViewBag.PostLikeList = PicturesLike;
                }
            }

            return View();
        }



        public JsonResult GetUsername()
        {
            string username = (string)Session["username"];
            var json = JsonConvert.SerializeObject(username);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public User FindAdmin(string username)
        {
            var admin = db.Users.Find(username);
            return admin;
        }


        public ActionResult Post()
        {
            return RedirectToAction("Post", "Picture");
        }
        public ActionResult Home()
        {
            string username = (string)Session["username"];

            List<UsersFriend> usersFriends = db.UsersFriends.Where(friend => (friend.username == username || friend.friendsUsername == username)).ToList();
            ViewBag.friends = db.UsersFriends.Where(friend => (friend.username == username || friend.friendsUsername == username));
            ViewBag.friendRequestRecieve = db.FriendRequests.Where(friend => (friend.toWhomeRequestUsername == username));
            ViewBag.friendRequestSend = db.FriendRequests.Where((friend) => (friend.friendUsername == username));

            ViewBag.tagList = db.FriendTagLists.Where(taglist => taglist.friendUsernameWhoIsTagged == username);

            List<List<Picture>> PictureList = new List<List<Picture>> ();
            List<List<PicturesComment>> PicturesComment = new List<List<PicturesComment>>();
            List<PicturesLike> PicturesLike = new List<PicturesLike>();

            foreach (var item in usersFriends)
            {
                if (item != null)
                {
                    List<Picture> temPicture = db.Pictures.Where(picture=>(picture.username==item.username || picture.username == item.friendsUsername)).ToList();
                    PictureList.Add(temPicture);
                }
            }

            List<Picture> pictures = db.Pictures.Where(p => p.username == username).ToList();
            PictureList.Add(pictures);
            //List<User> usersFriends = db.UsersFriends.Include("Users").ToList();
            //ViewBag.userFriends = db.UsersFriends.Include("Users").ToList();

            foreach (List<Picture> pictureListForComment in PictureList)
            {
                if (pictureListForComment != null)
                {
                    foreach (var item in pictureListForComment)
                    {
                        if(item != null)
                        {
                            List<PicturesComment> singlePictureComments = db.PicturesComments.Where(comment => comment.pictureID == item.pictureID).ToList();
                            PicturesLike.Add(db.PicturesLikes.Where(like => like.postID == item.pictureID && like.username == username).FirstOrDefault());
                            PicturesComment.Add(singlePictureComments);
                        }
                    }
                }
            }


            if (PictureList != null)
            {
                ViewBag.PostList = PictureList;
                if(PicturesComment != null)
                {
                    ViewBag.PostCommentList = PicturesComment;
                }
                if (PicturesLike != null)
                {
                    ViewBag.PostLikeList = PicturesLike;
                }
            }


            //if (pictures != null)
            //{
            //    ViewBag.allPost = pictures;
            //    foreach (Picture item in pictures)
            //    {
            //        List<PicturesLike> listOfLikes = new List<PicturesLike>();
            //        List<PicturesComment> listOfComments = new List<PicturesComment>();
            //        if (item != null)
            //        {
            //            List<PicturesLike> pictureLikes = db.PicturesLikes.Where(p => p.likeID == item.pictureID).ToList();
            //            List<PicturesComment> pictureComments = db.PicturesComments.Where(p => p.commentID == item.pictureID).ToList();
            //            listOfLikes.AddRange(pictureLikes);
            //            listOfComments.AddRange(pictureComments);

            //        }
            //        if (listOfLikes != null)
            //        {
            //            ViewBag.postLikes = listOfLikes;
            //        }
            //        if (listOfComments != null)
            //        {
            //            ViewBag.postComments = listOfComments;
            //        }
            //    }
            //}


            return RedirectToAction("ViewHome");
            //return View();
        }



        public ActionResult AdminHome()
        {
            int totalUsers = db.Users.Count();
            int totalPost = db.Pictures.Count();
            TempData["totalUsers"] = totalUsers;
            TempData["totalPost"] = totalPost;
            var userData = db.Users.ToList();
            var postData = db.Pictures.ToList();
            ViewBag.userData = userData;
            ViewBag.postData = postData;
            return View();
        }

        public ActionResult AllUsers()
        {
            int totalUsers = db.Users.Count();
            TempData["totalUsers"] = totalUsers;
            var userData = db.Users.ToList();
            ViewBag.userData = userData;
            return View();
        }

        public ActionResult AllPost()
        {
            int totalPost = db.Pictures.Count();
            TempData["totalPost"] = totalPost;
            var postData = db.Pictures.ToList();
            ViewBag.postData = postData;
            return View();
        }


        public ActionResult AddUserByAdmin()
        {
            ViewBag.roles = db.Roles.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserByAdmin(User user)
        {
            if (user != null)
            {
                User userTem = db.Users.Find(user.username);
                if (userTem != null)
                {
                    TempData["AlertMessage"] = "This username is already been register";
                    return RedirectToAction("AddUserByAdmin");
                }
                else
                {
                    if (user.ImageFile == null)
                    {
                        user.profilePicture = "~/Images/defaultProfilePicture.png";
                    }
                    else
                    {
                        string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
                        string extension = Path.GetExtension(user.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        user.profilePicture = "~/Images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        user.ImageFile.SaveAs(fileName);
                    }
                    user.registrationDate = DateTime.Now;
                    db.Users.Add(user);
                    db.SaveChanges();
                    TempData["UserAdded"] = "User added successfully";
                    return RedirectToAction("AddUserByAdmin");
                }
            }
            return RedirectToAction("AdminHome");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(User user)
        {
            if (user != null)
            {
                User userTem = db.Users.Find(user.username);
                if (userTem != null)
                {
                    List<Picture> pictures = db.Pictures.Where(p => p.username == userTem.username).ToList();
                    List<PicturesLike> picturesLikes = db.PicturesLikes.Where(u=>u.username== userTem.username).ToList();
                    List<PicturesComment> picturesComments = db.PicturesComments.Where(u => u.username == userTem.username).ToList();
                    List<FriendRequest> friendRequests = db.FriendRequests.Where(u=>u.friendUsername == userTem.username || u.toWhomeRequestUsername== userTem.username).ToList();
                    List<FriendTagList> friendTagLists = db.FriendTagLists.Where(u => u.friendUsername == userTem.username || u.friendUsernameWhoIsTagged == userTem.username).ToList();
                    List<UsersFriend> usersFriends= db.UsersFriends.Where(u => u.username == userTem.username || u.friendsUsername == userTem.username).ToList();

                    if (friendRequests.Count > 0)
                    {
                        foreach (var item in friendRequests)
                        {
                            if (item != null)
                            {
                                db.FriendRequests.Remove(item);
                                db.SaveChanges();
                            }
                        }
                    }
                    if (friendTagLists.Count > 0)
                    {
                        foreach (var item in friendTagLists)
                        {
                            if (item != null)
                            {
                                db.FriendTagLists.Remove(item);
                                db.SaveChanges();
                            }
                        }
                    }

                    if (picturesComments.Count > 0)
                    {
                        foreach (var item in picturesComments)
                        {
                            if (item != null)
                            {
                                db.PicturesComments.Remove(item);
                                db.SaveChanges();
                            }
                        }
                    }

                    if (picturesLikes.Count > 0)
                    {
                        foreach (var item in picturesLikes)
                        {
                            if (item != null)
                            {
                                db.PicturesLikes.Remove(item);
                                db.SaveChanges();
                            }
                        }
                    }
                    if (usersFriends.Count > 0)
                    {
                        foreach (var item in usersFriends)
                        {
                            if (item != null)
                            {
                                db.UsersFriends.Remove(item);
                                db.SaveChanges();
                            }
                        }
                    }

                    if (pictures.Count>0)
                    {
                        foreach (var item in pictures)
                        {
                            if (item != null)
                            {
                                db.Pictures.Remove(item);
                                db.SaveChanges();
                            }
                        }
                    }
                    db.Users.Remove(userTem);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("AdminHome", "User");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePicture(User user)
        {
            if (user != null)
            {
                int pictureId = user.role;
                Picture picture = db.Pictures.Find(pictureId);
                if (picture != null)
                {
                    db.Pictures.Remove(picture);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("AdminHome");
        }

        public ActionResult GeneralSetting()
        {
            return View();
        }
        public ActionResult PrivacySetting()
        {
            return View();
        }
        public ActionResult SecuritySetting()
        {
            return View();
        }
        public ActionResult EmailNotification()
        {
            return View();
        }

        // GET: UserDatas
        public ActionResult Index()
        {
            //var data1 =  db.UserDatas.Include("Employee").Include("").ToList();
            //var data2 =  db.UserDatas.ToList();
            //ViewBag.data1 = data1;
            //ViewBag.data2 = data2;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            if (user == null)
            {
                TempData["AlertMessage"] = "Please enter your data";
                return RedirectToAction("Registeration");
            }
            else
            {

                User userTem = db.Users.Find(user.username);
                if (userTem != null)
                {
                    TempData["AlertMessage"] = "This username is already been register";
                    return RedirectToAction("Registeration");
                }
                else
                {
                    if (user.firstName == null || user.lastName == null || user.username == null || user.password == null || user.email == null)
                    {
                        TempData["AlertMessage"] = "Please enter the missing data";
                        return RedirectToAction("Registeration");

                    }
                    else
                    {
                        //default
                        user.role = 1;
                        if (user.ImageFile == null)
                        {
                            user.profilePicture = "~/Images/defaultProfilePicture.png";
                        }
                        else
                        {
                            string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
                            string extension = Path.GetExtension(user.ImageFile.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            user.profilePicture = "~/Images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                            user.ImageFile.SaveAs(fileName);
                        }
                        user.registrationDate = DateTime.Now;
                        db.Users.Add(user);
                        db.SaveChanges();
                        //ModelState.Clear();
                        TempData["AlertMessage"] = "You have successfully created your account please login your account";
                        return RedirectToAction("Login");
                    }
                }
            }

        }

        public ActionResult Registeration()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        //This Needs to be edit for Admin login
        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user != null)
            {
                if (user.username != null && user.password != null)
                {
                    User userDataTem = db.Users.Find(user.username);

                    if (userDataTem != null)
                    {
                        if (userDataTem.password == user.password)
                        {
                            if (userDataTem.role == 1)
                            {
                                Session["User"] = userDataTem;
                                Session["username"] = (string)user.username;
                                return RedirectToAction("ViewHome");
                            }
                            else if (userDataTem.role == 2)
                            {
                                Session["Admin"] = userDataTem;
                                return RedirectToAction("AdminHome");
                            }
                        }
                        else
                        {
                            TempData["loginError"] = "User not found you may have entered incorrect username or password";
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        TempData["loginError"] = "User not found you may have entered incorrect username or password";
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    TempData["loginError"] = "Enter your username and password";
                    return RedirectToAction("Login");
                }
            }
            TempData["loginError"] = "Enter your username and password";
            return RedirectToAction("Login");
        }


        public ActionResult UploadPicture()
        {
            return RedirectToAction("Pictures", "Login");
        }


        public ActionResult ShowAllFriends()
        {
            return RedirectToAction("UsersFriends", "ShowAllFriends");
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}