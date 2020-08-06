using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PasswordGenerator
{
    public class Generator
    {
        private readonly GeneratorConfig config;
        private string[][] lists;

        public Generator(IStreamSource streamSource)
        {
            using (var streams = streamSource.GetStreams())
            { 
                lists = streams.Select(stream =>
                {
                    var streamList = new List<string>();
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            streamList.Add(reader.ReadLine());
                        }
                    }
                    return streamList.ToArray();
                }).ToArray();
            }
        }

        public Generator(IStreamSource streamSource, GeneratorConfig config)
            : this(streamSource)
        {
            this.config = config;
        }

        public string Generate(DateTime date, string salt = null)
        {
            var seed = SeedGenerator.GenerateSeed(config.Seed.Formula ?? "yyyyMMdd", date);
            var suffix = config?.Password.Suffix ?? "";
            if (config?.Password.SuffixFormula != null)
            {
                var suffixResult = SeedGenerator.GenerateSeed(config.Password.SuffixFormula, date);
                suffix += suffixResult;
            }
            return Generate(seed, salt, suffix);
        }

        public string Generate(int seed, string salt = null, string suffix = null)
        {
            var saltVal = CalculateSalt(salt);
            var rnd = CreateRnd(seed, saltVal);

            var words = new string[lists.Length];
            for (var listIndex = 0; listIndex < lists.Length; listIndex++)
            {
                var list = lists[listIndex];
                words[listIndex] = list[rnd.Next(list.Length)];
            }

            suffix = suffix ?? config?.Password.Suffix;
            if (suffix != null)
            {
                words = words.Concat(new[] {suffix}).ToArray();
            }

            return String.Join("-", words);
        }

        private static Random CreateRnd(int seed, int saltVal)
        {
            var rnd = new Random(seed);
            for (var i = 0; i < saltVal; i++)
            {
                rnd.Next();
            }

            return rnd;
        }

        public static int CalculateSalt(string salt)
        {
            var saltVal = 0;

            if (salt != null)
            {
                saltVal = salt.ToCharArray().Sum(x => (byte) x);
            }

            return saltVal;
        }
    }
}
