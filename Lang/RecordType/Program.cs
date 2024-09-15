namespace RecordType;

record SomeRecord(string Value1, int Value2);

record struct SomeRecordStruct(string Value1, int Value2);

class Program
{
    static void Main(string[] args)
    {
        var record1 = new SomeRecord(Value1: "Hello", Value2: 100);
        Console.WriteLine(record1);
        Console.WriteLine(record1.Value1);
        Console.WriteLine(record1.Value2);

        var record2 = new SomeRecord(Value1: "Hello", Value2: 100);
        Console.WriteLine(record2 == record1);

        var struct1 = new SomeRecordStruct(Value1: "Hello", Value2 : 100);
        Console.WriteLine(struct1);
        Console.WriteLine(struct1.Value1);

        //NOTE: Record class is immutable, while record struct is not.
        //record1.Value1 = "ABC";
        struct1.Value1 = "ABC";
        Console.WriteLine(struct1.Value1);
    }
}
