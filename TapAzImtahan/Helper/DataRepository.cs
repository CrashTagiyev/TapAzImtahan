using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TapAzImtahan.Model;

namespace TapAzImtahan.Helper
{
    // Shared service class
    public class DataRepository
    {
        private ObservableCollection<Product> sharedCollection;

        public DataRepository()
        {
            sharedCollection = new ObservableCollection<Product>();
        }

        public ObservableCollection<Product> GetSharedCollection()
        {
            return sharedCollection;
        }
    }

}
