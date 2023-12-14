using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;
using SPG.Werkstatt.Domian.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG.Werkstatt_Backoff_Mongo.extraWindows.Update
{
    public class Up_Model
    {
        private WerkstattMongoContext _db;
        public TerminMongo C_termin;
        public CustomerMongo kunde;


        public Up_Model(WerkstattMongoContext db, TerminMongo termin)
        {
            _db = db;
            this.C_termin = termin;
            kunde = C_termin.Kunde;
        }
    }
}
