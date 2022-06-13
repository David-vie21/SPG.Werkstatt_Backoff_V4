using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SPG.Werkstatt_Backoff_V3.Converter
{
    public class LookupConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)values[0];
            var dates = values[1] as HashSet<DateTime>;
            HashSet<DateTime> nDates = new HashSet<DateTime>();
            //only datume:a
            foreach (var d in dates)
            {
                nDates.Add(d.Date);
            }
            bool b = nDates.Contains(date.Date);
            return b;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
