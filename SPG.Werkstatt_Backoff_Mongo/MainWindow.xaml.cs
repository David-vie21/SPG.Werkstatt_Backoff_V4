using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;
using SPG.Werkstatt_Backoff_Mongo.extraWindows;
using SPG.Werkstatt_Backoff_Mongo.extraWindows.Update;
using SPG.Werkstatt.Domian.MongoModels;
using MongoDB.Driver;

namespace SPG.Werkstatt_Backoff_Mongo
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly WerkstattContext _db;
        private readonly WerkstattMongoContext _dbMongo;


        public MainWindow()
        {
            InitializeComponent();

            //DbContextOptions options = new DbContextOptionsBuilder().EnableSensitiveDataLogging()
            //   //.UseSqlite("Data Source= Werkstatt.db")
            //   .UseSqlite("Data Source= D:\\5 Klasse\\DBI\\Projekt\\SPG.Werkstatt_Backoff_V4\\Werkstatt.db")
            //   //C:/ Users / User / Desktop / SPG.Werkstatt_Backoff_V4 / SPG.Werkstatt_Backoff_V4 / Context_Test / bin / Debug / net6.0 / Werkstatt.db
            //    .Options;

            //_db = new WerkstattContext(options);
            string connectionString = "mongodb://root:1234@localhost:27017"; // Dein Connection String hier
            string databaseName = "WerkstattDB"; // Der Name deiner Datenbank

            _dbMongo = new WerkstattMongoContext(connectionString, databaseName);

            MinHeight = 800;

            DataContext = new MainWindowViewModel(_dbMongo);

            TerminListe.MinHeight = 300;

            //Gewisse Daten im Kalender anzeigen lassen:
            //var tListVAR = _db.Termine;
            var tListVAR = _dbMongo._termineCollection.Find(Builders<TerminMongo>.Filter.Empty).ToList();


            List<TerminMongo> tList = new List<TerminMongo>();
            if (tListVAR != null)
            {
                tList = tListVAR.Cast<TerminMongo>().ToList();
            }
            //Datume rausholen
            List<DateTime> dates = new List<DateTime>();
            foreach (TerminMongo t in tList)
            {
                dates.Add(t.Datetime);
            }

            DateTime[] dtArra = dates.ToArray();
        }

        private void Kalender1_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            TerminListe.ItemsSource = null;
            TerminListe.Items.Refresh();
            TerminListe.ItemsSource = ((MainWindowViewModel)DataContext).selectC_changed();
            TerminListe.Items.Refresh();

        }
        private void CheckBoxAcc_Checked(object sender, RoutedEventArgs e)
        {
            //try
            //{
            TerminMongo termin = sender.GetType().GetProperty("DataContext").GetValue(sender, null) as TerminMongo;
            //Init
            //TerminMongo? existingTermin = _db.Termine.SingleOrDefault(t => t.Id == termin.Id);
            TerminMongo? existingTermin = (TerminMongo?)_dbMongo._termineCollection.Find(t => t.Id == termin.Id).First();

            if (existingTermin == null)
            {
                MessageBox.Show("... not found!");
                return;
            }

            // Act
            termin.accepted = true;
            existingTermin.accepted = true;

            //Save
            //try
            //{
            //_db.Termine.Update(termin);
            _dbMongo._termineCollection.ReplaceOne(t => t.Id == termin.Id, termin);
            //_db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    MessageBox.Show("... went wrong!");
            //}
            //catch (DbUpdateException ex)
            //{
            //    MessageBox.Show("... went wrong!");
            //}

            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show("Somethink went wrong!" + ex);
            //}

            //Update Item Source From Termine List
            TerminListe.ItemsSource = null;
            TerminListe.Items.Refresh();

            TerminListe.ItemsSource = ((MainWindowViewModel)DataContext).selectC_changed();
            TerminListe.Items.Refresh();

        }

        private void CheckBoxAcc_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                TerminMongo t2 = sender.GetType().GetProperty("DataContext").GetValue(sender, null) as TerminMongo;

                //Init
                //TerminMongo? existingTermin = _db.Termine.SingleOrDefault(t => t.Id == t2.Id);
                TerminMongo? existingTermin = (TerminMongo?)_dbMongo._termineCollection.Find(t => t.Id == t2.Id).First();
                if (existingTermin == null)
                {
                    MessageBox.Show("... not found!");
                    return;
                }

                // Act
                t2.accepted = false;
                existingTermin.accepted = false;

                //Save
                try
                {
                    //_db.Termine.Update(t2);
                    _dbMongo._termineCollection.ReplaceOne(t => t.Id == t2.Id, t2);

                   // _db.SaveChanges();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    MessageBox.Show("... went wrong!");
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show("... went wrong!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Somethink went wrong!" + ex);
            }

            //Update Item Source From Termine List
            TerminListe.ItemsSource = null;
            TerminListe.Items.Refresh();

            TerminListe.ItemsSource = ((MainWindowViewModel)DataContext).selectC_changed();
            TerminListe.Items.Refresh();

        }

        private void CheckBoxDone_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                TerminMongo t2 = sender.GetType().GetProperty("DataContext").GetValue(sender, null) as TerminMongo;
                //Init
                var existingTermin = (TerminMongo)_dbMongo._termineCollection.Find<TerminMongo>(t => t.Id == t2.Id).First();
                

                if (existingTermin == null)
                {
                    MessageBox.Show("... not found!");
                    return;
                }

                // Act
                t2.IsDone = true;
                existingTermin.IsDone = true;

                //Save
                try
                {
                    //_db.Termine.Update(t2);
                    _dbMongo._termineCollection.ReplaceOne(t => t.Id == t2.Id, t2);
                    //_db.SaveChanges();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    MessageBox.Show("... went wrong!");
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show("... went wrong!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Somethink went wrong!" + ex);
            }

            //Update Item Source From Termine List
            TerminListe.ItemsSource = null;
            TerminListe.Items.Refresh();

            TerminListe.ItemsSource = ((MainWindowViewModel)DataContext).selectC_changed();
            TerminListe.Items.Refresh();

        }

        private void CheckBoxDone_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                TerminMongo t2 = sender.GetType().GetProperty("DataContext").GetValue(sender, null) as TerminMongo;
                //Init
                TerminMongo? existingTermin = (TerminMongo?)_dbMongo._termineCollection.Find(t => t.Id == t2.Id).First();
                if (existingTermin == null)
                {
                    MessageBox.Show("... not found!");
                    return;
                }

                // Act
                t2.IsDone = false;
                existingTermin.IsDone = false;

                //Save
                try
                {
                    _dbMongo._termineCollection.ReplaceOne(t => t.Id == t2.Id, t2);
                    //_db.SaveChanges();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    MessageBox.Show("... went wrong!");
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show("... went wrong!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Somethink went wrong!" + ex);
            }

            //Update Item Source From Termine List
            TerminListe.ItemsSource = null;
            TerminListe.Items.Refresh();

            TerminListe.ItemsSource = ((MainWindowViewModel)DataContext).selectC_changed();
            TerminListe.Items.Refresh();

        }

        private void ButtonDEL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TerminMongo t1 = sender.GetType().GetProperty("DataContext").GetValue(sender, null) as TerminMongo;
                if (t1 == null)
                    t1 = ((MainWindowViewModel)DataContext).CurrentTermin;

                TerminMongo? existingTermin = (TerminMongo?)_dbMongo._termineCollection.Find(t => t.Id == t1.Id).First();
                if (existingTermin == null || t1 == null)
                {
                    MessageBox.Show("... not found!");
                    return;
                }

                //_db.Remove<TerminMongo>(t1);
                _dbMongo._termineCollection.DeleteOne(t => t.Id == t1.Id);
                //_db.SaveChanges();


                TerminListe.ItemsSource = null;
                TerminListe.Items.Refresh();

                TerminListe.ItemsSource = ((MainWindowViewModel)DataContext).selectC_changed();
                TerminListe.Items.Refresh();
                Console.WriteLine("Termin: " + t1.Id + " -DELETED");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Somethink went wrong as the Del Part! " + ex);
            }
        }

        private void Button_Refrech(object sender, RoutedEventArgs e)
        {
            DataContext = new MainWindowViewModel(_dbMongo);
        }

        private void ButtonNEW_Click(object sender, RoutedEventArgs e)
        {
            NewTerminWindow win2 = new NewTerminWindow(_dbMongo);
            win2.Show();
        }

        private void ButtonUPDATE_Click(object sender, RoutedEventArgs e)
        {
            if (((MainWindowViewModel)DataContext).CurrentTermin is not null)
            {
                Up_W win3 = new Up_W(((MainWindowViewModel)DataContext).CurrentTermin, _dbMongo);
                win3.Show();
            }
            else 
            {
                MessageBox.Show("Kein Termin ausgewählt");
            }
        }

        //private void TxtFilter_Change(object sender, TextChangedEventArgs e)
        //{
        //    CollectionViewSource.GetDefaultView(KundenListe.ItemsSource).Refresh();

        //}

        //private bool UserFilter(object item)
        //{
        //    if (String.IsNullOrEmpty(TxtFilter.Text))
        //        return true;
        //    else
        //        return ((item as Customer).Name.IndexOf(TxtFilter.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);
        //}

    }

}