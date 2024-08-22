using SaFarHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SaFarHub.Controllers
{
    public class PictureController : Controller
    {
        //Database Reference Object
        private readonly SaFarHubDatabaseEntities db = new SaFarHubDatabaseEntities();

        public ActionResult Post()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(Picture picture)
        {
            string username = (string)Session["username"];
            picture.username = username;
            picture.uplaodDateTime = DateTime.Now;
            string fileName = Path.GetFileNameWithoutExtension(picture.ImageFile.FileName);
            string extension = Path.GetExtension(picture.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            picture.pictureAddress = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            picture.ImageFile.SaveAs(fileName);
            db.Pictures.Add(picture);
            db.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("Home", "User");
        }


        public ActionResult Index()
        {
            //var pictures = db.Pictures.Include(p => p.Admin).Include(p => p.User);
            //return View(pictures.ToList());
            return View();
        }

        // GET: Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: Pictures/Create
        public ActionResult Create()
        {
            //ViewBag.username = new SelectList(db.Admins, "username", "firstName");
            ViewBag.username = new SelectList(db.Users, "username", "firstName");
            return View();
        }

        // POST: Pictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pictureID,username,pictureAddress,pictureCaption")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Pictures.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.username = new SelectList(db.Admins, "username", "firstName", picture.username);
            ViewBag.username = new SelectList(db.Users, "username", "firstName", picture.username);
            return View(picture);
        }

        // GET: Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            //ViewBag.username = new SelectList(db.Admins, "username", "firstName", picture.username);
            ViewBag.username = new SelectList(db.Users, "username", "firstName", picture.username);
            return View(picture);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pictureID,username,pictureAddress,pictureCaption")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.username = new SelectList(db.Admins, "username", "firstName", picture.username);
            ViewBag.username = new SelectList(db.Users, "username", "firstName", picture.username);
            return View(picture);
        }

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Pictures.Find(id);
            db.Pictures.Remove(picture);
            db.SaveChanges();
            return RedirectToAction("Login");
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