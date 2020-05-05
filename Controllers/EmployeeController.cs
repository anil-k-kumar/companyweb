using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using companyweb1.Models;
using System.Data.Entity;
using System.Net;

namespace companyweb1.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private companyEntities c = new companyEntities();
        // GET: Employee
        public ActionResult Index()
        {
            return View(c.Employees.ToList());
        }
        public ActionResult Create()
        {

                 IEnumerable<SelectListItem> list= new SelectList(c.Designations, "Name", "Name");
            ViewBag.Designation = list;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,salary,Designation,DOF,Gender,D_O_Joining")] Employee employee)
        {
            if (ModelState.IsValid)
            {
               c.Employees.Add(employee);
                c.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }



        //edit information of current employee
        //Get employee by Id
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = c.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            IEnumerable<SelectListItem> list = new SelectList(c.Designations, "Name", "Name");
            ViewBag.Designation = list;
            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Employee employee)
        {
            if (ModelState.IsValid)
            {
                c.Entry(employee).State = EntityState.Modified;
                c.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        //view employee details by Search by Id
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = c.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //delete an employee

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = c.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = c.Employees.Find(id);
            c.Employees.Remove(employee);
            c.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Designation()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Designation([Bind(Include = "Id,Name")] Designation de)
        {
            bool Status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                c.Designations.Add(de);
                c.SaveChanges();
                message = "Departement Sucessfully added";
                Status = true;
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                c.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}