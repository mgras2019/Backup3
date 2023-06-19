namespace Microsoft.IPG.HRDDIntegrated.Reporting
{
    using ClosedXML.Excel;
    using Microsoft.IPG.HRDDIntegrated.Reporting.Interface;
    using System;
    using System.Data;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Excel Export class
    /// </summary>
    public class ExcelExport : IExcelExport
    {
        /// <summary>
        /// Return the datatable as excel file
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public byte[] ExportToExcel(DataTable dt)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public byte[] ExportToCsv(DataTable dt, bool isHeaderAppended = false)
        {
            StringBuilder sb = new StringBuilder();
            int columns = dt.Columns.Count;

            // appending column Header
            if (!isHeaderAppended)
            {
                int count = 0;
                foreach (DataColumn column in dt.Columns)
                {
                    count++;
                         sb.Append(String.Format("\"{0}\"", column.ColumnName));
                  //  sb.Append($"{column.ColumnName}");
                    if (count != columns)
                    {
                        sb.Append(",");
                    }
                }
            }
            // line break
            sb.AppendLine();

            // Appending rows
            int rows = dt.Rows.Count;
            int columnCount;
            int rowCount = 0;
            foreach (DataRow row in dt.Rows)
            {
                rowCount++;
                columnCount = 0;
                foreach (var item in row.ItemArray)
                {
                    columnCount++;
                    var result = EscapeValue(Convert.ToString(item));
                    sb.Append(String.Format("\"{0}\"", result));
                    if (columnCount != columns)
                    {
                        sb.Append(",");
                    }
                }
                if (rowCount != rows)
                {
                    sb.AppendLine();
                }
            }
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private static String EscapeValue(String value)
        {
            // quick short-circuit check
            if (String.IsNullOrEmpty(value)) return String.Empty;

            // If the value has a comma or a quote, we need to:
            // 1. Wrap the value in quotations
            // 2. Convert any existing quotations to double-quotations
            if (value.IndexOf(',') > -1 || value.IndexOf('"') > -1)
            {
                return value.Replace("\"", "'");
            }
            // No modification needed--return as-is.
            return value;
        }
    }
}
