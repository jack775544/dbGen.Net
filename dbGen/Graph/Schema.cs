using System;
using System.Linq;
using System.Collections.Generic;

namespace dbGen
{
    ///<summary>
    ///An Immutable 
    public class Schema
    {
        private List<DatabaseTable> _Tables;
        public List<DatabaseTable> Tables
        {
            get
            {
                return new List<DatabaseTable>(_Tables);
            }
            private set
            {
                _Tables = new List<DatabaseTable>(value);
            }
        }
        private List<DatabaseTable> OrderedTables;

        public Schema(List<DatabaseTable> tables)
        {
            Tables = new List<DatabaseTable>(tables);
            OrderTables();
        }

        public string GetAllSQLDDLStatements(){
            return String.Join($"{Environment.NewLine}", Tables.Select(x => x.GetSQLCreateTableStatements()));
        }

        public IEnumerable<string> Lines()
        {
            yield break;
        }

        /// Orders the tables in a way such that the first table references no others
        /// and the second references only the first (or not at all) and so on.
        /// The final ordered list will be placed in OrderedTables
        private void OrderTables()
        {
            if (OrderedTables != null)
            {
                return;
            }
            var ts = new List<DatabaseTable>(Tables);
            for (var i = 0; i < ts.Count; i++)
            {
                var done = false;
                var j = i;
                while (!done)
                {
                    var current = ts[j];
                    if (current.HasReferences() == false)
                    {
                        // No references, can add to the list
                        OrderedTables.Add(current);
                        ts.Remove(current);
                        done = true;
                    }
                    else
                    {
                        if (current.GetReferencedTables().All(x => OrderedTables.Contains(x)))
                        {
                            // Every referenced item is in the the OrderedList already so add it
                            OrderedTables.Add(current);
                            ts.Remove(current);
                            done = true;
                        }
                        else
                        {
                            // This table depends on something that isn't in the current OrderedTables List
                            // So skip it and cover it next iteration
                            j += 1;
                            // If there are no more tables on the list and there are still references that can't be 
                            // resolved then there is something wrong with the schema. This is almost certainly a self
                            // referential error or a circular dependency
                            if (j >= Tables.Count)
                            {
                                throw new InvalidSchemaException("The schema has a circular dependency or self referential table");
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            var tables = Tables.ToString();
            return $"[{tables}]";
        }

        private class InvalidSchemaException : Exception
        {
            public InvalidSchemaException(string message) : base(message)
            {

            }
        }
    }
}