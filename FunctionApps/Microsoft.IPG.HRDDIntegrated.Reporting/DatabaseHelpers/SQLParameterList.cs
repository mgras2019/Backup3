namespace Microsoft.IPG.HRDDIntegrated.Reporting.DatabaseHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    internal class SqlParameterList
    {
        /// <summary>
        /// The parameters
        /// </summary>
        private readonly List<SqlParameter> parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlParameterList"/> class.
        /// </summary>
        public SqlParameterList()
        {
            parameters = new List<SqlParameter>();
        }

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns></returns>
        public SqlParameter[] ToArray()
        {
            return parameters.ToArray();
        }

        /// <summary>
        /// Adds the int parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        internal void AddIntParameter(string name, int value)
        {
            var intParameter = new SqlParameter(name, System.Data.SqlDbType.Int);
            intParameter.Direction = System.Data.ParameterDirection.Input;
            intParameter.Value = value;

            parameters.Add(intParameter);
        }

        /// <summary>
        /// Adds the varchar parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        internal void AddVarcharParameter(string name, string value)
        {
            var varcharParameter = new SqlParameter(name, System.Data.SqlDbType.VarChar);
            varcharParameter.Direction = System.Data.ParameterDirection.Input;
            varcharParameter.Value = value;

            parameters.Add(varcharParameter);
        }

        /// <summary>
        /// Adds the boolean parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        internal void AddBooleanParameter(string name, bool value)
        {
            var varcharParameter = new SqlParameter(name, System.Data.SqlDbType.Bit);
            varcharParameter.Direction = System.Data.ParameterDirection.Input;
            varcharParameter.Value = value;

            parameters.Add(varcharParameter);
        }

        /// <summary>
        /// Adds the boolean parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        internal void AddIntOutParameter(string name)
        {
            var outParameter = new SqlParameter(name, System.Data.SqlDbType.Int);
            outParameter.Direction = System.Data.ParameterDirection.Output;

            parameters.Add(outParameter);
        }

        /// <summary>
        /// Adds the long parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        internal void AddLongParameter(string name, long value)
        {
            var intParameter = new SqlParameter(name, System.Data.SqlDbType.BigInt);
            intParameter.Direction = System.Data.ParameterDirection.Input;
            intParameter.Value = value;

            parameters.Add(intParameter);
        }

        /// <summary>
        /// Adds the long parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        internal void AddDateParameter(string name, DateTime value)
        {
            var intParameter = new SqlParameter(name, System.Data.SqlDbType.DateTime);
            intParameter.Direction = System.Data.ParameterDirection.Input;
            intParameter.Value = value;

            parameters.Add(intParameter);
        }

        /// <summary>
        /// Adds the nvarchar parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        internal void AddNVarcharParameter(string name, string value)
        {
            var varcharParameter = new SqlParameter(name, System.Data.SqlDbType.NVarChar);
            varcharParameter.Direction = System.Data.ParameterDirection.Input;
            varcharParameter.Value = value;

            parameters.Add(varcharParameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="typeName"></param>
        internal void AddStructuredParameter(string name, DataTable value, string typeName)
        {
            var structuredParameter = new SqlParameter(name, System.Data.SqlDbType.Structured);
            structuredParameter.Direction = System.Data.ParameterDirection.Input;
            structuredParameter.Value = value;
            structuredParameter.TypeName = typeName;

            parameters.Add(structuredParameter);
        }
    }
}
