using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;
using SPG.Werkstatt.Domian.MongoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG.Werkstatt_Backoff_V3.extraWindows.Update
{
    public class Up_Model
    {
        private WerkstattContext _db;
        public Termin C_termin;
        public CustomerMongo kunde;


        public Up_Model(WerkstattContext db, Termin termin)
        {
            _db = db;
            this.C_termin = termin;
            kunde = C_termin.Kunde;
        }
    }
}
