using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;
using SPG.Werkstatt.Domian.MongoModels;

namespace SPG.Werkstatt_Backoff_Mongo
{
    public class MainWindowViewModel
    {
        //private WerkstattContext _db;
        private readonly WerkstattMongoContext _dbMongo;
        public List<TerminMongo> Termine { get; set; } = new List<TerminMongo>();
        public List<TerminMongo> TermineTD = new List<TerminMongo>();  //TermineTD
        public HashSet<TerminMongo> TerminsHS { get; } = new HashSet<TerminMongo>();
        public HashSet<DateTime> DatesHS { get; } = new HashSet<DateTime>();
        public List<CustomerMongo> KundenListe { get; set; } = new List<CustomerMongo>();
        public List<CustomerMongo> KundenListeFILTER { get; set; } = new List<CustomerMongo>();


        private TerminMongo _currentTermin;
        public TerminMongo CurrentTermin { get { return _currentTermin; } set { _currentTermin = value; } }

        private DateTime _selectedDate;
        public DateTime SelectedDate { get { return _selectedDate; } set { _selectedDate = value; } }
        public CustomerMongo CurrentCustomer { get; set; }



        public MainWindowViewModel(WerkstattMongoContext db)
        {
            _dbMongo = db;
            Model model = new Model(_dbMongo);
            Termine.AddRange(model.TermineDB);
            KundenListe.AddRange(model.KundeListeDB);
            selectC_changed2();
            //Termine = TermineTD;
            TerminsHS = Termine.ToHashSet<TerminMongo>();
            foreach (TerminMongo t in TerminsHS)
            {
                DatesHS.Add(t.Datetime);
            }
        }
        public List<TerminMongo> selectC_changed()
        {
            Model m = new Model(_dbMongo);
            Termine = m.TermineDB;

            TermineTD.Clear();

            foreach (TerminMongo t in Termine)
            {
                if (DateTime.Compare(t.Datetime.Date, _selectedDate.Date) == 0)
                {
                    TermineTD.Add(t);
                }


            }
            TermineTD = TermineTD.OrderBy(x => x.Datetime).ToList();
            return TermineTD;
        }


        public void selectC_changed2()
        {
            TermineTD.Clear();
            foreach (TerminMongo t in Termine)
            {
                if (DateTime.Compare(t.Datetime.Date, DateTime.Today) == 0)
                {
                    TermineTD.Add(t);
                }

            }
        }

        public List<TerminMongo> changeErledigtTrue()
        {
            _currentTermin.IsDone = true;
            return TermineTD;
        }
        public List<TerminMongo> changeErledigtFalse()
        {

            _currentTermin.IsDone = false;
            return TermineTD;
        }

        public List<CustomerMongo> filterCustomers(string vor, string nach, string tel, string add)
        {
            return KundenListe.Where(c =>
                (string.IsNullOrEmpty(vor) || c.Vorname.ToLower().Contains(vor.ToLower())) &&
                (string.IsNullOrEmpty(nach) || c.Nachname.ToLower().Contains(nach.ToLower())) &&
                (string.IsNullOrEmpty(tel) || c.Tel.ToLower().Contains(tel.ToLower())) &&
                (string.IsNullOrEmpty(add) || c.Addrese.ToLower().Contains(add.ToLower()))
            ).ToList();
        }
        
    }
}
