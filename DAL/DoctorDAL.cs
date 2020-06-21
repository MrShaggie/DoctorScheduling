using ProjectScheduling.Infrastructure;
using ProjectScheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectScheduling.DAL
{
    public class DoctorDAL : IDoctorDAL
    {
        private readonly SchedulingDbContext db;

        public DoctorDAL()
        {

        }
        public DoctorDAL(SchedulingDbContext _db)
        {
            db = _db;
        }

        public IQueryable<DoctorModel> GetDoctors()
        {
            try
            {
                var doctors = from d in db.Doctors
                              select d;

                return doctors;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DoctorModel GetDoctorByName(string name)
        {
            try
            {
                var doctor = db.Doctors.First(u => u.Name == name);

                return doctor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetDoctorName(int id)
        {
            try
            {
                var doctorName = db.Doctors.Where(x => x.ID == id).Select(y => y.Name).FirstOrDefault();

                return doctorName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApplicationUser GetUserDetails(string id)
        {
            try
            {
                var user = db.ApplicationUsers.First(u => u.UserName == id);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}