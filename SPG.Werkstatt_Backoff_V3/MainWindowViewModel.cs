using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;
using SPG.Werkstatt.Domian.MongoModels;

namespace SPG.Werkstatt_Backoff_V3
{
    public class MainWindowViewModel
    {
        private WerkstattContext _db;
        public List<Termin> Termine { get; set; }
        public List<Termin> TermineTD = new List<Termin>();  //TermineTD
        public HashSet<Termin> TerminsHS { get; } = new HashSet<Termin>();
        public HashSet<DateTime> DatesHS { get; } = new HashSet<DateTime>();
        public List<CustomerMongo> KundenListe { get; set; } = new List<CustomerMongo>();


        private Termin _currentTermin;
        public Termin CurrentTermin { get { return _currentTermin; } set { _currentTermin = value; } }

        private DateTime _selectedDate;
        public DateTime SelectedDate { get { return _selectedDate; } set { _selectedDate = value; } }
        public CustomerMongo CurrentCustomer { get; set; }


        public MainWindowViewModel(WerkstattContext db)
        {
            _db = db;
            Termine = new Model(_db).TermineDB;
            KundenListe = new Model(_db).KundeListeDB;
            selectC_changed2();
            //Termine = TermineTD;
            TerminsHS = Termine.ToHashSet<Termin>();
            foreach (Termin t in TerminsHS)
            {
                DatesHS.Add(t.Datetime);
            }
        }
        public List<Termin> selectC_changed()
        {
            Model m = new Model(_db);
            Termine = m.TermineDB;

            TermineTD.Clear();

            foreach (Termin t in Termine)
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
            foreach (Termin t in Termine)
            {
                if (DateTime.Compare(t.Datetime.Date, DateTime.Today) == 0)
                {
                    TermineTD.Add(t);
                }

            }
        }

        public List<Termin> changeErledigtTrue()
        {
            _currentTermin.IsDone = true;
            return TermineTD;
        }
        public List<Termin> changeErledigtFalse()
        {

            _currentTermin.IsDone = false;
            return TermineTD;
        }
    }
}
