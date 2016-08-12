using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoDataLayerAbstraction.Data
{
    public class DbSomeObjectData : DbBaseData, ISomeObjectData
    {
        private StuffDataFactory _DataFactory;

        public DbSomeObjectData(StuffDataFactory dataFactory)
            :  base(dataFactory == null ? null : dataFactory.DataContext)
        {
            _DataFactory = dataFactory;

            _Table = "SomeObjects";
            _Identity = "Id";
            this._NonIdentityPrimaryKeys = new List<string>()
            {
                //none
            };
            this._Columns = new List<string>()
            {
                "Property1",
                "Property2",
            };

            this._Orders = new List<string>()
            {
                "Property1" + this.KeywordAscending,
            };
        }

        protected virtual void TCreateListFromReader(IDataReader reader, ref List<SomeObject> newList)
        {
            int id = reader.GetOrdinal("Id");
            int property1 = reader.GetOrdinal("Property1");
            int property2 = reader.GetOrdinal("Property2");

            while(reader.Read())
            {
                SomeObject newItem = new SomeObject(this._DataFactory);
                newItem.Id = DbHelper.CIntNull(reader[id]);
                newItem.Property1 = DbHelper.CStrNull(reader[property1]);
                newItem.Property2 = DbHelper.CStrNull(reader[property2]);
            }
        }

        protected virtual void AddInsertParameters(IDbCommand command, SomeObject value)
        {
            DbHelper.AddParameterToCommand(command, "@Property1", value.Property1);
            DbHelper.AddParameterToCommand(command, "@Property2", value.Property2);
        }

        protected virtual void AddUpdateParameters(IDbCommand command, SomeObject value)
        {
            DbHelper.AddParameterToCommand(command, "@Id", value.Id);
            AddInsertParameters(command, value);
        }

        public SomeObject GetById(int id)
        {
            return this.GetById<SomeObject>(id, TCreateListFromReader);
        }

        public int Insert(SomeObject value)
        {
            return this.Insert<SomeObject>(value, AddInsertParameters, (x, id) => x.Id = id);
        }

        public bool Update(SomeObject value)
        {
            return this.Update<SomeObject>(value, AddUpdateParameters);
        }

        public bool Delete(SomeObject value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return this.DeleteById(value.Id);
        }
    }
}
