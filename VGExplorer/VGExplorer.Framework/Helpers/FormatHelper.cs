using System;
using System.Globalization;

namespace VGExplorer.Framework.Helpers
{
    public class FormatHelper
    {

        public const String DATE_FORMAT = "dd/MM/yyyy";

        public const String DATETIME_FORMAT = "dd/MM/yyyy hh:mm:ss";

        public static String GetFormattedDate(DateTime value)
        {
            return value.ToString(DATE_FORMAT);
        }

        public static String GetFormattedDateTime(DateTime value)
        {
            return value.ToString(DATETIME_FORMAT);
        }

        public static DateTime GetFormattedDateAsDate(String value)
        {
            var culture = new CultureInfo("es-ES");
            var date = DateTime.MinValue;
            var result = DateTime.TryParseExact(value, DATE_FORMAT, culture, DateTimeStyles.None, out date);
            return date;
        }

        public static bool IsValidDate(String value)
        {
            var culture = new CultureInfo("es-ES");
            var date = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            var result = DateTime.TryParseExact(value, DATE_FORMAT, culture, DateTimeStyles.None, out date);
            return result;
        }

    }
}
