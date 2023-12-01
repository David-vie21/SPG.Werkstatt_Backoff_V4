using DBI.MongoRepoGeneric;
using MongoDB.Driver;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Context_Test
{
    public class MongoContextTest
    {
        [Fact]
        public void Test1()
        {
            //user:pw@host
            string connectionString = "mongodb://root:1234@localhost:27017"; // Dein Connection String hier
            string databaseName = "WerkstattDB"; // Der Name deiner Datenbank

            var database = new MongoClient(connectionString).GetDatabase(databaseName);
            var werkstattMongoContext = new WerkstattMongoContext(database);


            werkstattMongoContext.Seed();

            Assert.True(werkstattMongoContext._dayCollection.CountDocuments(FilterDefinition<Day>.Empty) > 0);

            var day = new Day();
            day.Date = new DateOnly(2021, 10, 10);
            day.Termine = new List<TerminMongo>()
            {
                new TerminMongo()
            };
            werkstattMongoContext._dayCollection.InsertOne(day);
            Assert.NotNull(werkstattMongoContext._dayCollection.Find(d => d.Date == new DateOnly(2021, 10, 10)));
            var test = werkstattMongoContext._dayCollection.Find(d => d.Date == new DateOnly(2021, 10, 10)); 
        }
           
        }
    }
