using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace AdoDataLayerAbstraction
{
    public static class DbHelper
    {
        public delegate void TCreateListFromReader<T>(IDataReader reader, ref List<T> newList);
        public delegate void TCreateListFromReaderWithLookUp<T1, T2, T3>(IDataReader reader, ref IList<T1> newList, out IDictionary<T2, T3> newLookUp);

        //Suggestions from Mike...these may be useful at some point
        //public delegate void TCreateListFromReader2<T>(IDataReader reader, IList<T> list, Func<T> newItem);
        //public delegate Dictionary<T2, T3> TCreateListFromReaderWithLookUp2<T1, T2, T3>(IDataReader reader, IList<T1> list);
        //public delegate void LoadItemFromReader<T>(IDataReader reader, T item);

        #region Type Conversions

        /// <summary>
        /// Converts the value to to a string, or empty string if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value as a string, or <see cref="String.Empty"/> if <see cref="value"/> is null.</returns>
        public static string CStrNull(object value)
        {
            return CStrNull(value, string.Empty);
        }

        /// <summary>
        /// Converts the value to a string, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value to return if <see cref="value"/> is null.</param>
        /// <returns>The value as a string, or <see cref="defaultValue"/> if <see cref="value"/> is null.</returns>
        public static string CStrNull(object value, string defaultValue)
        {
            return CTypeNull(value, defaultValue, v => v.ToString());
        }

        /// <summary>
        /// Converts the value to a boolean, or false if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value as a boolean, or false if null.</returns>
        public static bool CBoolNull(object value)
        {
            return CBoolNull(value, false);
        }

        /// <summary>
        /// Converts the value to a boolean, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if <see cref="value"/> is null.</param>
        /// <returns>The value as a boolean, or <see cref="defaultValue"/> if <see cref="value"/> is null.</returns>
        public static bool CBoolNull(object value, bool defaultValue)
        {
            return CTypeNull(value, defaultValue, Convert.ToBoolean);
        }

        /// <summary>
        /// Converts the value to an integer, or 0 if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value as an integer, or 0 if <see cref="value"/> is null.</returns>
        public static int CIntNull(object value)
        {
            return CIntNull(value, 0);
        }

        /// <summary>
        /// Converts the value to an integer, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if <see cref="value"/> is null.</param>
        /// <returns>The value as an integer, or <see cref="defaultValue"/> if <see cref="value"/> is null.</returns>
        public static int CIntNull(object value, int defaultValue)
        {
            return CTypeNull(value, defaultValue, Convert.ToInt32);
        }

        /// <summary>
        /// Converts the value to a long, or 0 if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value as a long, or 0 if null.</returns>
        public static long CLongNull(object value)
        {
            return CLongNull(value, 0);
        }

        /// <summary>
        /// Converts the value to a long, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if <see cref="value"/> is null.</param>
        /// <returns>The value as a long, or <see cref="defaultValue"/> if <see cref="value"/> is null.</returns>
        public static long CLongNull(object value, long defaultValue)
        {
            return CTypeNull(value, defaultValue, Convert.ToInt64);
        }

        /// <summary>
        /// Converts the value to a short, or 0 if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value as a short, or 0 if null.</returns>
        public static short CShortNull(object value)
        {
            return CShortNull(value, 0);
        }

        /// <summary>
        /// Converts the value to a short, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if <see cref="value"/> is null.</param>
        /// <returns>The value as a short, or <see cref="defaultValue"/> if <see cref="value"/> is null.</returns>
        public static short CShortNull(object value, short defaultValue)
        {
            return CTypeNull(value, defaultValue, Convert.ToInt16);
        }

        /// <summary>
        /// Converts the value to a <see cref="DateTime"/>, or <see cref="DefaultValues.DateTimeMinValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value as a <see cref="DateTime"/> or <see cref="DefaultValues.DateTimeMinValue"/> if null.</returns>
        public static DateTime CDateNull(object value)
        {
            return CDateNull(value, DefaultValues.DateTimeMinValue);
        }

        /// <summary>
        /// Converts the value to a <see cref="DateTime"/>, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if <see cref="value"/> is null.</param>
        /// <returns>The value as a <see cref="DateTime"/>, or <see cref="defaultValue"/> if <see cref="value"/> is null.</returns>
        public static DateTime CDateNull(object value, DateTime defaultValue)
        {
            return CTypeNull(value, defaultValue, Convert.ToDateTime);
        }

        /// <summary>
        /// Converts the value to a <see cref="DateTime"/>, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value as a <see cref="DateTime"/>, or DefaultValues.DateTimeMaxValue if <see cref="value"/> is null.</returns>
        public static DateTime CDateMaxNull(object value)
        {
            return CTypeNull(value, DefaultValues.DateTimeMaxValue, Convert.ToDateTime);
        }

        /// <summary>
        /// Converts the value to a decimal, or 0 if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value as a decimal, or 0 if null.</returns>
        public static decimal CDecNull(object value)
        {
            return CDecNull(value, 0);
        }

        /// <summary>
        /// Converts the value to a decimal, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if <see cref="value"/> is null.</param>
        /// <returns>The value as a decimal, or <see cref="defaultValue"/> if <see cref="value"/> is null.</returns>
        public static decimal CDecNull(object value, decimal defaultValue)
        {
            return CTypeNull(value, defaultValue, Convert.ToDecimal);
        }

        /// <summary>
        /// Converts the value to a double, or 0 if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value as a double, or 0 if null.</returns>
        public static double CDblNull(object value)
        {
            return CDblNull(value, 0);
        }

        /// <summary>
        /// Converts the value to a double, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if <see cref="value"/> is null.</param>
        /// <returns>The value as a double, or <see cref="defaultValue"/> if <see cref="value"/> is null.</returns>
        public static double CDblNull(object value, double defaultValue)
        {
            return CTypeNull(value, defaultValue, Convert.ToDouble);
        }

        /// <summary>
        /// Converts the value to a <see cref="Guid"/>, or <see cref="Guid.Empty"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value as a <see cref="System.Guid"/>, or <see cref="Guid.Empty"/> if null.</returns>
        public static Guid CTypeGuidNull(object value)
        {
            return CTypeGuidNull(value, Guid.Empty);
        }

        /// <summary>
        /// Converts the value to a <see cref="Guid"/> or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if <see cref="value"/> is null.</param>
        /// <returns>The value as a <see cref="Guid"/> or <see cref="defaultValue"/> if <see cref="value"/> is null.</returns>
        public static Guid CTypeGuidNull(object value, Guid defaultValue)
        {
            return CTypeNull(value, defaultValue, v => (Guid)v);
        }

        /// <summary>
        /// Converts the value as the specified type, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <typeparam name="T">The type to which the <see cref="value"/> will be converted.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if <see cref="value"/> is null.</param>
        /// <returns>The value as the specified type, or <see cref="defaultValue"/> if null.</returns>
        public static T CTypeNull<T>(object value, T defaultValue)
        {
            return CTypeNull(value, defaultValue, v => (T)v);
        }

        /// <summary>
        /// Converts the value to specified type using <see cref="converter"/>, or <see cref="defaultValue"/> if null.
        /// </summary>
        /// <typeparam name="T">The type to which the <see cref="value"/> will be converted.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The value to return if <see cref="value"/> is null.</param>
        /// <param name="converter">The method that converts the <see cref="value"/> to the destination type.</param>
        /// <returns>The value as the specified type, or <see cref="defaultValue"/> if null.</returns>
        public static T CTypeNull<T>(object value, T defaultValue, Converter<object, T> converter)
        {
            if (value != null && !Convert.IsDBNull(value))
            {
                return converter(value);
            }
            return defaultValue;
        }

        #endregion

        #region Property Initializer helpers

        // Identity

        public static string InitializeIdentityT1(string identity)
        {

            // id column for pricing
            // with t1 table alias used for joins
            // t1.ColumnId

            // call idenitity base
            return InitializeIdentityBase("t1.", identity);

        }

        public static string InitializeIdentityParameter(string identity)
        {

            // id column for pricing
            // with @ for passing parameters
            // @ColumnId

            // call idenitity base
            return InitializeIdentityBase("@", identity);

        }

        private static string InitializeIdentityBase(string preFix, string identity)
        {

            // build a string that contains identity with a prefix

            // check if valid identity 
            if (string.IsNullOrEmpty(identity))
            {
                return string.Empty;
            }

            // make valid prefix if nothing
            if (string.IsNullOrEmpty(preFix))
            {
                preFix = string.Empty;
            }

            // return identity with prefix
            return preFix + identity;

        }

        // Columns
        public static string InitializeColumnParameter(string columnName)
        {
            //this is identical to InitializeIdentityParameter, but added the overload for clarity when used
            return InitializeIdentityParameter(columnName);
        }

        public static string InitializeColumns(IEnumerable<string> columns)
        {

            // build a string that contains all columns passed separated by a comma
            // ColumnName1, ColumnName2

            // call columns string builder base
            return InitializeColumnsBase(columns, string.Empty, ", ");

        }

        public static string InitializeColumnsT1(IEnumerable<string> columns)
        {

            // build a string that contains all columns passed pefixed with a t1. separated by a comma
            // t1.ColumnName1, t1.ColumnName2

            // call columns string builder base
            return InitializeColumnsBase(columns, "t1.", ", ");

        }

        public static string InitializeColumnParameters(IEnumerable<string> columns)
        {

            // build a string that contains all columns passed pefixed with an @ separated by a comma
            // @ColumnName1, @ColumnName2

            // call columns string builder base
            return InitializeColumnsBase(columns, "@", ", ");

        }

        public static string InitializeColumnsBase(IEnumerable<string> columns, string prefix, string delimiter)
        {
            if (columns == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = string.Empty;
            }
            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = ", ";
            }
            string[] columnList = columns
                .Where(c => !string.IsNullOrEmpty(c))
                .Select(c => string.Concat(prefix, c))
                .ToArray();
            return string.Join(delimiter, columnList);
        }

        public static string InitializeColumnsUpdate(IEnumerable<string> columns)
        {
            if (columns == null)
            {
                return string.Empty;
            }
            const string delimiter = ", ";
            string[] columnList = columns
                .Where(c => !string.IsNullOrEmpty(c))
                .Select(c => string.Concat(c, " = @", c))
                .ToArray();
            return string.Join(delimiter, columnList);
        }

        public static string InitializeWhereColumns(IEnumerable<string> columns)
        {
            if (columns == null)
            {
                return string.Empty;
            }
            const string delimiter = " AND ";
            string[] columnList = columns
                .Where(c => !string.IsNullOrEmpty(c))
                .Select(c => string.Concat(c, " = @", c))
                .ToArray();
            return string.Join(delimiter, columnList);
        }

        public static string InitializeWhereColumn(string column)
        {
            if (string.IsNullOrEmpty(column))
            {
                return string.Empty;
            }
            return column + " = @" + column;
        }

        public static string InitializeColumnUpdate(string column)
        {
            return InitializeWhereColumn(column);
        }

        public static string CombineColumnStrings(string columns1, string columns2)
        {
            const string delimiter = ", ";
            if (string.IsNullOrEmpty(columns1))
            {
                return columns2;
            }
            if (string.IsNullOrEmpty(columns2))
            {
                return columns1;
            }
            return columns1 + delimiter + columns2;
        }

        #endregion

        #region DataTable/Column/Row Helpers


        public static DataColumn DefineColumn(string name, string dataType)
        {

            // set increment type to false
            // not an identity field
            return DefineColumn(name, dataType, false, -1);

        }

        public static DataColumn DefineColumn(string name, string dataType, bool increment, int seed)
        {

            // TODO: Verify Data Type valid

            DataColumn newColumn = new DataColumn();
            newColumn.DataType = System.Type.GetType(dataType);
            newColumn.ColumnName = name;
            newColumn.AutoIncrement = increment;
            if (newColumn.AutoIncrement)
            {
                // if auto increment (identity or counter)
                // set the starting or seed number
                newColumn.AutoIncrementSeed = seed;
            }
            newColumn.Caption = name;
            newColumn.ReadOnly = false;
            newColumn.Unique = false;

            return newColumn;

        }

        #endregion

        #region Connection related methods

        public static IDbConnection CreateConnection(DataProviderTypes dataProviderType, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            IDbConnection connection;

            switch (dataProviderType)
            {
                case DataProviderTypes.SqlServerDataProvider:
                    connection = new SqlConnection(connectionString);
                    break;
                //case DataProviderTypes.SQLiteDataProvider:
                //    connection = new SQLiteConnection(connectionString);
                //    break;
                default:
                    throw new NotImplementedException();
            }

            return connection;
        }

        public static bool CheckConnection(DataProviderTypes dataProviderType, string connectionString)
        {
            bool success = true;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            IDbConnection connection = null;

            try
            {
                connection = OpenConnection(dataProviderType, connectionString);
            }
            catch (Exception ex)
            {
                success = false;
            }

            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }

            return success;
        }

        public static IDbConnection OpenConnection(DataProviderTypes dataProviderType, string connectionString)
        {

            // create new Connection
            IDbConnection connection = CreateConnection(dataProviderType, AdjustConnectionTimeout(connectionString));

            try
            {
                OpenConnection(connection);
            }
            catch (DbException e)
            {
                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
                throw;
            }

            return connection;
        }

        public static void OpenConnection(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (connection.State != ConnectionState.Closed)
            {
                return;
            }

            connection
                .Retry()
                .LogOnRetry("DbHelper.OpenConnection")
                .Open();
        }

        private static string AdjustConnectionTimeout(string connectionString)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            builder.ConnectTimeout = 60;
            return builder.ConnectionString;
        }

        #endregion

        #region Command related methods

        public static IDbCommand CreateCommand(DataProviderTypes dataProviderType)
        {
            IDbCommand command;

            switch (dataProviderType)
            {
                case DataProviderTypes.SqlServerDataProvider:
                    command = new SqlCommand();
                    break;
                //case DataProviderTypes.SQLiteDataProvider:
                //    command = new SQLiteCommand();
                //    break;
                default:
                    throw new NotImplementedException();
            }

            return command;
        }

        public static void SetCommandType(IDbCommand command, CommandType commandType, string commandText)
        {

            // set the sql command type
            command.CommandType = commandType;

            // set the stored procedure or sql query text 
            command.CommandText = commandText;

        }

        #endregion

        #region Parameter related methods

        public static string BuildParameterList(string parameterName, int count)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                return string.Empty;
            }
            if (!parameterName.StartsWith("@"))
            {
                parameterName.Insert(0, "@");
            }
            const string delimiter = ", ";
            string[] parameterList = Enumerable
                .Range(1, count)
                .Select(index => string.Format("{0}{1}", parameterName, index))
                .ToArray();
            return string.Join(delimiter, parameterList);
        }

        public static void AddParameterToCommand(IDbCommand command, string parameterName, object value)
        {

            // add parameters to sql command

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (parameterName == string.Empty)
            {
                throw new ArgumentOutOfRangeException("parameterName");
            }

            IDbDataParameter newParameter = command.CreateParameter();
            newParameter.ParameterName = parameterName;

            if (value == null)
            {
                newParameter.Value = DBNull.Value;
            }
            else
            {
                newParameter.Value = value;
            }

            // add parameter
            command.Parameters.Add(newParameter);

        }

        public static void AddParameterToCommand(IDbCommand command, string parameterName, DbType dataType, int size, ParameterDirection direction, object value)
        {

            // add parameters to sql command

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (parameterName == string.Empty)
            {
                throw new ArgumentOutOfRangeException("parameterName");
            }

            IDbDataParameter newParameter = command.CreateParameter();
            newParameter.ParameterName = parameterName;
            newParameter.DbType = dataType;
            newParameter.Direction = direction;

            if (size > 0)
            {
                newParameter.Size = size;
            }

            if (value == null)
            {
                newParameter.Value = DBNull.Value;
            }
            else
            {
                newParameter.Value = value;
            }

            // add parameter
            command.Parameters.Add(newParameter);

        }

        public static void AddParameterToCommand(IDbCommand command, string parameterName, object value, object emptyStringValue)
        {
            // add parameters to sql command
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (parameterName == string.Empty)
            {
                throw new ArgumentOutOfRangeException("parameterName");
            }

            IDbDataParameter newParameter = command.CreateParameter();
            newParameter.ParameterName = parameterName;

            if (value == null || value.ToString().Length <= 0)
            {
                newParameter.Value = emptyStringValue;
            }
            else
            {
                newParameter.Value = value;
            }

            // add parameter
            command.Parameters.Add(newParameter);
        }

        public static void AddParameterListToCommand<T>(IDbCommand command, string parameterName, IEnumerable<T> values)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }
            if (!parameterName.StartsWith("@"))
            {
                parameterName.Insert(0, "@");
            }

            int index = 0;
            foreach (T item in values)
            {
                IDbDataParameter newParameter = command.CreateParameter();
                newParameter.ParameterName = string.Format("{0}{1}", parameterName, index + 1);
                if (item == null)
                {
                    newParameter.Value = DBNull.Value;
                }
                else
                {
                    newParameter.Value = item;
                }
                command.Parameters.Add(newParameter);
                index++;
            }
        }
        #endregion

        #region Execute methods

        /// <summary>
        /// The ExecuteNonQuery method will execute an IDbCommand and return the number of rows affected
        /// This is most commonly used for updates and does not include connection retry logic for cases of database failover
        /// </summary>
        /// <param name="dataProviderType"></param>
        /// <param name="commandText">Command to execute</param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DataProviderTypes dataProviderType, string commandText, string connectionString)
        {

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentOutOfRangeException("commandText");
            }

            // setup new SQL Command
            using (IDbCommand command = CreateCommand(dataProviderType))
            {
                SetCommandType(command, CommandType.Text, commandText);

                return ExecuteNonQuery(dataProviderType, command, connectionString);
            }
        }

        /// <summary>
        /// The ExecuteNonQuery method will execute an IDbCommand and return the number of rows affected
        /// This is most commonly used for updates and does not include connection retry logic for cases of database failover
        /// </summary>
        /// <param name="dataProviderType"></param>
        /// <param name="command"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DataProviderTypes dataProviderType, IDbCommand command, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("connectionString");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            // create new SQL Connection or get matching connection from pool
            using (IDbConnection connection = OpenConnection(dataProviderType, connectionString))
            {
                int returnValue = 0;

                try
                {
                    // execute non query command
                    returnValue = ExecuteNonQuery(command, connection);
                }
                finally
                {
                    connection.Close();
                }

                return returnValue;
            }
        }

        /// <summary>
        /// The ExecuteNonQuery method will execute an IDbCommand and return the number of rows affected
        /// This is most commonly used for updates and does not include connection retry logic for cases of database failover
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IDbCommand command, IDbConnection connection)
        {

            // returns the number of rows affected
            // by the non query command

            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            // HACK:
            if (command.CommandTimeout < 600)
            {
                command.CommandTimeout = 600;
            }

            // set command to use created connection
            command.Connection = connection;

            //make sure the connection is open
            OpenConnection(connection);

            return command
                .Retry()
                .LogOnRetry("DbHelper.ExecuteNonQuery")
                .ExecuteNonQuery();

        }

        /// <summary>
        /// The ExecuteNonQuery method will execute an IDbCommand and return the number of rows affected
        /// This is most commonly used for updates and does not include connection retry logic for cases of database failover
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IDbCommand command, IDbTransaction transaction)
        {

            // returns the number of rows affected
            // by the non query command

            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            command.Transaction = transaction;

            return ExecuteNonQuery(command, transaction.Connection);
        }

        /// <summary>
        /// The ExecuteScalar method will execute an IDbCommand and return the first column of the first row that results
        /// This is most commonly used for inserts and does not include connection retry logic for cases of database failover
        /// If used for retrieving/selecting data, the ExecuteScalarReader method is preferrable due to the retry logic within
        /// </summary>
        /// <param name="dataProviderType"></param>
        /// <param name="commandText"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static object ExecuteScalar(DataProviderTypes dataProviderType, string commandText, string connectionString)
        {

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentOutOfRangeException("commandText");
            }

            // setup new SQL Command
            using (IDbCommand command = CreateCommand(dataProviderType))
            {
                SetCommandType(command, CommandType.Text, commandText);

                return ExecuteScalar(dataProviderType, command, connectionString);
            }
        }

        /// <summary>
        /// The ExecuteScalar method will execute an IDbCommand and return the first column of the first row that results
        /// This is most commonly used for inserts and does not include connection retry logic for cases of database failover
        /// If used for retrieving/selecting data, the ExecuteScalarReader method is preferrable due to the retry logic within
        /// </summary>
        /// <param name="dataProviderType"></param>
        /// <param name="command"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static object ExecuteScalar(DataProviderTypes dataProviderType, IDbCommand command, string connectionString)
        {
            // returns the first object returned by the scaler command
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("connectionString");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            // create new Connection or get matching connection from pool
            using (IDbConnection connection = OpenConnection(dataProviderType, connectionString))
            {
                object returnValue = null;

                try
                {
                    // execute non query command
                    returnValue = ExecuteScalar(command, connection);
                }
                finally
                {
                    connection.Close();
                }

                return returnValue;
            }
        }

        /// <summary>
        /// The ExecuteScalar method will execute an IDbCommand and return the first column of the first row that results
        /// This is most commonly used for inserts and does not include connection retry logic for cases of database failover
        /// If used for retrieving/selecting data, the ExecuteScalarReader method is preferrable due to the retry logic within
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static object ExecuteScalar(IDbCommand command, IDbConnection connection)
        {

            // returns the first object returned by the scaler command
            // normally used for inserts and it will return the indentity

            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            // HACK:
            if (command.CommandTimeout < 600)
            {
                command.CommandTimeout = 600;
            }

            // set command to use created connection
            command.Connection = connection;

            //make sure the connection is open
            OpenConnection(connection);

            return command
                .Retry()
                .LogOnRetry("DbHelper.ExecuteScalar")
                .ExecuteScalar();
        }


        /// <summary>
        /// The ExecuteScalarReader is intended when using the ExecuteScalar command for retrieving data
        /// Rather than inserts or updates because it includes connection retry logic for cases of database failover
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static object ExecuteScalarReader(DataProviderTypes dataProviderType, IDbCommand command, string connectionString)
        {
            // returns the first object returned by the scaler command
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("connectionString");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            // create new Connection or get matching connection from pool
            using (IDbConnection connection = OpenConnection(dataProviderType, connectionString))
            {
                try
                {
                    // execute sql command
                    return ExecuteScalarReader(command, connection);
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        /// <summary>
        /// The ExecuteScalarReader is intended when using the ExecuteScalar command for retrieving data
        /// Rather than inserts or updates because it includes connection retry logic for cases of database failover
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static object ExecuteScalarReader(IDbCommand command, IDbConnection connection)
        {

            // returns the first object returned by the scaler command
            // normally used for inserts and it will return the indentity

            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            // HACK:
            if (command.CommandTimeout < 600)
            {
                command.CommandTimeout = 600;
            }

            // set command to use created connection
            command.Connection = connection;

            ExecuteReaderRetryer retryer = new ExecuteReaderRetryer();
            return retryer.Retry(
                () =>
                {
                    //make sure the connection is open
                    OpenConnection(connection);

                    return command
                        .Retry()
                        .LogOnRetry("DbHelper.ExecuteScalar")
                        .ExecuteScalar();
                });
        }

        /// <summary>
        /// The ExecuteScalar method will execute an IDbCommand and return the first column of the first row that results
        /// This is most commonly used for inserts and does not include connection retry logic for cases of database failover
        /// If used for retrieving/selecting data, the ExecuteScalarReader method is preferrable due to the retry logic within
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static object ExecuteScalar(IDbCommand command, IDbTransaction transaction)
        {

            // returns the first object returned by the scaler command
            // normally used for inserts and it will return the indentity

            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            command.Transaction = transaction;

            return ExecuteScalar(command, transaction.Connection);
        }

        public static void TExecuteReader<T>(DataProviderTypes dataProviderType, IDbCommand command, string connectionString, TCreateListFromReader<T> delegateAddress, ref List<T> list)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("connectionString");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            // HACK:
            if (command.CommandTimeout < 600)
            {
                command.CommandTimeout = 600;
            }

            if (list == null)
            {
                list = new List<T>();
            }
            ExecuteReaderRetryer retryer = new ExecuteReaderRetryer();
            list.AddRange(retryer.Retry(
                () =>
                {

                    // create new SQL Connection or get matching connection from pool
                    using (IDbConnection connection = OpenConnection(dataProviderType, connectionString))
                    {

                        // set command to use created connection
                        command.Connection = connection;

                        List<T> results = new List<T>();

                        using (IDataReader reader = command.Retry().LogOnRetry("DbHelper.TExecuteReader").ExecuteReader())
                        {
                            // build list through delegate
                            delegateAddress.Invoke(reader, ref results);
                            reader.Close();
                        }
                        connection.Close();
                        return results;
                    }
                }
            ));
        }

        public static void TExecuteReader<T>(IDbCommand command, IDbTransaction transaction, TCreateListFromReader<T> delegateAddress, ref List<T> list)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }


            // HACK:
            if (command.CommandTimeout < 600)
            {
                command.CommandTimeout = 600;
            }

            // set command to use created connection
            command.Transaction = transaction;
            command.Connection = transaction.Connection;

            ExecuteReaderRetryer retryer = new ExecuteReaderRetryer();
            list = retryer.Retry(
                () =>
                {

                    //make sure the connection is open
                    OpenConnection(transaction.Connection);

                    List<T> results = new List<T>();
                    using (IDataReader reader = command.Retry().LogOnRetry("DbHelper.TExecuteReader").ExecuteReader())
                    {
                        // build list through delegate
                        delegateAddress.Invoke(reader, ref results);
                        reader.Close();
                    }
                    return results;
                }
            );

        }

        public static void TExecuteReaderWithLookUp<T1, T2, T3>(DataProviderTypes dataProviderType, IDbCommand command, string connectionString, TCreateListFromReaderWithLookUp<T1, T2, T3> delegateAddress, ref IList<T1> list, out IDictionary<T2, T3> newLookUp)
        {

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("connectionString");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            // HACK:
            if (command.CommandTimeout < 600)
            {
                command.CommandTimeout = 600;
            }

            newLookUp = null;

            // create new SQL Connection or get matching connection from pool
            using (IDbConnection connection = OpenConnection(dataProviderType, connectionString))
            {

                // set command to use created connection
                command.Connection = connection;

                list = new List<T1>();
                // execute command and create reader
                using (IDataReader reader = command.Retry().LogOnRetry("DbHelper.TExecuteReaderWithLookup").ExecuteReader())
                {
                    // build list through delegate
                    delegateAddress.Invoke(reader, ref list, out newLookUp);
                    reader.Close();
                }

                connection.Close();
            }

        }

        public static void TCreateSimpleListFromReader<T>(IDataReader reader, ref List<T> newList)
        {
            // simple list builder of type T
            while (reader.Read())
            {

                T temp = default(T);

                try
                {
                    //need to handle strings in a special way
                    if (typeof(T) == typeof(string))
                    {
                        temp = (T)(object)CStrNull(reader[0]);
                    }
                    else
                    {
                        temp = CTypeNull<T>(reader[0], default(T));
                    }
                }
                catch (Exception ex)
                {
                    temp = default(T);
                    //temp = String.Empty
                }

                // add returned row to list
                newList.Add(temp);

            }

        }

        #endregion

        #region IdSequence table helpers

        public static int GetMaxId(DataProviderTypes dataProviderType, string connectionString, IdSequenceTableInfo idSequenceTableInfo, string key, int count)
        {
            if (idSequenceTableInfo == null)
            {
                throw new ArgumentNullException("idSequenceTableInfo");
            }
            return GetMaxId(dataProviderType, connectionString, idSequenceTableInfo.TableName, idSequenceTableInfo.KeyColumnName, idSequenceTableInfo.IdColumnName, key, count);
        }

        public static int GetMaxId(DataProviderTypes dataProviderType, IDbTransaction transaction, IdSequenceTableInfo idSequenceTableInfo, string key, int count)
        {
            if (idSequenceTableInfo == null)
            {
                throw new ArgumentNullException("idSequenceTableInfo");
            }
            return GetMaxId(dataProviderType, transaction, idSequenceTableInfo.TableName, idSequenceTableInfo.KeyColumnName, idSequenceTableInfo.IdColumnName, key, count);
        }

        public static int GetMaxId(DataProviderTypes dataProviderType, string connectionString, string table, string keyColumn, string idColumn, string key, int count)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("SQLite connection string cannot be empty.");
            }

            int returnId = 0;

            using (IDbConnection connection = OpenConnection(dataProviderType, connectionString))
            {

                IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    returnId = GetMaxId(dataProviderType, transaction, table, keyColumn, idColumn, key, count);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return returnId;
        }

        public static int GetMaxId(DataProviderTypes dataProviderType, IDbTransaction transaction, string table, string keyColumn, string idColumn, string key, int count)
        {
            if (string.IsNullOrEmpty(table))
            {
                throw new ArgumentOutOfRangeException("table");
            }
            if (string.IsNullOrEmpty(keyColumn))
            {
                throw new ArgumentOutOfRangeException("keyColumn");
            }
            if (string.IsNullOrEmpty(idColumn))
            {
                throw new ArgumentOutOfRangeException("idColumn");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            int returnId = 0;

            using (IDbCommand command = CreateCommand(dataProviderType))
            {
                //get the current max Id
                string sqlText = String.Format("SELECT {0} FROM {1} WITH (ROWLOCK,XLOCK) WHERE {2} = @KeyName", idColumn, table, keyColumn);
                SetCommandType(command, CommandType.Text, sqlText);

                AddParameterToCommand(command, "@KeyName", key);

                //if the row was not found in the table, then returnVal will be DBNull, and will get cast to 0, which is what we want
                object returnVal = ExecuteScalar(command, transaction);
                returnId = DbHelper.CIntNull(returnVal);

                if (count > 0)
                {
                    //update the max id
                    sqlText = String.Format("UPDATE {0} SET {1} = ({1} + @Count) WHERE {2} = @KeyName", table, idColumn, keyColumn);
                    SetCommandType(command, CommandType.Text, sqlText);

                    //this parameter is already on the command...leaving this line here so we remember
                    //AddParametersToCommand(command, "@KeyName", key);

                    AddParameterToCommand(command, "@Count", count);

                    int rowCount = ExecuteNonQuery(command, transaction);

                    //if there were no rows updated, then that means this row wasn't in the table and needs to be added with the correct max id
                    if (rowCount < 1)
                    {
                        //make sure the row exists in the table
                        sqlText = String.Format("INSERT INTO {1} ({2}, {0}) VALUES (@KeyName, @Count)", idColumn, table, keyColumn);
                        SetCommandType(command, CommandType.Text, sqlText);

                        //this parameter is already on the command...leaving this line here so we remember
                        //AddParametersToCommand(command, "@KeyName", key);
                        //AddParametersToCommand(command, "@Count", count);

                        ExecuteNonQuery(command, transaction);
                    }
                }
            }

            return returnId;
        }

        public static void SetMaxIdAtLeast(DataProviderTypes dataProviderType, string connectionString, IdSequenceTableInfo idSequenceTableInfo, string key, int maxId)
        {
            if (idSequenceTableInfo == null)
            {
                throw new ArgumentNullException("idSequenceTableInfo");
            }
            SetMaxIdAtLeast(dataProviderType, connectionString, idSequenceTableInfo.TableName, idSequenceTableInfo.KeyColumnName, idSequenceTableInfo.IdColumnName, key, maxId);
        }

        public static void SetMaxIdAtLeast(DataProviderTypes dataProviderType, IDbTransaction transaction, IdSequenceTableInfo idSequenceTableInfo, string key, int maxId)
        {
            if (idSequenceTableInfo == null)
            {
                throw new ArgumentNullException("idSequenceTableInfo");
            }
            SetMaxIdAtLeast(dataProviderType, transaction, idSequenceTableInfo.TableName, idSequenceTableInfo.KeyColumnName, idSequenceTableInfo.IdColumnName, key, maxId);
        }

        public static void SetMaxIdAtLeast(DataProviderTypes dataProviderType, string connectionString, string table, string keyColumn, string idColumn, string key, int maxId)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("SQLite connection string cannot be empty.");
            }

            using (IDbConnection connection = OpenConnection(dataProviderType, connectionString))
            {

                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    SetMaxIdAtLeast(dataProviderType, transaction, table, keyColumn, idColumn, key, maxId);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public static void SetMaxIdAtLeast(DataProviderTypes dataProviderType, IDbTransaction transaction, string table, string keyColumn, string idColumn, string key, int maxId)
        {
            if (string.IsNullOrEmpty(table))
            {
                throw new ArgumentOutOfRangeException("table");
            }
            if (string.IsNullOrEmpty(keyColumn))
            {
                throw new ArgumentOutOfRangeException("keyColumn");
            }
            if (string.IsNullOrEmpty(idColumn))
            {
                throw new ArgumentOutOfRangeException("idColumn");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }

            using (IDbCommand command = CreateCommand(dataProviderType))
            {
                //get the current max Id
                string sqlText = String.Format("SELECT {0} FROM {1} WHERE {2} = @KeyName", idColumn, table, keyColumn);
                SetCommandType(command, CommandType.Text, sqlText);

                AddParameterToCommand(command, "@KeyName", key);

                //if the row was not found in the table, then returnVal will be DBNull, and will get cast to 0, which is what we want
                object returnVal = ExecuteScalar(command, transaction);
                int returnId = DbHelper.CIntNull(returnVal);

                if (maxId > returnId)
                {
                    //update the max id
                    sqlText = String.Format("UPDATE {0} SET {1} = @MaxId WHERE {2} = @KeyName", table, idColumn, keyColumn);
                    SetCommandType(command, CommandType.Text, sqlText);

                    //this parameter is already on the command...leaving this line here so we remember
                    //AddParametersToCommand(command, "@KeyName", key);

                    AddParameterToCommand(command, "@MaxId", maxId);

                    int rowCount = ExecuteNonQuery(command, transaction);

                    //if there were no rows updated, then that means this row wasn't in the table and needs to be added with the correct max id
                    if (rowCount < 1)
                    {
                        //make sure the row exists in the table
                        sqlText = String.Format("INSERT INTO {1} ({2}, {0}) VALUES (@KeyName, @MaxId)", idColumn, table, keyColumn);
                        SetCommandType(command, CommandType.Text, sqlText);

                        //this parameter is already on the command...leaving this line here so we remember
                        //AddParametersToCommand(command, "@KeyName", key);
                        //AddParametersToCommand(command, "@MaxId", maxId);

                        ExecuteNonQuery(command, transaction);
                    }
                }
            }
        }

        #endregion

        #region Script helpers

        public static void RunScript(DataProviderTypes dataProviderType, string connectionString, string schemaScript)
        {
            //if the script is empty, there is nothing to do
            if (string.IsNullOrEmpty(schemaScript))
            {
                return;
            }
            //check this again, just in case
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentOutOfRangeException("connectionString");
            }

            using (IDbConnection connection = OpenConnection(dataProviderType, connectionString))
            {
                //need to split the schema script up into individual statements
                List<string> commands = GetIndividualSqlCommands(schemaScript);

                //create the command
                IDbCommand command = CreateCommand(dataProviderType);

                //run all the commands
                foreach (string commandText in commands)
                {
                    SetCommandType(command, CommandType.Text, commandText);
                    ExecuteNonQuery(command, connection);
                }

                //close the connection
                connection.Close();
            }
        }

        //this method was copied over from the Admin tool
        private static List<string> GetIndividualSqlCommands(string sqlTextAll)
        {
            //break sql queries into individual sql commands
            //cannot just use split on 'go' since have fields and commands with 'go' in them like category

            char[] delimiters = { '\r', '\f' }; // New String() {vbCr, vbLf};
            string[] sqlLines = sqlTextAll.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            List<string> sqlTexts = null;

            if (sqlLines != null && sqlLines.Count() > 0)
            {
                //build individual sql commands
                sqlTexts = new List<string>
                {
                }; //probably not right

                StringBuilder sqlTextBuilder = new StringBuilder();

                foreach (string sqlLine in sqlLines)
                {
                    // check if command separation
                    if (sqlLine.Trim().ToLower() == "go")
                    {
                        // new line
                        if (sqlTextBuilder.Length > 0)
                        {
                            // add to list of sql commands
                            sqlTexts.Add(sqlTextBuilder.ToString());
                        }

                        // reset sqlTextBuilder
                        sqlTextBuilder.Length = 0;

                    }
                    else
                    {
                        // add line to latest command 
                        sqlTextBuilder.AppendLine(sqlLine);
                    }
                }
            }

            return sqlTexts;
        }

        #endregion

        #region Miscellaneous other helpers

        public static void NeedsSingleQuote(ref string stringParameter)
        {

            // helper routine for single string items passed
            // without single quotes around the parameter
            if (!(string.IsNullOrEmpty(stringParameter)))
            {
                if (!(stringParameter.StartsWith("'")))
                {
                    stringParameter = "'" + stringParameter;
                }
                if (!(stringParameter.EndsWith("'")))
                {
                    stringParameter += "'";
                }
            }

        }


        /// <summary>
        /// Takes a list of type T and joins them using joiner.
        /// </summary>
        /// <typeparam name="T">The type of the List being passed in.</typeparam>
        /// <param name="list">The list containing items to join.</param>
        /// <param name="joiner">The string to use between items.</param>
        /// <param name="averageItemLength">The average length of the items in the List. This is used to create the initial size of the StringBuilder.</param>
        /// <returns></returns>
        public static string JoinList<T>(List<T> list, string joiner, int averageItemLength)
        {
            if (list.Count < 1)
            {
                return String.Empty;
            }

            System.Text.StringBuilder joinedString = new System.Text.StringBuilder((joiner.Length + averageItemLength) * list.Count);

            for (int i = 0; i < list.Count - 1; i++)
            {
                joinedString.Append(list[i]);
                joinedString.Append(joiner);
            }
            joinedString.Append(list.Last());

            return joinedString.ToString();
        }

        #endregion
    }

}
