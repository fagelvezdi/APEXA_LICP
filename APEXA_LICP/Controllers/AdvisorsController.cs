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
    public class AdvisorsController : Controller
    {
        private LICPEntities db = new LICPEntities();

        // GET: Advisors
        public ActionResult Index()
        {
            return View(db.Advisors.ToList());
        }

        // GET: Advisors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advisor advisor = db.Advisors.Find(id);
            if (advisor == null)
            {
                return HttpNotFound();
            }
            return View(advisor);
        }

        // GET: Advisors/Create
        public ActionResult Create()
        {

           

            return View();
        }

        // POST: Advisors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Address,PhoneNumber,HealthStatus")] Advisor advisor)
        {
            string healthStatus = "";


            List<KeyValuePair<string, double>> elements = new List<KeyValuePair<string, double>>();
            elements.Add(new KeyValuePair<string, double>("Green", 0.7));
            elements.Add(new KeyValuePair<string, double>("Red", 0.3));
            Random r = new Random();
            double rndColor = r.NextDouble();

            double cumulative = 0.0;
            for (int i = 0; i < elements.Count; i++)
            {
                cumulative += elements[i].Value;
                if (rndColor < cumulative)
                {
                    healthStatus = elements[i].Key;
                    break;
                }
            }           

            if (ModelState.IsValid)
            {
                int advisorId = (from a in db.Advisors
                                    orderby a.Id
                                    descending
                                    select a.Id).First();

                advisor.Id = advisorId + 1;
                advisor.HealthStatus = healthStatus;
                db.Advisors.Add(advisor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(advisor);
        }

        // GET: Advisors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advisor advisor = db.Advisors.Find(id);
            if (advisor == null)
            {
                return HttpNotFound();
            }
            return View(advisor);
        }

        // POST: Advisors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Address,PhoneNumber,HealthStatus")] Advisor advisor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(advisor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(advisor);
        }

        // GET: Advisors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advisor advisor = db.Advisors.Find(id);
            if (advisor == null)
            {
                return HttpNotFound();
            }
            return View(advisor);
        }

        // POST: Advisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Advisor advisor = db.Advisors.Find(id);

            List<Contract> lstContract = db.Contract.Where(c => c.AdvisorId == id).ToList<Contract>();
            db.Contract.RemoveRange(lstContract);
            db.Advisors.Remove(advisor);
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
