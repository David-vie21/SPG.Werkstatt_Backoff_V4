using DBI.MongoRepoGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG.Werkstatt.Domian.MongoModels
{
    [BsonCollection("Day")]
    public class Day : Document
    {
        public DateOnly Date { get; set; }
        public List<TerminMongo> Termine { get; set; }
    }
}
