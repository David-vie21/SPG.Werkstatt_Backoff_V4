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
using SPG.Werkstatt.Domian.MongoModels;

namespace SPG.Werkstatt_Backoff_V3
{
    public class Model
    { 

        public List<Termin> TermineDB { get; } = new List<Termin>();
        public List<CustomerMongo> KundeListeDB { get; set; } = new List<CustomerMongo>();

        private  WerkstattContext _db;


        // Dependency Injection

        public Model(WerkstattContext db)
        {
            
            _db = db;
            getKundenListeVonDB();
            getTermineFromDB();
        }

        public void getTermineFromDB()
        {
            TermineDB.Clear();
            TermineDB.AddRange(_db.Termine.OrderBy(c => c.Datetime).Include(c => c.Auto).ToList());
        }

        public void getKundenListeVonDB() 
        {
            KundeListeDB.Clear();
            KundeListeDB.AddRange(_db.Customers.OrderBy(c => c.Nachname).ToList());
        }


        public CustomerMongo getKunde(int kundenID)
        {
            CustomerMongo newKunde = (CustomerMongo)_db.Customers.Where(c => c.Id == kundenID);
            return newKunde;
        }

        public CarMongo getAuto(int carId)
        {
            CarMongo newCar = (CarMongo)_db.Cars.Where(c => c.Id == carId);
            return newCar;
        }

        
    }
}
