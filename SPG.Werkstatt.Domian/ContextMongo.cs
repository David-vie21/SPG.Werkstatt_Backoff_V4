using Bogus;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SPG.Werkstatt.Domian.Model;
using SPG.Werkstatt.Domian.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPG.Werkstatt.Domian
{
    public class WerkstattMongoContext
    {
        private readonly IMongoDatabase _database;

        // MongoDB Kollektionen
        public IMongoCollection<CustomerMongo> _customerCollection;
        public IMongoCollection<CarMongo> _carCollection;
        public IMongoCollection<TerminMongo> _termineCollection;
        public IMongoCollection<Day> _dayCollection;


        // Konstruktor
        public WerkstattMongoContext(string ConnectionString, string DBName) 
        {
            //string connectionString = configuration.GetConnectionString("MongoConnection");

            //// Hole den Datenbanknamen aus der appsettings.json-Datei (optional)
            //string databaseName = configuration.GetSection("MongoSettings:DatabaseName").Value;

            // Setze die Verbindung zur MongoDB
            var database = new MongoClient(ConnectionString).GetDatabase(DBName);


            _database = database;
            InitializeCollections();
        }

        // Initialisierung der MongoDB-Kollektionen
        private void InitializeCollections()
        {
            _customerCollection = _database.GetCollection<CustomerMongo>("Customers");
            _carCollection = _database.GetCollection<CarMongo>("Cars");
            _termineCollection = _database.GetCollection<TerminMongo>("Termine");
            _dayCollection = _database.GetCollection<Day>("Day");

        }

        // Methode zum Hinzufügen von Testdaten in MongoDB
        public void Seed(int multi)
        {
            Randomizer.Seed = new Random(1017);
            List<CustomerMongo> customers = new Faker<CustomerMongo>("de")
               .Rules((f, c) =>
               {                    
                   c.guid = f.Random.Guid();
                   c.Vorname = f.Name.FirstName(Bogus.DataSets.Name.Gender.Female);
                   c.Nachname = f.Name.LastName();
                   c.Email = f.Internet.Email();
                   c.Tel = f.Person.Phone;
                   c.Addrese = f.Address.FullAddress();

               })
           .Generate(10*multi)
           .ToList();
            _customerCollection.InsertMany(customers);

            List<CarMongo> cars = new Faker<CarMongo>("de")
                .Rules((f, c) =>
                {
                    c.guid = f.Random.Guid();
                    c.Besitzer = customers[f.Random.Number(0, customers.Count()-1)];
                    c.Marke = f.Company.CompanyName();
                    c.Modell = f.Commerce.ProductName();
                    c.Kennzeichen = f.Random.Replace("W##-###");
                    c.Erstzulassung = f.Date.Past(10, DateTime.Now);
                    c.Kw = f.Random.Decimal(60, 500);

                })
                .Generate(10*multi);
            _carCollection.InsertMany(cars);

            List<TerminMongo> termine = new Faker<TerminMongo>().Rules((f, s) =>
            {
                //         public Customer Kunde { get; set; }
                //public DateTime Datetime { get; set; }
                //public Car Auto { get; set; }
                //public string Summery { get; set; }
                //public string Date { get { return Datetime.ToShortDateString(); } }
                //public string Time { get { return Datetime.TimeOfDay.ToString(); } }
                //public bool accepted { get; set; }
                //public bool IsDone { get; set; }

                s.guid = f.Random.Guid();
                s.Kunde = customers[f.Random.Number(0, customers.Count()-1)];
                s.Datetime = f.Date.Between(new DateTime(2023, 12, 1), new DateTime(2023, 12, 10).AddDays(multi));
                s.Auto = cars[f.Random.Number(0, cars.Count()-1)];
                s.Summery = f.Lorem.Sentence();
                s.accepted = f.Random.Bool();
                s.IsDone = f.Random.Bool();
            })
            .Generate(20*multi)
            .ToList();
            _termineCollection.InsertMany(termine);



            List<Day> days = new Faker<Day>("de")
                .Rules((f, d) =>
                {
                    d.Date = new DateOnly(2023,12,1).AddDays(f.Random.Number(1, 1 * multi));
                    d.Termine = termine.Where(t => new DateOnly(d.Date.Year, d.Date.Month, d.Date.Day) == new DateOnly(t.Datetime.Year, t.Datetime.Month, t.Datetime.Day)).ToList();
                })
                .Generate(10 * multi);
            _dayCollection.InsertMany(days);

            //var testTermine = termine.ToList();
            //foreach (Day day in days)
            //{
            //    for (int i = 1; i <= 2; i++)
            //    {
            //        Random random = new Random();
            //        var termin = testTermine.OrderBy(x => random.Next()).Take(2).ToList();

            //        day.Termine.Add()
            //    }
            //}
        }

        public void Seed_with_Aggregation(int multi)
        {
            Randomizer.Seed = new Random(1017);
            List<CustomerMongo> customers = new Faker<CustomerMongo>("de")
               .Rules((f, c) =>
               {
                   c.guid = f.Random.Guid();
                   c.Vorname = f.Name.FirstName(Bogus.DataSets.Name.Gender.Female);
                   c.Nachname = f.Name.LastName();
                   c.Email = f.Internet.Email();
                   c.Tel = f.Person.Phone;
                   c.Addrese = f.Address.FullAddress();

               })
           .Generate(100 * multi)
           .ToList();
            _customerCollection.InsertMany(customers);

            List<CarMongo> cars = new Faker<CarMongo>("de")
                .Rules((f, c) =>
                {
                    c.guid = f.Random.Guid();
                    c.Besitzer = customers[f.Random.Number(0, customers.Count() - 1)];
                    c.Marke = f.Company.CompanyName();
                    c.Modell = f.Commerce.ProductName();
                    c.Kennzeichen = f.Random.Replace("W##-###");
                    c.Erstzulassung = f.Date.Past(10, DateTime.Now);
                    c.Kw = f.Random.Decimal(60, 500);

                })
                .Generate(10 * multi);
            _carCollection.InsertMany(cars);

            List<TerminMongo> termine = new Faker<TerminMongo>().Rules((f, s) =>
            {
                //         public Customer Kunde { get; set; }
                //public DateTime Datetime { get; set; }
                //public Car Auto { get; set; }
                //public string Summery { get; set; }
                //public string Date { get { return Datetime.ToShortDateString(); } }
                //public string Time { get { return Datetime.TimeOfDay.ToString(); } }
                //public bool accepted { get; set; }
                //public bool IsDone { get; set; }

                s.guid = f.Random.Guid();
                s.Kunde = customers[f.Random.Number(0, customers.Count() - 1)];
                s.Datetime = f.Date.Between(new DateTime(2023, 12, 1), new DateTime(2023, 12, 10).AddDays(multi));
                s.Auto = cars[f.Random.Number(0, cars.Count() - 1)];
                s.Summery = f.Lorem.Sentence();
                s.accepted = f.Random.Bool();
                s.IsDone = f.Random.Bool();
            })
            .Generate(20 * multi)
            .ToList();
            _termineCollection.InsertMany(termine);



            List<Day> days = new Faker<Day>("de")
                .Rules((f, d) =>
                {
                    d.Date = new DateOnly(2023, 12, 1).AddDays(f.Random.Number(1, 1 * multi));
                    d.Termine = termine.Where(t => new DateOnly(d.Date.Year, d.Date.Month, d.Date.Day) == new DateOnly(t.Datetime.Year, t.Datetime.Month, t.Datetime.Day)).ToList();
                })
                .Generate(10 * multi);
            _dayCollection.InsertMany(days);

            //var testTermine = termine.ToList();
            //foreach (Day day in days)
            //{
            //    for (int i = 1; i <= 2; i++)
            //    {
            //        Random random = new Random();
            //        var termin = testTermine.OrderBy(x => random.Next()).Take(2).ToList();

            //        day.Termine.Add()
            //    }
            //}
        }
    }
}
