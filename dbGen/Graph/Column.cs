using dbGen;

namespace dbGen.Net
{
    public class DatabaseColumn
    {
        public string Name {get; set;}
        public DataGenerator DataType {get; set;}
        public DatabaseTable ReferenceTable {get; set;}
        public DatabaseColumn ReferenceColumn {get; set;}
        public bool RandomValue {get; set;}
        public bool PrimaryKey {get; set;}
        public bool Unique {get; set;}
        public bool NotNull {get; set;}
        
        public DatabaseColumn(string name, DataGenerator dataType)
        {
            
        }
    }

    public class DerivedDatabaseColumn : DatabaseColumn
    {
        public DerivedDatabaseColumn(string name, DataGenerator dataType) : base(name, dataType)
        {
            
        }
    }
}