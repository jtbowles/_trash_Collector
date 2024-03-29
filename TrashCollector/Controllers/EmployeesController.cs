﻿using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public ActionResult Index()
        {
            DayOfWeek currentDay = DateTime.Today.DayOfWeek;
            DateTime currentDate = DateTime.Today;
            var applicationId = User.Identity.GetUserId();
            Employee employee = db.Employees.Where(e => e.ApplicationId == applicationId).FirstOrDefault();

            var listOfCustomersByZip = db.Customers.Where(c => c.ZipCode == employee.ZipCode);
            var listOfCustomersByPickUp = listOfCustomersByZip.Where(c => c.ExtraPickUp == DateTime.Today || c.Day.Name == currentDay.ToString());
            var listOfUnconfirmedCustomers = listOfCustomersByPickUp.Where(c => c.Balance == 0).ToList();
            List<Customer> filteredCustomers = new List<Customer>();

            foreach (Customer customer in listOfUnconfirmedCustomers)
            {
                if(customer.StartHoldDate == null && customer.EndHoldDate == null)
                {
                    filteredCustomers.Add(customer);
                    continue;
                }
                else if (currentDate.CompareTo(customer.StartHoldDate) <= 0 && currentDate.CompareTo(customer.EndHoldDate) >= 0)
                {
                    filteredCustomers.Add(customer);
                    continue;
                }
                else if (currentDate.CompareTo(customer.StartHoldDate) > 0 && currentDate.CompareTo(customer.EndHoldDate) < 0)
                {
                    continue;
                }
            }
            return View(filteredCustomers);
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
            var applicationId = User.Identity.GetUserId();
            Employee currentEmployee = db.Employees.Where(e => e.ApplicationId == applicationId).FirstOrDefault();

            var listOfCustomersByZip = db.Customers.Where(c => c.ZipCode == currentEmployee.ZipCode);
            List<Customer> listOfCustomersByPickUp = listOfCustomersByZip.Where(c => c.Day.Name == day.ToString()).ToList();

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

        // GET: CUSTOMER/DETAILS
        public ActionResult CustomerDetails(int? id)
        {
            Customer customer = db.Customers.Find(id);
            customer.Days = db.Days.ToList();

            var address = customer.Address + "," + customer.City + "," + customer.State;
            var Key = Keys.GoogleApiKey;
            var requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={Key}";
            var result = new WebClient().DownloadString(requestUrl);
            var jo = JObject.Parse(result);

            var lat = jo["results"][0]["geometry"]["location"]["lat"];
            var lng = jo["results"][0]["geometry"]["location"]["lng"];

            customer.Lat = Convert.ToDouble(lat);
            customer.Lng = Convert.ToDouble(lng);

            return View(customer);
        }

        // CONFIRM Customer PickUp
        public ActionResult ConfirmPickup(int? id)
        {
            Customer customer = db.Customers.Find(id);
            customer.Balance += 15;
            db.SaveChanges();
            return View(customer);
        }

        //public GeoCode GeoLocate(string address)
        //{
        //    var requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key=AIzaSyCvNx58z28nAtRCUDGJU6xi2qisdrmE1dQ";
        //    var result = new WebClient().DownloadString(requestUrl);
        //    GeoCode geocode = JsonConvert.DeserializeObject<GeoCode>(result);
        //    return geocode;
        //}

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
