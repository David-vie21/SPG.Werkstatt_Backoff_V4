//using DBI.MongoRepoGeneric;
using Bogus;
using MongoDB.Driver;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;
using SPG.Werkstatt.Domian.MongoModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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

            //var database = new MongoClient(connectionString).GetDatabase(databaseName);
            var werkstattMongoContext = new WerkstattMongoContext(connectionString, databaseName);


            werkstattMongoContext.Seed(1);

            Assert.True(werkstattMongoContext._dayCollection.CountDocuments(FilterDefinition<Day>.Empty) > 0);
            Assert.True(werkstattMongoContext._termineCollection.CountDocuments(FilterDefinition<TerminMongo>.Empty) > 0);
            Assert.True(werkstattMongoContext._customerCollection.CountDocuments(FilterDefinition<CustomerMongo>.Empty) > 0);
            Assert.True(werkstattMongoContext._carCollection.CountDocuments(FilterDefinition<CarMongo>.Empty) > 0);


            //var day = new Day();
            //day.Date = new DateOnly(2021, 10, 10);
            //day.Termine = new List<TerminMongo>()
            //{
            //    new TerminMongo()
            //};
            //werkstattMongoContext._dayCollection.InsertOne(day);
            //Assert.NotNull(werkstattMongoContext._dayCollection.Find(d => d.Date == new DateOnly(2021, 10, 10)));
            //var test = werkstattMongoContext._dayCollection.Find(d => d.Date == new DateOnly(2021, 10, 10)); 
        }


        [Fact]
        public void SeedTimeTest_Writings()
        {

            //user:pw@host
            string connectionString = "mongodb://root:1234@localhost:27017"; // Dein Connection String hier
            string databaseName = "WerkstattDB"; // Der Name deiner Datenbank

            //var database = new MongoClient(connectionString).GetDatabase(databaseName);
            var werkstattMongoContext = new WerkstattMongoContext(connectionString, databaseName);

            //x1
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            werkstattMongoContext.Seed(1);

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
               ts.Hours, ts.Minutes, ts.Seconds,
               ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SeedTime x1 (hh:mm:ss:ms:ns):" + elapsedTime);

            //x10
            stopwatch.Start();

            werkstattMongoContext.Seed(10);

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
               ts.Hours, ts.Minutes, ts.Seconds,
               ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SeedTime x10 (hh:mm:ss:ms:ns):" + elapsedTime);

            //x100
            stopwatch.Start();

            werkstattMongoContext.Seed(100);

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
               ts.Hours, ts.Minutes, ts.Seconds,
               ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SeedTime x100 (hh:mm:ss:ms:ns):" + elapsedTime);


            //x1000
            stopwatch.Start();

            werkstattMongoContext.Seed(1000);

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
               ts.Hours, ts.Minutes, ts.Seconds,
               ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SeedTime x1000 (hh:mm:ss:ms:ns):" + elapsedTime);

            ////x10000
            //stopwatch.Start();

            //werkstattMongoContext.Seed(1000);

            //stopwatch.Stop();
            //ts = stopwatch.Elapsed;
            //elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
            //   ts.Hours, ts.Minutes, ts.Seconds,
            //   ts.Milliseconds / 10, ts.Milliseconds);
            //LogWriter.LogWrite("SeedTime x10000 (hh:mm:ss:ms:ns):" + elapsedTime);
            Assert.True(true);
        }
        [Fact]
        public void SeedTimeTest_Readings()
        {

            //user:pw@host
            string connectionString = "mongodb://root:1234@localhost:27017"; // Dein Connection String hier
            string databaseName = "WerkstattDB"; // Der Name deiner Datenbank

            //var database = new MongoClient(connectionString).GetDatabase(databaseName);
            var werkstattMongoContext = new WerkstattMongoContext(connectionString, databaseName);
            werkstattMongoContext.Seed(100);
            Stopwatch stopwatch = new Stopwatch();
            TimeSpan ts;
            string elapsedTime = "";

            LogWriter.LogWrite("Mongo - Find:");

            //x100 - Termin without Filter
            stopwatch.Start();

            var termin_withoutFilter = werkstattMongoContext._termineCollection.Find(FilterDefinition<TerminMongo>.Empty);

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("Mongo: Find Termin without Filter (hh:mm:ss:ms:ns):" + elapsedTime);


            //x100 - Date
            stopwatch.Start();

            var day = werkstattMongoContext._dayCollection.Find(d => d.Date == new DateOnly(2023, 12, 1));

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("Mongo: Find Day (hh:mm:ss:ms:ns):" + elapsedTime);

            //x100 - Termin
            stopwatch.Start();

            var termin = werkstattMongoContext._termineCollection.Find(d => d.Datetime.Date == new DateTime(2023, 12, 1));

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("Mongo: Find Termin (hh:mm:ss:ms:ns):" + elapsedTime);

            //x100 - Customer
            stopwatch.Start();

            var customer = werkstattMongoContext._customerCollection.Find(d => d.Name.ToLower() == "david");

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("Mongo: Find Customer (hh:mm:ss:ms:ns):" + elapsedTime);

            //x100 - Car
            stopwatch.Start();

            var car = werkstattMongoContext._carCollection.Find(d => d.Besitzer.Equals(customer));

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("Mongo: Find Car (hh:mm:ss:ms:ns):" + elapsedTime);
        }

        [Fact]
        public void TimeTest_Readings_Filter_Sort_Project()
        {

            //user:pw@host
            string connectionString = "mongodb://root:1234@localhost:27017"; // Dein Connection String hier
            string databaseName = "WerkstattDB"; // Der Name deiner Datenbank

            //var database = new MongoClient(connectionString).GetDatabase(databaseName);
            var werkstattMongoContext = new WerkstattMongoContext(connectionString, databaseName);
            werkstattMongoContext.Seed(100);
            Stopwatch stopwatch = new Stopwatch();
            TimeSpan ts;
            string elapsedTime = "";

            LogWriter.LogWrite("Mongo Find - Filter - Sort:");

            //x100 - Termin -- Filter and Sort 
            stopwatch.Start();

            var termin = werkstattMongoContext._termineCollection.Find( t => t.accepted == true).SortBy(t => t.Kunde).ToList();

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("Mongo: Find Termin  - Filter and Sort (hh:mm:ss:ms:ns):" + elapsedTime);

            //x100 - Termin -- Filter and Project

            var filter = Builders<TerminMongo>.Filter.Eq(t => t.accepted,true);
            var projection = Builders<TerminMongo>.Projection.Include(p => p.guid); //.Exclude(p => p.Auto)
            stopwatch.Start();


            var result = werkstattMongoContext._termineCollection.Find(filter).Project(projection).FirstOrDefault();

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("Mongo: Find Termin  - Filter and Projekt (hh:mm:ss:ms:ns):" + elapsedTime);

            //x100 - Termin -- Filter, Project and Sort

            var filter2 = Builders<TerminMongo>.Filter.Eq(t => t.accepted, true);
            var projection2 = Builders<TerminMongo>.Projection.Include(p => p.guid); //.Exclude(p => p.Auto)
            stopwatch.Start();


            var result2 = werkstattMongoContext._termineCollection.Find(filter).Project(projection).FirstOrDefault();

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("Mongo: Find Termin  - Filter, Projekt and Sort (hh:mm:ss:ms:ns):" + elapsedTime);
        }
    }

    // Logging Class
    public static class LogWriter
    {
        private static string m_exePath = string.Empty;
        public static void LogWrite(string logMessage)
        {
            //m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            m_exePath = "I:\\Dokumente 4TB\\HTL\\5 Klasse\\DBI\\Projekt\\SPG.Werkstatt_Backoff_V4";
            if (!File.Exists(m_exePath + "\\" + "log.txt"))
                File.Create(m_exePath + "\\" + "log.txt");

            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                    AppendLog(logMessage, w);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static void AppendLog(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
