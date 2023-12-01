using DBI.MongoRepoGeneric;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG.Werkstatt.Domian.Model
{
    [BsonCollection("Customer")]

    public class CustomerMongo : Document
    {
        //public int Id { get; set; }
        public Guid guid { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Addrese { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Name { get { return Vorname + " " + Nachname; } }

        public override string ToString()
        {
            return $"Name: {Vorname} {Nachname}, Tel.: {Tel}";
        }
        public string getAllInfos()
        {
            return $"{Id} {Vorname} {Nachname} {Addrese} {Tel} {Email}";
        }
        public CustomerMongo(ObjectId id, string vorname, string nachname, string addrese, string tel, string email, Guid guid)
        {
            this.Id = id;
            this.Vorname = vorname;
            this.Nachname = nachname;
            this.Addrese = addrese;
            this.Tel = tel;
            this.Email = email;

        }
        public CustomerMongo(string vorname, string nachname, string addrese, string tel, string email, Guid guid)
        {
            this.Vorname = vorname;
            this.Nachname = nachname;
            this.Addrese = addrese;
            this.Tel = tel;
            this.Email = email;
            this.guid = guid;

        }
        public CustomerMongo() { }
    }
}
