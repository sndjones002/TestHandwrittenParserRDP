﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestHandwrittenRDP;
class Program
{
    static void Main(string[] args)
    {
        string toParse = @"def empty(x, y){}";
        var myParser = new RecursiveDescentParserForOOP();
        var parsedResult = myParser.Parse(toParse);

        Console.WriteLine(JsonSerializer.Serialize(parsedResult, new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true
        }));
    }
}

