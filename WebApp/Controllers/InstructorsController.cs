using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.DAL;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class InstructorsController : Controller
    {
        private UniversityContext db = new UniversityContext();

        // GET: Intructors
        public ActionResult Index(string search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                return View(db.Instructors.Where(inst => inst.FullName.Contains(search)).ToList());
            }
            return View(db.Instructors.ToList());
        }



        // GET: Intructors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Intructors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IntructorID,FullName,HireDate")] Instructor intructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(intructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(intructor);
        }

        // GET: Intructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor intructor = db.Instructors.Find(id);
            if (intructor == null)
            {
                return HttpNotFound();
            }
            return View(intructor);
        }

        // POST: Intructors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IntructorID,FullName,HireDate")] Instructor intructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(intructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(intructor);
        }

        // GET: Intructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor intructor = db.Instructors.Find(id);
            if (intructor == null)
            {
                return HttpNotFound();
            }
            return View(intructor);
        }

        // POST: Intructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor intructor = db.Instructors.Find(id);
            db.Instructors.Remove(intructor);
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
