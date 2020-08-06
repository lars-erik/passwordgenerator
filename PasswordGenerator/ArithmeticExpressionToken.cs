using Superpower.Display;

namespace PasswordGenerator
{
    public enum ArithmeticExpressionToken
    {
        None,

        Number,
        Identifier,

        [Token(Category = "operator", Example = "+")]
        Plus,

        [Token(Category = "operator", Example = "-")]
        Minus,

        [Token(Category = "operator", Example = "*")]
        Times,

        [Token(Category = "operator", Example = "-")]
        Divide,

        [Token(Category = "operator", Example = "^")]
        Power,

        [Token(Example = "(")]
        LParen,

        [Token(Example = ")")]
        RParen
    }
}