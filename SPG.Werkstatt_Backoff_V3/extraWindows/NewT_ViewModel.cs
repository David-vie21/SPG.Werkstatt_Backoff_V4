using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;


namespace SPG.Werkstatt_Backoff_V3.extraWindows
{
    public class NewT_ViewModel
    {
        //List<Customer> Customers = new List<Customer>();

        public List<Customer> Customers { get; set; } = new List<Customer>();
        public HashSet<Customer> CustomersHS { get; set; } = new HashSet<Customer>();

        public List<Car> CarsFromKunde { get; set; } = new List<Car>();
        public HashSet<Car> CarsFromKundeHS { get; set; } = new HashSet<Car>();

        public Customer CurrentKunde { get; set; }
        public Car CurrentCar { get; set; }
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

        

        public HashSet<Car> getCarsFromKunde(Customer kunde)
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
        public Customer getKunde(int kundenID)
        {

            Customer customer = (Customer)_db.Customers.Where(c => c.Id == kundenID);
            return customer;

        }

    }
}
