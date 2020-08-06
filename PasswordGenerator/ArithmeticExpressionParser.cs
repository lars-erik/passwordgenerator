using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Superpower;
using Superpower.Parsers;

namespace PasswordGenerator
{
    public class ArithmeticExpressionParser
    {
        static double Evaluate(string format, DateTime date)
        {
            return Convert.ToInt32(date.ToString(format));
        }

        static TokenListParser<ArithmeticExpressionToken, ExpressionType> Operator(ArithmeticExpressionToken op, ExpressionType opType)
        {
            return Token.EqualTo(op).Value(opType);
        }

        private static MethodInfo EvaluateMethod = typeof(ArithmeticExpressionParser)
            .GetMethod("Evaluate", BindingFlags.Static | BindingFlags.NonPublic);

        static readonly TokenListParser<ArithmeticExpressionToken, ExpressionType> Add = Operator(ArithmeticExpressionToken.Plus, ExpressionType.AddChecked);
        static readonly TokenListParser<ArithmeticExpressionToken, ExpressionType> Subtract = Operator(ArithmeticExpressionToken.Minus, ExpressionType.SubtractChecked);
        static readonly TokenListParser<ArithmeticExpressionToken, ExpressionType> Multiply = Operator(ArithmeticExpressionToken.Times, ExpressionType.MultiplyChecked);
        static readonly TokenListParser<ArithmeticExpressionToken, ExpressionType> Divide = Operator(ArithmeticExpressionToken.Divide, ExpressionType.Divide);
        static readonly TokenListParser<ArithmeticExpressionToken, ExpressionType> Power = Operator(ArithmeticExpressionToken.Power, ExpressionType.Power);

        static readonly TokenListParser<ArithmeticExpressionToken, Expression> Constant =
            Token.EqualTo(ArithmeticExpressionToken.Number)
                .Apply(Numerics.Decimal)
                .Select(n => (Expression)Expression.Convert(Expression.Constant(Convert.ToDouble(n.ToString(), CultureInfo.InvariantCulture)), typeof(double)));

        private static readonly TokenListParser<ArithmeticExpressionToken, Expression> Identifier =
            Token.EqualTo(ArithmeticExpressionToken.Identifier)
                .Apply(Superpower.Parsers.Identifier.CStyle)
                .Select(x =>
                {
                    var format = x.ToString();
                    try
                    {
                        var strVal = DateTime.Now.ToString(format);
                        var intVal = Convert.ToInt32(strVal);
                    }
                    catch
                    {
                        return null;
                    }

                    return (Expression) Expression.Call(
                        EvaluateMethod,
                        Expression.Constant(x.ToString()),
                        Parameter
                    );
                })
                .Where(x => x != null);

        static readonly TokenListParser<ArithmeticExpressionToken, Expression> Factor =
            (from lparen in Token.EqualTo(ArithmeticExpressionToken.LParen)
                from expr in Parse.Ref(() => Expr)
                from rparen in Token.EqualTo(ArithmeticExpressionToken.RParen)
                select expr)
            .Or(Constant.Or(Identifier.Named("identifier")));

        static readonly TokenListParser<ArithmeticExpressionToken, Expression> Operand =
            (from sign in Token.EqualTo(ArithmeticExpressionToken.Minus)
                from factor in Factor
                select (Expression)Expression.Negate(factor))
            .Or(Factor).Named("expression");

        static readonly TokenListParser<ArithmeticExpressionToken, Expression> Term = Parse.Chain(Multiply.Or(Divide).Or(Power), Operand, Expression.MakeBinary);

        static readonly TokenListParser<ArithmeticExpressionToken, Expression> Expr = Parse.Chain(Add.Or(Subtract), Term, Expression.MakeBinary);

        private static readonly ParameterExpression Parameter = Expression.Parameter(typeof(DateTime), "today");

        public static readonly TokenListParser<ArithmeticExpressionToken, Expression<Func<DateTime, double>>> Lambda =
            Expr
                .AtEnd()
                .Select(body => Expression.Lambda<Func<DateTime, double>>(body, Parameter));

    }
}