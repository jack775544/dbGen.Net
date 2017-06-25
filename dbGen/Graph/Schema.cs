using System;
using System.Collections.Generic;

namespace dbGen
{
    ///<summary>
    ///An Immutable 
    public class Schema
    {
        private List<DatabaseTable> _Tables;
        public List<DatabaseTable> Tables {
            get 
            {
                return new List<DatabaseTable>(_Tables);
            }
            private set
            {
                _Tables = new List<DatabaseTable>(value);   
            }
        }

        public Schema()
        {
            Tables = new List<DatabaseTable>();
        }

        public Schema(List<DatabaseTable> tables)
        {
            Tables = new List<DatabaseTable>(tables);
        }

        public override string ToString()
        {
            return $"[{Tables.ToString()}]";
        }
    }
}