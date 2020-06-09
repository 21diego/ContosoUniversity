using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.DAL;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class EnrollmentsController : Controller
    {
        private UniversityContext db = new UniversityContext();

        // GET: Enrollments
        public ActionResult Index(string search, int? DepartmentID)
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "departmentid", "Title");
            ViewBag.Enrollments = db.CourseStudents.ToList();

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
                return View(db.Courses.Include(c => c.Department).Include(c => c.Instructor).ToList());
            }
        }


        //GET: Enroll/id
        public ActionResult Enroll(int? id)
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

            ViewBag.Course = db.Courses.Where(c => c.CourseID == id).First();
            return View();
        }

        //POST: Enroll/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enroll([Bind(Include = "StudentID,FullName,Email")] Student student, int? id)
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
            if (ModelState.IsValid)
            {
                Student queryStudent = db.Students.ToList().Find(s => s.FullName == student.FullName && s.Email == student.Email);
                if( queryStudent != null)
                {
                    CourseStudent queryCourseStudent = db.CourseStudents.ToList().Find(cs => cs.StudentId == queryStudent.StudentID && cs.CourseId == id);
                    if (queryCourseStudent != null)
                    {
                        ViewBag.Course = db.Courses.Find(id);
                        ViewBag.Student = student;
                        return View("ErrorEnroll");
                    }
                    else
                    {
                        CourseStudent newEnrollment = new CourseStudent();
                        newEnrollment.CourseId = (int)id;
                        newEnrollment.StudentId = queryStudent.StudentID;
                        db.CourseStudents.Add(newEnrollment);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    queryStudent = db.Students.ToList().Find(s => s.FullName == student.FullName && s.Email == student.Email);
                    CourseStudent newEnrollment = new CourseStudent();
                    newEnrollment.CourseId = (int)id;
                    newEnrollment.StudentId = queryStudent.StudentID;
                    db.CourseStudents.Add(newEnrollment);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

    }
}