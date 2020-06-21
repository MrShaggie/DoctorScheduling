using ProjectScheduling.Infrastructure;
using ProjectScheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace ProjectScheduling.DAL
{
    public class AppointmentDAL : IAppointmentDAL
    {
        private readonly SchedulingDbContext db;

        public AppointmentDAL()
        {

        }

        public AppointmentDAL(SchedulingDbContext _db)
        {
            db = _db;
        }

        public IOrderedQueryable<AppointmentModel> GetAppointments()
        {
            try
            {
                var appointments = db.Appointments.Include(a => a.Doctor).OrderByDescending(a => a.Date);
                return appointments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<AdministrationModel> GetAdministration()
        {
            try
            {
                var result = from admin in db.Administrations
                             select admin;

                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void CreateDoctorAppointment(AppointmentModel appointment)
        {
            try
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}