using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapAzImtahan.Model
{
    public class TapAzAccount
    {
        public TapAzAccount(string? email, string? password, string? name, string? surname, DateTime? age)
        {
            Email = email;
            Password = password;
            Name = name;
            Surname = surname;
            Age = age;
        }

        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? Age { get; set; }



    }
}
