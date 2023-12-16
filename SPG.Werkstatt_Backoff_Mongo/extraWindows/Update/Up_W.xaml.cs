using MongoDB.Driver;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;
using SPG.Werkstatt.Domian.MongoModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SPG.Werkstatt_Backoff_Mongo.extraWindows.Update
{
    /// <summary>
    /// Interaktionslogik für Up_W.xaml
    /// </summary>
    public partial class Up_W : Window
    {
        public WerkstattMongoContext _db;
        public TerminMongo termin;
        

        public Up_W(TerminMongo termin, WerkstattMongoContext db)
        {
            InitializeComponent();
            this.termin = termin;
            _db = db;
            DataContext = new Up_Model(_db, termin);

            K_Name.Content = termin.Kunde.Name;
            Summ.Text = termin.Summery;
            Auto.Text = termin.Auto.ToString();
            if (termin.IsDone)
                Er.IsChecked = true;
            if (termin.accepted)
                Ac.IsChecked = true;

            //CarList.ItemsSource = _db.Cars.Where(c => c.Besitzer.Id == termin.Kunde.Id).ToList();
            CarList.ItemsSource = _db._carCollection.Find(c => c.Besitzer.Id == termin.Kunde.Id).ToList();

            DP.SelectedDate = termin.Datetime;

            K_VN.Text = termin.Kunde.Vorname;
            K_NN.Text = termin.Kunde.Nachname;
            K_AD.Text = termin.Kunde.Addrese;
            K_TE.Text = termin.Kunde.Tel;
            K_MA.Text = termin.Kunde.Email;

            C_MA.Text = termin.Auto.Marke;
            C_MO.Text = termin.Auto.Modell;
            C_ER.Text = termin.Auto.Erstzulassung.ToString();
            C_KW.Text = termin.Auto.Kw.ToString();
            C_KE.Text = termin.Auto.Kennzeichen;



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CarMongo car = termin.Auto;
            CustomerMongo kunde = termin.Kunde;
           


            termin.Summery = Summ.Text;
            if (Er.IsChecked == true)
                termin.IsDone = true;
            if (Er.IsChecked == false)
                termin.IsDone = false;
            if (Ac.IsChecked == true)
                termin.accepted = true;
            if (Ac.IsChecked == false)
                termin.accepted = false;


            termin.Kunde.Vorname = K_VN.Text;
            termin.Kunde.Nachname = K_NN.Text;
            termin.Kunde.Addrese = K_AD.Text;
            termin.Kunde.Tel = K_TE.Text;
            termin.Kunde.Email = K_MA.Text;


            termin.Auto.Marke = C_MA.Text;
            termin.Auto.Modell = C_MO.Text;
            DateTime dt = DateTime.Parse(C_ER.Text);
            termin.Auto.Erstzulassung = dt;
            termin.Auto.Kennzeichen = C_KE.Text;

            //_db.Termine.Update(termin);
            _db._termineCollection.ReplaceOne(t => t.Id == termin.Id, termin);
            //_db.Cars.Update(termin.Auto);
            _db._carCollection.ReplaceOne(c => c.Id == termin.Auto.Id, termin.Auto);
            //_db.Customers.Update(termin.Kunde);
            _db._customerCollection.ReplaceOne(k => k.Id == termin.Kunde.Id, termin.Kunde);
            //_db.SaveChanges();

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                   ts.Hours, ts.Minutes, ts.Seconds,
                   ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            RefrechTEXT.Text = "Refrech Time(hh:mm:ss:ms): " + elapsedTime;
        }
    }
}
