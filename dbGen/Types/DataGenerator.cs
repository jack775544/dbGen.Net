namespace dbGen
{
    public interface DataGenerator
    {
        string Opener {get; set;}
        string Closer {get; set;}
        string DatabaseTypeString {get; set;}
    }
}