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
                _Columns = new List<DatabaseColumn>(value);
            }
        }
        /// The length of the table
        public int Length {get; private set;}
        /// Current position in the table for the generator
        public int Current {get; private set;}
        
        /// The constructor for the database table
        public DatabaseTable(string tableName, int tableLength, List<DatabaseColumn> columns)
        {
            Current = 0;
            TableName = tableName;
            TableLength = tableLength;
            if (columns.Select(x => x.ColumnName).ToHashSet().Count() == columns.Count())
            {
                throw new InvalidOperationException("The columns have duplicate names");
            }
            Columns = new List<DatabaseColumn>(columns);
            Length = 1;
        }

        public int ColumnCount()
        {
            return Columns.Count();
        }

        ///Returns true iff the table has foreign keys
        public bool HasReferences()
        {
            return Columns.Any(x => x is ForeignKeyColumn);
        }

        /// Gets any tables referenced by the foreign keys in the table
        public List<DatabaseTable> GetReferencedTables()
        {
            return Columns.Where(x => x is ForeignKeyColumn)
                .Select(x => (ForeignKeyColumn) x)
                .Select(x => x.ReferenceTable)
                .ToList();
        }

        private IEnumerable<List<(String, String)>> Rows()
        {
            if (Current == Length)
            {
                yield break;
            }
            Current++;
            yield return Columns.Select(x => (x.ColumnName, x.Generator.Opener + x.Generator.next() + x.Generator.Closer)).ToList();
        }

        public IEnumerable<string> SQLRows()
        {
            foreach (var r in Rows())
            {
                var header = $"INSERT INTO {TableName} (";
                var body = "VALUES (";
                foreach (var c in r)
                {
                    header += c.Item1 + ",";
                    body += c.Item2 + ",";
                }
                header = header.TrimEnd(',');
                body = body.TrimEnd(',');
                header += $"){Environment.NewLine}";
                body += $");{Environment.NewLine}";
                yield return header + body;
            }
            yield break;
        }

        /// Make the SQL Create table statements
        public string GetSQLCreateTableStatements()
        {
            /*
            SQL Create Table statements have the following form
            CREATE TABLE table_name (
                column1 column1_datatype,
                column2 column2_datatype,
                column3 column3_datatype,
            )

            Now account for the foreign key relations and other constraints
            There are 4 different types of constraints: Primary Key, Foreign Key, Not Null and Unique
            They have the following forms:
            
            ALTER TABLE table_name
            ADD CONSTRAINT PK_column1
            PRIMARY KEY (column1);

            ALTER TABLE table_name
            ADD CONSTRAINT FK_column3_TO_table_name2_column1
            FOREIGN KEY (column3) REFERENCES table_name2 (column1);

            ALTER TABLE table_name
            ADD CONSTRAINT NN_column1
            CHECK (column1 IS NOT NULL);

            ALTER TABLE table_name
            ADD CONSTRAINT UN_column1 UNIQUE (column1); 
             */
            var line = Environment.NewLine;
            var result = $"CREATE TABLE {TableName}({line}";
            var constraintResult = "";
            foreach (var c in Columns)
            {
                // Do FK check first since it requires a type check
                if (c is ForeignKeyColumn)
                {
                    // Foreign Keys need the type of the value they are referencing
                    var fc = (ForeignKeyColumn) c;
                    result += $"    {fc.ColumnName} {fc.ReferenceColumn.Generator.DatabaseTypeString}{line}";
                    constraintResult += $"ALTER TABLE {TableName}{line}" +
                        $"ADD CONSTRAINT FK_{fc.ColumnName}_TO_{fc.ReferenceTable.TableName}_{fc.ReferenceColumn.ColumnName}{line}" +
                        $"FOREIGN KEY ({fc.ColumnName}) REFERENCES {fc.ReferenceTable.TableName} ({fc.ReferenceColumn.ColumnName});{line}";
                } else {
                    // Non foreign keys can use their own type
                    result += $"    {c.ColumnName} {c.Generator.DatabaseTypeString}{line}";
                }

                // Do PK check next
                if (c.PrimaryKey == true)
                {
                    constraintResult += $"ALTER TABLE {TableName}{line}" +
                        $"ADD CONSTRAINT PK_{c.ColumnName}{line}" +
                        $"PRIMARY KEY ({c.ColumnName});{line}";
                }

                // Do Not Null Check
                if (c.NotNull == true)
                {
                    constraintResult += $"ALTER TABLE {TableName}{line}" +
                        $"ADD CONSTRAINT NN_{c.ColumnName}{line}" +
                        $"CHECK ({c.ColumnName} IS NOT NULL);{line}";
                }

                // Do Unique Check
                if (c.Unique == true)
                {
                    constraintResult += $"ALTER TABLE {TableName}{line}" +
                        $"ADD CONSTRAINT UN_column1 UNIQUE ({c.ColumnName});{line}";
                }
            }
            result += $");{line}";
            return result;
        }

        public override string ToString()
        {
            var text = string.Join(", ", Columns);
            return $"({text})";
        }
    }
}