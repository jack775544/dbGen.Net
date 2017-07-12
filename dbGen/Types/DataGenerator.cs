using System;
using System.Collections.Generic;

namespace dbGen
{
    public class DataGenerator
    {
        public Dictionary<string, IDataGenerator> Mappings
        {
            get
            {
                var map = new Dictionary<string, IDataGenerator>();
                return map;
            }
        }

    }

    public interface IDataGenerator
    {
        string Opener {get;}
        string Closer {get;}
        string DatabaseTypeString {get;}
        bool Ordered {get;}
        bool Unique {get;}
        List<String> Data {get;}
        
        string next();
    }

    public class OrderedIntegerDataGenerator : IDataGenerator
    {
        public string Opener {get;}
        public string Closer {get;}
        public string DatabaseTypeString {get;}
        public bool Ordered {get;}
        public bool Unique {get;}
        public List<String> Data {get;}
        private int Current;

        public OrderedIntegerDataGenerator()
        {
            Current = 0;
            Opener = "";
            Closer = "";
            DatabaseTypeString = "NUMBER(255)";
            Ordered = true;
            Unique = true;
            Data = new List<String>();
        }

        public OrderedIntegerDataGenerator(int start)
        {
            Current = start - 1;
            Opener = "";
            Closer = "";
            DatabaseTypeString = "NUMBER(255)";
            Ordered = true;
            Unique = true;
            Data = new List<String>();
        }

        public OrderedIntegerDataGenerator(int start, string databaseType)
        {
            Current = start - 1;
            Opener = "";
            Closer = "";
            DatabaseTypeString = databaseType;
            Ordered = true;
            Unique = true;
            Data = new List<String>();
        }

        public string next()
        {
            Current += 1;
            var result = Current.ToString();
            Data.Add(result);
            return result;
        }
    }

    public class RandomIntegerDataGenerator : IDataGenerator
    {
        public string Opener {get;}
        public string Closer {get;}
        public string DatabaseTypeString {get;}
        public bool Ordered {get;}
        public bool Unique {get;}
        public List<String> Data {get;}
        private int High;
        private int Low;
        private Random rng;

        public RandomIntegerDataGenerator(int low, int high)
        {
            High = high;
            Low = low;
            rng = new Random();
            Opener = "";
            Closer = "";
            DatabaseTypeString = "NUMBER(255)";
            Ordered = true;
            Unique = true;
            Data = new List<String>();
        }

        public RandomIntegerDataGenerator(int low, int high, string databaseType)
        {
            High = high;
            Low = low;
            rng = new Random();
            Opener = "";
            Closer = "";
            DatabaseTypeString = databaseType;
            Ordered = true;
            Unique = true;
            Data = new List<String>();
        }

        public string next()
        {
            var result = rng.Next(Low, High).ToString();
            Data.Add(result);
            return result;
        }
    }
}