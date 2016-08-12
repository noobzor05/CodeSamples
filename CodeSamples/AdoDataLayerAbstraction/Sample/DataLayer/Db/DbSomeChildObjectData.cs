using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoDataLayerAbstraction.Data
{
    public class DbSomeChildObjectData : DbBaseData, ISomeChildObjectData
    {
        private StuffDataFactory _DataFactory;

        public DbSomeChildObjectData(StuffDataFactory dataFactory)
            :  base(dataFactory == null ? null : dataFactory.DataContext)
        {
            _DataFactory = dataFactory;

            _Table = "SomeObjects";
            _Identity = "Id";
            this._NonIdentityPrimaryKeys = new List<string>()
            {
                "ParentId",
            };
            this._Columns = new List<string>()
            {
                "SomeProperty",
            };

            this._Orders = new List<string>()
            {
                "SomeProperty" + this.KeywordAscending,
            };
        }

        protected virtual void TCreateListFromReader(IDataReader reader, ref List<SomeChildObject> newList)
        {
            int id = reader.GetOrdinal("Id");
            int parentId = reader.GetOrdinal("ParentId");
            int someProperty = reader.GetOrdinal("SomeProperty");

            while (reader.Read())
            {
                SomeChildObject newItem = new SomeChildObject();
                newItem.Id = DbHelper.CIntNull(reader[id]);
                newItem.ParentId = DbHelper.CIntNull(reader[parentId]);
                newItem.SomeProperty = DbHelper.CStrNull(reader[someProperty]);
            }
        }

        protected virtual void AddInsertParameters(IDbCommand command, SomeChildObject value)
        {
            DbHelper.AddParameterToCommand(command, "@ParentId", value.ParentId);
            DbHelper.AddParameterToCommand(command, "@SomeProperty", value.SomeProperty);
        }

        protected virtual void AddUpdateParameters(IDbCommand command, SomeChildObject value)
        {
            DbHelper.AddParameterToCommand(command, "@Id", value.Id);
            AddInsertParameters(command, value);
        }

        public SomeChildObject GetById(int id)
        {
            return this.GetById<SomeChildObject>(id, TCreateListFromReader);
        }

        public List<SomeChildObject> GetAllByParentId(int parentId)
        {
            return this.GetAllByColumn<SomeChildObject>("ParentId", parentId, TCreateListFromReader);
        }

        public int Insert(SomeChildObject value)
        {
            return this.Insert<SomeChildObject>(value, AddInsertParameters, (x, id) => x.Id = id);
        }

        public bool Update(SomeChildObject value)
        {
            return this.Update<SomeChildObject>(value, AddUpdateParameters);
        }

        public bool Delete(SomeChildObject value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return this.DeleteById(value.Id);
        }

        public bool DeleteAllByParentId(int parentId)
        {
            return this.DeleteAllByColumn("ParentId", parentId);
        }
    }
}
