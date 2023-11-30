using Bogus;
using MongoDB.Driver;
using SPG.Werkstatt.Domian.Model;
using SPG.Werkstatt.Domian.MongoModels;
using System;
using System.Collections.Generic;

namespace SPG.Werkstatt.Domian
{
    public class WerkstattMongoContext
    {
        private readonly IMongoDatabase _database;

        // MongoDB Kollektionen
        private IMongoCollection<CustomerMongo> _customerCollection;
        private IMongoCollection<CarMongo> _carCollection;
        private IMongoCollection<Termin> _termineCollection;

        // Konstruktor
        public WerkstattMongoContext(IMongoDatabase database)
        {
            _database = database;
            InitializeCollections();
        }

        // Initialisierung der MongoDB-Kollektionen
        private void InitializeCollections()
        {
            _customerCollection = _database.GetCollection<CustomerMongo>("Customers");
            _carCollection = _database.GetCollection<CarMongo>("Cars");
            _termineCollection = _database.GetCollection<Termin>("Termine");
        }

        // Methode zum Hinzufügen von Testdaten in MongoDB
        public void Seed()
        {
            Randomizer.Seed = new Random(1017);

            List<CustomerMongo> customers = new Faker<CustomerMongo>("de")
                .Rules((f, c) =>
                {
                    // Hier bleibt deine Logik für das Generieren von Kundenobjekten unverändert
                    // ...

                })
                .Generate(80);

            _customerCollection.InsertMany(customers);

            // Ähnlich kannst du für Autos und Termine vorgehen
            // Beachte, dass dies ein Beispiel ist und du deine eigenen Logiken für das Generieren und Hinzufügen der Daten implementieren musst.

            // List<CarMongo> cars = ... // Generierung der Autodaten
            // _carCollection.InsertMany(cars);

            // List<Termin> termine = ... // Generierung der Termin-Daten
            // _termineCollection.InsertMany(termine);
        }
    }
}
