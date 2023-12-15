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
using MongoDB.Driver;
using MongoDB.Bson;

namespace SPG.Werkstatt_Backoff_Mongo
{
    public class Model
    { 

        public List<TerminMongo> TermineDB { get; } = new List<TerminMongo>();
        public List<CustomerMongo> KundeListeDB { get; set; } = new List<CustomerMongo>();

        //private  WerkstattContext _db;
        private readonly WerkstattMongoContext _db;




        // Dependency Injection

        public Model(WerkstattMongoContext db)
        {

            _db = db;
            getKundenListeVonDB();
            getTermineFromDB();
        }

        public async void getTermineFromDB()
        {
            TermineDB.Clear();
            //TermineDB.AddRange(_db.Termine.OrderBy(c => c.Datetime).Include(c => c.Auto).ToList());
            var termine = _db._termineCollection
            .Find(FilterDefinition<TerminMongo>.Empty)
            .Sort(Builders<TerminMongo>.Sort.Ascending("Datetime"))
            .ToList();
            TermineDB.AddRange(termine);

        }

        public async void getKundenListeVonDB() 
        {
            KundeListeDB.Clear();
            //KundeListeDB.AddRange(_db.Customers.OrderBy(c => c.Nachname).ToList());
            var customers = _db._customerCollection
                .Find(Builders<CustomerMongo>.Filter.Empty)
                .Sort(Builders<CustomerMongo>.Sort.Ascending("Nachname"))
                .ToList();

            KundeListeDB.AddRange(customers);

        }


        public CustomerMongo getKunde(ObjectId kundenID)
        {
            //CustomerMongo newKunde = (CustomerMongo)_db.Customers.Where(c => c.Id == kundenID);
            CustomerMongo newKunde = _db._customerCollection.Find(c => c.Id == kundenID).FirstOrDefault();
            return newKunde;
        }

        public CarMongo getAuto(ObjectId carId)
        {
            //CarMongo newCar = (CarMongo)_db.Cars.Where(c => c.Id == carId);
            CarMongo newCar = _db._carCollection.Find(c => c.Id == carId).FirstOrDefault();
            return newCar;
        }

        
    }
}
