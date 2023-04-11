using System;
using System.Reflection;
using Xunit.Sdk;

namespace TestHandwrittenRDPxUTests
{
	public class TextFileDataAttribute : DataAttribute
	{
        private readonly string _filePath;

        public TextFileDataAttribute(string filePath)
		{
            _filePath = filePath;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) { throw new ArgumentNullException(nameof(testMethod)); }

            // Get the absolute path to the JSON file
            var path = Path.IsPathRooted(_filePath)
                ? _filePath
                : Path.Combine(Directory.GetCurrentDirectory(), "TestData", _filePath);

            if (!File.Exists(path))
            {
                throw new ArgumentException($"Could not find file at path: {path}");
            }

            // Load the file
            yield return new object[] { File.ReadAllText(path) };
        }
    }
}

