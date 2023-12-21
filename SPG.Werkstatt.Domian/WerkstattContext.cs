using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//using Microsoft.EntityFrameworkCore.SQLServer;
using SPG.Werkstatt.Domian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG.Werkstatt.Domian
{
    public class WerkstattContext : DbContext
    {
        // 1. Entitäten 
        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Termin> Termine => Set<Termin>();



        // 2. Konstruktor erstellen
        public WerkstattContext(DbContextOptions options)
            : base(options)
        { }

        // 3. DB Konfigurieren
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                .UseSqlServer("Server=dbi-sql-server.database.windows.net;Database=Werkstatt;Trusted_Connection=False;User ID=DBI.SQL;Password=admin!123;MultipleActiveResultSets=true",
                options => options.CommandTimeout(180)); // 180 seconds = 3 minutes
                //optionsBuilder.UseSqlServer("Server=dbi-sql-server.database.windows.net;Database=Werkstatt;Trusted_Connection=False;User ID=DBI.SQL;Password=admin!123;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration<Customer>(new CustomerConfiguration());
        }

        public void Seed(int multi)
        {
            Randomizer.Seed = new Random(1017);

            List<Customer> customers = new Faker<Customer>("de")
                .Rules((f, c) =>
                {                      //public string Nachname { get; set; }
                                       //public string Addrese { get; set; }
                                       //public string Tel { get; set; }
                                       //public string Email { get; set; }

                    c.guid = f.Random.Guid();
                    c.Vorname = f.Name.FirstName(Bogus.DataSets.Name.Gender.Female);
                    c.Nachname = f.Name.LastName();
                    c.Email = f.Internet.Email();
                    c.Tel = f.Person.Phone;
                    c.Addrese = f.Address.FullAddress();

                })
            .Generate(10*multi)
            .ToList();
            Customers.AddRange(customers);
            SaveChanges();


            List<Car> cars = new Faker<Car>("de")
            .Rules((f, p) =>
            {
                //public string Marke { get; set; }
                //public string Modell { get; set; }
                //public string Kennzeichen { get; set; }
                //public DateTime Erstzulassung { get; set; }
                //public decimal Kw { get; set; }

                p.guid = f.Random.Guid();
                p.Besitzer = customers[f.Random.Number(0, customers.Count() - 1)];
                p.Marke = f.Company.CompanyName();
                p.Modell = f.Commerce.ProductName();
                p.Kennzeichen = f.Random.Replace("W##-###");
                p.Erstzulassung = f.Date.Past(10, DateTime.Now);
                p.Kw = f.Random.Decimal(60, 500);

            })
            .Generate(10 * multi)
            .ToList();
            Cars.AddRange(cars);
            SaveChanges();


            List<Termin> termine = new Faker<Termin>().Rules((f, s) =>
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
                s.Datetime = f.Date.Past(3, DateTime.Now);
                s.Auto = cars[f.Random.Number(1, cars.Count() - 1)];
                s.Summery = f.Lorem.Sentence();
                s.accepted = f.Random.Bool();
                s.IsDone = f.Random.Bool();
            })
            .Generate(10 * multi)
            .ToList();
            Termine.AddRange(termine);
            SaveChanges();



        }
    }
}
