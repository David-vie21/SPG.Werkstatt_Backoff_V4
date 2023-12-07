//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SPG.Werkstatt.Domian.Model
//{
//    public class Termin
//    {
       
//        public int Id { get; set; }
//        public Guid guid { get; set; }

//        public CustomerMongo Kunde { get; set; }
//        public DateTime Datetime { get; set; }
//        public CarMongo Auto { get; set; }
//        public string Summery { get; set; }
//        public string Date { get { return Datetime.ToShortDateString(); } }
//        public string Time { get { return Datetime.TimeOfDay.ToString(); } }
//        public bool accepted { get; set; }
//        public bool IsDone { get; set; }

//        public Termin() { }

//        public Termin( Guid guid, CustomerMongo kunde, DateTime datetime, CarMongo auto, string summery, bool accepted, bool isDone)
//        {
//            this.guid = guid;
//            Kunde = kunde;
//            Datetime = datetime;
//            Auto = auto;
//            Summery = summery;
//            this.accepted = accepted;
//            IsDone = isDone;
//        }

//        public Termin(int id, Guid guid, CustomerMongo kunde, DateTime datetime, CarMongo auto, string summery, bool accepted, bool isDone)
//        {
//            Id = id;
//            this.guid = guid;
//            Kunde = kunde;
//            Datetime = datetime;
//            Auto = auto;
//            Summery = summery;
//            this.accepted = accepted;
//            IsDone = isDone;
//        }
//    }
//}
