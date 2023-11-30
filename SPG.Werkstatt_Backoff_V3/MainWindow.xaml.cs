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
using SPG.Werkstatt_Backoff_V3.extraWindows;
using SPG.Werkstatt_Backoff_V3.extraWindows.Update;
using SPG.Werkstatt.Domian.MongoModels;

namespace SPG.Werkstatt_Backoff_V3
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WerkstattContext _db;


        public MainWindow()
        {
            InitializeComponent();

            DbContextOptions options = new DbContextOptionsBuilder().EnableSensitiveDataLogging()
               //.UseSqlite("Data Source= Werkstatt.db")
               .UseSqlite("Data Source= D:\\5 Klasse\\DBI\\Projekt\\SPG.Werkstatt_Backoff_V4\\Werkstatt.db")
               //C:/ Users / User / Desktop / SPG.Werkstatt_Backoff_V4 / SPG.Werkstatt_Backoff_V4 / Context_Test / bin / Debug / net6.0 / Werkstatt.db
.Options;

            _db = new WerkstattContext(options);
            MinHeight = 800;

            DataContext = new MainWindowViewModel(_db);

            TerminListe.MinHeight = 300;

            //Gewisse Daten im Kalender anzeigen lassen:
            var tListVAR = _db.Termine;

            List<Termin> tList = new List<Termin>();
            if (tListVAR != null)
            {
                tList = tListVAR.Cast<Termin>().ToList();
            }
            //Datume rausholen
            List<DateTime> dates = new List<DateTime>();
            foreach (Termin t in tList)
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
            Termin termin = sender.GetType().GetProperty("DataContext").GetValue(sender, null) as Termin;
            //Init
            Termin? existingTermin = _db.Termine.SingleOrDefault(t => t.Id == termin.Id);
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
            _db.Termine.Update(termin);
            _db.SaveChanges();
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
                Termin t2 = sender.GetType().GetProperty("DataContext").GetValue(sender, null) as Termin;

                //Init
                Termin? existingTermin = _db.Termine.SingleOrDefault(t => t.Id == t2.Id);
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
                    _db.Termine.Update(t2);
                    _db.SaveChanges();

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
                Termin t2 = sender.GetType().GetProperty("DataContext").GetValue(sender, null) as Termin;
                //Init
                Termin? existingTermin = _db.Termine.SingleOrDefault(t => t.Id == t2.Id);

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
                    _db.Termine.Update(t2);
                    _db.SaveChanges();

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
                Termin t2 = sender.GetType().GetProperty("DataContext").GetValue(sender, null) as Termin;
                //Init
                Termin? existingTermin = _db.Termine.SingleOrDefault(t => t.Id == t2.Id);
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
                    _db.Termine.Update(t2);
                    _db.SaveChanges();

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
                Termin t1 = sender.GetType().GetProperty("DataContext").GetValue(sender, null) as Termin;
                if (t1 == null)
                    t1 = ((MainWindowViewModel)DataContext).CurrentTermin;

                Termin? existingTermin = _db.Termine.SingleOrDefault(t => t.Id == t1.Id);
                if (existingTermin == null || t1 == null)
                {
                    MessageBox.Show("... not found!");
                    return;
                }

                _db.Remove<Termin>(t1);
                _db.SaveChanges();


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
            DataContext = new MainWindowViewModel(_db);
        }

        private void ButtonNEW_Click(object sender, RoutedEventArgs e)
        {
            NewTerminWindow win2 = new NewTerminWindow(_db);
            win2.Show();
        }

        private void ButtonUPDATE_Click(object sender, RoutedEventArgs e)
        {
            if (((MainWindowViewModel)DataContext).CurrentTermin is not null)
            {
                Up_W win3 = new Up_W(((MainWindowViewModel)DataContext).CurrentTermin, _db);
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