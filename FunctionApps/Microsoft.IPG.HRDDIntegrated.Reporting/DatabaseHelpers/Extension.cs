using System;
using System.Data;
using System.Globalization;

namespace Microsoft.IPG.HRDDIntegrated.Reporting.DatabaseHelpers
{
    public static class Extension
    {
        /// <summary>
        /// Extract the data for a given field from DataRow
        /// </summary>
        /// <typeparam name="T"> Type of the Field </typeparam>
        /// <param name="row">          Data Row containing data </param>
        /// <param name="fieldName">    Field name to be extracted from data row </param>
        /// <param name="defaultValue"> Default value to be returned if the value is NULL </param>
        /// <returns> Field value if it is not NULL else default value </returns>
        public static T ExtractFieldValue<T>(this DataRow row, string fieldName, T defaultValue)
        {
            if (row != null && row[fieldName] != DBNull.Value)
            {
                return (T)Convert.ChangeType(row[fieldName], typeof(T), CultureInfo.InvariantCulture);
            }

            return (T)defaultValue;
        }
    }
}
