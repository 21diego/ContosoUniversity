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
    public class CoursesController : Controller
    {
        private UniversityContext db = new UniversityContext();

        // GET: Courses
        public ActionResult Index(string search, int? DepartmentID)
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Title");

            if (!String.IsNullOrEmpty(search) && DepartmentID != null)
            {
                return View(db.Courses.Where(c => c.DepartmentID == DepartmentID && c.Title.Contains(search)).ToList());
            }
            else if (!String.IsNullOrEmpty(search))
            {
                return View(db.Courses.Where(c => c.Title.Contains(search)).ToList());
            }
            else if (DepartmentID != null)
            {
                return View(db.Courses.Where(c => c.DepartmentID == DepartmentID).ToList());
            }
            else
            {
                var courses = db.Courses.Include(c => c.Department).Include(v => v.Instructor);
                return View(courses.ToList());
            }
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var students = db.Students.ToList();
            var courseEnrollments = db.CourseStudents.Where(cs => cs.CourseId == course.CourseID).ToList();
            var enrolled = new List<Student>();
            foreach(var student in students)
            {
                if(courseEnrollments.Find(cs => cs.StudentId == student.StudentID) != null) { enrolled.Add(student); }
            }
            ViewBag.CourseEnrollments = enrolled;
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Title");
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FullName");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Capacity,DepartmentID,InstructorID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Title", course.DepartmentID);
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FullName", course.InstructorID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Title", course.DepartmentID);
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FullName", course.InstructorID);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Capacity,DepartmentID,InstructorID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Title", course.DepartmentID);
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FullName", course.InstructorID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var students = db.Students.ToList();
            var courseEnrollments = db.CourseStudents.Where(cs => cs.CourseId == course.CourseID).ToList();
            var enrolled = new List<Student>();
            foreach (var student in students)
            {
                if (courseEnrollments.Find(cs => cs.StudentId == student.StudentID) != null) { enrolled.Add(student); }
            }
            if (enrolled.Count == 0) { return View(course); }
            else
            {
                ViewBag.Course = course;
                return View("DeleteError", enrolled);
            }
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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
