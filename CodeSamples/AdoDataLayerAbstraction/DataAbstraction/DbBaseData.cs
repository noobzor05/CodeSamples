using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoDataLayerAbstraction
{
    public class DbBaseData
    {
        #region Protected fields

        protected DataContext _DataContext;

        protected string _Table;

        //This assumes that there will be no more than 1 identity column, and that if there is, it will always be part of the primary key
        protected string _Identity;

        //This is all the columns that are part of the PK, but are not an Identity
        protected List<string> _NonIdentityPrimaryKeys;

        //This is all of the non-PK columns
        protected List<string> _Columns;

        //These are the columns to order by, assuming ascending sort
        protected List<string> _Orders;

        //these are used for storing strings used in queries
        protected string _ClauseWhereIdentity;
        protected string _ClauseWherePrimaryKeys;
        protected string _ClauseOrderByDefault;

        protected string _QuerySelect;
        protected string _QuerySelectTop1;
        protected string _QuerySelectById;
        protected string _QuerySelectByPrimaryKeys;
        protected string _QueryUpdate;
        protected string _QueryInsert;
        protected string _QueryDelete;

        #endregion

        #region Private fields

        private string _NonIdentityPrimaryKeysString;
        private string _ColumnsString;
        private string _OrdersString;
        private string _PrimaryKeysString;
        private string _NonIdentityColumnsString;
        private string _FullColumnsString;

        private string _IdentityT1;
        private string _NonIdentityPrimaryKeysT1;
        private string _PrimaryKeysT1;
        private string _ColumnsT1;
        private string _NonIdentityColumnsT1;
        private string _FullColumnsT1;
        private string _OrdersT1;

        private string _IdentityParameter;
        private string _NonIdentityPrimaryKeyParameters;
        private string _PrimaryKeyParameters;
        private string _ColumnParameters;
        private string _NonIdentityColumnParameters;
        private string _FullColumnParameters;

        private string _ColumnsUpdate;
        private string _IdentityWhere;
        private string _PrimaryKeysWhere;

        #endregion



        #region Constructors

        private DbBaseData()
        {
            //must use the constructor that takes a data context
        }

        protected DbBaseData(DataContext dataContext)
        {
            if (dataContext == null)
            {
                throw new ArgumentNullException("dataContext");
            }
            _DataContext = dataContext;
        }

        #endregion

        #region Database Properties

        public DataContext DataContext
        {
            get
            {
                return _DataContext;
            }
        }

        public DataProviderTypes DataProviderType
        {
            get
            {
                if (this._DataContext == null)
                {
                    return DataProviderTypes.NotSet;
                }
                else
                {
                    return this._DataContext.DataProviderType;
                }
            }
        }

        public virtual string ConnectionString
        {
            get
            {
                if (this._DataContext == null || string.IsNullOrEmpty(this._DataContext.ConnectionString))
                {
                    return string.Empty;
                }
                else
                {
                    return this._DataContext.ConnectionString;
                }
            }
        }

        #endregion

        #region Table and Column properties

        public string Table
        {
            get
            {
                return _Table;
            }
        }

        public string Identity
        {
            get
            {
                return _Identity;
            }
        }

        public string NonIdentityPrimaryKeys
        {
            get
            {
                if (_NonIdentityPrimaryKeysString == null)
                {
                    _NonIdentityPrimaryKeysString = DbHelper.InitializeColumns(_NonIdentityPrimaryKeys);
                }
                return _NonIdentityPrimaryKeysString;
            }
        }

        public string PrimaryKeys
        {
            get
            {
                if (_PrimaryKeysString == null)
                {
                    _PrimaryKeysString = DbHelper.CombineColumnStrings(Identity, NonIdentityPrimaryKeys);
                }
                return _PrimaryKeysString;
            }
        }

        public string Columns
        {
            get
            {
                if (_ColumnsString == null)
                {
                    _ColumnsString = DbHelper.InitializeColumns(_Columns);
                }
                return _ColumnsString;
            }
        }

        public string NonIdentityColumns
        {
            get
            {
                if (_NonIdentityColumnsString == null)
                {
                    _NonIdentityColumnsString = DbHelper.CombineColumnStrings(NonIdentityPrimaryKeys, Columns);
                }
                return _NonIdentityColumnsString;
            }
        }

        public string FullColumns
        {
            get
            {
                if (_FullColumnsString == null)
                {
                    _FullColumnsString = DbHelper.CombineColumnStrings(Identity, NonIdentityColumns);
                }
                return _FullColumnsString;
            }
        }

        public string Orders
        {
            get
            {
                if (_OrdersString == null)
                {
                    _OrdersString = DbHelper.InitializeColumns(_Orders);
                }
                return _OrdersString;
            }
        }

        public string IdentityT1
        {
            get
            {
                if (_IdentityT1 == null)
                {
                    _IdentityT1 = DbHelper.InitializeIdentityT1(_Identity);
                }
                return _IdentityT1;
            }
        }

        public string NonIdentityPrimaryKeysT1
        {
            get
            {
                if (_NonIdentityPrimaryKeysT1 == null)
                {

                    _NonIdentityPrimaryKeysT1 = DbHelper.InitializeColumnsT1(_NonIdentityPrimaryKeys);
                }
                return _NonIdentityPrimaryKeysT1;
            }
        }

        public string PrimaryKeysT1
        {
            get
            {
                if (_PrimaryKeysT1 == null)
                {
                    _PrimaryKeysT1 = DbHelper.CombineColumnStrings(IdentityT1, NonIdentityPrimaryKeysT1);
                }
                return _PrimaryKeysT1;
            }
        }

        public string ColumnsT1
        {
            get
            {
                if (_ColumnsT1 == null)
                {
                    _ColumnsT1 = DbHelper.InitializeColumnsT1(_Columns);
                }
                return _ColumnsT1;
            }
        }

        public string NonIdentityColumnsT1
        {
            get
            {
                if (_NonIdentityColumnsT1 == null)
                {
                    _NonIdentityColumnsT1 = DbHelper.CombineColumnStrings(NonIdentityPrimaryKeysT1, ColumnsT1);
                }
                return _NonIdentityColumnsT1;
            }
        }

        public string FullColumnsT1
        {
            get
            {
                if (_FullColumnsT1 == null)
                {
                    _FullColumnsT1 = DbHelper.CombineColumnStrings(IdentityT1, NonIdentityColumnsT1);
                }
                return _FullColumnsT1;
            }
        }

        public string OrdersT1
        {
            get
            {
                if (_OrdersT1 == null)
                {
                    _OrdersT1 = DbHelper.InitializeColumnsT1(_Orders);
                }
                return _OrdersT1;
            }
        }

        public string IdentityParameter
        {
            get
            {
                if (_IdentityParameter == null)
                {
                    _IdentityParameter = DbHelper.InitializeIdentityParameter(_Identity);
                }
                return _IdentityParameter;
            }
        }

        public string NonIdentityPrimaryKeyParameters
        {
            get
            {
                if (_NonIdentityPrimaryKeyParameters == null)
                {
                    _NonIdentityPrimaryKeyParameters = DbHelper.InitializeColumnParameters(_NonIdentityPrimaryKeys);
                }
                return _NonIdentityPrimaryKeyParameters;
            }
        }

        public string PrimaryKeyParameters
        {
            get
            {
                if (_PrimaryKeyParameters == null)
                {
                    _PrimaryKeyParameters = DbHelper.CombineColumnStrings(IdentityParameter, NonIdentityPrimaryKeyParameters);
                }
                return _PrimaryKeyParameters;
            }
        }

        public string ColumnParameters
        {
            get
            {
                if (_ColumnParameters == null)
                {
                    _ColumnParameters = DbHelper.InitializeColumnParameters(_Columns);
                }
                return _ColumnParameters;
            }
        }

        public string NonIdentityColumnParameters
        {
            get
            {
                if (_NonIdentityColumnParameters == null)
                {
                    _NonIdentityColumnParameters = DbHelper.CombineColumnStrings(NonIdentityPrimaryKeyParameters, ColumnParameters);
                }
                return _NonIdentityColumnParameters;
            }
        }

        public string FullColumnParameters
        {
            get
            {
                if (_FullColumnParameters == null)
                {
                    _FullColumnParameters = DbHelper.CombineColumnStrings(IdentityParameter, NonIdentityColumnParameters);
                }
                return _FullColumnParameters;
            }
        }

        public string ColumnsUpdate
        {
            get
            {
                if (_ColumnsUpdate == null)
                {
                    _ColumnsUpdate = DbHelper.InitializeColumnsUpdate(_Columns);
                }
                return _ColumnsUpdate;
            }
        }

        public string IdentityWhere
        {
            get
            {
                if (_IdentityWhere == null)
                {
                    List<string> identity = new List<string>();
                    if (!string.IsNullOrEmpty(_Identity))
                    {
                        identity.Add(_Identity);
                    }
                    _IdentityWhere = DbHelper.InitializeWhereColumns(identity);
                }
                return _IdentityWhere;
            }
        }

        public string PrimaryKeysWhere
        {
            get
            {
                if (_PrimaryKeysWhere == null)
                {
                    List<string> pkColumns = new List<string>();
                    if (!string.IsNullOrEmpty(_Identity))
                    {
                        pkColumns.Add(_Identity);
                    }
                    if (_NonIdentityPrimaryKeys != null && _NonIdentityPrimaryKeys.Count > 0)
                    {
                        pkColumns.AddRange(_NonIdentityPrimaryKeys);
                    }
                    _PrimaryKeysWhere = DbHelper.InitializeWhereColumns(pkColumns);
                }
                return _PrimaryKeysWhere;
            }
        }

        #endregion

        #region Keyword Properties

        protected virtual string KeywordSelect
        {
            get
            {
                return " SELECT ";
            }
        }

        protected virtual string KeywordFrom
        {
            get
            {
                return " FROM ";
            }
        }

        protected virtual string KeywordWhere
        {
            get
            {
                return " WHERE ";
            }
        }

        protected virtual string KeywordOrderBy
        {
            get
            {
                return " ORDER BY ";
            }
        }

        protected virtual string KeywordInsert
        {
            get
            {
                return " INSERT INTO ";
            }
        }

        protected virtual string KeywordValues
        {
            get
            {
                return " VALUES ";
            }
        }

        protected virtual string KeywordUpdate
        {
            get
            {
                return " UPDATE ";
            }
        }

        protected virtual string KeywordSet
        {
            get
            {
                return " SET ";
            }
        }

        protected virtual string KeywordDelete
        {
            get
            {
                return " DELETE ";
            }
        }

        protected virtual string KeywordJoin
        {
            get
            {
                return " JOIN ";
            }
        }

        protected virtual string KeywordLeftJoin
        {
            get
            {
                return " LEFT OUTER JOIN ";
            }
        }

        protected virtual string KeywordRightJoin
        {
            get
            {
                return " RIGHT OUTER JOIN ";
            }
        }

        protected virtual string KeywordTop
        {
            get
            {
                return " TOP ";
            }
        }

        protected virtual string KeywordGroupBy
        {
            get
            {
                return " GROUP BY ";
            }
        }

        protected virtual string KeywordAscending
        {
            get
            {
                return " ASC ";
            }
        }

        protected virtual string KeywordDescending
        {
            get
            {
                return " DESC ";
            }
        }

        protected virtual string KeywordDistinct
        {
            get
            {
                return " DISTINCT ";
            }
        }

        protected virtual string HintNoLock
        {
            get
            {
                return " WITH (NOLOCK) ";
            }
        }

        #endregion

        #region Clause properties

        /// <summary>
        /// Gets the default where clause.  This is string.Empty by default,
        /// but if a derived class needs to include a WHERE condition on all 
        /// normal select statements, it should override this property.
        /// </summary>
        /// <example>
        /// WHERE IsDeleted = 0
        /// </example>
        protected virtual string ClauseWhereDefault
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the where clause to select by identity.
        /// </summary>
        /// <example>
        /// WHERE TableId = @TableId
        /// </example>
        protected virtual string ClauseWhereIdentity
        {
            get
            {
                if (_ClauseWhereIdentity == null)
                {
                    if (string.IsNullOrEmpty(_Identity))
                    {
                        _ClauseWhereIdentity = string.Empty;
                    }
                    else
                    {
                        _ClauseWhereIdentity = this.KeywordWhere + this.IdentityWhere;
                    }
                }
                return _ClauseWhereIdentity;
            }
        }

        /// <summary>
        /// Gets the where clause to select by all primary keys.
        /// </summary>
        /// <example>
        /// WHERE TableId = @TableId AND PkOne = @PkOne AND PkTwo = @PkTwo
        /// </example>
        protected virtual string ClauseWherePrimaryKeys
        {
            get
            {
                if (_ClauseWherePrimaryKeys == null)
                {
                    if (string.IsNullOrEmpty(this.PrimaryKeys))
                    {
                        _ClauseWherePrimaryKeys = string.Empty;
                    }
                    else
                    {
                        _ClauseWherePrimaryKeys = this.KeywordWhere + this.PrimaryKeysWhere;
                    }
                }

                return _ClauseWherePrimaryKeys;
            }
        }

        /// <summary>
        /// Gets the default order by clause.
        /// </summary>
        /// <example>
        /// ORDER BY OrderCol1, OrderCol2, OrderCol3
        /// </example>
        protected virtual string ClauseOrderByDefault
        {
            get
            {
                if (_ClauseOrderByDefault == null)
                {
                    if (this._Orders == null || this._Orders.Count < 1)
                    {
                        _ClauseOrderByDefault = string.Empty;
                    }
                    else
                    {
                        _ClauseOrderByDefault = this.KeywordOrderBy + this.Orders;
                    }
                }
                return _ClauseOrderByDefault;
            }
        }

        /// <summary>
        /// Gets the clause to select the first result of a query.
        /// </summary>
        /// <example>
        /// TOP (1)
        /// </example>
        protected virtual string ClauseTop1
        {
            get
            {
                return this.KeywordTop + " (1) ";
            }
        }
        #endregion

        #region Query Properties

        /// <summary>
        /// Gets the standard SELECT statement, without any WHERE or ORDER BY clause.  
        /// </summary>
        /// <example>
        /// SELECT FullColumns FROM Table
        /// </example>
        protected virtual string QuerySelect
        {
            get
            {
                if (_QuerySelect == null)
                {
                    _QuerySelect = this.KeywordSelect + this.FullColumns + this.KeywordFrom + this.Table + this.HintNoLock;
                }
                return _QuerySelect;
            }
        }

        /// <summary>
        /// Gets the standard SELECT statement for a single row, without any WHERE or ORDER BY clause.  
        /// </summary>
        /// <example>
        /// SELECT TOP(1) FullColumns FROM Table
        /// </example>
        protected virtual string QuerySelectTop1
        {
            get
            {
                if (_QuerySelectTop1 == null)
                {
                    _QuerySelectTop1 = this.KeywordSelect + this.ClauseTop1 + this.FullColumns + this.KeywordFrom + this.Table + this.HintNoLock;
                }
                return _QuerySelectTop1;
            }
        }

        /// <summary>
        /// Gets a SELECT statement by Id, with the default WHERE clause. 
        /// </summary>
        /// <example>
        /// SELECT FullColumns FROM Table WHERE Identity = @IdentityT1
        /// </example>
        protected virtual string QuerySelectById
        {
            get
            {
                if (_QuerySelectById == null)
                {
                    _QuerySelectById = this.QuerySelect + this.ClauseWhereIdentity;
                }
                return _QuerySelectById;
            }
        }

        /// <summary>
        /// Gets a SELECT statement by all primary keys, with the default WHERE clause.
        /// </summary>
        /// <example>
        /// SELECT FullColumns FROM Table WHERE PrimaryKey1 = @PrimaryKey1 AND PrimaryKey2 = @PrimaryKey2
        /// </example>
        protected virtual string QuerySelectByPrimaryKeys
        {
            get
            {
                if (_QuerySelectByPrimaryKeys == null)
                {
                    _QuerySelectByPrimaryKeys = this.QuerySelect + this.ClauseWherePrimaryKeys;
                }
                return _QuerySelectByPrimaryKeys;
            }
        }

        /// <summary>
        /// Gets the query select the identity of the last row inserted.
        /// </summary>
        /// <example>
        /// SELECT @@Identity;
        /// </example>
        protected virtual string QuerySelectLastIdentity
        {
            get
            {
                return this.KeywordSelect + " @@Identity;";
            }
        }

        /// <summary>
        /// Gets the normal UPDATE statement.  This updates all of the non-PK columns
        /// and includes all of the PK columns in the WHERE clause.
        /// </summary>
        protected virtual string QueryUpdate
        {
            get
            {
                if (_QueryUpdate == null)
                {
                    _QueryUpdate = this.KeywordUpdate + this.Table + this.KeywordSet + this.ColumnsUpdate + this.ClauseWherePrimaryKeys;
                }
                return _QueryUpdate;
            }
        }

        /// <summary>
        /// Gets the normal INSERT statement.  This will insert all non-identity columns.
        /// If there is an Identity column specified, the 'SELECT @@Identity' will be 
        /// included.  Otherwise, it will be omitted.
        /// </summary>
        protected virtual string QueryInsert
        {
            get
            {
                if (_QueryInsert == null)
                {
                    _QueryInsert = this.KeywordInsert + this.Table + " (" + this.NonIdentityColumns + ") VALUES (" + this.NonIdentityColumnParameters + ");" + (string.IsNullOrEmpty(Identity) ? string.Empty : this.QuerySelectLastIdentity);
                }
                return _QueryInsert;
            }
        }

        /// <summary>
        /// Gets the query delete a row by PrimaryKeys, which is always the preferred way
        /// to delete a single row.
        /// </summary>
        protected virtual string QueryDelete
        {
            get
            {
                if (_QueryDelete == null)
                {
                    _QueryDelete = this.KeywordDelete + this.Table + this.ClauseWherePrimaryKeys;
                }
                return _QueryDelete;
            }
        }

        #endregion

        #region Generic Data Access Methods

        /// <summary>
        /// Gets all objects in the table
        /// </summary>
        /// <typeparam name="T">The type of the object in the table.</typeparam>
        /// <param name="createListFomReader">The method to create the list of objects from the reader.</param>
        /// <returns>The list of all objects in the table.</returns>
        protected virtual List<T> GetAll<T>(DbHelper.TCreateListFromReader<T> createListFomReader) where T : class
        {
            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.QuerySelect + this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);
                return GetAll(command, createListFomReader);
            }
        }

        /// <summary>
        /// Gets all elements from the supplied command using the <see cref="createListFomReader"/> method.
        /// </summary>
        /// <typeparam name="T">The type of element returned.</typeparam>
        /// <param name="command">The command used to retrieve the elements.</param>
        /// <param name="createListFomReader">The method used to create the elements.</param>
        /// <returns>A list containing all of the elements retrieved.</returns>
        protected virtual List<T> GetAll<T>(IDbCommand command, DbHelper.TCreateListFromReader<T> createListFomReader) where T : class
        {
            List<T> newList = new List<T>();
            DbHelper.TExecuteReader(DataProviderType, command, ConnectionString, createListFomReader, ref newList);
            return newList;
        }

        /// <summary>
        /// Gets the first value returned from the supplied method <see cref="createListFomReader"/>.
        /// </summary>
        /// <typeparam name="T">The type of element returned.</typeparam>
        /// <param name="command">The command used to retrieve the element.</param>
        /// <param name="createListFomReader">The method used to create the element.</param>
        /// <returns>The first element or null if no elements were found.</returns>
        protected virtual T GetFirst<T>(IDbCommand command, DbHelper.TCreateListFromReader<T> createListFomReader) where T : class
        {
            return GetAll(command, createListFomReader).FirstOrDefault();
        }

        /// <summary>
        /// Gets an object the by Identity
        /// </summary>
        /// <typeparam name="T">The type of the object in the table.</typeparam>
        /// <param name="id">The identity valye.</param>
        /// <param name="createListFomReader">The method to create the list of objects from the reader.</param>
        /// <returns>The selected object.</returns>
        protected virtual T GetById<T>(int id, DbHelper.TCreateListFromReader<T> createListFomReader) where T : class
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException("id");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, this.QuerySelectById);

                DbHelper.AddParameterToCommand(command, this.IdentityParameter, id);

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(this.DataProviderType, command, this.ConnectionString, createListFomReader, ref newList);

                if (newList.Count > 0)
                {
                    return newList[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a list of objects by a specified column
        /// </summary>
        /// <typeparam name="T">The type of the object in the table</typeparam>
        /// <param name="columnName">Name of the column to select by.</param>
        /// <param name="columnValue">The column value to select.</param>
        /// <param name="createListFomReader">The method to create the list of objects from the reader.</param>
        /// <returns>The list of objects selected.</returns>
        protected virtual List<T> GetAllByColumn<T>(string columnName, object columnValue, DbHelper.TCreateListFromReader<T> createListFomReader) where T : class
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentOutOfRangeException("columnName");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string columnParameter = DbHelper.InitializeColumnParameter(columnName);
            string columnWhere = DbHelper.InitializeWhereColumn(columnName);

            string queryText = this.QuerySelect + this.KeywordWhere + columnWhere + this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, columnParameter, columnValue);

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(this.DataProviderType, command, this.ConnectionString, createListFomReader, ref newList);

                return newList;
            }
        }

        protected virtual List<T> GetAllByColumn<T>(string columnName, object columnValue, DbHelper.TCreateListFromReader<T> createListFomReader, IDbTransaction transaction) where T : class
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentOutOfRangeException("columnName");
            }

            string columnParameter = DbHelper.InitializeColumnParameter(columnName);
            string columnWhere = DbHelper.InitializeWhereColumn(columnName);

            string queryText = this.QuerySelect + this.KeywordWhere + columnWhere + this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, columnParameter, columnValue);

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(command, transaction, createListFomReader, ref newList);

                return newList;
            }
        }

        protected virtual List<T> GetAllByColumnAndColumnIn<T, TVal>(string columnName, object columnValue, string columnInName, IEnumerable<TVal> columnInValues, DbHelper.TCreateListFromReader<T> createListFomReader) where T : class
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentOutOfRangeException("columnName");
            }

            if (string.IsNullOrEmpty(columnInName))
            {
                throw new ArgumentOutOfRangeException("columnInName");
            }

            if (columnInValues == null || columnInValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("columnInValues");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }


            string columnParameter = DbHelper.InitializeColumnParameter(columnName);
            string columnWhere = DbHelper.InitializeWhereColumn(columnName);

            string queryText = this.QuerySelect + this.KeywordWhere + columnWhere
                             + " AND " + columnInName + " IN (" + DbHelper.BuildParameterList(DbHelper.InitializeColumnParameter(columnInName), columnInValues.Count()) + ") "
                             + this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, columnParameter, columnValue);
                DbHelper.AddParameterListToCommand<TVal>(command, DbHelper.InitializeColumnParameter(columnInName), columnInValues);

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(this.DataProviderType, command, this.ConnectionString, createListFomReader, ref newList);

                return newList;
            }
        }

        /// <summary>
        /// Gets a list of objects by multiple specified columnsT1
        /// </summary>
        /// <typeparam name="T">The type of the object in the table</typeparam>
        /// <param name="columnValues">The column names and values to select.</param>
        /// <param name="createListFomReader">The method to create the list of objects from the reader.</param>
        /// <returns>The list of objects selected.</returns>
        protected virtual List<T> GetAllByColumns<T>(IEnumerable<KeyValuePair<string, object>> columnValues, DbHelper.TCreateListFromReader<T> createListFomReader) where T : class
        {
            if (columnValues == null || columnValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("columnValues must contain at least one element.");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.QuerySelect + this.KeywordWhere;
            queryText += columnValues
              .Select(columnValue => DbHelper.InitializeWhereColumn(columnValue.Key))
              .Aggregate((s1, s2) => s1 + " AND " + s2);
            queryText += this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                foreach (KeyValuePair<string, object> columnValue in columnValues)
                {
                    DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(columnValue.Key), columnValue.Value);
                }

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(this.DataProviderType, command, this.ConnectionString, createListFomReader, ref newList);

                return newList;
            }
        }

        protected virtual List<T> GetAllByColumns<T>(IEnumerable<KeyValuePair<string, object>> columnValues, DbHelper.TCreateListFromReader<T> createListFomReader, IDbTransaction transaction) where T : class
        {
            if (columnValues == null || columnValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("columnValues must contain at least one element.");
            }

            string queryText = this.QuerySelect + this.KeywordWhere;
            queryText += columnValues
              .Select(columnValue => DbHelper.InitializeWhereColumn(columnValue.Key))
              .Aggregate((s1, s2) => s1 + " AND " + s2);
            queryText += this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                foreach (KeyValuePair<string, object> columnValue in columnValues)
                {
                    DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(columnValue.Key), columnValue.Value);
                }

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(command, transaction, createListFomReader, ref newList);

                return newList;
            }
        }

        protected virtual List<T> GetAllByColumnsAndColumnIn<T, TVal>(IEnumerable<KeyValuePair<string, object>> columnValues, string columnInName, IEnumerable<TVal> columnInValues, DbHelper.TCreateListFromReader<T> createListFomReader) where T : class
        {
            if (columnValues == null || columnValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("columnValues must contain at least one element.");
            }

            if (string.IsNullOrEmpty(columnInName))
            {
                throw new ArgumentOutOfRangeException("columnInName");
            }

            if (columnInValues == null || columnInValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("columnInValues");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.QuerySelect + this.KeywordWhere;
            queryText += columnValues
              .Select(columnValue => DbHelper.InitializeWhereColumn(columnValue.Key))
              .Aggregate((s1, s2) => s1 + " AND " + s2);
            queryText += " AND " + columnInName + " IN (" + DbHelper.BuildParameterList(DbHelper.InitializeColumnParameter(columnInName), columnInValues.Count()) + ") "
                      + this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                foreach (KeyValuePair<string, object> columnValue in columnValues)
                {
                    DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(columnValue.Key), columnValue.Value);
                }
                DbHelper.AddParameterListToCommand<TVal>(command, DbHelper.InitializeColumnParameter(columnInName), columnInValues);

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(this.DataProviderType, command, this.ConnectionString, createListFomReader, ref newList);

                return newList;
            }
        }

        /// <summary>
        /// Gets all values for a single column in the table
        /// </summary>
        /// <typeparam name="T">The type of the column in the table.</typeparam>
        /// <param name="columnName">Name of the column to select by.</param>
        /// <param name="createListFomReader">The method to create the list of column value objects from the reader.</param>
        /// <returns>The list of all values in the specified from the table.</returns>
        protected virtual List<T> GetAllColumn<T>(string columnName, DbHelper.TCreateListFromReader<T> createListFomReader)
        {
            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.KeywordSelect + columnName + this.KeywordFrom + this.Table + this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(this.DataProviderType, command, this.ConnectionString, createListFomReader, ref newList);

                return newList;
            }
        }

        /// <summary>
        /// Gets a single object by a specified column
        /// </summary>
        /// <typeparam name="T">The type of the object in the table.</typeparam>
        /// <param name="columnName">Name of the column to select by.</param>
        /// <param name="columnValue">The column value to select.</param>
        /// <param name="createListFomReader">The method to create the list of objects from the reader.</param>
        /// <returns>The object selected.</returns>
        protected virtual T GetFirstByColumn<T>(string columnName, object columnValue, DbHelper.TCreateListFromReader<T> createListFomReader) where T : class
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentOutOfRangeException("columnName");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string columnParameter = DbHelper.InitializeColumnParameter(columnName);
            string columnWhere = DbHelper.InitializeWhereColumn(columnName);

            string queryText = this.QuerySelectTop1 + this.KeywordWhere + columnWhere + this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, columnParameter, columnValue);

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(this.DataProviderType, command, this.ConnectionString, createListFomReader, ref newList);

                if (newList != null && newList.Count > 0)
                {
                    return newList[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the first objects by multiple specified columns
        /// </summary>
        /// <typeparam name="T">The type of the object in the table</typeparam>
        /// <param name="columnValues">The column names and values to select.</param>
        /// <param name="createListFomReader">The method to create the list of objects from the reader.</param>
        /// <returns>The object selected.</returns>
        protected virtual T GetFirstByColumns<T>(IEnumerable<KeyValuePair<string, object>> columnValues, DbHelper.TCreateListFromReader<T> createListFomReader) where T : class
        {
            if (columnValues == null || columnValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("columnValues must contain at least one element.");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.QuerySelectTop1 + this.KeywordWhere;
            queryText += columnValues
              .Select(columnValue => DbHelper.InitializeWhereColumn(columnValue.Key))
              .Aggregate((s1, s2) => s1 + " AND " + s2);
            queryText += this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                foreach (KeyValuePair<string, object> columnValue in columnValues)
                {
                    DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(columnValue.Key), columnValue.Value);
                }

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(this.DataProviderType, command, this.ConnectionString, createListFomReader, ref newList);

                if (newList != null && newList.Count > 0)
                {
                    return newList[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a single value from the specified column, selecting by a different column.
        /// </summary>
        /// <typeparam name="T">The type of the value selected.</typeparam>
        /// <param name="getColumnName">Name of the column to select.</param>
        /// <param name="byColumnName">Name of the column to select by (in the WHERE clause).</param>
        /// <param name="byColumnValue">The column value to select (in the WHERE clause).</param>
        /// <returns>The value selected.</returns>
        protected virtual T GetFirstColumnByColumn<T>(string getColumnName, string byColumnName, object byColumnValue)
        {
            if (string.IsNullOrEmpty(getColumnName))
            {
                throw new ArgumentOutOfRangeException("getColumnName");
            }

            if (string.IsNullOrEmpty(byColumnName))
            {
                throw new ArgumentOutOfRangeException("byColumnName");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string byColumnParameter = DbHelper.InitializeColumnParameter(byColumnName);
            string byColumnWhere = DbHelper.InitializeWhereColumn(byColumnName);

            string queryText = this.KeywordSelect + this.ClauseTop1 + getColumnName + this.KeywordFrom + this.Table + this.HintNoLock + this.KeywordWhere + byColumnWhere + this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, byColumnParameter, byColumnValue);

                object returnVal = DbHelper.ExecuteScalarReader(this.DataProviderType, command, this.ConnectionString);

                return DbHelper.CTypeNull<T>(returnVal, default(T));
            }
        }

        /// <summary>
        /// Gets all values from the specified column, selecting by a different column.
        /// </summary>
        /// <typeparam name="T">The type of the value selected.</typeparam>
        /// <param name="getColumnName">Name of the column to select.</param>
        /// <param name="byColumnName">Name of the column to select by (in the WHERE clause).</param>
        /// <param name="byColumnValue">The column value to select (in the WHERE clause).</param>
        /// <returns>The value selected.</returns>
        protected virtual List<T> GetAllColumnByColumn<T>(string getColumnName, string byColumnName, object byColumnValue, DbHelper.TCreateListFromReader<T> createListFomReader)
        {
            if (string.IsNullOrEmpty(getColumnName))
            {
                throw new ArgumentOutOfRangeException("getColumnName");
            }

            if (string.IsNullOrEmpty(byColumnName))
            {
                throw new ArgumentOutOfRangeException("byColumnName");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string byColumnParameter = DbHelper.InitializeColumnParameter(byColumnName);
            string byColumnWhere = DbHelper.InitializeWhereColumn(byColumnName);

            string queryText = this.KeywordSelect + getColumnName + this.KeywordFrom + this.Table + this.HintNoLock + this.KeywordWhere + byColumnWhere + this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, byColumnParameter, byColumnValue);

                List<T> newList = new List<T>();

                DbHelper.TExecuteReader<T>(this.DataProviderType, command, this.ConnectionString, createListFomReader, ref newList);

                return newList;
            }
        }

        /// <summary>
        /// Gets a single value from the specified column, selecting by a different column.
        /// </summary>
        /// <typeparam name="T">The type of the value selected.</typeparam>
        /// <param name="getColumnName">Name of the column to select.</param>
        /// <param name="byColumnValues">The columns and values to select by (in the WHERE clause).</param>
        /// <returns>The value selected.</returns>
        protected virtual T GetFirstColumnByColumns<T>(string getColumnName, IEnumerable<KeyValuePair<string, object>> byColumnValues)
        {
            if (string.IsNullOrEmpty(getColumnName))
            {
                throw new ArgumentOutOfRangeException("getColumnName");
            }

            if (byColumnValues == null || byColumnValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("byColumnValues must contain at least one element.");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.KeywordSelect + this.ClauseTop1 + getColumnName
                             + this.KeywordFrom + this.Table + this.HintNoLock + this.KeywordWhere;
            queryText += byColumnValues
              .Select(columnValue => DbHelper.InitializeWhereColumn(columnValue.Key))
              .Aggregate((s1, s2) => s1 + " AND " + s2);
            queryText += this.ClauseOrderByDefault;

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                foreach (KeyValuePair<string, object> columnValue in byColumnValues)
                {
                    DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(columnValue.Key), columnValue.Value);
                }

                object returnVal = DbHelper.ExecuteScalarReader(this.DataProviderType, command, this.ConnectionString);

                return DbHelper.CTypeNull<T>(returnVal, default(T));
            }
        }

        /// <summary>
        /// Inserts an object into the table.
        /// </summary>
        /// <typeparam name="T">The type of the object being inserted.</typeparam>
        /// <param name="value">The object to insert.</param>
        /// <param name="addInsertParameters">The method to add insert parameters to the command.</param>
        /// <param name="setIdentity">The method to set the identity value on the object after it is inserted.</param>
        /// <returns>The identity value of the object inserted, if the table has an identity.  Otherwise, DefaultValues.IdentityNotSet.</returns>
        protected virtual int Insert<T>(T value, Action<IDbCommand, T> addInsertParameters, Action<T, int> setIdentity)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, this.QueryInsert);

                addInsertParameters(command, value);

                object retVal = DbHelper.ExecuteScalar(this.DataProviderType, command, this.ConnectionString);

                if (retVal == null)
                {
                    //if the table doesn't have an identity, return IdNotSet
                    return DefaultValues.IdNotSet;
                }
                else
                {
                    int id = DbHelper.CIntNull(retVal);
                    if (setIdentity != null)
                    {
                        setIdentity.Invoke(value, id);
                    }

                    return id;
                }
            }
        }

        /// <summary>
        /// Inserts an object into the table, as part of an existing transaction.
        /// </summary>
        /// <typeparam name="T">The type of the object being inserted.</typeparam>
        /// <param name="value">The object to insert.</param>
        /// <param name="addInsertParameters">The method to add insert parameters to the command.</param>
        /// <param name="setIdentity">The method to set the identity value on the object after it is inserted.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>
        /// The identity value of the object inserted, if the table has an identity.  Otherwise, DefaultValues.IdentityNotSet.
        /// </returns>
        protected virtual int Insert<T>(T value, Action<IDbCommand, T> addInsertParameters, Action<T, int> setIdentity, IDbTransaction transaction)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, this.QueryInsert);

                addInsertParameters(command, value);

                object retVal = DbHelper.ExecuteScalar(command, transaction);

                if (retVal == null)
                {
                    //if the table doesn't have an identity, return IdNotSet
                    return DefaultValues.IdNotSet;
                }
                else
                {
                    int id = DbHelper.CIntNull(retVal);
                    if (setIdentity != null)
                    {
                        setIdentity.Invoke(value, id);
                    }

                    return id;
                }

            }
        }

        /// <summary>
        /// Updates a row in the table with the values on the specified object.
        /// </summary>
        /// <typeparam name="T">The type of the object in the table.</typeparam>
        /// <param name="value">The object to update.</param>
        /// <param name="addUpdateParameters">The method to add update parameters to the command.</param>
        /// <returns>True if a row was updated, otherwise false.</returns>
        protected virtual bool Update<T>(T value, Action<IDbCommand, T> addUpdateParameters)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, this.QueryUpdate);

                addUpdateParameters(command, value);

                int rowCount = DbHelper.ExecuteNonQuery(this.DataProviderType, command, this.ConnectionString);

                return (rowCount > 0);
            }
        }

        /// <summary>
        /// Updates a row in the table with the values on the specified object, using an existing transaction.
        /// </summary>
        /// <typeparam name="T">The type of the object in the table.</typeparam>
        /// <param name="value">The object to update.</param>
        /// <param name="addUpdateParameters">The method to add update parameters to the command.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>
        /// True if a row was updated, otherwise false.
        /// </returns>
        protected virtual bool Update<T>(T value, Action<IDbCommand, T> addUpdateParameters, IDbTransaction transaction)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, this.QueryUpdate);

                addUpdateParameters(command, value);

                int rowCount = DbHelper.ExecuteNonQuery(command, transaction);

                return (rowCount > 0);
            }
        }

        /// <summary>
        /// Updates a column in the table with the value supplied on rows matching the supplied column name/value
        /// </summary>
        /// <param name="updateColumnName">The column to update.</param>
        /// <param name="updateColumnValue">The value to be assigned to the update column</param>
        /// <param name="byColumnName">The column to query by</param>
        /// <param name="byColumnValue">The value for the column being queried by</param>
        /// <returns>
        /// True if a row was updated, otherwise false.
        /// </returns>
        protected virtual bool UpdateColumnByColumn(string updateColumnName, object updateColumnValue, string byColumnName, object byColumnValue)
        {
            if (string.IsNullOrEmpty(updateColumnName))
            {
                throw new ArgumentNullException("updateColumnName");
            }
            if (string.IsNullOrEmpty(byColumnName))
            {
                throw new ArgumentNullException("byColumnName");
            }
            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.KeywordUpdate + this.Table
                             + this.KeywordSet + DbHelper.InitializeColumnUpdate(updateColumnName)
                             + this.KeywordWhere + DbHelper.InitializeWhereColumn(byColumnName);

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(updateColumnName), updateColumnValue);
                DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(byColumnName), byColumnValue);

                int rowCount = DbHelper.ExecuteNonQuery(this.DataProviderType, command, this.ConnectionString);

                return (rowCount > 0);
            }
        }

        protected virtual bool UpdateColumnByColumn(string updateColumnName, object updateColumnValue, string byColumnName, object byColumnValue, IDbTransaction transaction)
        {
            if (string.IsNullOrEmpty(updateColumnName))
            {
                throw new ArgumentNullException("updateColumnName");
            }
            if (string.IsNullOrEmpty(byColumnName))
            {
                throw new ArgumentNullException("byColumnName");
            }

            string queryText = this.KeywordUpdate + this.Table
                             + this.KeywordSet + DbHelper.InitializeColumnUpdate(updateColumnName)
                             + this.KeywordWhere + DbHelper.InitializeWhereColumn(byColumnName);

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(updateColumnName), updateColumnValue);
                DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(byColumnName), byColumnValue);

                int rowCount = DbHelper.ExecuteNonQuery(command, transaction);

                return (rowCount > 0);
            }
        }

        protected virtual bool UpdateColumnByValue(string updateColumnName, object oldValue, object newValue)
        {
            if (string.IsNullOrEmpty(updateColumnName))
            {
                throw new ArgumentNullException("updateColumnName");
            }
            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.KeywordUpdate + this.Table
                             + this.KeywordSet + updateColumnName + " = " + DbHelper.InitializeColumnParameter("NewValue")
                             + this.KeywordWhere + updateColumnName + " = " + DbHelper.InitializeColumnParameter("OldValue");

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter("NewValue"), newValue);
                DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter("OldValue"), oldValue);

                int rowCount = DbHelper.ExecuteNonQuery(this.DataProviderType, command, this.ConnectionString);

                return (rowCount > 0);
            }
        }

        protected virtual bool UpdateColumnByValue(string updateColumnName, object oldValue, object newValue, IDbTransaction transaction)
        {
            if (string.IsNullOrEmpty(updateColumnName))
            {
                throw new ArgumentNullException("updateColumnName");
            }

            string queryText = this.KeywordUpdate + this.Table
                             + this.KeywordSet + updateColumnName + " = " + DbHelper.InitializeColumnParameter("NewValue")
                             + this.KeywordWhere + updateColumnName + " = " + DbHelper.InitializeColumnParameter("OldValue");

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter("NewValue"), newValue);
                DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter("OldValue"), oldValue);

                int rowCount = DbHelper.ExecuteNonQuery(command, transaction);

                return (rowCount > 0);
            }
        }

        /// <summary>
        /// Updates a set of columns in the table with the values supplied on rows matching the supplied column name/value
        /// </summary>
        /// <param name="updateColumnValues">The list of column name/value pairs to be updated.</param>
        /// <param name="byColumnName">The column to query by</param>
        /// <param name="byColumnValue">The value for the column being queried by</param>
        /// <returns>
        /// True if a row was updated, otherwise false.
        /// </returns>
        protected virtual bool UpdateColumnsByColumn(IEnumerable<KeyValuePair<string, object>> updateColumnValues, string byColumnName, object byColumnValue)
        {
            if (updateColumnValues == null || updateColumnValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("updateColumnValues must contain at least one element.");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.KeywordUpdate + this.Table + this.KeywordSet
                             + updateColumnValues
                             .Select(columnValue => DbHelper.InitializeColumnUpdate(columnValue.Key))
                             .Aggregate((s1, s2) => s1 + " , " + s2)
                             + this.KeywordWhere + DbHelper.InitializeWhereColumn(byColumnName);

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                foreach (KeyValuePair<string, object> columnValue in updateColumnValues)
                {
                    DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(columnValue.Key), columnValue.Value);
                }
                DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(byColumnName), byColumnValue);

                int rowCount = DbHelper.ExecuteNonQuery(this.DataProviderType, command, this.ConnectionString);

                return (rowCount > 0);
            }
        }

        protected virtual bool UpdateColumnByColumns(string updateColumnName, object updateColumnValue, IEnumerable<KeyValuePair<string, object>> byColumnValues)
        {
            if (byColumnValues == null || byColumnValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("byColumnValues must contain at least one element.");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.KeywordUpdate + this.Table + this.KeywordSet
                             + DbHelper.InitializeColumnUpdate(updateColumnName) + this.KeywordWhere
                             + byColumnValues
                                .Select(columnValue => DbHelper.InitializeWhereColumn(columnValue.Key))
                                .Aggregate((s1, s2) => s1 + " AND " + s2);

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                foreach (KeyValuePair<string, object> columnValue in byColumnValues)
                {
                    DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(columnValue.Key), columnValue.Value);
                }
                DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(updateColumnName), updateColumnValue);

                int rowCount = DbHelper.ExecuteNonQuery(this.DataProviderType, command, this.ConnectionString);

                return (rowCount > 0);
            }
        }

        /// <summary>
        /// Deletes the rows in the table with a given identity value
        /// </summary>
        /// <param name="idValue">The identify value to delete by.</param>
        /// <returns>True if any rows were deleted, otherwise false.</returns>
        protected virtual bool DeleteById(object idValue)
        {
            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.KeywordDelete + this.Table + this.KeywordWhere + DbHelper.InitializeWhereColumn(this.Identity);

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, this.IdentityParameter, idValue);

                int rowCount = DbHelper.ExecuteNonQuery(this.DataProviderType, command, this.ConnectionString);

                return rowCount > 0;
            }
        }

        /// <summary>
        /// Deletes the rows in the table with a given identity value, using an existing transaction.
        /// </summary>
        /// <param name="idValue">The identify value to delete by.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>True if any rows were deleted, otherwise false.</returns>
        protected virtual bool DeleteById(object idValue, IDbTransaction transaction)
        {
            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string queryText = this.KeywordDelete + this.Table + this.KeywordWhere + DbHelper.InitializeWhereColumn(this.Identity);

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, this.IdentityParameter, idValue);

                int rowCount = DbHelper.ExecuteNonQuery(command, transaction);

                return rowCount > 0;
            }
        }

        /// <summary>
        /// Deletes all rows in the table by the specified column.
        /// </summary>
        /// <param name="columnName">Name of the column to delete by.</param>
        /// <param name="columnValue">The column value to delete by.</param>
        /// <returns>True if any rows were deleted, otherwise false.</returns>
        protected virtual bool DeleteAllByColumn(string columnName, object columnValue)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentOutOfRangeException("columnName");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string columnParameter = DbHelper.InitializeColumnParameter(columnName);
            string columnWhere = DbHelper.InitializeWhereColumn(columnName);

            List<string> keyColumns = new List<string>();
            if (this.Identity != null && this.Identity != string.Empty)
            {
                keyColumns.Add(this.Identity);
            }
            foreach (string col in this._NonIdentityPrimaryKeys)
            {
                keyColumns.Add(col);
            }

            string queryText;
            if (keyColumns.Any())
            {
                string columnJoinStatement = keyColumns
                    .Select(column => ("t1." + column + " = t2." + column))
                    .Aggregate((s1, s2) => s1 + " AND " + s2);

                queryText = "WITH t2 AS (" + this.KeywordSelect + this.KeywordDistinct + this.PrimaryKeys + this.KeywordFrom + this.Table + this.HintNoLock + this.KeywordWhere + columnWhere + ")"
                    + this.KeywordDelete + "t1" + this.KeywordFrom + this.Table + " t1 " + this.KeywordJoin + "t2 ON " + columnJoinStatement;
            }
            else
            {
                //no identity and no non-identity column(s)
                queryText = this.KeywordDelete + this.Table + this.KeywordWhere + columnWhere;
            }

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, columnParameter, columnValue);

                int rowCount = DbHelper.ExecuteNonQuery(this.DataProviderType, command, this.ConnectionString);

                return rowCount > 0;
            }
        }

        /// <summary>
        /// Deletes all rows in the table by the specified column, using an existing transaction.
        /// </summary>
        /// <param name="columnName">Name of the column to delete by.</param>
        /// <param name="columnValue">The column value to delete by.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>
        /// True if any rows were deleted, otherwise false.
        /// </returns>
        protected virtual bool DeleteAllByColumn(string columnName, object columnValue, IDbTransaction transaction)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentOutOfRangeException("columnName");
            }

            string columnParameter = DbHelper.InitializeColumnParameter(columnName);
            string columnWhere = DbHelper.InitializeWhereColumn(columnName);

            List<string> keyColumns = new List<string>();
            if (this.Identity != null && this.Identity != string.Empty)
            {
                keyColumns.Add(this.Identity);
            }
            foreach (string col in this._NonIdentityPrimaryKeys)
            {
                keyColumns.Add(col);
            }

            string queryText;
            if (keyColumns.Any())
            {
                string columnJoinStatement = keyColumns
                    .Select(column => ("t1." + column + " = t2." + column))
                    .Aggregate((s1, s2) => s1 + " AND " + s2);

                queryText = "WITH t2 AS (" + this.KeywordSelect + this.KeywordDistinct + this.PrimaryKeys + this.KeywordFrom + this.Table + this.HintNoLock + this.KeywordWhere + columnWhere + ")"
                    + this.KeywordDelete + "t1" + this.KeywordFrom + this.Table + " t1 " + this.KeywordJoin + "t2 ON " + columnJoinStatement;
            }
            else
            {
                //no identity and no non-identity column(s)
                queryText = this.KeywordDelete + this.Table + this.KeywordWhere + columnWhere;
            }

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                DbHelper.AddParameterToCommand(command, columnParameter, columnValue);

                int rowCount = DbHelper.ExecuteNonQuery(command, transaction);

                return rowCount > 0;
            }
        }

        protected virtual bool DeleteAllByColumns(IEnumerable<KeyValuePair<string, object>> columnValues)
        {
            if (columnValues == null || columnValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("columnValues must contain at least one element.");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentOutOfRangeException("ConnectionString is blank.");
            }

            string columnWhere = columnValues
              .Select(columnValue => DbHelper.InitializeWhereColumn(columnValue.Key))
              .Aggregate((s1, s2) => s1 + " AND " + s2);

            List<string> keyColumns = new List<string>();
            if (this.Identity != null && this.Identity != string.Empty)
            {
                keyColumns.Add(this.Identity);
            }
            foreach (string col in this._NonIdentityPrimaryKeys)
            {
                keyColumns.Add(col);
            }

            string queryText;
            if (keyColumns.Any())
            {
                string columnJoinStatement = keyColumns
                    .Select(column => ("t1." + column + " = t2." + column))
                    .Aggregate((s1, s2) => s1 + " AND " + s2);

                queryText = "WITH t2 AS (" + this.KeywordSelect + this.KeywordDistinct + this.PrimaryKeys + this.KeywordFrom + this.Table + this.HintNoLock + this.KeywordWhere + columnWhere + ")"
                    + this.KeywordDelete + "t1" + this.KeywordFrom + this.Table + " t1 " + this.KeywordJoin + "t2 ON " + columnJoinStatement;
            }
            else
            {
                //no identity and no non-identity column(s)
                queryText = this.KeywordDelete + this.Table + this.KeywordWhere + columnWhere;
            }


            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                foreach (KeyValuePair<string, object> columnValue in columnValues)
                {
                    DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(columnValue.Key), columnValue.Value);
                }

                int rowCount = DbHelper.ExecuteNonQuery(this.DataProviderType, command, this.ConnectionString);

                return rowCount > 0;
            }
        }

        protected virtual bool DeleteAllByColumns(IEnumerable<KeyValuePair<string, object>> columnValues, IDbTransaction transaction)
        {
            if (columnValues == null || columnValues.Count() < 1)
            {
                throw new ArgumentOutOfRangeException("columnValues must contain at least one element.");
            }

            string columnWhere = columnValues
              .Select(columnValue => DbHelper.InitializeWhereColumn(columnValue.Key))
              .Aggregate((s1, s2) => s1 + " AND " + s2);

            List<string> keyColumns = new List<string>();
            if (this.Identity != null && this.Identity != string.Empty)
            {
                keyColumns.Add(this.Identity);
            }
            foreach (string col in this._NonIdentityPrimaryKeys)
            {
                keyColumns.Add(col);
            }

            string queryText;
            if (keyColumns.Any())
            {
                string columnJoinStatement = keyColumns
                    .Select(column => ("t1." + column + " = t2." + column))
                    .Aggregate((s1, s2) => s1 + " AND " + s2);

                queryText = "WITH t2 AS (" + this.KeywordSelect + this.KeywordDistinct + this.PrimaryKeys + this.KeywordFrom + this.Table + this.HintNoLock + this.KeywordWhere + columnWhere + ")"
                    + this.KeywordDelete + "t1" + this.KeywordFrom + this.Table + " t1 " + this.KeywordJoin + "t2 ON " + columnJoinStatement;
            }
            else
            {
                //no identity and no non-identity column(s)
                queryText = this.KeywordDelete + this.Table + this.KeywordWhere + columnWhere;
            }

            using (IDbCommand command = DbHelper.CreateCommand(this.DataProviderType))
            {
                DbHelper.SetCommandType(command, CommandType.Text, queryText);

                foreach (KeyValuePair<string, object> columnValue in columnValues)
                {
                    DbHelper.AddParameterToCommand(command, DbHelper.InitializeColumnParameter(columnValue.Key), columnValue.Value);
                }

                int rowCount = DbHelper.ExecuteNonQuery(command, transaction);

                return rowCount > 0;
            }
        }

        #endregion
    }
}
