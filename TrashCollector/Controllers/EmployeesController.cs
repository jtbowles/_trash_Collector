using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        public ActionResult Index(int? id)
        {
            DayOfWeek currentDay = DateTime.Today.DayOfWeek;
            Employee employee = db.Employees.Find(id);

            var listOfCustomersByZip = db.Customers.Where(c => c.ZipCode == employee.ZipCode);
            var listOfCustomersByPickUp = listOfCustomersByZip.Where(c => c.ExtraPickUp == DateTime.Today || c.Day.Name == currentDay.ToString());
            //var listOfCustomersWithoutSuspension = listOfCustomersByPickUp.Where(c => c.)

            // CHECK SUSPENSIONS!!

            ViewBag.Name = new SelectList(db.Days.ToList(), "Id", "Name");
            return View(listOfCustomersByPickUp);
        }

        // SORT BY DAY
        public ActionResult Sort(int? id)
        {
            DayOfWeek day = DayOfWeek.Monday;

            switch (id)
            {
                case 1:
                    day = DayOfWeek.Monday;
                    break;

                case 2:
                    day = DayOfWeek.Tuesday;
                    break;

                case 3:
                    day = DayOfWeek.Wednesday;
                    break;

                case 4:
                    day = DayOfWeek.Thursday;
                    break;

                case 5:
                    day = DayOfWeek.Friday;
                    break;

                case 6:
                    day = DayOfWeek.Saturday;
                    break;

                case 7:
                    day = DayOfWeek.Sunday;
                    break;

                default:
                    break;
            }   
            Employee currentEmployee = db.Employees.Find(3);

            var listOfCustomersByZip = db.Customers.Where(c => c.ZipCode == currentEmployee.ZipCode);
            var listOfCustomersByPickUp = listOfCustomersByZip.Where(c => c.Day.Name == day.ToString());

            return View("Index", listOfCustomersByPickUp);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,FirstName,LastName,ZipCode")] Employee employee)
        {
            employee.ApplicationId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = employee.EmployeeId});
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,FirstName,LastName,ZipCode")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = employee.EmployeeId });
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = employee.EmployeeId });
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
