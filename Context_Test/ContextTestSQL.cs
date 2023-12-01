using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;

namespace Context_Test
{
    public class ContextTestSQL
    {
        [Fact]
        public void Test1()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                //.UseSqlite("Data Source= Werkstatt.db")
                .UseSqlite("Data Source= D:\\5 Klasse\\DBI\\Projekt\\SPG.Werkstatt_Backoff_V4\\Werkstatt.db")
                .Options;

            WerkstattContext context = new WerkstattContext(options);
            
            context.Database.EnsureDeleted();
            bool result = context.Database.EnsureCreated();
            Assert.True(result);
            context.Seed();
        }
    }
}
