using ProjectScheduling.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ProjectScheduling.Infrastructure
{
    public class SchedulingDbContext : IdentityDbContext
    {
        public SchedulingDbContext() : base("DefaultConnection")
        {
            //Empty constructor.
        }
            
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<DoctorModel> Doctors { get; set; }
        public DbSet<AppointmentModel> Appointments { get; set; }
        public DbSet<AdministrationModel> Administrations { get; set; }
    }
}