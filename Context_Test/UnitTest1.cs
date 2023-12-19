using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace Context_Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                //.UseSqlite("Data Source= Werkstatt.db")
                .UseSqlite("Data Source= I:\\Dokumente 4TB\\HTL\\5 Klasse\\DBI\\Projekt\\SPG.Werkstatt_Backoff_V4\\SPG.Werkstatt_Backoff_V3\\Werkstatt.db")
                .Options;

            WerkstattContext context = new WerkstattContext(options);
            
            context.Database.EnsureDeleted();
            bool result = context.Database.EnsureCreated();
            Assert.True(result);
            context.Seed(1);

        }

        [Fact]
        public void Seed_SpeedTest() 
        {


            LogWriter.LogWrite("SQL:");
            execut(1);
            execut(10);
            execut(100);
            execut(1000);
            execut(10000);


            
        }

        public void execut(int multi)
        {
            DbContextOptions options = new DbContextOptionsBuilder()
               //.UseSqlite("Data Source= Werkstatt.db")
               .UseSqlite("Data Source=Werkstatt.db")
               .Options;

            WerkstattContext context = new WerkstattContext(options);

            context.Database.EnsureDeleted();
            bool result = context.Database.EnsureCreated();


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            TimeSpan ts;
            string elapsedTime = "";
            //x1
            context.Seed(multi);

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
               ts.Hours, ts.Minutes, ts.Seconds,
               ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SQL: SeedTime x" + multi + " (hh:mm:ss:ms:ns):" + elapsedTime);
        }

        [Fact]
        public void SpeedTest_Reading() 
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                    //.UseSqlite("Data Source= Werkstatt.db")
                    .UseSqlite("Data Source= I:\\Dokumente 4TB\\HTL\\5 Klasse\\DBI\\Projekt\\SPG.Werkstatt_Backoff_V4\\SPG.Werkstatt_Backoff_V3\\Werkstatt.db")
                    .Options;

            WerkstattContext context = new WerkstattContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Seed(100);

            Stopwatch stopwatch = new Stopwatch();
            TimeSpan ts;
            string elapsedTime = "";

            LogWriter.LogWrite("SQL Read:");


            //x100 - Termin
            stopwatch.Start();

            var day = context.Termine.ToList().Where(t => t.Datetime.Date == new DateTime(2023, 12, 1));

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SQL: Find Termin (hh:mm:ss:ms:ns):" + elapsedTime);


            //x100 - Customer
            stopwatch.Start();

            var customer = context.Customers.ToList().Where(d => d.Name.ToLower().Equals("david"));

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SQL: Find Customer (hh:mm:ss:ms:ns):" + elapsedTime);

            //x100 - Car
            stopwatch.Start();

            var car = context.Cars.ToList().Where(d => d.Besitzer.Equals(customer));

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                              ts.Hours, ts.Minutes, ts.Seconds,
                                            ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SQL: Find Car (hh:mm:ss:ms:ns):" + elapsedTime);
        }

        [Fact]
        public void TimeTest_Update_One_SQL()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlite("Data Source= I:\\Dokumente 4TB\\HTL\\5 Klasse\\DBI\\Projekt\\SPG.Werkstatt_Backoff_V4\\SPG.Werkstatt_Backoff_V3\\Werkstatt.db")
                .Options;

            WerkstattContext context = new WerkstattContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Seed(100);

            Stopwatch stopwatch = new Stopwatch();
            TimeSpan ts;
            string elapsedTime = "";

            LogWriter.LogWrite("SQL Update:");

            // x100 - Termin Update 
            var termin = context.Termine.FirstOrDefault();
            termin.accepted = true;
            termin.guid = Guid.NewGuid();
            termin.Datetime = DateTime.Now;

            stopwatch.Start();

            if (termin != null)
            {
                context.Termine.Update(termin);
                context.SaveChanges();
            }

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SQL: Update Termin (hh:mm:ss:ms:ns):" + elapsedTime);

            // x100 - Kunden Update 
            var customer = context.Customers.FirstOrDefault();
            customer.guid = Guid.NewGuid();
            customer.Addrese = "Test Add";
            stopwatch.Start();

            if (customer != null)
            {
                context.Customers.Update(customer);
                context.SaveChanges();
            }

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SQL: Update Kunde (hh:mm:ss:ms:ns):" + elapsedTime);

            // x100 - Auto Update 
            var car = context.Cars.FirstOrDefault();
            car.guid = Guid.NewGuid();
            car.Kw = 100;
            stopwatch.Start();

            if (car != null)
            {                
                context.Cars.Update(car);
                context.SaveChanges();
            }

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SQL: Update Auto (hh:mm:ss:ms:ns):" + elapsedTime);
        }

        [Fact]
        public void TimeTest_Delete_SQL()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlite("Data Source= I:\\Dokumente 4TB\\HTL\\5 Klasse\\DBI\\Projekt\\SPG.Werkstatt_Backoff_V4\\SPG.Werkstatt_Backoff_V3\\Werkstatt.db")
                .Options;

            WerkstattContext context = new WerkstattContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Seed(100);

            Stopwatch stopwatch = new Stopwatch();
            TimeSpan ts;
            string elapsedTime = "";

            LogWriter.LogWrite("SQL Delete:");

            // x100 - Termin Delete  
            var termin = context.Termine.FirstOrDefault();

            stopwatch.Start();

            if (termin != null)
            {
                context.Termine.Remove(termin);
                context.SaveChanges();
            }

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SQL: Delete Termin (hh:mm:ss:ms:ns):" + elapsedTime);

            // x100 - Kunden Delete 
            var customer = context.Customers.FirstOrDefault();

            stopwatch.Start();

            if (customer != null)
            {
                context.Customers.Remove(customer);
                context.SaveChanges();
            }

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SQL: Delete Kunde (hh:mm:ss:ms:ns):" + elapsedTime);

            // x100 - Auto Delete 
            var car = context.Cars.FirstOrDefault();
            stopwatch.Start();

            if (car != null)
            {
                context.Cars.Remove(car);
                context.SaveChanges();
            }

            stopwatch.Stop();
            ts = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10, ts.Milliseconds);
            LogWriter.LogWrite("SQL: Delete Auto (hh:mm:ss:ms:ns):" + elapsedTime);
        }

    }

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
