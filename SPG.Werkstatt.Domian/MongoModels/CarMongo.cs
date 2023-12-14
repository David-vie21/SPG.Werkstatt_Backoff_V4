using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DBI.MongoRepoGeneric;
using MongoDB.Bson;
using SPG.Werkstatt.Domian.Model;

namespace SPG.Werkstatt.Domian.Model
{
    [BsonCollection("Car")]

    public class CarMongo : Document
    {
        //public int Id { get; set; }
        public Guid guid { get; set; }

        public string Marke { get; set; }
        public string? Modell { get; set; }
        public string Kennzeichen { get; set; }
        public DateTime? Erstzulassung { get; set; }
        public decimal Kw { get; set; }
        public decimal Ps
        {
            get { return Kw * 1.359m; }
        }
        public CustomerMongo? Besitzer { get; set; }

        public override string ToString()
        {
            return $" Auto: {Marke} {Modell} - {Kennzeichen}";
        }
        public CarMongo(ObjectId id, string marke, string kennzeichen, decimal kw, CustomerMongo besitzer2, string modell, DateTime erstzulassung, Guid guid) // 
        {
            Id = id;
            Marke = marke;
            Modell = modell;
            Kennzeichen = kennzeichen;
            Erstzulassung = erstzulassung;
            Kw = kw;
            if (besitzer2 != null)
                Besitzer = besitzer2;
            this.guid = guid;
        }

        public CarMongo( string marke, string kennzeichen, decimal kw, CustomerMongo besitzer2, string modell, DateTime erstzulassung, Guid guid) // 
        {
            Marke = marke;
            Modell = modell;
            Kennzeichen = kennzeichen;
            Erstzulassung = erstzulassung;
            Kw = kw;
            if (besitzer2 != null)
                Besitzer = besitzer2;
            this.guid = guid;
        }
        public CarMongo()
        { }

    }
}
