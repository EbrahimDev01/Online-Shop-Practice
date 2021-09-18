using MyEshop.Application.Utilities.Convert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyEshop.Test.UtilitiesTest.ConvertTest
{
    public class DateTimeConvertTest
    {
        [Theory]
        [InlineData("2021-09-05", "1400/06/14")]
        [InlineData("2019-05-30", "1398/03/09")]
        [InlineData(null, null)]
        public void Test_To_Solar_History_Input_DateTime_Gergoian_Result_Date_Time_Persian(DateTime gregorianHistory, string solarHistory)
        {
            string gregorianHistoryToSolarHisotry = gregorianHistory.ToSolarHistory();

            Assert.Equal(solarHistory, gregorianHistoryToSolarHisotry);
        }

        [Theory]
        [InlineData("2021-09-05", "1400/06/14")]
        [InlineData("2019-05-30", "1398/03/09")]
        [InlineData(null, null)]
        public void Test_To_Solar_History_Input_String_DateTime_Gergoin_Result_Date_Time_Persian(string gregorianHistory, string solarHistory)
        {
            string gregorianHistoryToSolarHisotry = gregorianHistory.ToSolarHistory();

            Assert.Equal(solarHistory, gregorianHistoryToSolarHisotry);
        }
    }
}
