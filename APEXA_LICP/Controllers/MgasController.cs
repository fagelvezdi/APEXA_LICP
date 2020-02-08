using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APEXA_LICP.Models;

namespace APEXA_LICP.Controllers
{
    public class MgasController : Controller
    {
        private LICPEntities db = new LICPEntities();

        // GET: Mgas
        public ActionResult Index()
        {
            return View(db.MGAs.ToList());
        }

        // GET: Mgas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MGA mGA = db.MGAs.Find(id);
            if (mGA == null)
            {
                return HttpNotFound();
            }
            return View(mGA);
        }

        // GET: Mgas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mgas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BusinessName,BusinessAddress,BusinessPhone")] MGA mGA)
        {
            if (ModelState.IsValid)
            {
                db.MGAs.Add(mGA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mGA);
        }

        // GET: Mgas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MGA mGA = db.MGAs.Find(id);
            if (mGA == null)
            {
                return HttpNotFound();
            }
            return View(mGA);
        }

        // POST: Mgas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BusinessName,BusinessAddress,BusinessPhone")] MGA mGA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mGA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mGA);
        }

        // GET: Mgas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MGA mGA = db.MGAs.Find(id);
            if (mGA == null)
            {
                return HttpNotFound();
            }
            return View(mGA);
        }

        // POST: Mgas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MGA mGA = db.MGAs.Find(id);
            db.MGAs.Remove(mGA);
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
