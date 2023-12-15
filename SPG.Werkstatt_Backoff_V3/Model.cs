using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPG.Werkstatt.Domian.Model;
using SPG.Werkstatt.Domian;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Windows.Input;

namespace SPG.Werkstatt_Backoff_V3
{
    public class Model
    { 

        public List<Termin> TermineDB { get; } = new List<Termin>();
        public List<Customer> KundeListeDB { get; set; } = new List<Customer>();

        private  WerkstattContext _db;


        // Dependency Injection

        public Model(WerkstattContext db)
        {
            
            _db = db;
            getKundenListeVonDB();
            getTermineFromDB();
        }

        public async void getTermineFromDB()
        {
            TermineDB.Clear();
            TermineDB.AddRange(await _db.Termine.OrderBy(c => c.Datetime).Include(c => c.Auto).ToListAsync());
        }

        public void getKundenListeVonDB() 
        {
            KundeListeDB.Clear();
            KundeListeDB.AddRange(_db.Customers.OrderBy(c => c.Nachname).ToList());
        }


        public Customer getKunde(int kundenID)
        {
            Customer newKunde = (Customer)_db.Customers.Where(c => c.Id == kundenID);
            return newKunde;
        }

        public Car getAuto(int carId)
        {
            Car newCar = (Car)_db.Cars.Where(c => c.Id == carId);
            return newCar;
        }

        
    }
}
