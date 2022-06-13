using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using SPG.Werkstatt.Domian;
using SPG.Werkstatt.Domian.Model;

namespace Context_Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                //.UseSqlite("Data Source= Werkstatt.db")
                .UseSqlite("Data Source= I:/Visuel Studio Projekte/SPG.Werkstatt_Backoff_V4/Context_Test/bin/Debug/net6.0/Werkstatt.db")
                .Options;

            WerkstattContext context = new WerkstattContext(options);
            
            context.Database.EnsureDeleted();
            bool result = context.Database.EnsureCreated();
            Assert.True(result);
            context.Seed();
        }
    }
}
