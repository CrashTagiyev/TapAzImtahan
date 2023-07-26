using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapAzImtahan.View
{
    public interface IAccountRegister : INotifyPropertyChanged
    {
        public string? Email { get;  }
        public string? Name { get;  }
        public string? Surname { get;  }
        public string? Password { get; }
        public DateTime? BirthDay { get; }
    }
}
