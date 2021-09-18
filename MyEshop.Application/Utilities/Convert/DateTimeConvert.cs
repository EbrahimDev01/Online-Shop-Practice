using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.Utilities.Convert
{
    public static class DateTimeConvert
    {
        public static string ToSolarHistory(this DateTime dateTime)
        {

            var pc = new PersianCalendar();

            try
            {
                return pc.GetYear(dateTime) + "/" +
                    pc.GetMonth(dateTime).ToString("00") + "/" +
                    pc.GetDayOfMonth(dateTime).ToString("00");
            }
            catch
            {
                return null;
            }
        }

        public static string ToSolarHistory(this string dateTime) =>
            (string.IsNullOrEmpty(dateTime)) ? null : DateTime.Parse(dateTime).ToSolarHistory();

    }

}
