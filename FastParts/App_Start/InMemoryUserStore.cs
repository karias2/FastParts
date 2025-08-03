using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastParts.App_Start
{

       public class ApplicationUser
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
       }

       public static class InMemoryUserStore
       {
            public static List<ApplicationUser> Users = new List<ApplicationUser>
    {
        new ApplicationUser { UserName = "Cliente@fastparts.com",  Password = "cliente123",  Role = "Cliente" },
        new ApplicationUser { UserName = "Mecanico@fastparts.com", Password = "mecanico123", Role = "Mecanico" },
        new ApplicationUser { UserName = "Admin@fastparts.com",    Password = "admin123",    Role = "Administrador" }
    };

            public static ApplicationUser Validate(string userName, string password)
                => Users.FirstOrDefault(u =>
                    string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase)
                    && u.Password == password);
        }
    
}