using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using giadinhthoxinh.Models;

namespace giadinhthoxinh.Areas.Admin.Controllers
{
    public class PermissionsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/Permissions
        public ActionResult Index()
        {
            return View(db.tblPermissions.ToList());
        }

        // GET: Admin/Permissions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPermission tblPermission = db.tblPermissions.Find(id);
            if (tblPermission == null)
            {
                return HttpNotFound();
            }
            return View(tblPermission);
        }

        // GET: Admin/Permissions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Permissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iPermissionID,sPermissionName,iState")] tblPermission tblPermission)
        {
            if (ModelState.IsValid)
            {
                db.tblPermissions.Add(tblPermission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblPermission);
        }

        // GET: Admin/Permissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPermission tblPermission = db.tblPermissions.Find(id);
            if (tblPermission == null)
            {
                return HttpNotFound();
            }
            return View(tblPermission);
        }

        // POST: Admin/Permissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iPermissionID,sPermissionName,iState")] tblPermission tblPermission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPermission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblPermission);
        }

        // GET: Admin/Permissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPermission tblPermission = db.tblPermissions.Find(id);
            if (tblPermission == null)
            {
                return HttpNotFound();
            }
            return View(tblPermission);
        }

        // POST: Admin/Permissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblPermission tblPermission = db.tblPermissions.Find(id);
            db.tblPermissions.Remove(tblPermission);
            db.SaveChanges();
            return RedirectToAction("Index");
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
