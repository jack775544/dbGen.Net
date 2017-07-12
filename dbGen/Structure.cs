using System;
using System.Collections.Generic;

namespace dbGen
{
    public struct ColumnStructure
    {
        public string Name;
        public bool PrimaryKey;
        public bool Unique;
        public bool NotNull;

        public bool IsForeignKey;
        public string ColumnReference;
        public string TableReference;
    }

    public struct TableStructure
    {
        public string Name;
        public int Length;
        public string DataType;
        public List<ColumnStructure> Columns;
    }

}