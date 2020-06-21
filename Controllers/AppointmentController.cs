using ProjectScheduling.Infrastructure;
using ProjectScheduling.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;

using System.Web.Mvc;
using ProjectScheduling.DAL;

namespace ProjectScheduling.Controllers
{
    public class AppointmentController : Controller
    {
        private SchedulingDbContext db = new SchedulingDbContext();

        private readonly IDoctorDAL doctorDAL;
        private readonly IAppointmentDAL appointmentDAL;

        public AppointmentController()
        {

        }

        public AppointmentController(IDoctorDAL _doctorDAL, IAppointmentDAL _appointmentDAL)
        {
            doctorDAL = _doctorDAL;
            appointmentDAL = _appointmentDAL;
        }

        // GET: /Appointments/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var appointments = appointmentDAL.GetAppointments();
            return View(appointments.ToList());
        }

        // GET: /Appointments/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            AppointmentModel appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return View("Error");
            }
            return View(appointment);
        }

        // GET: /Appointments/Get
        [AllowAnonymous]
        public ActionResult Create(int id)
        {
            ViewBag.DoctorName = doctorDAL.GetDoctorName(id);
            ViewBag.TimeBlockHelper = new SelectList(String.Empty);
            ViewBag.DoctorId = id;
            var model = new AppointmentModel
            {
                UserID = User.Identity.GetUserId()
            };
            return View("Create");
        }

        // POST: /Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Patient")]
        [AllowAnonymous]
       // [ValidateAntiForgeryToken]
        public ActionResult CreateAppointment([Bind(Include = "DoctorID,Date,TimeBlockHelper")] AppointmentModel appointment)
        {
            //Add userID
            appointment.UserID = User.Identity.GetUserId();

            //Verify Time
            if (appointment.TimeBlockHelper == "DONT")
                ModelState.AddModelError("", "No Appointments Available for " + appointment.Date.ToShortDateString());
            else
            {
                //Set Time
                appointment.Time = DateTime.Parse(appointment.TimeBlockHelper);

                //CheckWorkingHours
                var administration = appointmentDAL.GetAdministration();
                var span = (from admin in administration
                           where admin.Name.ToUpper() == "SPAN"
                           select admin.Value).FirstOrDefault();

                var doctorStartTime = (from admin in administration
                                       where admin.Name.ToUpper() == "STARTTIME"
                                       select admin.Value).FirstOrDefault();

                var doctorEndTime = (from admin in administration
                                     where admin.Name.ToUpper() == "ENDTIME"
                                     select admin.Value).FirstOrDefault();

                DateTime start = appointment.Date.Add(appointment.Time.TimeOfDay);
                DateTime end = (appointment.Date.Add(appointment.Time.TimeOfDay)).AddMinutes(double.Parse(span));
                
                if (!(BuisnessLogic.IsInWorkingHours(start, end)))
                    ModelState.AddModelError("", "Doctor Working Hours are from " + int.Parse(doctorStartTime) + " to " + int.Parse(doctorEndTime));

                //Check Appointment Clash
                string check = BuisnessLogic.ValidateNoAppoinmentClash(appointment);
                if (check != "")
                    ModelState.AddModelError("", check);
            }

            //Continue Normally
            if (ModelState.IsValid)
            {
                appointmentDAL.CreateDoctorAppointment(appointment);
                //return RedirectToAction("Details", new { Controller = "Appointment", Action = "AppointmentDetails" });
            }

            //Fill Neccessary ViewBags
            var availableDoctors = doctorDAL.GetDoctors();
            ViewBag.DoctorID = new SelectList(availableDoctors, "ID", "Name", appointment.DoctorID);
            ViewBag.TimeBlockHelper = new SelectList(String.Empty);
            return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
            
        }

        // GET: /Appointment/AppointmentDetails
        [Authorize(Roles = "Patient")]
        public ActionResult AppointmentDetails()
        {
            string id = User.Identity.Name;           
            var user = doctorDAL.GetUserDetails(id);
            var model = new EditUserViewModel(user);
            model.Appointments.Sort();
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Here or in model?
        [HttpPost]
        public JsonResult GetAvailableAppointments(int docID, DateTime date)
        {
            List<SelectListItem> resultlist = BuisnessLogic.AvailableAppointments(docID, date);
            return Json(resultlist);
        }
    }
}
