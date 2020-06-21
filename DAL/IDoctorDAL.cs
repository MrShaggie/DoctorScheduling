using ProjectScheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectScheduling.DAL
{
    public interface IDoctorDAL
    {
        IQueryable<DoctorModel> GetDoctors();
        DoctorModel GetDoctorByName(string name);
        ApplicationUser GetUserDetails(string id);
        string GetDoctorName(int id);
    }
}
