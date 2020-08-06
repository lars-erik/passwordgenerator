using System;
using NUnit.Framework;
using PasswordGenerator.SampleSource;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class Generating_Passwords
    {
        [Test]
        [TestCaseSource("Passwords")]
        public void Creates_Adjective_Noun_Pairs_From_Seed(int seed, string expected)
        {
            var actual = new Generator(new StreamSource()).Generate(seed);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Adds_Suffix()
        {
            var actual = new Generator(new StreamSource()).Generate(1, "salt", "SFX");
            Assert.That(actual, Is.EqualTo("simple-feeling-SFX"));
        }

        [Test]
        public void From_Config()
        {
            var generatorConfig = new GeneratorConfig("yyyyMMdd * 2", "SFX");

            var generator = new Generator(new StreamSource(), generatorConfig);
            var result = generator.Generate(new DateTime(2020, 08, 05), "salt");
            Assert.That(result, Is.EqualTo("small-code-SFX"));
        }

        [Test]
        public void With_Suffix_Formula()
        {
            var generatorConfig = new GeneratorConfig("yyyyMMdd * 2", "SFX", "dd");

            var generator = new Generator(new StreamSource(), generatorConfig);
            var result = generator.Generate(new DateTime(2020, 08, 05), "salt");
            Assert.That(result, Is.EqualTo("small-code-SFX5"));
        }

        public static object[][] Passwords()
        {
            return new[]
            {
                new object[] {1, "big-code"},
                new object[] {2, "simple-paper"},
                new object[] {3, "big-tweet"},
                new object[] {4, "huge-sun"},
                new object[] {5, "awesome-heart"},
            };
        }
    }
}
