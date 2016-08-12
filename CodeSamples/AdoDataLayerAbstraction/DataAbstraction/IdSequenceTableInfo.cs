using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDataLayerAbstraction
{
    public class IdSequenceTableInfo
    {
        private string _TableName;
        private string _KeyColumnName;
        private string _IdColumnName;

        public string TableName
        {
            get
            {
                return _TableName;
            }
            set
            {
                _TableName = value;
            }
        }

        public string KeyColumnName
        {
            get
            {
                return _KeyColumnName;
            }
            set
            {
                _KeyColumnName = value;
            }
        }

        public string IdColumnName
        {
            get
            {
                return _IdColumnName;
            }
            set
            {
                _IdColumnName = value;
            }
        }

        public IdSequenceTableInfo()
        {

        }

        public IdSequenceTableInfo(string table, string keyColumn, string idColumn)
        {
            _TableName = table;
            _KeyColumnName = keyColumn;
            _IdColumnName = idColumn;
        }
    }

}
