using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;


namespace SPG.Werkstatt_Backoff_Mongo.extraWindows
{
    public class NewT_ViewModel
    {
        //List<Customer> Customers = new List<Customer>();

        public List<CustomerMongo> Customers { get; set; } = new List<CustomerMongo>();
        public HashSet<CustomerMongo> CustomersHS { get; set; } = new HashSet<CustomerMongo>();

        public List<CarMongo> CarsFromKunde { get; set; } = new List<CarMongo>();
        public HashSet<CarMongo> CarsFromKundeHS { get; set; } = new HashSet<CarMongo>();

        public CustomerMongo CurrentKunde { get; set; }
        public CarMongo CurrentCar { get; set; }
        WerkstattContext _db;

        public NewT_ViewModel(WerkstattContext db)
        {
            //AutoListe
            if (CurrentKunde != null)
                getCarsFromKunde(CurrentKunde);
            _db = db;

            Customers = _db.Customers.ToList();
            CustomersHS = Customers.ToHashSet();
        }

        

        public HashSet<CarMongo> getCarsFromKunde(CustomerMongo kunde)
        {
            CarsFromKunde.Clear();
            CarsFromKundeHS.Clear();

            if (kunde != null)
            {

                CarsFromKunde = _db.Cars.Where(c => c.Id == kunde.Id).ToList();

                CarsFromKundeHS = CarsFromKunde.ToHashSet();
                return CarsFromKundeHS;

            }
            return null;
        }
        public CustomerMongo getKunde(int kundenID)
        {

            CustomerMongo customer = (CustomerMongo)_db.Customers.Where(c => c.Id == kundenID);
            return customer;

        }

    }
}
