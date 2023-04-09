using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestHandwrittenRDP;
class Program
{
    static void Main(string[] args)
    {
        string toParse = @"{456;}";
        var myParser = new RecursiveDescentParserForOOP();
        var parsedResult = myParser.Parse(toParse);
        var parsedResultSE = SExpressionSerializer.Serialize(parsedResult);

        Console.WriteLine(JsonSerializer.Serialize(parsedResult, new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true
        }));

        Console.WriteLine("-------------------------------------------------");

        /*Console.WriteLine(JsonSerializer.Serialize(parsedResultSE, new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true
        }));*/
    }
}

