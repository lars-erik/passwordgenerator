using System;
using System.Collections.Generic;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace PasswordGenerator
{
    public class ArithmeticExpressionTokenizer : Tokenizer<ArithmeticExpressionToken>
    {
        readonly Dictionary<char, ArithmeticExpressionToken> _operators = new Dictionary<char, ArithmeticExpressionToken>
        {
            ['+'] = ArithmeticExpressionToken.Plus,
            ['-'] = ArithmeticExpressionToken.Minus,
            ['*'] = ArithmeticExpressionToken.Times,
            ['/'] = ArithmeticExpressionToken.Divide,
            ['^'] = ArithmeticExpressionToken.Power,
            ['('] = ArithmeticExpressionToken.LParen,
            [')'] = ArithmeticExpressionToken.RParen,
        };

        protected override IEnumerable<Result<ArithmeticExpressionToken>> Tokenize(TextSpan span)
        {
            var next = SkipWhiteSpace(span);
            if (!next.HasValue)
                yield break;

            do
            {
                ArithmeticExpressionToken charToken;

                var ch = next.Value;
                if (ch >= '0' && ch <= '9')
                {
                    var number = Numerics.Decimal(next.Location);
                    next = number.Remainder.ConsumeChar();
                    yield return Result.Value(ArithmeticExpressionToken.Number, number.Location, number.Remainder);
                }
                else if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z')
                {
                    var identifier = Identifier.CStyle(next.Location);
                    next = identifier.Remainder.ConsumeChar();
                    yield return Result.Value(ArithmeticExpressionToken.Identifier, identifier.Location, identifier.Remainder);
                }
                else if (_operators.TryGetValue(ch, out charToken))
                {
                    yield return Result.Value(charToken, next.Location, next.Remainder);
                    next = next.Remainder.ConsumeChar();
                }
                else
                {
                    yield return Result.Empty<ArithmeticExpressionToken>(next.Location, new[] { "number", "operator", "identifier" });
                }

                next = SkipWhiteSpace(next.Location);
            } while (next.HasValue);
        }
    }
}