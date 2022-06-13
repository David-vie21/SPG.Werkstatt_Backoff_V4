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
using System.Windows.Shapes;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;


namespace SPG.Werkstatt_Backoff_V3.extraWindows
{
    /// <summary>
    /// Interaktionslogik für NewTerminWindow.xaml
    /// </summary>
    public partial class NewTerminWindow : Window
    {
        Customer selectedCustomerNA;
        WerkstattContext _db;


        public NewTerminWindow(WerkstattContext db)
        {
            _db = db;
            MinHeight = 530;
            InitializeComponent();
            DataContext = new NewT_ViewModel(_db);
            KundenListe.ItemsSource = _db.Customers.ToList();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(KundenListe.ItemsSource);
            view.Filter = UserFilter;

        }

        private void NewKundeNT_Click(object sender, RoutedEventArgs e)
        {
            VorKundeGrid.IsEnabled = false;
            VorKundeGrid.Visibility = Visibility.Hidden;

            KuAGrind.IsEnabled = true;
            KuAGrind.Visibility = Visibility.Visible;

            KundenListe.SelectedItem = null;
            AutoListe.SelectedItem = null;
        }

        private void ExsitKundeNT_Click(object sender, RoutedEventArgs e)
        {
            KuAGrind.IsEnabled = false;
            KuAGrind.Visibility = Visibility.Hidden;

            VorKundeGrid.IsEnabled = true;
            VorKundeGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void KundenListe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCustomerNA = KundenListe.SelectedItem as Customer;
            if (selectedCustomerNA != null)
                Console.WriteLine(selectedCustomerNA.ToString());
            Customer selectedCustomer = KundenListe.SelectedItem as Customer;

            AutoListe.ItemsSource = null;
            AutoListe.Items.Refresh();

            if (selectedCustomer != null)
            {
                AutoListe.ItemsSource = ((NewT_ViewModel)DataContext).getCarsFromKunde(selectedCustomer);
                AutoListe.Items.Refresh();
            }
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            if (KuAGrind.IsEnabled == true)
            {
                NewKundeAndNewCar_NEWTERMIN();
            }
            else if (VorKundeGrid.IsEnabled == true)
            {
                if (NeuCarVorhandKundeGrid.IsEnabled == true)
                {
                    NewTermin_ExsistKunde_NewCar();
                }
                else if (AutoListe.IsEnabled == true)
                {
                    newTermin_ExCAR_ExKunde();
                }
            }
        }

        public void NewKundeAndNewCar_NEWTERMIN()
        {


            Customer kunde = getSetKunde();
            int kundenId = kunde.Id;

            DateTime datum = t_Date.SelectedDate.Value;

            Car? car = getSetCar(kunde);

            string summ = BeschNT.Text;
            bool accep = true;
            bool isDone = false;



            //int id, Guid guid, Customer kunde, DateTime datetime, Car auto, string summery, bool accepted, bool isDone
            Guid guid = Guid.NewGuid();
            Console.WriteLine(guid);
            Termin newTermin = new Termin(guid: guid, kunde: kunde, datetime: datum, auto: car, summery: summ, accepted: accep, isDone: isDone);
            _db.Termine.Add(newTermin);
            _db.SaveChanges();

        }

        public Customer getSetKunde()
        {
            //KundenListe AutoListe       

            if (AutoListe.SelectedItem != null)
            {
                Car car = (Car)AutoListe.SelectedItem;
                return car.Besitzer;
            }
            else if (KundenListe.SelectedItem != null)
            {
                Customer customer = (Customer)KundenListe.SelectedItem;
                return customer;
            }
            else
            { //Neuer Kunde wird in DB geschriben und die ID wird zurückgegeben
                string VN = k_VN.Text;
                string NN = k_NN.Text;
                string ADD = k_ADDR.Text;
                string TELL = k_TEL.Text;
                string Mail = k_MAIL.Text;

                //string vorname, string nachname, string addrese, string tel, string email, Guid guid
                Guid guid = Guid.NewGuid();
                Customer newCus = new Customer(vorname: VN, nachname: NN, addrese: ADD, tel: TELL, email: Mail, guid: guid);

                //wird in DB geschrieben
                _db.Customers.Add(newCus);
                _db.SaveChanges();

                return newCus;
            }
        }

        //public int getKunde(string VN, string NN, string ADDR, string TELL, string Mail)
        //{
        //    char c = '"';
        //    string query2 = "Select k_Id from k_kunden where (k_vorname = " + c + VN + c + " AND k_nachname = " + c + NN + c + " AND k_addrese = " + c + ADDR + c + " AND k_tel = " + c + TELL + c + " AND k_mail = " + c + Mail + c + ")";
        //    string MySqlConnectionString2 = "datasource=127.0.0.1;port=3306;username=root;passwort=;database=2022_SPG_werkstatt";
        //    Console.WriteLine(query2);

        //    MySqlConnection databseConnection2 = new MySqlConnection(MySqlConnectionString2);
        //    MySqlCommand command2 = new MySqlCommand(query2, databseConnection2);
        //    command2.CommandTimeout = 60;
        //    try
        //    {
        //        databseConnection2.Open();
        //        MySqlDataReader myReader = command2.ExecuteReader();

        //        if (myReader.HasRows)
        //        {
        //            while (myReader.Read())
        //            {
        //                //Console.WriteLine(myReader.GetString(0) + " - " + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetString(3) + " - " + myReader.GetString(4) + " - " + myReader.GetString(5));

        //                //public int Id { get; set; }
        //                //public string Vorname { get; set; }
        //                //public string Nachname { get; set; }
        //                //public string Addrese { get; set; }
        //                //public string Tel { get; set; }
        //                //public string Email { get; set; }
        //                return myReader.GetInt32(0);
        //            }
        //        }
        //        else
        //        {
        //            //MessageBox.Show("Querry successfully executed");
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        MessageBox.Show("Querry is not executable GetKunde:. " + e.Message);

        //    }
        //    databseConnection2.Close();
        //    return -1;

        //}

        public Car? getSetCar(Customer kunde)
        {
            //AutoListe       

            if (AutoListe.SelectedItem != null)
            {
                Car car = (Car)AutoListe.SelectedItem;
                return car;
            }
            else
            { //Neues Auto wird in DB geschriben und die ID wird zurückgegeben
                string MA = a_Ma.Text;
                string MO = a_Mo.Text;
                string Ke = a_Ke.Text;

                if (a_Da.SelectedDate.Value == null)
                {
                    MessageBox.Show("Kein Auto oder Datum ausgewählt");
                    return null ;
                }

                DateTime Erst = a_Da.SelectedDate.Value;
                //  SqlDateTime sqlForm2 = (SqlDateTime)new SqlDateTime(Erst).ToSqlString();

                decimal KW = Convert.ToDecimal(a_KW.Text);


                //wird in DB geschrieben

                //string marke, string kennzeichen, decimal kw, Customer besitzer2, string modell, DateTime erstzulassung
                Guid g = Guid.NewGuid();
                Car newCar = new Car(MA, Ke, KW, kunde, MO, Erst, g);
                _db.Cars.Add(newCar);
                _db.SaveChanges();

                //get ID from Kunde
                return newCar;


            }
        }

        //public int getCar(int KundenID, string MA, string MO, string Ke, string sqlForm2, decimal KW)
        //{

        //    //HIER IST PROBLEM
        //    char c = '"';
        //    string query2 = "Select c_Id from c_cars where (c_k_besitzer = " + KundenID + " AND c_marke = " + c + MA + c + " AND c_model = " + c + MO + c + " AND c_kennzeichen = " + c + Ke + c + " AND c_erstzulassung = " + c + sqlForm2 + c + " AND c_kw = " + KW + ")";
        //    string MySqlConnectionString2 = "datasource=127.0.0.1;port=3306;username=root;passwort=;database=2022_SPG_werkstatt";
        //    Console.WriteLine(query2);

        //    MySqlConnection databseConnection2 = new MySqlConnection(MySqlConnectionString2);
        //    MySqlCommand command2 = new MySqlCommand(query2, databseConnection2);
        //    command2.CommandTimeout = 60;
        //    try
        //    {
        //        databseConnection2.Open();
        //        MySqlDataReader myReader = command2.ExecuteReader();

        //        if (myReader.HasRows)
        //        {

        //            //Console.WriteLine(myReader.GetString(0) + " - " + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetString(3) + " - " + myReader.GetString(4) + " - " + myReader.GetString(5));

        //            //public int Id { get; set; }
        //            //public string Vorname { get; set; }
        //            //public string Nachname { get; set; }
        //            //public string Addrese { get; set; }
        //            //public string Tel { get; set; }
        //            //public string Email { get; set; }
        //            myReader.Read();
        //            int i = myReader.GetInt32(0);

        //            return i;

        //        }
        //        else
        //        {
        //            //MessageBox.Show("Querry successfully executed");
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        MessageBox.Show("Querry is not executable in GetCAR:." + e.Message);

        //    }
        //    databseConnection2.Close();
        //    return -1;

        //}

        private void NeuesAutoZUVorhandenKunde_Clicker(object sender, RoutedEventArgs e)
        {
            if (NeuCarVorhandKundeGrid.IsEnabled == true)
            {
                NeuCarVorhandKundeGrid.Visibility = Visibility.Hidden;
                NeuCarVorhandKundeGrid.IsEnabled = false;

                AutoListe.IsEnabled = true;
                AutoListe.Visibility = Visibility.Visible;

            }
            else
            {
                NeuCarVorhandKundeGrid.Visibility = Visibility.Visible;
                NeuCarVorhandKundeGrid.IsEnabled = true;

                AutoListe.IsEnabled = false;
                AutoListe.Visibility = Visibility.Hidden;

            }


        }

        public void NewTermin_ExsistKunde_NewCar()
        {
            if (KundenListe.SelectedItem == null && selectedCustomerNA == null)
            {

                MessageBox.Show("Kein Kunde ausgewählt");
            }
            else
            {
                Guid guid = Guid.NewGuid();
                Customer c = new Customer("test", "test", "test", "test", "test", guid);
                if (KundenListe.SelectedItem != null)
                    c = selectedCustomerNA;
                Car car = SetGetNEWCar(c);
                SetNewTermin_ExsitsKunde_NewCar(car);


            }

        }

        public Car SetGetNEWCar(Customer kunde)
        {
            //Neues Auto wird in DB geschriben
            string MA = Na_Ma.Text;
            string MO = Na_Mo.Text;
            string Ke = Na_Ke.Text;

            if (Na_Da.SelectedDate.Value == null)
            {
                MessageBox.Show("Kein Datum ausgewählt");

            }

            DateTime Erst = Na_Da.SelectedDate.Value;

            decimal KW = Convert.ToDecimal(Na_KW.Text);


            //wird in DB geschrieben
            Guid guid = Guid.NewGuid();
            Car car = new Car(MA, Ke, KW, kunde, MO, Erst, guid);
            _db.Cars.Add(car);
            _db.SaveChanges();
            return car;

        }

        public void SetNewTermin_ExsitsKunde_NewCar(Car car)
        {

            Customer kunden = selectedCustomerNA;

            DateTime datum = t_Date.SelectedDate.Value;
            


            string summ = BeschNT.Text;
            bool accep = true;
            bool isDone = false;

            Guid guid = Guid.NewGuid();
            Termin termin = new Termin(guid, kunden, datum, car, summ, accep, isDone);
            _db.Termine.Add(termin);
            _db.SaveChanges();

        }

        public void newTermin_ExCAR_ExKunde()
        {

            Customer kunde = selectedCustomerNA;

            DateTime datum = t_Date.SelectedDate.Value;

            Car c1 = (Car)AutoListe.SelectedItem;

            string summ = BeschNT.Text;
            bool accep = true;
            bool isDone = false;

            Guid guid = Guid.NewGuid();
            Termin termin = new Termin(guid, kunde, datum, c1, summ, accep, isDone);
            _db.Add(termin);
            _db.SaveChanges();
        }

        private void TxtFilter_Change(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(KundenListe.ItemsSource).Refresh();

        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(TxtFilter.Text))
                return true;
            else
                return ((item as Customer).Name.IndexOf(TxtFilter.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }
    }



}

