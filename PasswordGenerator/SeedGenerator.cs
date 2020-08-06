using System;
using Superpower;

namespace PasswordGenerator
{
    public class SeedGenerator
    {
        public static int GenerateSeed(string expression, DateTime date)
        {
            var result = ExecuteScript(expression, date);
            return (int)(Math.Floor(result) % Int32.MaxValue);
        }

        private static double ExecuteScript(string expression, DateTime date)
        {
            var tok = new ArithmeticExpressionTokenizer();
            var tokens = tok.Tokenize(expression);
            var expr = ArithmeticExpressionParser.Lambda.Parse(tokens);
            var compiled = expr.Compile();
            var result = compiled(date);
            return result;
        }
    }
}