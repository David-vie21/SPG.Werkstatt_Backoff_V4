To start the projekt:
The solution file is in the SPG.Werkstatt_Backoff_V3 folder

1.
  Downlode the Nuget Packages: 
    Microsoft.EntityFrameworkCore.Sqlite,  
    Microsoft.EntityFrameworkCore.Tools,  
    Bogus,  
    xunit
  
2.
  Change the Path from the Database (Werkstatt.db) in the dates to you own path.

      SPG.Werkstatt_Backoff_V3.csproj
      MainWindow.xaml.cs
      UnitTest1.cs
      WerkstattContext.cs
   all paths have to be the same

3. Start the unit test => the database shoud be createt. 

4. if you get an exception from the converter => 
  delet the "DesignTimeBuild" folder in   "...\SPG.Werkstatt_Backoff_V4\SPG.Werkstatt_Backoff_V3\.vs\SPG.Werkstatt_Backoff_V3\DesignTimeBuild"
  rebuild the solution and start
  
