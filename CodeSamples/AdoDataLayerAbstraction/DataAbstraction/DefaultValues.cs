using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public sealed class DefaultValues
    {


        private DefaultValues()
        {
            // do not allow an instance to be created

        }

        public static DateTime DateTimeMinValue
        {

            //Return CDate(SqlDateTime.MinValue).AddYears(30)
            get
            {
                return Convert.ToDateTime("01-01-1900");
            }
        }

        public static DateTime DateTimeMaxValue
        {

            get
            {
                return new DateTime(9999, 12, 31, 23, 59, 59);
            }

        }

        public static int IdNotSet
        {

            get
            {
                return -1;
            }
        }


        public static int Priority
        {

            get
            {
                return 5;
            }
        }

        private static string _GuidEmptyString = Guid.Empty.ToString();
        public static string GuidEmptyString
        {
            get
            {
                return _GuidEmptyString;
            }
        }

    }
}
