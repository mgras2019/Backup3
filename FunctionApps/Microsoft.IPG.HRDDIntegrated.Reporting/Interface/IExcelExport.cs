namespace Microsoft.IPG.HRDDIntegrated.Reporting.Interface
{
    using System.Data;

    /// <summary>
    /// IExcelExport interface
    /// </summary>
    public interface IExcelExport
    {
        /// <summary>
        /// Export the datatable to excel file
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        byte[] ExportToExcel(DataTable dt);

        /// <summary>
        /// Export the datatable to csv file
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        byte[] ExportToCsv(DataTable dt,bool IsHeaderAppended = true);
    }
}
