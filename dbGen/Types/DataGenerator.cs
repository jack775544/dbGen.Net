using System;

namespace dbGen
{
    public interface IDataGenerator
    {
        string Opener {get;}
        string Closer {get;}
        string DatabaseTypeString {get;}
        bool Ordered {get;}
        bool Unique {get;}
        
        string next();
    }

    public class OrderedIntegerDataGenerator : IDataGenerator
    {
        public string Opener {get;}
        public string Closer {get;}
        public string DatabaseTypeString {get;}
        public bool Ordered {get;}
        public bool Unique {get;}
        private int Current;

        public OrderedIntegerDataGenerator(int start)
        {
            Current = start - 1;
            Opener = "";
            Closer = "";
            DatabaseTypeString = "NUMBER(255)";
            Ordered = true;
            Unique = true;
        }

        public OrderedIntegerDataGenerator(int start, string databaseType)
        {
            Current = start - 1;
            Opener = "";
            Closer = "";
            DatabaseTypeString = databaseType;
            Ordered = true;
            Unique = true;
        }

        public string next()
        {
            Current += 1;
            return Current.ToString();
        }
        
    }
}