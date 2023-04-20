using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestHandwrittenRDP;
class Program
{
    static void Main(string[] args)
    {
        string toParse = @"
            class Point {
                def constructor(x, y) {
                    this.x = x;
                    this.y = y;
                }

                def calc() {
                    return this.x + this.y;
                }
            }

            class Point3D : Point {
                def constructor(x, y, z) {
                    base(x, y);
                    this.z = z;
                }

                def calc() {
                    return base() + this.z;
                }
            }
";
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

