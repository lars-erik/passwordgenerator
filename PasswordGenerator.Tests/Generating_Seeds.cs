using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Superpower;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class Generating_Seeds
    {
        static readonly DateTime TestDate = new DateTime(2020, 08, 05);

        [Test]
        [TestCase("1.5 + 2 * 3", 7)]
        [TestCase("(1 + 2) * 3", 9)]
        [TestCase("(1 + 2) ^ 3", 27)]
        [TestCase("yyyyMMdd * 2 + 5", 40401615)]
        [TestCase("yyyyMMdd ^ 1.5 + ddMM ^ 2", 599080400)]
        [TestCase("yyyyMMdd ^ 1.5 + ddMM ^ 3", 729918848)]
        public void Passes_Clock_Through_Custom_Arithmetics(string expression, int expected)
        {
            var result = SeedGenerator.GenerateSeed(expression, TestDate);
            Assert.That(result, Is.EqualTo(expected));
            Console.WriteLine(result);
        }

        [Test]
        public void Writes_Nice_Exceptions()
        {
            Assert.That<int>(
                () => SeedGenerator.GenerateSeed("(1 + 2 * 3", TestDate), 
                Throws.TypeOf<ParseException>()
                    .With
                    .Property("Message")
                    .EqualTo("Syntax error: unexpected end of input, expected `)`.")
                );
        }

        [Test]
        public void Tokenizes_Identifiers()
        {
            var tok = new ArithmeticExpressionTokenizer();
            var tokens = tok.Tokenize("10000000 + yyyyMMdd");
            Console.WriteLine(JsonConvert.SerializeObject(
                tokens, 
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    Converters =
                    {
                        new StringEnumConverter(),
                        new TextSpanConverter()
                    }
                }
            ));
        }

        [Test]
        public void Parses_Date_Identifier()
        {
            var tok = new ArithmeticExpressionTokenizer();
            var tokens = tok.Tokenize("10000000 + yyyyMMdd");
            var expr = ArithmeticExpressionParser.Lambda.Parse(tokens);
            var compiled = expr.Compile();
            var result = compiled(TestDate);
            Console.WriteLine(result);
        }

        [Test]
        public void Explains_Error_For_Invalid_Identifiers()
        {
            var tok = new ArithmeticExpressionTokenizer();
            var tokens = tok.Tokenize("10000000 + xyz");
            Assert.That(() => ArithmeticExpressionParser.Lambda.Parse(tokens), Throws.TypeOf<ParseException>()
                .With
                .Property("Message")
                .EqualTo("Syntax error (line 1, column 12): unexpected identifier `xyz`, expected expression.")
            );
        }
    }
}
