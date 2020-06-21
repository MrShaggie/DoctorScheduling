using ProjectScheduling.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace ProjectScheduling.Models
{
    public class IdentityManager
    {
        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new SchedulingDbContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }
        
        public string RoleName(string roleId)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SchedulingDbContext()));
            var role = rm.FindById(roleId);
            return role.Name;
        }
    }
}