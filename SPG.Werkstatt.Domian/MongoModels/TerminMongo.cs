using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.MongoRepoGeneric;
using MongoDB.Bson;
using SPG.Werkstatt.Domian.Model;

namespace SPG.Werkstatt.Domian.MongoModels
{
    [BsonCollection("Termin")]
    public class TerminMongo : Document
    {

        //public int Id { get; set; }
        public Guid guid { get; set; }

        public CustomerMongo Kunde { get; set; }
        public DateTime Datetime { get; set; }
        public CarMongo Auto { get; set; }
        public string Summery { get; set; }
        public string Date { get { return Datetime.ToShortDateString(); } }
        public string Time { get { return Datetime.TimeOfDay.ToString(); } }
        public bool accepted { get; set; }
        public bool IsDone { get; set; }

        public TerminMongo() { }

        public TerminMongo(Guid guid, CustomerMongo kunde, DateTime datetime, CarMongo auto, string summery, bool accepted, bool isDone)
        {
            this.guid = guid;
            Kunde = kunde;
            Datetime = datetime;
            Auto = auto;
            Summery = summery;
            this.accepted = accepted;
            IsDone = isDone;
        }

        public TerminMongo(ObjectId id, Guid guid, CustomerMongo kunde, DateTime datetime, CarMongo auto, string summery, bool accepted, bool isDone)
        {
            Id = id;
            this.guid = guid;
            Kunde = kunde;
            Datetime = datetime;
            Auto = auto;
            Summery = summery;
            this.accepted = accepted;
            IsDone = isDone;
        }
    }
}
