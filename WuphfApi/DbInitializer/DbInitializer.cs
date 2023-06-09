using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuphfApi.Data;
using WuphfApi.Models;

namespace WuphfApi.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //in questo caso facciamo un accesso diretto a DbContext perché siamo
        //dentro DataAccess
        private readonly ApplicationDbContext _db;
        private readonly FirstAdminApplicationUserOptions _adminOptions;
        public DbInitializer(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db,
        IOptions<FirstAdminApplicationUserOptions> adminOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _adminOptions = adminOptions.Value;
        }
        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                //verifico automaticamente se ci sono migrazioni che non sono state applicate
                if (_db.Database.GetPendingMigrations().Any())
                {
                    //ogni volta che parte l'applicazione, se ci sono migrazioni pendenti
                    //le applico al database
                    _db.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("admin")).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("customer")).GetAwaiter().GetResult();

                //if roles are not created, then we will create admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = _adminOptions.UserName,
                    Email = _adminOptions.Email,
                    Name = _adminOptions.Name,
                    FotoProfilo = _adminOptions.FotoProfilo,
                    TelegramToken = _adminOptions.TelegramToken,
                    EmailConfirmed = true
                }, _adminOptions.Password)

                .GetAwaiter().GetResult();

                ApplicationUser? user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == _adminOptions.Email);

                if (user != null)
                {
                    _userManager.AddToRoleAsync(user, "admin").GetAwaiter().GetResult();
                }
            }

        }

    }
}
