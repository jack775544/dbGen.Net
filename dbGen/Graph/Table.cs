using System;
using System.Linq;
using System.Collections.Generic;

namespace dbGen
{
    public class DatabaseTable
    {
        /// The length of the table in rows
        public int TableLength {get; set;}
        /// The name of the table
        public string TableName {get; set;}
        /// Private Column list
        private List<DatabaseColumn> _Columns;
        /// The column list of the table
        public List<DatabaseColumn> Columns 
        {
            get
            {
                return new List<DatabaseColumn>(_Columns);
            }
            private set
            {
                _Columns = value;
            }
        }
        /// The length of the table
        public int length {get; private set;}
        
        /**
        * The constructor for the database table
        */
        public DatabaseTable(string tableName, int tableLength, List<DatabaseColumn> columns)
        {
            TableName = tableName;
            TableLength = tableLength;
            if (columns.Select(x => x.Name).ToHashSet().Count() == columns.Count())
            {
                throw new InvalidOperationException("The columns have duplicate names");
            }
            Columns = new List<DatabaseColumn>(columns);
            length = 1;
        }

        public int ColumnCount()
        {
            return Columns.Count();
        }

        /**
        * Returns true iff the table has foreign keys
        */
        public bool HasReferences()
        {
            return Columns.Any(x => x is ForeignKeyColumn);
        }

        /**
        * Gets any tables referenced by the foreign keys in the table
        */
        public List<DatabaseTable> GetReferencedTables()
        {
            return Columns.Where(x => x is ForeignKeyColumn)
                .Select(x => (ForeignKeyColumn) x)
                .Select(x => x.ReferenceTable)
                .ToList();
        }

        /**
        * Adds a list of <column name -> data> rows to the table. If no data is set for a column in the row then set to null.
        * If a not null column is set to null then throw an error.
        */
        public void AddRows(List<Dictionary<string, string>> data)
        {
            foreach (var row in data)
            {
                AddRow(row);
            }
        }

        /// <summary>
        /// Adds a row to the database
        /// </summary>
        /// <param name="data">
        /// A dictionary in the form of (column name -> column data)
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when there is an invalid insert (ie. NOT NULL column not set, wrong data for foreign key).
        /// </exception>
        public void AddRow(Dictionary<string, string> data)
        {
            // Get all the columns from the database as a tuple of (column, bool)
            var columnData = (from c in Columns
                select (c, c is ForeignKeyColumn))
                .ToList();
            foreach (var col in data)
            {
                // This should return a IEnumerable of size one, so we take that one off the top
                // If this was of any size greater then one then the table is going to have columns of duplicate names.
                var insertCol = columnData.Where(x => x.Item1.Name == col.Key).First();
                if (insertCol.Item2)
                {
                    var foreignKeyColumn = (ForeignKeyColumn) insertCol.Item1;
                    // If the reference of the foreign key does not have the data that is going to be inserted
                    // then throw an error as it is a violation of the foreign key constraint
                    if (foreignKeyColumn.ReferenceColumn.Data.Where(x => x == col.Value).Count() == 0)
                    {
                        throw new InvalidOperationException("A foreign key column had data inserted which was not referenced in the parent");
                    }
                }
                // Passed the checks so insert the data
                insertCol.Item1.Data.Add(col.Value);
                columnData.Remove(insertCol);
            }
            // A NOT NULL column had no data set so throw an error
            if (columnData.Any(x => x.Item2 == false))
            {
                throw new InvalidOperationException("A NOT NULL column was not set");
            }
            // Populate the rest of the columns with nulls
            columnData.ForEach(x => x.Item1.Data.Append("NULL"));
            // Increment the length of the table
            length += 1;
        }

        public override string ToString()
        {
            var text = string.Join(", ", Columns);
            return $"({text})";
        }
    }
}