using ProjectScheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectScheduling.DAL
{
    public interface IAppointmentDAL
    {
        IOrderedQueryable<AppointmentModel> GetAppointments();
        IQueryable<AdministrationModel> GetAdministration();
        void CreateDoctorAppointment(AppointmentModel appointment);
    }
}
