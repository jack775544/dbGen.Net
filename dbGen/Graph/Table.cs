using System;
using System.Linq;
using System.Collections.Generic;

namespace dbGen
{
    public class DatabaseTable
    {
        // The length of the table in rows
        public int TableLength {get; set;}
        // The name of the table
        public string TableName {get; set;}
        // The column list of the table
        public List<DatabaseColumn> Columns {get; set;}
        
        /**
        * The constructor for the database table
        */
        public DatabaseTable(string tableName, int tableLength, List<DatabaseColumn> columns)
        {
            TableName = tableName;
            TableLength = tableLength;
            Columns = new List<DatabaseColumn>(columns);
        }

        public int ColumnCount()
        {
            return Columns.Count;
        }

        /**
        * Returns true iff the table has foreign keys
        */
        public bool HasReferences()
        {
            return Columns.Any(x => x is DerivedDatabaseColumn);
        }

        /**
        * Gets any tables referenced by the foreign keys in the table
        */
        public List<DatabaseTable> GetReferencedTables()
        {
            return Columns.Where(x => x is DerivedDatabaseColumn)
                .Select(x => (DerivedDatabaseColumn) x)
                .Select(x => x.ReferenceTable)
                .ToList();
        }

        /**
        * Adds a list of <column name -> data> rows to the table. If no data is set for a column in the row then set to null.
        * If a not null column is set to null then throw an error.
        */
        public bool AddData(List<Dictionary<string, string>> data)
        {
            foreach (var row in data)
            {
                // CLone our row since we are going to mutate further down the function.
                var rowClone = new Dictionary<string, string>(row);
                foreach (var col in rowClone)
                {
                    
                }
            }
            return false;
        }
    }
}