using System;
using System.Collections.Generic;

namespace dbGen
{
    /**
    * A Column in the database. This column is not a foreign key.
    */
    public class DatabaseColumn
    {
        /// The name of the column
        public string ColumnName {get; set;}
        /// The generator of the data for the column
        public IDataGenerator Generator {get; set;}
        /// Is this column a primary key
        public bool PrimaryKey {get; set;}
        /// Is this column unique
        public bool Unique {get; set;}
        /// Is the column not null
        public bool NotNull {get; set;}
        /// The string version of the data of this column
        public List<string> Data {get; set;}
        /// Has this column had it's data already set
        public bool HasData {get; set;}
        
        public DatabaseColumn(string name, IDataGenerator dataGenerator)
        {
            /// Set name and generator
            ColumnName = name;
            Generator = dataGenerator;

            /// The table starts off empty
            Data = null;
            HasData = false;

            /// By default no special properties
            PrimaryKey = false;
            Unique = false;
            NotNull = false;
        }

        public override string ToString()
        {
            return "{" + ColumnName + "}";
        }
    }

    /**
    * A column in the database. This column is a foreign key
    */
    public class ForeignKeyColumn : DatabaseColumn
    {
        /// The table where the foreign key is located
        public DatabaseTable ReferenceTable {get; set;}
        /// The column which the foreign key refers to.
        public DatabaseColumn ReferenceColumn {get; set;}
        /// Does this column randomly select values from it's reference or sequentially
        public bool RandomValue {get; set;}

        /**
        * Constructor for the foreign key column. The constructor for this column sets the parent data generator to false since 
        * it refers to the data in it's reference table.
        */
        public ForeignKeyColumn(string name, DatabaseTable referenceTable, DatabaseColumn referenceColumn) : base(name, null)
        {
            ReferenceTable = referenceTable;
            ReferenceColumn = referenceColumn;
        }
    }
}