using System;
using System.Collections.Generic;

namespace dbGen
{
    class JsonProcessor
    {
        public static Schema Process(TableStructure[] tableArray)
        {
            var tables = new List<DatabaseTable>();
            foreach (var table in tableArray)
            {
                foreach (var c in table.Columns)
                {
                    if (c.IsForeignKey == true)
                    {

                    }
                }
            }
            return null;
        }
    }
}